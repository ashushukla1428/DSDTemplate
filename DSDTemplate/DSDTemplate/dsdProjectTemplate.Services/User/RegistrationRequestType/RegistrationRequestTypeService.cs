using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.User.Registration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.User.RegistrationRequestType
{
    public class RegistrationRequestTypeService: BaseServiceClass, IRegistrationRequestTypeService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "Registration request type";

        #region Main
        public RegistrationRequestTypeService()
        {
        }
        public async Task<ResponseModel> AddAsync(RegistrationRequestTypeViewModel request)
        {
            try
            {
                if (request.OrganizationId == 0)
                {
                    return new ResponseModel { Message = "Please select an organization ", Status = false, Id = request.Id };
                }
               
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.RegistrationRequestTypes.ToString(), "TypeName", request.TypeName, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.RegistrationRequestTypes + " (OrganizationId,TypeName,IsActive,CreatedBy,CreatedDate) VALUES (@OrganizationId,@TypeName, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@TypeName", request.TypeName);
                    parameters.Add("@OrganizationId", request.OrganizationId); 
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
                        TableName = AppTable.RegistrationRequestTypes.ToString(),
                        PrimaryKeyId = request.Id,
                        OrganizationId = request.OrganizationId,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                }
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(RegistrationRequestTypeViewModel request)
        {
            try
            {
                if (request.OrganizationId == 0)
                {
                    return new ResponseModel { Message = "Please select an organization ", Status = false, Id = request.Id };
                }
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.RegistrationRequestTypes.ToString(), "TypeName", request.TypeName, request.Id);
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
                        var query = "UPDATE " + AppTable.RegistrationRequestTypes + " SET  OrganizationId=@OrganizationId,TypeName=@TypeName,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@OrganizationId", request.OrganizationId);
                        parameters.Add("@TypeName", request.TypeName);

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
                        TableName = AppTable.RegistrationRequestTypes.ToString(),
                        OrganizationId=item.OrganizationId,
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
        public async Task<IEnumerable<RegistrationRequestTypeViewModel>> GetAllAsync(long organizationId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = "select * from " + AppTable.RegistrationRequestTypes + " (nolock)  ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query + " where OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", organizationId);
                    }
                    query = query + " order by id desc";
                    IEnumerable<RegistrationRequestTypeViewModel> response = await con.QueryAsync<RegistrationRequestTypeViewModel>(query, parameters, commandType: CommandType.Text);
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

        public async Task<List<SelectListItem>> GetDropListAsync(long organizationId)
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,TypeName   from " + AppTable.RegistrationRequestTypes + " (nolock) ";
                    var parameters = new DynamicParameters();
                    if (organizationId!=0)
                    {
                        query = query + " where OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", organizationId);
                    }
                    query = query + " order by TypeName asc ";

                    var _data = await con.QueryAsync<RegistrationRequestTypeViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.TypeName, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }
               
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetDropListAsync", ex);
                throw;
            }
        }
        private async Task<RegistrationRequestTypeViewModel> GetByIdAsync(long Id)
        {
            RegistrationRequestTypeViewModel response = new RegistrationRequestTypeViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.RegistrationRequestTypes + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<RegistrationRequestTypeViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        #endregion
    }
}
