using Sfacg.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
