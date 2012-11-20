using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmStrengthCalculation2 : Form
    {
        public int Grade = 1;
        public FrmStrengthCalculation2()
        {
            InitializeComponent();
        }

        ShaftStrength Shaft = new ShaftStrength();

        public FrmStrengthCalculation2(int i)
        {
            Grade = i;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");

            Shaft.Faz = double.Parse(xe.Element("faz").Value);
            Shaft.Fa1 = double.Parse(xe.Element("fa1").Value);
            Shaft.r1 = double.Parse(xe.Element("r1").Value);
            Shaft.Fbz = double.Parse(xe.Element("fbz").Value);
            Shaft.Fa2 = double.Parse(xe.Element("fa2").Value);
            Shaft.r2 = double.Parse(xe.Element("r2").Value);
            Shaft.Fay = double.Parse(xe.Element("fay").Value);
            Shaft.Fby = double.Parse(xe.Element("fby").Value);
            Shaft.L2 = double.Parse(xe.Element("L2").Value);
            Shaft.L3 = double.Parse(xe.Element("L3").Value);
            Shaft.L4 = double.Parse(xe.Element("L4").Value);
            //C-C 水平面 垂直面 合成弯矩
            txtMczL.Text = Shaft.ShaftMczL2().ToString("f4");
            txtMczR.Text = Shaft.ShaftMczR2().ToString("f4");

            txtMcy.Text = Shaft.ShaftMcy2().ToString("f4");

            txtMcL.Text = Shaft.SyntheticMoment(double.Parse(txtMczL.Text), double.Parse(txtMcy.Text)).ToString("f4");
            txtMcR.Text = Shaft.SyntheticMoment(double.Parse(txtMczR.Text), double.Parse(txtMcy.Text)).ToString("f4");

            //D-D水平面 垂直面 合成弯矩
            txtMdzR.Text = Shaft.ShaftMdzR2().ToString("f4");
            txtMdzL.Text = Shaft.ShaftMdzL2().ToString("f4");

            txtMdy.Text = Shaft.ShaftMdy2().ToString("f4");

            txtMdL.Text = Shaft.SyntheticMoment(double.Parse(txtMdzL.Text), double.Parse(txtMdy.Text)).ToString("f4");
            txtMdR.Text = Shaft.SyntheticMoment(double.Parse(txtMdzR.Text), double.Parse(txtMdy.Text)).ToString("f4");

        }

        private void button2_Click(object sender, EventArgs e)
        {  //cc左侧弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("df1").Value); //需要引用齿轮轴2的齿根直径 
            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMcL.Text);//剖面c左侧合成弯矩
            double t = 0;
            txtBendScl.Text = Shaft.CircleSecBendStre(d, alpha, m, t).ToString("f4");
        }

        private void button8_Click(object sender, EventArgs e)
        {   //C-C右侧弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("df1").Value); //需要引用齿轮轴2的齿根直径 
            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMcR.Text);//剖面C右侧合成弯矩
            double t = double.Parse(txtT.Text);//转矩
            txtBendScr.Text = Shaft.CircleSecBendStre(d, alpha, m, t).ToString("f4");
        }

        private void button4_Click(object sender, EventArgs e)
        {      //D-D剖面左侧弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("轴段5直径").Value); //需要引用齿轮轴2的齿根直径 
            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMdL.Text);//剖面dzuo侧合成弯矩
            double t = double.Parse(txtT.Text);
            double b = double.Parse(xe.Element("键槽b").Value);
            double ht = double.Parse(xe.Element("键槽t1").Value);
            txtBendSdl.Text = Shaft.SingleGroBendStre(d, alpha, m, t, b, ht).ToString("f4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //D-D剖面右侧弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("轴段5直径").Value); //需要引用齿轮轴2的齿根直径 
            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMdR.Text);//剖面d右侧合成弯矩
            double t = 0;
            double b = double.Parse(xe.Element("键槽b").Value);
            double ht = double.Parse(xe.Element("键槽t1").Value);
            txtBendSdr.Text = Shaft.SingleGroBendStre(d, alpha, m, t, b, ht).ToString("f4");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Form15 frm = new Form15();
            //frm.Show();

        }

        private void Form14_Load(object sender, EventArgs e)
        {
            XElement xe = XElement.Load(@"D:\减速器设计系统\第二级结构参数.xml");

            IEnumerable<XElement> elements = from 转速 in xe.Elements("动力参数")
                                             select 转速;
            txtT.Text = elements.ElementAt(0).Element("扭矩").Value;
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("强度计算");
            xe.Element("CD处扭矩").Value = txtT.Text;
            xe.Element("折合系数").Value = cmbBoxAlpha.Text;
            xe.Element("C剖面左侧弯曲应力").Value = txtBendScl.Text;
            xe.Element("C剖面右侧弯曲应力").Value = txtBendScr.Text;
            xe.Element("D剖面左侧弯曲应力").Value = txtBendSdl.Text;
            xe.Element("D剖面右侧弯曲应力").Value = txtBendSdr.Text;

            XElement childXe = xe.Element("D剖面弯矩");
            childXe.Element("水平面左侧").Value = txtMdzL.Text;
            childXe.Element("水平面右侧").Value = txtMdzR.Text;
            childXe.Element("垂直面").Value = txtMdy.Text;
            childXe.Element("左侧合成").Value = txtMdL.Text;
            childXe.Element("右侧合成").Value = txtMdR.Text;

            childXe = xe.Element("C剖面弯矩");
            childXe.Element("水平面左侧").Value = txtMczL.Text;
            childXe.Element("水平面右侧").Value = txtMczR.Text;
            childXe.Element("垂直面").Value = txtMcy.Text;
            childXe.Element("左侧合成").Value = txtMcL.Text;
            childXe.Element("右侧合成").Value = txtMcR.Text;

            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }

       

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
