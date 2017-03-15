using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Pojo
{
    class CategoryQO
    {
        public string id { get; set; }

        public string parentId { get; set; }

        public int level { get; set; }

        public string name { get; set; }

        public void clean()
        {
            this.parentId = null;
            this.level = 0;
            this.id = null;
            this.name = null;
        }
    }
}
