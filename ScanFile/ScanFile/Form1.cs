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

namespace ScanFile
{
    public partial class ScanFolder : Form
    {
        private double XiezhenPrice;
        private double UVPrice;
        private double KTPrice;
        private double DantouPrice;
        private double JiangpaiPrice;
        private Regex floderReg = new Regex(@"^\d{1,2}\.\d{1,2}$");
        private Regex lengthReg1 = new Regex(@"(?<length>\d{1,5}((\.|[点])\d{1,3}){0,1})[ X-]+(?<width>\d{1,5}((\.|[点])\d{1,3}){0,1})");
        private Regex qtyReg = new Regex(@"(?<QTY>([一二三四五六七八九十百千零]|\d)+)[张|块]");
        private DateTime currentDate;
        private DataTable fileList;

        private double[] MatchLength(string fileName)
        {

            double[] result = { 0, 0 };
            var temp = lengthReg1.Matches(fileName);
            if (temp == null || temp.Count == 0) return result;
            foreach (Match match in temp)
            {
                GroupCollection gc = match.Groups;
                result[0] = Convert.ToDouble(gc["length"].Value.Replace("点", "."));
                result[1] = Convert.ToDouble(gc["width"].Value.Replace("点", "."));
            }
            return result;
        }


        private int MatchQTY(string fileName)
        {

            int qty = 1;
            var temp = qtyReg.Matches(fileName);
            if (temp == null || temp.Count == 0) return qty;
            foreach (Match match in temp)
            {
                GroupCollection gc = match.Groups;
                qty = ConvertNum(gc["QTY"].Value);
            }
            return qty;
        }
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
            // this.pictureBox1.BackgroundImage = Image
            pictureBox1.Load("F://20140212231935_kaxFE.thumb.600_0.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void InitPrince()
        {
            XiezhenPrice = Convert.ToDouble(this.txtXiezhen.Text.Trim());
            UVPrice = Convert.ToDouble(this.txtUV.Text.Trim());
            KTPrice = Convert.ToDouble(this.txtKT.Text.Trim());
            DantouPrice = Convert.ToDouble(this.txtDantou.Text.Trim());
            JiangpaiPrice = Convert.ToDouble(this.txtJiangpai.Text.Trim());
        }
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog path = new FolderBrowserDialog();
                path.ShowDialog();
                this.txtAddress.Text = path.SelectedPath;
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
            ScanFile(folder);
            BindFileList();
        }

        private void BindFileList()
        {
            this.dataGridView1.DataSource = fileList;
        }

        private void ScanFile(string folder)
        {
            DirectoryInfo directory = new DirectoryInfo(folder);
            if (directory == null || !directory.Exists) return;
            foreach (FileInfo file in directory.GetFiles())
            {
                if (!file.Name.Trim().ToUpper().EndsWith("JPG"))
                    continue;
                string fullName = folder + "\\" + file.Name;
                DataRow dr = fileList.NewRow();
                dr["文件名"] = file.Name.Trim().ToUpper();
                dr["时间"] = currentDate.ToString("yyyy-MM-dd");
                dr["类型"] = GetPrintType(dr["文件名"].ToString());
                var price = GetUnitPrice(dr["类型"].ToString());
                dr["单价"] = price;
                var ld = MatchLength(dr["文件名"].ToString());
                dr["长度"] = (ld[0] / 100).ToString();
                dr["宽度"] = (ld[1] / 100).ToString();
                var size = ((ld[0] / 100) * (ld[1] / 100));
                dr["面积"] = size.ToString();
                var qty = MatchQTY(dr["文件名"].ToString());
                dr["数量"] = qty.ToString();
                dr["总价"] = GetTotalAmount(qty, price, size, dr["类型"].ToString());
                fileList.Rows.Add(dr);
            }
            if (this.cbxChild.Checked)
            {
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
            this.dataGridView1.DataSource = null;
            InitPrince();
            ImportFolder();
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {

        }

        private void btnExportFile_Click(object sender, EventArgs e)
        {

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
                case "灯片": unitPrice = 0; break;
                case "单透": unitPrice = DantouPrice; break;
            }
            return unitPrice;

        }


        private string GetPrintType(string fileName)
        {
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
            return "写真";
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
            return result;
        }

        private int ConvertNum(string num)
        {
            if (num.Contains("百"))
            {
                string[] baifen = num.Split('百');
                return switchNum(baifen[0]) * 100 + ConvertNum(baifen[1]);
            }
            if (num.Contains("十") && num.Length > 1)
            {
                string[] shifen = num.Split('十');
                return switchNum(shifen[0]) * 10 + switchNum(shifen[1]);
            }
            return switchNum(num);

        }

        private int switchNum(string n)
        {
            switch (n)
            {
                case "零":
                    {
                        return 0;
                    }
                case "一":
                    {
                        return 1;
                    }
                case "二":
                    {
                        return 2;
                    }
                case "三":
                    {
                        return 3;
                    }
                case "四":
                    {
                        return 4;
                    }
                case "五":
                    {
                        return 5;
                    }
                case "六":
                    {
                        return 6;
                    }
                case "七":
                    {
                        return 7;
                    }
                case "八":
                    {
                        return 8;
                    }
                case "九":
                    {
                        return 9;
                    }
                case "": return 0;
                default:
                    {
                        try { return Convert.ToInt32(n); }
                        catch
                        {
                            return 1;
                        }

                    }
            }
        }

    }
}
