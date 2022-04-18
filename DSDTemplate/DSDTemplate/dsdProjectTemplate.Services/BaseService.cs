using Dapper;
using dsdProjectTemplate.Utility;
using dsdProjectTemplate.ViewModel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace dsdProjectTemplate.Services
{
    public class BaseServiceClass
    {
        
        public bool CanEditRecords()
        {
            //TODO: We can add DB logic soon
            return UserSession.Current.CanEditRecords;
        }
       public bool CanAddRecords()
        {
            //TODO: We can add DB logic soon
            return UserSession.Current.CanAddRecords;
        }
        public  async Task<ResponseModel> ActiveOrDeActiveAsync(string tableName, int Id, bool activeFlg)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    var query = "UPDATE " + tableName + " SET  activeFlg=@activeFlg WHERE Id=@Id";

                    var parameters = new DynamicParameters();
                    parameters.Add("@activeFlg", activeFlg);
                    parameters.Add("@Id", Id);
                    parameters.Add("@UpdatedBy", UserSession.Current.loggedIn_UserId);
                    parameters.Add("@UpdatedOn", DateTime.UtcNow);
                    // Open Connection & Execute the query
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    await con.ExecuteAsync(query, parameters);
                    return new ResponseModel { Status = true, Message = ResponseMessages.SubjectUpdatedSuccess("record") };
                }
            }
            catch (Exception)
            {
                return new ResponseModel { Status = false, Message = ResponseMessages.System_Error };
            }
        }

        public  async Task<ResponseModel> CheckIfRecordIsExist(string tableName, string columName, string columValue, long Id = 0)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                if (Id == 0)// Check on insert
                {
                    query = "SELECT count(" + columName + ") FROM " + tableName + " WHERE " + columName + " =@" + columName;
                }
                else// Check on update
                {
                    query = "SELECT count(" + columName + ") FROM " + tableName + " WHERE Id !=@Id and " + columName + " =@" + columName;
                    parameters.Add("@Id", Id);
                }

                parameters.Add("@" + columName, columValue);
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var _result = await con.QueryFirstOrDefaultAsync<int>(query, parameters);
                    query = null;
                    if (_result != 0)
                    {
                        return new ResponseModel { Status = false, Message = ResponseMessages.SubjectAlreadyExists(columValue) };
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
        public async Task<ResponseModel> CheckIfRecordIsExist(string tableName, string columName, string columValue, long Id = 0,long OrganizationId=0)
        {
            string query = string.Empty;
            try
            {
                var parameters = new DynamicParameters();
                if (Id == 0)// Check on insert
                {
                    query = "SELECT count(" + columName + ") FROM " + tableName + " WHERE " + columName + " =@" + columName + " and OrganizationId=@OrganizationId";
                }
                else// Check on update
                {
                    query = "SELECT count(" + columName + ") FROM " + tableName + " WHERE Id !=@Id and " + columName + " =@" + columName + " and OrganizationId=@OrganizationId";
                    parameters.Add("@Id", Id);
                }

                parameters.Add("@" + columName, columValue);
                parameters.Add("@OrganizationId" , OrganizationId);
                using (IDbConnection con = new SqlConnection(SQLConnectionString.dbConnection))
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    var _result = await con.QueryFirstOrDefaultAsync<int>(query, parameters);
                    query = null;
                    if (_result != 0)
                    {
                        return new ResponseModel { Status = false, Message = ResponseMessages.SubjectAlreadyExists(columValue) };
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
