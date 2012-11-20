using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmBearForceAnalysis1 : Form
    {
        public int Grade = 1;
        public FrmBearForceAnalysis1()
        {
            InitializeComponent();
        }

        public FrmBearForceAnalysis1(int i)
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

            BRF.L1 = double.Parse(txtL1.Text);
            BRF.L3 = double.Parse(txtL3.Text);
            BRF.L4 = double.Parse(txtL4.Text);

            BRF.r2 = double.Parse(txtR.Text);

            if (cmbBoxBear.Text == "A固定B浮动")
            {
                double a = 0;
                txtFbx.Text = a.ToString("f4");
                txtFax.Text = (double.Parse(txtFa.Text) - double.Parse(txtFex.Text)).ToString("f4");
            }
            else
            {
                double a = 0;
                txtFax.Text = a.ToString("f4");
                txtFbx.Text = (double.Parse(txtFa.Text) - double.Parse(txtFex.Text)).ToString("f4");
            }

            txtFaz.Text = BRF.BearingFAz1().ToString("f4");
            txtFay.Text = BRF.BearingFAy1().ToString("f4");

            txtFbz.Text = BRF.BearingFBz1().ToString("f4");
            txtFby.Text = BRF.BearingFBy1().ToString("f4");

            //计算径向合力
            txtFra.Text = Math.Sqrt(Math.Pow(double.Parse(txtFaz.Text), 2) + Math.Pow(double.Parse(txtFay.Text), 2)).ToString("f4");
            txtFrb.Text = Math.Sqrt(Math.Pow(double.Parse(txtFbz.Text), 2) + Math.Pow(double.Parse(txtFby.Text), 2)).ToString("f4");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            FrmStrengthCalculation1 frm = new FrmStrengthCalculation1();
            frm.Show();




        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
           // int iGrade = Convert.ToInt32(SystemConstants.XmlProject.Element("总体要求").Element("级数").Value);

            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");

            xe.Element("轴承配置").Value = cmbBoxBear.Text;
            xe.Element("L1").Value = txtL1.Text;
            xe.Element("L3").Value = txtL3.Text;
            xe.Element("L4").Value = txtL4.Text;
            xe.Element("r").Value = txtR.Text;
            xe.Element("T").Value = txtT.Text;
            xe.Element("fez").Value = txtFez.Text;
            xe.Element("fey").Value = txtFey.Text;
            xe.Element("fex").Value = txtFex.Text;
            xe.Element("ft").Value = txtFt.Text;
            xe.Element("fr").Value = txtFr.Text;
            xe.Element("fa").Value = txtFa.Text;
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

        private void Form8_Load(object sender, EventArgs e)
        {
           // int iGrade = Convert.ToInt32(SystemConstants.XmlProject.Element("总体要求").Element("级数").Value);

            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数");
            double t = double.Parse(xe.Element("扭矩").Value);

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double beta = double.Parse(xe.Element("B1").Value);//螺旋角
            double alpha = double.Parse(xe.Element("alpha").Value);
            double d = double.Parse(xe.Element("d1").Value);
            txtD.Text = double.Parse(xe.Element("轴段5直径").Value).ToString("f4");
            txtT.Text = t.ToString("f4");
            double ft = 2 * t / d;//圆周力
            txtFt.Text = ft.ToString("f4");
            txtFr.Text = (ft * Math.Tan(alpha * Math.PI / 180) / Math.Cos(beta * Math.PI / 180)).ToString("f4");
            txtFa.Text = (ft * Math.Tan(beta * Math.PI / 180)).ToString("f4");
        }


        private void txtBearB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //int iGrade = Convert.ToInt32(SystemConstants.XmlProject.Element("总体要求").Element("级数").Value);
                XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");

                double c2 = double.Parse(xe.Element("轴段2长度").Value);
                double c3 = double.Parse(xe.Element("B").Value);
                double c4 = double.Parse(xe.Element("轴段4长度").Value);
                double c5 = double.Parse(xe.Element("轴段5长度").Value);
                double c6 = double.Parse(xe.Element("轴段6长度").Value);
                double r = double.Parse(xe.Element("d1").Value) / 2;
                double bearB = double.Parse(txtBearB.Text);//轴承宽度
                txtL1.Text = (c6 / 2 + c5 - bearB / 2).ToString("f4");
                txtL3.Text = (bearB / 2 + c4 + c3 / 2).ToString("f4");
                txtL4.Text = (c2 + c3 + c4 + bearB).ToString("f4");
                txtR.Text = r.ToString("f4");

                button1.Enabled = true;
                btSaveData.Enabled = true;
            }
        }

        private void btCancle_Click(object sender, EventArgs e)
        {
            //InputPartForm frm = new InputPartForm();
            //frm.Show();
            this.Close();
        }
        

        //private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.OKCancel);
        //    if (result == DialogResult.OK)
        //    {
        //        Application.ExitThread();
        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }

        //}


    }
}
