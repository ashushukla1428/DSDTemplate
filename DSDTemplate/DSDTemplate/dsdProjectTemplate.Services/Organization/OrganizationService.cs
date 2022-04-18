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

namespace dsdProjectTemplate.Services.Organization
{
    public class OrganizationService :BaseServiceClass, IOrganizationService
    {
        
        
        IActionsHistoryService _actionsHistoryService;
        string _serviceFor = " organization";
        
        public OrganizationService()
        { 
            _actionsHistoryService = new ActionsHistoryService();
        }
        public async Task<ResponseModel> AddAsync(OrganizationViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                 //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.Organizations.ToString(), "Name", request.Name, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.Organizations 
                    + " (ParentOrganizationId,Name,OrgCode,WorkNumber,FaxNumber,ContactEmail,IsAdminOnly,Address,ZipCode,StateId,CityId,IsActive,CreatedBy,CreatedDate) ";
                    query = query + "VALUES (@ParentOrganizationId,@Name,@OrgCode,@WorkNumber,@FaxNumber,@ContactEmail,@IsAdminOnly,@Address,@ZipCode,@StateId,@CityId, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                  
                    var parameters = new DynamicParameters();
                    parameters.Add("@ParentOrganizationId", request.ParentOrganizationId);
                    parameters.Add("@Name", request.Name);
                    parameters.Add("@OrgCode", request.OrgCode);
                    parameters.Add("@WorkNumber", request.WorkNumber);
                    parameters.Add("@FaxNumber", request.FaxNumber);
                    parameters.Add("@ContactEmail", request.ContactEmail);
                    parameters.Add("@IsAdminOnly", request.IsAdminOnly);
                    parameters.Add("@Address", request.Address);
                    parameters.Add("@ZipCode", request.ZipCode);
                    parameters.Add("@StateId", request.StateId);
                    parameters.Add("@CityId", request.CityId); 

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
                        TableName = AppTable.Organizations.ToString(),
                        OrganizationId = request.Id,
                        PrimaryKeyId = request.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                }

               
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->AddAsync", ex);
                throw;
            }
        }
        public async Task<ResponseModel> UpdateAsync(OrganizationViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                if (request.ParentOrganizationId == request.Id)
                {
                    return new ResponseModel { Message = "You cannot select same organization as a parent organization", Status = false, Id = 0 };
                }
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.Organizations.ToString(), "Name", request.Name, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }

                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.Organizations 
                            + " SET  ParentOrganizationId=@ParentOrganizationId,Name=@Name,OrgCode=@OrgCode" +
                            ",WorkNumber=@WorkNumber,FaxNumber=@FaxNumber,ContactEmail=@ContactEmail,IsAdminOnly=@IsAdminOnly,Address=@Address," +
                            "ZipCode=@ZipCode,StateId=@StateId,CityId=@CityId,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@ParentOrganizationId", request.ParentOrganizationId);
                        parameters.Add("@Name", request.Name);
                        parameters.Add("@OrgCode", request.OrgCode);
                        parameters.Add("@WorkNumber", request.WorkNumber);
                        parameters.Add("@FaxNumber", request.FaxNumber);
                        parameters.Add("@ContactEmail", request.ContactEmail);
                        parameters.Add("@IsAdminOnly", request.IsAdminOnly);
                        parameters.Add("@Address", request.Address);
                        parameters.Add("@ZipCode", request.ZipCode);
                        parameters.Add("@StateId", request.StateId);
                        parameters.Add("@CityId", request.CityId);

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
                        TableName = AppTable.Organizations.ToString(),
                        OrganizationId = item.Id,
                        PrimaryKeyId = item.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request, oldRec = JsonConvert.DeserializeObject(oldRecord) })
                    });
                    oldRecord = null;
                    //end ActionsHistory
                    return new ResponseModel { Message = ResponseMessages.SubjectUpdatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
                else
                {
                    return new ResponseModel { Message = ResponseMessages.Failure_To_Update, Status = false, Id = 0 };
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->UpdateAsync", ex);
                throw;
            }
        }
        public async Task<IEnumerable<OrganizationViewModel>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select * from " + AppTable.Organizations + " (nolock)  ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        if (UserSession.Current.IsSoftware_User)
                        {
                            query = query + " where IsAdminOnly=0";
                        }
                        else
                        {
                            query = query + " where IsAdminOnly=false and IsActive=1 and Id=@Id";
                            parameters.Add("@Id", UserSession.Current.SelectedOrgId);
                        }
                        
                    }
                    query = query + " order by id desc";
                    IEnumerable<OrganizationViewModel> response = await con.QueryAsync<OrganizationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->AddAsync", ex);
                throw;
            }
        }

        public async Task<List<SelectListItem>> GetDropOrganizationsAsync()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,Name from " + AppTable.Organizations + " (nolock) where IsActive=1 ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        if (UserSession.Current.IsSoftware_User)
                        {
                            query = query + " and IsAdminOnly=0";
                        }
                        else
                        {
                            query = query + " and IsAdminOnly=0  and Id=@Id";
                            parameters.Add("@Id", UserSession.Current.SelectedOrgId);
                        }
                    }
                     
                    query = query + " order by Name asc ";

                    var _data = await con.QueryAsync<OrganizationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.Add(new SelectListItem { Text = "Select Organization", Value = "0" });
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }

              
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetDropOrganizationsAsync", ex);
                throw;
            }
        }
        public async Task<List<SelectListItem>> GetDropOrganizationsForActionHistoryAsync()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,Name from " + AppTable.Organizations + " (nolock) where IsActive=1 ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        if (!UserSession.Current.IsSoftware_User)
                        {
                            //query = query + " and IsAdminOnly=0";
                            query = query + " and IsAdminOnly=0  and Id=@Id";
                            parameters.Add("@Id", UserSession.Current.SelectedOrgId);
                        }
                        //else
                        //{
                        //    query = query + " and IsAdminOnly=0  and Id=@Id";
                        //    parameters.Add("@Id", UserSession.Current.SelectedOrgId);
                        //}
                    }
                    else
                    {
                        _listData.Add(new SelectListItem { Text = "Master Tables", Value = "0" });
                    }

                    query = query + " order by Name asc ";

                    var _data = await con.QueryAsync<OrganizationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }

               
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetDropOrganizationsAsync", ex);
                throw;
            }
        }
        private async Task<OrganizationViewModel> GetByIdAsync(long Id)
        {
            OrganizationViewModel response = new OrganizationViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.Organizations + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<OrganizationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
    }
}
