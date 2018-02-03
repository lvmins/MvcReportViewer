using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{  
	public partial class Coin_DeliveryAddress
	{
          public int ID { get; set; }// 
          public string DeliveryName { get; set; }// 
          public string Phone { get; set; }// 
          public int CustomerID { get; set; }// 
          public string Province { get; set; }// 
          public string City { get; set; }// 
          public string County { get; set; }// 
          public string Street { get; set; }// 
          public string AddressDetails { get; set; }// 
          public bool IsDefault { get; set; }// 
          	}
} 
