using LogCenterReader.Model;
using LogCenterReader.Pojo;
using LogCenterReader.Pojo.QO;
using LogCenterReader.Pojo.VO;
using LogCenterReader.Util.httpUtil;
using LogCenterReader.Util.logUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogCenterReader.Service
{
    public class LogFinder
    {
        private static LogCenterUtil logger =  new LogCenterUtil("dev","logCenterReaderApp");

        public async static Task<PageVO<LogMessage>> getLogByPage(int pageNo,int pageSize, LogMessageQO qo)
        {
            var urltemplate = Constant.getSetting(Constant.LOGQUERYURL);
            var url = String.Format(urltemplate, pageNo, pageSize);
            var result = await CHttpClient.postJson(url,JsonConvert.SerializeObject(qo));
            try
            {
                return JsonConvert.DeserializeObject<PageVO<LogMessage>>(result);
            }
            catch (Exception ex)
            {
                logger.error("查询日志出错", LogCenterUtil.getStackTrace(ex));
                return null;
            }
        }
    }
}
