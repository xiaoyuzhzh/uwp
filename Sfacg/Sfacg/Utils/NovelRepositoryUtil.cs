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

            //删除现有缓存
            NovelQO qo = new NovelQO();
            qo.novelId = novel.novelId;
            var novels = getList(qo);
            if (novels != null && novels.Count > 0)
            {
                novels.ForEach(n => NovelRepositoryUtil.deleteOne<Novel>(n));
            }

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

        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="novel"></param>
        public static void updateOne(Novel novel)
        {
            if(novel.id == null)
            {
                return;
            }
            SQLiteConnection conn = null;
            try
            {
                conn = AppDatabaseUtil.GetDbConnection();
                conn.Update(novel);
            }
            finally
            {
                if (conn != null)
                {
                    AppDatabaseUtil.closeConn(conn);
                }
            }
        }

        public static List<Novel> getList(NovelQO qo)
        {
            List<Novel> novels = new List<Novel>();
            var conn = AppDatabaseUtil.GetDbConnection();
            try
            {
                TableQuery<Novel> tb = conn.Table<Novel>();
                if (!string.IsNullOrEmpty(qo.novelId))
                {
                    tb = tb.Where(c => c.novelId.Equals(qo.novelId));
                }
                novels = tb.ToList();
                return novels;
            }
            catch (Exception)
            {
                return novels;
            }
            finally
            {
                if (conn != null)
                {
                    AppDatabaseUtil.closeConn(conn);
                }
            }
        }
    }
}
