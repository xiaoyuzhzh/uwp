using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.ApiVO
{
    [DataContract]
    class SearchSuggessApiVO :BaseApiVO
    {
        [DataMember]
        public List<string> data { get; set; }
    }
}
