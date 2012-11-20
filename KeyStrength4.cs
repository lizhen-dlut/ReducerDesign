using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class KeyStrength4 : Form
    {
        private int Grade = 0;
        public KeyStrength4()
        {
            InitializeComponent();
        }

        public KeyStrength4(int i)
        {
            Grade = i;
            InitializeComponent();
        }

        private void btKeyCalculation1_Click(object sender, EventArgs e)
        {
            double t1 = double.Parse(txtT1.Text);
            double d1 = double.Parse(txtD1.Text);
            double k1 = double.Parse(txtK1.Text);
            double l1 = double.Parse(txtL1.Text);
            txtSigma1.Text = (2 * t1 / (d1 * k1 * l1)).ToString("f4");
        }

        private void btKeyCalculation2_Click(object sender, EventArgs e)
        {
            double t2 = double.Parse(txtT2.Text);
            double d2 = double.Parse(txtD2.Text);
            double k2 = double.Parse(txtK2.Text);
            double l2 = double.Parse(txtL2.Text);
            txtSigma2.Text = (2 * t2 / (d2 * k2 * l2)).ToString("f4");
        }

        private void btKeyCalculation3_Click(object sender, EventArgs e)
        {
            double t3 = double.Parse(txtT3.Text);
            double d3 = double.Parse(txtD3.Text);
            double k3 = double.Parse(txtK3.Text);
            double l3 = double.Parse(txtL3.Text);
            txtSigma3.Text = (2 * t3 / (d3 * k3 * l3)).ToString("f4");
        }

        private void KeyStrength4_Load(object sender, EventArgs e)
        {
            #region 键槽2
            double t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数").Element("扭矩").Value);
            txtT2.Text = t.ToString("f4");

            t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数").Element("T1").Value);
            txtT1.Text = t.ToString("f4");

            t = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数").Element("T2").Value);
            txtT3.Text = t.ToString("f4");

            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            txtD1.Text = xe.Element("轴段1直径").Value;
            txtD2.Text = xe.Element("轴段5直径").Value;
            txtD3.Text = xe.Element("轴段7直径").Value;

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽2");

            double b2 = double.Parse(xe.Element("键槽b").Value);
            double h2 = double.Parse(xe.Element("键槽h").Value);
            double l2 = double.Parse(xe.Element("键槽L").Value);
            txtK2.Text = (h2 / 2).ToString("f4");
            txtL2.Text = (l2 - b2).ToString("f4");
            #endregion

            #region 键槽1
            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽1");
            double b1 = double.Parse(xe.Element("键槽b").Value);
            double h1 = double.Parse(xe.Element("键槽h").Value);
            double l1 = double.Parse(xe.Element("键槽L").Value);
            txtK1.Text = (h1 / 2).ToString("f4");
            txtL1.Text = (l1 - b1).ToString("f4");
            #endregion
            #region 键槽3
            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽3");
            double b3 = double.Parse(xe.Element("键槽b").Value);
            double h3 = double.Parse(xe.Element("键槽h").Value);
            double l3 = double.Parse(xe.Element("键槽L").Value);
            txtK3.Text = (h3 / 2).ToString("f4");
            txtL3.Text = (l3 - b3).ToString("f4");

            #endregion

        }

        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("键计算");
         
            xe.Element("挤压应力1").Value = txtSigma1.Text;
            xe.Element("挤压应力2").Value = txtSigma2.Text;
            xe.Element("挤压应力3").Value = txtSigma3.Text;
            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");

        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
