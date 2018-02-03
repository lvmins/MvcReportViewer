using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;
using System.Data.SqlClient;
using System.Collections;

namespace DAL
{
    public class RoleAuthorizeDAL
    {
        public RoleAuthorizeDAL()
        {

        }

        public DataTable GetRoleAuthorizeAll()
        {
            string sql = "select * from Coin_RoleAuthorize";
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetRoleAuthorize(string ControllerName, string ActionName)
        {
            string sql = "select * from Coin_RoleAuthorize where ControllerName='" + ControllerName + "' and ActionName='" + ActionName + "'";
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetRoleAuthorizeListsForRoleId(int roleId)
        {
            string sql = string.Format("select * from Coin_RoleAuthorize where RoleIds like '%{0}%'", roleId);
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetThisRoleAuthorizeForID(string roleId)
        {
            string sql = string.Format("select * from Coin_RoleAuthorize where ID in ({0})", roleId);
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetRoleAuthorizeForID(string ID)
        {
            string sql = string.Format("select * from Coin_RoleAuthorize where ID={0}", ID);
            return SQLHelper.ExecuteDataTable(sql);
        }

        public DataTable GetRoleAuthorizeLists(string where, int currentPage, int pageSize, out int rows)
        {
            string sql = null;
            sql += "select count(*) from [Coin_RoleAuthorize] where 1=1 " + where + ";";
            sql += "select top " + pageSize + " * from (";
            sql += "select row_number() over(order by a.ID desc) as rowid,a.* from [Coin_RoleAuthorize] a where 1=1 " + where + "";
            sql += ") as tb where rowid>" + pageSize + "*(" + currentPage + "-1)";
            DataSet ds = SQLHelper.ExecuteDataSet(sql);
            rows = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            DataTable dt = ds.Tables[1];
            return dt;
        }

        public int AddRoleAuthorize(Coin_RoleAuthorize ra)
        {
            string sql = @"INSERT INTO Coin_RoleAuthorize(Name,ControllerName,ActionName,RoleIds) VALUES (@Name,@ControllerName,@ActionName,@RoleIds);";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@Name",ra.Name), 
                new SqlParameter("@ControllerName",ra.ControllerName),
                new SqlParameter("@ActionName",ra.ActionName), 
                new SqlParameter("@RoleIds",ra.RoleIds), 
                };
            return SQLHelper.ExecuteNonQuery(sql, paras);
        }

        public int UpdateRoleAuthorize(Coin_RoleAuthorize ra)
        {
            string sql = @"update Coin_RoleAuthorize set Name=@Name,ControllerName=@ControllerName,ActionName=@ActionName,RoleIds=@RoleIds where ID=@ID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@ID",ra.ID),
                new SqlParameter("@Name",ra.Name), 
                new SqlParameter("@ControllerName",ra.ControllerName),
                new SqlParameter("@ActionName",ra.ActionName), 
                new SqlParameter("@RoleIds",ra.RoleIds), 
                };
            return SQLHelper.ExecuteNonQuery(sql, paras);
        }

        public int UpdateRoleAuthorizeForID(Coin_RoleAuthorize ra)
        {
            string sql = @"update Coin_RoleAuthorize set RoleIds=@RoleIds where ID=@ID";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@ID",ra.ID), 
                new SqlParameter("@RoleIds",ra.RoleIds)
                };
            return SQLHelper.ExecuteNonQuery(sql, paras);
        }

        public int AddRoleAuthorizeForRoleIds(string RoleAuthorizeIds, int ID)
        {
            string id = RoleAuthorizeIds;
            if (RoleAuthorizeIds.Contains(','))
            {
                int length = RoleAuthorizeIds.Length;
                id = RoleAuthorizeIds.Substring(0, length - 1);  //截取到最后一个逗号的长度，去除最后一个逗号
            }
            string raSql = string.Format("select * from Coin_RoleAuthorize where ID in ({0})", id);
            DataTable dt = SQLHelper.ExecuteDataTable(raSql);
            string sql = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string roleIds = dt.Rows[i]["RoleIds"].ToString() + "," + ID + ";";
                roleIds = FilterRepetitionChar(roleIds);
                sql += "update Coin_RoleAuthorize set RoleIds='" + roleIds + "' where ID=" + dt.Rows[i]["ID"] + ";";
            }
            return SQLHelper.ExecuteNonQuery(sql);
        }

