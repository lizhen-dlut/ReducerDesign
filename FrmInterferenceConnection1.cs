using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmInterferenceConnection1 : Form
    {
        public int Grade = 1;
        public double T = 0.0;
        public double D = 0.0;
        public FrmInterferenceConnection1()
        {
            InitializeComponent();
        }

        public FrmInterferenceConnection1(int i,double t,double d)
        {
            Grade = i;
            T = t;
            D = d;
            InitializeComponent();
        }
        private void FrmInterferenceConnection1_Load(object sender, EventArgs e)
        {
            //double t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数").Element("扭矩").Value);
            txtT.Text = T.ToString("f4");

            //t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("轴段6直径").Value);
            txtDf.Text = D.ToString("f4");
        }
        private void btInterference_Click(object sender, EventArgs e)
        {
            if (cmbAssembly.Text == "胀缩法")
            {
                //调用过盈计算并给变量赋值
                Interference infe = new Interference();
                infe.T = double.Parse(txtT.Text);
                infe.Df = double.Parse(txtDf.Text);
                infe.Lf = double.Parse(txtLf.Text);
                infe.Mu = double.Parse(txtMu.Text);
                infe.Da = double.Parse(txtDa.Text);
                infe.Di = double.Parse(txtDi.Text);
                infe.Ea = double.Parse(txtEa.Text);
                infe.Ei = double.Parse(txtEi.Text);
                infe.Va = double.Parse(txtVa.Text);
                infe.Vi = double.Parse(txtVi.Text);
                infe.Sigmasa = double.Parse(txtSigmasa.Text);
                infe.Sigmasi = double.Parse(txtSigmasi.Text);
                // 求解
                txtPfmin.Text = infe.Pfmin().ToString("f4");
                txtQa.Text = infe.Qa().ToString("f4");
                txtQi.Text = infe.Qi().ToString("f4");
                txtEamin.Text = infe.Eamin().ToString("f4");
                txtEimin.Text = infe.Eimin().ToString("f4");
                txtDeltaemin.Text = infe.Deltaemin().ToString("f4");
                txtDeltamin.Text = infe.Deltamin().ToString("f4");
                txtPfamax.Text = infe.Pfamax().ToString("f4");
                txtPfimax.Text = infe.Pfimax().ToString("f4");
                txtPfmax.Text = infe.Pfmax().ToString("f4");
                txtFt.Text = infe.Ft().ToString("f4");
                txtEamax.Text = infe.Eamax().ToString("f4");
                txtEimax.Text = infe.Eimax().ToString("f4");
                txtDeltaemax.Text = infe.Deltaemax().ToString("f4");
                txtDeltab.Text = infe.Deltab().ToString("f4");
            }
        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            if (cmbAssembly.Text == "胀缩法")
            {    //选定配合公差后计算最大最小过盈量
                double deltamax = Math.Abs(double.Parse(txtDeviation.Text)) + double.Parse(txtShaftTolerance.Text);
                double deltamin = Math.Abs(double.Parse(txtDeviation.Text)) - double.Parse(txtHoleTolerance.Text);
                txtMax.Text = deltamax.ToString("f4");
                txtMin.Text = deltamin.ToString("f4");
                if (deltamax <= double.Parse(txtDeltaemax.Text) && deltamin > double.Parse(txtDeltamin.Text))
                {
                    MessageBox.Show("通过");
                }
                else
                {
                    MessageBox.Show("重新选择配合公差");
                }
            }
        }

        
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

        private void btInquiry_Click(object sender, EventArgs e)
        {
            FrmPicInterferenceTable frm = new FrmPicInterferenceTable();
            frm.Show();
        }

        private void cmbFit_SelectedIndexChanged(object sender, EventArgs e)
        {
            double d = double.Parse(txtDf.Text);
            string str = cmbFit.Text;
            InterferenceTolerance it = new InterferenceTolerance();
            txtDeviation.Text = (it.getTolerance(d, str) / 1000).ToString("f4");
        }

        private void cmbHoleGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            double d = double.Parse(txtDf.Text);
            int holegrade = int.Parse(cmbHoleGrade.Text);
            InterferenceTolerance it = new InterferenceTolerance();
            txtHoleTolerance.Text = (it.getStandardOfTolerance(d, holegrade) / 1000).ToString("f4");
        }

        private void cmbShaftGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            double d = double.Parse(txtDf.Text);
            int shaftgrade = int.Parse(cmbShaftGrade.Text);
            InterferenceTolerance it = new InterferenceTolerance();
            txtShaftTolerance.Text = (it.getStandardOfTolerance(d, shaftgrade) / 1000).ToString("f4");
        }
        
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string[] name = new string[]{"扭矩T","接合直径df","接合长度lf","摩擦因数mu","包容件外径da","被包容件外径di",
                "包容件弹性模量Ea","被包容件弹性模量Ei","包容件泊松比va","被包容件泊松比vi","包容件材料屈服点sigmasa","被包容件材料屈服点sigmasi","装配方式","基准制",
                "最小接合压强pfmin","包容件直径比qa","被包容件直径比qi", "包容件最小直径变化量eamin","被包容件最小直径变化量eimin","最小有效过盈量deltaemin", "考虑压平量的最小过盈量deltamin",
                "包容件最大接合压强pfamax","被包容件最大接合压强pfimax", "被联接件最大接合压强pfmax","被联接件不产生塑性变形传递力Ft","包容件最大直径变化量eamax", "被包容件最大直径变化量eimax",
                "最大有效过盈量deltaemax","初选基本过盈量deltab", "孔配合公差","轴配合公差","最大过盈量deltamax","最小过盈量deltamin"};
            if (cmbDatum.Text == "基孔制")
            {
                string holemate = "H" + cmbHoleGrade.Text;
                string shaftmate = cmbFit.Text + cmbShaftGrade.Text;

                string[] parameter = new string[]{txtT.Text,txtDf.Text,txtLf.Text,txtMu.Text,txtDa.Text,txtDi.Text,
                                          txtEa.Text,txtEi.Text,txtVa.Text,txtVi.Text,txtSigmasa.Text,txtSigmasi.Text,cmbAssembly.Text,cmbDatum.Text,
                                          txtPfmin.Text,txtQa.Text,txtQi.Text,txtEamin.Text,txtEimin.Text,txtDeltaemin.Text,txtDeltamin.Text,
                                          txtPfamax.Text,txtPfimax.Text,txtPfmax.Text,txtFt.Text,txtEamax.Text,txtEimax.Text,
                                          txtDeltaemax.Text,txtDeltab.Text,holemate,shaftmate,txtMax.Text,txtMin.Text};

                XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("过盈计算").Element("输入参数");
                for (int i = 0; i < 14; i++)
                {
                    xe.Element(name[i]).Value = parameter[i];
                }

                xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("过盈计算").Element("输出参数");
                for (int i = 14; i < 33; i++)
                {
                   xe.Element(name[i]).Value = parameter[i];
                }
                SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
            }
        }
    }
}
