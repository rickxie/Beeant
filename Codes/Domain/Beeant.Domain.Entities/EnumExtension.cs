using System;
using System.Collections.Generic;
using System.Text;
using Component.Extension;
using Winner;
using Winner.Dislan;

namespace Beeant.Domain.Entities
{
    public static class EnumExtension
    {
        public static string GetName(this Enum type)
        {
            return
              Creator.Get<ILanguage>()
                           .GetName(type.GetType().FullName, type.ToString());
        }
        public static IList<LanguageInfo> GetNames<T>()
        {
            return
               Creator.Get<ILanguage>()
                            .GetNames(typeof(T).FullName);
        }
        public static IList<LanguageInfo> GetNames(string name)
        {
            return
               Creator.Get<ILanguage>()
                            .GetNames(name);
        }
        public static string BuildeName<T>(this T[] types)
        {
            if (types == null || types.Length == 0) 
                return null;
            var builder = new StringBuilder();
            foreach (var type in types)
            {
                builder.AppendFormat("{0},", type.Convert<Enum>().GetName());
            }
            if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        
      
      
    }
}
