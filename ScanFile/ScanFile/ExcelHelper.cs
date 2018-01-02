using Microsoft.CSharp.RuntimeBinder;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

public static class ExcelHelper
{
    public static string ConvertColumnIndexToColumnName(int index)
    {
        index++;
        int num = 0x1a;
        char[] chArray = new char[100];
        int capacity = 0;
        while (index > 0)
        {
            int num3 = index % num;
            if (num3 == 0)
            {
                num3 = num;
            }
            chArray[capacity++] = (char)((num3 - 1) + 0x41);
            index = (index - 1) / 0x1a;
        }
        StringBuilder builder = new StringBuilder(capacity);
        for (int i = capacity - 1; i >= 0; i--)
        {
            builder.Append(chArray[i]);
        }
        return builder.ToString();
    }

    public static bool ConvertToBoolen(object b)
    {
        string str = (b ?? "").ToString().Trim();
        bool result = false;
        if (bool.TryParse(str, out result))
        {
            return result;
        }
        if ((str != "0") && (str != "1"))
        {
            throw new Exception("布尔格式不正确，转换布尔类型失败！");
        }
        return (str == "0");
    }

    public static DateTime ConvertToDate(object date)
    {
        string s = (date ?? "").ToString();
        DateTime result = new DateTime();
        if (!DateTime.TryParse(s, out result))
        {
            try
            {
                string str2 = "";
                if (s.Contains("-"))
                {
                    str2 = "-";
                }
                else if (s.Contains("/"))
                {
                    str2 = "/";
                }
                string[] strArray = s.Split(str2.ToCharArray());
                int num = Convert.ToInt32(strArray[2]);
                int num2 = Convert.ToInt32(strArray[0]);
                int num3 = Convert.ToInt32(strArray[1]);
                string str3 = Convert.ToString(num);
                string str4 = Convert.ToString(num2);
                string str5 = Convert.ToString(num3);
                if (str4.Length == 4)
                {
                    result = Convert.ToDateTime(date);
                }
                else
                {
                    if (str3.Length == 1)
                    {
                        str3 = "0" + str3;
                    }
                    if (str4.Length == 1)
                    {
                        str4 = "0" + str4;
                    }
                    if (str5.Length == 1)
                    {
                        str5 = "0" + str5;
                    }
                    result = Convert.ToDateTime("20" + str3 + "-" + str4 + "-" + str5);
                }
            }
            catch
            {
                throw new Exception("日期格式不正确，转换日期类型失败！");
            }
        }
        return result;
    }

    public static decimal ConvertToDecimal(object d)
    {
        string s = (d ?? "").ToString();
        decimal result = 0M;
        if (!decimal.TryParse(s, out result))
        {
            throw new Exception("数字格式不正确，转换数字类型失败！");
        }
        return result;
    }

    private static IWorkbook CreateWorkbook(bool isCompatible)
    {
        if (isCompatible)
        {
            return new HSSFWorkbook();
        }
        return new XSSFWorkbook();
    }

