using AutoMapper;
using Sfacg.Model.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model.ApiVO
{
    [DataContract]
    class BaseApiVO
    {
        [DataMember]
        public Status status { get; set; }
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
}
