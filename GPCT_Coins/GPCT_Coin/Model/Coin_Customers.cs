using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{  
	public partial class Coin_Customers
	{
          public int ID { get; set; }//客户ID
          public string UserName { get; set; }//登陆名
          public string PhoneNumber { get; set; }//手机号
          public string Email { get; set; }//邮箱
          public string CustomerName { get; set; }//客户名字
          public int Sex { get; set; }// 
          public string Province { get; set; }//省
          public string City { get; set; }//市
          public string County { get; set; }//县/区
          public string AddressDetails { get; set; }//街道地址
          public int PersonalCoins { get; set; }//个人积分 订单购买积分
          public DateTime CreateDate { get; set; }// 
          public DateTime LastLoginDate { get; set; }//最后一次登录时间
          public bool Valid { get; set; }//是否有效
          	}
} 
