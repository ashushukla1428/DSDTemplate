using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;

using dsdProjectTemplate.Services.User.UsersRole;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using dsdProjectTemplate.ViewModel.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration.SubMenuConfiguration
{
    public class SubMenuConfigurationService: BaseServiceClass, ISubMenuConfigurationService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        string _serviceFor = "Sub menu";

        public SubMenuConfigurationService()
        {
        }
        public async Task<ResponseModel> AddAsync(SubMenuConfigurationViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                var _existRecordResponse = await CheckIfMenuIsExist(request.Name,request.MainMenuId, request.MenuHeaderID, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                //List<UserSubMenu> _lstMenuHeaderAssignmen=    MenuHeaderAssignment(request.ListMenu);
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.UserSubMenu + " (DisplayName,MenuHeaderID,MenuID,DisplayOrder,IsActive,CreatedBy,CreatedDate) " +
                        "VALUES (@DisplayName,@MenuHeaderID,@MenuID,@DisplayOrder, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@DisplayName", request.Name.Trim());
                    parameters.Add("@MenuHeaderID", request.MenuHeaderID);
                    parameters.Add("@MenuID", request.MainMenuId);
                    parameters.Add("@DisplayOrder", request.DisplayOrder);

                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@CreatedDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();

                    
                }
               
                //start ActionsHistory
                var InsertedItem = await GetByIdAsync(request.Id);
                var _role = await new MenuHeaderConfigurationService().GetByIdAsync(request.MenuHeaderID);
                var _org = await new UsersRoleService().GetByIdAsync(_role.UserRoleId);
                await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                {
                    ActionType = UserActon.Insert.ToString(),
                    TableName = AppTable.UserSubMenu.ToString(),
                    PrimaryKeyId = request.Id,
                    OrganizationId= _org.OrganizationId,
                    Data = JsonConvert.SerializeObject(new { newRec = InsertedItem })
                });
                //end ActionsHistory
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = request.Id };
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }

        public async Task<ResponseModel> AddUpdateSubMenu(List<SelectListItem> listMenu, long menuHeaderId)
        {
            
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                //var _existRecordResponse = await CheckIfMenuIsExist(request.Name, request.MainMenuId, request.MenuHeaderID, request.Id);
                //if (!_existRecordResponse.Status)
                //{
                //    return _existRecordResponse;
                //}
                //List<UserSubMenu> _lstMenuHeaderAssignmen=    MenuHeaderAssignment(request.ListMenu);
                // Insert record in the database
                var query = "";
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    //get Master Values
                    var parameters = new DynamicParameters();
                    query = "DELETE FROM " + AppTable.UserSubMenu + " WHERE  MenuHeaderID = " + menuHeaderId;
                    //parameters.Add("@MenuHeaderID", request.MenuHeaderID);
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    await con.ExecuteAsync(query, parameters);

                    if(listMenu != null)
                    {
                        foreach (var menuItems in listMenu)
                        {
                            if(menuItems != null)
                            {
                                query = "SELECT * FROM " + AppTable.MenuConfiguration + " WHERE Id = " + menuItems.Value;
                                IEnumerable<MenuConfigurationViewModel> _menuConfigurationViewModels = await con.QueryAsync<MenuConfigurationViewModel>(query, commandType: CommandType.Text);
                                var _menuConfiguration = _menuConfigurationViewModels.ToList().FirstOrDefault();
                                query = "INSERT INTO " + AppTable.UserSubMenu + " (DisplayName,MenuHeaderID,MenuID,DisplayOrder,IsActive,CreatedBy,CreatedDate) " +
                                    "VALUES (@DisplayName,@MenuHeaderID,@MenuID,@DisplayOrder, @IsActive,@CreatedBy,@CreatedDate)";
                                query = query + " SELECT CAST(scope_identity() AS int);";

                                parameters.Add("@DisplayName", _menuConfiguration.Name);
                                parameters.Add("@MenuHeaderID", menuHeaderId);
                                parameters.Add("@MenuID", _menuConfiguration.Id);
                                parameters.Add("@DisplayOrder", _menuConfiguration.DisplayOrder);

                                parameters.Add("@IsActive", true);
                                parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                                parameters.Add("@CreatedDate", DateTime.UtcNow);
                                //request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                                var respId = await con.ExecuteScalarAsync<int>(query, parameters);
                                //start ActionsHistory
                                var InsertedItem = await GetByIdAsync(respId);
                                var _role = await new MenuHeaderConfigurationService().GetByIdAsync(menuHeaderId);
                                var _org = await new UsersRoleService().GetByIdAsync(_role.UserRoleId);
                                await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                                {
                                    ActionType = UserActon.Insert.ToString(),
                                    TableName = AppTable.UserSubMenu.ToString(),
                                    PrimaryKeyId = respId,
                                    OrganizationId = _org.OrganizationId,
                                    Data = JsonConvert.SerializeObject(new { newRec = InsertedItem })
                                });
                                //end ActionsHistory
                            }
                        }
                    }

                    
                    con.Close();
                }

                
                return new ResponseModel { Message = ResponseMessages.SubjectCreatedSuccess(_serviceFor), Status = true, Id = 0 };
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->AddAsync", ex);
                return new ResponseModel { Message = ResponseMessages.System_Error, Status = false, Id = 0 };
            }
        }
        public async Task<ResponseModel> UpdateAsync(SubMenuConfigurationViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                var _existRecordResponse = await CheckIfMenuIsExist(request.Name, request.MainMenuId, request.MenuHeaderID, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                
              //  var mainMenu = await _context.MenuConfigurations.Where(m => m.Id == request.MainMenuId).FirstOrDefaultAsync();
                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);

                if (item != null)
                {
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.UserSubMenu + " SET  DisplayName=@DisplayName,DisplayOrder=@DisplayOrder," +
                            "MenuHeaderID=@MenuHeaderID,MenuID=@MenuID,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@DisplayName", request.Name.Trim());
                        parameters.Add("@MenuHeaderID", request.MenuHeaderID);
                        parameters.Add("@MenuID", request.MainMenuId);
                        parameters.Add("@DisplayOrder", request.DisplayOrder);

                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }

                    var _role = await new MenuHeaderConfigurationService().GetByIdAsync(request.MenuHeaderID);
                    var _org = await new UsersRoleService().GetByIdAsync(_role.UserRoleId);
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.UserSubMenu.ToString(),
                        OrganizationId= _org.OrganizationId,
                        PrimaryKeyId = request.Id,
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
        public async Task<IEnumerable<SubMenuConfigurationViewModel>> GetAllAsync(HeaderConfigurationSearchRequest request)
        {
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    string query = @"select uSubMainMenu.id,uSubMainMenu.DisplayName as [Name],  uSubMainMenu.IsActive
                            ,uSubMainMenu.DisplayOrder,uSubMainMenu.MenuID,uSubMainMenu.MenuHeaderID
                            ,menuCon.MenuAction,menuCon.MenuController,menuCon.Id as MainMenuId
                         from UserSubMenu as uSubMainMenu
                        inner join
                        MenuConfiguration as menuCon on menuCon.Id = uSubMainMenu.MenuID ";
                    var parameters = new DynamicParameters();
                    if (request.HeaderMenuId != 0)
                    {
                        query = query + " and  uSubMainMenu.MenuHeaderID=@MenuHeaderID";
                        parameters.Add("@MenuHeaderID", request.HeaderMenuId);
                    }
                    IEnumerable<SubMenuConfigurationViewModel> response = await con.QueryAsync<SubMenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
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

        public async Task<List<SelectListItem>> GetAssignedUserSubMenu(long menuHeaderID)
        {
            var _listData = new List<SelectListItem>();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    string query = @"select uSubMainMenu.MenuID as Id,menuCon.Name as [Name]
                         from UserSubMenu as uSubMainMenu
                        inner join
                        MenuConfiguration as menuCon on menuCon.Id = uSubMainMenu.MenuID  ";
                    var parameters = new DynamicParameters();
                    if (menuHeaderID != 0)
                    {
                        query = query + " and  uSubMainMenu.MenuHeaderID=@MenuHeaderID";
                        parameters.Add("@MenuHeaderID", menuHeaderID);
                    }
                    var response = await con.QueryAsync<SubMenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    _listData.AddRange(response.Select(g => new SelectListItem { Text = g.Name, Value = g.Id.ToString() }).ToList());
                    return _listData;
                }
            }
            catch (Exception ex)
            {
                await ErrorLogUtility.SaveErrorLogAsync(ErrorPriority.Medium, this.GetType().Name + "->GetAssignedUserSubMenu", ex);
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
                    var query = "select Id,DisplayName as [Name] from " + AppTable.UserSubMenu + " (nolock) order by DisplayName asc ";
                    var parameters = new DynamicParameters();
                    var _data = await con.QueryAsync<MenuHeaderConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
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
        public async Task<SubMenuConfigurationViewModel> GetByIdAsync(long Id)
        {
            SubMenuConfigurationViewModel response = new SubMenuConfigurationViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,MenuHeaderID,MenuID as MainMenuId,DisplayName as Name,DisplayOrder,IsActive from " + AppTable.UserSubMenu + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<SubMenuConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        private async Task<ResponseModel> CheckIfMenuIsExist(string Name, int MenuID, int MenuHeaderID, long Id = 0)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                query = "SELECT count(id) FROM " + AppTable.UserSubMenu + " WHERE DisplayName=@DisplayName and MenuID =@MenuID and MenuHeaderID=@MenuHeaderID";
                if (Id != 0)// Check on insert
                {
                    query = query + " and Id!=@Id";
                    parameters.Add("@Id", Id);

                }
                parameters.Add("@DisplayName", Name);
                parameters.Add("@MenuID", MenuID);
                parameters.Add("@MenuHeaderID", MenuHeaderID);
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var _result = await con.QueryFirstOrDefaultAsync<int>(query, parameters);
                    query = null;
                    if (_result != 0)
                    {
                        return new ResponseModel { Status = false, Message = ResponseMessages.SubjectAlreadyExists("Menu Header") };
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
