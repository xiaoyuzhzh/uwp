using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model
{
    class DailySummarize
    {
        public int day { get; set; }

        public int month { get; set; }

        public int year { get; set; }

        public string weekName { get; set; }

        public int income { get; set; }

        public int payout { get; set; }
        
    }
}
