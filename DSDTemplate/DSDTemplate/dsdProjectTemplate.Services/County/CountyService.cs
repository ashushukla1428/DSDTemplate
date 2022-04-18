using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Counties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.County
{
    public class CountyService : BaseServiceClass, ICountyService
    {
        IActionsHistoryService _actionsHistoryService;
        string _serviceFor = "county";
       
        public CountyService()
        {
            _actionsHistoryService = new ActionsHistoryService();
        }
        public async Task<ResponseModel> AddAsync(CountiesResponse request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                //Check, if the record already exists in the Database
                if (await CheckIfRecordIsExist(request) != null)
                {
                    return new ResponseModel { Message = ResponseMessages.SubjectAlreadyExists(request.Name + " " + _serviceFor), Status = false, Id = 0 };
                }
                // Update record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.Counties + " (CountyName,StateId,IsActive,CreatedBy,CreatedDate) VALUES (@CountyName,@StateId, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@CountyName", request.Name);
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
                        TableName = AppTable.Counties.ToString(),
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
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(CountiesResponse request)
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
                        var query = "UPDATE " + AppTable.Counties + " SET  CountyName=@CountyName,StateId=@StateId,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@CountyName", request.Name);
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
                        TableName = AppTable.Counties.ToString(),
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
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<IEnumerable<CountiesResponse>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    var query = "select [Id],[StateId],[CountyName] as Name,[IsActive],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] from " + AppTable.Counties + " (nolock) order by id desc ";
                    var parameters = new DynamicParameters();
                    IEnumerable<CountiesResponse> response = await con.QueryAsync<CountiesResponse>(query, parameters, commandType: CommandType.Text);
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

        public async Task<List<SelectListItem>> GetDropCountesAsync(int sateId)
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,CountyName as Name from " + AppTable.Counties + " (nolock)  where IsActive=1 and StateId=@StateId order by CityName asc ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@StateId", sateId);
                    var _data = await con.QueryAsync<CountiesResponse>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetDropCountesAsync", ex);
                throw;
            }
        }
        private async Task<CountiesResponse> GetByIdAsync(long Id)
        {
            CountiesResponse response = new CountiesResponse();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select [Id],[StateId],[CountyName] AS Name,[IsActive],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] from " + AppTable.Counties + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<CountiesResponse>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        private async Task<CountiesResponse> CheckIfRecordIsExist(CountiesResponse request)
        {
            CountiesResponse response = new CountiesResponse();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select [Id] from " + AppTable.Counties + " (nolock) where CountyName=@CountyName and StateId=@StateId ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@CountyName", request.Name);
                    parameters.Add("@StateId", request.StateId);
                    parameters.Add("@Id", request.Id);
                    if (request.Id != 0)
                    {
                        query = query + " and Id!=@Id ";
                        parameters.Add("@Id", request.Id);
                    }

                    response = await con.QueryFirstOrDefaultAsync<CountiesResponse>(query, parameters, commandType: CommandType.Text);
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
