using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


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
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow row = sheet.CreateRow(0);
            DataColumn col = null;
            //NPOI.SS.UserModel.CellType cellType;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                col = dt.Columns[i];
                row.CreateCell(i, CellType.String).SetCellValue(col.ColumnName);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = sheet.CreateRow(i + 1);//创建内容行     
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    double temp;
                    if (double.TryParse(dt.Rows[i][j].ToString(), out temp))
                        row.CreateCell(j, CellType.Numeric).SetCellValue(temp);
                    else
                        row.CreateCell(j, CellType.String).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            return ms;

        }
        catch (Exception ex)
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
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow row = sheet.CreateRow(0);
            DataColumn col = null;
            //NPOI.SS.UserModel.CellType cellType;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                col = dt.Columns[i];
                row.CreateCell(i, CellType.String).SetCellValue(col.ColumnName);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = sheet.CreateRow(i + 1); //创建内容行     
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    double temp;
                    if (double.TryParse(dt.Rows[i][j].ToString(), out temp))
                        row.CreateCell(j, CellType.Numeric).SetCellValue(temp);
                    else
                        row.CreateCell(j, CellType.String).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            Stream ms = new MemoryStream();
            workbook.Write(ms);
            return ms;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

