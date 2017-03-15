using EasyAccount.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model
{
    [Table("TRANSACTION")]
    public class Transaction : BaseModel
    {
        /**
        * 一级分类
        */
        public String classOne { get; set; }
        /**
        * 金额
        */
        public Decimal amount { get; set; }
        /**
        * 二级分类
        */
        public String classTwo { get; set; }
        /**
        * 三级分类
        */
        public String classThree { get; set; }
        /**
        * 交易时间
        */
        public DateTime tradeTime { get; set; }
        /**
        * 备注
        */
        [MaxLength(255)]
        public String remark { get; set; }
    }
}
