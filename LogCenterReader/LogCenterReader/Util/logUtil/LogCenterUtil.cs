using LogCenterReader.Model;
using LogCenterReader.Pojo;
using LogCenterReader.Util.httpUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterReader.Util.logUtil
{
    public class LogCenterUtil
    {
        public string envId { get; set; }
        public string projectId { get; set; }

        public LogCenterUtil(string envId,string projectId)
        {
            this.envId = envId;
            this.projectId = projectId;
        }

        public void log(String title ,String content ,String level) {

            var log = new LogMessage();

            log.envId = this.envId;
            log.projectId = this.projectId;
            log.logDate = DateTime.Now;
            log.title = title;
            log.content = content;
            log.level = level;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            var logUrl = Constant.getSetting(Constant.LOGADDURL);
            CHttpClient.postJson(logUrl, JsonConvert.SerializeObject(log, setting));
        }

        public void error(String title, String content)
        {
            try
            {
                log(title, content, "ERROR");
            }
            catch (Exception)
            {

            }
            
        }


        public static string getStackTrace(Exception e)
        {
            var builder = new StringBuilder();
            getStackTraceStr(builder, e);
            return builder.ToString();
        }

        private static void getStackTraceStr(StringBuilder builder,Exception e)
        {
            builder.Append(e.GetType().ToString()).Append("  :").Append(e.Message).Append("\n").Append(e.StackTrace);
            if (e.InnerException != null)
            {
                builder.Append("\n causedBy: \n");
                getStackTraceStr(builder,e.InnerException);
            }
        }
    }
}
