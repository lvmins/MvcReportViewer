using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using System.Data.SqlClient;

namespace DAL
{
    public class RoleDAL
    {
        public RoleDAL()
        {

        }

        public DataTable GetRoleAll()
        {
            string sql = "select * from Coin_Role";
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetRoleForId(int RoleId)
        {
            string sql = "select * from Coin_Role where ID=" + RoleId + "";
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetRoleLists(string where, int currentPage, int pageSize, out int rows)
        {
            string sql = null;
            sql += "select count(*) from [Coin_Role] where 1=1 " + where + ";";
            sql += "select top " + pageSize + " * from (";
            sql += "select row_number() over(order by a.ID desc) as rowid,a.*,stuff((select ','+LoginAccount from Coin_SysAdmin where RoleId=a.ID FOR xml PATH('')), 1, 1, '') as Coin_SysAdmin from [Coin_Role] a where 1=1 " + where + "";
            sql += ") as tb where rowid>" + pageSize + "*(" + currentPage + "-1)";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            rows = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            DataTable dt = ds.Tables[1];
            return dt;
        }

        public int AddRole(Coin_Role role)
        {
            string sql = @"INSERT INTO Coin_Role(RoleName,Description) VALUES (@RoleName,@Description);select @@IDENTITY;";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@RoleName",role.RoleName), 
                new SqlParameter("@Description",role.Description) 
                };
            return SQLHelper.ExecuteScalar(sql, paras);
        }

        public int UpdateRole(Coin_Role role)
        {
            string sql = @"update Coin_Role set RoleName=@RoleName,Description=@Description where ID=@ID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@ID",role.ID), 
                new SqlParameter("@RoleName",role.RoleName), 
                new SqlParameter("@Description",role.Description) 
                };
            return SQLHelper.ExecuteNonQuery(sql, paras);
        }

    }
}
