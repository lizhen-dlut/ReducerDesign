using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmSingleGradeCalculate : Form
    {

        public FrmSingleGradeCalculate()
        {
            InitializeComponent();
        }

        private void FrmSingleGradeCalculate_Load(object sender, EventArgs e)
        {
            XElement tempXE = SystemConstants.XmlProject.Element("总体要求");
            if (tempXE != null)
            {
                //groupBox1.Text = tempXE.Element("级数").Value + " 级减速器";
                //labPower.Text = "总功率：" + tempXE.Element("总功率").Value;
                //labSpeed.Text = "转  速：" + tempXE.Element("转速").Value;
                //labN.Text = "总速比：" + tempXE.Element("总速比").Value;
                txtTotalRatio.Text = tempXE.Element("总速比").Value;
                //tx txt. _iGrade = int.Parse(tempXE.Element("级数").Value);
            }

            txtEqualRatio.Text = string.Format("{0:.000}",
                                               Math.Pow(double.Parse(txtTotalRatio.Text),
                                                        1.0/
                                                        double.Parse(
                                                            SystemConstants.XmlProject.Element("总体要求").Element("级数").
                                                                Value)));

            double ratio = double.Parse(txtTotalRatio.Text);
            double firstratio = 3.928487998932040e-8*Math.Pow(ratio, 3) - 4.467530322239769e-5*Math.Pow(ratio, 2) +
                                0.027959596444647*ratio + 2.357147782093125;
            txtFirst.Text = firstratio.ToString("0.000");
            double secondratio = 0.190240534000327*Math.Pow(ratio, 0.492815033945799) + 2.988001580102834;
            txtSecond.Text = secondratio.ToString("0.000");
            txtThird.Text = (ratio/(firstratio*secondratio)).ToString("0.000");

            txtU.Text = txtEqualRatio.Text;
        }

        private void btnToothWidth_Click(object sender, EventArgs e)
        {
            FrmToothWidthCoefficient frm = new FrmToothWidthCoefficient();
            frm.Show();
        }

        private void txtZXJ_Validated(object sender, EventArgs e)
        {
            cmbCM_TextChanged(sender, e);
        }

        private void butCentreDistance_Click(object sender, EventArgs e)
        {
            var zxjjs = new cljbjs();
            if (radioButton1.Checked)
            {
                zxjjs.T = double.Parse(txtP.Text)*9550/double.Parse(txtN.Text);
            }
            else
            {
                zxjjs.T = double.Parse(txtT.Text);
            }

            zxjjs.k = double.Parse(txtK.Text);
            zxjjs.Aa = double.Parse(txtAa.Text);
            zxjjs.u = double.Parse(txtU.Text);
            zxjjs.psi_d = double.Parse(txtPSID.Text);
            zxjjs.sigma_hp = double.Parse(txtsigma.Text)*0.9;
            zxjjs.CKXS = 1; // 执行此语句
            txtZXJ.Text = zxjjs.ZXJ.ToString("f4"); //string.Format("{0:.000}", zxjjs.ZXJ); //

            cmbCM_TextChanged(sender, e);
            butPinionNumber.Enabled = true;
        }


        private void cmbCM_TextChanged(object sender, EventArgs e)
        {
            if (cmbCM.Text == "硬齿面")
            {
                txtMF1.Text = (0.016*double.Parse(txtZXJ.Text)).ToString("f2");
                txtMF2.Text = (0.0315*double.Parse(txtZXJ.Text)).ToString("f2");
            }
            else
            {
                txtMF1.Text = (0.007*double.Parse(txtZXJ.Text)).ToString("f2");
                txtMF2.Text = (0.02*double.Parse(txtZXJ.Text)).ToString("f2");
            }
        }

        private void butGearModule_Click(object sender, EventArgs e)
        {
            FrmGearModule childFrme = new FrmGearModule();
            childFrme.Show();
        }

        private void butGearNumber_Click(object sender, EventArgs e)
        {
            int data;
            if (int.TryParse(txtXCS.Text, out data))
            {
                txtDCS.Text = (int.Parse(txtXCS.Text)*(double.Parse(txtU.Text))).ToString();
                butDeflectionCalculate.Enabled = true;
            }
            else
            {
                MessageBox.Show("小齿轮齿数 Z1 不是整数");
            }
        }

        private void butPinionNumber_Click(object sender, EventArgs e)
        {
            double xcs = 2*double.Parse(txtZXJ.Text)*Math.Cos(double.Parse(txtLXJ.Text)*Math.PI/180)/
                         (double.Parse(txtMS.Text)*(double.Parse(txtU.Text) + 1));
            //if (xcs < 17)
            //{
            //    txtXCS.Text = "17";
            //}
            //else
            //{
            txtXCS.Text = xcs.ToString("f2");
            //}
            butGearNumber.Enabled = true;
           
        }

        private void butDeflectionCalculate_Click(object sender, EventArgs e)
        {
            //未变位中心距
            double a = (double.Parse(txtDCS.Text) + double.Parse(txtXCS.Text))*double.Parse(txtMS.Text)/(2*Math.Cos(double.Parse(txtLXJ.Text)*Math.PI/180));
            //端面压力角
            double alphaT = Math.Atan(Math.Tan(double.Parse(txtCXJ.Text)*Math.PI/180)/Math.Cos(double.Parse(txtLXJ.Text)*Math.PI/180));
            //啮合角
            double alphaT1 = Math.Acos(a*Math.Cos(alphaT)/double.Parse(txtZXJ.Text));
            //总变为系数
            double temp = Math.Tan(alphaT1) - alphaT1 - (Math.Tan(alphaT) - alphaT);
            double xN = (double.Parse(txtDCS.Text) + double.Parse(txtXCS.Text))*temp/(2*Math.Tan(double.Parse(txtCXJ.Text)*Math.PI/180));

            txtZBW.Text = xN.ToString("f4");
            if (xN < 0)
            {
                MessageBox.Show("不建议采用负传动,请重新设计");
                button8.Enabled = false;
            }
            else
            {
                button8.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var mc = new ModificationCoefficient();
            mc.Z1 = double.Parse(txtXCS.Text); //大齿数
            mc.Z2 = double.Parse(txtDCS.Text); //小齿数
            mc.Mn = double.Parse(txtMS.Text); //模数
            mc.Beta = double.Parse(txtLXJ.Text); //螺旋角
            mc.Alpha = double.Parse(txtCXJ.Text); //压力角
            mc.Han = double.Parse(txtHa.Text); //齿顶高系数
            mc.A1 = double.Parse(txtZXJ.Text); //变位中心距
            txtXBW.Text = mc.ModificationCalculation().ToString("f5"); //小齿轮变位系数
            txtMinModification.Text = mc.MinModification1().ToString("f5"); //最小变位系数
        }

        private void butChecking_Click(object sender, EventArgs e)
        {
            ModificationCoefficient modc = new ModificationCoefficient();
            modc.Z1 = double.Parse(txtXCS.Text); //大齿数
            modc.Z2 = double.Parse(txtDCS.Text); //小齿数
            modc.Mn = double.Parse(txtMS.Text); //模数
            modc.Beta = double.Parse(txtLXJ.Text); //螺旋角
            modc.Alpha = double.Parse(txtCXJ.Text); //压力角
            modc.Han = double.Parse(txtHa.Text); //齿顶高系数
            modc.A1 = double.Parse(txtZXJ.Text); //变位中心距
            modc.Mc1 = double.Parse(txtXBW.Text); //小齿轮变位系数
            double[] checking = modc.Checking();
            if (checking[0] > 0.4*double.Parse(txtMS.Text)) //淬火要求齿顶厚大于0.4m
            {
                MessageBox.Show("齿顶厚检验通过","",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("齿顶厚不满足要求","",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            if (checking[1] >= 1.2) //重合度要求大于等于1.2
            {
                MessageBox.Show("重合度检验通过", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("重合度不满足要求", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (checking[2] >= 0)
            {
                MessageBox.Show("小齿轮齿根与大齿轮齿顶干涉检验通过", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("小齿轮齿根与大齿轮齿顶产生干涉", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (checking[3] >= 0)
            {
                MessageBox.Show("大齿轮齿根与小齿轮齿顶干涉检验通过", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("大齿轮齿根与小齿轮齿顶产生干涉", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void rbEqualRatio_CheckedChanged(object sender, EventArgs e)
        {
            //等速比计算
            if (rbEqualRatio.Checked)
            {
                txtEqualRatio.Text = string.Format("{0:.000}",
                                                   Math.Pow(double.Parse(txtTotalRatio.Text),
                                                            1.0/
                                                            double.Parse(
                                                                SystemConstants.XmlProject.Element("总体要求").Element("级数")
                                                                    .Value)));
            }
        }



        private void rbEqualStrength_CheckedChanged(object sender, EventArgs e)
        {
            //等强度计算 较小外形 重量
            if (rbEqualStrength.Checked)
            {
                double ratio = double.Parse(txtTotalRatio.Text);
                double firstratio = 3.928487998932040e-8*Math.Pow(ratio, 3) - 4.467530322239769e-5*Math.Pow(ratio, 2) +
                                    0.027959596444647*ratio + 2.357147782093125;
                txtFirst.Text = firstratio.ToString("0.000");
                double secondratio = 0.190240534000327*Math.Pow(ratio, 0.492815033945799) + 2.988001580102834;
                txtSecond.Text = secondratio.ToString("0.000");
                txtThird.Text = (ratio/(firstratio*secondratio)).ToString("0.000");
            }
        }

        // 小齿轮齿数计算
        private void txtXCS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                double d1 = double.Parse(txtXCS.Text)*double.Parse(txtMS.Text)/
                            Math.Cos(double.Parse(txtLXJ.Text)*Math.PI/180);
                double b = d1*double.Parse(txtPSID.Text);
                //圆整末尾为5或0;
                double b1 = Math.Round(b/5)*5;
                txtB.Text = b1.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void txtXCS_Validated(object sender, EventArgs e)
        {
            double d1 = double.Parse(txtXCS.Text)*double.Parse(txtMS.Text)/
                        Math.Cos(double.Parse(txtLXJ.Text)*Math.PI/180);
            double b = d1*double.Parse(txtPSID.Text);
            //圆整末尾为5或0;
            double b1 = Math.Round(b/5)*5;
            txtB.Text = b1.ToString(CultureInfo.InvariantCulture);
        }

        // 大齿轮齿数计算
        private void txtDCS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int data;
                if (int.TryParse(txtDCS.Text, out data))
                {
                    txtU.Text = (double.Parse(txtDCS.Text)/double.Parse(txtXCS.Text)).ToString("f4");
                    int z1 = int.Parse(txtXCS.Text);
                    int z2 = int.Parse(txtDCS.Text);
                    if (!isRelativelyPrime(z1, z2))
                    {
                        MessageBox.Show("z1、z2不是互质数");
                    }
                }
                else
                {
                    MessageBox.Show("z2不是整数");
                    txtDCS.Focus();
                }
            }
        }

        private bool  isRelativelyPrime(int z1,int  z2)
        {
            for (int i = 2; i <= z1 - 1; i++)
            {
                if (z1 % i == 0 & z2 % i == 0)
                {
                    return false;
                    break;
                }
            }
            return true;
        }
                

        private void txtDCS_Validated(object sender, EventArgs e)
        {
            if (!txtDCS.Text.Equals("0"))
            {
                int data;
                if (int.TryParse(txtDCS.Text, out data))
                {
                    txtU.Text = (double.Parse(txtDCS.Text)/double.Parse(txtXCS.Text)).ToString();
                    int z1 = int.Parse(txtXCS.Text);
                    int z2 = int.Parse(txtDCS.Text);
                    for (int i = 2; i <= z1 - 1; i++)
                    {
                        if (z1%i == 0 & z2%i == 0)
                        {
                            MessageBox.Show("z1、z2不是互质数");
                            txtDCS.Focus();
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("z2不是整数");
                    txtDCS.Focus();
                }
            }
        }
        private void btSaveData1_Click(object sender, EventArgs e)
        {
            var element = SystemConstants.XmlProject.Element("齿轮参数计算");
            if (element != null)
            {
                XElement xElement =element.Element("第" + SystemConstants.SingleCalcultGrade + "级齿轮参数").Element("输入参数");
                if (xElement != null)
                {
                    xElement.Element("a").Value = txtZXJ.Text;
                    xElement.Element("Mn").Value = txtMS.Text;
                    xElement.Element("Z1").Value = txtXCS.Text;
                    xElement.Element("Z2").Value = txtDCS.Text;
                    xElement.Element("ha").Value = txtHa.Text;
                    xElement.Element("c").Value = txtC.Text;
                    xElement.Element("B1").Value = txtLXJ.Text;
                    xElement.Element("alpha").Value = txtCXJ.Text;
                    xElement.Element("x1").Value = txtXBW.Text;
                    xElement.Element("B").Value = txtB.Text;
                    SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
                }
            }

            //IEnumerable<XElement> xe = SystemConstants.XmlProject.Elements("齿轮参数计算");
            //XElement.Load(@"D:\减速器设计系统\齿轮计算参数.xml"));
            //IEnumerable<XElement> elements = from a in xe.Elements("第一级齿轮参数").Elements("输入参数")                               select a;
           
            //xe.Save(@"D:\减速器设计系统\齿轮计算参数.xml");
            //this.Parent.Show();
            this.Close();

            var frm = new FrmGearCalculate();
            frm.Show();
        }

        private void txtZXJ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                cmbCM_TextChanged(sender, e);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            var frm = new FrmGearCalculate();
            frm.Show();
        }
    }
}



       



       
        //private void btSaveData2_Click(object sender, EventArgs e)
        //{
        //    XElement xe = XElement.Load(@"D:\减速器设计系统\齿轮计算参数.xml");
        //    IEnumerable<XElement> elements = from a in xe.Elements("第二级齿轮参数").Elements("输入参数")
        //                                     select a;
        //    elements.ElementAt(0).Element("a").Value = txtZXJ.Text;
        //    elements.ElementAt(0).Element("Mn").Value = txtMS.Text;
        //    elements.ElementAt(0).Element("Z1").Value = txtXCS.Text;
        //    elements.ElementAt(0).Element("Z2").Value = txtDCS.Text;
        //    elements.ElementAt(0).Element("ha").Value = txtHa.Text;
        //    elements.ElementAt(0).Element("c").Value = txtC.Text;
        //    elements.ElementAt(0).Element("B1").Value = txtLXJ.Text;
        //    elements.ElementAt(0).Element("alpha").Value = txtCXJ.Text;
        //    elements.ElementAt(0).Element("x1").Value = txtXBW.Text;
        //    elements.ElementAt(0).Element("B").Value = txtB.Text;
        //    xe.Save(@"D:\减速器设计系统\齿轮计算参数.xml");
        //    this.Close();
        //    Form2 frm = new Form2();
        //    frm.Show();


        //}

        //private void btSaveData1_Click(object sender, EventArgs e)
        //{
        //    XElement xe = XElement.Load(@"D:\减速器设计系统\齿轮计算参数.xml");
        //    IEnumerable<XElement> elements = from a in xe.Elements("第一级齿轮参数").Elements("输入参数")
        //                                     select a;
        //    elements.ElementAt(0).Element("a").Value = txtZXJ.Text;
        //    elements.ElementAt(0).Element("Mn").Value = txtMS.Text;
        //    elements.ElementAt(0).Element("Z1").Value = txtXCS.Text;
        //    elements.ElementAt(0).Element("Z2").Value = txtDCS.Text;
        //    elements.ElementAt(0).Element("ha").Value = txtHa.Text;
        //    elements.ElementAt(0).Element("c").Value = txtC.Text;
        //    elements.ElementAt(0).Element("B1").Value = txtLXJ.Text;
        //    elements.ElementAt(0).Element("alpha").Value = txtCXJ.Text;
        //    elements.ElementAt(0).Element("x1").Value = txtXBW.Text;
        //    elements.ElementAt(0).Element("B").Value = txtB.Text;
        //    xe.Save(@"D:\减速器设计系统\齿轮计算参数.xml");
        //    this.Close();
        //    Form2 frm = new Form2();
        //    frm.Show();
        //}

        //private void btSaveData3_Click(object sender, EventArgs e)
        //{
        //    XElement xe = XElement.Load(@"D:\减速器设计系统\齿轮计算参数.xml");
        //    IEnumerable<XElement> elements = from a in xe.Elements("第三级齿轮参数").Elements("输入参数")
        //                                     select a;
        //    elements.ElementAt(0).Element("a").Value = txtZXJ.Text;
        //    elements.ElementAt(0).Element("Mn").Value = txtMS.Text;
        //    elements.ElementAt(0).Element("Z1").Value = txtXCS.Text;
        //    elements.ElementAt(0).Element("Z2").Value = txtDCS.Text;
        //    elements.ElementAt(0).Element("ha").Value = txtHa.Text;
        //    elements.ElementAt(0).Element("c").Value = txtC.Text;
        //    elements.ElementAt(0).Element("B1").Value = txtLXJ.Text;
        //    elements.ElementAt(0).Element("alpha").Value = txtCXJ.Text;
        //    elements.ElementAt(0).Element("x1").Value = txtXBW.Text;
        //    elements.ElementAt(0).Element("B").Value = txtB.Text;
        //    xe.Save(@"D:\减速器设计系统\齿轮计算参数.xml");
        //    this.Close();
        //    Form2 frm = new Form2();
        //    frm.Show();
        //}

        //private void btChecking_Click(object sender, EventArgs e)
        //{
