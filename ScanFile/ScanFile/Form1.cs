using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ScanFile
{
    public partial class ScanFolder : Form
    {
        private double XiezhenPrice;
        private double UVPrice;
        private double KTPrice;
        private double DantouPrice;
        private double JiangpaiPrice;
        private DateTime currentDate;
        private DataTable fileList;
        private double DengpianPrice;


        public ScanFolder()
        {
            InitializeComponent();
            CheckAuthentication();
            InitTextBox();
            fileList = CreateTable();

        }
        /// <summary>
        /// 验证MAC地址是否已授权 未授权则提示并关闭程序
        /// </summary>
        private void CheckAuthentication()
        {
            var MAC_Address = MachineUtil.GetNetCardMAC().ToUpper().Trim();
            string Mac_AddressGroup = ConfigurationManager.AppSettings["MacAddressGroup"].ToUpper().Trim();
            if (!Mac_AddressGroup.Contains(MAC_Address))
            {
                bool showMacAddress = ConfigurationManager.AppSettings["showMacAddress"].ToUpper().Trim() == "TRUE";
                if (showMacAddress)
                {
                    MessageBox.Show(MAC_Address);
                }
                else
                {
                    MessageBox.Show("该机器未授权，不能使用本软件！");
                }
                System.Environment.Exit(0);
            }
        }

        private void InitTextBox()
        {
            this.txtYear.Text = DateTime.Now.Year.ToString();
        }

        private void InitPrince()
        {
            XiezhenPrice = Convert.ToDouble(this.txtXiezhen.Text.Trim());
            UVPrice = Convert.ToDouble(this.txtUV.Text.Trim());
            KTPrice = Convert.ToDouble(this.txtKT.Text.Trim());
            DantouPrice = Convert.ToDouble(this.txtDantou.Text.Trim());
            JiangpaiPrice = Convert.ToDouble(this.txtJiangpai.Text.Trim());
            DengpianPrice = Convert.ToDouble(this.txtDengpian.Text.Trim());
        }
        static string lastpath = "";
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog path = new FolderBrowserDialog();
                if (lastpath != path.SelectedPath)
                    path.SelectedPath = lastpath;
                path.ShowDialog();
                this.txtAddress.Text = path.SelectedPath;
                lastpath = this.txtAddress.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ImportFolder()
        {
            string folder = this.txtAddress.Text.Trim();
            ScanBind(folder);
            return;
            Action<string> ac = new Action<string>(ScanBind);
            ac.BeginInvoke(folder, null, null);
        }

        public void ScanBind(string folder)
        {
            if (this.txtCustomer.Text.Trim() == "")
            {
                string[] folders = folder.Replace("/", "\\").Split('\\');
                Regex regYear = new Regex(@"\d+年");
                this.txtCustomer.Text = regYear.Replace(folders[folders.Length - 1], "");
            }
            ScanFile(folder);
            BindFileList();
        }

        private void BindFileList()
        {
            DataView dv = fileList.DefaultView;
            dv.Sort = "时间 Asc";
            this.dataGridView1.DataSource = dv.ToTable();

        }

        private void ScanFile(string folder)
        {
            DirectoryInfo directory = new DirectoryInfo(folder);
            if (directory == null || !directory.Exists) return;
            foreach (FileInfo file in directory.GetFiles())
            {
                if (!file.Name.Trim().ToUpper().EndsWith("JPG"))
                    continue;

                string fileName = file.Name.Trim().ToUpper();
                string[] folderList = folder.Split('\\');
                string currentFolder = folderList[folderList.Length - 1].Trim().ToUpper();
                string currentFolder2 = folderList.Length > 1 ? folderList[folderList.Length - 2].Trim().ToUpper() : "";

                string fullName = folder + "\\" + file.Name;
                DataRow dr = fileList.NewRow();

                dr["文件名"] = fileName;
                dr["完全路径"] = fullName;
                dr["时间"] = currentDate.ToString("yyyy-MM-dd");
                var qty = fileName.MatchQTY();
                dr["数量"] = qty.ToString();

                var ld = fileName.MatchLength();
                if (ld[0] == 0) ld = currentFolder.MatchLength();
                if (ld[0] == 0) ld = currentFolder2.MatchLength();
                dr["长度"] = (ld[0] / 100).ToString();
                dr["宽度"] = (ld[1] / 100).ToString();
                var size = ((ld[0] / 100) * (ld[1] / 100));
                dr["面积"] = size.ToString();

                string printType = GetPrintType(fileName);
                if (printType == "")
                    printType = GetPrintType(currentFolder);
                if (printType == "")
                    printType = GetPrintType(currentFolder2);
                if (printType == "")
                    printType = "写真";
                dr["类型"] = printType;
                var price = GetUnitPrice(printType);
                dr["单价"] = price;

                dr["总价"] = GetTotalAmount(qty, price, size, printType);
                fileList.Rows.Add(dr);
            }
            if (this.cbxChild.Checked)
            {
                Regex floderReg = new Regex(ConfigurationManager.AppSettings["floderReg"]);
                foreach (DirectoryInfo folderItem in directory.GetDirectories())
                {
                    if (floderReg.IsMatch(folderItem.Name))
                        currentDate = Convert.ToDateTime(this.txtYear.Text.Trim() + "-" + folderItem.Name.Replace(".", "-"));
                    ScanFile(folder + "\\" + folderItem.Name);
                }
            }
        }

        private double GetTotalAmount(int qty, double price, double size, string type)
        {
            if (type == "奖牌")
            {
                return qty * price;
            }
            return qty * price * size;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                this.fileList.Clear();
                InitPrince();
                ImportFolder();
                dataGridView1.Columns["完全路径"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {

        }

        private void btnExportFile_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (dataGridView1.DataSource as DataTable);
                string defaultname = this.txtCustomer.Text.Trim() + this.txtYear.Text.Trim() + ".xls";
                string savePaht = ExcelHelper.GetSaveFilePath(defaultname);
                string sheet = this.txtCustomer.Text.Trim() == "" ? this.txtYear.Text.Trim() : this.txtCustomer.Text.Trim();
                if (string.IsNullOrEmpty(savePaht)) return;
                ExcelHelper.ExportToExcel(dt, sheet, savePaht);
                OpenFileDir(savePaht);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private double GetUnitPrice(string printType)
        {
            double unitPrice = 0;
            switch (printType)
            {
                case "写真": unitPrice = XiezhenPrice; break;
                case "UV": unitPrice = UVPrice; break;
                case "KT板": unitPrice = KTPrice; break;
                case "奖牌": unitPrice = JiangpaiPrice; break;
                case "灯片": unitPrice = DengpianPrice; break;
                case "单透": unitPrice = DantouPrice; break;
            }
            return unitPrice;

        }


        private string GetPrintType(string fileName)
        {
            if (fileName.Contains("写真") || fileName.Contains("车贴"))
            {
                return "写真";
            }
            if (fileName.Contains("灯片"))
            {
                return "灯片";
            }
            if (fileName.Contains("UV"))
            {
                return "UV";
            }
            if (fileName.Contains("KT板"))
            {
                return "KT板";
            }
            if (fileName.Contains("奖牌"))
            {
                return "奖牌";
            }
            if (fileName.Contains("单透"))
            {
                return "单透";
            }
            return "";
        }

        private DataTable CreateTable()
        {
            DataTable result = new DataTable();
            result.Columns.Add("时间");
            result.Columns.Add("文件名");
            result.Columns.Add("长度");
            result.Columns.Add("宽度");
            result.Columns.Add("面积");
            result.Columns.Add("类型");
            result.Columns.Add("数量");
            result.Columns.Add("单价");
            result.Columns.Add("总价");
            result.Columns.Add("完全路径");
            return result;
        }



        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var filePath = dataGridView1.Rows[e.RowIndex].Cells["完全路径"].Value.ToString();
                OpenFileDir(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenFileDir(string filePath)
        {
            Process open = new Process();
            open.StartInfo.FileName = "explorer";
            open.StartInfo.Arguments = @"/select," + filePath;
            open.Start();
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var index = e.RowIndex;
                dataGridView1.Rows[index].Cells["面积"].Value = Convert.ToDouble(dataGridView1.Rows[index].Cells["长度"].Value) * Convert.ToDouble(dataGridView1.Rows[index].Cells["宽度"].Value);
                dataGridView1.Rows[index].Cells["总价"].Value = (dataGridView1.Rows[index].Cells["类型"].Value.ToString() == "奖牌" ? 1 : Convert.ToDouble(dataGridView1.Rows[index].Cells["面积"].Value))
                    * Convert.ToDouble(dataGridView1.Rows[index].Cells["单价"].Value)
                    * Convert.ToDouble(dataGridView1.Rows[index].Cells["数量"].Value);
            }
            catch
            {
                MessageBox.Show("程序异常：是否输入错误数值");
            }
        }

    }
}
