using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model; 
using System.Data.SqlClient;

namespace DAL
{
   public class SysAdminDAL
    {   
       public DataTable GetAdminAll()
       {
           string sql = "select * from Coin_SysAdmin"; 
           return SQLHelper.ExecuteDataTable(sql);
       }

       public DataTable GetAdminForUserName(string UserName)
       {
           string sql = "select * from Coin_SysAdmin where LoginAccount='" + UserName + "'";
           return SQLHelper.ExecuteDataTable(sql);
       }

       public DataTable GetAdminForID(int ID)
       {
           string sql = string.Format("select * from Coin_SysAdmin where ID={0}", ID);
           return SQLHelper.ExecuteDataTable(sql);
       }

       public DataTable GetAdminLists(string where, int currentPage, int pageSize, out int rows)
       {
           string sql = null;
           sql += "select count(*) from [Coin_SysAdmin] where 1=1 " + where + ";";
           sql += "select top " + pageSize + " * from (";
           sql += "select row_number() over(order by a.ID desc) as rowid,a.*,c.RoleName,c.Description from [Coin_SysAdmin] a left join [Coin_Role] c on a.RoleId=c.ID where 1=1 " + where + "";
           sql += ") as tb where rowid>" + pageSize + "*(" + currentPage + "-1)";
           DataSet ds = SQLHelper.ExecuteDataSet(sql);
           rows = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
           DataTable dt = ds.Tables[1];
           return dt;
       }

       public int AddAdmin(Coin_SysAdmin admin)
       {
           string sql = @"INSERT INTO Coin_SysAdmin(RoleId,UserName,Area,WebChatAccount,LoginAccount,Password,Gender,PhoneNumber,Email)
                    VALUES (@RoleId,@UserName,@Area,@WebChatAccount,@LoginAccount,@Password,@Gender,@PhoneNumber,@Email);select @@IDENTITY;";
           SqlParameter[] paras = {
			            new SqlParameter("@State",admin.State) ,            
                        new SqlParameter("@RoleId",admin.RoleId) ,            
                        new SqlParameter("@UserName",admin.UserName),            
                        new SqlParameter("@Area",admin.Area),            
                        new SqlParameter("@WebChatAccount",admin.WebChatAccount),            
                        new SqlParameter("@LoginAccount",admin.LoginAccount),            
                        new SqlParameter("@Password",admin.Password),            
                        new SqlParameter("@Gender",admin.Gender),            
                        new SqlParameter("@PhoneNumber",admin.PhoneNumber),            
                        new SqlParameter("@Email",admin.Email)             
              
            };
           return SQLHelper.ExecuteScalar(sql, paras);
       }

       public int UpdateAdmin(Coin_SysAdmin admin)
       {
           string sql = @"update SysAdmin set State=@State,                                                
                        RoleId=@RoleId,                                                
                        UserName=@UserName,                                                
                        Area=@Area,                                                
                        WebChatAccount=@WebChatAccount,                                                
                        LoginAccount=@LoginAccount,                                                
                        Password=@Password,                                                
                        Gender=@Gender,                                                
                        PhoneNumber=@PhoneNumber,                                                
                        Email=@Email where ID=@ID";
           SqlParameter[] paras = {
			            new SqlParameter("@State",admin.State) ,            
                        new SqlParameter("@RoleId",admin.RoleId) ,            
                        new SqlParameter("@UserName",admin.UserName),            
                        new SqlParameter("@Area",admin.Area),            
                        new SqlParameter("@WebChatAccount",admin.WebChatAccount),            
                        new SqlParameter("@LoginAccount",admin.LoginAccount),            
                        new SqlParameter("@Password",admin.Password),            
                        new SqlParameter("@Gender",admin.Gender),            
                        new SqlParameter("@PhoneNumber",admin.PhoneNumber),            
                        new SqlParameter("@Email",admin.Email),
                        new SqlParameter("@ID",admin.ID)
              
            };
           return SQLHelper.ExecuteNonQuery(sql, paras);
       }

       public int BatchDeleteAdmin(string ID)
       {
           string sql = string.Format("delete Coin_SysAdmin where ID in ({0})", ID);
           return SQLHelper.ExecuteNonQuery(sql);
       }

       public int SoftDelete(string ID)
       {
           string sql = string.Format("update Coin_SysAdmin set State=false where ID in ({0})", ID);
           return SQLHelper.ExecuteNonQuery(sql);
       }

    }
}
