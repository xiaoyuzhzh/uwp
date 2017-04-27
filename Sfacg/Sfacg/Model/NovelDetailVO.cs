using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model
{
    [DataContract]
    class NovelDetailVO
    {
        [DataMember]
        public string NovelName { get; set; }
        [DataMember]
        public string AuthorName { get; set; }
        [DataMember]
        public string AuthorAvatar { get; set; }
        [DataMember]
        public string TypeName { get; set; }
		[DataMember]
        public string Point { get; set; }
		[DataMember]
        public string NovelCover { get; set; }
		[DataMember]
        public string NovelID { get; set; }
		[DataMember]
        public string CharCount { get; set; }
		[DataMember]
        public string Serial { get; set; }
		[DataMember]
        public string ViewTimes { get; set; }
		[DataMember]
        public string MarkCount { get; set; }
		[DataMember]
        public string FavSticks { get; set; }
		[DataMember]
        public string MonthTicket { get; set; }
		[DataMember]
        public string AuthorID { get; set; }
		[DataMember]
        public string Intro { get; set; }
		[DataMember]
        public string AuthorIntro { get; set; }
		[DataMember]
        public string LastUpdateTime { get; set; }
		[DataMember]
        public List<AuthorBooks> AuthorBooks { get; set; }
		[DataMember]
        public List<RecommendBooks> RecommendBooks { get; set; }
		[DataMember]
        public string Status { get; set; }
		[DataMember]
        public string SignStatus { get; set; }
		[DataMember]
        public string HasbeenMarked { get; set; }
		[DataMember]
        public List<string> Tags { get; set; }
		[DataMember]
        public AlbumInfo AlbumInfo { get; set; }
    }

    [DataContract]
    public class AuthorBooks
    {
        [DataMember]
        public string NovelID { get; set; }
		[DataMember]
        public string NovelName { get; set; }
		[DataMember]
        public string NovelCover { get; set; }
    }

    [DataContract]
    public class RecommendBooks
    {
        [DataMember]
        public string NovelID { get; set; }
		[DataMember]
        public string NovelName { get; set; }
		[DataMember]
        public string NovelCover { get; set; }
    }

    [DataContract]
    public class AlbumInfo
    {
        [DataMember]
        public string AlbumID { get; set; }
        [DataMember]
        public string Count { get; set; }
		[DataMember]
        public string LastUpdateTime { get; set; }
    }

}
