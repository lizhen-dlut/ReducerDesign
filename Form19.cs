using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class Form19 : Form
    {
        ShaftStrength shaftstre = new ShaftStrength();
        public int Grade = 1;

        public Form19()
        {
            InitializeComponent();
        }

        public Form19(int i)
        {
            Grade = i;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");
            shaftstre.Fhz = double.Parse(xe.Element("fhz").Value);
            shaftstre.L5 = double.Parse(xe.Element("L5").Value);
            shaftstre.L4 = double.Parse(xe.Element("L4").Value);
            shaftstre.L3 = double.Parse(xe.Element("L3").Value);
            shaftstre.Fbz = double.Parse(xe.Element("fbz").Value);
            shaftstre.Fa2 = double.Parse(xe.Element("fa").Value);
            shaftstre.r2 = double.Parse(xe.Element("r").Value);
            shaftstre.Fez = double.Parse(xe.Element("fez").Value);
            shaftstre.Fby = double.Parse(xe.Element("fby").Value);
            shaftstre.Fhy = double.Parse(xe.Element("fhy").Value);
            shaftstre.Fey = double.Parse(xe.Element("fey").Value);
            shaftstre.L1 = double.Parse(xe.Element("L1").Value);
            //D-D剖面水平面右侧弯矩
            txtMdzR.Text = shaftstre.ShaftMdzR4().ToString("f4");
            //D-D剖面水平面左侧弯矩
            txtMdzL.Text = shaftstre.ShaftMdzL4().ToString("f4");
            //D-D剖面垂直面弯矩
            txtMdy.Text = shaftstre.ShaftMdy4().ToString("f4");
            ////D-D剖面右侧合成弯矩
            txtMdR.Text = shaftstre.SyntheticMoment(double.Parse(txtMdzR.Text), double.Parse(txtMdy.Text)).ToString("f4");
            //D-D剖面左侧合成弯矩
            txtMdL.Text = shaftstre.SyntheticMoment(double.Parse(txtMdzL.Text), double.Parse(txtMdy.Text)).ToString("f4");
            //A-A 剖面水平面弯矩
            txtMaz.Text = shaftstre.ShaftMaz4().ToString("f4");
            //A-A剖面垂直面弯矩
            txtMay.Text = shaftstre.ShaftMay4().ToString("f4");
            //A-A剖面合成弯矩
            txtMa.Text = shaftstre.SyntheticMoment(double.Parse(txtMaz.Text), double.Parse(txtMay.Text)).ToString("f4");
            //B-B剖面水平弯矩
            txtMbz.Text = shaftstre.ShaftMbz4().ToString("f4");
            //B-B剖面垂直面弯矩
            txtMby.Text = shaftstre.ShaftMby4().ToString("f4");
            //B-B剖面合成弯矩
            txtMb.Text = shaftstre.SyntheticMoment(double.Parse(txtMbz.Text), double.Parse(txtMby.Text)).ToString("f4");
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("受力计算参数");
            txtT1.Text = xe.Element("T1").Value;
            txtT2.Text = xe.Element("T2").Value;

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("动力参数");
            txtT3.Text = xe.Element("扭矩").Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {   //A 弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("轴段2直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMa.Text);
            double t = double.Parse(txtT1.Text);
            if (m == 0)
            {
                txtBendSa.Text = shaftstre.CircleSecTorStre(d, t).ToString("f4");//此时为扭转应力
            }
            else
            {
                txtBendSa.Text = shaftstre.CircleSecBendStre(d, alpha, m, t).ToString("f4");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //D-D左侧弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("轴段5直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMdL.Text);
            double t = double.Parse(txtT1.Text);

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽2");
            double b = double.Parse(xe.Element("键槽b").Value);//键槽尺寸  
            double ht = double.Parse(xe.Element("键槽t1").Value);//键槽尺寸  
            txtBendSdl.Text = shaftstre.SingleGroBendStre(d, alpha, m, t, b, ht).ToString("f4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //D-D右侧弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("轴段5直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMdR.Text);
            double t = double.Parse(txtT2.Text);

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴").Element("键槽2");
            double b = double.Parse(xe.Element("键槽b").Value);//键槽尺寸  
            double ht = double.Parse(xe.Element("键槽t1").Value);//键槽尺寸    
            txtBendSdr.Text = shaftstre.SingleGroBendStre(d, alpha, m, t, b, ht).ToString("f4");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //B-B剖面弯曲应力
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("齿轮轴");
            double d = double.Parse(xe.Element("轴段6直径").Value);
            double alpha = double.Parse(cmbBoxAlpha.Text);
            double m = double.Parse(txtMb.Text);
            double t = double.Parse(txtT2.Text);
            if (m == 0)
            {
                txtBendSb.Text = shaftstre.CircleSecTorStre(d, t).ToString("f4");//此时为扭转应力
            }
            else
            {
                txtBendSb.Text = shaftstre.CircleSecBendStre(d, alpha, m, t).ToString("f4");
            }
        }


        private void btSaveData_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("强度计算").Element("D剖面弯矩");
            
            xe.Element("水平面左侧").Value = txtMdzL.Text;
            xe.Element("水平面右侧").Value = txtMdzR.Text;
            xe.Element("垂直面").Value = txtMdy.Text;
            xe.Element("左侧合成").Value = txtMdL.Text;
            xe.Element("右侧合成").Value = txtMdR.Text;

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("强度计算").Element("A剖面弯矩");
           xe.Element("水平面").Value = txtMaz.Text;
            xe.Element("垂直面").Value = txtMay.Text;
            xe.Element("合成").Value = txtMa.Text;

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("强度计算").Element("B剖面弯矩");
           xe.Element("水平面").Value = txtMbz.Text;
            xe.Element("垂直面").Value = txtMby.Text;
            xe.Element("合成").Value = txtMb.Text;

            xe = SystemConstants.XmlProject.Element("第" + Grade + "级轴串").Element("强度计算");
            xe.Element("E处扭矩").Value = txtT1.Text;
            xe.Element("D处扭矩").Value = txtT3.Text;
            xe.Element("H处扭矩").Value = txtT2.Text;
            xe.Element("折合系数").Value = cmbBoxAlpha.Text;
            xe.Element("A剖面弯曲应力").Value = txtBendSa.Text;
            xe.Element("D剖面左侧弯曲应力").Value = txtBendSdl.Text;
            xe.Element("D剖面右侧弯曲应力").Value = txtBendSdr.Text;
            xe.Element("B剖面弯曲应力").Value = txtBendSb.Text;

            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }

       

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
