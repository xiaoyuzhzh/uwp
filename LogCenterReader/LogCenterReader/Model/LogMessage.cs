using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterReader.Model
{
    public class LogMessage
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
        /**
         * 日志标题
         */
        public String title { get; set; }
        /**
         * 日志内容
         */
        public String content { get; set; }
        /**
         * 日志打印时间
         */
        public DateTime logDate { get; set; }

        public string logheader {
            get {
                var builder = new StringBuilder();
                if (envId != null)
                {
                    builder.Append("[").Append(envId).Append("]");
                }
                if (projectId != null)
                {
                    builder.Append("[").Append(projectId).Append("]");
                }
                if (logDate != null)
                {
                    builder.Append("[").Append(logDate).Append("]");
                }
                if (level != null)
                {
                    builder.Append("[").Append(level).Append("]");
                }
                if (title != null)
                {
                    builder.Append("[").Append(title).Append("]");
                }
                
                return builder.ToString();
            }
        }

    }
}
