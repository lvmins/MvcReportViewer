using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using Model;
using Common;

namespace BLL
{
    public class SysAdminBLL
    {
        SysAdminDAL dal;
        ModelHandler<Coin_SysAdmin> handler;
        public SysAdminBLL()
        {
            dal = new SysAdminDAL();
            handler = new ModelHandler<Coin_SysAdmin>();
        }

        public DataTable GetAdminAll()
        {
            return dal.GetAdminAll();
        }

        public Coin_SysAdmin GetAdminForID(int ID)
        {
            DataTable dt = dal.GetAdminForID(ID);
            return handler.FillModel(dt.Rows.Count > 0 ? dt.Rows[0] : null);
        }

        public Coin_SysAdmin GetAdminForUserName(string UserName)
        {
            DataTable dt = dal.GetAdminForUserName(UserName);
            return handler.FillModel(dt.Rows.Count > 0 ? dt.Rows[0] : null);
        }

        public DataTable GetAdminLists(string where, int currentPage, int pageSize, out int rows)
        {
            return dal.GetAdminLists(where,currentPage,pageSize,out rows);
        }

        public int AddAdmin(Coin_SysAdmin admin)
        {
            return dal.AddAdmin(admin);
        }

        public int UpdateAdmin(Coin_SysAdmin admin)
        {
            return dal.UpdateAdmin(admin);
        }

        public int BatchDeleteAdmin(string ID)
        {
            return dal.BatchDeleteAdmin(ID);
        }

        public int SoftDelete(string ID)
        {
            return dal.SoftDelete(ID);
        }

    }
}
