using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.City;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.City
{
    public class CityService : BaseServiceClass, ICityService
    {   
        IActionsHistoryService _actionsHistoryService;
        string _serviceFor = "city";
        public CityService()
        {
            _actionsHistoryService = new ActionsHistoryService();
        }
        public async Task<ResponseModel> AddAsync(CityViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }

            try
            {
                if (await CheckIfRecordIsExist(request) != null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectAlreadyExists(request.Name + " " + _serviceFor), Status = false, Id = 0 };
                }

                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.Cities + " (CityName,StateId,IsActive,CreatedBy,CreatedDate) VALUES (@CityName,@StateId, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@CityName", request.Name);
                    parameters.Add("@StateId", request.StateId);

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
                        TableName = AppTable.Cities.ToString(),
                        PrimaryKeyId = request.Id,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                }
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
            }
            catch (Exception ex)
            {

                await  ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(CityViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                if (await CheckIfRecordIsExist(request) != null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectAlreadyExists(request.Name + " " + _serviceFor), Status = false, Id = 0 };
                }
                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    // Update record in the database
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.Cities + " SET  CityName=@CityName,StateId=@StateId,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@CityName", request.Name);
                        parameters.Add("@StateId", request.StateId);

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
                        TableName = AppTable.Cities.ToString(),
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
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->UpdateAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<IEnumerable<CityViewModel>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select [Id],[StateId],[CityName] as Name,[IsActive],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] from " + AppTable.Cities + " (nolock) order by id desc ";
                    var parameters = new DynamicParameters();
                    IEnumerable<CityViewModel> response = await con.QueryAsync<CityViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetAllAsync", ex);
                throw;
            }
            
        }
        public async Task<List<SelectListItem>> GetDropCitesAsync()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,CityName as Name from " + AppTable.Cities + " (nolock) where IsActive=1 order by CityName asc ";
                    var parameters = new DynamicParameters();
                    var _data = await con.QueryAsync<CityViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetDropCitesAsync", ex);
                throw;
            }
        }
        private async Task<CityViewModel> GetByIdAsync(long Id)
        {
            CityViewModel response = new CityViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select [Id],[StateId],[CityName],[IsActive],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] from " + AppTable.Cities + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<CityViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        private async Task<CityViewModel> CheckIfRecordIsExist(CityViewModel request)
        {
            CityViewModel response = new CityViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select [Id] from " + AppTable.Cities + " (nolock) where CityName=@CityName and StateId=@StateId ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@CityName", request.Name);
                    parameters.Add("@StateId", request.StateId);
                    parameters.Add("@Id", request.Id);
                    if (request.Id != 0)
                    {
                        query= query+   " and Id!=@Id ";
                        parameters.Add("@Id", request.Id);
                    }
                   
                    response = await con.QueryFirstOrDefaultAsync<CityViewModel>(query, parameters, commandType: CommandType.Text);
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
