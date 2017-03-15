using EasyAccount.Model;
using EasyAccount.Pojo;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Services.Respository
{
    /**
     * 流水资源库 
     */
    class TransactionRepository
    {
        private static TransactionRepository instance = new TransactionRepository();

        private TransactionRepository()
        {

        }

        public static TransactionRepository getIntance()
        {
            return instance;
        }


        /**
         * 查询列表 
         */
        public static List<Transaction> getList(TransactionQO qo)
        {
            TableQuery<Transaction> tb = AppDatabase.GetDbConnection().Table<Transaction>();

            return tb.ToList();
        }
    }
}
