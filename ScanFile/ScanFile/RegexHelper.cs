namespace ScanFile
{
    using System;
    using System.Configuration;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    //匹配长度 同一长度为米
    public static class RegexHelper
    {
        public static bool Match3Split(string source, ref double[] result)
        {
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["has3SplitReg"]).Matches(source);
            if ((matchs == null) || (matchs.Count == 0))
            {
                return false;
            }
            foreach (Match match in matchs)
            {
                GroupCollection groups = match.Groups;
                double num = Convert.ToDouble(groups["length"].Value.Replace("点", "."));
                double num2 = Convert.ToDouble(groups["width"].Value.Replace("点", "."));
                double num3 = Convert.ToDouble(groups["flag"].Value.Replace("点", "."));
                if (num < num3)
                {
                    result[0] = num2;
                    result[1] = num3;
                }
                else
                {
                    result[0] = num;
                    result[1] = num2;
                    result[2] = num3;
                }
            }
            return true;
        }

        public static bool MatchAfterUnitLength(string source, ref double[] result)
        {
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["hasUnitAfterReg"]).Matches(source);
            if ((matchs == null) || (matchs.Count == 0))
            {
                return false;
            }
            foreach (Match match in matchs)
            {
                GroupCollection groups = match.Groups;
                result[0] = Convert.ToDouble(groups["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(groups["width"].Value.Replace("点", "."));
                string unit = groups["Unit"].Value;
                FormatLength(ref result, unit);
            }
            return true;
        }

        private static void FormatLength(ref double[] result, string unit)
        {
            if (unit == "MM" || unit == "毫米")
            {
                result[0] /= 1000.0;
                result[1] /= 1000.0;
            }
            else if (unit == "米" || unit == "M")
            {
                result[0] = result[0];
                result[1] = result[1];
            }
            if (unit == "CM" || unit == "公分" || unit == "厘米")
            {
                result[0] /= 100.0;
                result[1] /= 100.0;
            }
        }

        private static void FormatLength(ref double[] result)
        {
            if (result[0] > 400 && result[1] > 400)
            {
                result[0] /= 1000.0;
                result[1] /= 1000.0;
            }
            else if (result[0] > 20 && result[1] > 20)
            {
                result[0] /= 100.0;
                result[1] /= 100.0;
            }
        }

        public static bool MatchUnitAfterWithSideReg(string source, ref double[] result)
        {
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["hasUnitAfterWithSideReg"]).Matches(source);
            if ((matchs == null) || (matchs.Count == 0))
            {
                return false;
            }
            foreach (Match match in matchs)
            {
                GroupCollection groups = match.Groups;
                result[0] = Convert.ToDouble(groups["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(groups["width"].Value.Replace("点", "."));
                string unit = groups["Unit"].Value;
                FormatLength(ref result, unit);
                var side = new double[2];
                side[0] = Convert.ToDouble(groups["lengthSide"].Value.Replace("点", "."));
                side[1] = Convert.ToDouble(groups["widthSide"].Value.Replace("点", "."));
                unit = groups["SiteUnit"].Value;
                FormatLength(ref side, unit);
                result[0] += side[0] * 2;
                result[1] += side[1] * 2;
            }
            return true;
        }

        public static bool MatchBeforeUnitLength(string source, ref double[] result)
        {
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["hasUnitBeforeReg"]).Matches(source);
            if ((matchs == null) || (matchs.Count == 0))
            {
                return false;
            }
            foreach (Match match in matchs)
            {
                GroupCollection groups = match.Groups;
                result[0] = Convert.ToDouble(groups["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(groups["width"].Value.Replace("点", "."));
                string unit = groups["Unit"].Value;
                FormatLength(ref result, unit);
            }
            return true;
        }

        public static bool MatchBothUnitLength(string source, ref double[] result)
        {
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["bothUnitReg"]).Matches(source);
            if ((matchs == null) || (matchs.Count == 0))
            {
                return false;
            }
            foreach (Match match in matchs)
            {
                GroupCollection groups = match.Groups;
                result[0] = Convert.ToDouble(groups["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(groups["width"].Value.Replace("点", "."));
                string unit = groups["Unit"].Value;
                FormatLength(ref result, unit);
            }
            return true;
        }

        public static double[] MatchLength(this string fileName)
        {
            double[] result = new double[3];
            if ((!MatchUnitAfterWithSideReg(fileName, ref result)
                && !MatchBothUnitLength(fileName, ref result)
                && !MatchAfterUnitLength(fileName, ref result))
                && ((!MatchBeforeUnitLength(fileName, ref result)
                && !Match3Split(fileName, ref result))
                && !MatchNoUnitLength(fileName, ref result)))
            {
                return result;
            }
            return result;
        }

        public static bool MatchNoUnitLength(string source, ref double[] result)
        {
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["noUnitReg"]).Matches(source);
            if ((matchs == null) || (matchs.Count == 0))
            {
                return false;
            }
            foreach (Match match in matchs)
            {
                GroupCollection groups = match.Groups;
                result[0] = Convert.ToDouble(groups["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(groups["width"].Value.Replace("点", "."));
                FormatLength(ref result);
            }
            return true;
        }

        public static int MatchQTY(this string source)
        {
            int num = 1;
            MatchCollection matchs = new Regex(ConfigurationManager.AppSettings["qtyReg"]).Matches(source);
            if ((matchs != null) && (matchs.Count != 0))
            {
                foreach (Match match in matchs)
                {
                    var tag = match.Groups["Tag"].Value;
                    if (tag == null || tag.ToString() != "第")
                        num = match.Groups["QTY"].Value.ToNumber();
                }
            }
            return num;
        }

        public static string ToDBC(this string input)
        {
            char[] chArray = input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '　')
                {
                    chArray[i] = ' ';
                }
                else if ((chArray[i] > 0xff00) && (chArray[i] < 0xff5f))
                {
                    chArray[i] = (char)(chArray[i] - 0xfee0);
                }
            }
            return new string(chArray);
        }
    }
}

