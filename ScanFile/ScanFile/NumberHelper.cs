
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile
{
    public static class NumberHelper
    {
        public static Dictionary<string, int> numbers = new Dictionary<string, int>(){
            {"一",1},
            {"二",2},
            {"两",2},
            {"三",3},
            {"四",4},
            {"五",5},
            {"六",6},
            {"七",7},
            {"八",8},
            {"九",9},
            {"十",10}, 
            {"零",0},
            {"",0},
        };

        public static int ToNumber(this string source)
        {
            try
            {
                source.TrimStart('零');
                if (source.Contains("亿"))
                {
                    string[] numbers = source.Split('亿');
                    return numbers[0].ToNumber() * 100000000 + numbers[1].ToNumber();
                }
                if (source.Contains("万"))
                {
                    string[] numbers = source.Split('万');
                    return numbers[0].ToNumber() * 10000 + numbers[1].ToNumber();
                }
                if (source.Contains("千"))
                {
                    string[] numbers = source.Split('千');
                    return numbers[0].ToNumber() * 1000 + numbers[1].ToNumber();
                }
                if (source.Contains("百"))
                {
                    string[] numbers = source.Split('百');
                    return numbers[0].ToNumber() * 100 + numbers[1].ToNumber();
                }
                if (source.Contains("十") && source.Length > 1)
                {
                    string[] numbers = source.Split('十');
                    numbers[0] = numbers[0] == "" ? "1" : numbers[0];
                    return numbers[0].ToNumber() * 10 + numbers[1].ToNumber();
                }
                return ConvertToNumber(source);
            }
            catch
            {
                return -1;
            }
        }

        private static string ReplaceQuanjiao(string source)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(source);
            if (source.Contains("０"))
            {
                sb.Replace("０", "0");
            }
            if (source.Contains("１"))
            {
                sb.Replace("１", "1");
            }
            if (source.Contains("２"))
            {
                sb.Replace("２", "2");
            }
            if (source.Contains("３"))
            {
                sb.Replace("３", "3");
            }
            if (source.Contains("４"))
            {
                sb.Replace("４", "4");
            }
            if (source.Contains("５"))
            {
                sb.Replace("５", "5");
            }
            if (source.Contains("６"))
            {
                sb.Replace("６", "6");
            }
            if (source.Contains("７"))
            {
                sb.Replace("７", "7");
            }
            if (source.Contains("８"))
            {
                sb.Replace("８", "8");
            }
            if (source.Contains("９"))
            {
                sb.Replace("９", "9");
            }
            return sb.ToString();
        }

        private static int ConvertToNumber(string source)
        {

            if (numbers.Keys.Contains(source))
                return numbers[source];
            source = ReplaceQuanjiao(source);
            int result;
            if (!int.TryParse(source, out result))
            {
                int temp = 1;

                foreach (char item in source.Reverse())
                {
                    result += ConvertToNumber(item + "") * temp;
                    temp = temp * 10;
                }
            }
            return result;
        }
    }
}
