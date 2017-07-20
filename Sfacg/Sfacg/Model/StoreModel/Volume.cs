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
    [Table("VOLUME")]
    [DataContract]
    class Volume : BaseModel
    {
        [DataMember]
        public string volumeId { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string sno { get; set; }
        [DataMember]
        [Ignore]
        public List<Chapter> chapters { get; set; }

    }

    [Table("CHAPTER")]
    [DataContract]
    public class Chapter : BaseModel
    {
        [DataMember]
        public string chapId { get; set; }
        [DataMember]
        public string novelId { get; set; }
        [DataMember]
        public string volumeId { get; set; }
        [DataMember]
        public string needFireMoney { get; set; }
        [DataMember]
        public string charCount { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string content { get; set; }
        [DataMember]
        public string sno { get; set; }
        [DataMember]
        public string isVip { get; set; }
        [DataMember]
        public string AddTime { get; set; }

        /**
         * 列表位置
         */
        [Ignore]
        public string listPosition { get; set; }
        /**
         * 列表项
         */
        [Ignore]
        public int itemId { get; set; }
        /**
         * 项高度
         */
        [Ignore]
        public double itemContainerHeight { get; set; }
    }
}
