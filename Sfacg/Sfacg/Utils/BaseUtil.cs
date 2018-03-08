using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Sfacg.Utils
{


    class BaseUtil
    {
        public static readonly string READ_POINT_PREFIX = "readPoint";


        private static ApplicationData applicationData = null;
        private static ApplicationDataContainer roamingSettings = null;
        private static ApplicationDataContainer localSettings = null;
        private static bool inited = false;

        static BaseUtil()
        {
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.RoamingSettings;
            localSettings = applicationData.LocalSettings;
        }


        public static T getSetting<T>(string key,bool withJSON)
        {
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

        public static void setLocalSetting<T>(string key, T value, bool withJSON)
        {
            try
            {
                if (value == null)
                {
                    return;
                }
                if (withJSON)
                {
                    var str = JSONUtil.serialize<T>(value);
                    localSettings.Values[key] = str;
                }
                else
                {
                    localSettings.Values[key] = value;
                }
            }
            catch (Exception)
            {
            }
        }

        public static T getLocalSetting<T>(string key, bool withJSON)
        {
            try
            {
                if (withJSON)
                {
                    var str = localSettings.Values[key] as string;
                    if(str == null)
                    {
                        return default(T);
                    }
                    T t = JSONUtil.deSerialize<T>(str);
                    return t;
                }
                else
                {
                    return (T)(localSettings.Values[key] as Object);
                }
            }
            catch (Exception)
            {
            }

            return default(T);
        }

        public static void deleteLocalSetting(string key)
        {
            localSettings.Values[key] = null;
        }


        public static T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);
            var children = new FrameworkElement[childrenCount];

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                children[i] = child;
                if (child is T)
                    return (T)child;
            }

            for (int i = 0; i < childrenCount; i++)
                if (children[i] != null)
                {
                    var subChild = FindFirstChild<T>(children[i]);
                    if (subChild != null)
                        return subChild;
                }

            return null;
        }

    }
}