        public int UpdateRoleAuthorizeForRoleIds(string RoleAuthorizeIds, string ID)
        {
            string RaIdList = RoleAuthorizeIds;//修改操作所选择授权的权限ID
            if (RoleAuthorizeIds.Contains(','))
            {
                int length = RoleAuthorizeIds.Length;
                RaIdList = RoleAuthorizeIds.Substring(0, length - 1);
            }
            DataTable dt = GetRoleAuthorizeListsForRoleId(Convert.ToInt32(ID));//已有的权限ID 
            string sql = "";
            string dbID = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dbID += dt.Rows[i]["ID"].ToString() + ",";
            }
            string[] inputList = RaIdList.Split(',');//传入的权限id
            string[] dbIDList = dbID.Split(',');//已有的权限ID
            for (int i = 0; i < inputList.Count(); i++)
            {
                if (!dbID.Contains(inputList[i]))//当前角色在数据库中没有传入的权限  即添加操作
                {
                    DataTable thisdt = GetThisRoleAuthorizeForID(inputList[i]);
                    for (int j = 0; j < thisdt.Rows.Count; j++)
                    {
                        string roleIds = thisdt.Rows[j]["RoleIds"].ToString() + "," + ID + "";
                        roleIds = FilterRepetitionChar(roleIds);
                        sql += "update Coin_RoleAuthorize set RoleIds='" + roleIds + "' where ID=" + thisdt.Rows[j]["ID"] + ";";
                    }
                }
            }
            for (int i = 0; i < dbIDList.Count(); i++)
            {
                if (!RaIdList.Contains(dbIDList[i]))
                {
                    DataTable thisdt = GetThisRoleAuthorizeForID(dbIDList[i]);
                    for (int j = 0; j < thisdt.Rows.Count; j++)
                    {
                        string roleIds = "";
                        if (thisdt.Rows[j]["RoleIds"].ToString().IndexOf("" + ID + "") > 1)
                        {
                            roleIds = thisdt.Rows[j]["RoleIds"].ToString().Replace("," + ID, "");
                        }
                        else {
                            roleIds = thisdt.Rows[j]["RoleIds"].ToString().Replace(ID + ",", "");
                        }
                        roleIds = FilterRepetitionChar(roleIds);
                        sql += "update Coin_RoleAuthorize set RoleIds='" + roleIds + "' where ID=" + thisdt.Rows[j]["ID"] + ";";
                    }
                }
            }
            return SQLHelper.ExecuteNonQuery(sql);
        }


        #region 过滤字符串中的重复字符
        /// <summary>
        /// 过滤字符串中的重复字符
        /// </summary>
        /// <param name="str">要过滤的字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public string FilterRepetitionChar(string sourceStr)
        {
            string returnStr = string.Empty;
            string[] strList = sourceStr.Split(',');
            Hashtable ht = new Hashtable();
            foreach (string strChar in strList)
            {
                if (!ht.ContainsKey(strChar))
                {
                    ht.Add(strChar, strChar);//这里让ht的key和value值相等，不影响下面的程序
                    returnStr += strChar + ",";//字符以逗号分隔
                }
            }
            returnStr = returnStr.Trim(',');//去掉最后一个逗号
            return returnStr;
        }

        public string[] CompareString(string thisString1, string thisString2)
        {
            string[] arr1 = thisString1.Split(',');
            string[] arr2 = thisString2.Split(',');

            //找出相同元素(即交集)
            var sameArr = arr1.Intersect(arr2).ToArray();

            //找出不同的元素(即交集的补集)
            var diffArr = arr1.Where(c => !arr2.Contains(c)).ToArray();
            return diffArr;
        }


        #endregion

        public int BatchDeleteRoleAuthorize(string ID)
        {
            string sql = string.Format("delete Coin_RoleAuthorize where ID in ({0})", ID);
            return SQLHelper.ExecuteNonQuery(sql);
        }

    }
}
