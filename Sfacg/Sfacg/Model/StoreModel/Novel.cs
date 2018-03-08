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
    [Table("NOVEL")]
    class Novel : BaseModel
    {
        public string novelId { get; set; }
        public string novelName { get; set; }
        public string novelCover { get; set; }
        [DataMember]
        public string authorId { get; set; }
        [DataMember]
        public string lastUpdateTime { get; set; }
        [DataMember]
        public string markCount { get; set; }
        [DataMember]
        public string bgBanner { get; set; }
        [DataMember]
        public string point { get; set; }
        [DataMember]
        public string isFinish { get; set; }
        [Ignore]
        public string serial { get{

                    try
                    {
                        if (bool.Parse(isFinish))
                        {
                            return "已完结";
                        }
                        else
                        {
                            return "连载中";
                        }
                    }
                    catch (Exception)
                    {
                    return "";
                    }
                }
                set { }
        }
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
        public string intro { get; set; }
        [DataMember]
        public string ticket { get; set; }
        [DataMember]
        public string fav { get; set; }
        [DataMember]
        public string typeName { get; set; }
        [DataMember]
        [Ignore]
        public List<string> tags { get; set; }
        [DataMember]
        public string pointCount { get; set; }
        [DataMember]
        public string signLevel { get; set; }
    }
}
