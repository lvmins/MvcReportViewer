using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Web.Security;
using BLL; 

namespace GPCT_Coin.Areas.SysManager.Controllers
{
    public class LoginController : Controller
    {
        SysAdminBLL bll = new SysAdminBLL();
        //
        // GET: /Admin/Login/ 
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string LoginName, string Password)//, string txtCode
        {
            int intError = 0;
            if (string.IsNullOrEmpty(LoginName))
            {
                intError++;
                ViewBag.Account = "登录名不能为空.";
            }
            if (string.IsNullOrEmpty(Password))
            {
                intError++;
                ViewBag.Password = "密码不能为空.";
            }
            ViewData["LoginName"] = LoginName;
            ViewData["Password"] = Password;
            ////判断验证码
            //if (txtCode != Session["Code"].ToString())
            //{
            //    intError++;
            //    ViewBag.Code = "Code can't Equals.";
            //}
            Model.Coin_SysAdmin model = bll.GetAdminForUserName(LoginName);
            if (model != null)
            {
                if (model.UserName == null)
                {
                    intError++;
                    ViewBag.Code = "用户不存在.";
                }
                else if (model.Password == null)
                {
                    intError++;
                    ViewBag.Code = "出现异常.";
                } 
            }
            else
            {
                intError++;
                ViewBag.Code = "用户不存在.";
            }
            if (intError > 0)
            {
                return RedirectToAction("Index","Login");
            }
            string sa = Common.Encrypt.MD5Encrypt(Password, true);
            //判断密码  
            if (model.Password.Trim() == Common.Encrypt.MD5Encrypt(Password, true))
            {
                //FormsAuthentication.SetAuthCookie(model.UserName + "," + model.ID, true);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                    (1,
                        model.UserName,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(200),
                        true,
                        model.ID.ToString(),
                        "/"
                    );
                CookieHelper.SetCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Password = "Login Account or Password is wrong!";
                return View();
            }
        }

        public ActionResult ShowValidateCode()
        {
            Common.ValidateCode ValidateCode = new Common.ValidateCode();

            string code = ValidateCode.CreateValidateCode(4);//生成验证码，传几就是几位验证码
            Session["Code"] = code;
            byte[] buffer = ValidateCode.CreateValidateGraphic(code);//把验证码画到画布
            return File(buffer, "image/jpeg");
        }


    }
}
