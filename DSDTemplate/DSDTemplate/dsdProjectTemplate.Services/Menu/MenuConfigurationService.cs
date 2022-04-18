using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu
{
    public class MenuConfigurationService:BaseServiceClass, IMenuConfigurationService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "Menu configuration";

        public MenuConfigurationService()
        {
        }
        public async Task<ResponseModel> AddAsync(MenuConfigurationViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }

            try
            {

                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.MenuConfiguration.ToString(), "Name", request.Name, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.MenuConfiguration + " (Name,DisplayOrder,MenuAction,MenuController,MenuIcon,IsAdminOnly,MenuKey,IsActive,CreatedBy,CreatedDate,AreaId) " +
                        "VALUES (@Name,@DisplayOrder,@MenuAction,@MenuController,@MenuIcon,@IsAdminOnly,@MenuKey, @IsActive,@CreatedBy,@CreatedDate,@AreaId)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", request.Name);
                    parameters.Add("@DisplayOrder", request.DisplayOrder);
                    parameters.Add("@MenuAction", request.MenuAction);
                    parameters.Add("@MenuController", request.MenuController);
                    parameters.Add("@MenuIcon", request.MenuIcon);
                    parameters.Add("@IsAdminOnly", request.IsAdminOnly);
                    parameters.Add("@MenuKey", request.MenuController + "_" + request.MenuAction);

                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@CreatedDate", DateTime.UtcNow);
                    parameters.Add("@AreaId", request.AreaId);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();

                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Insert.ToString(),
                        TableName = AppTable.MenuConfiguration.ToString(),
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
        public async Task<ResponseModel> UpdateAsync(MenuConfigurationViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                //Check, if the record already exists in the Database
                var _existRecordResponse = await CheckIfRecordIsExist(AppTable.MenuConfiguration.ToString(), "Name", request.Name, request.Id);
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
                        var query = "UPDATE " + AppTable.MenuConfiguration + " SET  Name=@Name,DisplayOrder=@DisplayOrder," +
                            "MenuAction=@MenuAction,MenuController=@MenuController,MenuIcon=@MenuIcon,IsAdminOnly=@IsAdminOnly,MenuKey=@MenuKey,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate,AreaId=@AreaId WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@Name", request.Name);
                        parameters.Add("@DisplayOrder", request.DisplayOrder);
                        parameters.Add("@MenuAction", request.MenuAction);
                        parameters.Add("@MenuController", request.MenuController);
                        parameters.Add("@MenuIcon", request.MenuIcon);
                        parameters.Add("@IsAdminOnly", request.IsAdminOnly);
                        parameters.Add("@MenuKey", request.MenuController + "_" + request.MenuAction);

                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        parameters.Add("@AreaId", request.AreaId);
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
                        TableName = AppTable.MenuConfiguration.ToString(),
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
        public async Task<IEnumerable<MenuConfigurationViewModel>> GetAllAsync()
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {

                    // var query = "select Id,GenderType,IsActive from " + AppTable.Gender + " (nolock) order by id desc ";
                    var query = "select * from " + AppTable.MenuConfiguration + " (nolock)  order by id desc";
                    var parameters = new DynamicParameters();
                  
                    IEnumerable<MenuConfigurationViewModel> response = await con.QueryAsync<MenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
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
        public async Task<List<SelectListItem>> GetDropListAsync()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,Name from " + AppTable.MenuConfiguration + " (nolock) where IsActive=1  and IsAdminOnly=0 order by Name asc";
                    //if (!UserSession.Current.IsSuperAdmin)
                    //{
                    //    query += " and IsAdminOnly=0 order by Name asc ";
                    //}
                    //else
                    //{
                    //    query += " order by Name asc ";
                    //}
                        
                    var parameters = new DynamicParameters();
                    var _data = await con.QueryAsync<MenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    var item = new SelectListItem();
                    item.Text = "--Select Menu Page--";
                    item.Value = "0";
                    item.Selected = true;
                    _listData.Add(item);
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList());
                    
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetDropListAsync", ex);
                throw;
            }
        }
        public async Task<MenuConfigurationViewModel> GetByIdAsync(long? Id)
        {
            MenuConfigurationViewModel response = new MenuConfigurationViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select * from " + AppTable.MenuConfiguration + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<MenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        public async Task<List<SelectListItem>> GetAllAppAreaMasterAsync()
        {
            try
            {
                var _listData = new List<SelectListItem>();
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id as AreaId,AreaName from " + AppTable.AppAreas + " (nolock) where IsActive=1 ";
                    var parameters = new DynamicParameters();
                    var _data = await con.QueryAsync<MenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.Add(new SelectListItem { Text = "Select Area", Value = "0" });
                    _listData.AddRange(_data.Select(g => new SelectListItem { Text = g.AreaName, Value = g.AreaId.ToString() }).ToList());
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetAllAppAreaMasterAsync", ex);
                throw;
            }
        }
    }
}
