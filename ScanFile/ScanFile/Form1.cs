﻿using System;
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

namespace ScanFile
{
    public partial class Form1 : Form
    {
        private double XiezhenPrice;
        private double UVPrice;
        private double PTPrice;
        private double DantouPrice;
        private double JiangpaiPrice;
        public Form1()
        {
            InitializeComponent();
            CheckAuthentication();
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

        private void InitPrince()
        {
            XiezhenPrice = Convert.ToDouble(this.txtXiezhen.Text.Trim());
            UVPrice = Convert.ToDouble(this.txtUV.Text.Trim());
            PTPrice = Convert.ToDouble(this.txtPT.Text.Trim());
            DantouPrice = Convert.ToDouble(this.txtDantou.Text.Trim());
            JiangpaiPrice = Convert.ToDouble(this.txtJiangpai.Text.Trim());
        }

        public List<TFile> fileList = new List<TFile>();
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
            BindingSource bs = new BindingSource();
            bs.DataSource = fileList;
            this.dataGridView1.DataSource = bs;
        }

        private void ScanFile(string folder)
        {
            DirectoryInfo directory = new DirectoryInfo(folder);
            if (directory == null || !directory.Exists) return;
            foreach (FileInfo file in directory.GetFiles())
            {
                string fullName = folder + "\\" + file.Name;
                var existsItem = fileList.Where(x => x.FullName == fullName).ToList();
                if (existsItem != null && existsItem.Count > 0) continue;
                fileList.Add(new TFile(file.Name, fullName));
            }
            if (this.cbxChild.Checked)
            {
                foreach (DirectoryInfo folderItem in directory.GetDirectories())
                    ScanFile(folder + "\\" + folderItem.Name);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ImportFolder();
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {

        }

        private void btnExportFile_Click(object sender, EventArgs e)
        {

        }
    }
}
