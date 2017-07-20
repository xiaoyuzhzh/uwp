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
	class SearchNovelsApiVO : BaseApiVO
    {
		[DataMember]
		public SearchNovelsApiVOData data { get; set; }

        internal List<Novel> getNovel()
        {
            var novels = new List<Novel>();

            if (this.data != null && this.data.novels!=null && this.data.novels.Count > 0)
            {
                this.data.novels.ForEach(n => {
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
	public class SearchNovelsApiVOData
    {
        [DataMember]
		public List<SearchNovelsApiVONovels> novels { get; set; }
        [DataMember]
		public List<SearchNovelsApiVOComics> comics { get; set; }
        [DataMember]
		public List<SearchNovelsApiVOAlbums> albums { get; set; }
    }

	[DataContract]
	public class SearchNovelsApiVOComicsExpand
    {
        [DataMember]
		public string typeName { get; set; }
        [DataMember]
		public List<string> tags { get; set; }
        [DataMember]
		public string intro { get; set; }
        [DataMember]
		public string authorName { get; set; }
    }

	[DataContract]
	public class SearchNovelsApiVOComics
    {
        [DataMember]
		public string comicId { get; set; }
        [DataMember]
		public string comicName { get; set; }
        [DataMember]
		public string folderName { get; set; }
        [DataMember]
		public string typeId { get; set; }
        [DataMember]
		public string viewTimes { get; set; }
        [DataMember]
		public string point { get; set; }
        [DataMember]
		public string isFinished { get; set; }
        [DataMember]
		public string comicCover { get; set; }
        [DataMember]
		public string latestChapterTitle { get; set; }
        [DataMember]
		public string lastUpdateTime { get; set; }
        [DataMember]
		public string authorId { get; set; }
        [DataMember]
		public SearchNovelsApiVOComicsExpand expand { get; set; }
    }

	[DataContract]
	public class SearchNovelsApiVOAlbumsExpand
    {
        [DataMember]
		public string intro { get; set; }
        [DataMember]
		public string latestchaptitle { get; set; }
        [DataMember]
		public string latestchapintro { get; set; }
        [DataMember]
		public string authorName { get; set; }
    }


	[DataContract]
	public class SearchNovelsApiVOAlbums
    {
        [DataMember]
		public string albumId { get; set; }
        [DataMember]
		public string novelId { get; set; }
        [DataMember]
		public string authorId { get; set; }
        [DataMember]
		public string latestChapterId { get; set; }
        [DataMember]
		public string visitTimes { get; set; }
        [DataMember]
		public string name { get; set; }
        [DataMember]
		public string lastUpdateTime { get; set; }
        [DataMember]
		public string coverBig { get; set; }
        [DataMember]
		public string coverSmall { get; set; }
        [DataMember]
		public string coverMedium { get; set; }
        [DataMember]
		public SearchNovelsApiVOAlbumsExpand expand { get; set; }
    }


	[DataContract]
	public class SearchNovelsApiVONovelsExpand
    {
        [DataMember]
		public string typeName { get; set; }
        [DataMember]
		public List<string> tags { get; set; }
        [DataMember]
		public string intro { get; set; }
    }


	[DataContract]
	public class SearchNovelsApiVONovels
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
		public SearchNovelsApiVONovelsExpand expand { get; set; }
    }


}
