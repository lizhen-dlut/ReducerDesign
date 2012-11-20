using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmBearingLife : Form
    {
        public int Grade=1;
        Bearingcmp bearing = new Bearingcmp();

        public FrmBearingLife()
        {
            InitializeComponent();
        }

        public FrmBearingLife(int i)
        {
            Grade = i;
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bearing.Fa = double.Parse(txtFax.Text);//轴向力
            bearing.Fr = double.Parse(txtFra.Text);//径向力
            bearing.c = double.Parse(txtC.Text);//额定动载荷
            bearing.e = double.Parse(txtE.Text);//判定系数
            bearing.Y1 = double.Parse(txtY1.Text);//计算系数
            bearing.Y2 = double.Parse(txtY2.Text);//计算系数
            bearing.fn = double.Parse(txtFn.Text);//速度因数
            bearing.ft = double.Parse(txtFt.Text);//温度系数
            bearing.fm = double.Parse(txtFm.Text);//力矩载荷因数
            bearing.fd = double.Parse(txtFd.Text);//冲击载荷因数
            if (cmbBox1.Text == "调心滚子轴承")
            {
                bearing.EquDynLoad();//计算当量动载荷
                txtFha.Text = Math.Round(bearing.BearingLifeFac(), 3).ToString("f4");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bearing.Fa = double.Parse(txtFbx.Text);//轴向力
            bearing.Fr = double.Parse(txtFrb.Text);//径向力
            bearing.c = double.Parse(txtC.Text);//额定动载荷
            bearing.e = double.Parse(txtE.Text);//判定系数
            bearing.Y1 = double.Parse(txtY1.Text);//计算系数
            bearing.Y2 = double.Parse(txtY2.Text);//计算系数
            bearing.fn = double.Parse(txtFn.Text);//速度因数
            bearing.ft = double.Parse(txtFt.Text);//温度系数
            bearing.fm = double.Parse(txtFm.Text);//力矩载荷因数
            bearing.fd = double.Parse(txtFd.Text);//冲击载荷因数
            if (cmbBox1.Text == "调心滚子轴承")
            {
                bearing.EquDynLoad();//计算当量动载荷
                txtFhb.Text = Math.Round(bearing.BearingLifeFac(), 3).ToString("f4");
            }
        }

        private void txtN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                bearing.N = double.Parse(txtN.Text);
                if (cmbBox1.Text == "调心滚子轴承")
                {
                    txtFn.Text = bearing.RoBearSpdFac().ToString("f4");
                }
                else
                {
                    txtFn.Text = bearing.BaBearSpdFac().ToString("f4");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bearing.Fh = double.Parse(txtFha.Text);
            if (cmbBox1.Text == "调心滚子轴承")
            {
                txtHa.Text = bearing.RoBearLife().ToString("f4");
            }
            else
            {
                txtHa.Text = bearing.BaBearLife().ToString("f4");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bearing.Fh = double.Parse(txtFhb.Text);
            if (cmbBox1.Text == "调心滚子轴承")
            {
                txtHb.Text = bearing.RoBearLife().ToString("f4");
            }
            else
            {
                txtHb.Text = bearing.BaBearLife().ToString("f4");
            }
        }

        
        private void Form7_Load(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");
            txtFax.Text = Math.Abs(double.Parse(xe.Element("fax").Value)).ToString("f4");
            txtFra.Text = double .Parse( xe.Element("A径向力").Value).ToString( "f4");
            txtFbx.Text = Math.Abs(double.Parse(xe.Element("fbx").Value)).ToString("f4");
            txtFrb.Text = double.Parse( xe.Element("B径向力").Value).ToString("f4");
          
            txtN.Text = double.Parse(SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数").Element("转速").Value).ToString( "f4");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("轴承寿命计算");
            
            var name = new string[]{"轴承类型","轴承型号","额定动载荷","判定系数e","计算系数Y1","计算系数Y2",
                                          "轴承转速","速度因数","力矩载荷因数","冲击载荷因数","温度因数","A轴承寿命因数",
                                         "A轴承寿命","B轴承寿命因数","B轴承寿命"};
            var parameter = new string[]{cmbBox1.Text,txtModel.Text,txtC.Text,txtE.Text,txtY1.Text,txtY2.Text,
                                           txtN.Text,txtFn.Text,txtFm.Text,txtFd.Text,txtFt.Text,txtFha.Text,
                                           txtHa.Text,txtFhb.Text,txtHb.Text};
            for (int i = 0; i < 15; i++)
            {
                xe.Element(name[i]).Value = parameter[i];
            }
            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }

        private void btImpactTable_Click(object sender, EventArgs e)
        {
            FrmPicImpactloadFactor frmPic = new FrmPicImpactloadFactor();
            frmPic.Show();
        }

        private void btTempreatureTable_Click(object sender, EventArgs e)
        {
            FrmPicTemperatureFactor frm = new FrmPicTemperatureFactor();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
