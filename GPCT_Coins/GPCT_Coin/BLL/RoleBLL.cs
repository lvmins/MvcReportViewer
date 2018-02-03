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
    public class RoleBLL
    {
        RoleDAL dal;
        ModelHandler<Coin_Role> handler;
        public RoleBLL()
        {
            dal = new RoleDAL();
            handler = new ModelHandler<Coin_Role>();
        }

        public DataTable GetRoleAll()
        {
            return dal.GetRoleAll();
        }

        public List<Coin_Role> GetRoleList()
        {
            DataTable dt = dal.GetRoleAll();
            return handler.FillModel(dt);
        }

        public DataTable GetRoleLists(string where, int currentPage, int pageSize, out int rows)
        {
            return dal.GetRoleLists(where, currentPage, pageSize, out rows);
        }

        public Coin_Role GetRoleForRoleId(int RoleId)
        {
            DataTable dt = dal.GetRoleForId(RoleId);
            return handler.FillModel(dt.Rows.Count > 0 ? dt.Rows[0] : null);
        }

        public int AddRole(Coin_Role role)
        {
            return dal.AddRole(role);
        }

        public int UpdateRole(Coin_Role role)
        {
            return dal.UpdateRole(role);
        }

    }
}
