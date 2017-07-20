using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Sfacg.Model.StoreModel;
using AutoMapper;
using Sfacg.Utils;

namespace Sfacg.Model.ApiVO
{
    [DataContract]
    class JPNovelApiVO : BaseApiVO
    {


        [DataMember]
        public List<JPNovelApiVOData> data { get; set; }

        internal List<Novel> getNovel()
        {
            var novels = new List<Novel>();

            if (this.data != null && this.data.Count > 0)
            {
                this.data.ForEach(n => {
                    var novel = new Novel();
                    novel = MapperUtil.map(n, novel);
                    novel = MapperUtil.map(n.expand, novel);
                    novels.Add(novel);
                });
            }

            return novels;
        }
    }

    [DataContract]
    public class JPNovelApiVOData
    {
        [DataMember]
        public string novelId { get; set; }
        [DataMember]
        public string novelName { get; set; }
        [DataMember]
        public string novelCover { get; set; }
        [DataMember]
        public string typeId { get; set; }
        [DataMember]
        public string authorName { get; set; }
        [DataMember]
        public string signStatus { get; set; }
        [DataMember]
        public string point { get; set; }
        [DataMember]
        public JPNovelApiVOExpand expand { get; set; }
    }

    [DataContract]
    public class JPNovelApiVOExpand
    {
        [DataMember]
        public List<string> tags { get; set; }
        [DataMember]
        public string typeName { get; set; }
    }
}
