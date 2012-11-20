using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{

    // [Guid("1D9440C8-E719-4F3B-8F09-888E51A39530")]//"6D1C55AF-7073-4B2E-BBC7-614EB0921965")]
    public partial class FrmAssemblySketch : Form
    {
        Process process = new Process();

        public FrmAssemblySketch()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //this.Close();
            //InputPartForm frm = new InputPartForm();
            //frm.Show();
        }

         //[DllImport("Interop.EModelView.dll")]
        //public static extern int DllRegisterServer();//注册时用
        private void Form6_Load(object sender, EventArgs e)
        {
            #region 调用外部程序打开装配草图

            string exe_path = SystemConstants.StrProjectPath;// @"C:\Program Files (x86)\Common Files\eDrawings2011\";
           
            process.StartInfo.FileName = "EModelViewer.exe";
            process.StartInfo.WorkingDirectory = exe_path;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = SystemConstants.XmlProject.Element("装配草图").Attribute("文件").Value ;// @"D:\减速器设计系统\装配草图1.SLDDRW";
            process.Start();

            #endregion

            //axEModelViewControl1.OpenDoc(@"D:\减速器设计系统\装配草图1.SLDDRW", false, false, false, "");
            //axEModelViewControl1.Refresh();

            XElement xe = SystemConstants.XmlProject.Element("齿轮参数计算");
            txtL26.Text = double.Parse(xe.Element("第1级齿轮参数").Element("输入参数").Element("a").Value).ToString();
            txtL27.Text = double.Parse(xe.Element("第2级齿轮参数").Element("输入参数").Element("a").Value).ToString();
            txtL28.Text = double.Parse(xe.Element("第3级齿轮参数").Element("输入参数").Element("a").Value).ToString();

            first = cmbBoxDirection.Text;
        }

        private void rB1_CheckedChanged(object sender, EventArgs e)
        {
            if (rB1.Checked)
            {
                txtL8.Enabled = false;
                txtL8.BackColor = Color.Red;
            }
            else
            {
                txtL8.Enabled = true;
                txtL8.BackColor = Color.NavajoWhite;
            }
        }

        private void rB3_CheckedChanged(object sender, EventArgs e)
        {
            if (rB3.Checked)
            {
                txtL13.Enabled = false;
                txtL13.BackColor = Color.Red;
                txtL14.Enabled = false;
                txtL14.BackColor = Color.Red;
            }
            else
            {
                txtL13.Enabled = true;
                txtL13.BackColor = Color.NavajoWhite;
                txtL14.Enabled = true;
                txtL14.BackColor = Color.NavajoWhite;
            }
        }

        private void rB5_CheckedChanged(object sender, EventArgs e)
        {
            if (rB5.Checked)
            {
                txtL19.Enabled = false;
                txtL19.BackColor = Color.Red;
                txtL20.Enabled = false;
                txtL20.BackColor = Color.Red;
            }
            else
            {
                txtL19.Enabled = true;
                txtL19.BackColor = Color.NavajoWhite;
                txtL20.Enabled = true;
                txtL20.BackColor = Color.NavajoWhite;
            }

        }
        #region 各轴系尺寸定义
        //输出轴系各段轴长度，俯视图从下至上排列
        double output1, output2, output3, output4, output5, output6, output7, outputB, outputDa;
        //中间轴2长度，从上至下排列，B1，Da1代表齿轮轴的宽度齿顶圆
        double middleSec1, middleSec2, middleSec3, middleSec4, middleSec5, middleSec6, middleSec7, middleSecB1, middleSecB2, middleSecDa1, middleSecDa2;
        //中间轴1长度 从下至上  B1，Da1代表齿轮轴的宽度齿顶圆
        double middleFir1, middleFir2, middleFir3, middleFir4, middleFir5, middleFir6, middleFir7, middleFirB1, middleFirB2, middleFirDa1, middleFirDa2;
        //输入轴长度从上至下 
        double input1, input2, input3, input4, input5, input6, inputB, inputDa;
        // 判断旋向  angel装配草图旋向角度 first 输入轴旋向
        double angle;
        string first, second;

        #  endregion
        private void button4_Click(object sender, EventArgs e)
        {
            XElement xe = SystemConstants.XmlProject.Element("齿轮参数计算");
            #region   输出轴长度计算
            //IEnumerable<XElement> elements = from a in xe.Elements("第三级齿轮参数").Elements("输入参数")  // where PInfo.Attribute("ID").Value == strID
            //                               select a;
            outputB = double.Parse(xe.Element("第3级齿轮参数").Element("输入参数").Element("B").Value);  //输出轴齿宽
            //elements = from a in xe.Elements("第三级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            outputDa = double.Parse(xe.Element("第3级齿轮参数").Element("输出参数").Element("da2").Value);  //输出轴齿顶圆直径

            output1 = double.Parse(txtL3.Text);
            output2 = double.Parse(txtL4.Text) - double.Parse(txtL2.Text) / 2;
            output3 = double.Parse(txtL2.Text) - double.Parse(txtL9.Text) - outputB - double.Parse(txtL7.Text);
            output4 = double.Parse(txtL7.Text);
            if (rB1.Checked)
            {
                output5 = double.Parse(txtL9.Text) + outputB;
                output6 = double.Parse(txtL5.Text) - double.Parse(txtL2.Text) / 2;
            }
            else if (rB2.Checked)
            {
                output5 = double.Parse(txtL8.Text);
                output6 = double.Parse(txtL5.Text) + double.Parse(txtL2.Text) / 2 - output3;
            }
            output7 = double.Parse(txtL6.Text);
            #endregion
            #region   中间轴2计算
            //elements = from a in xe.Elements("第三级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            middleSecDa1 = double.Parse(xe.Element("第3级齿轮参数").Element("输出参数").Element("da1").Value);//中间轴2齿轮轴齿顶圆
            middleSecB1 = outputB + 10;//中间轴2齿轮轴齿宽
            //中间轴2齿轮齿宽
            //elements = from a in xe.Elements("第二级齿轮参数").Elements("输入参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            middleSecB2 = double.Parse(xe.Element("第2级齿轮参数").Element("输入参数").Element("B").Value);
            middleSecDa2 = double.Parse(xe.Element("第2级齿轮参数").Element("输出参数").Element("da2").Value);
            middleSec1 = double.Parse(txtL10.Text);
            middleSec2 = double.Parse(txtL9.Text) - double.Parse(txtL11.Text);

            middleSec3 = middleSecB1;
            middleSec4 = double.Parse(txtL12.Text) - (10 - double.Parse(txtL11.Text));
            if (rB3.Checked)
            {
                middleSec5 = middleSecB2 + 5; //齿轮所在轴短长度 默认齿宽+5
                middleSec6 = double.Parse(txtL2.Text) - middleSec2 - middleSec3 - middleSec4 - middleSec5;
            }
            else if (rB4.Checked)
            {
                middleSec5 = double.Parse(txtL13.Text);
                middleSec6 = double.Parse(txtL14.Text);
            }
            middleSec7 = double.Parse(txtL15.Text);


            #endregion
            #region 中间轴1计算
            //中间轴1齿轮轴齿顶圆
            //elements = from a in xe.Elements("第二级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            middleFirDa1 = double.Parse(xe.Element("第2级齿轮参数").Element("输出参数").Element("da1").Value);
            //中间轴1齿轮轴齿宽
            middleFirB1 = middleSecB2 + 10;

            //中间轴1齿轮齿宽
            //elements = from a in xe.Elements("第一级齿轮参数").Elements("输入参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            middleFirB2 = double.Parse(xe.Element("第1级齿轮参数").Element("输入参数").Element("B").Value);
            //中间轴1齿轮齿顶圆
            //elements = from a in xe.Elements("第一级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            middleFirDa2 = double.Parse(xe.Element("第1级齿轮参数").Element("输出参数").Element("da2").Value);
            middleFir1 = double.Parse(txtL16.Text);
            middleFir2 = double.Parse(txtL2.Text) - double.Parse(txtL9.Text) - outputB - double.Parse(txtL12.Text)
                          - middleSecB2 - (10 - double.Parse(txtL17.Text));

            middleFir3 = middleFirB1;
            middleFir4 = double.Parse(txtL18.Text) - double.Parse(txtL17.Text);
            if (rB5.Checked)
            {
                middleFir5 = middleFirB2 + 5; // 默认轴长度为齿宽+5
                middleFir6 = double.Parse(txtL2.Text) - middleFir2 - middleFir3 - middleFir4 - middleFir5;
            }
            else if (rB6.Checked)
            {
                middleFir5 = double.Parse(txtL19.Text);
                middleFir6 = double.Parse(txtL20.Text);
            }
            middleFir7 = double.Parse(txtL21.Text);
            #endregion
            #region 输入轴计算
            //elements = from a in xe.Elements("第一级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
            //           select a;
            inputDa = double.Parse(xe.Element("第1级齿轮参数").Element("输出参数").Element("da1").Value);
            inputB = middleFirB2 + 10;
            input1 = double.Parse(txtL23.Text);
            input2 = double.Parse(txtL9.Text) + double.Parse(txtL12.Text) + outputB - double.Parse(txtL18.Text) - middleFirB2 - double.Parse(txtL22.Text);
            input3 = inputB;
            input4 = double.Parse(txtL2.Text) - input2 - input3;
            input5 = double.Parse(txtL25.Text) - double.Parse(txtL2.Text) / 2;
            input6 = double.Parse(txtL24.Text);
            #endregion

            btSaveData.Enabled = true;
        }
        //检验尺寸函数
        private void txtL38_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                XElement xe = SystemConstants.XmlProject.Element("齿轮参数计算");
                //IEnumerable<XElement> elements = from a in xe.Elements("第三级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
                //                                 select a;
                double outda2 = double.Parse(xe.Element("第3级齿轮参数").Element("输出参数").Element("da2").Value);  //输出轴齿顶圆直径
                double midsecda1 = double.Parse(xe.Element("第3级齿轮参数").Element("输出参数").Element("da1").Value);//中间轴2齿轮轴齿顶圆


                //中间轴2齿轮齿顶圆
                //elements = from a in xe.Elements("第二级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
                //           select a;

                double midsecda2 = double.Parse(xe.Element("第2级齿轮参数").Element("输出参数").Element("da2").Value);

                //中间轴1齿轮齿顶圆
                //elements = from a in xe.Elements("第一级齿轮参数").Elements("输出参数")  // where PInfo.Attribute("ID").Value == strID
                //           select a;
                double midfirda2 = double.Parse(xe.Element("第1级齿轮参数").Element("输出参数").Element("da2").Value);

                # region 检验尺寸计算
                txtTest1.Text = (((double.Parse(txtL33.Text) + double.Parse(txtL26.Text)) * Math.Tan(15 * Math.PI / 180) + double.Parse(txtL32.Text)) * Math.Sin(75 * Math.PI / 180) - midfirda2 / 2).ToString();
                txtTest2.Text = (((double.Parse(txtL33.Text) + double.Parse(txtL26.Text) + double.Parse(txtL27.Text)) * Math.Tan(15 * Math.PI / 180) + double.Parse(txtL32.Text)) * Math.Sin(75 * Math.PI / 180) - midsecda2 / 2).ToString();
                txtTest3.Text = (double.Parse(txtL34.Text) - outda2 / 2 - double.Parse(txtL31.Text)).ToString();
                txtTest4.Text = (double.Parse(txtL29.Text) - outda2 / 2 - double.Parse(txtL31.Text)).ToString();
                txtTest5.Text = (double.Parse(txtL27.Text) - midfirda2 / 2 - midsecda1 / 2).ToString();
                # endregion
            }
        }


        private void btSaveData_Click(object sender, EventArgs e)
        {
            cmbBoxDirection_SelectedIndexChanged(sender, e);
            if (first == null)
            {
                MessageBox.Show("请指定齿轮旋向");
                cmbBoxDirection.Focus();
                return;
            }
               
            #region 保存到第一级结构参数
            //XElement xe = XElement.Load(@"D:\减速器设计系统\第一级结构参数.xml");
            XElement xe = SystemConstants.XmlProject.Element("第1级轴串");
            //IEnumerable<XElement> elements = from 轴段1长度 in xe.Elements("齿轮轴")
            //                                 select 轴段1长度;
            xe.Element("齿轮轴").Element("轴段1长度").Value = input1.ToString();
            xe.Element("齿轮轴").Element("轴段1直径").Value = txtD17.Text;
            xe.Element("齿轮轴").Element("轴段2长度").Value = input2.ToString();
            xe.Element("齿轮轴").Element("轴段2直径").Value = txtD20.Text;
            xe.Element("齿轮轴").Element("轴段4长度").Value = input4.ToString();
            xe.Element("齿轮轴").Element("轴段4直径").Value = txtD19.Text;
            xe.Element("齿轮轴").Element("轴段5长度").Value = input5.ToString();
            xe.Element("齿轮轴").Element("轴段5直径").Value = txtD18.Text;
            xe.Element("齿轮轴").Element("轴段6长度").Value = input6.ToString();
            xe.Element("齿轮轴").Element("轴段6直径").Value = txtD17.Text;
            xe.Element("齿轮轴").Element("旋向").Value = first;
            //elements = from Z2 in xe.Elements("齿轮")select Z2;
            xe.Element("齿轮").Element("旋向").Value = second;

            //xe.Save(@"D:\减速器设计系统\第一级结构参数.xml");
            #endregion
            #region 保存到第二级结构参数
            xe = SystemConstants.XmlProject.Element("第2级轴串");
            //elements = from 轴段1长度 in xe.Elements("齿轮轴")
            //           select 轴段1长度;
            xe.Element("齿轮轴").Element("轴段1长度").Value = middleFir1.ToString();
            xe.Element("齿轮轴").Element("轴段1直径").Value = txtD12.Text;
            xe.Element("齿轮轴").Element("轴段2长度").Value = middleFir2.ToString();
            xe.Element("齿轮轴").Element("轴段2直径").Value = txtD13.Text;
            xe.Element("齿轮轴").Element("轴段4长度").Value = middleFir4.ToString();
            xe.Element("齿轮轴").Element("轴段4直径").Value = txtD14.Text;
            xe.Element("齿轮轴").Element("轴段5长度").Value = middleFir5.ToString();
            xe.Element("齿轮轴").Element("轴段5直径").Value = txtD15.Text;
            xe.Element("齿轮轴").Element("轴段6长度").Value = middleFir6.ToString();
            xe.Element("齿轮轴").Element("轴段6直径").Value = txtD16.Text;
            xe.Element("齿轮轴").Element("轴段7长度").Value = middleFir7.ToString();
            xe.Element("齿轮轴").Element("轴段7直径").Value = txtD12.Text;
            xe.Element("齿轮轴").Element("旋向").Value = second;
            //elements = from Z2 in xe.Elements("齿轮")select Z2;
            xe.Element("齿轮").Element("旋向").Value = first;
            //xe.Save(@"D:\减速器设计系统\第二级结构参数.xml");
            #endregion
            #region 保存到第三级结构参数
            xe = SystemConstants.XmlProject.Element("第3级轴串");
            //elements = from 轴段1长度 in xe.Elements("齿轮轴")select 轴段1长度;
            xe.Element("齿轮轴").Element("轴段1长度").Value = middleSec1.ToString();
            xe.Element("齿轮轴").Element("轴段1直径").Value = txtD7.Text;
            xe.Element("齿轮轴").Element("轴段2长度").Value = middleSec2.ToString();
            xe.Element("齿轮轴").Element("轴段2直径").Value = txtD11.Text;
            xe.Element("齿轮轴").Element("轴段4长度").Value = middleSec4.ToString();
            xe.Element("齿轮轴").Element("轴段4直径").Value = txtD10.Text;
            xe.Element("齿轮轴").Element("轴段5长度").Value = middleSec5.ToString();
            xe.Element("齿轮轴").Element("轴段5直径").Value = txtD9.Text;
            xe.Element("齿轮轴").Element("轴段6长度").Value = middleSec6.ToString();
            xe.Element("齿轮轴").Element("轴段6直径").Value = txtD8.Text;
            xe.Element("齿轮轴").Element("轴段7长度").Value = middleSec7.ToString();
            xe.Element("齿轮轴").Element("轴段7直径").Value = txtD7.Text;
            xe.Element("齿轮轴").Element("旋向").Value = first;
            //elements = from Z2 in xe.Elements("齿轮")select Z2;
            xe.Element("齿轮").Element("旋向").Value = second;

            //xe.Save(@"D:\减速器设计系统\第三级结构参数.xml");
            #endregion
            #region 保存到输出轴结构参数
            xe = SystemConstants.XmlProject.Element("第4级轴串");
            //elements = from 轴段1长度 in xe.Elements("轴")select 轴段1长度;
            xe.Element("齿轮轴").Element("轴段1长度").Value = output1.ToString();
            xe.Element("齿轮轴").Element("轴段1直径").Value = txtD1.Text;
            xe.Element("齿轮轴").Element("轴段2长度").Value = output2.ToString();
            xe.Element("齿轮轴").Element("轴段2直径").Value = txtD2.Text;
            xe.Element("齿轮轴").Element("轴段3长度").Value = output3.ToString();
            xe.Element("齿轮轴").Element("轴段3直径").Value = txtD3.Text;
            xe.Element("齿轮轴").Element("轴段4长度").Value = output4.ToString();
            xe.Element("齿轮轴").Element("轴段4直径").Value = txtD4.Text;
            xe.Element("齿轮轴").Element("轴段5长度").Value = output5.ToString();
            xe.Element("齿轮轴").Element("轴段5直径").Value = txtD5.Text;
            xe.Element("齿轮轴").Element("轴段6长度").Value = output6.ToString();
            xe.Element("齿轮轴").Element("轴段6直径").Value = txtD2.Text;
            xe.Element("齿轮轴").Element("轴段7长度").Value = output7.ToString();
            xe.Element("齿轮轴").Element("轴段7直径").Value = txtD6.Text;
            #endregion

            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");
        }

        private void btQuit_Click(object sender, EventArgs e)
        {
            this.Close();
            process.CloseMainWindow();
            process.Close();
            
            //退出系统提示
            //if (MessageBox.Show("是否要退出本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
            //    Application.Exit();
            //}
        }

        private void cmbBoxDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            first = cmbBoxDirection.Text;
            if (first == "左")
            {
                angle = 20;
                second = "右";
            }
            else
            {
                angle = 160;
                second = "左";
            }
        }
    }
}
