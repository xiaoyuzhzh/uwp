using Sfacg.Model.Base;
using Sfacg.Model.StoreModel;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sfacg.Utils
{
    class AppDatabaseUtil
    {
        /// <summary>
        /// 数据库文件所在路径，这里使用 LocalFolder，数据库文件名叫 test.db。
        /// </summary>
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sfacg.db");


        public static SQLiteConnection GetDbConnection()
        {
            // 连接数据库，如果数据库文件不存在则创建一个空数据库。
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);

            return conn;
        }

        public static void closeConn(SQLiteConnection conn)
        {
            conn.Close();
        }

        public static T saveOne<T>(T t, SQLiteConnection conn, bool re) where T : BaseModel
        {
            if (t.id == null)
            {
                t.id = Guid.NewGuid().ToString();
            }
            conn.Insert(t);
            if (re)
            {
                t = conn.Get<T>(t.id);
            }
            return t;
        }

        public static int deleteOne<T>(T t) where T : BaseModel
        {
            SQLiteConnection conn = GetDbConnection();
            try
            {
                return conn.Delete(t);
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (conn != null)
                {
                    closeConn(conn);
                }
            }
        }




        public static void initTable()
        {
            var conn = GetDbConnection();
            conn.CreateTable<Bookmark>();
            conn.CreateTable<Novel>();
            closeConn(conn);
        }

    }
}
