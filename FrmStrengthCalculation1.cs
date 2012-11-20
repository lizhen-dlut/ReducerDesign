using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmStrengthCalculation1 : Form
    {
        private int Grade;
        ShaftStrength ss = new ShaftStrength();

        public FrmStrengthCalculation1()
        {
            InitializeComponent();
        }

        public FrmStrengthCalculation1(int i)
        {
            Grade = i;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");

            ss.Fez = double.Parse(xe.Element("fez").Value);
            ss.Fex = double.Parse(xe.Element("fex").Value);
            ss.Fey = double.Parse(xe.Element("fey").Value);

            ss.Ft2 = double.Parse(xe.Element("ft").Value);
            ss.Fr2 = double.Parse(xe.Element("fr").Value);
            ss.Fa2 = double.Parse(xe.Element("fa").Value);
            ss.Faz = double.Parse(xe.Element("faz").Value);
            ss.Fay = double.Parse(xe.Element("fay").Value);
            ss.Fax = double.Parse(xe.Element("fax").Value);
            ss.Fbz = double.Parse(xe.Element("fbz").Value);
            ss.Fby = double.Parse(xe.Element("fby").Value);
            ss.Fbx = double.Parse(xe.Element("fbx").Value);
            ss.L1 = double.Parse(xe.Element("L1").Value);
            ss.L4 = double.Parse(xe.Element("L4").Value);
            ss.L3 = double.Parse(xe.Element("L3").Value);
            ss.r2 = double.Parse(xe.Element("r").Value);

            txtT.Text = double.Parse(xe.Element("T").Value).ToString("f4");

            //D-D水平面  垂直面分弯矩
            txtMdzR.Text = ss.ShaftMdzR1().ToString("f4");
            txtMdzL.Text = ss.ShaftMdzL1().ToString("f4");
            txtMdy.Text = ss.ShaftMdy1().ToString("f4");

            //A-A水平面  垂直面分弯矩
            txtMaz.Text = ss.ShaftMaz1().ToString("f4");
            txtMay.Text = ss.ShaftMay1().ToString("f4");

            //D-D合成弯矩
            txtMdR.Text = ss.SyntheticMoment(double.Parse(txtMdzR.Text), double.Parse(txtMdy.Text)).ToString("f4");
            txtMdL.Text = ss.SyntheticMoment(double.Parse(txtMdzL.Text), double.Parse(txtMdy.Text)).ToString("f4");
            
            //A-A 合成弯矩
            txtMa.Text = ss.SyntheticMoment(double.Parse(txtMaz.Text), double.Parse(txtMay.Text)).ToString("f4");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
           
            double d = double.Parse(xe.Element("轴段5直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMa.Text);//剖面A合成弯矩
            double t = double.Parse(txtT.Text);//转矩
            if (m == 0)
            {
                txtBendSa.Text = ss.CircleSecTorStre(d, t).ToString("f4");//此时为扭转应力
            }
            else
            {
                txtBendSa.Text = ss.CircleSecBendStre(d, alpha, m, t).ToString("f4");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {   //剖面D左侧弯曲应力计算
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");

            double d = double.Parse(xe.Element("df1").Value);

            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMdL.Text);

            double t = double.Parse(txtT.Text);//转矩
            txtBendSdl.Text = ss.CircleSecBendStre(d, alpha, m, t).ToString("f4");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("df1").Value);

            double alpha = double.Parse(cmbBoxAlpha.Text);//折合系数
            double m = double.Parse(txtMdR.Text);//剖面D右侧
            double t = 0;
            txtBendSdr.Text = ss.CircleSecBendStre(d, alpha, m, t).ToString("f4");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FrmBearingLife frm = new FrmBearingLife();
            frm.Show();

        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("强度计算");

            XElement childXe = xe.Element("D剖面弯矩");
            childXe.Element("水平面左侧").Value = txtMdzL.Text;
            childXe.Element("水平面右侧").Value = txtMdzR.Text;
            childXe.Element("垂直面").Value = txtMdy.Text;
            childXe.Element("左侧合成").Value = txtMdL.Text;
            childXe.Element("右侧合成").Value = txtMdR.Text;

            childXe = xe.Element("A剖面弯矩");
            childXe.Element("水平面").Value = txtMaz.Text;
            childXe.Element("垂直面").Value = txtMay.Text;
            childXe.Element("合成").Value = txtMa.Text;
            
            xe.Element("EA处扭矩").Value = txtT.Text;
            xe.Element("折合系数").Value = cmbBoxAlpha.Text;
            xe.Element("A剖面弯曲应力").Value = txtBendSa.Text;
            xe.Element("D剖面左侧弯曲应力").Value = txtBendSdl.Text;
            xe.Element("D剖面右侧弯曲应力").Value = txtBendSdr.Text;

            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }


        private void btKeyForm_Click(object sender, EventArgs e)
        {
            //KeyStrength1 frm = new KeyStrength1();
            //frm.Show();
        }

        private void btInterferenceForm_Click(object sender, EventArgs e)
        {
            //InterferenceConnection1 frm = new InterferenceConnection1();
            //frm.Show();

        }

        private void FrmStrengthCalculation_Load(object sender, EventArgs e)
        {
            int iGrade = Convert.ToInt32(SystemConstants.XmlProject.Element("总体要求").Element("级数").Value);
            XElement xe = SystemConstants.XmlProject.Element("第" + iGrade + "级轴串").Element("动力参数");
            txtT.Text = double.Parse(xe.Element("扭矩").Value).ToString("f4");
        }
    }
}
