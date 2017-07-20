using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sfacg.Model.StoreModel;
using Sfacg.Model.InAppVO;
using AutoMapper;
using Sfacg.Utils;

namespace Sfacg.Model.ApiVO
{

    [DataContract]
    class NovelDetailApiVO : BaseApiVO
    {

        [DataMember]
        public Data data { get; set; }

        internal Novel getNovel()
        {
            var novel = new Novel();
            novel = MapperUtil.map(this.data,novel);
            novel = MapperUtil.map(this.data.expand, novel);
            return novel;
        }
    }

    [DataContract]
    public class Expand
    {
        [DataMember]
        public string intro { get; set; }
        [DataMember]
        public string ticket { get; set; }
        [DataMember]
        public string fav { get; set; }
        [DataMember]
        public string typeName { get; set; }
        [DataMember]
        public List<string> tags { get; set; }
        [DataMember]
        public List<SysTags> sysTags { get; set; }
        [DataMember]
        public string pointCount { get; set; }
        [DataMember]
        public string signLevel { get; set; }
    }

    [DataContract]
    public class Data
    {
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        public string lastUpdateTime { get; set; }
        [DataMember]
        public string markCount { get; set; }
        [DataMember]
        public string novelCover { get; set; }
        [DataMember]
        public string bgBanner { get; set; }
        [DataMember]
        public string novelId { get; set; }
        [DataMember]
        public string novelName { get; set; }
        [DataMember]
        public string point { get; set; }
        [DataMember]
        public string isFinish { get; set; }
        [DataMember]
        public string authorName { get; set; }
        [DataMember]
        public string charCount { get; set; }
        [DataMember]
        public string viewTimes { get; set; }
        [DataMember]
        public string typeId { get; set; }
        [DataMember]
        public string allowDown { get; set; }
        [DataMember]
        public string signStatus { get; set; }
        [DataMember]
        public Expand expand { get; set; }
    }

    [DataContract]
    public class SysTags
    {
        [DataMember]
        public string sysTagId { get; set; }
        [DataMember]
        public string tagName { get; set; }
    }

}
