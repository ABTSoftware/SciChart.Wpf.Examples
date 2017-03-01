using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace SciChart.Examples.Demo.Behaviors
{
    public class SharedResourceDictionary : ResourceDictionary
    {
        private string _relativeSource;

        public string RelativeSource
        {
            get { return _relativeSource; }
            set
            {
                _relativeSource = value;
                base.Source = MakePackUri(System.Reflection.Assembly.GetExecutingAssembly(), value);
            }
        }

#if !SILVERLIGHT
        public static Uri MakePackUri(Assembly assembly, string resource)
        {
            return new Uri(string.Format("pack://application:,,,/{0};component/{1}", GetDllName(assembly), resource));
        }
#else
        public static Uri MakePackUri(Assembly assembly, string resource)
        {
            return new Uri(string.Format("/{0};component/{1}", GetDllName(assembly),
                                    resource), UriKind.RelativeOrAbsolute);
        }
#endif

        private static string GetDllName(Assembly assembly)
        {
            int index = assembly.FullName.IndexOf(",");
            return assembly.FullName.Substring(0, index);
        }

    }
}
