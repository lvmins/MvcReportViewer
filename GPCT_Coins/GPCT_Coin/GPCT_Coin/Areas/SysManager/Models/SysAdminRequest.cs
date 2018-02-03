using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPCT_Coin.Models
{
    public class SysAdminRequest
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string Area { get; set; }
        public string WebChatAccount { get; set; }
        public string LoginAccount { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool State { get; set; }
    }
}