using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Common
{
    public class ConfigHelper
    {
        #region EntityFramework上下文重写数据库连接
        //public Sands_GPCT_KOCASOContext()
        //    //: base("Name=Sands_GPCT_KOCASOContext")
        //{
        //    base.Database.Connection.ConnectionString = ConfigHelper.Sands_GPCT_KOCASOContext;
        //}
        #endregion
        #region SQLHelper.cs更改
        //public static readonly string ConnStr = Common.ConfigHelper.Sands_GPCT_Orders;
        #endregion
        public static string ConfigPath = ConfigurationManager.AppSettings["ConfigPath"];
        public static Configuration Config
        {
            get
            {
                if (File.Exists(ConfigPath))
                {
                    ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                    map.ExeConfigFilename = ConfigPath;
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                    return config;
                }
                return null;
            }
        }
        #region connectionStrings
        public static string Sands_GPCT_Orders
        {
            get
            {
                if (Config != null)
                {
                    return Config.ConnectionStrings.ConnectionStrings["Sands_GPCT_Orders"].ConnectionString;
                }
                return System.Configuration.ConfigurationManager.ConnectionStrings["Sands_GPCT_Orders"].ConnectionString;
            }
        }
        public static string Sands_GPCT_Coin
        {
            get
            {
                if (Config != null)
                {
                    return Config.ConnectionStrings.ConnectionStrings["Sands_GPCT_Coin"].ConnectionString;
                }
                return System.Configuration.ConfigurationManager.ConnectionStrings["Sands_GPCT_Coin"].ConnectionString;
            }
        }
        #endregion
        #region appSettings
        #region Url
        public static string UrlWeb
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["UrlWeb"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["UrlWeb"].ToString();
            }
        }
        public static string UrlAdmin
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["UrlAdmin"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["UrlAdmin"].ToString();
            }
        }
        public static string M01Web
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["M01Web"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["M01Web"].ToString();
            }
        }
        #endregion
        #region Img
        public static string ImgUrl
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["ImgUrl"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["ImgUrl"].ToString();
            }
        }
        public static string ImgPath
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["ImgAddress"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["ImgAddress"].ToString();
            }
        }
        public static string ImgType
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["ImgType"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["ImgType"].ToString();
            }
        }
        #endregion
        #region ftp
        public static string FTPPath
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["FTPPath"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["FTPPath"].ToString();
            }
        }
        public static string FTPName
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["FTPName"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["FTPName"].ToString();
            }
        }
        public static string FTPPwd
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["FTPPwd"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["FTPPwd"].ToString();
            }
        }
        #endregion webservice
        #region 邮件
        public static string EmailHost
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["EmailHost"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["EmailHost"].ToString();
            }
        }
        public static string EmailPort
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["EmailPort"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["EmailPort"].ToString();
            }
        }
        public static string EmailName
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["EmailName"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["EmailName"].ToString();
            }
        }
        public static string EmailFrom
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["EmailFrom"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString();
            }
        }
        public static string EmailPwd
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["EmailPwd"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["EmailPwd"].ToString();
            }
        }
        public static string EmailTo
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["ToEmail"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["ToEmail"].ToString();
            }
        }
        #endregion
        #region webservice
        /// <summary>
        /// WebService验证的用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["UserName"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["UserName"].ToString();
            }
        }
        /// <summary>
        /// WebService验证的密码
        /// </summary>
        public static string Password
        {
            get
            {
                if (Config != null)
                {
                    return Config.AppSettings.Settings["Password"].Value.ToString();
                }
                return System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();
            }
        }
        #endregion
        #endregion
    }
}
