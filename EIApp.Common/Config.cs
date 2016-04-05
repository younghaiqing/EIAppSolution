using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Common
{
    public abstract class Config
    {
        public static Hashtable DBConnString
        {
            get
            {
                Hashtable ht = new Hashtable();
                ht.Add("EIConString", @"Data Source=DO3FIT0920\SQLEXPRESS;Initial Catalog=EIDB;User Id=sa;Password=654321;");
                return ht;
            }
        }
    }
}