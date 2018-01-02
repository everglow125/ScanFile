using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;

public class DataTableHelper
{
    public static MemoryStream DateTableToMemoryStream(DataTable dt)
    {
        if (dt == null)
        {
            return null;
        }
        try
        {
            int num;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow row = sheet.CreateRow(0);
            DataColumn column = null;
            for (num = 0; num < dt.Columns.Count; num++)
            {
                column = dt.Columns[num];
                row.CreateCell(num, CellType.String).SetCellValue(column.ColumnName);
            }
            for (num = 0; num < dt.Rows.Count; num++)
            {
                row = sheet.CreateRow(num + 1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    double num3;
                    if (double.TryParse(dt.Rows[num][i].ToString(), out num3))
                    {
                        row.CreateCell(i, CellType.Numeric).SetCellValue(num3);
                    }
                    else
                    {
                        row.CreateCell(i, CellType.String).SetCellValue(dt.Rows[num][i].ToString());
                    }
                }
            }
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static Stream DateTableToStream(DataTable dt)
    {
        if (dt == null)
        {
            return null;
        }
        try
        {
            int num;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow row = sheet.CreateRow(0);
            DataColumn column = null;
            for (num = 0; num < dt.Columns.Count; num++)
            {
                column = dt.Columns[num];
                row.CreateCell(num, CellType.String).SetCellValue(column.ColumnName);
            }
            for (num = 0; num < dt.Rows.Count; num++)
            {
                row = sheet.CreateRow(num + 1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    double num3;
                    if (double.TryParse(dt.Rows[num][i].ToString(), out num3))
                    {
                        row.CreateCell(i, CellType.Numeric).SetCellValue(num3);
                    }
                    else
                    {
                        row.CreateCell(i, CellType.String).SetCellValue(dt.Rows[num][i].ToString());
                    }
                }
            }
            Stream stream = new MemoryStream();
            workbook.Write(stream);
            return stream;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

