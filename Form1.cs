using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using ZedGraph;


namespace ReducerDesign
{
    public partial class Form1 : Form
    {
        private DataSet _newDataSet;
        private XmlDocument xmlDoc;
        private string fileName;

        private MyPlot DrawPic;

        public Form1()
        {
            InitializeComponent();
            DrawPic = new MyPlot();
        }

        //外啮合齿轮参数计算
        private double[] WGeoCal(double zx, double mn2, double lb12, double zd, double hax1, double had1,
            double xnx, double xnd, double cnx, double cnd, double an2, double dpqx, double dpqd, double bx)
        { ////dfd
            //下面的为输入参数
            /*
             * zx 小齿轮齿数
             * mn2 法向模数
             * lb12 螺旋角，单位：度
             * zd 大齿轮齿数
             * hax1 小齿轮齿顶高系数
             * had1 大齿轮齿顶高系数
             * xnx 小齿轮法向变位系数
             * xnd 大齿轮法向变位系数
             * cnx 小齿轮顶隙系数 
             * cnd 大齿轮顶隙系数 
             * an2 压力角
             * dpqx 小齿轮圆棒(球)直径（mm）
             * dpqd 大齿轮圆棒(球)直径（mm）
             * bx 齿宽（mm）
             */


            //下面的为计算过程
            double[] returnResult = new double[52];
            double rangel = Math.PI / 18;
            double rangeR = Math.PI / 6;
            double at12 = Math.Atan(Math.Tan(an2 * Math.PI / 180.0) / Math.Cos(lb12 * Math.PI / 180.0));
            double at2 = at12 * 180.0 / Math.PI;
            returnResult[0] = at2;

            double iap2 = 2 * (xnx + xnd) / (zd + zx) * Math.Tan(an2 * Math.PI / 180.0) + Math.Tan(at2 * Math.PI / 180.0) - at2 * Math.PI / 180.0;
            SolveFun Funi = new SolveFun();
            double aph2 = Funi.Gexianfa(rangel, rangeR, 0.000000001, iap2);
            double ap2 = aph2 * 180.0 / Math.PI;
            returnResult[1] = ap2;                                                             //求端面啮合角

            double a2 = mn2 * (zx + zd) * Math.Cos(at2 * Math.PI / 180.0) / (2 * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(aph2));
            returnResult[2] = a2;
            double xnmnx = xnx * mn2;
            double xnmnd = xnd * mn2;
            returnResult[3] = xnmnx;
            returnResult[4] = xnmnd;

            double hfx1 = hax1 + cnx;
            double hfd1 = had1 + cnd;
            returnResult[5] = hfx1;
            returnResult[6] = hfd1;

            double mt2 = mn2 / Math.Cos(lb12 * Math.PI / 180.0);
            returnResult[7] = mt2;
            double dx = (zx * mn2) / Math.Cos(lb12 * Math.PI / 180.0);
            returnResult[8] = dx;                                                              //求小齿轮分度圆直径


            double dd = (zd * mn2) / Math.Cos(lb12 * Math.PI / 180);
            returnResult[9] = dd;                                                             //求大齿轮分度圆直径

            double u2 = zd / zx;
            returnResult[10] = u2;
            double dpx = 2 * a2 / (u2 + 1);
            double dpd = u2 * dpx;
            returnResult[11] = dpx;                                                            //求小齿轮节圆直径  
            returnResult[12] = dpd;                                                            //求大齿轮节圆直径


            double dax = dx + 2 * (hax1 + xnx - xnx - xnd + a2 / mn2 - 0.5 * (zx + zd) / Math.Cos(lb12 * Math.PI / 180.0)) * mn2;
            double dad = dd + 2 * (hax1 + xnd - xnx - xnd + a2 / mn2 - 0.5 * (zx + zd) / Math.Cos(lb12 * Math.PI / 180.0)) * mn2;
            returnResult[13] = dax;                                                            //求小齿轮齿顶圆直径              
            returnResult[14] = dad;                                                            //求大齿轮齿顶圆直径


            double dfx = dx - 2 * (hax1 + cnx - xnx) * mn2;
            double dfd = dd - 2 * (hax1 + cnx - xnd) * mn2;
            returnResult[15] = dfx;                                                            //求小齿轮齿根圆直径
            returnResult[16] = dfd;                                                            //求大齿轮齿根圆直径



            double dbx = dx * Math.Cos(at2 * Math.PI / 180);
            double dbd = dd * Math.Cos(at2 * Math.PI / 180);
            returnResult[17] = dbx;                                                            //求小齿轮基圆直径
            returnResult[18] = dbd;                                                            //求大齿轮基圆直径

            double hax = 0.5 * (dax - dx);
            double had = 0.5 * (dad - dd);
            returnResult[19] = hax;                                                            //求小齿轮齿顶高
            returnResult[20] = had;                                                            //求大齿轮齿顶高

            double hfx2 = 0.5 * (dx - dfx);
            double hfd2 = 0.5 * (dd - dfd);
            returnResult[21] = hfx2;                                                           //求小齿轮齿跟高
            returnResult[22] = hfd2;                                                           //求大齿轮齿跟高

            double hx = hax + hfx2;
            double hd = had + hfd2;
            returnResult[23] = hx;                                                             //求小齿轮全齿高
            returnResult[24] = hd;                                                             //求大齿轮全齿高


            double scnx = mn2 * Math.Cos(an2 * Math.PI / 180.0) * Math.Cos(an2 * Math.PI / 180.0) * (Math.PI / 2.0 + 2 * xnx * Math.Tan(an2 * Math.PI / 180.0));
            double hcnx = hax - 0.182 * scnx;
            returnResult[25] = scnx;                                                            //求小齿轮固定弦齿厚
            returnResult[26] = hcnx;                                                            //求小齿轮固定弦齿高


            double aatd = Math.Acos(dbd / dad);
            double aatx = Math.Acos(dbx / dax);
            double aad = aatd * 180.0 / Math.PI;
            double aax = aatx * 180.0 / Math.PI;
            returnResult[27] = aad;                                                            //求大齿轮齿顶压力角
            returnResult[28] = aax;                                                            //求小齿轮齿顶压力角

            double scnd = mn2 * Math.Cos(an2 * Math.PI / 180.0) * Math.Cos(an2 * Math.PI / 180.0) * (Math.PI / 2.0 + 2 * xnd * Math.Tan(an2 * Math.PI / 180.0));
            double hcnd = had - 0.182 * scnd;
            returnResult[29] = scnd;                                                           //求大齿轮固定弦齿厚
            returnResult[30] = hcnd;                                                           //求大齿轮固定弦齿高

            double dtx = (90 + 41.7 * xnx) / (zx / (Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0)));
            double srnx = zx / (Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0)) * mn2 * Math.Sin(dtx * Math.PI / 180.0);
            double hrnx = hax + 0.5 * zx / (Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0)) * mn2 * (1 - Math.Cos(dtx * Math.PI / 180.0));
            returnResult[31] = srnx;                                                           //求小齿轮分度圆弦齿厚
            returnResult[32] = hrnx;                                                           //求小齿轮分度圆弦齿高

            double dtd = (90 + 41.7 * xnd) / (zd / (Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0)));
            double srnd = zd / (Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0)) * mn2 * Math.Sin(dtd * Math.PI / 180.0);
            double hrnd = had + 0.5 * zd / (Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0) * Math.Cos(lb12 * Math.PI / 180.0)) * mn2 * (1 - Math.Cos(dtd * Math.PI / 180.0));
            returnResult[33] = srnd;                                                           //求大齿轮分度圆弦齿厚
            returnResult[34] = hrnd;                                                           //求大齿轮分度圆弦齿高

            double zpx = zx * (Math.Tan(at2 * Math.PI / 180.0) - at2 * Math.PI / 180.0) / (Math.Tan(an2 * Math.PI / 180.0) - an2 * Math.PI / 180.0);
            double kx = an2 * zpx / 180.0 + 0.5 + 2 * xnx / Math.Tan(an2 * Math.PI / 180.0) / Math.PI;
            double zpd = zpx * zd / zx;
            double kd = an2 * zpd / 180.0 + 0.5 + 2 * xnd / Math.Tan(an2 * Math.PI / 180.0) / Math.PI;
            returnResult[35] = kx;                                                             //求小齿轮公法线跨齿数
            returnResult[36] = kd;                                                             //求大齿轮公法线跨齿数

            double dtwnx = 2 * xnx * Math.Sin(an2 * Math.PI / 180.0);
            double wknx = Math.Cos(an2 * Math.PI / 180.0) * ((Math.PI * (kx - 0.5) + zpx * (Math.Tan(an2 * Math.PI / 180.0) - an2 * Math.PI / 180.0)));
            double wnkx = (wknx + dtwnx) * mn2;
            double dtwnd = 2 * xnd * Math.Sin(an2 * Math.PI / 180.0);
            double wknd = Math.Cos(an2 * Math.PI / 180.0) * ((Math.PI * (kd - 0.5) + zpd * (Math.Tan(an2 * Math.PI / 180.0) - an2 * Math.PI / 180.0)));
            double wnkd = (wknd + dtwnd) * mn2;
            returnResult[37] = wnkx;                                                           //求小齿轮公法线长度
            returnResult[38] = wnkd;                                                           //求大齿轮公法线长度


            double iamx = Math.Tan(at2 * Math.PI / 180.0) - at2 * Math.PI / 180.0 + dpqx / (mn2 * zx * Math.Cos(an2 * Math.PI / 180.0)) - 0.5 * Math.PI / zx + 2 * xnx * Math.Tan(an2 * Math.PI / 180.0) / zx;


            SolveFun Fun3 = new SolveFun();
            double amxh = Fun3.Gexianfa(rangel, rangeR, 0.0000000001, iamx);


            double iamd = Math.Tan(at2 * Math.PI / 180.0) - at2 * Math.PI / 180.0 + dpqx / (mn2 * zd * Math.Cos(an2 * Math.PI / 180.0)) - 0.5 * Math.PI / zd + 2 * xnd * Math.Tan(an2 * Math.PI / 180.0) / zd;


            SolveFun Fun4 = new SolveFun();
            double amdh = Fun4.Gexianfa(rangel, rangeR, 0.000000001, iamd);

            double amx = amxh * 180 / Math.PI;
            double amd = amdh * 180 / Math.PI;

            returnResult[39] = amx;                                                            //求小齿轮量棒中心所在圆的压力角
            returnResult[40] = amd;                                                            //求大齿轮量棒中心所在圆的压力角

            double mqx;
            if (zx % 2 == 0)
                mqx = mt2 * zx * Math.Cos(at2 * Math.PI / 180.0) / Math.Cos(amxh) + dpqx;
            else
                mqx = mt2 * zx * Math.Cos(at2 * Math.PI / 180.0) / Math.Cos(amxh) * Math.Cos(90 / zx * Math.PI / 180.0) + dpqx;

            double mqd;
            if (zd % 2 == 0)
                mqd = mt2 * zd * Math.Cos(at2 * Math.PI / 180.0) / Math.Cos(amdh) + dpqd;
            else
                mqd = mt2 * zd * Math.Cos(at2 * Math.PI / 180.0) / Math.Cos(amdh) * Math.Cos(90 / zd * Math.PI / 180.0) + dpqd;



            returnResult[41] = mqx;                                                            //求小齿轮跨棒（球）距
            returnResult[42] = mqd;                                                            //求大齿轮跨棒（球）距

            double snax = mn2 * (Math.PI / 2.0 + 2 * Math.Tan(an2 * Math.PI / 180.0) * xnx);
            double snad = mn2 * (Math.PI / 2.0 + 2 * Math.Tan(an2 * Math.PI / 180.0) * xnd);
            returnResult[43] = snax;                                                           //求小齿轮法向齿顶厚
            returnResult[44] = snad;                                                           //求大齿轮法向齿顶厚

            double dc2 = 0.5 / Math.PI * (zx * (Math.Tan(aatx) - Math.Tan(aph2)) + zd * (Math.Tan(aatd) - Math.Tan(aph2)));
            double zc2 = bx * Math.Sin(lb12 * Math.PI / 180.0) / (Math.PI * mn2);
            double dzc2 = dc2 + zc2;
            returnResult[45] = dc2;                                                            //求端面重合度
            returnResult[46] = zc2;                                                            // 求轴向重合度
            returnResult[47] = dzc2;                                                           //求总重合度

            double ba2 = bx / a2;
            double bd12 = bx / dx;
            returnResult[48] = ba2;                                                            //求齿宽系数
            returnResult[49] = bd12;                                                           //求宽径比系数

            double lx = (Math.Tan(aatd) - Math.Tan(aph2)) * ((u2 + 1) / u2) / ((1 + zx / zd) * Math.Tan(aph2) - Math.Tan(aatd));
            double ld = (Math.Tan(aatx) - Math.Tan(aph2)) * (u2 + 1) / ((1 + zd / zx) * Math.Tan(aph2) - Math.Tan(aatx));
            returnResult[50] = lx;
            returnResult[51] = ld;
            //计算完毕，返回结果
            return returnResult;
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            PointPairList list1 = new PointPairList();
           
            list1 = circleCalculate(0,0, double.Parse(dgvDiameter[3,0].Value.ToString().Trim()) * 0.5);           
            LineItem myCurve = GraphControl.GraphPane.AddCurve("第一级输入齿轮", list1, Color.Blue, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 
            //Make the curve smooth
            myCurve.Line.IsSmooth = true;

            list1 = circleCalculate(double.Parse(dataGridView[2, 0].Value.ToString().Trim()), 0, double.Parse(dgvDiameter[4,0].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第一级输出齿轮", list1, Color.Gold, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dataGridView[2, 0].Value.ToString().Trim()), 0, double.Parse(dgvDiameter[3, 1].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第二级输入齿轮", list1, Color.Red, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dataGridView[2, 0].Value.ToString().Trim()) + double.Parse(dataGridView[2, 1].Value.ToString().Trim()), 0, 
                double.Parse(dgvDiameter[4, 1].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第二级输出齿轮", list1, Color.Green , SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dataGridView[2, 0].Value.ToString().Trim()) + double.Parse(dataGridView[2, 1].Value.ToString().Trim()), 0, 
                double.Parse(dgvDiameter[3, 2].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第三级输入齿轮", list1, Color.Pink, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dataGridView[2, 0].Value.ToString().Trim()) + double.Parse(dataGridView[2, 1].Value.ToString().Trim())+
                double.Parse(dataGridView[2, 2].Value.ToString().Trim()) , 0,
                double.Parse(dgvDiameter[4, 2].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第三级输出齿轮", list1, Color.Purple , SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 
            
            GraphControl.AxisChange();
            graphPane_AxisChangeEvent(GraphControl.GraphPane);
            GraphControl.Show();

          
            /*//齿轮参数定义
            /*int transSeries = int.Parse(transmissionSeriesTextBox.Text);//传动级数赋值
            double[] transRatio = new double[transSeries];//传动比数组定义
            double[] Mn = new double[transSeries];//模数数组定义
            double[] a = new double[transSeries];//中心距数组定义
            double[] a0 = new double[transSeries];//变位中心距数组定义
            double[] Z1 = new double[transSeries];//小齿轮齿数数组定义
            double[] Z2 = new double[transSeries];//大齿轮齿数组定义
            double[] B1 = new double[transSeries];//螺旋角数组定义
            double[] b = new double[transSeries];//齿宽数组定义
            double[] An = new double[transSeries];//压力角数组定义
            double[] at = new double[transSeries];//端面压力角数组定义
            double[] atp = new double[transSeries];//啮合角的cos值数组定义  
            double[] atpie = new double[transSeries];//反余弦啮合角数组定义
            double[] invat = new double[transSeries];//端面压力角渐开线函数值数组定义
            double[] invatpie = new double[transSeries];//啮合角渐开线函数值数组定义
            double[] Xn = new double[transSeries];//总变位系数数组定义
            double[] Yn = new double[transSeries];//中心距变动系数数组定义
            double[] dy = new double[transSeries];//齿顶高变动系数数组定义
            double[] x1 = new double[transSeries];//小齿轮变位系数数组定义
            double[] x2 = new double[transSeries]; //大齿轮变位系数数组定义
            double[] hax1 = new double[transSeries];//小齿顶圆变位系数数组定义
            double[] hax2 = new double[transSeries]; //大齿顶圆变位系数数组定义
            double[] ha1 = new double[transSeries];//小齿齿顶高数组定义
            double[] ha2 = new double[transSeries]; //大齿齿顶高数组定义
            double[] da1 = new double[transSeries];//小齿顶圆直径数组定义
            double[] da2 = new double[transSeries];    //大齿顶圆直径数组定义   
            double[] d1 = new double[transSeries];//小齿分度圆直径数组定义
            double[] d2 = new double[transSeries];    //大齿分度圆直径数组定义   */

            /*double pianYi = 0;//X轴绘图时每次的平移量
            double pianyiYfu = 10;//负Y轴标线多出的部分
            double pianyiYzheng = 80;//正Y轴标线多出的部分
            //绘图

            ////////////////////////////////////////////
            //WGeoClass newGeoClass = new WGeoClass();
            double[] CalResult = new double[52];
            CalResult = WGeoCal(zx, mn2, lb12, zd, hax1, had1, xnx, xnd, cnx, cnd, an2, dpqx, dpqd, bx);
           

            //MessageBox.Show(x1[i].ToString());
            // MessageBox.Show(dy[i].ToString());//这个数不一样？？？？？？
            // MessageBox.Show(ha1[i].ToString());
            // MessageBox.Show(ha2[i].ToString());
            // MessageBox.Show(da1[i].ToString());//为什么会有误差呢？



            //da1[0] = 63.6934368;
            //da2[0] = 187.9892985;
            // da1[1] = 88.5131932;
            // da2[1] = 226.8275775;
            // da1[2] = 105.8859505;
            // da2[2] = 313.7395377;
            double r;
            double theta;
            double[] x = new double[1001];
            double[] xx = new double[1001];
            double[] y = new double[1001];
            // double[] xx = new double[1001];
            //a[0] = 120;
            // a[1] = 150;
            // a[2] = 200;

            double aAll = 0;
            double xGanshe1 = 0;
            double xGanshe2 = 0;
            String title = "";

            for (int i = transSeries - 1; i >= 0; i--)
            {
                r = 0.5 * da2[i];

                for (int j = 0; j <= 1000; j++)
                {
                    theta = 2 * Math.PI / 1000 * j;
                    x[j] = pianYi + r * Math.Cos(theta);
                    y[j] = r * Math.Sin(theta);
                    // xx[i] = x[i] + 10;
                }
                ///

                DrawPic.MyDrawPic1(this.GraphControl, x, y, title, "", "");

                // pianyiy = pianyiy + 0.5 * da2[transSeries - 1 - i] + 30;
                for (int j = 0; j <= 1000; j++)
                {
                    x[j] = pianYi;
                    y[j] = -0.5 * da2[i] - pianyiYfu + (0.5 * da2[transSeries - 1] + pianyiYzheng + 0.5 * da2[i]) / 1000 * j;
                    // xx[i] = x[i] + 10;
                }
                DrawPic.MyDrawPic1(this.GraphControl, x, y, title, "", "");

                pianYi = pianYi + a[i];

                r = 0.5 * da1[i];
                for (int j = 0; j <= 1000; j++)
                {
                    theta = 2 * Math.PI / 1000 * j;
                    x[j] = pianYi + r * Math.Cos(theta);
                    y[j] = r * Math.Sin(theta);
                    // xx[i] = x[i] + 10;

                }



                DrawPic.MyDrawPic1(this.GraphControl, x, y, title, "", "");


            }

            for (int j = 0; j <= 1000; j++)
            {

                x[j] = pianYi;
                y[j] = -0.5 * da1[0] - pianyiYfu + (0.5 * da1[0] + 0.5 * da2[transSeries - 1] + pianyiYzheng) / 1000 * j;
                // xx[i] = x[i] + 10;

            }
            DrawPic.MyDrawPic1(this.GraphControl, x, y, title, "", "");



            // 干涉检查线绘制
            for (int i = transSeries - 1; i >= 2; i--)
            {
                aAll = aAll + a[i];

                xGanshe1 = aAll + 0.5 * da1[i];

                xGanshe2 = aAll + a[i - 1] - 0.5 * da2[i - 2];
                MessageBox.Show((xGanshe2 - xGanshe1).ToString());

                for (int j = 0; j <= 1000; j++)
                {
                    x[j] = xGanshe1;
                    y[j] = (0.5 * da2[transSeries - 1] + pianyiYzheng - 40) / 1000 * j;
                    xx[j] = xGanshe2;
                }


                DrawPic.MyDrawPic1(this.GraphControl, x, y, title, "", "");
                DrawPic.MyDrawPic1(this.GraphControl, xx, y, title, "", "");

                // DrawPic.MyPointValueHandler(this.GraphControl,"" ,"", 2);

                //齿轮距离值输出？？？？
                //Font fnt = new Font("Verdana", 16);
                //  Graphics g = new Graphics();
                //  g.DrawString(（(xGanshe2 - xGanshe1).ToString()）, fnt, new SolidBrush(Color.Red), x[j], y[j]);


            }




            //X轴水平线绘制

            aAll = 0;
            for (int i = transSeries - 1; i >= 0; i--)
            {
                aAll = aAll + a[i];
            }

            for (int j = 0; j <= 1000; j++)
            {

                x[j] = -0.5 * da2[transSeries - 1] - 10 + (20 + aAll + 0.5 * da2[transSeries - 1] + 0.5 * da1[0]) / 1000 * j;//这个地方要改
                y[j] = 0;
            }

            DrawPic.MyDrawPic1(this.GraphControl, x, y, title, "", "");
            */


        }

        private void MessageBox(string p)
        {
            throw new NotImplementedException();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {

        }

        private void escButton_Click(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            /*try
           {
               if (dataGridView.Rows.Count > 1)
               {
                   var result = MessageBox.Show("是否加载", "提示", MessageBoxButtons.YesNo);
                   if (result == DialogResult.Yes)
                   {

                       if (openFileDialog1.ShowDialog() == DialogResult.OK)
                       {
                           _newDataSet.Clear();
                           string fileNames = openFileDialog1.FileName;
                           _newDataSet.ReadXml(fileNames);
                           // DataSet1.ReadXml(@"E:\Items.xml", XmlReadMode.Auto);
                           dataGridView.DataSource = _newDataSet.Tables[0];
                       }
                   }
               }
               else if (openFileDialog1.ShowDialog() == DialogResult.OK)
               {*/

            /* XmlNodeList nodes = xmlDoc.SelectNodes("//基本信息");
             foreach (XmlNode node in nodes)
             {
                 XmlElement xe = (XmlElement)node;
                 string name = xe.ChildNodes[0].InnerText;//.InnerText; //("总传动比");
                 string leibie = xe.ChildNodes[1].InnerText; //GetAttribute("传动级数");
                 tbxTotalTransmissionRatio.Text = name;
                 tbxTransmissionSeries.Text = leibie;                        
             }
                    */

            XmlNodeList nodes = xmlDoc.SelectNodes("//各级齿轮参数");

            /*   comboBox1.Items.Clear();
               for (int i = 1; i <= nodes.Count; i++)
               {
                   comboBox1.Items.Add(i);
               }*/

            foreach (XmlNode node in nodes)
            {
                XmlElement xe = (XmlElement)node;
                string jishu = xe.ChildNodes[0].InnerText;//级数
                string moshu = xe.ChildNodes[1].InnerText; //模数
                string zhongxinju = xe.ChildNodes[2].InnerText;//中心距
                string chishu1 = xe.ChildNodes[3].InnerText;//齿数1
                string chishu2 = xe.ChildNodes[4].InnerText;//齿数2
                string luoxuanjiao = xe.ChildNodes[5].InnerText;//螺旋角
                string yalijiao = xe.ChildNodes[6].InnerText;//压力角
                string bianweixishu1 = xe.ChildNodes[7].InnerText;//变位系数1
                string bianweixishu2 = xe.ChildNodes[8].InnerText;//变位系数2
                string chidinggaoxishu1 = xe.ChildNodes[9].InnerText;//齿顶高系数1
                string chidinggaoxishu2 = xe.ChildNodes[10].InnerText;//齿顶高系数2
                string dingxixishu1 = xe.ChildNodes[11].InnerText;//顶隙系数1
                string dingxixishu2 = xe.ChildNodes[12].InnerText;//顶隙系数1
                string chikuan = xe.ChildNodes[13].InnerText;//齿宽

                /*object[] oo = { jishu, moshu, zhongxinju, chishu1, chishu2, luoxuanjiao, yalijiao, bianweixishu1, bianweixishu2,
                                  chidinggaoxishu1,chidinggaoxishu2,dingxixishu1,dingxixishu2, chikuan };
                dataGridView.Rows.Add(oo);*/

                double[] CalResult = new double[52];

                CalResult = WGeoCal(double.Parse(chishu1), double.Parse(moshu), double.Parse(luoxuanjiao),
                     double.Parse(chishu2), double.Parse(chidinggaoxishu1), double.Parse(chidinggaoxishu2),
                      double.Parse(bianweixishu1), double.Parse(bianweixishu2), double.Parse(dingxixishu1),
                       double.Parse(dingxixishu2), double.Parse(yalijiao), 24, 24, 100);

                double dx = CalResult[8];// = dx;                          //求小齿轮分度圆直径
                double dd = CalResult[9];// = dx;                          //求大齿轮分度圆直径
                double dax = CalResult[13];// = dax;                                                            //求小齿轮齿顶圆直径              
                double dad = CalResult[14];// = dad;                                                            //求大齿轮齿顶圆直径
                object[] oo = { jishu, string.Format("{0:N3}", dx), string.Format("{0:N3}", dd), string.Format("{0:N3}", dax), string.Format("{0:N3}", dad) };
                dgvDiameter.Rows.Add(oo);
            }/*
         }
     }
     catch (Exception u)
     { MessageBox.Show("无法加载文件", "异常", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _newDataSet = new DataSet();// 临时定义的数据
            xmlDoc = new XmlDocument();
            // filename = Application.StartupPath + "/Test.xml";       
            fileName = @"F:\减速器.xml";// openFileDialog1.FileName;
            xmlDoc.Load(fileName);

            XmlNodeList nodes = xmlDoc.SelectNodes("//各级齿轮参数");

            foreach (XmlNode node in nodes)
            {
                XmlElement xe = (XmlElement)node;
                string jishu = xe.ChildNodes[0].InnerText;//级数
                string moshu = xe.ChildNodes[1].InnerText; //模数
                string zhongxinju = xe.ChildNodes[2].InnerText;//中心距
                string chishu1 = xe.ChildNodes[3].InnerText;//齿数1
                string chishu2 = xe.ChildNodes[4].InnerText;//齿数2
                string luoxuanjiao = xe.ChildNodes[5].InnerText;//螺旋角
                string yalijiao = xe.ChildNodes[6].InnerText;//压力角
                string bianweixishu1 = xe.ChildNodes[7].InnerText;//变位系数1
                string bianweixishu2 = xe.ChildNodes[8].InnerText;//变位系数2
                string chidinggaoxishu1 = xe.ChildNodes[9].InnerText;//齿顶高系数1
                string chidinggaoxishu2 = xe.ChildNodes[10].InnerText;//齿顶高系数2
                string dingxixishu1 = xe.ChildNodes[11].InnerText;//顶隙系数1
                string dingxixishu2 = xe.ChildNodes[12].InnerText;//顶隙系数1
                string chikuan = xe.ChildNodes[13].InnerText;//齿宽

                object[] oo = { jishu, moshu, zhongxinju, chishu1, chishu2, luoxuanjiao, yalijiao, bianweixishu1, bianweixishu2,
                                          chidinggaoxishu1,chidinggaoxishu2,dingxixishu1,dingxixishu2, chikuan };
                dataGridView.Rows.Add(oo);

                double[] CalResult = new double[52];

                CalResult = WGeoCal(double.Parse(chishu1), double.Parse(moshu), double.Parse(luoxuanjiao),
                     double.Parse(chishu2), double.Parse(chidinggaoxishu1), double.Parse(chidinggaoxishu2),
                      double.Parse(bianweixishu1), double.Parse(bianweixishu2), double.Parse(dingxixishu1),
                       double.Parse(dingxixishu2), double.Parse(yalijiao), 24, 24, 100);
            }                  
        }

        private void GraphControl_Load(object sender, EventArgs e)
        {

        }

        private PointPairList circleCalculate(double x0, double y0, double radius) //求圆上的点，以便画圆
        {
            PointPairList list = new PointPairList();

            int nodeNumber = 200;

            for (double i = 0; i <= nodeNumber; i++)
            {
                double x = x0 + radius * Math.Cos(i * Math.PI * 2 / nodeNumber);
                double y = y0 + radius * Math.Sin(i * Math.PI * 2 / nodeNumber);
                list.Add(x, y);
            }
            return list;
        }

        private void graphPane_AxisChangeEvent(GraphPane target)  //控制两个坐标轴的显示比例，前提条件是zde控件的外形必修是正方形
        {
            GraphPane graphPane = GraphControl.GraphPane;

            // Correct the scale so that the two axes are 1:1 aspect ratio
            double scalex2 = (graphPane.XAxis.Scale.Max - graphPane.XAxis.Scale.Min) / graphPane.Rect.Width;
            double scaley2 = (graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min) / graphPane.Rect.Height;
            if (scalex2 > scaley2)
            {
                double diff = graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min;
                double new_diff = graphPane.Rect.Height * scalex2;
                graphPane.YAxis.Scale.Min -= (new_diff - diff) / 2.0;
                graphPane.YAxis.Scale.Max += (new_diff - diff) / 2.0;
            }
            else if (scaley2 > scalex2)
            {
                double diff = graphPane.XAxis.Scale.Max - graphPane.XAxis.Scale.Min;
                double new_diff = graphPane.Rect.Width * scaley2;
                graphPane.XAxis.Scale.Min -= (new_diff - diff) / 2.0;
                graphPane.XAxis.Scale.Max += (new_diff - diff) / 2.0;
            }

            // Recompute the grid lines
            float scaleFactor = graphPane.CalcScaleFactor();
            Graphics g = GraphControl.CreateGraphics();
            graphPane.XAxis.Scale.PickScale(graphPane, g, scaleFactor);
            graphPane.YAxis.Scale.PickScale(graphPane, g, scaleFactor);
        }
    }
}
