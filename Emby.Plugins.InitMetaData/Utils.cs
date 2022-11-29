using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Emby.Plugins.InitMetaData
{
    public static class Utils
    {
        public static string[] GetTag(string val)
        {
            Match tagMatch = Regex.Match(val, @"\(.*\)");
            List<string> tag = new List<string>();

            if (tagMatch.Success)
            {
                string[] _t = tagMatch.Value.Replace("(", " ").Replace(")", "").Split(' ');

                for (int i = 0; i < _t.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_t[i]))
                    {
                        tag.Add(_t[i].Trim(' '));
                    }
                }
            }

            return tag.ToArray();
        }
    }
}
