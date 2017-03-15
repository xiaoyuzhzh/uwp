using EasyAccount.Model;
using EasyAccount.Model.Base;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EasyAccount.Services
{
    public static class AppDatabase
    {
        private static String DB_NAME = "Account.db";
        private static string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DB_NAME);
        private static String TABLE_NAME = "SampleTable";
        private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS (?) (Key TEXT,Value TEXT);";
        private static String SQL_QUERY_VALUE = "SELECT Value FROM " + TABLE_NAME + " WHERE Key = (?);";
        private static String SQL_INSERT = "INSERT INTO " + TABLE_NAME + " VALUES(?,?);";
        private static String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET Value = ? WHERE Key = ?";
        private static String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE Key = ?";


        /// <summary>
        /// 数据库文件所在路径，这里使用 LocalFolder，数据库文件名叫 test.db。
        /// </summary>
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "account.db");
        //public readonly static string DbPath = Path.Combine("d:\\uwpdb", "account.db");



        public static SQLiteConnection GetDbConnection()
        {
            // 连接数据库，如果数据库文件不存在则创建一个空数据库。
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            
            return conn;
        }

        public static void initTable()
        {
            var conn = GetDbConnection();
            createTableTransaction(conn);
            createTableCategory(conn);
            closeConn(conn);
        }

        private static void createTableCategory(SQLiteConnection conn)
        {
            conn.CreateTable<Category>();
        }

        private static void createTableTransaction(SQLiteConnection conn)
        {
            conn.CreateTable<Transaction>();
        }

        public static void closeConn(SQLiteConnection conn)
        {
            conn.Close();
        }

        public static T saveOne<T>(T t, SQLiteConnection conn,bool re) where T:BaseModel
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

        public static void saveTransaction(Transaction transaction)
        {
            try
            {
                var conn = GetDbConnection();
                conn.Insert(transaction);
                closeConn(conn);
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }

        }

        public static void deleteTransaction(Transaction transaction)
        {
            try
            {
                var conn = GetDbConnection();
                conn.Delete<Transaction>(transaction.id);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public static List<Transaction> getAllTransaction()
        {
            try
            {
                var conn = GetDbConnection();
                var list = conn.Table<Transaction>().ToList() ;
            
                closeConn(conn);
                return list;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return null;
             }
        }
    }
}
