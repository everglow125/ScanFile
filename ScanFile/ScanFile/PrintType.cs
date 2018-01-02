namespace ScanFile
{
    using System;
    using System.Runtime.CompilerServices;

    public static class PrintType
    {
        public static string GetPrintType(this string fileName)
        {
            if ((fileName.Contains("打孔") || fileName.Contains("喷绘")) || fileName.Contains("喷布"))
            {
                return "喷绘";
            }
            if (fileName.Contains("黑胶"))
            {
                return "黑胶";
            }
            if (fileName.Contains("灯片"))
            {
                return "灯片";
            }
            if (fileName.Contains("UV"))
            {
                return "UV";
            }
            if (fileName.Contains("KT"))
            {
                return "KT板";
            }
            if (fileName.Contains("奖牌"))
            {
                return "奖牌";
            }
            if (fileName.Contains("透明"))
            {
                return "透明车贴";
            }
            if (fileName.Contains("单透"))
            {
                return "单透";
            }
            if (fileName.Contains("车贴"))
            {
                return "车贴";
            }
            if (fileName.Contains("写真"))
            {
                return "写真";
            }
            return "";
        }
    }
}

