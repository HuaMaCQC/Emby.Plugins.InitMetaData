using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Emby.Plugins.InitMetaData
{
    public class SeasonNameRule
    {
        public static string GetSeasonName(string val)
        {
            string newVal = val.Split('(')[0].Trim(' ');

            if (Regex.IsMatch(newVal, @"^[0-9]{1,2}"))
            {
                string[] v = newVal.Split(' ');
                newVal = "第 " + v[0] + " 季";

                for (int i = 1; i < v.Length; i++)
                {
                    newVal += (" " + v[i]);
                }
            }

            return newVal;
        }

        public static string RemovePrefix(string val)
        {
            string newVal = val;

            if (Regex.IsMatch(newVal, @"^season ", RegexOptions.IgnoreCase))
            {
                newVal = Regex.Replace(newVal, @"^season ", "", RegexOptions.IgnoreCase).Trim(' ');
            }
            else if (Regex.IsMatch(newVal, @"^s ", RegexOptions.IgnoreCase))
            {
                newVal = Regex.Replace(newVal, @"^s ", "", RegexOptions.IgnoreCase);
            }

            return newVal;
        }

        public static string GetSortName(string val)
        {
            return val.Split('(')[0].Trim(' ');
        }
    }
}
