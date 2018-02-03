using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPCT_Coin.Models
{
    public class RoleRequest
    {
        public string ID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string RoleAuthorize { get; set; }
    }
}