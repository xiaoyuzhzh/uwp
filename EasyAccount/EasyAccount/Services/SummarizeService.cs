using EasyAccount.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Services
{
    class SummarizeService
    {

        public void getWeeklySummarize(ObservableCollection<DailySummarize> dailySummarizes)
        {
            if(dailySummarizes == null)
            {
                return;
            }

            for (var i = 0; i < 30; i++)
            {
                DailySummarize summarize = new DailySummarize();
                summarize.income = 10;
                summarize.payout = 20;
                summarize.day = 1;
                summarize.weekName = "Monday";
                dailySummarizes.Add(summarize);
            }
        }
    }
}
