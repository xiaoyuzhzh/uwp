using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sfacg.Utils
{


    class BaseUtil
    {
        public static readonly string READ_POINT_PREFIX = "readPoint";


        private static ApplicationData applicationData = null;
        private static ApplicationDataContainer roamingSettings = null;
        private static bool inited = false;


        public static T getSetting<T>(string key,bool withJSON)
        {
            if (!inited)
            {
                init();
            }
            try
            {
                if (withJSON)
                {
                    var str = roamingSettings.Values[key] as string;
                    T t = JSONUtil.deSerialize<T>(str);
                    return t;
                }
                else
                {
                    return (T)(roamingSettings.Values[key] as Object);
                }
            }
            catch (Exception)
            {
            }

            return default(T);
        }

        public static void setSetting<T>(string key ,T value, bool withJSON)
        {
            if (!inited)
            {
                init();
            }
            try
            {
                if(value == null)
                {
                    return;
                }
                if (withJSON)
                {
                    var str = JSONUtil.serialize<T>(value);
                    roamingSettings.Values[key] = str;
                }
                else
                {
                    roamingSettings.Values[key] = value;
                }
            }
            catch (Exception)
            {
            }
            
        }

        private static void init()
        {
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.RoamingSettings;
        }
    }
}
