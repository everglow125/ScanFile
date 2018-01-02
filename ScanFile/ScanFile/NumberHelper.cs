namespace ScanFile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class NumberHelper
    {
        public static Dictionary<string, int> numbers;

        static NumberHelper()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add("一", 1);
            dictionary.Add("二", 2);
            dictionary.Add("两", 2);
            dictionary.Add("三", 3);
            dictionary.Add("四", 4);
            dictionary.Add("五", 5);
            dictionary.Add("六", 6);
            dictionary.Add("七", 7);
            dictionary.Add("八", 8);
            dictionary.Add("九", 9);
            dictionary.Add("十", 10);
            dictionary.Add("零", 0);
            dictionary.Add("", 0);
            numbers = dictionary;
        }

        private static int ConvertToNumber(string source)
        {
            source = ReplaceQuanjiao(source);
            if (numbers.Keys.Contains(source))
                return numbers[source];
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

        private static string ReplaceQuanjiao(string source)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(source);
            if (source.Contains("０"))
            {
                builder.Replace("０", "0");
            }
            if (source.Contains("１"))
            {
                builder.Replace("１", "1");
            }
            if (source.Contains("２"))
            {
                builder.Replace("２", "2");
            }
            if (source.Contains("３"))
            {
                builder.Replace("３", "3");
            }
            if (source.Contains("４"))
            {
                builder.Replace("４", "4");
            }
            if (source.Contains("５"))
            {
                builder.Replace("５", "5");
            }
            if (source.Contains("６"))
            {
                builder.Replace("６", "6");
            }
            if (source.Contains("７"))
            {
                builder.Replace("７", "7");
            }
            if (source.Contains("８"))
            {
                builder.Replace("８", "8");
            }
            if (source.Contains("９"))
            {
                builder.Replace("９", "9");
            }
            return builder.ToString();
        }


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
    }
}

