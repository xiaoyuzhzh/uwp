using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterReader.Pojo.QO
{
    public class LogMessageQO
    {
        /**
         * 环境id
         */
        public String envId { get; set; }
        /**
        * 工程id
        */
        public String projectId { get; set; }
        /**
         * 日志等级
         */
        public String level { get; set; }
        public String content { get; set; }
        public String title { get; set; }
    }
}
