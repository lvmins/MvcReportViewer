using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GPCT_Coin.Models;
using System.Data;
using Common;
using System.Web.Security;
using System.Transactions;
using BLL;
using Model;

namespace GPCT_Coin.Areas.SysManager.Controllers
{
    [FilterHelper]
    public class SysAdminController : Controller
    {
        SysAdminBLL adminBll;
        RoleBLL roleBll;
        RoleAuthorizeBLL raBll;
        dynamic ticket;
        public SysAdminController()
        {
            var cookie = CookieHelper.GetCookieValue(FormsAuthentication.FormsCookieName);
            ticket = FormsAuthentication.Decrypt(cookie);
            adminBll = new SysAdminBLL();
            roleBll = new RoleBLL();
            raBll = new RoleAuthorizeBLL();
        }
        //
        // GET: /Admin/Admin/ 
        public ActionResult Index(int? page)//, string SearchBy, string keyWord
        {
            PagingHelper.CurrentPage = page ?? 1;
            PagingHelper.PageSize = 10;
            int count = 0;
            //ViewData["SearchBy"] = SearchBy;
            //ViewData["keyWord"] = keyWord;
            //string strSql = "";
            //if (!string.IsNullOrEmpty(keyWord) && !string.IsNullOrEmpty(SearchBy))
            //{
            //    if (SearchBy == "createtime")
            //    {
            //        strSql += " and " + SearchBy.Trim() + ">='" + Convert.ToDateTime(keyWord.Trim()) + "'";
            //    }
            //    else if (SearchBy == "typename")
            //    {
            //        strSql += " and typeid in (select top 1 id FROM dbo.ArcType where typename like '%" + keyWord.Trim() + "%')";
            //    }
            //    else
            //    {
            //        strSql += " and " + SearchBy.Trim() + " like '%" + keyWord.Trim() + "%'";
            //    }
            //}
            DataTable dts = adminBll.GetAdminLists(" ", PagingHelper.CurrentPage, PagingHelper.PageSize, out count);//+ strSql
            PagingHelper.TotalCount = count;

            return View(dts);
        }

        #region 管理员管理

