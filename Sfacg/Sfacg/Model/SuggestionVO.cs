using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model
{
    [DataContract]
    public class SuggestionVO
    {
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public List<string> data { get; set; }
    }
}
