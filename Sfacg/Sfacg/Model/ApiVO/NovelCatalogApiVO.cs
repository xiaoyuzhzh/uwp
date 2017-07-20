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
    class NovelCatalogApiVO
    {
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public NovelCatalogApiVOData data { get; set; }

        internal List<Volume> getVolumes()
        {
            var volumes = new List<Volume>();

            if (this.data != null && this.data.volumeList!= null && this.data.volumeList.Count > 0)
            {
                this.data.volumeList.ForEach(v => {
                    var volume = new Volume();
                    volume = MapperUtil.map(v, volume);
                    var chapters = new List<Chapter>();
                    if (v.chapterList != null && v.chapterList.Count > 0)
                    {
                        chapters = v.getChapters();
                    }
                    volume.chapters = chapters;
                    volumes.Add(volume);
                });
            }

            return volumes;
        }
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

        /**
         * 列表位置
         */
        public string listPosition { get; set; }
        /**
         * 列表项
         */
        public int itemId { get; set; }
        /**
         * 项高度
         */
        public double itemContainerHeight { get; set; }
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

        internal List<Chapter> getChapters()
        {
            var chapters = new List<Chapter>();
            if (this.chapterList != null && this.chapterList.Count > 0)
            {
                this.chapterList.ForEach(c => {
                    var chapter = new Chapter();
                    chapter = MapperUtil.map(c, chapter);
                    chapters.Add(chapter);
                });
            }
            return chapters;
        }
    }

    [DataContract]
    public class NovelCatalogApiVOData
    {
        [DataMember]
        public string novelId { get; set; }
        [DataMember]
        public string lastUpdateTime { get; set; }
        [DataMember]
        public List<VolumeList> volumeList { get; set; }
    }

}
