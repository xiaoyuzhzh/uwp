using Sfacg.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.StoreModel
{
    [Table("BOOKMARK")]
    class Bookmark : BaseModel
    {
        /**
        * 小说id
        */
        public string novelId { get; set; }
        /**
        * 章节id
        */
        public string chapId { get; set; }
        /**
        * 章节名称
        */
        public string chapName { get; set; }
        /**
        * 阅读进度
        */
        public int progress { get; set; }
    }
}
