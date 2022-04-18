
using Dapper;
using dsdProjectTemplate.Services.AppActionsHistory;
using dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration.SubMenuConfiguration;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace dsdProjectTemplate.Services.Menu.MenuHeaderConfiguration
{
    public class MenuHeaderConfigurationService: BaseServiceClass,IMenuHeaderConfigurationService
    {
        IActionsHistoryService _actionsHistoryService = new ActionsHistoryService();
        ISubMenuConfigurationService _subMenuConfigurationService = new SubMenuConfigurationService();
        string _serviceFor = "Menu Header";

        public MenuHeaderConfigurationService()
        {
        }
        public async Task<ResponseModel> AddAsync(MenuHeaderConfigurationViewModel request)
        {
            if (!this.CanAddRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }

            try
            {
                var _existRecordResponse = await CheckIfMenuIsExist(request.MainMenuId,request.UserRoleId,request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }
                //List<UserSubMenu> _lstMenuHeaderAssignmen=    MenuHeaderAssignment(request.ListMenu);
                var role = await new UsersRoleService().GetByIdAsync(request.UserRoleId);
                var mainMenu = await new MenuConfigurationService().GetByIdAsync(request.MainMenuId);
                mainMenu = (mainMenu == null) ? new MenuConfigurationViewModel() : mainMenu;
                // Insert record in the database
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "INSERT INTO " + AppTable.UserMainMenu + " (DisplayName,MenuID,MenuKey,DisplayOrder,UserRoleID,IsActive,CreatedBy,CreatedDate) " +
                        "VALUES (@DisplayName,@MenuID,@MenuKey,@DisplayOrder,@UserRoleID, @IsActive,@CreatedBy,@CreatedDate)";
                    query = query + " SELECT CAST(scope_identity() AS int);";
                    var parameters = new DynamicParameters();
                    parameters.Add("@DisplayName", request.Name);
                    if(request.MainMenuId == 0)
                    {
                        parameters.Add("@MenuID", null);
                    }
                    else
                    {
                        parameters.Add("@MenuID", request.MainMenuId);
                    }
                    
                    parameters.Add("@MenuKey", Regex.Replace(role.RoleName + request.Name + Convert.ToString(mainMenu.MenuController) + Convert.ToString(mainMenu.MenuAction), @"\s+", ""));
                    parameters.Add("@DisplayOrder", request.DisplayOrder);
                    parameters.Add("@UserRoleID", request.UserRoleId);

                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@CreatedBy", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@CreatedDate", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    request.Id = await con.ExecuteScalarAsync<int>(query, parameters);
                    con.Close();
                    await _subMenuConfigurationService.AddUpdateSubMenu(request.ListMenu, request.Id);
                    //start ActionsHistory
                    var InsertedItem =  await GetByIdAsync(request.Id);
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Insert.ToString(),
                        TableName = AppTable.UserMainMenu.ToString(),
                        PrimaryKeyId = request.Id,
                        OrganizationId = role.OrganizationId,
                        Data = JsonConvert.SerializeObject(new { newRec = InsertedItem })
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

        public async Task<ResponseModel> UpdateAsync(MenuHeaderConfigurationViewModel request)
        {
            if (!this.CanEditRecords())
            {
                return new ResponseModel { Message = ResponseMessages.NotAuthorized, Status = false, Id = 0 };
            }
            try
            {
                var _existRecordResponse = await CheckIfMenuIsExist(request.MainMenuId, request.UserRoleId, request.Id);
                if (!_existRecordResponse.Status)
                {
                    return _existRecordResponse;
                }

                var item = await GetByIdAsync(request.Id);
                string oldRecord = JsonConvert.SerializeObject(item);
                if (item != null)
                {
                    var role = await new UsersRoleService().GetByIdAsync(request.UserRoleId);
                    var mainMenu = await new MenuConfigurationService().GetByIdAsync(request.MainMenuId);
                    mainMenu = (mainMenu == null) ? new MenuConfigurationViewModel(): mainMenu;
                    
                    using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                    {
                        var query = "UPDATE " + AppTable.UserMainMenu + " SET  DisplayName=@DisplayName,DisplayOrder=@DisplayOrder," +
                            "MenuID=@MenuID,MenuKey=@MenuKey,UserRoleID=@UserRoleID,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate WHERE Id=@Id";

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", request.Id);
                        parameters.Add("@DisplayName", request.Name);
                        parameters.Add("@DisplayOrder", request.DisplayOrder);
                        if(request.MainMenuId == 0)
                        {
                            parameters.Add("@MenuID", null);
                        }
                        else
                        {
                            parameters.Add("@MenuID", request.MainMenuId);
                        }
                        
                        parameters.Add("@MenuKey", Regex.Replace(role.RoleName + request.Name + Convert.ToString(mainMenu.MenuController) + Convert.ToString(mainMenu.MenuAction), @"\s+", ""));
                        parameters.Add("@UserRoleID", request.UserRoleId);

                        parameters.Add("@IsActive", request.IsActive);
                        parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        // Open Connection & Execute the query
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        await con.ExecuteAsync(query, parameters);
                        con.Close();
                    }
                    await _subMenuConfigurationService.AddUpdateSubMenu(request.ListMenu,request.Id);                   

                    //start ActionsHistory
                    await _actionsHistoryService.AddAsync(new ActionsHistoryViewModel
                    {
                        ActionType = UserActon.Update.ToString(),
                        TableName = AppTable.UserMainMenu.ToString(),
                        PrimaryKeyId = item.Id,
                        OrganizationId = role.OrganizationId,
                        Data = JsonConvert.SerializeObject(new { newRec = request, oldRec = JsonConvert.DeserializeObject(oldRecord) })
                    });
                    //oldRecord = null;
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
        public async Task<IEnumerable<MenuHeaderConfigurationViewModel>> GetAllAsync(HeaderConfigurationSearchRequest request)
        {
            try
            {
              
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    string query = @"select uMainMenu.id,uMainMenu.DisplayName as [Name],  uMainMenu.IsActive,uMainMenu.DisplayOrder,uMainMenu.UserRoleID,uRole.OrganizationId,menuCon.MenuAction,menuCon.MenuController,menuCon.Id as MainMenuId,menuCon.Name AS MainMenu
                         from UserMainMenu as uMainMenu
                        left join
                        MenuConfiguration as menuCon on menuCon.Id = uMainMenu.MenuID
                        inner join
                        UsersRoles as uRole on uRole.Id = uMainMenu.UserRoleID";
                    var parameters = new DynamicParameters();
                    if (request.RoleId != 0)
                    {
                        query = query + " and  uMainMenu.UserRoleID=@RoleID";
                       parameters.Add("@RoleID", request.RoleId);
                    }
                    IEnumerable<MenuHeaderConfigurationViewModel> response = await con.QueryAsync<MenuHeaderConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();                    
                    foreach (var item in response)
                    {
                        item.ListMenu = await _subMenuConfigurationService.GetAssignedUserSubMenu(item.Id);
                    }

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
                    var query = "select Id,DisplayName as [Name] from " + AppTable.UserMainMenu + " (nolock) order by DisplayName asc ";
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
        private async Task<ResponseModel> CheckIfMenuIsExist(int? MainMenuId,int UserRoleId, long Id = 0)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                query = "SELECT count(id) FROM " + AppTable.UserMainMenu + " WHERE MenuID =@MainMenuId and UserRoleId=@UserRoleId";
                if (Id != 0)// Check on insert
                {
                    query = query + " and Id!=@Id";
                    parameters.Add("@Id", Id);
                   
                }

                parameters.Add("@MainMenuId" , MainMenuId);
                parameters.Add("@UserRoleId", UserRoleId);
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
        public async Task<MenuHeaderConfigurationViewModel> GetByIdAsync(long Id)
        {
            MenuHeaderConfigurationViewModel response = new MenuHeaderConfigurationViewModel();
            try
            {
                using (var con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "select Id,UserRoleID,MenuID as MainMenuId,MenuKey,DisplayName as Name,DisplayOrder,IsActive from " + AppTable.UserMainMenu + " (nolock) where Id=@Id ";
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id);
                    response = await con.QueryFirstOrDefaultAsync<MenuHeaderConfigurationViewModel>(query, parameters, commandType: CommandType.Text);
                    con.Close();
                    return response;
                }

            }
            catch (Exception)
            {

                return response;
            }
        }
        //private List<UserSubMenu> MenuHeaderAssignment(List<SelectListItem> ListMenu)
        //{
        //    List<UserSubMenu> _lstMenuHeaderAssignment = new List<UserSubMenu>();
        //    if (ListMenu != null)
        //    {
        //        foreach (var item in ListMenu)
        //        {
        //            int MenuConfigurationsID = Convert.ToInt32(item.Value);
        //            UserSubMenu MenuHeaderAssignmentToInsert = new UserSubMenu
        //            {
        //                MenuConfiguration = _context.MenuConfigurations.Where(e => e.Id == MenuConfigurationsID).FirstOrDefault(),
        //                CreatedBy = UserSession.Current.loggedIn_UserId,
        //                CreatedDate = DateTime.UtcNow

        //            };
        //            _lstMenuHeaderAssignment.Add(MenuHeaderAssignmentToInsert);
        //        }

        //    }
        //    return _lstMenuHeaderAssignment;
        //}
    }
}
