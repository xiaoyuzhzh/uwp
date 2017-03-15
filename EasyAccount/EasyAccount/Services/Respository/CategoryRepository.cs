using EasyAccount.Model;
using EasyAccount.Pojo;
using EasyAccount.Services;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Services.Respository
{
    /**
     * 分类资源库
     */
    class CategoryRepository
    {

        private static CategoryRepository instance = new CategoryRepository();

        private CategoryRepository() {

        }

        public static CategoryRepository getInstance()
        {
            return instance;
        }

        public static void initData()
        {
            SQLiteConnection conn = null;
            try
            {
                conn = AppDatabase.GetDbConnection();
                List<Category> categories = new List<Category>();


                CategoryQO qo = new CategoryQO();
                qo.parentId = "0";
                if(getList(qo).Count == 0)
                {
                    Category c1 = new Category { id = "1", name = "收入", value = "income", level = 1, parentId = "0" }; categories.Add(c1);
                    Category c2 = new Category { id = "2", name = "支出", value = "outlay", level = 1, parentId = "0" }; categories.Add(c2);
                }

                qo.parentId = "1";
                if(getList(qo).Count == 0){
                    Category c11 = new Category { name = "工资", value = "salary", level = 2, parentId = "1" }; categories.Add(c11);
                }

                qo.parentId = "2";
                if (getList(qo).Count == 0)
                {
                    Category c21 = new Category { name = "吃饭", value = "eat", level = 2, parentId = "2" }; categories.Add(c21);
                }
                


                batchSave(categories);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    AppDatabase.closeConn(conn);
                }
            }
        }

        public static List<Category> getList(CategoryQO qo)
        {
            List<Category> categories;
            TableQuery<Category> tb = AppDatabase.GetDbConnection().Table<Category>();
            if (!string.IsNullOrEmpty(qo.parentId))
            {
                tb = tb.Where(c => c.parentId.Equals(qo.parentId));
            }
            if (qo.level != 0)
            {
                tb = tb.Where(c => c.level == qo.level);
            }
            if (!string.IsNullOrEmpty(qo.id))
            {
                tb = tb.Where(c => c.id.Equals(qo.id));
            }

            categories = tb.ToList();
            return categories;
        }

        public static Category save(Category category)
        {
            CategoryQO qo = new CategoryQO();
            qo.level = category.level;
            qo.name = category.name;

            if(getList(qo).Count>0) throw new Exception("分类已经存在");

            SQLiteConnection conn = null;
            try
            {
                conn = AppDatabase.GetDbConnection();
                category = AppDatabase.saveOne<Category>(category, conn, false);
            }
            catch (Exception)
            {
                throw;
            }
            finally {
                if (conn != null)
                {
                    AppDatabase.closeConn(conn);
                }
            }
            return category;
        }

        
        private static Category saveOne(Category category, SQLiteConnection conn,bool re)
        {
            if (category.id == null)
            {
                category.id = Guid.NewGuid().ToString();
            }
            conn.Insert(category);
            if (re)
            {
                category = conn.Get<Category>(category.id);
            }            
            return category;
        }

        public static int batchSave(List<Category> categories)
        {
            SQLiteConnection conn = null;
            int saved = 0;
            try
            {
                conn = AppDatabase.GetDbConnection();
                categories.ForEach(c=> {
                    AppDatabase.saveOne<Category>(c,conn,false);
                    saved++;
                });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (conn != null)
                {
                    AppDatabase.closeConn(conn);
                }
            }
            return 0;
        }
    }
}
