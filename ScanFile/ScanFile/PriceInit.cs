namespace ScanFile
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class PriceInit
    {
        public static List<PriceInfo> GetPrices()
        {
            List<PriceInfo> list = new List<PriceInfo>();
            DataTable table = ExcelHelper.ImportFromExcel(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"priceConfig\price.xls", "Sheet1", 0);
            foreach (DataRow row in table.Rows)
            {
                PriceInfo item = new PriceInfo
                {
                    Customer = row["客户"].ToString(),
                    Dantou = Convert.ToDouble(row["单透"].ToString()),
                    Touming = Convert.ToDouble(row["透明车贴"].ToString()),
                    Chetie = Convert.ToDouble(row["车贴"].ToString()),
                    Xiezhen = Convert.ToDouble(row["写真"].ToString()),
                    Dengpian = Convert.ToDouble(row["灯片"].ToString()),
                    UV = Convert.ToDouble(row["UV"].ToString()),
                    KTBan = Convert.ToDouble(row["KT板"].ToString()),
                    Heijiao = Convert.ToDouble(row["黑胶"].ToString()),
                    Penhui = Convert.ToDouble(row["喷绘"].ToString()),
                    Jiangpai = Convert.ToDouble(row["奖牌"].ToString())
                };
                list.Add(item);
            }
            return list;
        }
    }
}

