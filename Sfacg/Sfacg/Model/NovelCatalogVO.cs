using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model
{
    [DataContract]
    class NovelCatalogVO
    {
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public NovelCatalogVOData data { get; set; }
    }

    [DataContract]
    public class ChapterList
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
    }

    [DataContract]
    public class VolumeList
    {
        [DataMember]
        public string volumeId { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string sno { get; set; }
        [DataMember]
        public List<ChapterList> chapterList { get; set; }
    }

    [DataContract]
    public class NovelCatalogVOData
    {
        [DataMember]
        public string novelId { get; set; }
        [DataMember]
        public string lastUpdateTime { get; set; }
        [DataMember]
        public List<VolumeList> volumeList { get; set; }
    }

}
