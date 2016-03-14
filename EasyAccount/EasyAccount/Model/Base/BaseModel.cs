using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model.Base
{
     public class BaseModel
    {
        [PrimaryKey]
        public string id { get; set; }
    }
}
