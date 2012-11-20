using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class Form11 : Form
    {
        public int Grade = 0;

        public Form11()
        {
            InitializeComponent();
        }

        public Form11(int i)
        {
            Grade = i;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BearingReaFor BRF = new BearingReaFor();
            BRF.Fez = double.Parse(txtFez.Text);
            BRF.Fey = double.Parse(txtFey.Text);

            BRF.Ft2 = double.Parse(txtFt.Text);
            BRF.Fr2 = double.Parse(txtFr.Text);
            BRF.Fa2 = double.Parse(txtFa.Text);

            BRF.Fhz = double.Parse(txtFhz.Text);
            BRF.Fhy = double.Parse(txtFhy.Text);


            BRF.L1 = double.Parse(txtL1.Text);

            BRF.L3 = double.Parse(txtL3.Text);
            BRF.L4 = double.Parse(txtL4.Text);
            BRF.L5 = double.Parse(txtL5.Text);

            BRF.r2 = double.Parse(txtR.Text);


            txtFaz.Text = BRF.BearingFAz4().ToString("f4");
            txtFay.Text = BRF.BearingFAy4().ToString("f4");

            txtFbz.Text = BRF.BearingFBz4().ToString("f4");
            txtFby.Text = BRF.BearingFBy4().ToString("f4");
            if (cmbBoxBear.Text == "A固定B浮动")
            {
                double a = 0;
                txtFbx.Text = a.ToString("f4");
                txtFax.Text =
                    (-double.Parse(txtFa.Text) - double.Parse(txtFex.Text) - double.Parse(txtFhx.Text)).ToString("f4");
            }
            else
            {
                double a = 0;
                txtFax.Text = a.ToString("f4");
                txtFbx.Text =
                    (-double.Parse(txtFa.Text) - double.Parse(txtFex.Text) - double.Parse(txtFhx.Text)).ToString("f4");
            }

            //径向合力
            txtFra.Text =
                Math.Sqrt(Math.Pow(double.Parse(txtFay.Text), 2) + Math.Pow(double.Parse(txtFaz.Text), 2)).ToString("f4");
            txtFrb.Text =
                Math.Sqrt(Math.Pow(double.Parse(txtFby.Text), 2) + Math.Pow(double.Parse(txtFbz.Text), 2)).ToString("f4");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Form19 frm = new Form19();
            //frm.Show();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            int temp = Grade - 1;

            XElement xe = SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("动力参数");
            double t = double.Parse(xe.Element("扭矩").Value);

            xe = SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("齿轮轴");
            double beta = double.Parse(xe.Element("B1").Value); //螺旋角
            double alpha = double.Parse(xe.Element("alpha").Value);
            double d = double.Parse(xe.Element("d1").Value);
            double ft = 2*t/d; //圆周力

            txtFt.Text = ft.ToString("f4");
            txtFr.Text = (ft*Math.Tan(alpha*Math.PI/180)/Math.Cos(beta*Math.PI/180)).ToString("f4");
            txtFa.Text = (ft*Math.Tan(beta*Math.PI/180)).ToString("f4");

            xe = SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("齿轮");
            txtR.Text = (double.Parse(xe.Element("d2").Value)/2).ToString("f4");

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            txtD.Text = xe.Element("轴段2直径").Value;
        }

        private void txtBearB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                #region 查询输出轴段长度

                XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");

                double c1 = double.Parse(xe.Element("轴段1长度").Value);
                double c2 = double.Parse(xe.Element("轴段2长度").Value);
                double c3 = double.Parse(xe.Element("轴段3长度").Value);
                double c4 = double.Parse(xe.Element("轴段4长度").Value);
                double c5 = double.Parse(xe.Element("轴段5长度").Value);
                double c6 = double.Parse(xe.Element("轴段6长度").Value);
                double c7 = double.Parse(xe.Element("轴段7长度").Value);

                int temp = Grade - 1;
                xe = SystemConstants.XmlProject.Element("第" + temp + "级轴串").Element("齿轮");
                double b = double.Parse(xe.Element("B").Value);

                #endregion

                #region 计算内箱宽

                xe = SystemConstants.XmlProject.Element("第1级轴串").Element("齿轮轴");
                double first2 = double.Parse(xe.Element("轴段2长度").Value);
                double first3 = double.Parse(xe.Element("B").Value);
                double first4 = double.Parse(xe.Element("轴段4长度").Value);

                #endregion

                double bearb = double.Parse(txtBearB.Text);
                txtL1.Text = (c1/2 + c2 - bearb/2).ToString("f4");
                txtL3.Text = (bearb/2 + c3 + c4 + b/2).ToString("f4");
                txtL4.Text = (first2 + first3 + first4 + bearb).ToString("f4");
                txtL5.Text = (c7/2 + c6 - (first2 + first3 + first4 - (c3 + c4 + c5)) - bearb/2).ToString("f4");
            }
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");
            xe.Element("轴承配置").Value = cmbBoxBear.Text;
            xe.Element("L1").Value = txtL1.Text;
            xe.Element("L3").Value = txtL3.Text;
            xe.Element("L4").Value = txtL4.Text;
            xe.Element("L5").Value = txtL5.Text;
            xe.Element("r").Value = txtR.Text;
            xe.Element("T1").Value = txtT1.Text;
            xe.Element("T2").Value = txtT2.Text;
            xe.Element("fez").Value = txtFez.Text;
            xe.Element("fey").Value = txtFey.Text;
            xe.Element("fex").Value = txtFex.Text;
            xe.Element("ft").Value = txtFt.Text;
            xe.Element("fr").Value = txtFr.Text;
            xe.Element("fa").Value = txtFa.Text;
            xe.Element("fhz").Value = txtFhz.Text;
            xe.Element("fhy").Value = txtFhy.Text;
            xe.Element("fhx").Value = txtFhx.Text;
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
