using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;
using Model;
using DAL;

namespace BLL
{
    public class RoleAuthorizeBLL
    {
        RoleAuthorizeDAL dal;
        ModelHandler<Coin_RoleAuthorize> handler;
        public RoleAuthorizeBLL()
        {
            dal = new RoleAuthorizeDAL();
            handler = new ModelHandler<Coin_RoleAuthorize>();
        }

        public DataTable GetRoleAuthorizeAll()
        {
            return dal.GetRoleAuthorizeAll();
        }
        public DataTable GetRoleAuthorizeLists(string where, int currentPage, int pageSize, out int rows)
        {
            return dal.GetRoleAuthorizeLists(where,currentPage,pageSize,out rows);
        }
        
        public List<Coin_RoleAuthorize> GetRoleAuthorizeLists()
        {
            DataTable dt = dal.GetRoleAuthorizeAll();
            return handler.FillModel(dt);
        }

        public List<Coin_RoleAuthorize> GetRoleAuthorizeListsForRoleId(int roleId)
        {
            DataTable dt = dal.GetRoleAuthorizeListsForRoleId(roleId);
            return handler.FillModel(dt);
        }

        public Coin_RoleAuthorize GetRoleAuthorize(string ControllerName, string ActionName)
        {
            DataTable dt = dal.GetRoleAuthorize(ControllerName, ActionName);
            return handler.FillModel(dt.Rows.Count > 0 ? dt.Rows[0] : null); 
        }

        public Coin_RoleAuthorize GetRoleAuthorizeForID(string ID)
        {
            DataTable dt = dal.GetRoleAuthorizeForID(ID);
            return handler.FillModel(dt.Rows.Count > 0 ? dt.Rows[0] : null); 
        }

        public int AddRoleAuthorize(Coin_RoleAuthorize ra)
        { 
            return dal.AddRoleAuthorize(ra);
        }

        public int UpdateRoleAuthorize(Coin_RoleAuthorize ra)
        { 
            return dal.UpdateRoleAuthorize(ra);
        }

        public int UpdateRoleAuthorizeForID(Coin_RoleAuthorize ra)
        {
            return dal.UpdateRoleAuthorizeForID(ra);
        }

        public int AddRoleAuthorizeForRoleIds(string RoleAuthorizeIds, int ID)
        {
            return dal.AddRoleAuthorizeForRoleIds(RoleAuthorizeIds, ID);
        }

        public int UpdateRoleAuthorizeForRoleIds(string RoleAuthorizeIds, string ID)
        {
            return dal.UpdateRoleAuthorizeForRoleIds(RoleAuthorizeIds, ID);
        }

        public int BatchDeleteRoleAuthorize(string ID)
        {
            return dal.BatchDeleteRoleAuthorize(ID);
        }

    }
}