        #region 添加管理员
        public ActionResult AddAdminView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(SysAdminRequest admin)
        {
            if (admin == null)
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请稍后再试！" });
            }
            if (string.IsNullOrEmpty(admin.LoginAccount))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写登录账号！" });
            }
            if (string.IsNullOrEmpty(admin.Password))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写密码！" });
            }

            int num = adminBll.AddAdmin(new Coin_SysAdmin()
            {
                UserName = admin.UserName ?? "",
                RoleId = admin.RoleId,
                Area = admin.Area ?? "",
                WebChatAccount = admin.WebChatAccount ?? "",
                LoginAccount = admin.LoginAccount,
                Password = Common.Encrypt.MD5Encrypt(admin.Password, true),
                Gender = admin.Gender ?? "未知",
                PhoneNumber = admin.PhoneNumber ?? "",
                Email = admin.Email ?? "",
                State = true
                // = ticket != null ? Convert.ToInt32(ticket.UserData) : 0, 
            });
            if (num > 0)
            {
                return Json(new ResultModel { Result = true, Msg = "添加管理员成功" });
            }
            else
            {
                return Json(new ResultModel { Result = false, Msg = "添加管理员失败！" });
            }
        }
        #endregion

        #region 更新管理员

        public ActionResult UpdateAdminView(int ID)
        {
            if (ID < 0)
            {
                ViewBag.Errer = "ID异常";
                return View();
            }
            Coin_SysAdmin admin = adminBll.GetAdminForID(ID);
            SysAdminRequest model = new SysAdminRequest()
            {
                ID = admin.ID,
                UserName = admin.UserName,
                State = admin.State,
                RoleId = admin.RoleId,
                Area = admin.Area,
                WebChatAccount = admin.WebChatAccount,
                LoginAccount = admin.LoginAccount,
                Password = admin.Password,
                Gender = admin.Gender,
                PhoneNumber = admin.PhoneNumber,
                Email = admin.Email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateAdmin(SysAdminRequest admin)
        {
            if (admin == null)
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请稍后再试！" });
            }
            if (string.IsNullOrEmpty(admin.LoginAccount))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写登录账号！" });
            }
            if (string.IsNullOrEmpty(admin.Password))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写密码！" });
            }
            Coin_SysAdmin adminTemp = adminBll.GetAdminForID(admin.ID);
            int num = adminBll.UpdateAdmin(new Coin_SysAdmin()
            {
                ID = admin.ID,
                UserName = admin.UserName,
                RoleId = admin.RoleId,
                Area = admin.Area ?? "",
                WebChatAccount = admin.WebChatAccount ?? "",
                LoginAccount = admin.LoginAccount,
                Password = admin.Password == null ? adminTemp.Password : Common.Encrypt.MD5Encrypt(admin.Password, true),
                Gender = admin.Gender ?? "未知",
                PhoneNumber = admin.PhoneNumber ?? "",
                Email = admin.Email ?? "",
                State = true
                // = ticket != null ? Convert.ToInt32(ticket.UserData) : 0, 
            });
            if (num > 0)
            {
                return Json(new ResultModel { Result = true, Msg = "修改管理员成功！" });
            }
            else
            {
                return Json(new ResultModel { Result = false, Msg = "修改管理员失败！" });
            }
        }


        public PartialViewResult RoleItem()
        {
            BLL.RoleBLL roleBLL = new BLL.RoleBLL();
            List<Coin_Role> role = roleBLL.GetRoleList();
            return PartialView("_RoleItem", role);
        }

        #endregion

        #region 删除

        public ActionResult DeleteAdmin(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return Json(new ResultModel() { Result = false, Msg = "参数异常" });
            }
            int length = ID.Length;
            string id = ID;  //截取到最后一个逗号的长度，去除最后一个逗号  
            if (id.Contains(','))
            {
                id = ID.Substring(0, length - 1);
            }
            if (adminBll.SoftDelete(id) > 0)
            {
                return Json(new ResultModel() { Result = true, Msg = "" });
            }
            else
            {
                return Json(new ResultModel() { Result = false, Msg = "删除失败" });
            }
        }
        #endregion

        #endregion

        #region 角色管理

        public ActionResult RoleList(int? page)
        {
            PagingHelper.CurrentPage = page ?? 1;
            PagingHelper.PageSize = 10;
            int count = 0;
            DataTable dts = roleBll.GetRoleLists(" ", PagingHelper.CurrentPage, PagingHelper.PageSize, out count);//+ strSql
            PagingHelper.TotalCount = count;
            return View(dts);
        }

        public ActionResult AddRoleView()
        {
            return View();
        }

        public ActionResult AddRole(RoleRequest role)
        {
            if (role == null)
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请稍后再试！" });
            }
            if (string.IsNullOrEmpty(role.RoleName))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写角色名！" });
            }
            if (string.IsNullOrEmpty(role.RoleAuthorize))
            {
                return Json(new ResultModel { Result = false, Msg = "请选择授权！" });
            }
            int isTrue = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BLL.RoleBLL roleBll = new BLL.RoleBLL(); ;
                    BLL.RoleAuthorizeBLL raBll = new BLL.RoleAuthorizeBLL(); ;
                    int id = roleBll.AddRole(new Coin_Role()
                    {
                        RoleName = role.RoleName,
                        Description = role.Description ?? ""
                    });
                    if (id > 0)
                    {
                        using (TransactionScope ts1 = new TransactionScope(TransactionScopeOption.Required))
                        {
                            isTrue = raBll.AddRoleAuthorizeForRoleIds(role.RoleAuthorize, id);
                            ts1.Complete();
                        }
                    }
                    ts.Complete();
                }
                catch (Exception)
                {
                    return Json(new ResultModel { Result = false, Msg = "提交失败！" });
                }
            }
            return Json(new ResultModel { Result = isTrue > 0, Msg = "" });
        }

        public ActionResult UpdateRoleView(int raid)
        {
            if (raid < 0)
            {
                ViewBag.Errer = "ID异常";
                return View();
            }
            Coin_Role role = roleBll.GetRoleForRoleId(raid);
            List<Coin_RoleAuthorize> ra = raBll.GetRoleAuthorizeListsForRoleId(raid);
            if (role == null)
            {
                role = new Coin_Role();
            }
            if (ra == null)
            {
                ra = new List<Coin_RoleAuthorize>();
            }
            string roleAuthorize = "";
            for (int i = 0; i < ra.Count; i++)
            {
                roleAuthorize += ra[i].ID + ",";
            }
            RoleRequest rq = new RoleRequest()
            {
                ID = role.ID.ToString(),
                RoleName = role.RoleName,
                Description = role.Description,
                RoleAuthorize = roleAuthorize
            };
            return View(rq);
        }

        [HttpPost]
        public ActionResult UpdateRole(RoleRequest role)
        {
            if (role == null)
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请稍后再试！" });
            }
            if (string.IsNullOrEmpty(role.ID))
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常,请重新操作！" });
            }
            if (string.IsNullOrEmpty(role.RoleName))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写角色名！" });
            }
            if (string.IsNullOrEmpty(role.RoleAuthorize))
            {
                return Json(new ResultModel { Result = false, Msg = "请选择授权！" });
            }
            int isTrue = 0;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    BLL.RoleBLL roleBll = new BLL.RoleBLL(); ;
                    BLL.RoleAuthorizeBLL raBll = new BLL.RoleAuthorizeBLL(); ;
                    isTrue = roleBll.UpdateRole(new Coin_Role()
                    {
                        ID = Convert.ToInt32(role.ID),
                        RoleName = role.RoleName,
                        Description = role.Description ?? ""
                    });
                    if (isTrue > 0)
                    {
                        using (TransactionScope ts1 = new TransactionScope(TransactionScopeOption.Required))
                        {
                            isTrue = raBll.UpdateRoleAuthorizeForRoleIds(role.RoleAuthorize, role.ID);
                            ts1.Complete();
                        }
                    }
                    ts.Complete();
                }
                catch (Exception)
                {
                    return Json(new ResultModel { Result = false, Msg = "提交失败！" });
                }
            }
            return Json(new ResultModel { Result = isTrue > 0, Msg = "" });
            //return View();
        }

        public PartialViewResult RoleAuthorizeItem()
        {
            BLL.RoleBLL roleBLL = new BLL.RoleBLL();
            List<Coin_RoleAuthorize> roleAuthorize = raBll.GetRoleAuthorizeLists();
            return PartialView("_RoleAuthorizeItem", roleAuthorize);
        }

        #endregion

        #region 权限管理

        public ActionResult RoleAuthorizeList(int? page)
        {
            PagingHelper.CurrentPage = page ?? 1;
            PagingHelper.PageSize = 10;
            int count = 0;
            DataTable dts = raBll.GetRoleAuthorizeLists(" ", PagingHelper.CurrentPage, PagingHelper.PageSize, out count);//+ strSql
            PagingHelper.TotalCount = count;
            return View(dts);
        }

        public ActionResult AddRoleAuthorizeView()
        {
            return View();
        }

        public ActionResult AddRoleAuthorize(RoleAuthorizeRequest rar)
        {
            if (rar == null)
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请稍后再试！" });
            }
            if (string.IsNullOrEmpty(rar.Name))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写权限名称！" });
            }
            if (string.IsNullOrEmpty(rar.ControllerName))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写控制器名！" });
            }
            if (string.IsNullOrEmpty(rar.ActionName))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写方法名！" });
            }
            int isTrue = raBll.AddRoleAuthorize(new Coin_RoleAuthorize()
            {
                Name = rar.Name,
                ControllerName = rar.ControllerName,
                ActionName = rar.ActionName,
                RoleIds=""
            });
            if (isTrue > 0)
            {
                return Json(new ResultModel { Result = true, Msg = "" });
            }
            else
            {
                return Json(new ResultModel { Result =false, Msg = "添加权限失败！" });
            }
        }

        public ActionResult UpdateRoleAuthorizeView(int id)
        {
            if (id < 0)
            {
                ViewBag.Errer = "ID异常";
                return View();
            }
            Coin_RoleAuthorize ra = raBll.GetRoleAuthorizeForID(id.ToString()); 
            if (ra == null)
            {
                ra = new Coin_RoleAuthorize();
            }
            RoleAuthorizeRequest rar = new RoleAuthorizeRequest()
            {
                ID=ra.ID.ToString(),
                Name = ra.Name,
                ControllerName = ra.ControllerName,
                ActionName = ra.ActionName 
            };
            return View(rar);
        }

        [HttpPost]
        public ActionResult UpdateRoleAuthorize(RoleAuthorizeRequest rar)
        {
            if (rar == null)
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请稍后再试！" });
            }
            if (string.IsNullOrEmpty(rar.ID))
            {
                return Json(new ResultModel { Result = false, Msg = "接收参数异常，请重新进入！" });
            }
            if (string.IsNullOrEmpty(rar.Name))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写权限名称！" });
            }
            if (string.IsNullOrEmpty(rar.ControllerName))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写控制器名！" });
            }
            if (string.IsNullOrEmpty(rar.ActionName))
            {
                return Json(new ResultModel { Result = false, Msg = "请填写方法名！" });
            }
            int isTrue = raBll.UpdateRoleAuthorize(new Coin_RoleAuthorize()
            {
                ID=Convert.ToInt32(rar.ID),
                Name = rar.Name,
                ControllerName = rar.ControllerName,
                ActionName = rar.ActionName,
                RoleIds=""
            });
            if (isTrue > 0)
            {
                return Json(new ResultModel { Result = true, Msg = "" });
            }
            else
            {
                return Json(new ResultModel { Result =false, Msg = "添加权限失败！" });
            } 
        }

        public ActionResult DeleteRoleAuthorize(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return Json(new ResultModel() { Result = false, Msg = "参数异常" });
            }
            int length = ID.Length;
            string id = ID;  //截取到最后一个逗号的长度，去除最后一个逗号  
            if (id.Contains(','))
            {
                id = ID.Substring(0, length - 1);
            }
            if (raBll.BatchDeleteRoleAuthorize(id) > 0)
            {
                return Json(new ResultModel() { Result = true, Msg = "" });
            }
            else
            {
                return Json(new ResultModel() { Result = false, Msg = "删除失败" });
            }
        }
        #endregion
    }
}
