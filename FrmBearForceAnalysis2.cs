using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmBearForceAnalysis2 : Form
    {
        public int Grade = 1;
        public FrmBearForceAnalysis2()
        {
            InitializeComponent();
        }

        public FrmBearForceAnalysis2(int i)
        {
            Grade = i;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //轴承支反力计算
            BearingReaFor BRF = new BearingReaFor();
            BRF.Ft1 = double.Parse(txtFt1.Text);
            BRF.Fr1 = double.Parse(txtFr1.Text);
            BRF.Fa1 = double.Parse(txtFa1.Text);
            BRF.r1 = double.Parse(txtR1.Text);

            BRF.Ft2 = double.Parse(txtFt2.Text);
            BRF.Fr2 = double.Parse(txtFr2.Text);
            BRF.Fa2 = double.Parse(txtFa2.Text);
            BRF.r2 = double.Parse(txtR2.Text);


            BRF.L2 = double.Parse(txtL2.Text);
            BRF.L3 = double.Parse(txtL3.Text);
            BRF.L4 = double.Parse(txtL4.Text);

            txtFaz.Text = BRF.BearingFAz2().ToString("f4");
            txtFay.Text = BRF.BearingFAy2().ToString("f4");


            txtFbz.Text = BRF.BearingFBz2().ToString("f4");
            txtFby.Text = BRF.BearingFBy2().ToString("f4");

            if (cmbBoxBear.Text == "A固定B浮动")
            {
                double a = 0;
                txtFbx.Text = a.ToString("f4");
                txtFax.Text = (double.Parse(txtFa1.Text) - double.Parse(txtFa2.Text)).ToString("f4");
            }
            else
            {
                double a = 0;
                txtFax.Text = a.ToString("f4");
                txtFbx.Text = (double.Parse(txtFa1.Text) - double.Parse(txtFa2.Text)).ToString("f4");
            }
            //计算径向合力
            txtFra.Text = Math.Sqrt(Math.Pow(double.Parse(txtFaz.Text), 2) + Math.Pow(double.Parse(txtFay.Text), 2)).ToString("f4");
            txtFrb.Text = Math.Sqrt(Math.Pow(double.Parse(txtFbz.Text), 2) + Math.Pow(double.Parse(txtFby.Text), 2)).ToString("f4");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Form14 frm = new Form14();
            //frm.Show();

        }

        private void Form9_Load(object sender, EventArgs e)
        {

            // int iGrade = Convert.ToInt32(SystemConstants.XmlProject.Element("总体要求").Element("级数").Value);
            #region 计算齿轮1受力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数");
            double t1 = double.Parse(xe.Element("扭矩").Value);

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double beta1 = double.Parse(xe.Element("B1").Value);//螺旋角
            double alpha1 = double.Parse(xe.Element("alpha").Value);
            double d1 = double.Parse(xe.Element("d1").Value);
            txtD.Text = double.Parse(xe.Element("轴段1直径").Value).ToString("f4");

            double ft1 = 2 * t1 / d1;//圆周力
            txtFt1.Text = ft1.ToString("f4");
            txtFr1.Text = (ft1 * Math.Tan(alpha1 * Math.PI / 180) / Math.Cos(beta1 * Math.PI / 180)).ToString("f4");
            txtFa1.Text = (ft1 * Math.Tan(beta1 * Math.PI / 180)).ToString("f4");
            txtR1.Text = (d1 / 2).ToString("f4");

            #endregion
            #region 计算齿轮2受力
            int temp = Grade - 1;
            xe = SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("动力参数");
            double t2 = double.Parse(xe.Element("扭矩").Value);
            xe = SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("齿轮轴");

            double beta2 = double.Parse(xe.Element("B1").Value);//螺旋角
            double alpha2 = double.Parse(xe.Element("alpha").Value);
            double d2 = double.Parse(xe.Element("d1").Value);

            double ft2 = 2 * t2 / d2;//圆周力
            txtFt2.Text = ft2.ToString("f4");
            txtFr2.Text = (ft2 * Math.Tan(alpha2 * Math.PI / 180) / Math.Cos(beta2 * Math.PI / 180)).ToString("f4");
            txtFa2.Text = (ft2 * Math.Tan(beta2 * Math.PI / 180)).ToString("f4");
            txtR2.Text = (double.Parse(SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("齿轮").Element("d2").Value) / 2).ToString("f4");

            #endregion

        }

        private void txtBearB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                XElement xe = XElement.Load(@"D:\减速器设计系统\第二级结构参数.xml");

                IEnumerable<XElement> elements = from Z1 in xe.Elements("齿轮轴")
                                                 select Z1;
                double c2 = double.Parse(elements.ElementAt(0).Element("轴段2长度").Value);
                double c3 = double.Parse(elements.ElementAt(0).Element("B").Value);
                double c4 = double.Parse(elements.ElementAt(0).Element("轴段4长度").Value);

                xe = XElement.Load(@"D:\减速器设计系统\第一级结构参数.xml");
                elements = from Z2 in xe.Elements("齿轮")
                           select Z2;
                double b2 = double.Parse(elements.ElementAt(0).Element("B").Value);

                //计算内箱体宽度
                elements = from Z1 in xe.Elements("齿轮轴")
                           select Z1;
                double first2 = double.Parse(elements.ElementAt(0).Element("轴段2长度").Value);
                double first3 = double.Parse(elements.ElementAt(0).Element("B").Value);
                double first4 = double.Parse(elements.ElementAt(0).Element("轴段4长度").Value);
                double bearB = double.Parse(txtBearB.Text);
                txtL4.Text = (first2 + first3 + first4 + bearB).ToString("f4");

                double l2 = bearB / 2 + c2 + c3 / 2;
                txtL2.Text = l2.ToString("f4");
                txtL3.Text = (l2 + c3 / 2 + c4 + b2 / 2).ToString("f4");

            }
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");
           
            xe.Element("轴承配置").Value = cmbBoxBear.Text;
            xe.Element("L2").Value = txtL2.Text;
            xe.Element("L3").Value = txtL3.Text;
            xe.Element("L4").Value = txtL4.Text;
            xe.Element("r1").Value = txtR1.Text;
            xe.Element("ft1").Value = txtFt1.Text;
            xe.Element("fr1").Value = txtFr1.Text;
            xe.Element("fa1").Value = txtFa1.Text;
            xe.Element("r2").Value = txtR2.Text;
            xe.Element("ft2").Value = txtFt2.Text;
            xe.Element("fr2").Value = txtFr2.Text;
            xe.Element("fa2").Value = txtFa2.Text;
            xe.Element("faz").Value = txtFaz.Text;
            xe.Element("fay").Value = txtFay.Text;
            xe.Element("fax").Value = txtFax.Text;
            xe.Element("A径向力").Value = txtFra.Text;
            xe.Element("fbz").Value = txtFbz.Text;
            xe.Element("fby").Value = txtFby.Text;
            xe.Element("fbx").Value = txtFbx.Text;
            xe.Element("B径向力").Value = txtFrb.Text;

            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }

        private void btCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
