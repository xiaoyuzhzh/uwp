using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Model
{
    class NovelContentVO
    {
        public int id { get; set; }
        /**
         * 小说段落
         */
        public string paragraph { get; set; }
        /**
         * 图片
         */
        public string imageUrl { get; set; }
    }
}
