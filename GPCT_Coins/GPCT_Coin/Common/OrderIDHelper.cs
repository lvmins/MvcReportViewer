using System;
namespace Common
{
    public class OrderIDHelper
    {
        public static object _lock = new object();
        public string GetRandomOrderNumber(int userID)
         {
             lock(_lock)
             {
                 Random ran = new Random();
                 return userID+ DateTime.Now.ToString("yyyyMMddHHmmss") + ran.Next(1000, 9999).ToString();
             }
         }
    }
}
