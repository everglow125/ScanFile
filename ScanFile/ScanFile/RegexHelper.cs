using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScanFile
{
    public static class RegexHelper
    {
        private static bool MatchNoUnitLength(string source, ref double[] result)
        {
            Regex reg = new Regex(ConfigurationManager.AppSettings["noUnitReg"]);
            var matchs = reg.Matches(source);
            if (matchs == null || matchs.Count == 0)
                return false;
            foreach (Match match in matchs)
            {
                GroupCollection gc = match.Groups;
                result[0] = Convert.ToDouble(gc["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(gc["width"].Value.Replace("点", "."));
                if (result[0] < 4)
                {
                    result[0] = result[0] * 100;
                    result[1] = result[1] * 100;
                }
            }
            return true;
        }

        private static bool MatchUnitLength(string source, ref double[] result)
        {
            Regex reg = new Regex(ConfigurationManager.AppSettings["hasUnitReg"]);
            var matchs = reg.Matches(source);
            if (matchs == null || matchs.Count == 0)
                return false;
            foreach (Match match in matchs)
            {
                GroupCollection gc = match.Groups;
                result[0] = Convert.ToDouble(gc["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(gc["width"].Value.Replace("点", "."));
                string unit = gc["Unit"].Value;
                if (unit != "CM")
                {
                    result[0] = result[0] * 100;
                    result[1] = result[1] * 100;
                }
                if (unit == "MM")
                {
                    result[0] = result[0] / 1000;
                    result[1] = result[1] / 1000;
                }
            }
            return true;
        }

        private static bool Match3Split(string source, ref double[] result)
        {
            Regex reg = new Regex(ConfigurationManager.AppSettings["has3SplitReg"]);
            var matchs = reg.Matches(source);
            if (matchs == null || matchs.Count == 0)
                return false;
            foreach (Match match in matchs)
            {
                GroupCollection gc = match.Groups;
                double flag1, flag2, flag3;
                flag1 = Convert.ToDouble(gc["length"].Value.Replace("点", "."));
                flag2 = Convert.ToDouble(gc["width"].Value.Replace("点", "."));
                flag3 = Convert.ToDouble(gc["flag"].Value.Replace("点", "."));

                if (flag1 < flag3)
                {
                    result[0] = flag2;
                    result[1] = flag3;
                }
                else
                {
                    result[0] = flag1;
                    result[1] = flag2;
                }
                if (result[0] < 3)
                {
                    result[0] = result[0] * 100;
                    result[1] = result[1] * 100;
                }
            }
            return true;
        }

        public static double[] MatchLength(this string fileName)
        {
            double[] result = { 0, 0 };
            if (!MatchUnitLength(fileName, ref result))
            {
                if (!Match3Split(fileName, ref  result))
                {
                    if (!MatchNoUnitLength(fileName, ref  result))
                    {
                        return result;
                    }
                }
            }
            return result;
        }

        public static int MatchQTY(this string source)
        {
            int qty = 1;
            Regex reg = new Regex(ConfigurationManager.AppSettings["qtyReg"]);
            var matchs = reg.Matches(source);
            if (matchs == null || matchs.Count == 0) return qty;
            foreach (Match match in matchs)
            {
                GroupCollection gc = match.Groups;
                qty = gc["QTY"].Value.ToNumber();
            }
            return qty;
        }
    }
}