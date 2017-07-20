using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sfacg.Model.StoreModel;
using Sfacg.Utils;

namespace Sfacg.Model.ApiVO
{
    [DataContract]
    class PushNovelApiVO
    {
        [DataMember]
        public PushNovelApiVOStatus status { get; set; }
        [DataMember]
        public List<PushNovelApiVOData> data { get; set; }

        internal List<Novel> getNovel()
        {
            var novels = new List<Novel>();

            if (this.data != null && this.data.Count > 0)
            {
                this.data.ForEach(n => {
                    var novel = new Novel();
                    novel = MapperUtil.map(n, novel);
                    novels.Add(novel);
                });
            }

            return novels;
        }
    }

    [DataContract]
    public class PushNovelApiVOStatus
    {
        [DataMember]
        public string httpCode { get; set; }
        [DataMember]
        public string errorCode { get; set; }
        [DataMember]
        public string msg { get; set; }
    }

    [DataContract]
    public class PushNovelApiVOData
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

    }

}
