using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model
{
    [DataContract]
    public class NovelJSON
    {
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public Data data { get; set; }
    }

    [DataContract]
    public class Status
    {
        [DataMember]
        public string httpCode { get; set; }
        [DataMember]
        public string errorCode { get; set; }
        [DataMember]
        public string msg { get; set; }
    }
    [DataContract]
    public class Expand
    {
        [DataMember]
        public string needFireMoney { get; set; }
        [DataMember]
        public string content { get; set; }
    }
    [DataContract]
    public class Data
    {
        [DataMember]
        public string chapId { get; set; }
        [DataMember]
        public string novelId { get; set; }
        [DataMember]
        public string volumeId { get; set; }
        [DataMember]
        public string charCount { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string addTime { get; set; }
        [DataMember]
        public string sno { get; set; }
        [DataMember]
        public string isVip { get; set; }
        [DataMember]
        public Expand expand { get; set; }
    }


}
