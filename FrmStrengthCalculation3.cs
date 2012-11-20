using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmStrengthCalculation3 : Form
    {
        public int Grade = 0;
        public FrmStrengthCalculation3()
        {
            InitializeComponent();
        }
        ShaftStrength Shaft = new ShaftStrength();

        public FrmStrengthCalculation3(int i)
        {
            Grade = i;
            InitializeComponent(); ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XElement xe = XElement.Load(@"D:\减速器设计系统\中间轴2计算参数.xml");

            IEnumerable<XElement> elements = from 轴承配置 in xe.Elements("受力计算参数")
                                             select 轴承配置;

            Shaft.Faz = double.Parse(elements.ElementAt(0).Element("faz").Value);
            Shaft.Fa1 = double.Parse(elements.ElementAt(0).Element("fa1").Value);
            Shaft.r1 = double.Parse(elements.ElementAt(0).Element("r1").Value);
            Shaft.Fbz = double.Parse(elements.ElementAt(0).Element("fbz").Value);
            Shaft.Fa2 = double.Parse(elements.ElementAt(0).Element("fa2").Value);
            Shaft.r2 = double.Parse(elements.ElementAt(0).Element("r2").Value);
            Shaft.Fay = double.Parse(elements.ElementAt(0).Element("fay").Value);
            Shaft.Fby = double.Parse(elements.ElementAt(0).Element("fby").Value);
            Shaft.L2 = double.Parse(elements.ElementAt(0).Element("L2").Value);
            Shaft.L3 = double.Parse(elements.ElementAt(0).Element("L3").Value);
            Shaft.L4 = double.Parse(elements.ElementAt(0).Element("L4").Value);
            //C-C 水平面 垂直面 合成弯矩
            txtMczL.Text = Shaft.ShaftMczL3().ToString();
            txtMczR.Text = Shaft.ShaftMczR3().ToString();

            txtMcy.Text = Shaft.ShaftMcy3().ToString();

            txtMcL.Text = Shaft.SyntheticMoment(double.Parse(txtMczL.Text), double.Parse(txtMcy.Text)).ToString();
            txtMcR.Text = Shaft.SyntheticMoment(double.Parse(txtMczR.Text), double.Parse(txtMcy.Text)).ToString();

            //D-D水平面 垂直面 合成弯矩
            txtMdzR.Text = Shaft.ShaftMdzR3().ToString();
            txtMdzL.Text = Shaft.ShaftMdzL3().ToString();

            txtMdy.Text = Shaft.ShaftMdy3().ToString();

            txtMdL.Text = Shaft.SyntheticMoment(double.Parse(txtMdzL.Text), double.Parse(txtMdy.Text)).ToString();
            txtMdR.Text = Shaft.SyntheticMoment(double.Parse(txtMdzR.Text), double.Parse(txtMdy.Text)).ToString();





        }

        private void button2_Click(object sender, EventArgs e)
        {
            //C-C左侧弯曲应力
            XElement xe = XElement.Load(@"D:\减速器设计系统\第三级结构参数.xml");

            IEnumerable<XElement> elements = from Z1 in xe.Elements("齿轮轴")
                                             select Z1;
            double d = double.Parse(elements.ElementAt(0).Element("轴段5直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMcL.Text);//剖面d侧合成弯矩
            double t = 0;

            double b = double.Parse(elements.ElementAt(0).Element("键槽b").Value);//键槽尺寸
            double ht = double.Parse(elements.ElementAt(0).Element("键槽t1").Value); ;//键槽尺寸
            txtBendScl.Text = Shaft.SingleGroBendStre(d, alpha, m, t, b, ht).ToString();


        }

        private void button8_Click(object sender, EventArgs e)
        {
            //c-c右侧弯曲应力
            XElement xe = XElement.Load(@"D:\减速器设计系统\第三级结构参数.xml");

            IEnumerable<XElement> elements = from Z1 in xe.Elements("齿轮轴")
                                             select Z1;
            double d = double.Parse(elements.ElementAt(0).Element("轴段5直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMcR.Text);
            double t = double.Parse(txtT.Text);
            double b = double.Parse(elements.ElementAt(0).Element("键槽b").Value);//键槽尺寸
            double ht = double.Parse(elements.ElementAt(0).Element("键槽t1").Value); ;//键槽尺寸
            txtBendScr.Text = Shaft.SingleGroBendStre(d, alpha, m, t, b, ht).ToString();



        }

        private void button4_Click(object sender, EventArgs e)
        {
            //d-d左侧弯曲应力
            XElement xe = XElement.Load(@"D:\减速器设计系统\第三级结构参数.xml");

            IEnumerable<XElement> elements = from Z1 in xe.Elements("齿轮轴")
                                             select Z1;
            double d = double.Parse(elements.ElementAt(0).Element("df1").Value); //齿根圆
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMdL.Text);
            double t = double.Parse(txtT.Text);
            txtBendSdl.Text = Shaft.CircleSecBendStre(d, alpha, m, t).ToString();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            //d-d右侧弯曲应力
            XElement xe = XElement.Load(@"D:\减速器设计系统\第三级结构参数.xml");

            IEnumerable<XElement> elements = from Z1 in xe.Elements("齿轮轴")
                                             select Z1;
            double d = double.Parse(elements.ElementAt(0).Element("df1").Value); //齿根圆
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMdR.Text);
            double t = 0;
            txtBendSdr.Text = Shaft.CircleSecBendStre(d, alpha, m, t).ToString();

        }

       

        private void Form16_Load(object sender, EventArgs e)
        {
            XElement xe = XElement.Load(@"D:\减速器设计系统\第三级结构参数.xml");

            IEnumerable<XElement> elements = from 转速 in xe.Elements("动力参数")
                                             select 转速;
            txtT.Text = elements.ElementAt(0).Element("扭矩").Value; 
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = XElement.Load(@"D:\减速器设计系统\中间轴2计算参数.xml");

            IEnumerable<XElement> elements = from 水平面左侧 in xe.Elements("强度计算").Elements("D剖面弯矩")
                                             select 水平面左侧;
            elements.ElementAt(0).Element("水平面左侧").Value = txtMdzL.Text;
            elements.ElementAt(0).Element("水平面右侧").Value = txtMdzR.Text;
            elements.ElementAt(0).Element("垂直面").Value = txtMdy.Text;
            elements.ElementAt(0).Element("左侧合成").Value = txtMdL.Text;
            elements.ElementAt(0).Element("右侧合成").Value = txtMdR.Text;
            elements = from 水平面左侧 in xe.Elements("强度计算").Elements("C剖面弯矩")
                       select 水平面左侧;
            elements.ElementAt(0).Element("水平面左侧").Value = txtMczL.Text;
            elements.ElementAt(0).Element("水平面右侧").Value = txtMczR.Text;
            elements.ElementAt(0).Element("垂直面").Value = txtMcy.Text;
            elements.ElementAt(0).Element("左侧合成").Value = txtMcL.Text;
            elements.ElementAt(0).Element("右侧合成").Value = txtMcR.Text;

            elements = from CD处扭矩 in xe.Elements("强度计算")
                       select CD处扭矩;
            elements.ElementAt(0).Element("CD处扭矩").Value = txtT.Text;
            elements.ElementAt(0).Element("折合系数").Value = cmbBoxAlpha.Text;
            elements.ElementAt(0).Element("C剖面左侧弯曲应力").Value = txtBendScl.Text;
            elements.ElementAt(0).Element("C剖面右侧弯曲应力").Value = txtBendScr.Text;
            elements.ElementAt(0).Element("D剖面左侧弯曲应力").Value = txtBendSdl.Text;
            elements.ElementAt(0).Element("D剖面右侧弯曲应力").Value = txtBendSdr.Text;
            xe.Save(@"D:\减速器设计系统\中间轴2计算参数.xml");

        }

       

        private void button6_Click(object sender, EventArgs e)
        {
            //InterferenceConnection3 frm = new InterferenceConnection3();
            //frm.Show();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
