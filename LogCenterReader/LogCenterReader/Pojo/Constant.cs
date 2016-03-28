using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LogCenterReader.Pojo
{
    class Constant
    {
        public const string LOGQUERYURL = "LOGQUERYURL";

        public const string LOGADDURL = "LOGADDURL";

        public static ApplicationDataContainer roamingSettings = null;

        static Constant() {
            roamingSettings = ApplicationData.Current.RoamingSettings;

            roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values[Constant.LOGQUERYURL] == null)
            {
                roamingSettings.Values[Constant.LOGQUERYURL] = "http://192.168.181.60:8080/log/querylogsbypage/{0}/{1}";
            }
            if (roamingSettings.Values[Constant.LOGADDURL] == null)
            {
                roamingSettings.Values[Constant.LOGADDURL] = "http://192.168.181.60:8080/log/addlog";
            }
        }

        public static string getSetting(string settingName) {
            return (string)roamingSettings.Values[settingName];
        }
    }
}
