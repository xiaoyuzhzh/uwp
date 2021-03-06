﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.InAppVO
{
    class NovelVO
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
