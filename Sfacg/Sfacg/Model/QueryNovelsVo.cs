using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model
{
    [DataContract]
    class QueryNovelsVo
    {
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public List<Novels> Novels { get; set; }
    }

    [DataContract]
    public class Novels
    {
        [DataMember]
        public string NovelName { get; set; }
        [DataMember]
        public string AuthorName { get; set; }
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public string Point { get; set; }
        [DataMember]
        public string NovelCover { get; set; }
        [DataMember]
        public string NovelID { get; set; }
        [DataMember]
        public string Tags { get; set; }
    }

}
