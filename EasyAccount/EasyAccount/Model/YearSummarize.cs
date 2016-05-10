using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model
{
    class YearSummarize
    {
        public int year { get; set; }

        public List<MonthSummarize> months { get; set; }
    }
}
