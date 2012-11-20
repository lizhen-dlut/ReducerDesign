using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmGearCalculate : Form
    {
        private int _iGrade = 0;

        public FrmGearCalculate()
        {
            InitializeComponent();
        }

        private void frmGearCalculate_Load(object sender, EventArgs e)
        {

            SystemConstants.XmlProject = XElement.Load(SystemConstants.StrProjectPath + @"\减速器.xml");
            XElement tempXE = SystemConstants.XmlProject.Element("总体要求");
            if (tempXE != null)
            {
                groupBox1.Text = tempXE.Element("级数").Value + " 级减速器";
                labPower.Text = "总功率：" + tempXE.Element("总功率").Value;
                labSpeed.Text = "转  速：" + tempXE.Element("转速").Value;
                labN.Text = "总速比：" + tempXE.Element("总速比").Value;
                _iGrade = int.Parse(tempXE.Element("级数").Value);
                numericUpDown1.Maximum = Convert.ToDecimal(tempXE.Element("级数").Value);
            }
            var dt = new DataTable();
            dt.Columns.Add("项  目", typeof(string));
            for (int i = 1; i <= _iGrade; i++)
            {
                dt.Columns.Add("第 " + i + " 级", typeof(double));
            }
            string[] strGearParameters = new string[]
                                             {
                                                 "中心距",
                                                 "模数",
                                                 "小齿轮齿数",
                                                 "大齿轮齿数",
                                                 "螺旋角",
                                                 "变位系数 X1",
                                                 "压力角",
                                                 "齿顶高系数",
                                                 "顶隙系数",
                                                 "齿宽 B",
                                                 "变位系数 X2",
                                                 "齿顶高变动系数 dyn",
                                                 "齿顶高 ha1",
                                                 "齿顶高 ha2",
                                                 "分度圆 d1",
                                                 "齿顶圆 da1",
                                                 "齿根圆 df1",
                                                 "分度圆 d2",
                                                 "齿顶圆 da2",
                                                 "齿根圆 df2",
                                                 "跨齿数 k1",
                                                 "公法线 W1",
                                                 "跨齿数 k2",
                                                 "公法线 W2",
                                                 "端面重合度 Ea",
                                                 "轴向重合度 Eb",
                                                 "总重合度 E"
                                             };



            XElement tempGearXe = SystemConstants.XmlProject.Element("齿轮参数计算");

            string[] gearstring = new string[27]
                                      {
                                          "a", "Mn", "Z1", "Z2", "B1", "x1", "alpha", "ha", "c", "B", "x2", "dyn",
                                          "ha1", "ha2", "d1", "da1", "df1", "d2", "da2", "df2", "k1", "w1", "k2",
                                          "w2", "Ea", "Eb", "E"
                                      };
            double[,] dGear1 = new double[10, _iGrade]; //{ 140, 3, 18, 72, 11, 0.25, 20, 1, 0.25, 60 };



            for (int i = 0; i < _iGrade; i++)
            {
                if (tempGearXe != null)
                {
                    int ii = i + 1;
                    IEnumerable<XElement> elements = from a in tempGearXe.Elements("第" + ii + "级齿轮参数").Elements("输入参数")
                                                     select a;
                    for (int j = 0; j < 10; j++)
                    {
                        dGear1[j, i] = Convert.ToDouble(elements.ElementAt(0).Element(gearstring[j]).Value);
                        // = gearParameter2[i].ToString();
                    }
                }

            }

            for (int i = 0; i < 10; i++)
            {
                object[] tempObj = new object[_iGrade + 1];
                tempObj[0] = strGearParameters[i];
                for (int j = 0; j < _iGrade; j++)
                {
                    tempObj[j + 1] = dGear1[i, j];

                    //dt.Rows.Add(strGearParameters[i], dGear1[i,j], 22);
                }
                dt.Rows.Add(tempObj);
            }

            for (int i = 10; i < strGearParameters.Length; i++)
            {
                object[] tempObj = new object[_iGrade + 1];
                tempObj[0] = strGearParameters[i];
                for (int j = 0; j < _iGrade; j++)
                {
                    tempObj[j + 1] = 0;
                }
                dt.Rows.Add(tempObj);
            }
            bindingSource1.DataSource = dt.DefaultView;

            dataGridView1.DataSource = bindingSource1;

            //DataGridView禁止用户单击文本框列的标题，会对datagirdview中的数据排序 
            for (int i = 0; i < dataGridView1.Columns.Count; i++) //循环遍历DataGridView控件中的每一列
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable; //设定每一列的排序类型为不排序
            }

            // DataGridView禁止用户追加新行
            dataGridView1.AllowUserToAddRows = false;
            // 避免自动生成列
            dataGridView1.AutoGenerateColumns = false;

            string[] strDynamicParameters = new string[] { "转速 rpm", "功率 kw", "转矩 N·mm" };
            double[,] dDynamicParameters = new double[3, _iGrade + 1];
            dDynamicParameters[0, 0] = Convert.ToDouble(SystemConstants.XmlProject.Element("总体要求").Element("转速").Value);
            dDynamicParameters[1, 0] = Convert.ToDouble(SystemConstants.XmlProject.Element("总体要求").Element("总功率").Value);
            dDynamicParameters[2, 0] = 9550000 * dDynamicParameters[1, 0] / dDynamicParameters[0, 0];
            for (int i = 1, j = 0; i < _iGrade + 1; i++, j++)
            {
                dDynamicParameters[0, i] = dDynamicParameters[0, i - 1] /
                                           ((double)dataGridView1[i, 3].Value / (double)dataGridView1[i, 2].Value);
                dDynamicParameters[1, i] = dDynamicParameters[1, i - 1] * 0.99 * 0.97;
                dDynamicParameters[2, i] = 9550000 * dDynamicParameters[1, i] / dDynamicParameters[0, i];
            }
            //SystemConstants.XmlProject.Element("总体要求").Element("结构形式").Value = cmbTypeOfReducer.Text;
            //SystemConstants.XmlProject.Element("总体要求").Element("总功率").Value = txtP.Text;
            //SystemConstants.XmlProject.Element("总体要求").Element("转速").Value = txtN.Text;
            //SystemConstants.XmlProject.Element("总体要求").Element("总速比").Value = txtTotalRate.Text;
            //SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");

            var dtDynamicParameters = new DataTable();
            dtDynamicParameters.Columns.Add("项  目", typeof(string));
            dtDynamicParameters.Columns.Add("输入轴", typeof(double));
            for (int i = 1; i < _iGrade; i++)
            {
                dtDynamicParameters.Columns.Add("中间轴 " + i, typeof(double));
            }
            dtDynamicParameters.Columns.Add("输出轴", typeof(double));

            for (int i = 0; i < 3; i++)
            {
                var tempObj = new object[_iGrade + 2];
                tempObj[0] = strDynamicParameters[i];
                for (int j = 0; j <= _iGrade; j++)
                {
                    tempObj[j + 1] = 0; // dDynamicParameters[i, j];

                    //dt.Rows.Add(strGearParameters[i], dGear1[i,j], 22);
                }
                dtDynamicParameters.Rows.Add(tempObj);
            }

            bindingSource2.DataSource = dtDynamicParameters.DefaultView;

            dataGridView2.DataSource = bindingSource2;
            //DataGridView禁止用户单击文本框列的标题，会对datagirdview中的数据排序 
            for (int i = 0; i < dataGridView2.Columns.Count; i++) //循环遍历DataGridView控件中的每一列
            {
                dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable; //设定每一列的排序类型为不排序
            }

            // DataGridView禁止用户追加新行
            dataGridView2.AllowUserToAddRows = false;
            // 避免自动生成列
            dataGridView2.AutoGenerateColumns = false;
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("aa");
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var gearcmp1 = new BasicGeraCalculte();
                var t = dataGridView1[1, 2].Value;

                gearcmp1.A1 = (double)dataGridView1[1, 0].Value;
                gearcmp1.Mn = (double)dataGridView1[1, 1].Value;
                gearcmp1.Z1 = int.Parse(s: dataGridView1[1, 2].Value.ToString());
                gearcmp1.Z2 = int.Parse(s: dataGridView1[1, 3].Value.ToString());
                gearcmp1.Lxj = (double)dataGridView1[1, 4].Value;
                gearcmp1.X1 = (double)dataGridView1[1, 5].Value;
                gearcmp1.An = (double)dataGridView1[1, 6].Value;
                gearcmp1.H = (double)dataGridView1[1, 7].Value;
                gearcmp1.C = (double)dataGridView1[1, 8].Value;
                gearcmp1.B = int.Parse(s: dataGridView1[1, 9].Value.ToString());
                gearcmp1.Nbjs();

                dataGridView1[1, 10].Value = gearcmp1.X2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 11].Value = gearcmp1.Ha1.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 12].Value = gearcmp1.Ha2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 13].Value = gearcmp1.D1.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 14].Value = gearcmp1.Da1.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 15].Value = gearcmp1.Df1.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 16].Value = gearcmp1.D2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 17].Value = gearcmp1.Da2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 18].Value = gearcmp1.Df2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 19].Value = gearcmp1.K1.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 20].Value = gearcmp1.W1.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 21].Value = gearcmp1.K2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 22].Value = gearcmp1.W2.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 23].Value = gearcmp1.Ea.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 24].Value = gearcmp1.Eb.ToString(CultureInfo.InvariantCulture);
                dataGridView1[1, 25].Value = gearcmp1.E.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                MessageBox.Show("aa");
                throw;
            }

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            // MessageBox.Show("数据输入错误");
            //dataGridView1.CausesValidation = true;
            //string a = dataGridView1.Columns[e.ColumnIndex].ValueType.ToString();   //获取当前所处字段的类型
            //if (MessageBox.Show("请输入数字！", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{

            //    e.Cancel = true;

            //}
            //else
            //{
            //    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            //    e.Cancel = false;
            //}

            MessageBox.Show("第 " + anError.RowIndex + " 数据输入错误");

            //if (anError.Context == DataGridViewDataErrorContexts.Commit)
            //{
            //    MessageBox.Show("Commit error");
            //}
            //if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            //{
            //    MessageBox.Show("Cell change");
            //}
            //if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            //{
            //    MessageBox.Show("parsing error");
            //}
            //if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            //{
            //    MessageBox.Show("leave control error");
            //}

            //if ((anError.Exception) is ConstraintException)
            //{
            //    DataGridView view = (DataGridView)sender;
            //    view.Rows[anError.RowIndex].ErrorText = "an error";
            //    view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

            //    anError.ThrowException = false;
            //}


        }

        private void butSystemCalculate_Click(object sender, EventArgs e)
        {
            var gearParameters = new BasicGeraCalculte[_iGrade];

            for (int i = 0, j = 1; i < _iGrade; i++, j++)
            {
                gearParameters[i] = new BasicGeraCalculte
                                        {
                                            A1 = (double)dataGridView1[j, 0].Value,
                                            Mn = (double)dataGridView1[j, 1].Value,
                                            Z1 = int.Parse(s: dataGridView1[j, 2].Value.ToString()),
                                            Z2 = int.Parse(s: dataGridView1[j, 3].Value.ToString()),
                                            Lxj = (double)dataGridView1[j, 4].Value,
                                            X1 = (double)dataGridView1[j, 5].Value,
                                            An = (double)dataGridView1[j, 6].Value,
                                            H = (double)dataGridView1[j, 7].Value,
                                            C = (double)dataGridView1[j, 8].Value,
                                            B = int.Parse(s: dataGridView1[j, 9].Value.ToString())
                                        };
                gearParameters[i].Nbjs();

                dataGridView1[j, 10].Value = gearParameters[i].X2.ToString("f5");
                dataGridView1[j, 11].Value = gearParameters[i].Dyn.ToString("f5");
                dataGridView1[j, 12].Value = gearParameters[i].Ha1.ToString("f5");
                dataGridView1[j, 13].Value = gearParameters[i].Ha2.ToString("f5");
                dataGridView1[j, 14].Value = gearParameters[i].D1.ToString("f5");
                dataGridView1[j, 15].Value = gearParameters[i].Da1.ToString("f5");
                dataGridView1[j, 16].Value = gearParameters[i].Df1.ToString("f5");
                dataGridView1[j, 17].Value = gearParameters[i].D2.ToString("f5");
                dataGridView1[j, 18].Value = gearParameters[i].Da2.ToString("f5");
                dataGridView1[j, 19].Value = gearParameters[i].Df2.ToString("f5");
                dataGridView1[j, 20].Value = gearParameters[i].K1.ToString("f5");
                dataGridView1[j, 21].Value = gearParameters[i].W1.ToString("f5");
                dataGridView1[j, 22].Value = gearParameters[i].K2.ToString("f5");
                dataGridView1[j, 23].Value = gearParameters[i].W2.ToString("f5");
                dataGridView1[j, 24].Value = gearParameters[i].Ea.ToString("f5");
                dataGridView1[j, 25].Value = gearParameters[i].Eb.ToString("f5");
                dataGridView1[j, 26].Value = gearParameters[i].E.ToString("f5");
            }

            #region 速比误差计算

            double dRatio = 1.0;
            for (int i = 0; i < gearParameters.Length; i++)
            {
                dRatio = dRatio * gearParameters[i].Z2 / gearParameters[i].Z1;
            }

            double dSpeedError = (Math.Abs(double.Parse(SystemConstants.XmlProject.Element("总体要求").Element("总速比").Value) - dRatio)) / double.Parse(SystemConstants.XmlProject.Element("总体要求").Element("总速比").Value);
            labSpeedC.Text = labSpeedC.Text + "：" + dRatio.ToString("f4");
            labSpeedError.Text = labSpeedError.Text + "：" + dSpeedError.ToString("f4");

            SystemConstants.XmlProject.Element("齿轮参数计算").Element("计算速比").Value = dRatio.ToString("f8");
            SystemConstants.XmlProject.Element("齿轮参数计算").Element("速比误差").Value = dSpeedError.ToString("f8");

            #endregion

            #region 轴的动力参数计算

            double[,] dDynamicParameters = new double[3, _iGrade + 1];
            dDynamicParameters[0, 0] = Convert.ToDouble(SystemConstants.XmlProject.Element("总体要求").Element("转速").Value);
            dDynamicParameters[1, 0] = Convert.ToDouble(SystemConstants.XmlProject.Element("总体要求").Element("总功率").Value);
            dDynamicParameters[2, 0] = 9550000 * dDynamicParameters[1, 0] / dDynamicParameters[0, 0];
            for (int i = 1, j = 0; i < _iGrade + 1; i++, j++)
            {
                dDynamicParameters[0, i] = dDynamicParameters[0, i - 1] / ((double)dataGridView1[i, 3].Value / (double)dataGridView1[i, 2].Value);
                dDynamicParameters[1, i] = dDynamicParameters[1, i - 1] * 0.99 * 0.97;
                dDynamicParameters[2, i] = 9550000 * dDynamicParameters[1, i] / dDynamicParameters[0, i];
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j <= _iGrade + 1; j++)
                {
                    dataGridView2[j, i].Value = dDynamicParameters[i, j - 1].ToString("f4");
                }
            }

            for (int j = 1; j <= _iGrade + 1; j++)
            {
                SystemConstants.XmlProject.Element("第" + j + "级轴串").Element("动力参数").Element("转速").Value =
                   dDynamicParameters[0, j - 1].ToString("f8");
                SystemConstants.XmlProject.Element("第" + j + "级轴串").Element("动力参数").Element("功率").Value =
                   dDynamicParameters[1, j - 1].ToString("f8");
                SystemConstants.XmlProject.Element("第" + j + "级轴串").Element("动力参数").Element("扭矩").Value =
                   dDynamicParameters[2, j - 1].ToString("f8");
            }

            #endregion

            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");

        }

        private void butSingleCalculat_Click(object sender, EventArgs e)
        {
            SystemConstants.SingleCalcultGrade = (int)numericUpDown1.Value;

            FrmSingleGradeCalculate childFrame = new FrmSingleGradeCalculate();
            childFrame.Show();
            this.Close();
        }
    }
}
