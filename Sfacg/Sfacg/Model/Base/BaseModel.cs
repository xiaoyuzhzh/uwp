using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.Base
{
    public class BaseModel
    {
        [PrimaryKey]
        public string id { get; set; }
        /**
        * 创建时间
        */
        public DateTime createTime { get; set; } = DateTime.Now;
        /**
        * 更新时间
        */
        public DateTime updateTime { get; set; } = DateTime.Now;
    }
}
