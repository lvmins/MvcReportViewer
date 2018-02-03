using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPCT_Coin.Models
{
    public class RoleAuthorizeRequest
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}