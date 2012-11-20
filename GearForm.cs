using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using System.Collections;

using System.Data.OleDb;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
//using SolidWorksTools;
//using SolidWorksTools.File;
using System.Xml;
using System.Xml.Linq;
using ZedGraph;

namespace ReducerDesign
{
    public partial class GearForm : Form
    {
        //private DataSet _newDataSet;
        private XmlDocument xmlDoc;
        //private string fileName;

        IModelDoc2 modDoc;

        private double pressuerAngle;// 压力角
        // 小齿轮
       // private double pinionReferenceDiameter; //分度圆直径
        private double pinionBaseDiameter;  // 基圆直径  Db = D * Cos(a) 
       // private double pinionToothThickness; //齿厚
        private double pinionTipDiameter;//齿顶圆
        private int pinionZ;  // 齿数

        public GearForm()
        {
            InitializeComponent();
        }

        private void Gear_Load(object sender, EventArgs e)
        {
            // xmlDoc = new XmlDocument();
            // filename = Application.StartupPath + "/Test.xml";       
            //fileName = @"F:\减速器.xml";// openFileDialog1.FileName;
            //xmlDoc.Load(fileName);
        }
        
        private void button1_Click(object sender, EventArgs e) //齿轮参数计算
        {
            bool checktext = checkTextBoxes_1();
            if (checktext)
            {
                double zx = Convert.ToDouble(this.txtPinionZ.Text);
                double mn2 = Convert.ToDouble(this.text_mn2.Text);
                double lb12 = Convert.ToDouble(this.text_lb12.Text);
                double zd = Convert.ToDouble(this.text_zd.Text);
                double hax1 = Convert.ToDouble(this.text_hax1.Text);
                double had1 = Convert.ToDouble(this.text_had1.Text);
                double xnx = Convert.ToDouble(this.text_xnx.Text);
                double xnd = Convert.ToDouble(this.text_xnd.Text);
                double cnx = Convert.ToDouble(this.text_cax.Text);
                double cnd = Convert.ToDouble(this.text_cad.Text);
                double an2 = Convert.ToDouble(this.text_an2.Text);
                double dpqx = Convert.ToDouble(this.text_dpqx.Text);
                double dpqd = Convert.ToDouble(this.text_dpqd.Text);
                double bx = Convert.ToDouble(this.text_bx.Text);

                ////////////////////////////////////////////
                //WGeoClass newGeoClass = new WGeoClass();
                double[] CalResult = new double[52];
                CalResult = WGeoCal(zx, mn2, lb12, zd, hax1, had1, xnx, xnd, cnx, cnd, an2, dpqx, dpqd, bx);
                double at2 = CalResult[0];
                this.text_at2.Text = at2.ToString("0.0000");

                double ap2 = CalResult[1];
                this.text_ap2.Text = ap2.ToString("0.0000");                                         //求端面啮合角

                double a2 = CalResult[2];
                this.text_a2.Text = a2.ToString("0.0000");
                double xnmnx = CalResult[3];
                double xnmnd = CalResult[4];
                this.text_xnmnx.Text = xnmnx.ToString("0.0000");
                this.text_xnmnd.Text = xnmnd.ToString("0.0000");

                double hfx1 = CalResult[5];
                double hfd1 = CalResult[6];
                this.text_hfx1.Text = hfx1.ToString("0.0000");
                this.text_hfd1.Text = hfd1.ToString("0.0000");

                double mt2 = CalResult[7];
                this.text_mt2.Text = mt2.ToString("0.0000");
                double dx = CalResult[8];
                this.text_dx.Text = dx.ToString("0.0000");                                        //求小齿轮分度圆直径
                
                double dd = CalResult[9];
                this.text_dd.Text = dd.ToString("0.0000");                                        //求大齿轮分度圆直径

                double u2 = CalResult[10];
                this.text_u2.Text = u2.ToString("0.0000");
                double dpx = CalResult[11];
                double dpd = CalResult[12];
                this.text_dpx.Text = dpx.ToString("0.0000");                                       //求小齿轮节圆直径  
                this.text_dpd.Text = dpd.ToString("0.0000");                                       //求大齿轮节圆直径
                
                double dax = CalResult[13];
                double dad = CalResult[14];
                this.txtPinionTipDiameter.Text = dax.ToString("0.0000");                                       //求小齿轮齿顶圆直径              
                this.text_dad.Text = dad.ToString("0.0000");                                       //求大齿轮齿顶圆直径
                
                double dfx = CalResult[15];
                double dfd = CalResult[16];
                this.text_dfx.Text = dfx.ToString("0.0000");                                       //求小齿轮齿根圆直径
                this.text_dfd.Text = dfd.ToString("0.0000");                                       //求大齿轮齿根圆直径
                
                double dbx = CalResult[17];
                double dbd = CalResult[18];
                this.txtPinionBaseDiameter.Text = dbx.ToString("0.0000");                                       //求小齿轮基圆直径
                this.text_dbd.Text = dbd.ToString("0.0000");                                       //求大齿轮基圆直径

                double hax = CalResult[19];
                double had = CalResult[20];
                this.text_hax.Text = hax.ToString("0.0000");                                     //求小齿轮齿顶高
                this.text_had.Text = had.ToString("0.0000");                                     //求大齿轮齿顶高

                double hfx2 = CalResult[21];
                double hfd2 = CalResult[22];
                this.text_hfx2.Text = hfx2.ToString("0.0000");                                     //求小齿轮齿跟高
                this.text_hfd2.Text = hfd2.ToString("0.0000");                                     //求大齿轮齿跟高

                double hx = CalResult[23];
                double hd = CalResult[24];
                this.text_hx.Text = hx.ToString("0.0000");                                       //求小齿轮全齿高
                this.text_hd.Text = hd.ToString("0.0000");                                       //求大齿轮全齿高
                
                double scnx = CalResult[25];
                double hcnx = CalResult[26];
                this.text_scnx.Text = scnx.ToString("0.0000");                                     //求小齿轮固定弦齿厚
                this.text_hcnx.Text = hcnx.ToString("0.0000");                                     //求小齿轮固定弦齿高
                
                double aad = CalResult[27];
                double aax = CalResult[28];
                this.text_aad.Text = aad.ToString("0.0000");                                       //求大齿轮齿顶压力角
                this.text_aax.Text = aax.ToString("0.0000");                                       //求小齿轮齿顶压力角

                double scnd = CalResult[29];
                double hcnd = CalResult[30];
                this.text_scnd.Text = scnd.ToString("0.0000");                                     //求大齿轮固定弦齿厚
                this.text_hcnd.Text = hcnd.ToString("0.0000");                                     //求大齿轮固定弦齿高

                double srnx = CalResult[31];
                double hrnx = CalResult[32];
                this.text_srnx.Text = srnx.ToString("0.0000");                                     //求小齿轮分度圆弦齿厚
                this.text_hrnx.Text = hrnx.ToString("0.0000");                                     //求小齿轮分度圆弦齿高

                double srnd = CalResult[33];
                double hrnd = CalResult[34];
                this.text_srnd.Text = srnd.ToString("0.0000");                                     //求大齿轮分度圆弦齿厚
                this.text_hrnd.Text = hrnd.ToString("0.0000");                                     //求大齿轮分度圆弦齿高

                double kx = CalResult[35];
                double kd = CalResult[36];
                this.text_kx.Text = kx.ToString("0");                                              //求小齿轮公法线跨齿数
                this.text_kd.Text = kd.ToString("0");                                              //求大齿轮公法线跨齿数

                double wnkx = CalResult[37];
                double wnkd = CalResult[38];
                this.text_wnkx.Text = wnkx.ToString("0.0000");                                     //求小齿轮公法线长度
                this.text_wnkd.Text = wnkd.ToString("0.0000");                                     //求大齿轮公法线长度

                double amx = CalResult[39];
                double amd = CalResult[40];
                this.text_amx.Text = amx.ToString("0.0000");                                       //求小齿轮量棒中心所在圆的压力角
                this.text_amd.Text = amd.ToString("0.0000");                                       //求大齿轮量棒中心所在圆的压力角

                double mqx = CalResult[41];
                double mqd = CalResult[42];
                this.text_mqx.Text = mqx.ToString("0.0000");                                       //求小齿轮跨棒（球）距
                this.text_mqd.Text = mqd.ToString("0.0000");                                       //求大齿轮跨棒（球）距

                double snax = CalResult[43];
                double snad = CalResult[44];
                this.text_snax.Text = snax.ToString("0.0000");                                     //求小齿轮法向齿顶厚
                this.text_snad.Text = snad.ToString("0.0000");                                     //求大齿轮法向齿顶厚

                double dc2 = CalResult[45];
                double zc2 = CalResult[46];
                double dzc2 = CalResult[47];
                this.text_dc2.Text = dc2.ToString("0.0000");                                         //求端面重合度
                this.text_zc2.Text = zc2.ToString("0.0000");                                         // 求轴向重合度
                this.text_dzc2.Text = dzc2.ToString("0.0000");                                       //求总重合度

                double ba2 = CalResult[48];
                double bd12 = CalResult[49];
                this.text_ba2.Text = ba2.ToString("0.0000");                                         //求齿宽系数
                this.text_bd12.Text = bd12.ToString("0.0000");                                       //求宽径比系数

                double lx = CalResult[50];
                double ld = CalResult[51];
                this.text_lx.Text = lx.ToString("0.0000");
                this.text_ld.Text = ld.ToString("0.0000");
            }
            /*// 分度圆直径
           referenceDiameter = (int)numberOfTeethGearnumericUpDown.Value * int.Parse (moduleComboBox.Text);
           referenceDiameterTextBox.Text = Convert.ToString(referenceDiameter);
           */
            pressuerAngle = double.Parse(text_an2.Text) * Math.PI / 180;
            // 小齿轮
            pinionBaseDiameter = double.Parse(txtPinionBaseDiameter.Text) / 1000; //小齿轮基圆直径
            pinionTipDiameter = double.Parse(txtPinionTipDiameter.Text) / 1000;//小齿轮齿顶圆直径
            pinionZ = int.Parse(txtPinionZ.Text);
            //referenceDiameter * Math.Cos(double.Parse(pressureAngleTextBox.Text)*Math.PI /180);//  Db = D * Cos(a) 
            /*baseDiameterTextBox.Text = Convert.ToString(baseDiameter);
            //齿厚 toothThicknessTextBox
            toothThickness = double.Parse(text_bx.Text);           */
            // 小齿轮，同时把原来 毫米 单位 变成 米 单位， 因为Solidwork API 的单位是米
            pinionBaseDiameter = double.Parse(txtPinionBaseDiameter.Text) / 1000; //小齿轮基圆直径
            pinionTipDiameter = double.Parse(txtPinionTipDiameter.Text) / 1000;//小齿轮齿顶圆直径
        }
        