    /// <summary>
    /// 创建工作薄(依据文件流)
    /// </summary>
    /// <param name="isCompatible"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static IWorkbook CreateWorkbook(bool isCompatible, dynamic stream)
    {
        if (isCompatible)
        {
            return new HSSFWorkbook(stream);
        }
        else
        {
            return new XSSFWorkbook(stream);
        }
    }
    public static string ExportToExcel(DataSet sourceDs, string filePath = null)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = GetSaveFilePath("");
        }
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        IWorkbook workbook = CreateWorkbook(GetIsCompatible(filePath));
        ICellStyle cellStyle = GetCellStyle(workbook);
        for (int i = 0; i < sourceDs.Tables.Count; i++)
        {
            DataTable table = sourceDs.Tables[i];
            string sheetname = "result" + i.ToString();
            ISheet sheet = workbook.CreateSheet(sheetname);
            IRow row = sheet.CreateRow(0);
            foreach (DataColumn column in table.Columns)
            {
                ICell cell = row.CreateCell(column.Ordinal);
                cell.SetCellValue(column.ColumnName);
                cell.CellStyle = cellStyle;
            }
            int rownum = 1;
            foreach (DataRow row2 in table.Rows)
            {
                IRow row3 = sheet.CreateRow(rownum);
                foreach (DataColumn column in table.Columns)
                {
                    row3.CreateCell(column.Ordinal).SetCellValue((row2[column] ?? "").ToString());
                }
                rownum++;
            }
        }
        FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        workbook.Write(stream);
        stream.Dispose();
        workbook = null;
        return filePath;
    }

    public static string ExportToExcel(DataTable sourceTable, string sheetName = "result", string filePath = null)
    {
        if (sourceTable.Rows.Count <= 0)
        {
            return null;
        }
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = GetSaveFilePath("");
        }
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        IWorkbook workbook = CreateWorkbook(GetIsCompatible(filePath));
        ICellStyle cellStyle = GetCellStyle(workbook);
        ISheet sheet = workbook.CreateSheet(sheetName);
        IRow row = sheet.CreateRow(0);
        NPOI.SS.UserModel.IFont font = workbook.CreateFont();
        font.Boldweight = 700;
        font.FontHeight = 256.0;
        ICellStyle style2 = workbook.CreateCellStyle();
        style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style2.FillForegroundColor = 0x16;
        style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        style2.VerticalAlignment = VerticalAlignment.Top;
        style2.SetFont(font);
        ICellStyle style3 = workbook.CreateCellStyle();
        style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        style3.VerticalAlignment = VerticalAlignment.Top;
        IDataFormat format = workbook.CreateDataFormat();
        ICellStyle style4 = workbook.CreateCellStyle();
        style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style4.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        style4.VerticalAlignment = VerticalAlignment.Top;
        style4.DataFormat = format.GetFormat("mm月dd日");
        ICellStyle style5 = workbook.CreateCellStyle();
        style5.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style5.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style5.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style5.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style5.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
        style5.VerticalAlignment = VerticalAlignment.Top;
        foreach (DataColumn column in sourceTable.Columns)
        {
            ICell cell = row.CreateCell(column.Ordinal);
            cell.SetCellValue(column.ColumnName);
            cell.CellStyle = style2;
        }
        int rownum = 1;
        int firstRow = 1;
        int num3 = 0;
        string str = sourceTable.Rows[0][0].ToString();
        foreach (DataRow row2 in sourceTable.Rows)
        {
            IRow row3 = sheet.CreateRow(rownum);
            foreach (DataColumn column in sourceTable.Columns)
            {
                ICell cell2 = row3.CreateCell(column.Ordinal);
                if (((((column.ColumnName == "长度") || (column.ColumnName == "宽度")) || ((column.ColumnName == "面积") || (column.ColumnName == "单价"))) || (column.ColumnName == "总价")) || (column.ColumnName == "数量"))
                {
                    cell2.SetCellValue(Convert.ToDouble(row2[column]));
                    cell2.SetCellType(CellType.Numeric);
                    if (column.ColumnName == "面积")
                    {
                        cell2.SetCellFormula(string.Format("C{0}*D{0}", rownum + 1));
                    }
                    if (column.ColumnName == "总价")
                    {
                        if (row2["类型"].ToString() != "奖牌")
                        {
                            cell2.SetCellFormula(string.Format("E{0}*H{0}*G{0}", rownum + 1));
                        }
                        else
                        {
                            cell2.SetCellFormula(string.Format("H{0}*G{0}", rownum + 1));
                        }
                    }
                }
                else if (column.ColumnName == "时间")
                {
                    DateTime date = Convert.ToDateTime(row2["时间"].ToString()).Date;
                    cell2.SetCellValue(date);
                }
                else
                {
                    cell2.SetCellValue((row2[column] ?? "").ToString());
                }
                cell2.CellStyle = (column.ColumnName == "文件名") ? style5 : ((column.ColumnName == "时间") ? style4 : style3);
                if (column.ColumnName == "完全路径")
                {
                    IHyperlink hyperlink = new HSSFHyperlink(HyperlinkType.Url);
                    if (ConfigurationManager.AppSettings["IsOpenFile"].Trim().ToLower() == "true")
                    {
                        hyperlink.Address = row2[column].ToString();
                        cell2.SetCellValue("打开文件");
                    }
                    else
                    {
                        hyperlink.Address = row2[column].ToString().Substring(0, row2[column].ToString().LastIndexOf(@"\"));
                        cell2.SetCellValue("打开文件夹");
                    }
                    cell2.Hyperlink = hyperlink;
                }
            }
            if (rownum > 1)
            {
                string str2 = sourceTable.Rows[rownum - 1]["时间"].ToString();
                if ((rownum != sourceTable.Rows.Count) && (str2 == str))
                {
                    num3++;
                }
                else
                {
                    if (rownum == sourceTable.Rows.Count)
                    {
                        num3++;
                    }
                    str = str2;
                    sheet.AddMergedRegion(new CellRangeAddress(firstRow, firstRow + num3, 0, 0));
                    firstRow = rownum;
                    num3 = 0;
                }
            }
            rownum++;
        }
        for (int i = 0; i < (sourceTable.Rows.Count - 1); i++)
        {
            if (i == 1)
            {
                sheet.SetColumnWidth(i, 0x1900);
            }
            else
            {
                sheet.SetColumnWidth(i, 0xc00);
            }
        }
        sheet.CreateRow(rownum).CreateCell(8).SetCellFormula(string.Format("SUM(I2:I{0})", rownum));
        sheet.CreateFreezePane(0, 1, 0, 1);
        FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        workbook.Write(stream);
        stream.Dispose();
        sheet = null;
        row = null;
        workbook = null;
        return filePath;
    }

    public static string ExportToExcel(DataGridView grid, string sheetName = "result", string filePath = null)
    {
        if (grid.Rows.Count <= 0)
        {
            return null;
        }
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = GetSaveFilePath("");
        }
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        IWorkbook workbook = CreateWorkbook(GetIsCompatible(filePath));
        ICellStyle cellStyle = GetCellStyle(workbook);
        ISheet sheet = workbook.CreateSheet(sheetName);
        IRow row = sheet.CreateRow(0);
        for (int i = 0; i < grid.Columns.Count; i++)
        {
            ICell cell = row.CreateCell(i);
            cell.SetCellValue(grid.Columns[i].HeaderText);
            cell.CellStyle = cellStyle;
        }
        int rownum = 1;
        foreach (DataGridViewRow row2 in (IEnumerable)grid.Rows)
        {
            IRow row3 = sheet.CreateRow(rownum);
            for (int j = 0; j < grid.Columns.Count; j++)
            {
                row3.CreateCell(j).SetCellValue((row2.Cells[j].Value ?? "").ToString());
            }
            rownum++;
        }
        FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        workbook.Write(stream);
        stream.Dispose();
        sheet = null;
        row = null;
        workbook = null;
        return filePath;
    }

    public static string ExportToExcel<T>(List<T> data, IList<KeyValuePair<string, string>> headerNameList, string sheetName = "result", string filePath = null) where T : class
    {
        KeyValuePair<string, string> pair;
        if (data.Count <= 0)
        {
            return null;
        }
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = GetSaveFilePath("");
        }
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        IWorkbook workbook = CreateWorkbook(GetIsCompatible(filePath));
        ICellStyle cellStyle = GetCellStyle(workbook);
        ISheet sheet = workbook.CreateSheet(sheetName);
        IRow row = sheet.CreateRow(0);
        for (int i = 0; i < headerNameList.Count; i++)
        {
            ICell cell = row.CreateCell(i);
            pair = headerNameList[i];
            cell.SetCellValue(pair.Value);
            cell.CellStyle = cellStyle;
        }
        System.Type type = typeof(T);
        int rownum = 1;
        foreach (T local in data)
        {
            IRow row2 = sheet.CreateRow(rownum);
            for (int j = 0; j < headerNameList.Count; j++)
            {
                pair = headerNameList[j];
                object obj2 = type.GetProperty(pair.Key).GetValue(local, null);
                row2.CreateCell(j).SetCellValue((obj2 ?? "").ToString());
            }
            rownum++;
        }
        FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        workbook.Write(stream);
        stream.Dispose();
        sheet = null;
        row = null;
        workbook = null;
        return filePath;
    }

    private static ICellStyle GetCellStyle(IWorkbook workbook)
    {
        ICellStyle style = workbook.CreateCellStyle();
        style.FillPattern = FillPattern.SolidForeground;
        style.FillForegroundColor = 0x16;
        style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Top;
        return style;
    }

    private static DataTable GetDataTableFromSheet(ISheet sheet, int headerRowIndex)
    {
        int num2;
        DataTable table = new DataTable();
        IRow row = sheet.GetRow(headerRowIndex);
        int lastCellNum = row.LastCellNum;
        for (num2 = row.FirstCellNum; num2 < lastCellNum; num2++)
        {
            if ((row.GetCell(num2) == null) || (row.GetCell(num2).StringCellValue.Trim() == ""))
            {
                lastCellNum = num2;
                break;
            }
            DataColumn column = new DataColumn(row.GetCell(num2).StringCellValue);
            table.Columns.Add(column);
        }
        for (num2 = headerRowIndex + 1; num2 <= sheet.LastRowNum; num2++)
        {
            IRow row2 = sheet.GetRow(num2);
            if ((row2 != null) && !string.IsNullOrEmpty(row2.GetCell(0).ToString()))
            {
                DataRow row3 = table.NewRow();
                for (int i = row2.FirstCellNum; i < lastCellNum; i++)
                {
                    row3[i] = row2.GetCell(i).ToString();
                }
                table.Rows.Add(row3);
            }
        }
        return table;
    }

    private static bool GetIsCompatible(string filePath)
    {
        return filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetOpenFilePath()
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Filter = "Excel Office97-2003(*.xls)|*.xls|Excel Office2007及以上(*.xlsx)|*.xlsx",
            FilterIndex = 0,
            Title = "打开",
            CheckFileExists = true,
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        };
        string fileName = null;
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            fileName = dialog.FileName;
        }
        return fileName;
    }

    public static string GetSaveFilePath(string fileName = "")
    {
        SaveFileDialog dialog = new SaveFileDialog
        {
            Filter = "Excel Office97-2003(*.xls)|*.xls|Excel Office2007及以上(*.xlsx)|*.xlsx",
            FileName = fileName,
            FilterIndex = 0,
            Title = "导出到",
            OverwritePrompt = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        };
        string str = null;
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            str = dialog.FileName;
        }
        return str;
    }

    public static DataSet ImportFromExcel(string excelFilePath, int headerRowIndex)
    {
        if (string.IsNullOrEmpty(excelFilePath))
        {
            excelFilePath = GetOpenFilePath();
        }
        if (string.IsNullOrEmpty(excelFilePath))
        {
            return null;
        }
        using (FileStream stream = File.OpenRead(excelFilePath))
        {
            bool isCompatible = GetIsCompatible(excelFilePath);
            return ImportFromExcel(stream, headerRowIndex, isCompatible);
        }
    }

    public static DataSet ImportFromExcel(Stream excelFileStream, int headerRowIndex, bool isCompatible)
    {
        DataSet set = new DataSet();
        IWorkbook workbook = CreateWorkbook(isCompatible, excelFileStream);
        for (int i = 0; i < workbook.NumberOfSheets; i++)
        {
            DataTable dataTableFromSheet = GetDataTableFromSheet(workbook.GetSheetAt(i), headerRowIndex);
            set.Tables.Add(dataTableFromSheet);
        }
        excelFileStream.Close();
        workbook = null;
        return set;
    }

    public static DataTable ImportFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
    {
        if (string.IsNullOrEmpty(excelFilePath))
        {
            excelFilePath = GetOpenFilePath();
        }
        if (string.IsNullOrEmpty(excelFilePath))
        {
            return null;
        }
        using (FileStream stream = File.OpenRead(excelFilePath))
        {
            bool isCompatible = GetIsCompatible(excelFilePath);
            return ImportFromExcel(stream, sheetName, headerRowIndex, isCompatible);
        }
    }

    public static DataTable ImportFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex, bool isCompatible)
    {
        IWorkbook workbook = CreateWorkbook(isCompatible, excelFileStream);
        ISheet sheetAt = null;
        int result = -1;
        if (int.TryParse(sheetName, out result))
        {
            sheetAt = workbook.GetSheetAt(result);
        }
        else
        {
            sheetAt = workbook.GetSheet(sheetName);
        }
        DataTable dataTableFromSheet = GetDataTableFromSheet(sheetAt, headerRowIndex);
        excelFileStream.Close();
        workbook = null;
        sheetAt = null;
        return dataTableFromSheet;
    }
}

