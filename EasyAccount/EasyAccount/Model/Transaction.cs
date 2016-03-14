using EasyAccount.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model
{
    public class Transaction : BaseModel
    {

        public String classOne { get; set; }
        public Decimal amount { get; set; }
        public String classTwo { get; set; }
        public String classThree { get; set; }
        public DateTime createTime { get; set; }
        public DateTime tradeTime { get; set; }
    }
}
