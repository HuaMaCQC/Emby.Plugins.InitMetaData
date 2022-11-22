using System;
using System.Collections.Generic;
using System.Text;

namespace Emby.Plugins.InitMetaData
{

    public class INameRule
    {
        public string Regular { get; }
        public Func<string, string> Replace { get; }

        public INameRule(string _regular, Func<string, string> _Replace)
        {
            Regular = _regular;
            Replace = _Replace;
        }
    }

    public class NameRule
    {
        public static string BasicReplace(string str)
        {
            return str.Replace("[", "第").Replace("]", "集");
        }

        public static string Replace01(string str)
        {
            return str.Replace("(", "第").Replace(")", "集");
        }

        public static string Replace02(string str)
        {
            return str.Replace("話", "集");
        }

        public static string Replace04(string str)
        {
            return str.Replace("[", "").Replace("]", "");
        }

        public static string Replace05(string str)
        {
            return str.Replace(" -", "第").Replace(" ", "集");
        }
        public static string Replace06(string str)
        {
            return str.Replace(" - ", "第").Replace(" ", "集");
        }

        public static string NoReplace(string str)
        {
            return str;
        }

        // 會依序檢查 所以規則越嚴格 請放越上面
        public static INameRule[] Rule = 
        {
            new INameRule(@"\[[0-9][0-9]\]", BasicReplace),
            new INameRule(@"\[[0-9][0-9][0-9]\]", BasicReplace),
            new INameRule(@"\([0-9][0-9]\)", Replace01),
            new INameRule(@"\([0-9][0-9][0-9]\)", Replace01),
            new INameRule(@"\[[0-9][0-9]_[A-Za-z]+\]", BasicReplace),
            new INameRule(@"\[[0-9][0-9]-[A-Za-z]+\]", BasicReplace),

            new INameRule(@"\[[0-9][0-9]_[A-Za-z0-9]+\]", BasicReplace),
            new INameRule(@"\[[0-9][0-9]-[A-Za-z0-9]+\]", BasicReplace),
            new INameRule(@"\[[0-9][0-9] [A-Za-z0-9]+\]", BasicReplace),

            new INameRule(@"第[0-9][0-9][0-9]話", Replace02),
            new INameRule(@"第[0-9][0-9]話", Replace02),
            new INameRule(@"第[0-9][0-9][0-9]集", NoReplace),
            new INameRule(@"第[0-9][0-9]集", NoReplace),

            new INameRule(@"\[SP\]", Replace04),
            new INameRule(@"\[SP [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[SP_[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[SP-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[SP[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[OVA\]", Replace04),
            new INameRule(@"\[OVA [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[OVA_[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[OVA-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[OVA[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[OAD\]", Replace04),
            new INameRule(@"\[OAD [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[OAD_[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[OAD-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[OAD[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[Menu\]", Replace04),
            new INameRule(@"\[Menu [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[Menu[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[Menu-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[Menu[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[NCOP\]", Replace04),
            new INameRule(@"\[NCOP [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[NCOP[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[NCOP-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[NCOP[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[NCED\]", Replace04),
            new INameRule(@"\[NCED [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[NCED[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[NCED-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[NCED[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[PV\]", Replace04),
            new INameRule(@"\[PV [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[PV[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[PV-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[PV[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[CM\]", Replace04),
            new INameRule(@"\[CM [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[CM[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[CM-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[CM[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[VOL\]", Replace04),
            new INameRule(@"\[VOL [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[VOL[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[VOL-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[VOL[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[VOL\.[A-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[Information\]", Replace04),
            new INameRule(@"\[Information [A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[Information[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[Information-[A-Za-z0-9]+\]", Replace04),
            new INameRule(@"\[InformationA-Za-z0-9]+\]", Replace04),

            new INameRule(@"\[[0-9]\]", BasicReplace),
            new INameRule(@" -[0-9][0-9] ", Replace05),
            new INameRule(@" - [0-9][0-9] ", Replace06),
            new INameRule(@" -[0-9] ", Replace05),
            new INameRule(@" - [0-9] ", Replace06),
        };
    }
}
