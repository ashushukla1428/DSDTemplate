using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Providers.ProvidersContactTypes
{
    public class ProvidersContactTypesService:BaseServiceClass, IProvidersContactTypesService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "contact type";
       
        #region Main
        public ProvidersContactTypesService()
        {
        }
        public async Task<ResponseModel> AddAsync(ProvidersContactTypesViewModel request)
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
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.ProvidersContactTypes.ToString(), "ContactTypeName", request.ContactTypeName, request.Id, request.OrganizationId);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.ProvidersContactTypes + " (OrganizationId,ContactTypeName,IsActive,CreatedBy,CreatedDate) VALUES (@OrganizationId,@ContactTypeName, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@OrganizationId", request.OrganizationId);
                    parameters.Add("@ContactTypeName", request.ContactTypeName);
                  
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
                        TableName = AppTable.ProvidersContactTypes.ToString(),
                        PrimaryKeyId = request.Id,
                        OrganizationId = request.OrganizationId,
                        Data = JsonConvert.SerializeObject(new { newRec = request })
                    });
                    //end ActionsHistory
                    return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
                }
            }
            catch (Exception ex)
            {

                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(ProvidersContactTypesViewModel request)
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
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.ProvidersContactTypes.ToString(), "ContactTypeName", request.ContactTypeName, request.Id, request.OrganizationId);
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
                        var query = "UPDATE " + AppTable.ProvidersContactTypes + " SET  OrganizationId=@OrganizationId,ContactTypeName=@ContactTypeName,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";
                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@OrganizationId", request.OrganizationId);
                        parameters.Add("@ContactTypeName", request.ContactTypeName);

                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                         await con.ExecuteScalarAsync<int>(query, parameters);
                        con.Close();
                    }
                       
                  
                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.ProvidersContactTypes.ToString(),
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
        public async Task<IEnumerable<ProvidersContactTypesViewModel>> GetAllAsync(long organizationId)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = "select * from " + AppTable.ProvidersContactTypes + " (nolock)  ";
                    var parameters = new DynamicParameters();
                    if (!UserSession.Current.IsSuperAdmin)
                    {
                        query = query + " where OrganizationId=@OrganizationId";
                        parameters.Add("@OrganizationId", organizationId);
                    }
                    query = query + " order by id desc";
                    IEnumerable<ProvidersContactTypesViewModel> response = await con.QueryAsync<ProvidersContactTypesViewModel>(query, parameters, commandType: CommandType.Text);
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

        public async Task<List<SelectListItem>> GetDropProvidersContactTypesAsync(long organizationId)
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select id, ContactTypeName from " + AppTable.ProvidersContactTypes + " (nolock) where    IsActive=1 and OrganizationId=@OrganizationId  order by ContactTypeName asc";
                    var parameters = new DynamicParameters();
                    var _data = await con.QueryAsync<ProvidersContactTypesViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(_data.OrderBy(c => c.ContactTypeName).Select(g => new SelectListItem { Text = g.ContactTypeName, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }

               
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.High, this.GetType().Name + "->GetDropProvidersContactTypesAsync", ex);
                throw;
            }
        }
        #endregion
        private async Task<ProvidersContactTypesViewModel> GetByIdAsync(long Id)
        {
            ProvidersContactTypesViewModel response = new ProvidersContactTypesViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.ProvidersContactTypes + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<ProvidersContactTypesViewModel>(query, parameters, commandType: CommandType.Text);
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
