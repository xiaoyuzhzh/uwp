using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model
{
    /**
    * 月份统计对象
    */
    class MonthSummarize
    {
        /**
        * 月份名称
        */
        public string monthName { get; set; }
        /**
        * 每日统计列表
        */
        public List<DailySummarize> dailys { get; set; }
        /**
        * 月份值
        */
        public int month { get; set; }

        public int year { get; set; }
    }
}