        private void bulidGearButton_Click(object sender, EventArgs e)
        {
            //int Errors=0;
            //ModelDoc2 swModel;
            // swModel = (ModelDoc2)SwAddin.iSwApp.ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);                      

            //make sure we have a part open
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.NewDocument(partTemplate, (int)swDwgPaperSizes_e.swDwgPaperA2size, 0.0, 0.0);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }
            // Part.InsertSketch2 True
            //boolstatus = Part.Extension.SelectByID2("前视基准面", "PLANE", 0, 0, 0, False, 0, Nothing, 0)
            //            Part.ClearSelection2 True
            //画齿顶圆

            modDoc.InsertSketch2(true); //建立草图
            modDoc.CreateCircleByRadius2(0, 0, 0, 0.5 * pinionTipDiameter);//  SketchRectangle(0, 0, 0, .1, .1, .1, false);
            //Extrude the sketch
            IFeatureManager featMan = modDoc.FeatureManager;
            featMan.FeatureExtrusion(true,
                false, false,
                (int)swEndConditions_e.swEndCondBlind, (int)swEndConditions_e.swEndCondBlind,
                double.Parse(text_bx.Text) / 1000, 0.0,
                false, false,
                false, false,
                0.0, 0.0,
                false, false,
                false, false,
                true,
                false, false);

            double[] x = new double[15], y = new double[15], x1 = new double[15], y1 = new double[15];

            double U = Math.Sqrt(Math.Pow(pinionTipDiameter / pinionBaseDiameter, 2) - 1);//  滚动角弧度
            // 画渐开线
            for (int i = 0; i < 15; i++)
            {
                x[i] = 0.5 * pinionBaseDiameter * (Math.Sin(U * i / 14) - (U * i / 14) * Math.Cos(U * i / 14));
                y[i] = 0.5 * pinionBaseDiameter * (Math.Cos(U * i / 14) + (U * i / 14) * Math.Sin(U * i / 14));
            }
            // S = pi * m / 2 + 2 * X1 * m * Tan(a)  's为分度圆齿厚
            //sb = m * cos(alpha * pi / 180) * (0.5 * pi + z * (tan(alpha * pi / 180) - alpha * pi / 180));
            double m = double.Parse(text_mn2.Text);
            double Sb = m * Math.Cos(pressuerAngle) * (0.5 * Math.PI + pinionZ * (Math.Tan(pressuerAngle) - pressuerAngle));
            // text_mn2.Text) * 0.5 + 2 * double.Parse(text_xnx.Text) * double.Parse(text_mn2.Text) * Math.Tan(pressuerAngle);  //s为分度圆齿厚
            // double inva = Math.Tan(pressuerAngle) - pressuerAngle;
            // double Sb = double.Parse(text_mn2.Text) * Math.Cos(pressuerAngle)*(0.5 * Math.PI + pinionZ * inva);// *Math.Cos(pressuerAngle);//    's为基圆齿厚

            double temp = -(Math.PI / pinionZ - Sb / (1000 * pinionBaseDiameter));

            // 渐开线转动一个角度，使得渐开线对称，
            for (int i = 0; i < 15; i++)
            {
                x1[i] = x[i] * Math.Cos(temp) - y[i] * Math.Sin(temp);
                y1[i] = x[i] * Math.Sin(temp) + y[i] * Math.Cos(temp);
            }

            modDoc.InsertSketch2(true); //建立草图
            //'绘渐开线
            for (int i = 0; i < 15; i++)
            {
                modDoc.SketchSpline(14 - i, -x1[i], y1[i], 0);
            }
            //'绘渐开线
            for (int i = 0; i < 15; i++)
            {
                modDoc.SketchSpline(14 - i, x1[i], y1[i], 0);
            }

            double df = double.Parse(text_dfx.Text) / 1000;
            double temp1 = Math.Sqrt(0.5 * df * 0.5 * df - x1[0] * x1[0]);
            modDoc.CreateArc2(0, 0, 0, x1[14], y1[14], 0, -x1[14], y1[14], 0, 1);
            modDoc.CreateArc2(0, 0, 0, x1[0], temp1, 0, -x1[0], temp1, 0, 1);

            modDoc.CreateLine2(x1[0], y1[0], 0, x1[0], temp1, 0);
            modDoc.CreateLine2(-x1[0], y1[0], 0, -x1[0], temp1, 0);
            //Debug.Print("Error code "+temp1.ToString());
            // modDoc.CreateLine2(-x1[0], y1[0], 0, -x1[0], temp1, 0);
            //modDoc.CreateArc2(0, 0, 0, x1[0], temp1, 0, -x1[0], temp1, 0, 1);


            ModelDoc2 swModel;
            ModelDocExtension swExt;
            SelectionMgr swSelMgr;
            ISketchManager swSktMgr;
            bool boolstatus;
            //int Errors = 0;
            swModel = SwAddin.iSwApp.ActiveDoc as ModelDoc2;
            swSktMgr = swModel.SketchManager;// SwAddin.iSwApp.ActiveDoc as ISketchManager;
            //swModel = (ModelDoc2)SwAddin.iSwApp.ActiveDoc ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);
            //Debug.Print("Error code after document activation: " + Errors.ToString());) ;//. ac .ActivateDoc();// as ModelDoc2;

            swExt = swModel.Extension;

            swSelMgr = swModel.SelectionManager as SelectionMgr;

            /*boolstatus = swExt.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            //boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", x1[0], y1[0] - 0.001, 0, true, 0, null, 0);
//            boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", 0, 0.5 * df, 0, true, 0, null, 0);
            /*modDoc.ClearSelection2(true);
            boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", x1[0], y1[0] - 0.001, 0, false, 0, null, 0);
            boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", 0, 0.5 * df, 0, true, 0, null, 0);
            if (boolstatus)
            {
                swSktMgr.CreateFillet(0.2 * double.Parse(text_mn2.Text) * 0.001, (int)swConstrainedCornerAction_e.swConstrainedCornerInteract);
            }
            else
            {
                MessageBox.Show("fail");
            }
           // modDoc.SketchFillet2(0.2 * double.Parse(text_mn2.Text) * 0.001, 2);
           // MessageBox.Show("fail");

            //boolstatus = swExt.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            //boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", -x1[0], y1[0] - 0.001, 0, true, 0, null, 0);
            //boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", 0, 0.5 * df, 0, true, 0, null, 0);
            //modDoc.ClearSelection2(true);
            boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", -x1[0], y1[0] - 0.001, 0, false , 0, null, 0);
            boolstatus = swExt.SelectByID2("", "SKETCHSEGMENT", 0, 0.5 * df, 0, true, 0, null, 0);
            if (boolstatus)
            {
                swSktMgr.CreateFillet(0.2 * double.Parse(text_mn2.Text) * 0.001, (int)swConstrainedCornerAction_e.swConstrainedCornerInteract);
            }
            else
            {
                MessageBox.Show("fail");
            }
            //modDoc.SketchFillet2(0.2 * double.Parse(text_mn2.Text) * 0.001, 2);
            modDoc.ClearSelection2(true);*/

            /* boolstatus = Part.Extension.SelectByID2("Spline1", "SKETCHSEGMENT", X(1), y(1), 0, False, 0, Nothing, 0)
            Part.SketchAddConstraints "sgFIXED"
            '    boolstatus = swModelDocExt.SelectByID2("草图2", "SKETCH", 0, 0.025, 0, False, 0, Nothing, 0)
            'Set swFeature = swFeatureManager.FeatureCut3(True, False, True, swEndCondThroughAll, swEndCondBlind, 0.01, 0.01, False, False, False, False, 0.01745329251994, 0.01745329251994, False, False, False, False, False, True, True, False, False, False, swStartSketchPlane, 0, False)


            Part.SketchManager.InsertSketch True
            Part.EditRebuild3
            swModel.ClearSelection2 True
            ' Part.SelectionManager.EnableContourSelection = True*/
            modDoc.EditRebuild3();

            boolstatus = swExt.SelectByID2("草图2", "SKETCH", 0, 0, 0, false, 4, null, 0);
            if (!boolstatus)
            {
                MessageBox.Show("fail-SKETCH");
            }
            swSelMgr.EnableContourSelection = true;
            boolstatus = swExt.SelectByID2("草图2", "SKETCHREGION", 0, 0.5 * df + 0.001, 0, false, 4, null, 0);
            boolstatus = swExt.SelectByID2("草图2", "SKETCHREGION", 0, 0.5 * df + 0.001, 0, false, 4, null, 0);
            boolstatus = swExt.SelectByID2("草图2", "SKETCHREGION", 0, 0.5 * df + 0.001, 0, false, 4, null, 0);

            FeatureManager swFeatureManager = default(FeatureManager);
            Feature swFeature = default(Feature);

            swFeatureManager = (FeatureManager)modDoc.FeatureManager;


            if (!boolstatus)
            {
                MessageBox.Show("fail-SKETCHREGION");
            }

