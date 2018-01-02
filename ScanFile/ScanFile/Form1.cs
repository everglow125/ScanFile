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
        private static string lastpath = "";
        private double PenhuiPrice;
        public List<PriceInfo> prices;
        private DataTable fileList;
        private double ToumingPrice;
        private double UVPrice;
        private double XiezhenPrice;
        private double HeijiaoPrice;
        private double JiangpaiPrice;
        private double KTPrice;
        private double ChetiePrice;
        private DateTime currentDate;
        private double DantouPrice;
        private double DengpianPrice;

        private void InitPrince()
        {
            this.XiezhenPrice = Convert.ToDouble(this.txtXiezhen.Text.Trim());
            this.UVPrice = Convert.ToDouble(this.txtUV.Text.Trim());
            this.KTPrice = Convert.ToDouble(this.txtKT.Text.Trim());
            this.DantouPrice = Convert.ToDouble(this.txtDantou.Text.Trim());
            this.JiangpaiPrice = Convert.ToDouble(this.txtJiangpai.Text.Trim());
            this.DengpianPrice = Convert.ToDouble(this.txtDengpian.Text.Trim());
            this.PenhuiPrice = Convert.ToDouble(this.txt_Penhui.Text.Trim());
            this.HeijiaoPrice = Convert.ToDouble(this.txtHeijiao.Text.Trim());
            this.ChetiePrice = Convert.ToDouble(this.txtChetie.Text.Trim());
            this.ToumingPrice = Convert.ToDouble(this.txtTouming.Text.Trim());
        }

        private void InitTextBox()
        {
            this.txtYear.Text = DateTime.Now.Year.ToString();
        }

        private double[] MatchLength(string fileFullName)
        {
            double[] numArray = new double[2];
            using (FileStream stream = new FileStream(fileFullName, FileMode.Open, FileAccess.Read))
            {
                using (Image image = Image.FromStream(stream))
                {
                    numArray[1] = ((double)image.Height) / 100.0;
                    numArray[0] = ((double)image.Width) / 100.0;
                }
            }
            return numArray;
        }

        private void OpenFileDir(string filePath)
        {
            new Process { StartInfo = { FileName = "explorer", Arguments = "/select," + filePath } }.Start();
        }

        public void ScanBind(string folder)
        {
            if (this.txtCustomer.Text.Trim() == "")
            {
                string[] strArray = folder.Replace("/", @"\").Split(new char[] { '\\' });
                this.txtCustomer.Text = new Regex(@"\d+年").Replace(strArray[strArray.Length - 1], "");
            }
            this.ScanFile(folder);
            this.BindFileList();
        }
        static string tmp = @"公示栏240 120喷绘3张.JPG;
156 183.JPG;
2喷绘344X282CM.JPG;
90X60喷绘.JPG;
喷绘344X282CM.JPG;
嘉豪农业-竖排2.4X0.4米喷绘.JPG;
180X240CM瑶族婚礼副屏喷绘15-20CM白边2.JPG;
180X240CM瑶族婚礼副屏喷绘留15-20CM白边.JPG;
猎豹7.8X0.7米喷绘横幅.JPG;
依家小店4.2X1米喷绘.JPG;
公路局公示栏190X95喷绘.JPG;
再聚冷饮8.4X1米喷绘.JPG;
华源小店4.2X1米喷绘.JPG;
口味冰8.4X1米喷绘.JPG;
大广场4.2X1米喷绘.JPG;
温馨4.2X1米喷绘.JPG;
特色4.2X1米喷绘.JPG;
金华石材8X1.12米喷绘.JPG;
2九拍游车4.5X2.5米喷绘2张.JPG;
九拍游车2X1米喷绘4张.JPG;
九拍游车4.5X2.5米喷绘2张.JPG;
星飞翔240X260CM喷绘2张 留15公分白边.JPG;
        治超办公示栏1.2X4.6米喷绘.JPG;
治超办工作职责1.2X4.6米喷绘.JPG;
公示栏240 120喷绘3张.JPG;
兄弟陶瓷800 120喷绘1张.JPG;
育苗幼儿园横幅12.5X1.5MI喷绘.JPG;
760 540.JPG;
小夫子80CMX120CM喷绘2张.JPG;
英豪啤酒200 180灯箱喷绘1个.JPG;
150-80.JPG;
星飞翔12X4.2米喷绘 包绳1张.JPG;
        泷台 保险公司26.3X3.66米 喷绘1张.JPG;
        泷台 波涛楼上15.5X8.5米 喷绘1张.JPG;
01泷台13.7X3米1张.JPG;
02泷台13.7X3米1张.JPG;
03泷台11.5X3米1张.JPG;
04泷台4.45X3米1张.JPG;
05泷台5.75X3米1张.JPG;
06泷台6.45X3米1张.JPG;
泷台12.76X3米1张 63.8米第3张.JPG;
泷台23.64X3米1张 71.1米第3张.JPG;
泷台23.73X3米1张 71.1米第1张.JPG;
泷台23.73X3米1张 71.1米第2张.JPG;
泷台25.52X3米1张 63.8米第2张.JPG;
泷台25.52X3米1张 63.8米第1张.JPG;
诚鑫超市425 300喷绘1张.JPG;
泷台喷绘4X2.4米 1张.JPG;
1.5X1.08M 喷绘 修改旺家福购物广场招租.JPG;
75 105喷绘.JPG;
泷台喷绘4X2.4米 1张.JPG;
大清仓170X150喷绘2张.JPG;
大清仓200X60喷绘.JPG;
天地壹号-兴湘大酒店450X245CM喷绘.JPG;
立天电梯销售5X1.2米喷绘.JPG;
97 200喷绘1张.JPG;
农村淘宝150 100喷绘4张.JPG;
118.3 190喷绘1张.JPG;
180 370.JPG;
317 277.JPG;
605 210.JPG;
龙华3.8X2米喷绘1张.JPG;
120 180喷绘1张.JPG;
400X70CM喷绘.JPG;
辉煌装饰210 270喷绘1张.JPG;
鸿兴汽贸720 307喷绘1张.JPG;
龙华3.8X2米喷绘1张.JPG;
90X122.5CM喷绘2张.JPG;
全场90X122.5CM喷绘2张.JPG;
第二件90X122.5CM喷绘2张.JPG;
100 60喷绘1张.JPG;
龙华3.6X1.2米喷绘1张.JPG;
1600 163.JPG;
3120 315.JPG;
星飞翔12X4.2米 喷绘1张  修改后的.JPG;
星飞翔12X4.2米 喷绘包绳1张.JPG;
        沁园 4.5X2.5喷绘 2张.JPG;
沁园2X1.5喷绘 2张.JPG;
沁园2X1喷绘 4张.JPG;
沁园4.5X2.5喷绘 2张(2).JPG;
博艺幼儿园120 80喷绘1张.JPG;
湖南建工分月实施计划190X110CM喷绘.JPG;
湖南建工工作流程190X110CM喷绘.JPG;
湖南建工指挥架构图190X110CM喷绘.JPG;
湖南建工简介大厅750X312CM喷绘.JPG;
湖南建工鸟瞰图190X110CM喷绘.JPG;
湖南第一工程简介190X110CM喷绘.JPG;
350 300.JPG;
湖南建工宣传240X120CM喷绘.JPG;
湖南建工宣传栏240X120CM喷绘.JPG;
博艺100 70喷绘1张.JPG;
博艺150 100喷绘1张.JPG;
沁园 4.5X2.5喷绘 1张.JPG;
沁园2X1喷绘 2张.JPG;
沁园4.5X2.5喷绘 1张.JPG;
龙华17.5X5.5米 网格布喷绘1张.JPG;
        龙华17.5X5.5米 网格布喷绘1张.JPG;
        龙华26X19米 网格布喷绘1张.JPG;
        龙华6X5.5米 网格布喷绘1张.JPG;
        博艺幼儿园120 80喷绘1张.JPG;
沁园10X0.8米横幅喷绘 1张.JPG;
沁园7.5X0.8米喷绘1张.JPG;
122 82两张.JPG;
352 68喷绘.JPG;
510 243两张.JPG;
81 510两张.JPG;
862 202.JPG;
862 202G.JPG;
001.JPG;
002.JPG;
003.JPG;
004.JPG;
1沁园净水器门头柱子116X275CM喷绘.JPG;
2沁园净水器门头柱子122X275CM喷绘.JPG;
华鑫北城耀世开盘11X4.2米喷绘.JPG;
小张挖机2.72X2.5米喷绘.JPG;
沁园净水器门头地贴5X2米喷绘.JPG;
沁园净水器门头横幅7X0.68米喷绘.JPG;
湖南一建620 250喷绘1张 改.JPG;
        东车智能353X298CM喷绘.JPG;
东车智能358X270CM喷绘.JPG;
东车智能405X222CM喷绘.JPG;
东车智能405X371CM喷绘.JPG;
东车智能500X248CM喷绘.JPG;
东车智能650X380喷绘.JPG;
东车智能750X256喷绘.JPG;
华鑫北城耀世开盘7X3.5米喷绘背景.JPG;
850 135喷绘.JPG;
850 135喷绘一.JPG;
核心价值观22.9X2.8米喷绘1张.JPG;
湖南一建862 202喷绘1张.JPG;
46米第一张 一方泷台26.5X1.2米喷绘1张.JPG;
46米第二张 一方泷台19.5X1.2米喷绘1张.JPG;
一方泷台20.2X1.2米喷绘1张.JPG;
一方泷台24.2X1.2米喷绘1张.JPG;
湖南一建122 82喷绘2张.JPG;
湖南一建510 243喷绘2张.JPG;
湖南一建81 510喷绘2张.JPG;
湖南一建862    202喷绘1张.JPG;
星飞翔12X4.2米 喷绘1张.JPG;
1沁园净水器门头柱子116X275CM喷绘.JPG;
2沁园净水器门头柱子122X275CM喷绘.JPG;
350 60喷绘.JPG;
696 167喷绘.JPG;
泷台湖南建工100 150喷绘1张.JPG;
泷台湖南建工300 146喷绘1张.JPG;
泷台湖南建工312 100喷绘1张.JPG;
风火水土95X150CM喷绘.JPG;
湖南建工6X0.7米2张喷绘.JPG;
湖南建工9X0.7米2张喷绘.JPG;
01湖南一建110 150喷绘1张.JPG;
02湖南一建110 150喷绘1张.JPG;
03湖南一建110 150喷绘1张.JPG;
04湖南一建110 150喷绘1张.JPG;
05湖南一建110 150喷绘1张.JPG;
06湖南一建110 150喷绘1张.JPG;
07湖南一建110 150喷绘1张.JPG;
白芒营车展150X1.84喷绘1张.JPG;
白芒营车展170X140CM喷绘1张.JPG;
白芒营车展2.44X1.84喷绘2张.JPG;
白芒营车展2.7X1.7米喷绘2张.JPG;
老板电器7X1.7米喷绘1张.JPG;
百味源8.5X2.2米 喷绘1张.JPG;
        福运多购物中心7X1.2米喷绘.JPG;
鸿兴汽贸15X1米喷绘.JPG;
340 290.JPG;
560 300.JPG;
600 145.JPG;
600 60.JPG;
640 400.JPG;
120 80喷绘一张.JPG;
猎豹横幅7.8X0.7米喷绘.JPG;
福真坊80 120喷绘1张.JPG;
诚信百货360X165CM喷绘.JPG;
鸿兴汽贸15X1米喷绘.JPG;
1掌上明珠-水星家居1.5X2.8米喷绘.JPG;
2掌上明珠-水星家居1.5X2.8米喷绘.JPG;
天地壹号102   230喷绘1张.JPG;
天地壹号102 230喷绘1张.JPG;
掌上明珠-水星家居10X3米喷绘.JPG;
盲人按摩60CMX40CMX2块喷绘.JPG;
01湖南一建300 83喷绘车贴1张.JPG;
02湖南一建300 83喷绘车贴1张.JPG;
03湖南一建300 83喷绘车贴1张.JPG;
04湖南一建300 83喷绘车贴1张.JPG;
水星家具户外广告3X4米喷绘.JPG;
1白芒营车展1.5X2.2米喷绘4块.JPG;
2白芒营车展1.5X2.2米喷绘4块.JPG;
四周穿绳 江华秋季车展背景 8X3M喷绘.JPG;
01龙华6.06X3.92喷绘1张.JPG;
02龙华3.63X3.92喷绘1张.JPG;
03龙华3.63X3.92喷绘1张.JPG;
04龙华3.63X3.92喷绘1张.JPG;
05龙华3.63X3.92喷绘1张.JPG;
06龙华3.63X3.92喷绘1张.JPG;
07龙华3.63X3.92喷绘1张.JPG;
08龙华3.63X3.92喷绘1张.JPG;
09龙华3.63X3.92喷绘1张.JPG;
10龙华3.63X3.92喷绘1张.JPG;
11龙华3.63X3.92喷绘1张.JPG;
12龙华3.63X3.92喷绘1张.JPG;
13龙华3.63X3.92喷绘1张.JPG;
14龙华3.63X3.92喷绘1张.JPG;
15龙华3.63X3.92喷绘1张.JPG;
16龙华3.63X3.92喷绘1张.JPG;
17龙华3.63X3.92喷绘1张.JPG;
18龙华3.63X3.92喷绘1张.JPG;
19龙华3.63X3.92喷绘1张.JPG;
20龙华3.6X3.92喷绘1张.JPG;
21龙华3.82X3.92喷绘1张.JPG;
22龙华3.63X3.92喷绘1张.JPG;
23龙华4.83X3.92喷绘1张.JPG;
24龙华3.63X3.92喷绘1张.JPG;
25龙华3.63X3.92喷绘1张.JPG;
26龙华1.8X3.92喷绘1张.JPG;
27龙华3.63X3.92喷绘1张.JPG;
28龙华3.63X3.92喷绘1张.JPG;
29龙华3.38X3.92喷绘1张.JPG;
30龙华3.63X3.92喷绘1张.JPG;
31龙华3.63X3.92喷绘1张.JPG;
32龙华3.63X3.92喷绘1张.JPG;
33龙华3.63X3.92喷绘1张.JPG;
34龙华3.63X3.92喷绘1张.JPG;
35龙华3.63X3.92喷绘1张.JPG;
36龙华3.63X3.92喷绘1张.JPG;
37龙华3.63X3.92喷绘1张.JPG;
38龙华3.63X3.92喷绘1张.JPG;
39龙华3.63X3.92喷绘1张.JPG;
40龙华3.63X3.92喷绘1张.JPG;
博才教育招生2X3米喷绘4张.JPG;
80X80CM喷绘20张.JPG;
婚礼3X2.4米喷绘.JPG;
婚礼6.4X3米喷绘.JPG;
富新超市 4X3米喷绘1张.JPG;
新天地超市4X3米喷绘2张.JPG;
01.JPG;
02.JPG;
16龙华3.63X3.92喷绘1张.JPG;
3.JPG;
4.JPG;
40 240喷绘.JPG;
博才教育3X2米喷绘.JPG;
龙华3.85X3.92米 喷绘1张.JPG;
        龙华4.06X3.92米 喷绘1张.JPG;
600X70海尔电器喷绘.JPG;
VIVO 200X160喷绘.JPG;
VIVO 330X160喷绘.JPG;
VIVO 400X160喷绘4张.JPG;
VIVO530X160喷绘3张.JPG;
VIVO600X160喷绘2张.JPG;
易地扶贫1.2X2.4米喷绘.JPG;
03龙华3.63X2.92喷绘1张.JPG;
200 300两张.JPG;
22龙华3.63X2.92喷绘1张.JPG;
300 200两张.JPG;
印象风65 200喷绘1张.JPG;
绿源电动车150 200喷绘1张.JPG;
东车智能2X2.5米喷绘.JPG;
农商银行480X385CM喷绘.JPG;
十字路汽车服务中心100 80喷绘1张.JPG;
十字路汽车服务中心12.13X1.5米 喷绘1张.JPG;
        十字路汽车服务中心160 150喷绘1张.JPG;
国庆桁架6X3米喷绘.JPG;
风火水土 120X160喷绘2张.JPG;
风火水土 202X90喷绘.JPG;
风火水土 235X150喷绘.JPG;
风火水土 280X150喷绘.JPG;
风火水土 450X250CM喷绘.JPG;
小新星庆中秋203X147CM喷绘.JPG;
小新星庆中秋350X200CM喷绘.JPG;
星飞翔3X2.4米 喷绘1张.JPG;
        护芳阁2.6X2.2米 喷绘1张.JPG;
        护芳阁3.4X2米 喷绘1张.JPG;
        湘味源土菜馆5.5X1.5米 喷绘1张.JPG;
        九龙2.4X1.6米 喷绘.JPG;
        湖南建工150X800喷绘2张.JPG;
01泷台湖南建工7.36MX1M喷绘1张.JPG;
02泷台湖南建工6.3MX1M喷绘1张.JPG;
03泷台湖南建工7.36X1喷绘1张.JPG;
04泷台湖南建工7.36X1喷绘1张.JPG;
招聘-江华2.2X2.2米喷绘.JPG;
泷台8.5X4.2米喷绘1张.JPG;
米祖服装店2.9X4.9米喷绘.JPG;
鸿兴汽贸483 300喷绘1张.JPG;
鸿兴汽贸500 225喷绘2张.JPG;
鸿兴汽贸505 300喷绘1张.JPG;
0001.JPG;
400 151.JPG;
四季花城便利店3.65X1.4米 喷绘1张.JPG;
        四季花城便利店5.8X1.4米 喷绘1张.JPG;
        鸿兴汽贸15X1.4米 喷绘1张.JPG;
700 250喷绘1张.JPG;
80 60喷绘3块.JPG;
泷台1000 302喷绘.JPG;
泷台220 220喷绘一张.JPG;
足缘足道2X1米 喷绘2张.JPG;
        足缘足道4.5X2.5米 喷绘2张.JPG;
2三联桁架2.2X2.2米喷绘.JPG;
漓泉横幅4X0.4米喷绘40条.JPG;
粗石江油茶2.5X1.5米灯箱喷绘.JPG;
通达汽车7.7X1.6米 喷绘1张.JPG;
01湖南一建1.1X1.5米 喷绘1张.JPG;
02湖南一建1.1X1.5米 喷绘1张.JPG;
03湖南一建1.1X1.5米 喷绘1张.JPG;
04湖南一建1.1X1.5米 喷绘1张.JPG;
05湖南一建1.1X1.5米 喷绘1张.JPG;
06湖南一建1.1X1.5米 喷绘1张.JPG;
07湖南一建1.1X1.5米 喷绘1张.JPG;
        喷绘2张 140X60CM.JPG;
大圩工程概况牌110X150喷绘.JPG;
桥市工程概况牌110X150喷绘.JPG;
泷台15.5X8.5米 喷绘1张.JPG;
        湖南一建642X122CM喷绘2张.JPG;
1大锡五牌一图110X150CM喷绘.JPG;
1码市五牌一图110X150CM喷绘.JPG;
2大锡五牌一图110X150CM喷绘.JPG;
2码市五牌一图110X150CM喷绘.JPG;
3大锡五牌一图110X150CM喷绘.JPG;
3码市五牌一图110X150CM喷绘.JPG;
4大锡五牌一图110X150CM喷绘.JPG;
4码市五牌一图110X150CM喷绘.JPG;
5大锡五牌一图110X150CM喷绘.JPG;
5码市五牌一图110X150CM喷绘.JPG;
6大锡五牌一图110X150CM喷绘.JPG;
6码市五牌一图110X150CM喷绘.JPG;
7大锡五牌一图110X150CM喷绘.JPG;
7码市五牌一图110X150CM喷绘.JPG;
湖南一建122 82喷绘4张.JPG;
湖南一建510 243喷绘4张.JPG;
湖南一建642X122CM喷绘2张.JPG;
湖南一建81 510喷绘4张.JPG;
湖南一建862    202喷绘2张.JPG;
湖南一建862 202喷绘1张 大圩.JPG;
        湖南一建862 202喷绘1张 小圩.JPG;
        电信餐厅2.5X1.5米 喷绘1张.JPG;
        电信餐厅3.62X0.9米 喷绘1张.JPG;
        湖南一建642X122CM喷绘2张.JPG;
鑫梦门头590X200CM喷绘.JPG;
泷台湖南建工7.36X1米 喷绘1张.JPG;
        泷台湖南建工7.36X1米 喷绘1张.JPG;
        封顶大吉8X2米喷绘.JPG;
01.JPG;
02.JPG;
03.JPG;
201.JPG;
202.JPG;
270X50CM喷绘.JPG;
300X32CM喷绘.JPG;
兄弟陶瓷1.26X1.6米 喷绘1张.JPG;
        兄弟陶瓷15X7米 喷绘1张.JPG;
        兄弟陶瓷8X4米 喷绘1张.JPG;
        双虎家私1.35X1.5喷绘2张.JPG;
双虎家私2.05X2.65喷绘1张.JPG;
双虎家私2.2X1.5喷绘4张.JPG;
横幅7X0.45米喷绘.JPG;
橱窗300X62CM喷绘.JPG;
橱窗3X2.4米喷绘.JPG;
门头7X2米喷绘.JPG;
门框3X0.45米喷绘.JPG;
汇金国际6.06X3米 喷绘1张.JPG;
        汇金国际6.4X3米 喷绘1张.JPG;
        湖南一建642X122CM喷绘6张.JPG;
湖南一建862 202喷绘1张 大石桥.JPG;
        条幅7X0.6米喷绘.JPG;
汇金国际6.4X3米 喷绘1张.JPG;
        湖南一建122 82喷绘2张.JPG;
湖南一建510 243喷绘2张.JPG;
湖南一建862    202喷绘1张.JPG;
科目二2X1米喷绘.JPG;
140 600喷绘.JPG;
80 60六张喷绘.JPG;
80 60十张喷绘.JPG;
大路铺五里营0.7MX1.2MX4张喷绘.JPG;
安全通道260X146CM喷绘.JPG;
安全通道260X166CM喷绘.JPG;
安全通道290X126CM喷绘.JPG;
安全通道76X200CM喷绘.JPG;
安全通道76X320CM喷绘.JPG;
汇金国际6.06X3米 喷绘1张.JPG;
        湖南一建81 510喷绘2张.JPG;
皇朝家私2000-210绘 1张.JPG;
皇朝家私4.3X1.65喷绘 1张.JPG;
皇朝家私6X3喷绘 1张.JPG;
40 240喷绘一张.JPG;
泷台 湖南建工100 100喷绘1张.JPG;
450 270喷绘.JPG;
印象风3X0.7米喷绘.JPG;
印象风6X0.7米喷绘.JPG;
通明商行7.5X1.5米 喷绘1张.JPG;
        金碗酒店1.7X3米 喷绘1张.JPG;
        金碗酒店7.6X2.7米 喷绘1张.JPG;
        鸿兴汽贸26.6X2.06米喷绘.JPG;
鸿兴汽贸1.5X1.84 喷绘1张.JPG;
鸿兴汽贸2.44X1.84 喷绘2张.JPG;
排球手举牌 闭幕式 喷绘 8×3M.JPG;
星飞翔5X3米喷绘.JPG;
足缘足道180X100CM喷绘3张.JPG;
足缘足道180X250CM喷绘6张.JPG;
01.JPG;
02.JPG;
整体转让14X4.1米喷绘.JPG;
整体转让9X3米喷绘.JPG;
汇金桁架6.06X3米喷绘.JPG;
汇金桁架6.4X3米喷绘.JPG;
老余汽修2.1X1.5米 喷绘1张.JPG;
        老余汽修8X1米 喷绘1张.JPG;
2新日电动车1.5X2.25米喷绘.JPG;
新日电动车0.4X1.1米喷绘.JPG;
新日电动车1.5X1.1米喷绘.JPG;
新日电动车1.5X2.25米喷绘.JPG;
01星飞翔2X2.6米 喷绘1张 两边留白10CM.JPG;
02星飞翔2X2.6米 喷绘1张 两边留白10CM.JPG;
60 90喷绘1张.JPG;
一品鹅香2.11X3.18米 喷绘1张.JPG;
        一品鹅香3.76X2.53米 喷绘1张.JPG;
        十九大报告2.4X1.2米喷绘.JPG;
学习十九大会议精神2.4X1.2米喷绘.JPG;
沁园7X0.6米 喷绘1张.JPG;
        沁园7X3米 喷绘2张.JPG;
        双虎家私4X2.74米 喷绘3张.JPG;
        天地壹号  1.2X0.8米 喷绘30张.JPG;
        天地壹号1.2X0.8米 喷绘30张.JPG;
        星飞翔12X4.5米 喷绘1张.JPG;
        泷台 保险公司26.3X3.66米 喷绘1张.JPG;
100 150喷绘1张.JPG;
国有林场80 60喷绘200张.JPG;
01九龙医院2.4X1.6喷绘5张.JPG;
02九龙医院2.4X1.6喷绘5张.JPG;
03九龙医院2.4X1.6喷绘5张.JPG;
04九龙医院2X1喷绘5张.JPG;
05九龙医院2X1喷绘5张.JPG;
06九龙医院1.3X0.9喷绘5张.JPG;
1220 300.JPG;
300 200喷绘两张.JPG;
553 300.JPG;
587.5  300.JPG;
138  159.JPG;
138 159.JPG;
140 40.JPG;
165 96.7车后.JPG;
56 160.JPG;
72 165车后.JPG;
93 160车身.JPG;
94 160.JPG;
96 160车身.JPG;
97 160.JPG;
2汇金8.5X3米喷绘.JPG;
汇金7.5X3米喷绘.JPG;
汇金8.5X3米喷绘.JPG;
汇金9X3米喷绘.JPG;
华斌1570 220喷绘.JPG;
胡桃里4X3米喷绘.JPG;
皇朝16X0.8米喷绘2张.JPG;
胡桃里4.5X3米喷绘.JPG;
胡桃里4X4米喷绘.JPG;
格力空调4.5X2.5米 喷绘4张.JPG;
        龙华 8.5X2.5米 喷绘1张.JPG;
        龙华 9.6X2.4米 喷绘1张.JPG;
        诚兴超市4.25X3米 喷绘1张.JPG;
        星飞翔12X4.2米 喷绘1张.JPG;
        游车200X100CM喷绘4张.JPG;
游车450X250CM喷绘4张.JPG;
舞台背景5X3.5米喷绘.JPG;
双虎  9X2.88米 喷绘1张.JPG;
        双虎1.93X1.8米 喷绘1张.JPG;
        双虎9X2.88米 喷绘1张.JPG;
        喷绘600 70一张.JPG;
牛掌柜18X1.92米 喷绘1张.JPG;
";


        private void ScanFile(string folder)
        {

            string[] tmpArr = tmp.Split(';');
            foreach (var info2 in tmpArr)
            {

                string fileName = info2.Trim().ToUpper();
                string[] strArray2 = folder.Split(new char[] { '\\' });
                string currentFolder = strArray2[strArray2.Length - 1].Trim().ToUpper();
                string str5 = (strArray2.Length > 1) ? strArray2[strArray2.Length - 2].Trim().ToUpper() : "";
                string str6 = folder + @"\" + info2;
                DataRow dr = this.fileList.NewRow();
                dr["文件名"] = fileName;
                dr["完全路径"] = str6;
                dr["时间"] = this.currentDate.ToString("yyyy-MM-dd");
                if (!this.SetCellValue(ref dr, fileName, currentFolder, str5, folder))
                {
                    this.SetCellValue(ref dr, fileName.ToDBC(), currentFolder, str5, folder);
                }
                this.fileList.Rows.Add(dr);
            }

        }
        //private void ScanFile(string folder)
        //{
        //    DirectoryInfo info = new DirectoryInfo(folder);
        //    if ((info != null) && info.Exists)
        //    {
        //        string[] strArray = ConfigurationManager.AppSettings["EndExp"].ToUpper().Trim().Split(new char[] { '|' });
        //        foreach (FileInfo info2 in info.GetFiles())
        //        {
        //            bool flag = false;
        //            string str = info2.Name.Trim().ToUpper();
        //            foreach (string str2 in strArray)
        //            {
        //                if (str.EndsWith(str2))
        //                {
        //                    flag = true;
        //                    break;
        //                }
        //            }
        //            if (flag)
        //            {
        //                string fileName = info2.Name.Trim().ToUpper();
        //                string[] strArray2 = folder.Split(new char[] { '\\' });
        //                string currentFolder = strArray2[strArray2.Length - 1].Trim().ToUpper();
        //                string str5 = (strArray2.Length > 1) ? strArray2[strArray2.Length - 2].Trim().ToUpper() : "";
        //                string str6 = folder + @"\" + info2.Name;
        //                DataRow dr = this.fileList.NewRow();
        //                dr["文件名"] = fileName;
        //                dr["完全路径"] = str6;
        //                dr["时间"] = this.currentDate.ToString("yyyy-MM-dd");
        //                if (!this.SetCellValue(ref dr, fileName, currentFolder, str5, folder))
        //                {
        //                    this.SetCellValue(ref dr, fileName.ToDBC(), currentFolder, str5, folder);
        //                }
        //                this.fileList.Rows.Add(dr);
        //            }
        //        }
        //        if (this.cbxChild.Checked)
        //        {
        //            Regex regex = new Regex(ConfigurationManager.AppSettings["floderReg"]);
        //            foreach (DirectoryInfo info3 in info.GetDirectories())
        //            {
        //                if (regex.IsMatch(info3.Name))
        //                {
        //                    try
        //                    {
        //                        this.currentDate = Convert.ToDateTime(this.txtYear.Text.Trim() + "-" + info3.Name.Replace(".", "-"));
        //                    }
        //                    catch
        //                    {
        //                        this.currentDate = DateTime.MinValue;
        //                    }
        //                }
        //                this.ScanFile(folder + @"\" + info3.Name);
        //            }
        //        }
        //    }
        //}

        private void ScanFolder_Load(object sender, EventArgs e)
        {
            this.prices = PriceInit.GetPrices();
            this.cbxCustomer.DataSource = (from x in this.prices select x.Customer).ToList<string>();
            this.cbxCustomer.SelectedIndex = 0;
        }

        private bool SetCellValue(ref DataRow dr, string fileName, string currentFolder, string currentFolder2, string folder)
        {
            string printType = fileName.GetPrintType();
            if (printType == "")
            {
                printType = currentFolder.GetPrintType();
            }
            if (printType == "")
            {
                printType = currentFolder2.GetPrintType();
            }
            if (printType == "")
            {
                printType = "写真";
            }
            double[] numArray = fileName.MatchLength();
            if (numArray[0] == 0.0)
            {
                numArray = currentFolder.MatchLength();
            }
            if (numArray[0] == 0.0)
            {
                numArray = currentFolder2.MatchLength();
            }
            if (printType == "喷绘")
            {

            }
            //if ((numArray[0] == 0.0) && this.cbx_AutoRead.Checked)
            //{
            //    numArray = this.MatchLength(folder + "/" + fileName);
            //}
            dr["长度"] = (numArray[0]).ToString();
            dr["宽度"] = (numArray[1]).ToString();
            double size = numArray[0] * numArray[1];
            dr["面积"] = size;
            int qty = 1;
            if (this.cbx3split.Checked && (numArray[2] > 0.0))
            {
                qty = Convert.ToInt32(numArray[2]);
            }
            else
            {
                qty = fileName.MatchQTY();
            }
            dr["数量"] = qty;
            dr["类型"] = printType;
            double unitPrice = this.GetUnitPrice(printType);
            dr["单价"] = unitPrice;
            double num4 = Math.Round(this.GetTotalAmount(qty, unitPrice, size, printType), 2);
            dr["总价"] = num4;
            if (num4 == 0.0)
            {
                return false;
            }
            return true;
        }

        private void SetRecountCount()
        {
            this.lblRecordCount.Text = (this.dataGridView1.DataSource as DataTable).Rows.Count.ToString();
            this.lblAmountCount.Text = (this.dataGridView1.DataSource as DataTable).AsEnumerable().Sum<DataRow>(((Func<DataRow, double>)(x => ((double)x["总价"])))).ToString();
        }

        private void CheckAuthentication()
        {
            string str = MachineUtil.GetNetCardMAC().ToUpper().Trim();
            if (!ConfigurationManager.AppSettings["MacAddressGroup"].Dencrypt().Contains(str))
            {
                if (ConfigurationManager.AppSettings["showMacAddress"].ToUpper().Trim() == "TRUE")
                {
                    MessageBox.Show(str);
                }
                else
                {
                    MessageBox.Show("该机器未授权，不能使用本软件！");
                }
                Environment.Exit(0);
            }
        }

        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("时间", typeof(DateTime));
            table.Columns.Add("文件名");
            table.Columns.Add("长度", typeof(double));
            table.Columns.Add("宽度", typeof(double));
            table.Columns.Add("面积", typeof(double));
            table.Columns.Add("类型");
            table.Columns.Add("数量", typeof(int));
            table.Columns.Add("单价", typeof(double));
            table.Columns.Add("总价", typeof(double));
            table.Columns.Add("完全路径");
            return table;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string filePath = this.dataGridView1.Rows[e.RowIndex].Cells["完全路径"].Value.ToString();
                this.OpenFileDir(filePath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                this.dataGridView1.Rows[rowIndex].Cells["面积"].Value = Convert.ToDouble(this.dataGridView1.Rows[rowIndex].Cells["长度"].Value) * Convert.ToDouble(this.dataGridView1.Rows[rowIndex].Cells["宽度"].Value);
                this.dataGridView1.Rows[rowIndex].Cells["总价"].Value = (((this.dataGridView1.Rows[rowIndex].Cells["类型"].Value.ToString() == "奖牌") ? 1.0 : Convert.ToDouble(this.dataGridView1.Rows[rowIndex].Cells["面积"].Value)) * Convert.ToDouble(this.dataGridView1.Rows[rowIndex].Cells["单价"].Value)) * Convert.ToDouble(this.dataGridView1.Rows[rowIndex].Cells["数量"].Value);
                this.SetRecountCount();
            }
            catch
            {
                MessageBox.Show("程序异常：是否输入错误数值");
            }
        }


        private double GetTotalAmount(int qty, double price, double size, string type)
        {
            if (type == "奖牌")
            {
                return (qty * price);
            }
            return ((qty * price) * size);
        }

        private double GetUnitPrice(string printType)
        {
            switch (printType)
            {
                case "写真":
                    return this.XiezhenPrice;

                case "UV":
                    return this.UVPrice;

                case "KT板":
                    return this.KTPrice;

                case "奖牌":
                    return this.JiangpaiPrice;

                case "灯片":
                    return this.DengpianPrice;

                case "单透":
                    return this.DantouPrice;

                case "喷绘":
                    return this.PenhuiPrice;

                case "黑胶":
                    return this.HeijiaoPrice;

                case "车贴":
                    return this.ChetiePrice;

                case "透明车贴":
                    return this.ToumingPrice;
            }
            return 0.0;
        }

        public void ImportFolder()
        {
            string folder = this.txtAddress.Text.Trim();
            this.ScanBind(folder);
            this.SetRecountCount();
        }
        public ScanFolder()
        {
            this.InitializeComponent();
            this.CheckAuthentication();
            this.InitTextBox();
            this.fileList = this.CreateTable();
        }

        private void BindFileList()
        {
            DataView defaultView = this.fileList.DefaultView;
            defaultView.Sort = "时间 Asc";
            this.dataGridView1.DataSource = defaultView.ToTable();
        }

        private void BindPrice()
        {
            string customerName = this.cbxCustomer.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(customerName))
            {
                PriceInfo info = (from x in this.prices
                                  where x.Customer == customerName
                                  select x).FirstOrDefault<PriceInfo>();
                this.txt_Penhui.Text = info.Penhui.ToString();
                this.txtDantou.Text = info.Dantou.ToString();
                this.txtDengpian.Text = info.Dengpian.ToString();
                this.txtHeijiao.Text = info.Heijiao.ToString();
                this.txtJiangpai.Text = info.Jiangpai.ToString();
                this.txtKT.Text = info.KTBan.ToString();
                this.txtUV.Text = info.UV.ToString();
                this.txtXiezhen.Text = info.Xiezhen.ToString();
                this.txtTouming.Text = info.Touming.ToString();
                this.txtChetie.Text = info.Chetie.ToString();
            }
        }

        private void btnExportFile_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataSource = this.dataGridView1.DataSource as DataTable;
                string saveFilePath = ExcelHelper.GetSaveFilePath(this.txtCustomer.Text.Trim() + this.txtYear.Text.Trim() + ".xls");
                string sheetName = (this.txtCustomer.Text.Trim() == "") ? this.txtYear.Text.Trim() : this.txtCustomer.Text.Trim();
                if (!string.IsNullOrEmpty(saveFilePath))
                {
                    ExcelHelper.ExportToExcel(dataSource, sheetName, saveFilePath);
                    this.OpenFileDir(saveFilePath);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                this.fileList.Clear();
                this.InitPrince();
                this.ImportFolder();
                this.dataGridView1.Columns["完全路径"].Visible = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + exception.StackTrace);
            }
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (lastpath != dialog.SelectedPath)
                {
                    dialog.SelectedPath = lastpath;
                }
                dialog.ShowDialog();
                this.txtAddress.Text = dialog.SelectedPath;
                lastpath = this.txtAddress.Text;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void cbxCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindPrice();
        }




    }
}
