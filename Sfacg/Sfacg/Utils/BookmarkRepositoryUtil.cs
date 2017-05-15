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
    class BookmarkRepositoryUtil : AppDatabaseUtil
    {
        public static Bookmark save(Bookmark bookmark)
        {
            BookmarkQO qo = new BookmarkQO();
            qo.chapId = bookmark.chapId;
            qo.novelId = bookmark.novelId;
            qo.progress = bookmark.progress;

            if (getList(qo).Count > 0) return bookmark;

            SQLiteConnection conn = null;
            try
            {
                conn = AppDatabaseUtil.GetDbConnection();
                bookmark = AppDatabaseUtil.saveOne<Bookmark>(bookmark, conn, false);
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
            return bookmark;
        }

        public static List<Bookmark> getList(BookmarkQO qo)
        {
            List<Bookmark> bookmarks;
            TableQuery<Bookmark> tb = AppDatabaseUtil.GetDbConnection().Table<Bookmark>();
            if (!string.IsNullOrEmpty(qo.chapId))
            {
                tb = tb.Where(c => c.chapId.Equals(qo.chapId));
            }
            if (!string.IsNullOrEmpty(qo.novelId))
            {
                tb = tb.Where(c => c.novelId == qo.novelId);
            }
            tb = tb.Where(c => c.progress == qo.progress);

            bookmarks = tb.ToList();
            return bookmarks;
        }
    }
}