            swFeature = featMan.FeatureCut3(true, false, true,
                (int)swEndConditions_e.swEndCondBlind, (int)swEndConditions_e.swEndCondBlind,
                0.01, 0.01, false, false, false, false,
                0.01745329251994, 0.01745329251994,
                false, false, false, false, false,
                true, true, false, false, false, (int)swStartConditions_e.swStartSketchPlane, 0, false);

            /*
            ' Part.FeatureManager.FeatureCut3 False, False, False, swEndCondThroughAll, swEndCondBlind, 0.01, 0.01, False, False, False, False, 0.01745329251994, 0.01745329251994, False, False, False, False, False, True, True, False, False, False, swStartSketchPlane, 0, False
            ' Part.FeatureManager.FeatureCut3 True, False, False, swEndCondThroughAll, swEndCondBlind, 0.01, 0.01, False, False, False, False, 0.01745329251994, 0.01745329251994, False, False, False, False, False, True, True, False, False, False, swStartSketchPlane, 0, False
            Part.FeatureManager.FeatureCut3 False, False, True, swEndCondThroughAll, swEndCondBlind, 0.01, 0.01, False, False, False, False, 0.01745329251994, 0.01745329251994, False, False, False, False, False, True, True, False, False, False, swStartSketchPlane, 0, False

            '建立轴
            boolstatus = Part.Extension.SelectByID2("Point1@原点", "EXTSKETCHPOINT", 0, 0, 0, True, 0, Nothing, 0)
            boolstatus = Part.Extension.SelectByID2("", "FACE", 0, 0, 0, True, 0, Nothing, 0)
            boolstatus = Part.InsertAxis2(True)
            '阵列

            Part.ClearSelection2 True
            Part.ActivateSelectedFeature
            boolstatus = Part.Extension.SelectByID2("", "AXIS", 0, 0, 0, False, 1, Nothing, 0)
            boolstatus = Part.Extension.SelectByID2("切除-拉伸1", "BODYFEATURE", 0, 0, 0, True, 4, Nothing, 0)
            'Part.ActivateSelectedFeature
            'Part.ClearSelection2 True
            'boolstatus = Part.Extension.SelectByID2("切除-拉伸1", "BODYFEATURE", -1.69661489928785E-03, 2.34775776629448E-02, 1.60262992130811E-03, False, 4, Nothing, 0)
            'boolstatus = Part.Extension.SelectByID2("", "AXIS", -3.75572662914793E-03, 5.28101193268626E-04, 6.18986555079533E-03, True, 1, Nothing, 0)
            Dim myFeature As Object
            ' Set myFeature = Part.FeatureManager.FeatureCircularPattern3(23, 6.2831853071796, True, "NULL", False, True)

            Set myFeature = Part.FeatureManager.FeatureCircularPattern3(z, 2 * pi, True, "NULL", False, True)*/

        }


