using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Organization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Organization.OrganizationYears
{
    public class OrganizationYearsService :BaseServiceClass, IOrganizationYearsService
    {
       
        IActionsHistoryService _actionsHistoryService;
        string _serviceFor = "organization year";
        public OrganizationYearsService()
        {
            _actionsHistoryService = new ActionsHistoryService();
        }
        public async Task<ResponseModel> AddAsync(OrganizationYearsViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                if (request.OrganizationId == 0)
                {
                    return new ResponseModel { Message = "Please select an organization ", Status = false, Id = request.Id };
                }
                if (request.BeginDate.Date> request.EndDate.Date)
                {
                    return new ResponseModel { Message = "Begin date cannot be greater than end date", Status = false, Id = 0 };
                }
                var _existRecordResponse = await CheckIfOrgUsersIsExist(request.BeginDate, request.EndDate, request.OrganizationId, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.OrganizationYears 
                        + " (OrganizationId,BeginDate,EndDate,LongDescription,ShortDescription,IsActive,CreatedBy,CreatedDate) " +
                        "VALUES (@OrganizationId,@BeginDate,@EndDate,@LongDescription,@ShortDescription, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@OrganizationId", request.OrganizationId);
                    parameters.Add("@BeginDate", request.BeginDate);
                    parameters.Add("@EndDate", request.EndDate);
                    parameters.Add("@LongDescription", request.LongDescription);
                    parameters.Add("@ShortDescription", request.ShortDescription);

                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@CreatedDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();

                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Insert.ToString(),
                        TableName = AppTable.OrganizationYears.ToString(),
                        OrganizationId = request.OrganizationId,
                        PrimaryKeyId = request.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                    return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name+"->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(OrganizationYearsViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                if (request.OrganizationId == 0)
                {
                    return new ResponseModel { Message = "Please select an organization ", Status = false, Id = request.Id };
                }
                if (request.BeginDate.Date > request.EndDate.Date)
                {
                    return new ResponseModel { Message = "Begin date cannot be greater than end date", Status = false, Id = 0 };
                }
  
                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    if (item.BeginDate.Date == request.BeginDate.Date && item.EndDate.Date == request.EndDate.Date && item.Id != request.Id && item.EndDate.Date == request.EndDate.Date)
                    {
                        return new ResponseModel { Message = ResponseMessages.SubjectAlreadyExists(_serviceFor), Status = false, Id = 0 };
                    }

                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.OrganizationYears + " SET  OrganizationId=@OrganizationId,BeginDate=@BeginDate," +
                            "EndDate=@EndDate,LongDescription=@LongDescription,ShortDescription=@ShortDescription,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@OrganizationId", request.OrganizationId);
                        parameters.Add("@BeginDate", request.BeginDate);
                        parameters.Add("@EndDate", request.EndDate);
                        parameters.Add("@LongDescription", request.LongDescription);
                        parameters.Add("@ShortDescription", request.ShortDescription);
                        
                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }
                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.OrganizationYears.ToString(),
                        OrganizationId = (long)item.OrganizationId,
                        PrimaryKeyId = item.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request, oldRec = JsonConvert.DeserializeObject(oldRecord) })
                    });
                    oldRecord = null;
                    //end ActionsHistory
                    return new ResponseModel { Message = ResponseMessages.SubjectUpdatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
                else
                {
                    return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false, Id = request.Id };
                }
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->UpdateAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<IEnumerable<OrganizationYearsViewModel>> GetAllAsync(long organizationId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = "select * from " + AppTable.OrganizationYears + " (nolock)  ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query + " where OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", organizationId);
                    }
                    query = query + " order by id desc";
                    IEnumerable<OrganizationYearsViewModel> response = await con.QueryAsync<OrganizationYearsViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

              
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetAllAsync", ex);
                throw;
            }
        }

        public async Task<List<SelectListItem>> GetDropOrganizationYearsAsync(long organizationId)
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.UsersRoles + " (nolock) where IsActive=1  and OrganizationId=@OrganizationId  order by BeginDate asc";
                    var parameters = new DynamicParameters();
                   
                    parameters.Add("@OrganizationId", organizationId);
                    query = query + " order by RoleName asc ";

                    var _data = await con.QueryAsync<OrganizationYearsViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.OrderBy(c => c.BeginDate).Select(g => new SelectListItem { Text = g.BeginDate.Date.ToString() + "-" + g.EndDate.Date.ToString(), Value = g.Id.ToString() }).ToList());
                    return _listData;
                }
               
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetDropOrganizationYearsAsync", ex);
                throw;
            }
        }

        private async Task<OrganizationYearsViewModel> GetByIdAsync(long Id)
        {
            OrganizationYearsViewModel response = new OrganizationYearsViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.OrganizationYears + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<OrganizationYearsViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        public async Task<ResponseModel> CheckIfOrgUsersIsExist(DateTime BeginDate, DateTime EndDate, long OrganizationId,long Id)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                query = "select Id from " + AppTable.OrganizationYears + " where CAST(BeginDate AS DATE) >=CAST(@BeginDate AS DATE) and CAST(EndDate AS DATE) <=CAST(@EndDate AS DATE) and OrganizationId=@OrganizationId";
                parameters.Add("@OrganizationId", OrganizationId);
                parameters.Add("@BeginDate", BeginDate);
                parameters.Add("@EndDate", EndDate);
                if (Id != 0)
                {
                    query = query + " and Id!=@Id";
                    parameters.Add("@Id", Id);
                }
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var _result = await con.QueryFirstOrDefaultAsync<int>(query, parameters);
                    query = null;
                    if (_result != 0)
                    {
                        return new ResponseModel { Status = false, Message = ResponseMessages.SubjectAlreadyExists("Organization Year") };
                    }
                    else
                    {
                        return new ResponseModel { Status = true, Message = null };
                    }
                }

            }
            catch (Exception ex)
            {
                query = null;
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
