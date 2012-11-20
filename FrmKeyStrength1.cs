using System;
using System.Windows.Forms;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmKeyStrength1 : Form
    {
        public int Grade = 1;

        public double T = 0.0;
        public double D = 0.0;
        public double B = 0.0;
        public double H = 0.0;
        public double L = 0.0;

        public FrmKeyStrength1()
        {
            InitializeComponent();
        }

        public FrmKeyStrength1(int i,double t,double d,double b,double h,double l)
        {
            Grade = i;
            T = t;
            D = d;
            B = b;
            H = h;
            L = l;
            InitializeComponent();
        }

        private void btKeyCalculation_Click(object sender, EventArgs e)
        {
            double t = double.Parse(txtT.Text);
            double d = double.Parse(txtD.Text);
            double k = double.Parse(txtK.Text);
            double l = double.Parse(txtL.Text);
            txtSigma.Text = (2 * t / (d * k * l)).ToString("f4");

        }

        private void KeyStrength1_Load(object sender, EventArgs e)
        {
            //double t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数").Element("扭矩").Value);
            txtT.Text = T.ToString("f4");

            //t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("轴段6直径").Value);
            txtD.Text =     D.ToString("f4");

            //double b = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽b").Value);
            //double h = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽h").Value); //键宽
            //double L1 = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽L").Value);//键长度
            txtK.Text = (H / 2).ToString("f4");
            txtL.Text = (L - B).ToString("f4");
            txtL.Text = (L - B).ToString("f4");
        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("键计算").Element("挤压应力").Value = txtSigma.Text;
            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

