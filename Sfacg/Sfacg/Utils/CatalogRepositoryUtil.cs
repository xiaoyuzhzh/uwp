using Sfacg.Model.StoreModel;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Utils
{
    class CatalogRepositoryUtil : AppDatabaseUtil
    {
        public static Catalog saveOrUpdate(Catalog catalog)
        {
            var log = getCatalog(catalog.novelId);


            SQLiteConnection conn = null;
            try
            {
                conn = AppDatabaseUtil.GetDbConnection();
                if (log != null)
                {
                    catalog.id = log.id;
                    conn.Update(catalog);
                }
                else
                {
                    catalog = AppDatabaseUtil.saveOne<Catalog>(catalog, conn, false);
                }
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
            return catalog;
        }

        public static Catalog getCatalog(string novelId)
        {
            List<Catalog> catalogs;
            TableQuery<Catalog> tb = AppDatabaseUtil.GetDbConnection().Table<Catalog>();
            tb = tb.Where(c => c.novelId.Equals(novelId));
            catalogs = tb.ToList();
            if (catalogs.Count > 0)
            {
                return catalogs.First();
            }
            return null;
        }
    }
}
