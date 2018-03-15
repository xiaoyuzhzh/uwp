using Sfacg.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.StoreModel
{
    /// <summary>
    /// 章节保管类
    /// </summary>
    [Table("NOVEL")]
    class Catalog : BaseModel
    {
        /// <summary>
        /// 小说id
        /// </summary>
        public string novelId { get; set; }
        /// <summary>
        /// 序列化的章节字段
        /// </summary>
        public string volumesStr { get; set; }
    }
}
