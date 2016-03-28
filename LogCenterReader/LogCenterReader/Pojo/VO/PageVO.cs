using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterReader.Pojo.VO
{
    public class PageVO<T>
    {
        public List<T> list { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int pageCount { get; set; }
    }
}
