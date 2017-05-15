using Sfacg.Model.QueryModel;
using Sfacg.Model.StoreModel;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Utils
{
    class NovelRepositoryUtil : AppDatabaseUtil
    {
        public static Novel save(Novel novel)
        {
            NovelQO qo = new NovelQO();
            qo.novelId = novel.novelId;

            if (getList(qo).Count > 0) return novel;

            SQLiteConnection conn = null;
            try
            {
                conn = AppDatabaseUtil.GetDbConnection();
                novel = AppDatabaseUtil.saveOne<Novel>(novel, conn, false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    AppDatabaseUtil.closeConn(conn);
                }
            }
            return novel;
        }

        public static List<Novel> getList(NovelQO qo)
        {
            List<Novel> novels;
            TableQuery<Novel> tb = AppDatabaseUtil.GetDbConnection().Table<Novel>();
            if (!string.IsNullOrEmpty(qo.novelId))
            {
                tb = tb.Where(c => c.novelId.Equals(qo.novelId));
            }
            novels = tb.ToList();
            return novels;
        }
    }
}
