using EasyAccount.Model.Base;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAccount.Model
{
    /**
     * 分类
     */
    [Table("CATEGORY")]
    class Category : BaseModel
    {
        /**
        * 分类名称
        */
        public string name { get; set; }
        /**
         * 分类值
         */
        public string value { get; set; }
        /**
         * 分类级别
         */
        public int level { get; set; }
        /**
         * 父分类
         */
        public string parentId { get; set; }
    }
}
