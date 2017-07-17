using Sfacg.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.StoreModel
{
    [Table("BOOKMARK")]
    [DataContract]
    public class Bookmark : BaseModel
    {
        /**
        * 小说id
        */
        [DataMember]
        public string novelId { get; set; }
        /**
        * 章节id
        */
        [DataMember]
        public string chapId { get; set; }
        /**
        * 章节名称
        */
        [DataMember]
        public string chapName { get; set; }
        /**
        * 阅读进度
        */
        [DataMember]
        public int progress { get; set; }
        /**
         * 列表位置
         */
        [DataMember]
        public string listPosition { get; set; }
        /**
         * 列表项
         */
        [DataMember]
        public int itemId { get; set; }
        /**
         * 项高度
         */
        [DataMember]
        public double itemContainerHeight { get; set; }
    }
}