        public double[] WGeoCal(double zx, double mn2, double lb12, double zd, double hax1, double had1, double xnx, double xnd, double cnx, double cnd,
           double an2, double dpqx, double dpqd, double bx)
        {
            //下面的为输入参数

            /*double zx = Convert.ToDouble(this.text_zx.Text);
            double mn2 = Convert.ToDouble(this.text_mn2.Text);
            double lb12 = Convert.ToDouble(this.text_lb12.Text);
            double zd = Convert.ToDouble(this.text_zd.Text);
            double hax1 = Convert.ToDouble(this.text_hax1.Text);
            double had1 = Convert.ToDouble(this.text_had1.Text);
            double xnx = Convert.ToDouble(this.text_xnx.Text);
            double xnd = Convert.ToDouble(this.text_xnd.Text);
            double cnx = Convert.ToDouble(this.text_cax.Text);
            double cnd = Convert.ToDouble(this.text_cad.Text);
            double an2 = Convert.ToDouble(this.text_an2.Text);
            double dpqx = Convert.ToDouble(this.text_dpqx.Text);
            double dpqd = Convert.ToDouble(this.text_dpqd.Text);
            double bx = Convert.ToDouble(this.text_bx.Text);
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
            returnResult[25] = scnx;                                                           //求小齿轮固定弦齿厚
            returnResult[26] = hcnx;                                                           //求小齿轮固定弦齿高


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

        private void text_mn2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_an2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_lb12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_zx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_zd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_xnx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_xnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_bx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_bd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_hax1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_had1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_cax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_cad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_dpqx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_dpqd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private bool checkTextBoxes_1()
        {
            if (txtPinionZ.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_zd.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_mn2.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_an2.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_cax.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_cad.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_lb12.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_dpqx.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_dpqd.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_bx.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_hax1.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_had1.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_xnx.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (text_xnd.Text == "")
            {
                MessageBox.Show("有的参数为空，不能计算出结果", "提示！！！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnGearBuild_Click(object sender, EventArgs e)
        {
            //int Errors=0;
            //ModelDoc2 swModel;
            // swModel = (ModelDoc2)SwAddin.iSwApp.ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);                      

            //make sure we have a part open
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"c:\gear.SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            EquationMgr MyEqu = modDoc.GetEquationMgr();// default(EquationMgr);
            MessageBox.Show(MyEqu.get_Equation(1));
            MyEqu.set_Equation(1, "\"" + "m" + "\"" + "=" + text_mn2.Text);//\\"\"" + System.Convert.ToChar(Index) + "\"=" 
            MyEqu.EvaluateAll();
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"C:\HelicalGearSharftModel_42.SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            ModelDoc2 swModel;
            ModelDocExtension swExt;
            SelectionMgr swSelMgr;
            ISketchManager swSktMgr;
            bool boolstatus;
            //int Errors = 0;
            swModel = SwAddin.iSwApp.ActiveDoc as ModelDoc2;
            swSktMgr = swModel.SketchManager;// SwAddin.iSwApp.ActiveDoc as ISketchManager;
            //swModel = (ModelDoc2)SwAddin.iSwApp.ActiveDoc ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);
            //Debug.Print("Error code after document activation: " + Errors.ToString());) ;//. ac .ActivateDoc();// as ModelDoc2;

            swExt = swModel.Extension;

            swSelMgr = swModel.SelectionManager as SelectionMgr;

            boolstatus = swExt.SelectByID2("轴段2", "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
            swModel.EditSuppress2();//                .EditUnsuppress2();// Unsuppress (unfold) flat-pattern feature
        }

        private void gearbuild(string filename, double[] parameter)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //int Errors=0;
            //ModelDoc2 swModel;
            // swModel = (ModelDoc2)SwAddin.iSwApp.ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);                      

            //make sure we have a part open
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"c:\mygear.SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            EquationMgr MyEqu = modDoc.GetEquationMgr();

            //滚动角
            double u, d_a, d_b;
            d_a = double.Parse(txtPinionTipDiameter.Text);
            d_b = double.Parse(txtPinionBaseDiameter.Text);
            u = Math.Sqrt((d_a / d_b) * (d_a / d_b) - 1);

            // 螺距
            double luoju = Math.PI * double.Parse(text_dx.Text) / Math.Tan(double.Parse(text_lb12.Text) * Math.PI / 180);

            MyEqu.set_Equation(0, "\"" + "d_b" + "\"" + "=" + txtPinionBaseDiameter.Text);//基圆
            MyEqu.set_Equation(1, "\"" + "U" + "\"" + "=" + u);//滚动角
            MyEqu.set_Equation(2, "\"" + "d" + "\"" + "=" + text_dx.Text);//分度圆
            MyEqu.set_Equation(7, "\"" + "s" + "\"" + "=" + double.Parse(text_srnx.Text));//齿弦厚
            MyEqu.set_Equation(8, "\"" + "d_f" + "\"" + "=" + text_dfx.Text);//齿根圆
            MyEqu.set_Equation(10, "\"" + "B" + "\"" + "=" + text_bx.Text);//齿宽
            MyEqu.set_Equation(13, "\"" + "z" + "\"" + "=" + txtPinionZ.Text);//齿数           
            MyEqu.set_Equation(15, "\"" + "d_a" + "\"" + "=" + txtPinionTipDiameter.Text);//齿顶圆            
            MyEqu.set_Equation(18, "\"" + "luoju" + "\"" + "=" + luoju);//螺距
            MyEqu.set_Equation(21, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角
            // 腹板参数
            MyEqu.set_Equation(23, "\"" + "D_2" + "\"" + "=" + textFBWJ.Text);//腹板外径
            MyEqu.set_Equation(24, "\"" + "D_1" + "\"" + "=" + texFBNJ.Text);//腹板内径
            MyEqu.set_Equation(25, "\"" + "R_FB" + "\"" + "=" + textFBYJ.Text);//腹板外圆角
            MyEqu.set_Equation(26, "\"" + "C_FB" + "\"" + "=" + textFBDJ.Text);//腹板外倒角
            MyEqu.set_Equation(27, "\"" + "KJ_FB" + "\"" + "=" + textFBKJ.Text);//腹板孔径
            MyEqu.set_Equation(28, "\"" + "N_FB" + "\"" + "=" + textFBKS.Text);//腹板孔数

            MyEqu.set_Equation(33, "\"" + "D_FB" + "\"" + "=" + textFBH.Text);//腹板厚


            // 齿根圆角简单的计算公式：if ( "齿顶高" > = 1 , 0.38 * "m_n" , 0.46 * "m_n" )                 
            /*double R_CG; 
            if (double.Parse(text_hax.Text) >= 1)
            {
                R_CG = 0.38 * double.Parse(text_mn2.Text);
            }
            else
            {
                R_CG = 0.46 * double.Parse(text_mn2.Text);
            }
            MyEqu.set_Equation(20, "\"" + "R_CG" + "\"" + "=" + R_CG);//齿根圆角
            MyEqu.set_Equation(21, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角
            */





            //齿弦厚
            //double z_v,psi_v,s;
            //z_v = double.Parse (txtPinionZ.Text )/Math.Pow(Math.Cos(double.Parse(text_lb12.Text)*Math.PI /180),3);
            //psi_v = 90/z_v +(360*double.Parse (text_xnx.Text)*Math.Tan(double.Parse(text_an2.Text)))/(Math.PI *z_v);
            //s = z_v*double.Parse (text_mn2.Text)*Math.Sin(psi_v*Math.PI /180)/Math.Cos(double.Parse(text_lb12.Text)*Math.PI /180);



            // 轮毂参数
            MyEqu.set_Equation(41, "\"" + "D_LG" + "\"" + "=" + textLGZJ.Text);//轮毂直径
            MyEqu.set_Equation(42, "\"" + "J_K" + "\"" + "=" + textJK.Text);//键宽
            MyEqu.set_Equation(43, "\"" + "J_S" + "\"" + "=" + textJS.Text);//键深
            MyEqu.set_Equation(44, "\"" + "C_LG" + "\"" + "=" + textZXKDJ.Text);//轮毂倒角


            /*
            MyEqu.set_Equation(11, "\"" + "R_FBW" + "\"" + "=" + textFBWYJ.Text);//腹板外圆角
            MyEqu.set_Equation(8, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角
            MyEqu.set_Equation(11, "\"" + "R_FBW" + "\"" + "=" + textFBWYJ.Text);//腹板外圆角
            MyEqu.set_Equation(9, "\"" + "n_FBK" + "\"" + "=" + textFBKS.Text);//腹板孔数
           
           // MyEqu.set_Equation(11, "\"" + "R_FBW" + "\"" + "=" + textFBWYJ.Text);//腹板外圆角
            //MyEqu.set_Equation(12, "\"" + "R_FBN" + "\"" + "=" + textFBNYJ.Text);//腹板内圆角
            MyEqu.set_Equation(13, "\"" + "C_FBW" + "\"" + "=" + textFBWDJ.Text);//腹板外倒角
            MyEqu.set_Equation(14, "\"" + "C_FBN" + "\"" + "=" + textFBNDJ.Text);//腹板内倒角
            MyEqu.set_Equation(15, "\"" + "d_Z" + "\"" + "=" + textLGZJ.Text);//轮毂直径
            MyEqu.set_Equation(16, "\"" + "j_B" + "\"" + "=" + textJK.Text);//键宽
            MyEqu.set_Equation(17, "\"" + "j_D" + "\"" + "=" + textJS.Text);//键深
            MyEqu.set_Equation(18, "\"" + "beta" + "\"" + "=" + text_lb12.Text);//螺旋角

            MyEqu.set_Equation(20, "\"" + "x_n1" + "\"" + "=" + text_xnx.Text);//本齿轮法向变位系数

            MyEqu.set_Equation(21, "\"" + "x_n2" + "\"" + "=" + text_xnd.Text);//啮合齿轮法向变位系数

            // MyEqu.set_Equation(22, "\"" + "y" + "\"" + "=" + text_xnd.Text);

            //MyEqu.set_Equation(21, "\"" + "x_n2" + "\"" + "=" + text_xnd.Text);*/

            MyEqu.EvaluateAll();
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //int Errors=0;
            //ModelDoc2 swModel;
            // swModel = (ModelDoc2)SwAddin.iSwApp.ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);                      

            //make sure we have a part open
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"c:\mygear.SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            EquationMgr MyEqu = modDoc.GetEquationMgr();

            //滚动角
            double u, d_a, d_b;
            d_a = double.Parse(text_dad.Text);
            d_b = double.Parse(text_dbd.Text);
            u = Math.Sqrt((d_a / d_b) * (d_a / d_b) - 1);

            // 螺距
            double luoju = Math.PI * double.Parse(text_dd.Text) / Math.Tan(double.Parse(text_lb12.Text) * Math.PI / 180);

            MyEqu.set_Equation(0, "\"" + "d_b" + "\"" + "=" + text_dbd.Text);//基圆
            MyEqu.set_Equation(1, "\"" + "U" + "\"" + "=" + u);//滚动角
            MyEqu.set_Equation(2, "\"" + "d" + "\"" + "=" + text_dd.Text);//分度圆
            MyEqu.set_Equation(7, "\"" + "s" + "\"" + "=" + double.Parse(text_srnd.Text));//齿弦厚
            MyEqu.set_Equation(8, "\"" + "d_f" + "\"" + "=" + text_dfd.Text);//齿根圆
            MyEqu.set_Equation(10, "\"" + "B" + "\"" + "=" + text_bd.Text);//齿宽
            MyEqu.set_Equation(13, "\"" + "z" + "\"" + "=" + text_zd.Text);//齿数           
            MyEqu.set_Equation(15, "\"" + "d_a" + "\"" + "=" + text_dad.Text);//齿顶圆            
            MyEqu.set_Equation(18, "\"" + "luoju" + "\"" + "=" + luoju);//螺距
            MyEqu.set_Equation(21, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角
            // 腹板参数
            MyEqu.set_Equation(23, "\"" + "D_2" + "\"" + "=" + textFBWJ.Text);//腹板外径
            MyEqu.set_Equation(24, "\"" + "D_1" + "\"" + "=" + texFBNJ.Text);//腹板内径
            MyEqu.set_Equation(25, "\"" + "R_FB" + "\"" + "=" + textFBYJ.Text);//腹板外圆角
            MyEqu.set_Equation(26, "\"" + "C_FB" + "\"" + "=" + textFBDJ.Text);//腹板外倒角
            MyEqu.set_Equation(27, "\"" + "KJ_FB" + "\"" + "=" + textFBKJ.Text);//腹板孔径
            MyEqu.set_Equation(28, "\"" + "N_FB" + "\"" + "=" + textFBKS.Text);//腹板孔数

            MyEqu.set_Equation(33, "\"" + "D_FB" + "\"" + "=" + textFBH.Text);//腹板厚


            // 齿根圆角简单的计算公式：if ( "齿顶高" > = 1 , 0.38 * "m_n" , 0.46 * "m_n" )                 
            /*double R_CG; 
            if (double.Parse(text_hax.Text) >= 1)
            {
                R_CG = 0.38 * double.Parse(text_mn2.Text);
            }
            else
            {
                R_CG = 0.46 * double.Parse(text_mn2.Text);
            }
            MyEqu.set_Equation(20, "\"" + "R_CG" + "\"" + "=" + R_CG);//齿根圆角
            MyEqu.set_Equation(21, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角
            */





            //齿弦厚
            //double z_v,psi_v,s;
            //z_v = double.Parse (txtPinionZ.Text )/Math.Pow(Math.Cos(double.Parse(text_lb12.Text)*Math.PI /180),3);
            //psi_v = 90/z_v +(360*double.Parse (text_xnx.Text)*Math.Tan(double.Parse(text_an2.Text)))/(Math.PI *z_v);
            //s = z_v*double.Parse (text_mn2.Text)*Math.Sin(psi_v*Math.PI /180)/Math.Cos(double.Parse(text_lb12.Text)*Math.PI /180);



            // 轮毂参数
            MyEqu.set_Equation(41, "\"" + "D_LG" + "\"" + "=" + textLGZJ.Text);//轮毂直径
            MyEqu.set_Equation(42, "\"" + "J_K" + "\"" + "=" + textJK.Text);//键宽
            MyEqu.set_Equation(43, "\"" + "J_S" + "\"" + "=" + textJS.Text);//键深
            MyEqu.set_Equation(44, "\"" + "C_LG" + "\"" + "=" + textZXKDJ.Text);//轮毂倒角


            /*
            MyEqu.set_Equation(11, "\"" + "R_FBW" + "\"" + "=" + textFBWYJ.Text);//腹板外圆角
            MyEqu.set_Equation(8, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角
            MyEqu.set_Equation(11, "\"" + "R_FBW" + "\"" + "=" + textFBWYJ.Text);//腹板外圆角
            MyEqu.set_Equation(9, "\"" + "n_FBK" + "\"" + "=" + textFBKS.Text);//腹板孔数
           
           // MyEqu.set_Equation(11, "\"" + "R_FBW" + "\"" + "=" + textFBWYJ.Text);//腹板外圆角
            //MyEqu.set_Equation(12, "\"" + "R_FBN" + "\"" + "=" + textFBNYJ.Text);//腹板内圆角
            MyEqu.set_Equation(13, "\"" + "C_FBW" + "\"" + "=" + textFBWDJ.Text);//腹板外倒角
            MyEqu.set_Equation(14, "\"" + "C_FBN" + "\"" + "=" + textFBNDJ.Text);//腹板内倒角
            MyEqu.set_Equation(15, "\"" + "d_Z" + "\"" + "=" + textLGZJ.Text);//轮毂直径
            MyEqu.set_Equation(16, "\"" + "j_B" + "\"" + "=" + textJK.Text);//键宽
            MyEqu.set_Equation(17, "\"" + "j_D" + "\"" + "=" + textJS.Text);//键深
            MyEqu.set_Equation(18, "\"" + "beta" + "\"" + "=" + text_lb12.Text);//螺旋角

            MyEqu.set_Equation(20, "\"" + "x_n1" + "\"" + "=" + text_xnx.Text);//本齿轮法向变位系数

            MyEqu.set_Equation(21, "\"" + "x_n2" + "\"" + "=" + text_xnd.Text);//啮合齿轮法向变位系数

            // MyEqu.set_Equation(22, "\"" + "y" + "\"" + "=" + text_xnd.Text);

            //MyEqu.set_Equation(21, "\"" + "x_n2" + "\"" + "=" + text_xnd.Text);*/

            MyEqu.EvaluateAll();
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
        }


        private void button2_Click_2(object sender, EventArgs e)
        {
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"c:\mygearsharft (2).SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            EquationMgr MyEqu = modDoc.GetEquationMgr();

            //滚动角
            double u, d_a, d_b;
            d_a = double.Parse(txtPinionTipDiameter.Text);
            d_b = double.Parse(txtPinionBaseDiameter.Text);
            u = Math.Sqrt((d_a / d_b) * (d_a / d_b) - 1);

            // 螺距
            double luoju = Math.PI * double.Parse(text_dx.Text) / Math.Tan(double.Parse(text_lb12.Text) * Math.PI / 180);

            MyEqu.set_Equation(0, "\"" + "d_b" + "\"" + "=" + txtPinionBaseDiameter.Text);//基圆
            MyEqu.set_Equation(1, "\"" + "U" + "\"" + "=" + u);//滚动角
            MyEqu.set_Equation(2, "\"" + "d" + "\"" + "=" + text_dx.Text);//分度圆
            MyEqu.set_Equation(7, "\"" + "s" + "\"" + "=" + double.Parse(text_srnx.Text));//齿弦厚
            MyEqu.set_Equation(8, "\"" + "d_f" + "\"" + "=" + text_dfx.Text);//齿根圆
            MyEqu.set_Equation(10, "\"" + "B" + "\"" + "=" + text_bx.Text);//齿宽
            MyEqu.set_Equation(13, "\"" + "z" + "\"" + "=" + txtPinionZ.Text);//齿数           
            MyEqu.set_Equation(15, "\"" + "d_a" + "\"" + "=" + txtPinionTipDiameter.Text);//齿顶圆            
            MyEqu.set_Equation(18, "\"" + "luoju" + "\"" + "=" + luoju);//螺距
            MyEqu.set_Equation(21, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角

            // 左轴段1
            MyEqu.set_Equation(36, "\"" + "zzzj1" + "\"" + "=" + textZZZJ1.Text);  //直径
            MyEqu.set_Equation(37, "\"" + "zzcd1" + "\"" + "=" + textZZCD1.Text);//长度
            MyEqu.set_Equation(38, "\"" + "zzyj1" + "\"" + "=" + textZZYJ1.Text);//圆角

            MyEqu.set_Equation(39, "\"" + "zzdj1" + "\"" + "=" + textZZDJ1.Text);//倒角

            // 左2
            MyEqu.set_Equation(40, "\"" + "zzzj2" + "\"" + "=" + textZZZJ2.Text);
            MyEqu.set_Equation(41, "\"" + "zzcd2" + "\"" + "=" + textZZCD2.Text);
            MyEqu.set_Equation(42, "\"" + "zzyj2" + "\"" + "=" + textZZYJ2.Text);
            MyEqu.set_Equation(43, "\"" + "zzdj2" + "\"" + "=" + textZZDJ2.Text);

            //左3
            MyEqu.set_Equation(44, "\"" + "zzzj3" + "\"" + "=" + textZZZJ3.Text);
            MyEqu.set_Equation(45, "\"" + "zzcd3" + "\"" + "=" + textZZCD3.Text);
            MyEqu.set_Equation(46, "\"" + "zzyj3" + "\"" + "=" + textZZYJ3.Text);
            MyEqu.set_Equation(47, "\"" + "zzdj3" + "\"" + "=" + textZZDJ3.Text);

            //左4
            MyEqu.set_Equation(48, "\"" + "zzzj4" + "\"" + "=" + textZZZJ4.Text);
            MyEqu.set_Equation(49, "\"" + "zzcd4" + "\"" + "=" + textZZCD4.Text);
            MyEqu.set_Equation(50, "\"" + "zzyj4" + "\"" + "=" + textZZYJ4.Text);
            MyEqu.set_Equation(51, "\"" + "zzdj4" + "\"" + "=" + textZZDJ4.Text);

            // 右1
            MyEqu.set_Equation(52, "\"" + "yzzj1" + "\"" + "=" + textYZZJ1.Text);
            MyEqu.set_Equation(53, "\"" + "yzcd1" + "\"" + "=" + textYZCD1.Text);
            MyEqu.set_Equation(54, "\"" + "yzyj1" + "\"" + "=" + textYZYJ1.Text);
            MyEqu.set_Equation(55, "\"" + "yzdj1" + "\"" + "=" + textYZDJ1.Text);

            //右2
            MyEqu.set_Equation(56, "\"" + "yzzj2" + "\"" + "=" + textYZZJ2.Text);
            MyEqu.set_Equation(57, "\"" + "yzcd2" + "\"" + "=" + textYZCD2.Text);
            MyEqu.set_Equation(58, "\"" + "yzyj2" + "\"" + "=" + textYZYJ2.Text);
            MyEqu.set_Equation(59, "\"" + "yzdj2" + "\"" + "=" + textYZDJ2.Text);

            //右3
            MyEqu.set_Equation(60, "\"" + "yzzj3" + "\"" + "=" + textYZZJ3.Text);

            MyEqu.set_Equation(61, "\"" + "yzcd3" + "\"" + "=" + textYZCD3.Text);
            MyEqu.set_Equation(62, "\"" + "yzyj3" + "\"" + "=" + textYZYJ3.Text);
            MyEqu.set_Equation(63, "\"" + "yzdj3" + "\"" + "=" + textYZDJ3.Text);

            //右4
            MyEqu.set_Equation(64, "\"" + "yzzj4" + "\"" + "=" + textYZZJ4.Text);
            MyEqu.set_Equation(65, "\"" + "yzcd4" + "\"" + "=" + textYZCD4.Text);
            MyEqu.set_Equation(66, "\"" + "yzyj4" + "\"" + "=" + textYZYJ4.Text);
            MyEqu.set_Equation(67, "\"" + "yzdj4" + "\"" + "=" + textYZDJ4.Text);


            MyEqu.EvaluateAll();
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"c:\mygearsharft.SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            EquationMgr MyEqu = modDoc.GetEquationMgr();

            int revatal = MyEqu.GetCount();
            for (int k = 0; k < (revatal); k++)
            {
                MyEqu.ChangeSuppressionForAllConfigurations(k, false);
            }


            int revatal1 = MyEqu.GetCount();
            for (int k = 0; k < (revatal1); k++)
            {
                MyEqu.ChangeSuppressionForAllConfigurations(k, false);

            }
            MessageBox.Show("解压缩");

            //滚动角
            double u, d_a, d_b;
            d_a = double.Parse(txtPinionTipDiameter.Text);
            d_b = double.Parse(txtPinionBaseDiameter.Text);
            u = Math.Sqrt((d_a / d_b) * (d_a / d_b) - 1);

            // 螺距
            double luoju = Math.PI * double.Parse(text_dx.Text) / Math.Tan(double.Parse(text_lb12.Text) * Math.PI / 180);

            MyEqu.set_Equation(0, "\"" + "d_b" + "\"" + "=" + txtPinionBaseDiameter.Text);//基圆
            MyEqu.set_Equation(1, "\"" + "U" + "\"" + "=" + u);//滚动角
            MyEqu.set_Equation(2, "\"" + "d" + "\"" + "=" + text_dx.Text);//分度圆
            MyEqu.set_Equation(7, "\"" + "s" + "\"" + "=" + double.Parse(text_srnx.Text));//齿弦厚
            MyEqu.set_Equation(8, "\"" + "d_f" + "\"" + "=" + text_dfx.Text);//齿根圆
            MyEqu.set_Equation(10, "\"" + "B" + "\"" + "=" + text_bx.Text);//齿宽
            MyEqu.set_Equation(13, "\"" + "z" + "\"" + "=" + txtPinionZ.Text);//齿数           
            MyEqu.set_Equation(15, "\"" + "d_a" + "\"" + "=" + txtPinionTipDiameter.Text);//齿顶圆            
            MyEqu.set_Equation(18, "\"" + "luoju" + "\"" + "=" + luoju);//螺距
            MyEqu.set_Equation(21, "\"" + "C_CD" + "\"" + "=" + textCDDJ.Text);//齿顶倒角

            // 左轴段1
            MyEqu.set_Equation(36, "\"" + "zzzj1" + "\"" + "=" + textZZZJ1.Text);  //直径
            MyEqu.set_Equation(37, "\"" + "zzcd1" + "\"" + "=" + textZZCD1.Text);//长度
            MyEqu.set_Equation(38, "\"" + "zzyj1" + "\"" + "=" + textZZYJ1.Text);//圆角

            MyEqu.set_Equation(39, "\"" + "zzdj1" + "\"" + "=" + textZZDJ1.Text);//倒角

            // 左2
            MyEqu.set_Equation(40, "\"" + "zzzj2" + "\"" + "=" + textZZZJ2.Text);
            MyEqu.set_Equation(41, "\"" + "zzcd2" + "\"" + "=" + textZZCD2.Text);
            MyEqu.set_Equation(42, "\"" + "zzyj2" + "\"" + "=" + textZZYJ2.Text);
            MyEqu.set_Equation(43, "\"" + "zzdj2" + "\"" + "=" + textZZDJ2.Text);

            //左3
            MyEqu.set_Equation(44, "\"" + "zzzj3" + "\"" + "=" + textZZZJ3.Text);
            MyEqu.set_Equation(45, "\"" + "zzcd3" + "\"" + "=" + textZZCD3.Text);
            MyEqu.set_Equation(46, "\"" + "zzyj3" + "\"" + "=" + textZZYJ3.Text);
            MyEqu.set_Equation(47, "\"" + "zzdj3" + "\"" + "=" + textZZDJ3.Text);

            //左4
            MyEqu.set_Equation(48, "\"" + "zzzj4" + "\"" + "=" + textZZZJ4.Text);
            MyEqu.set_Equation(49, "\"" + "zzcd4" + "\"" + "=" + textZZCD4.Text);
            MyEqu.set_Equation(50, "\"" + "zzyj4" + "\"" + "=" + textZZYJ4.Text);
            MyEqu.set_Equation(51, "\"" + "zzdj4" + "\"" + "=" + textZZDJ4.Text);

            // 右1
            MyEqu.set_Equation(52, "\"" + "yzzj1" + "\"" + "=" + textYZZJ1.Text);
            MyEqu.set_Equation(53, "\"" + "yzcd1" + "\"" + "=" + textYZCD1.Text);
            MyEqu.set_Equation(54, "\"" + "yzyj1" + "\"" + "=" + textYZYJ1.Text);
            MyEqu.set_Equation(55, "\"" + "yzdj1" + "\"" + "=" + textYZDJ1.Text);

            //右2
            MyEqu.set_Equation(56, "\"" + "yzzj2" + "\"" + "=" + textYZZJ2.Text);
            MyEqu.set_Equation(57, "\"" + "yzcd2" + "\"" + "=" + textYZCD2.Text);
            MyEqu.set_Equation(58, "\"" + "yzyj2" + "\"" + "=" + textYZYJ2.Text);
            MyEqu.set_Equation(59, "\"" + "yzdj2" + "\"" + "=" + textYZDJ2.Text);

            //右3
            MyEqu.set_Equation(60, "\"" + "yzzj3" + "\"" + "=" + textYZZJ3.Text);

            MyEqu.set_Equation(61, "\"" + "yzcd3" + "\"" + "=" + textYZCD3.Text);
            MyEqu.set_Equation(62, "\"" + "yzyj3" + "\"" + "=" + textYZYJ3.Text);
            MyEqu.set_Equation(63, "\"" + "yzdj3" + "\"" + "=" + textYZDJ3.Text);

            //右4
            MyEqu.set_Equation(64, "\"" + "yzzj4" + "\"" + "=" + textYZZJ4.Text);
            MyEqu.set_Equation(65, "\"" + "yzcd4" + "\"" + "=" + textYZCD4.Text);
            MyEqu.set_Equation(66, "\"" + "yzyj4" + "\"" + "=" + textYZYJ4.Text);
            MyEqu.set_Equation(67, "\"" + "yzdj4" + "\"" + "=" + textYZDJ4.Text);


            //  左键槽
            double b = double.Parse(text_bx.Text);
            double zzj1 = double.Parse(textZZZJ1.Text);
            double zcd1 = double.Parse(textZZCD1.Text);
            double zzj2 = double.Parse(textZZZJ2.Text);
            double zcd2 = double.Parse(textZZCD2.Text);
            double zzj3 = double.Parse(textZZZJ3.Text);
            double zcd3 = double.Parse(textZZCD3.Text);
            double zzj4 = double.Parse(textZZZJ4.Text);
            double zjwz = double.Parse(textZJCWZ.Text);
            double zjs = double.Parse(textZJCS.Text);
            int i = int.Parse(textZJCZD.Text);

            switch (i)
            {
                case 1:
                    double ZJCWZ1 = b + zjwz;
                    double ZJCS1 = zzj1 / 2 - zjs;
                    MyEqu.set_Equation(100, "\"" + "zjck" + "\"" + "=" + textZJCK.Text);
                    MyEqu.set_Equation(101, "\"" + "zjcc" + "\"" + "=" + textZJCC.Text);
                    MyEqu.set_Equation(102, "\"" + "zjcs" + "\"" + "=" + ZJCS1);
                    MyEqu.set_Equation(103, "\"" + "zjcwz" + "\"" + "=" + ZJCWZ1);
                    break;

                case 2:
                    double ZJCWZ2 = b + zcd1 + zjwz;
                    double ZJCS2 = zzj2 / 2 - zjs;
                    MyEqu.set_Equation(100, "\"" + "zjck" + "\"" + "=" + textZJCK.Text);
                    MyEqu.set_Equation(101, "\"" + "zjcc" + "\"" + "=" + textZJCC.Text);
                    MyEqu.set_Equation(102, "\"" + "zjcs" + "\"" + "=" + ZJCS2);
                    MyEqu.set_Equation(103, "\"" + "zjcwz" + "\"" + "=" + ZJCWZ2);

                    break;


                case 3:
                    double ZJCWZ3 = b + zcd1 + zcd2 + zjwz;
                    double ZJCS3 = zzj3 / 2 - zjs;
                    MyEqu.set_Equation(100, "\"" + "zjck" + "\"" + "=" + textZJCK.Text);
                    MyEqu.set_Equation(101, "\"" + "zjcc" + "\"" + "=" + textZJCC.Text);
                    MyEqu.set_Equation(102, "\"" + "zjcs" + "\"" + "=" + ZJCS3);
                    MyEqu.set_Equation(103, "\"" + "zjcwz" + "\"" + "=" + ZJCWZ3);

                    break;

                case 4:
                    double ZJCWZ4 = b + zcd1 + zcd2 + zcd3 + zjwz;
                    double ZJCS4 = zzj4 / 2 - zjs;
                    MyEqu.set_Equation(100, "\"" + "zjck" + "\"" + "=" + textZJCK.Text);
                    MyEqu.set_Equation(101, "\"" + "zjcc" + "\"" + "=" + textZJCC.Text);
                    MyEqu.set_Equation(102, "\"" + "zjcs" + "\"" + "=" + ZJCS4);
                    MyEqu.set_Equation(103, "\"" + "zjcwz" + "\"" + "=" + ZJCWZ4);
                    break;


            }


            //  右键槽

            double yzj1 = double.Parse(textYZZJ1.Text);
            double ycd1 = double.Parse(textYZCD1.Text);
            double yzj2 = double.Parse(textYZZJ2.Text);
            double ycd2 = double.Parse(textYZCD2.Text);
            double yzj3 = double.Parse(textYZZJ3.Text);
            double ycd3 = double.Parse(textYZCD3.Text);
            double yzj4 = double.Parse(textYZZJ4.Text);
            double yjwz = double.Parse(textYJCWZ.Text);
            double yjs = double.Parse(textYJCS.Text);
            int j = int.Parse(textYJCZD.Text);

            switch (j)
            {
                case 1:
                    double YJCWZ1 = yjwz;
                    double YJCS1 = yzj1 / 2 - yjs;
                    MyEqu.set_Equation(104, "\"" + "yjcc" + "\"" + "=" + textYJCC.Text);
                    MyEqu.set_Equation(105, "\"" + "yjcwz" + "\"" + "=" + YJCWZ1);
                    MyEqu.set_Equation(106, "\"" + "yjck" + "\"" + "=" + textYJCK.Text);
                    MyEqu.set_Equation(107, "\"" + "yjcs" + "\"" + "=" + YJCS1);
                    break;

                case 2:
                    double YJCWZ2 = ycd1 + yjwz;
                    double YJCS2 = yzj2 / 2 - yjs;
                    MyEqu.set_Equation(104, "\"" + "yjcc" + "\"" + "=" + textYJCC.Text);
                    MyEqu.set_Equation(105, "\"" + "yjcwz" + "\"" + "=" + YJCWZ2);

                    MyEqu.set_Equation(106, "\"" + "yjck" + "\"" + "=" + textYJCK.Text);
                    MyEqu.set_Equation(107, "\"" + "yjcs" + "\"" + "=" + YJCS2);
                    break;


                case 3:
                    double YJCWZ3 = ycd1 + ycd2 + yjwz;
                    double YJCS3 = yzj3 / 2 - yjs;
                    MyEqu.set_Equation(104, "\"" + "yjcc" + "\"" + "=" + textYJCC.Text);
                    MyEqu.set_Equation(105, "\"" + "yjcwz" + "\"" + "=" + YJCWZ3);
                    MyEqu.set_Equation(106, "\"" + "yjck" + "\"" + "=" + textYJCK.Text);
                    MyEqu.set_Equation(107, "\"" + "yjcs" + "\"" + "=" + YJCS3);

                    break;

                case 4:
                    double YJCWZ4 = ycd1 + ycd2 + ycd3 + yjwz;
                    double YJCS4 = yzj4 / 2 - yjs;
                    MyEqu.set_Equation(104, "\"" + "yjcc" + "\"" + "=" + textYJCC.Text);
                    MyEqu.set_Equation(105, "\"" + "yjcwz" + "\"" + "=" + YJCWZ4);
                    MyEqu.set_Equation(106, "\"" + "yjck" + "\"" + "=" + textYJCK.Text);
                    MyEqu.set_Equation(107, "\"" + "yjcs" + "\"" + "=" + YJCS4);
                    break;


            }

            MyEqu.EvaluateAll();
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);

            int revatal2 = MyEqu.GetCount();
            for (int k = 0; k < (revatal2); k++)
            {
                MyEqu.ChangeSuppressionForAllConfigurations(k, true);

            }
            MessageBox.Show("压缩");


        }

      

        private void button3_Click(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            XmlNode z;
            XmlNode root = xmlDoc.DocumentElement;
            z = root.SelectSingleNode("//各级齿轮参数[级数=" + "'" + comboBox1.Text.Trim() + "'" + "]");
            StringReader reader = new StringReader(z.OuterXml);
            ds.ReadXml(reader);
            text_mn2.Text = ds.Tables[0].Rows[0]["法向模数"].ToString().Trim();
            text_an2.Text = ds.Tables[0].Rows[0]["法向压力角"].ToString().Trim();
            text_lb12.Text = ds.Tables[0].Rows[0]["螺旋角"].ToString().Trim();
            txtPinionZ.Text = ds.Tables[0].Rows[0]["齿数1"].ToString().Trim();
            text_zd.Text = ds.Tables[0].Rows[0]["齿数2"].ToString().Trim();
            text_xnx.Text = ds.Tables[0].Rows[0]["法向变位系数1"].ToString().Trim();
            text_xnd.Text = ds.Tables[0].Rows[0]["法向变位系数2"].ToString().Trim();
            text_bx.Text = ds.Tables[0].Rows[0]["齿宽"].ToString().Trim();
            text_bd.Text = ds.Tables[0].Rows[0]["齿宽"].ToString().Trim();
            //.Text = ds.Tables[0].Rows[0]["螺旋角"].ToString().Trim();

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
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
            GraphPane graphPane = zedGraphControl1.GraphPane;

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
            Graphics g = zedGraphControl1.CreateGraphics();
            graphPane.XAxis.Scale.PickScale(graphPane, g, scaleFactor);
            graphPane.YAxis.Scale.PickScale(graphPane, g, scaleFactor);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //int Errors=0;
            //ModelDoc2 swModel;
            // swModel = (ModelDoc2)SwAddin.iSwApp.ActivateDoc3("loaded_document", false, (int)swRebuildOnActivation_e.swUserDecision, ref Errors);                      

            //make sure we have a part open
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"c:\mygear.SLDPRT",
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            //SldWorks swApp;
            IModelDoc2 swModel = default(IModelDoc2);
            ISelectionMgr swSelMgr = default(ISelectionMgr);
            Feature swFeat = default(Feature);
            HelixFeatureData swHelix = default(HelixFeatureData);
            bool bRet;
            bool boolstatus;

            swModel = SwAddin.iSwApp.ActiveDoc as ModelDoc2;
            swSelMgr = swModel.SelectionManager as SelectionMgr;

            // swModel = (ModelDoc2)swApp.ActiveDoc;
            boolstatus = swModel.Extension.SelectByID2("螺旋线", "REFERENCECURVES", 0, 0, 0, false, 0, null, 0);
            //   swSelMgr = (ISelectionMgr)swModel.SelectionManager;
            swFeat = swSelMgr.GetSelectedObject6(1, -1);
            swHelix = (HelixFeatureData)swFeat.GetDefinition();

            swHelix.Clockwise = false;
            bRet = swFeat.ModifyDefinition(swHelix, swModel, null);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            XElement root = XElement.Load(@"C:\圆柱齿轮.xml");
            root.Element("设计参数").Element("齿数").Value = txtPinionZ.Text;
            root.Save(@"C:\圆柱齿轮.xml");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            XElement root = XElement.Load(@"C:\圆柱齿轮.xml");
            DataSet myDataSet = new DataSet();
            myDataSet.ReadXml(root.Element("腹板结构").ToString());
            // root.Element("设计参数").Element("齿数").Value = txtPinionZ.Text;
            //root.Save(@"C:\圆柱齿轮.xml");
            //   dgvDiameter.DataSource = myDataSet;
        }
        //public string gearFileName;
        private void button1_Click_2(object sender, EventArgs e)
        {
            //类变量，调研前要赋值
            xlmFileName = @"C:\圆柱齿轮.xml";//齿轮的XML文件名字

            XElement xe = XElement.Load(xlmFileName);

            //读取齿轮模型文件名字
            IEnumerable<XElement> elements = from 齿轮名称 in xe.Elements("基本信息")// where PInfo.Attribute("ID").Value == strID
                                             select 齿轮名称;
            //MessageBox.Show(elements.ElementAt(0).Element("齿轮名称").Value);

            //利用齿轮模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(elements.ElementAt(0).Element("齿轮名称").Value,
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            elements = from 基圆直径 in xe.Elements("尺寸计算")// where PInfo.Attribute("ID").Value == strID
                       select 基圆直径;
            double d_b = double.Parse(elements.ElementAt(0).Element("基圆直径").Value) / 1000;          //基圆直径
            double d_a = double.Parse(elements.ElementAt(0).Element("齿顶圆直径").Value) / 1000;
            double u = Math.Sqrt(((d_a * 1000) / (d_b * 1000)) * ((d_a * 1000) / (d_b * 1000)) - 1);   //滚动角
            double d = double.Parse(elements.ElementAt(0).Element("分度圆直径").Value) / 1000;
            double s = double.Parse(elements.ElementAt(0).Element("齿弦厚").Value) / 1000;
            double d_f = double.Parse(elements.ElementAt(0).Element("齿根圆直径").Value) / 1000;
            double C_CD = double.Parse(elements.ElementAt(0).Element("齿顶倒角").Value) / 1000;


            elements = from 螺旋角 in xe.Elements("设计参数")// where PInfo.Attribute("ID").Value == strID
                       select 螺旋角;
            double beta = double.Parse(elements.ElementAt(0).Element("螺旋角").Value);
            double luoju = Math.PI * d / Math.Tan(beta * Math.PI / 180);
            double B = double.Parse(elements.ElementAt(0).Element("齿宽").Value) / 1000;
            double Z = double.Parse(elements.ElementAt(0).Element("齿数").Value);
            string xuanxiang = elements.ElementAt(0).Element("旋向").Value;

            elements = from 腹板深 in xe.Elements("腹板结构")// where PInfo.Attribute("ID").Value == strID
                       select 腹板深;
            double D_2 = double.Parse(elements.ElementAt(0).Element("腹板外径D2").Value) / 1000;
            double D_1 = double.Parse(elements.ElementAt(0).Element("腹板内径D1").Value) / 1000;
            double KJ_FB = double.Parse(elements.ElementAt(0).Element("腹板孔径dk").Value) / 1000;
            double N_FB = double.Parse(elements.ElementAt(0).Element("腹板孔数").Value);
            double D_FB = double.Parse(elements.ElementAt(0).Element("腹板深").Value) / 1000;
            double R_FB = double.Parse(elements.ElementAt(0).Element("腹板圆角").Value) / 1000;
            double C_FB = double.Parse(elements.ElementAt(0).Element("腹板倒角").Value) / 1000;


            elements = from 轮毂直径 in xe.Elements("轮毂结构")// where PInfo.Attribute("ID").Value == strID
                       select 轮毂直径;
            double D_LG = double.Parse(elements.ElementAt(0).Element("轮毂直径").Value) / 1000;
            double J_K = double.Parse(elements.ElementAt(0).Element("键宽").Value) / 1000;
            double J_S = double.Parse(elements.ElementAt(0).Element("键深").Value) / 1000;
            double C_LG = double.Parse(elements.ElementAt(0).Element("轮毂倒角").Value) / 1000;
            double R_LG = double.Parse(elements.ElementAt(0).Element("轮毂圆角").Value) / 1000;

            //SldWorks swApp;
            // IModelDoc2 swModel = default(IModelDoc2);
            ISelectionMgr swSelMgr = default(ISelectionMgr);
            Feature swFeat = default(Feature);
            HelixFeatureData swHelix = default(HelixFeatureData);
            bool bRet;
            bool boolstatus;

            swSelMgr = modDoc.SelectionManager as SelectionMgr;

            // swModel = (ModelDoc2)swApp.ActiveDoc;
            boolstatus = modDoc.Extension.SelectByID2("螺旋线", "REFERENCECURVES", 0, 0, 0, false, 0, null, 0);
            swFeat = swSelMgr.GetSelectedObject6(1, -1);
            swHelix = (HelixFeatureData)swFeat.GetDefinition();
            if (xuanxiang.Equals("L"))
            {
                swHelix.Clockwise = true;
            }
            else
            {
                swHelix.Clockwise = false;
            }
            bRet = swFeat.ModifyDefinition(swHelix, modDoc, null);



            IDimension myDimension = null;
           // int Config_count = 1;
            //string Config_names="aa";

            myDimension = (IDimension)modDoc.Parameter(@"基圆@草图1");
            myDimension.SetSystemValue3(d_b, (int)swInConfigurationOpts_e.swAllConfiguration, null);//.set.SystemValue = d_b;

            myDimension = (IDimension)modDoc.Parameter(@"U@草图1");
            myDimension.SetSystemValue3(u * Math.PI / 180, (int)swInConfigurationOpts_e.swAllConfiguration, null); 
            //myDimension.SystemValue = u * Math.PI / 180;              //滚动角

            myDimension = (IDimension)modDoc.Parameter(@"分度圆@草图1");
            myDimension.SetSystemValue3(d, (int)swInConfigurationOpts_e.swAllConfiguration, null);
            myDimension.SystemValue = d;

            myDimension = (IDimension)modDoc.Parameter(@"齿弦厚@草图1");
            myDimension.SystemValue = s;

            myDimension = (IDimension)modDoc.Parameter(@"齿根圆@草图5");
            myDimension.SystemValue = d_f;

            myDimension = (IDimension)modDoc.Parameter(@"D3@螺旋线");
            myDimension.SystemValue = B + 0.001;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿根圆");
            myDimension.SystemValue = B + 0.001;

            myDimension = (IDimension)modDoc.Parameter(@"B@草图6");
            myDimension.SystemValue = B;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿廓阵列");
            myDimension.SystemValue = Z;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图6");
            myDimension.SystemValue = d_a + 0.002;

            myDimension = (IDimension)modDoc.Parameter(@"D4@螺旋线");
            myDimension.SystemValue = luoju;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿顶倒角");
            myDimension.SystemValue = C_CD;

            myDimension = (IDimension)modDoc.Parameter(@"腹板外径@草图7");
            myDimension.SystemValue = D_2;

            myDimension = (IDimension)modDoc.Parameter(@"腹板内径@草图7");
            myDimension.SystemValue = D_1;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图7");
            myDimension.SystemValue = (D_1 + D_2) / 2;

            myDimension = (IDimension)modDoc.Parameter(@"D2@草图7");
            myDimension.SystemValue = 180 / N_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D1@腹板");
            myDimension.SystemValue = D_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D1@腹板圆角");
            myDimension.SystemValue = R_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D1@腹板倒角");
            myDimension.SystemValue = C_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D1@腹板孔阵列");
            myDimension.SystemValue = N_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D2@草图6");
            myDimension.SystemValue = B + 1;
           
            myDimension = (IDimension)modDoc.Parameter(@"D3@草图7");
            myDimension.SystemValue = KJ_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿顶倒角阵列");
            myDimension.SystemValue = Z;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图8");
            myDimension.SystemValue = D_LG;

            myDimension = (IDimension)modDoc.Parameter(@"D2@草图8");
            myDimension.SystemValue = J_K;

            myDimension = (IDimension)modDoc.Parameter(@"D3@草图8");
            myDimension.SystemValue = J_S;

            myDimension = (IDimension)modDoc.Parameter(@"D1@轮毂倒角");
            myDimension.SystemValue = C_LG;

            myDimension = (IDimension)modDoc.Parameter(@"D1@轮毂圆角");
            myDimension.SystemValue = R_LG;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图9");
            myDimension.SystemValue = d_f / 2 - 0.001;

            myDimension = (IDimension)modDoc.Parameter(@"D2@草图9");
            myDimension.SystemValue = d_a / 2;

            myDimension = (IDimension)modDoc.Parameter(@"D3@草图9");
            myDimension.SystemValue = B;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图10");
            myDimension.SystemValue = d_f;

            myDimension = (IDimension)modDoc.Parameter(@"D3@阵列1");
            myDimension.SystemValue = 180 / N_FB;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿顶倒角1");
            myDimension.SystemValue = C_CD;

            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
        }

        public string xlmFileName;
        private void button7_Click_1(object sender, EventArgs e)
        {
            //类变量，调研前要赋值
            xlmFileName = @"C:\齿轮轴.xml";//齿轮的XML文件名字

            XElement xe = XElement.Load(xlmFileName);

            //读取齿轮模型文件名字
            IEnumerable<XElement> elements = from 齿轮轴名称 in xe.Elements("基本信息")// where PInfo.Attribute("ID").Value == strID
                                             select 齿轮轴名称;
            //MessageBox.Show(elements.ElementAt(0).Element("齿轮名称").Value);

            //利用齿轮模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(elements.ElementAt(0).Element("齿轮轴名称").Value,
                    (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }


            elements = from 基圆直径 in xe.Elements("尺寸计算")// where PInfo.Attribute("ID").Value == strID
                       select 基圆直径;
            double d_b = double.Parse(elements.ElementAt(0).Element("基圆直径").Value) / 1000;          //基圆直径
            double d_a = double.Parse(elements.ElementAt(0).Element("齿顶圆直径").Value) / 1000;
            double u = Math.Sqrt(((d_a * 1000) / (d_b * 1000)) * ((d_a * 1000) / (d_b * 1000)) - 1);   //滚动角
            double d = double.Parse(elements.ElementAt(0).Element("分度圆直径").Value) / 1000;
            double s = double.Parse(elements.ElementAt(0).Element("齿弦厚").Value) / 1000;
            double d_f = double.Parse(elements.ElementAt(0).Element("齿根圆直径").Value) / 1000;
            double C_CD = double.Parse(elements.ElementAt(0).Element("齿顶倒角").Value) / 1000;


            elements = from 螺旋角 in xe.Elements("设计参数")// where PInfo.Attribute("ID").Value == strID
                       select 螺旋角;
            double beta = double.Parse(elements.ElementAt(0).Element("螺旋角").Value);
            double luoju = Math.PI * d / Math.Tan(beta * Math.PI / 180);
            double B = double.Parse(elements.ElementAt(0).Element("齿宽").Value) / 1000;
            double Z = double.Parse(elements.ElementAt(0).Element("齿数").Value);
            string xuanxiang = elements.ElementAt(0).Element("旋向").Value;

            double numberOfLeftShaft = double.Parse(elements.ElementAt(0).Element("左轴段数").Value);
            double numberOfRightShaft = double.Parse(elements.ElementAt(0).Element("右轴段数").Value);
            double numberOfKeySeat = double.Parse(elements.ElementAt(0).Element("键槽数").Value);
            

            //SldWorks swApp;
            // IModelDoc2 swModel = default(IModelDoc2);
            ISelectionMgr swSelMgr = default(ISelectionMgr);
            Feature swFeat = default(Feature);
            HelixFeatureData swHelix = default(HelixFeatureData);
            bool bRet;
            bool boolstatus;

            swSelMgr = modDoc.SelectionManager as SelectionMgr;

            // swModel = (ModelDoc2)swApp.ActiveDoc;
            boolstatus = modDoc.Extension.SelectByID2("螺旋线", "REFERENCECURVES", 0, 0, 0, false, 0, null, 0);
            swFeat = swSelMgr.GetSelectedObject6(1, -1);
            swHelix = (HelixFeatureData)swFeat.GetDefinition();
            if (xuanxiang.Equals("L"))
            {
                swHelix.Clockwise = true;
            }
            else
            {
                swHelix.Clockwise = false;
            }
            bRet = swFeat.ModifyDefinition(swHelix, modDoc, null);


            IDimension myDimension = null;

            myDimension = (IDimension)modDoc.Parameter(@"基圆@草图1");
            myDimension.SystemValue = d_b;

            myDimension = (IDimension)modDoc.Parameter(@"U@草图1");
            myDimension.SystemValue = u * Math.PI / 180;              //滚动角

            myDimension = (IDimension)modDoc.Parameter(@"分度圆@草图1");
            myDimension.SystemValue = d;

            myDimension = (IDimension)modDoc.Parameter(@"齿弦厚@草图1");
            myDimension.SystemValue = s;

            myDimension = (IDimension)modDoc.Parameter(@"齿根圆@草图5");
            myDimension.SystemValue = d_f;

            myDimension = (IDimension)modDoc.Parameter(@"D3@螺旋线");
            myDimension.SystemValue = B + 0.001;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿根圆");
            myDimension.SystemValue = B + 0.001;

            myDimension = (IDimension)modDoc.Parameter(@"B@草图6");
            myDimension.SystemValue = B;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿廓阵列");
            myDimension.SystemValue = Z;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图6");
            myDimension.SystemValue = d_a + 0.002;

            myDimension = (IDimension)modDoc.Parameter(@"D2@草图6");
            myDimension.SystemValue = B + 1;

            myDimension = (IDimension)modDoc.Parameter(@"D4@螺旋线");
            myDimension.SystemValue = luoju;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿顶倒角");
            myDimension.SystemValue = C_CD;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿顶倒角阵列");
            myDimension.SystemValue = Z;

            myDimension = (IDimension)modDoc.Parameter(@"D1@草图7");
            myDimension.SystemValue = d_a;

            myDimension = (IDimension)modDoc.Parameter(@"D1@齿顶倒角1");
            myDimension.SystemValue = C_CD;


            string zzd;
            XElement zzdEle = xe.Element("左轴段");
            for (int i = 1; i <= numberOfLeftShaft; i++)
            {
                zzd = "左轴段" + i.ToString();
               
                if (zzdEle != null)
                    elements = from 直径 in zzdEle.Elements(zzd)// where PInfo.Attribute("ID").Value == strID
                               select 直径;
                double D = double.Parse(elements.ElementAt(0).Element("直径").Value) / 1000;
                double L = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double C = double.Parse(elements.ElementAt(0).Element("倒角").Value) / 1000;
                double R = double.Parse(elements.ElementAt(0).Element("圆角").Value) / 1000;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd + "草图");
                myDimension.SystemValue = D;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd);
                myDimension.SystemValue = L;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd + "倒角");
                myDimension.SystemValue = C;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd + "圆角");
                myDimension.SystemValue = R;
            }
            XElement yzdEle = xe.Element("右轴段");
            string yzd;
            for (int i = 1; i <= numberOfRightShaft; i++)
            {
                yzd = "右轴段" + i.ToString();
                elements = from 直径 in yzdEle.Elements(yzd)// where PInfo.Attribute("ID").Value == strID
                           select 直径;
                double D = double.Parse(elements.ElementAt(0).Element("直径").Value) / 1000;
                double L = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double C = double.Parse(elements.ElementAt(0).Element("倒角").Value) / 1000;
                double R = double.Parse(elements.ElementAt(0).Element("圆角").Value) / 1000;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd + "草图");
                myDimension.SystemValue = D;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd);
                myDimension.SystemValue = L;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd + "倒角");
                myDimension.SystemValue = C;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd + "圆角");
                myDimension.SystemValue = R;
            }

            XElement keySeatEle = xe.Element("键槽");
            for (int i = 1; i <= numberOfKeySeat; i++)
            {
                string keySeat = "键槽" + i.ToString();
                elements = from 直径 in keySeatEle.Elements(keySeat)// where PInfo.Attribute("ID").Value == strID
                           select 直径;
                double K = double.Parse(elements.ElementAt(0).Element("宽").Value) / 1000;
                double L = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double C = double.Parse(elements.ElementAt(0).Element("定位").Value) / 1000;
                double S = double.Parse(elements.ElementAt(0).Element("深").Value) / 1000;
                string weizhi=elements.ElementAt(0).Element("位置").Value;
                if(weizhi.Substring(0,1).Equals ("左"))
                {
                   elements = from 直径 in zzdEle.Elements(weizhi)// where PInfo.Attribute("ID").Value == strID
                           select 直径;
                }
                else
                {
                    elements = from 直径 in yzdEle.Elements(weizhi)// where PInfo.Attribute("ID").Value == strID
                           select 直径;
                }
                double D = double.Parse(elements.ElementAt(0).Element("直径").Value)/ 1000 ;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + keySeat);
                myDimension.SystemValue = 0.5 * D -S;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + keySeat + "草图");
                myDimension.SystemValue = K;
                myDimension = (IDimension)modDoc.Parameter(@"D2@" + keySeat + "草图");
                myDimension.SystemValue = L;
                myDimension = (IDimension)modDoc.Parameter(@"D3@" + keySeat + "草图");
                myDimension.SystemValue = C;
            }        

            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
            modDoc.ViewZoomtofit2 ();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            //利用齿轮模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(@"C:\第三级齿轮.SLDDRW",
                    (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }
            
            IModelDocExtension swModelDocExt = default(IModelDocExtension);
            ICustomPropertyManager swCustProp = default(ICustomPropertyManager);
            
            int status;
            swModelDocExt = modDoc.Extension;

            // Get the custom property data
            swCustProp = swModelDocExt.get_CustomPropertyManager("");
            status = swCustProp.Set("齿形角","21");


        }
    }
  
}