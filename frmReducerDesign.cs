using System;
using System.Data;
using System.Windows.Forms;

using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Linq;

using System.Collections.Generic;

using System.Drawing;

using ZedGraph;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;

//using SolidWorksTools;
//using SolidWorksTools.File;

namespace ReducerDesign
{
    public struct Parameter
    {
        public string name;
        public double value;
    }

    public struct Coordinate
    {
        public double x;
        public double y;
    }

    public partial class frmReducerDesign : Form
    {
        private XmlDocument xmlDoc;

        private DataTable dtAsmType;//cmbType的数据源
        private DataTable dtdgvParameter;//dgvParameter的数据源
        private DataTable dtdgvDiameter;//dgvCalculate数据源

        private string strXMLPath;//XML的路径


        private string treeNode;

        //private Boolean blModify;//允许修改的权限

        //private XDocument xDoc;
        private XElement xEle;

        private double pressuerAngle;// 压力角
        // 小齿轮
        // private double pinionReferenceDiameter; //分度圆直径
        private double pinionBaseDiameter;  // 基圆直径  Db = D * Cos(a) 
        // private double pinionToothThickness; //齿厚
        private double pinionTipDiameter;//齿顶圆
        private int pinionZ;  // 齿数


        public frmReducerDesign()
        {
            InitializeComponent();
            dtAsmType = new DataTable();
            strXMLPath = @"C:\减速器1\XML\减速器结构.xml";

            dtdgvDiameter = new DataTable();
            dtdgvParameter = new DataTable();

            //blModify=new Boolean();
            GraphPane graphPane = GraphControl.GraphPane;
            //graphPane.XAxis.Title.Text = "Chord";
            //graphPane.YAxis.Title.Text = "Thickness";
            graphPane.XAxis.Title.IsVisible = false;
            graphPane.YAxis.Title.IsVisible = false;
            graphPane.XAxis.MajorGrid.IsVisible = true;
            graphPane.YAxis.MajorGrid.IsVisible = true;
            //graphPane.Legend.IsVisible = false;
            graphPane.Legend.Position = ZedGraph.LegendPos.InsideTopRight;
            graphPane.Title.IsVisible = false;
            //graphPane.Title.Text = "ZedGraph";   
            GraphControl.Resize += new System.EventHandler(this.zedGraphControl1_Resize);
            graphPane.AxisChangeEvent += new GraphPane.AxisChangeEventHandler(graphPane_AxisChangeEvent);
        }

        private void zedGraphControl1_Resize(object sender, EventArgs e)
        {
            GraphControl.AxisChange();
        }

        private void graphPane_AxisChangeEvent(GraphPane target)
        {
            GraphPane graphPane = GraphControl.GraphPane;
            // Correct the scale so that the two axes are 1:1 aspect ratio    
            double scalex2 = (graphPane.XAxis.Scale.Max - graphPane.XAxis.Scale.Min) / graphPane.Chart.Rect.Width;
            double scaley2 = (graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min) / graphPane.Chart.Rect.Height;
            if (scalex2 > scaley2)
            {
                double diff = graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min;
                double new_diff = graphPane.Chart.Rect.Height * scalex2;
                graphPane.YAxis.Scale.Min -= (new_diff - diff) / 2.0;
                graphPane.YAxis.Scale.Max += (new_diff - diff) / 2.0;
            }
            else if (scaley2 > scalex2)
            {
                double diff = graphPane.XAxis.Scale.Max - graphPane.XAxis.Scale.Min;
                double new_diff = graphPane.Chart.Rect.Width * scaley2;
                graphPane.XAxis.Scale.Min -= (new_diff - diff) / 2.0;
                graphPane.XAxis.Scale.Max += (new_diff - diff) / 2.0;
            }

            // Recompute the grid lines   
            float scaleFactor = graphPane.CalcScaleFactor();
            Graphics g = GraphControl.CreateGraphics();
            graphPane.XAxis.Scale.PickScale(graphPane, g, scaleFactor);
            graphPane.YAxis.Scale.PickScale(graphPane, g, scaleFactor);
        }
        #region Load函数

        private void frmReducerDesign_Load(object sender, EventArgs e)
        {
            //1.登陆界面后，首先查找XML文件，查询现有的所有减速机型号，赋给cmbType，以供选择 ；
            initdtAsmType();
            GetAssembleXML();


            this.cmbType.SelectedIndex = -1;

            initdtdgvParameter();
            initdtdgvDiameter();

            //blModify = false;//未获取修改权限
        }

        #endregion

        #region 初始化表格
        //初始化dtAsmType
        private void initdtAsmType()
        {

            dtAsmType.Rows.Clear();
            dtAsmType.Columns.Clear();

            dtAsmType.Columns.Add("TypeNO", System.Type.GetType("System.String"));

        }

        //初始化dtdgvParameter
        private void initdtdgvParameter()
        {

            dtdgvParameter.Rows.Clear();
            dtdgvParameter.Columns.Clear();

            dtdgvParameter.Columns.Add("jishu", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("moshu", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("zhongxinju", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("chishu1", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("chishu2", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("luoxuanjiao", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("yalijiao", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("bianweixishu1", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("bianweixishu2", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("chidinggaoxishu1", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("chidinggaoxishu2", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("dingxixishu1", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("dingxixishu2", System.Type.GetType("System.String"));
            dtdgvParameter.Columns.Add("chikuan", System.Type.GetType("System.String"));
        }

        //初始化dtdgvDiameter
        private void initdtdgvDiameter()
        {

            dtdgvDiameter.Rows.Clear();
            dtdgvDiameter.Columns.Clear();

            dtdgvDiameter.Columns.Add("jishu", System.Type.GetType("System.String"));
            dtdgvDiameter.Columns.Add("fenduyuan1", System.Type.GetType("System.String"));
            dtdgvDiameter.Columns.Add("fenduyuan2", System.Type.GetType("System.String"));
            dtdgvDiameter.Columns.Add("chidingyuan1", System.Type.GetType("System.String"));
            dtdgvDiameter.Columns.Add("chidingyuan2", System.Type.GetType("System.String"));
            dtdgvDiameter.Columns.Add("chigenyuan1", System.Type.GetType("System.String"));
            dtdgvDiameter.Columns.Add("chigenyuan2", System.Type.GetType("System.String"));

        }


        #endregion




        #region TreeView相关

        private void GetAssembleXML()
        {
            //XmlNodeList nodes = xmlDoc.SelectNodes("//各级齿轮参数");

            xEle = XElement.Load(strXMLPath);

            IEnumerable<XElement> childElements = xEle.Elements("减速机");

            IEnumerable<XElement> childnd = xEle.Elements();

            //循环获取的XML文件，提取其中的型号信息；
            foreach (XElement ch in childnd)
            {
                string strTypeNO = ch.Name.ToString();
                DataRow drAsm = dtAsmType.NewRow();
                drAsm["TypeNO"] = strTypeNO;
                dtAsmType.Rows.Add(drAsm);
                dtAsmType.AcceptChanges();
            }
            this.cmbType.DataSource = dtAsmType;
            this.cmbType.ValueMember = "TypeNO";
            this.cmbType.DisplayMember = "TypeNO";
            this.cmbType.SelectedIndex = -1;
        }


        //选择cmbType中的选项
        private void cmbType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.cmbType.SelectedIndex == -1)
            {
                this.treeAssembly.Nodes.Clear();
                return;
            }
            else
            {
                //获取选择项
                string strTypeNO = this.cmbType.SelectedValue.ToString();

                //根据选择的型号查询具体的结构
                this.xmlDoc = new XmlDocument();
                try
                {
                    this.xmlDoc.Load(strXMLPath);
                    string strFndNode = "减速机/" + strTypeNO;
                    XmlNodeList nodes = this.xmlDoc.SelectNodes(strFndNode);

                    this.treeAssembly.BeginUpdate();
                    this.treeAssembly.Nodes.Clear();
                    this.ConvertXmlNodeToTreeNode(nodes, this.treeAssembly.Nodes);
                    this.treeAssembly.EndUpdate();

                    this.treeAssembly.ExpandAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("程序发生错误：" + ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }


        //递归调用，建立TreeView结构树
        private void ConvertXmlNodeToTreeNode(XmlNodeList xmlNodes, TreeNodeCollection treeNodes)
        {
            foreach (XmlNode xmlNode in xmlNodes)
            {
                string nodeText = xmlNode.Name.ToString();

                string nodeTreeNode = "";
                string nodeValue = "";

                if (xmlNode.ChildNodes.Count == 1)
                {
                    nodeValue = xmlNode.InnerText.ToString();
                    nodeTreeNode = nodeValue;
                }
                //if (xmlNode.Value == null)

                else
                {
                    nodeTreeNode = nodeText;
                }

                TreeNode newTreeNode = new TreeNode(nodeTreeNode);
                newTreeNode.Tag = nodeValue;
                if (xmlNode.ChildNodes.Count != 1)
                {
                    this.ConvertXmlNodeToTreeNode(xmlNode.ChildNodes, newTreeNode.Nodes);
                }
                treeNodes.Add(newTreeNode);
            }
        }
        string currentLevel;
        string fatherLevel;
        string U2Level;
        string strModelFileName;

        //选择TreeView上的一个节点
        private void treeAssembly_AfterSelect(object sender, TreeViewEventArgs e)
        {

            xEle = XElement.Load(strXMLPath);
            DataTable leftShaftParameter = new DataTable();
            leftShaftParameter.Columns.Add("轴段", System.Type.GetType("System.String"));
            leftShaftParameter.Columns.Add("直径", System.Type.GetType("System.String"));
            leftShaftParameter.Columns.Add("长度", System.Type.GetType("System.String"));
            leftShaftParameter.Columns.Add("倒角", System.Type.GetType("System.String"));
            leftShaftParameter.Columns.Add("圆角", System.Type.GetType("System.String"));

            DataTable rightShaftParameter = new DataTable();
            rightShaftParameter.Columns.Add("轴段", System.Type.GetType("System.String"));
            rightShaftParameter.Columns.Add("直径", System.Type.GetType("System.String"));
            rightShaftParameter.Columns.Add("长度", System.Type.GetType("System.String"));
            rightShaftParameter.Columns.Add("倒角", System.Type.GetType("System.String"));
            rightShaftParameter.Columns.Add("圆角", System.Type.GetType("System.String"));

            DataTable keyParameter = new DataTable();
            keyParameter.Columns.Add("序号", System.Type.GetType("System.String"));
            keyParameter.Columns.Add("位置", System.Type.GetType("System.String"));
            keyParameter.Columns.Add("长度", System.Type.GetType("System.String"));
            keyParameter.Columns.Add("定位", System.Type.GetType("System.String"));
            keyParameter.Columns.Add("宽", System.Type.GetType("System.String"));
            keyParameter.Columns.Add("深", System.Type.GetType("System.String"));

            //显示所选节点的Text属性
            this.txtCurrentNode.Text = e.Node.Text.ToString();

            //如果点击的是第一个节点(根节点)，则需要查询干涉检验的状态
            if (e.Node.Level == 0)
            {
                treeNode = e.Node.Text;

                HidetbControl();

                this.tbpGeometic.Parent = this.tabControl1;
                this.tbpGearCalculate.Parent = null;
                this.tbpDrawing.Parent = null;
                this.tbpGearModeling.Parent = null;
                this.tbpShaft.Parent = null;
                this.tbpGearShaft.Parent = null;

                this.tabControl1.SelectedTab = this.tbpGeometic;

                string strTypeNO = this.treeAssembly.SelectedNode.Text.ToString();

                //根据减速机型号查询干涉检验的内容
                string strXML = @"C:\减速器1\XML\干涉检验状态记录.xml";
                xEle = XElement.Load(strXML);
                string strElement = strTypeNO;
                IEnumerable<XElement> childElements = xEle.Elements(strTypeNO);

                //1. 读取传动比信息
                IEnumerable<XElement> elmBasicInfo = childElements.Elements("基本信息");

                foreach (XElement elmBasic in elmBasicInfo)
                {
                    this.tbxTransmissionSeries.Text = elmBasic.Element("传动级数").Value.ToString();
                    this.tbxTotalTransmissionRatio.Text = elmBasic.Element("总传动比").Value.ToString();
                }

                //2. 读取各级齿轮参数
                dtdgvParameter.Rows.Clear();
                IEnumerable<XElement> elmGearParameter = childElements.Elements("各级齿轮参数");
                foreach (XElement elmParameter in elmGearParameter)
                {
                    DataRow drParameter = dtdgvParameter.NewRow();
                    drParameter["jishu"] = elmParameter.Element("级数").Value.ToString();
                    drParameter["moshu"] = elmParameter.Element("法向模数").Value.ToString();
                    drParameter["zhongxinju"] = elmParameter.Element("中心距").Value.ToString();
                    drParameter["chishu1"] = elmParameter.Element("齿数1").Value.ToString();
                    drParameter["chishu2"] = elmParameter.Element("齿数2").Value.ToString();
                    drParameter["luoxuanjiao"] = elmParameter.Element("螺旋角").Value.ToString();
                    drParameter["yalijiao"] = elmParameter.Element("法向压力角").Value.ToString();
                    drParameter["bianweixishu1"] = elmParameter.Element("法向变位系数1").Value.ToString();
                    drParameter["bianweixishu2"] = elmParameter.Element("法向变位系数2").Value.ToString();
                    drParameter["chidinggaoxishu1"] = elmParameter.Element("齿顶高系数1").Value.ToString();
                    drParameter["chidinggaoxishu2"] = elmParameter.Element("齿顶高系数2").Value.ToString();
                    drParameter["dingxixishu1"] = elmParameter.Element("顶隙系数1").Value.ToString();
                    drParameter["dingxixishu2"] = elmParameter.Element("顶隙系数2").Value.ToString();
                    drParameter["chikuan"] = elmParameter.Element("齿宽").Value.ToString();

                    dtdgvParameter.Rows.Add(drParameter);
                    dtdgvParameter.AcceptChanges();
                }
                this.dgvParameter.DataSource = dtdgvParameter;
                this.dgvParameter.Update();

                //3. 读取尺寸计算结果

                dtdgvDiameter.Rows.Clear();
                IEnumerable<XElement> elmGearDiameter = childElements.Elements("尺寸计算");
                foreach (XElement elmDiameter in elmGearDiameter)
                {
                    DataRow drDiameter = dtdgvDiameter.NewRow();
                    drDiameter["jishu"] = elmDiameter.Element("级数").Value.ToString();
                    drDiameter["fenduyuan1"] = elmDiameter.Element("分度圆直径1").Value.ToString();
                    drDiameter["fenduyuan2"] = elmDiameter.Element("分度圆直径2").Value.ToString();
                    drDiameter["chidingyuan1"] = elmDiameter.Element("齿顶圆直径1").Value.ToString();
                    drDiameter["chidingyuan2"] = elmDiameter.Element("齿顶圆直径2").Value.ToString();
                    drDiameter["chigenyuan1"] = elmDiameter.Element("齿根圆直径1").Value.ToString();
                    drDiameter["chigenyuan2"] = elmDiameter.Element("齿根圆直径2").Value.ToString();

                    dtdgvDiameter.Rows.Add(drDiameter);
                    dtdgvDiameter.AcceptChanges();

                }
                this.dgvDiameter.DataSource = dtdgvDiameter;
                this.dgvDiameter.Update();

            }

            if (e.Node.Level == 1)
            {
                treeNode = e.Node.Text;


                /*string firstlevel = e.Node.Text;
                string toplevel = e.Node.Parent.Text;
                XElement aa = xEle.Element(toplevel);
                string fileName = aa.Element(firstlevel).Attribute("文件").Value;
                XElement tempEle = XElement.Load(fileName);
                XElement childElmb = tempEle.Element(toplevel).Element(firstlevel);
                text_mn2.Text = childElmb.Element("法向模数").Value;
                //   IEnumerable<XElement> elmGearDiameter = from element in xEle.Element() xe.Elements("People")
                //                               select element;  
                //                                            select .Elements(e.Node.Text);
                // MessageBox.Show(e.Node.Text);*/

                currentLevel = e.Node.Text;
                fatherLevel = e.Node.Parent.Text;
                XElement fatherElement = xEle.Element(fatherLevel);
                strModelFileName = fatherElement.Element(currentLevel).Attribute("文件").Value;

                XElement currentElement = XElement.Load(strModelFileName);
                XElement childElmb = currentElement.Element(fatherLevel).Element(currentLevel);

                IEnumerable<XElement> elements = from element in currentElement.Element(fatherLevel).Elements("各级齿轮参数")
                                                 where element.Element("级数").Value == fatherElement.Element(currentLevel).Attribute("级数").Value
                                                 select element;

                text_mn2.Text = elements.ElementAt(0).Element("法向模数").Value;
                text_an2.Text = elements.ElementAt(0).Element("法向压力角").Value;
                text_lb12.Text = elements.ElementAt(0).Element("螺旋角").Value;
                txtPinionZ.Text = elements.ElementAt(0).Element("齿数1").Value;
                text_zd.Text = elements.ElementAt(0).Element("齿数2").Value;
                text_xnx.Text = elements.ElementAt(0).Element("法向变位系数1").Value;
                text_xnd.Text = elements.ElementAt(0).Element("法向变位系数2").Value;
                text_hax1.Text = elements.ElementAt(0).Element("齿顶高系数1").Value;
                text_had1.Text = elements.ElementAt(0).Element("齿顶高系数2").Value;
                text_cax.Text = elements.ElementAt(0).Element("顶隙系数1").Value;
                text_cad.Text = elements.ElementAt(0).Element("顶隙系数2").Value;
                text_bx.Text = elements.ElementAt(0).Element("齿宽").Value;
                text_bd.Text = elements.ElementAt(0).Element("齿宽").Value;

                gearCalculateButton_Click(sender, e);

                HidetbControl();
                this.tbpGeometic.Parent = null;
                this.tbpGearCalculate.Parent = this.tabControl1;
                this.tbpDrawing.Parent = null;
                this.tbpGearModeling.Parent = null;
                this.tbpShaft.Parent = null;
                this.tbpGearShaft.Parent = null;

                this.tabControl1.SelectedTab = this.tbpGearCalculate;

            }

            if (e.Node.Level == 2)
            {

                treeNode = e.Node.Text;

                string strNode = e.Node.Text.ToString();
                int i = strNode.Length - 2;
                string temp = strNode.Substring(i, 2);
                switch (temp)
                {
                    case "齿轮"://齿轮                      
                        currentLevel = e.Node.Text;
                        fatherLevel = e.Node.Parent.Text;
                        U2Level = e.Node.Parent.Parent.Text;
                        int strLength = currentLevel.Length;
                        btnToleranceQuery.Enabled = true;
                        XElement U2Element = xEle.Element(U2Level);
                        XElement fatherLevelElement = U2Element.Element(fatherLevel);
                        strModelFileName = fatherLevelElement.Element(currentLevel.Substring(strLength - 2, 2)).Attribute("文件").Value;
                        //xlmGearModelFileName = fileName;
                        XElement currentElement = XElement.Load(strModelFileName);

                        XElement childElmb = currentElement.Element("设计参数");

                        textBox24.Text = childElmb.Element("法向模数").Value;
                        textBox23.Text = childElmb.Element("法向压力角").Value;
                        textBox22.Text = childElmb.Element("螺旋角").Value;
                        textBox15.Text = childElmb.Element("齿数").Value;
                        textBox12.Text = childElmb.Element("齿顶高系数").Value;
                        textBox14.Text = childElmb.Element("法向变位系数").Value;
                        textBox11.Text = childElmb.Element("顶隙系数").Value;
                        textBox13.Text = childElmb.Element("齿宽").Value;


                        if (childElmb.Element("旋向").Value == "L")
                        {
                            comboBox1.Text = "左旋";
                        }
                        else
                        {
                            comboBox1.Text = "右旋";
                        }


                        // 腹板结构
                        childElmb = currentElement.Element("腹板结构");

                        textFBWJ.Text = childElmb.Element("腹板外径D2").Value;
                        texFBNJ.Text = childElmb.Element("腹板内径D1").Value;
                        textFBKJ.Text = childElmb.Element("腹板孔径dk").Value;
                        textFBH.Text = childElmb.Element("腹板深").Value;
                        textFBKS.Text = childElmb.Element("腹板孔数").Value;
                        textFBYJ.Text = childElmb.Element("腹板圆角").Value;
                        textFBDJ.Text = childElmb.Element("腹板倒角").Value;

                        //轮毂结构 
                        childElmb = currentElement.Element("轮毂结构");

                        textLGZJ.Text = childElmb.Element("轮毂直径").Value;
                        textJK.Text = childElmb.Element("键宽").Value;
                        textJS.Text = childElmb.Element("键深").Value;
                        textZXKDJ.Text = childElmb.Element("轮毂倒角").Value;
                        textBox1.Text = childElmb.Element("轮毂圆角").Value;

                        childElmb = currentElement.Element("尺寸计算");
                        textCDDJ.Text = childElmb.Element("齿顶倒角").Value;

                        HidetbControl();

                        this.tbpGeometic.Parent = null;
                        this.tbpGearCalculate.Parent = null;
                        this.tbpGearModeling.Parent = this.tabControl1;
                        this.tbpGearShaft.Parent = null;
                        this.tbpShaft.Parent = null;
                        this.tbpDrawing.Parent = this.tabControl1;

                        this.tabControl1.SelectedTab = this.tbpGearModeling;

                        this.tabControl1.Show();
                        // this.tabControl1.TabPages.Insert(0, new TabPage3());
                        //1,先移除 
                        //TabControl.TabPages.RemoveAt(3);//移除索引为3的TabPage
                        //2,再插入
                        //TabControl.TabPages.Insert(0,new TabPage3());//新建一个与原索引3的TabPage对象,插入到索引0处 
                        break;

                    case "级轴"://光轴

                        currentLevel = e.Node.Text;
                        fatherLevel = e.Node.Parent.Text;
                        U2Level = e.Node.Parent.Parent.Text;
                        strLength = currentLevel.Length;

                        U2Element = xEle.Element(U2Level);
                        fatherLevelElement = U2Element.Element(fatherLevel);
                        strModelFileName = fatherLevelElement.Element(currentLevel.Substring(strLength - 1, 1)).Attribute("文件").Value;
                        //xlmGearShaftModelFileName = fileName;
                        currentElement = XElement.Load(strModelFileName);

                        btnToleranceQuery.Enabled = false;

                        double numberOfLeftShaft = double.Parse(currentElement.Element("设计参数").Element("左轴段数").Value);
                        double numberOfRightShaft = double.Parse(currentElement.Element("设计参数").Element("右轴段数").Value);
                        double numberOfJiancao = double.Parse(currentElement.Element("设计参数").Element("键槽数").Value);

                        string pictureImageFile = currentElement.Element("基本信息").Element("项目目录").Value +
                            currentElement.Element("基本信息").Element("示意图").Value;

                        pictureBox3.Image = Image.FromFile(pictureImageFile);

                        //dgvLeftShaft.Rows.Clear();
                        dgvLeftGearShaft.DataSource = null;
                        dgvLeftGearShaft.Refresh();

                        string zzd;
                        string yzd;

                        textSZJZZJ.Text = currentElement.Element("中间轴段").Element("直径").Value;
                        textSZJZCD.Text = currentElement.Element("中间轴段").Element("长度").Value;
                        //.Text = currentElement.Element("中间轴段").Element("倒角").Value;
                        //.Text = currentElement.Element("中间轴段").Element("圆角").Value;



                        XElement zzdEle = currentElement.Element("左轴段");
                        for (int ii = 1; ii <= numberOfLeftShaft; ii++)
                        {
                            DataRow leftShaftRow = leftShaftParameter.NewRow();
                            zzd = "左轴段" + ii.ToString();
                            IEnumerable<XElement> elements = from 直径 in zzdEle.Elements(zzd)
                                                             select 直径;
                            leftShaftRow[0] = elements.ElementAt(0).Element("轴段").Value;
                            leftShaftRow[1] = elements.ElementAt(0).Element("直径").Value;
                            leftShaftRow[2] = elements.ElementAt(0).Element("长度").Value;
                            leftShaftRow[3] = elements.ElementAt(0).Element("倒角").Value;
                            leftShaftRow[4] = elements.ElementAt(0).Element("圆角").Value;
                            leftShaftParameter.Rows.Add(leftShaftRow);
                            leftShaftParameter.AcceptChanges();
                        }

                        this.dgvLeftShaft.DataSource = leftShaftParameter;
                        this.dgvLeftShaft.Update();


                        XElement yzdEle = currentElement.Element("右轴段");
                        for (int ii = 1; ii <= numberOfRightShaft; ii++)
                        {
                            DataRow rightShaftRow = rightShaftParameter.NewRow();
                            yzd = "右轴段" + ii.ToString();
                            IEnumerable<XElement> elements = from 直径 in yzdEle.Elements(yzd)
                                                             select 直径;
                            rightShaftRow[0] = elements.ElementAt(0).Element("轴段").Value;
                            rightShaftRow[1] = elements.ElementAt(0).Element("直径").Value;
                            rightShaftRow[2] = elements.ElementAt(0).Element("长度").Value;
                            rightShaftRow[3] = elements.ElementAt(0).Element("倒角").Value;
                            rightShaftRow[4] = elements.ElementAt(0).Element("圆角").Value;
                            rightShaftParameter.Rows.Add(rightShaftRow);
                            rightShaftParameter.AcceptChanges();
                        }

                        this.dgvRightShaft.DataSource = rightShaftParameter;
                        this.dgvRightShaft.Update();



                        XElement jianEle = currentElement.Element("键槽");
                        for (int ii = 1; ii <= numberOfJiancao; ii++)
                        {
                            DataRow drJiancao = keyParameter.NewRow();
                            string jiancao = "键槽" + ii.ToString();
                            IEnumerable<XElement> elements = from 长度 in jianEle.Elements(jiancao)
                                                             select 长度;
                            drJiancao[0] = ii.ToString();
                            drJiancao[1] = elements.ElementAt(0).Element("位置").Value;
                            drJiancao[2] = elements.ElementAt(0).Element("长度").Value;
                            drJiancao[3] = elements.ElementAt(0).Element("定位").Value;
                            drJiancao[4] = elements.ElementAt(0).Element("宽").Value;
                            drJiancao[5] = elements.ElementAt(0).Element("深").Value;

                            keyParameter.Rows.Add(drJiancao);
                            keyParameter.AcceptChanges();

                        }

                        this.dgvjiancao1.DataSource = keyParameter;
                        this.dgvjiancao1.Update();

                        HidetbControl();

                        this.tbpGeometic.Parent = null;
                        this.tbpGearCalculate.Parent = null;
                        this.tbpGearModeling.Parent = null;
                        this.tbpGearShaft.Parent = null;
                        this.tbpShaft.Parent = this.tabControl1;
                        this.tbpDrawing.Parent = this.tabControl1;

                        this.tabControl1.SelectedTab = this.tbpShaft;

                        this.tabControl1.Show();
                        break;

                    case "轮轴"://齿轮轴
                        currentLevel = e.Node.Text;
                        fatherLevel = e.Node.Parent.Text;
                        U2Level = e.Node.Parent.Parent.Text;
                        strLength = currentLevel.Length;


                        U2Element = xEle.Element(U2Level);
                        fatherLevelElement = U2Element.Element(fatherLevel);
                        strModelFileName = fatherLevelElement.Element(currentLevel.Substring(strLength - 1, 1)).Attribute("文件").Value;
                        //xlmGearShaftModelFileName = fileName;
                        currentElement = XElement.Load(strModelFileName);
                        numberOfLeftShaft = double.Parse(currentElement.Element("设计参数").Element("左轴段数").Value);
                        numberOfRightShaft = double.Parse(currentElement.Element("设计参数").Element("右轴段数").Value);

                        numberOfLeftShaft = double.Parse(currentElement.Element("设计参数").Element("左轴段数").Value);
                        numberOfRightShaft = double.Parse(currentElement.Element("设计参数").Element("右轴段数").Value);
                        numberOfJiancao = double.Parse(currentElement.Element("设计参数").Element("键槽数").Value);

                        pictureImageFile = currentElement.Element("基本信息").Element("项目目录").Value +
                           currentElement.Element("基本信息").Element("示意图").Value;

                        pictureBox2.Image = Image.FromFile(pictureImageFile);
                        btnToleranceQuery.Enabled = true;
                        //dgvLeftShaft.Rows.Clear();
                        dgvLeftGearShaft.DataSource = null;
                        dgvLeftGearShaft.Refresh();

                        leftShaftParameter = new DataTable();
                        leftShaftParameter.Columns.Add("轴段", System.Type.GetType("System.String"));
                        leftShaftParameter.Columns.Add("直径", System.Type.GetType("System.String"));
                        leftShaftParameter.Columns.Add("长度", System.Type.GetType("System.String"));
                        leftShaftParameter.Columns.Add("倒角", System.Type.GetType("System.String"));
                        leftShaftParameter.Columns.Add("圆角", System.Type.GetType("System.String"));

                        rightShaftParameter = new DataTable();
                        rightShaftParameter.Columns.Add("轴段", System.Type.GetType("System.String"));
                        rightShaftParameter.Columns.Add("直径", System.Type.GetType("System.String"));
                        rightShaftParameter.Columns.Add("长度", System.Type.GetType("System.String"));
                        rightShaftParameter.Columns.Add("倒角", System.Type.GetType("System.String"));
                        rightShaftParameter.Columns.Add("圆角", System.Type.GetType("System.String"));

                        zzdEle = currentElement.Element("左轴段");
                        for (int ii = 1; ii <= numberOfLeftShaft; ii++)
                        {
                            DataRow leftShaftRow = leftShaftParameter.NewRow();
                            zzd = "左轴段" + ii.ToString();
                            IEnumerable<XElement> elements = from 直径 in zzdEle.Elements(zzd)
                                                             select 直径;
                            leftShaftRow[0] = elements.ElementAt(0).Element("轴段").Value;
                            leftShaftRow[1] = elements.ElementAt(0).Element("直径").Value;
                            leftShaftRow[2] = elements.ElementAt(0).Element("长度").Value;
                            leftShaftRow[3] = elements.ElementAt(0).Element("倒角").Value;
                            leftShaftRow[4] = elements.ElementAt(0).Element("圆角").Value;
                            leftShaftParameter.Rows.Add(leftShaftRow);
                            leftShaftParameter.AcceptChanges();
                        }

                        this.dgvLeftGearShaft.DataSource = leftShaftParameter;
                        this.dgvLeftGearShaft.Update();

                        yzdEle = currentElement.Element("右轴段");
                        for (int ii = 1; ii <= numberOfRightShaft; ii++)
                        {
                            DataRow rightShaftRow = rightShaftParameter.NewRow();
                            yzd = "右轴段" + ii.ToString();
                            IEnumerable<XElement> elements = from 直径 in yzdEle.Elements(yzd)
                                                             select 直径;
                            rightShaftRow[0] = elements.ElementAt(0).Element("轴段").Value;
                            rightShaftRow[1] = elements.ElementAt(0).Element("直径").Value;
                            rightShaftRow[2] = elements.ElementAt(0).Element("长度").Value;
                            rightShaftRow[3] = elements.ElementAt(0).Element("倒角").Value;
                            rightShaftRow[4] = elements.ElementAt(0).Element("圆角").Value;
                            rightShaftParameter.Rows.Add(rightShaftRow);
                            rightShaftParameter.AcceptChanges();
                        }

                        this.dgvRightGearShaft.DataSource = rightShaftParameter;
                        this.dgvRightGearShaft.Update();


                        childElmb = currentElement.Element("设计参数");

                        textBox6.Text = childElmb.Element("法向模数").Value;
                        textBox5.Text = childElmb.Element("法向压力角").Value;
                        textBox4.Text = childElmb.Element("螺旋角").Value;
                        textBox17.Text = childElmb.Element("齿数").Value;
                        textBox8.Text = childElmb.Element("齿顶高系数").Value;
                        textBox16.Text = childElmb.Element("法向变位系数").Value;
                        textBox10.Text = childElmb.Element("顶隙系数").Value;
                        textBox9.Text = childElmb.Element("齿宽").Value;


                        if (childElmb.Element("旋向").Value == "L")
                        {
                            comboBox1.Text = "左旋";
                        }
                        else
                        {
                            comboBox1.Text = "右旋";
                        }

                        textBox7.Text = currentElement.Element("尺寸计算").Element("齿顶倒角").Value;

                        ////////////////////////

                        jianEle = currentElement.Element("键槽");
                        for (int ii = 1; ii <= numberOfJiancao; ii++)
                        {
                            DataRow drJiancao = keyParameter.NewRow();
                            string jiancao = "键槽" + ii.ToString();
                            IEnumerable<XElement> elements = from 长度 in jianEle.Elements(jiancao)
                                                             select 长度;
                            drJiancao[0] = ii.ToString();
                            drJiancao[1] = elements.ElementAt(0).Element("位置").Value;
                            drJiancao[2] = elements.ElementAt(0).Element("长度").Value;
                            drJiancao[3] = elements.ElementAt(0).Element("定位").Value;
                            drJiancao[4] = elements.ElementAt(0).Element("宽").Value;
                            drJiancao[5] = elements.ElementAt(0).Element("深").Value;

                            keyParameter.Rows.Add(drJiancao);
                            keyParameter.AcceptChanges();

                        }

                        this.dgvjiancao.DataSource = keyParameter;
                        this.dgvjiancao.Update();

                        HidetbControl();

                        this.tbpGeometic.Parent = null;
                        this.tbpGearCalculate.Parent = null;
                        this.tbpGearModeling.Parent = null;
                        this.tbpGearShaft.Parent = this.tabControl1;
                        this.tbpShaft.Parent = null;
                        this.tbpDrawing.Parent = this.tabControl1;

                        this.tabControl1.SelectedTab = this.tbpGearShaft;

                        this.tabControl1.Show();
                        break;
                }
            }
        }

        private void HidetbControl()
        {
            this.tbpGeometic.Parent = null;
            this.tbpGearCalculate.Parent = null;
            this.tbpGearModeling.Parent = null;
            this.tbpGearShaft.Parent = null;
            this.tbpShaft.Parent = null;
            this.tbpDrawing.Parent = null;
        }


        #endregion


        #region 齿轮几何计算

        //修改齿轮参数
        private void btnModify_Click(object sender, EventArgs e)
        {
            strModelFileName = @"C:\减速器1\XML\干涉检验状态记录.xml";
            XElement xe = XElement.Load(strModelFileName);

            if (this.dgvParameter.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的齿轮参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvParameter.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要修改的齿轮参数！", "提示", MessageBoxButtons.OK);
            }


            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改齿轮参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {

                frmModifyGearParameters a = new frmModifyGearParameters();

                //将dgv中的值传到窗口中
                a.strjishu = this.dgvParameter.SelectedRows[0].Cells["clmjishu"].Value.ToString();
                a.strmoshu = this.dgvParameter.SelectedRows[0].Cells["clmmoshu"].Value.ToString();
                a.strzhongxinju = this.dgvParameter.SelectedRows[0].Cells["clmzhongxinju"].Value.ToString();
                a.strchishu1 = this.dgvParameter.SelectedRows[0].Cells["clmchishuz1"].Value.ToString();
                a.strchishu2 = this.dgvParameter.SelectedRows[0].Cells["clmchishuz2"].Value.ToString();
                a.strluoxuanjiao = this.dgvParameter.SelectedRows[0].Cells["clmluoxuanjiao"].Value.ToString();
                a.stryalijiao = this.dgvParameter.SelectedRows[0].Cells["clmyalijiao"].Value.ToString();
                a.strbianweixishu1 = this.dgvParameter.SelectedRows[0].Cells["clmbianweixishu1"].Value.ToString();
                a.strbianweixishu2 = this.dgvParameter.SelectedRows[0].Cells["clmbianweixishu2"].Value.ToString();
                a.strchidinggaoxishu1 = this.dgvParameter.SelectedRows[0].Cells["clmchidinggaoxishu1"].Value.ToString();
                a.strchidinggaoxishu2 = this.dgvParameter.SelectedRows[0].Cells["clmchidinggaoxishu2"].Value.ToString();
                a.strdingxixishu1 = this.dgvParameter.SelectedRows[0].Cells["clmdingxixishu1"].Value.ToString();
                a.strdingxixishu2 = this.dgvParameter.SelectedRows[0].Cells["clmdingxixishu2"].Value.ToString();
                a.strchikuan = this.dgvParameter.SelectedRows[0].Cells["clmchikuan"].Value.ToString();


                if (a.ShowDialog() == DialogResult.OK)
                {
                    //更新dgvParameter中的值
                    this.dgvParameter.SelectedRows[0].Cells["clmjishu"].Value = a.strjishu;
                    this.dgvParameter.SelectedRows[0].Cells["clmmoshu"].Value = a.strmoshu;
                    this.dgvParameter.SelectedRows[0].Cells["clmzhongxinju"].Value = a.strzhongxinju;
                    this.dgvParameter.SelectedRows[0].Cells["clmchishuz1"].Value = a.strchishu1;
                    this.dgvParameter.SelectedRows[0].Cells["clmchishuz2"].Value = a.strchishu2;
                    this.dgvParameter.SelectedRows[0].Cells["clmluoxuanjiao"].Value = a.strluoxuanjiao;
                    this.dgvParameter.SelectedRows[0].Cells["clmyalijiao"].Value = a.stryalijiao;
                    this.dgvParameter.SelectedRows[0].Cells["clmbianweixishu1"].Value = a.strbianweixishu1;
                    this.dgvParameter.SelectedRows[0].Cells["clmbianweixishu2"].Value = a.strbianweixishu2;
                    this.dgvParameter.SelectedRows[0].Cells["clmchidinggaoxishu1"].Value = a.strchidinggaoxishu1;
                    this.dgvParameter.SelectedRows[0].Cells["clmchidinggaoxishu2"].Value = a.strchidinggaoxishu2;
                    this.dgvParameter.SelectedRows[0].Cells["clmdingxixishu1"].Value = a.strdingxixishu1;
                    this.dgvParameter.SelectedRows[0].Cells["clmdingxixishu2"].Value = a.strdingxixishu2;
                    this.dgvParameter.SelectedRows[0].Cells["clmchikuan"].Value = a.strchikuan;

                    //修改参数后重新计算与画图
                    CalculateDiameter();

                    //重新绘制齿顶圆
                    //DrawChidingyuan();
                    btnDraw_Click(sender, e);
                    //将修改后的信息存入XML
                    string strjishu = this.dgvParameter.SelectedRows[0].Cells["clmjishu"].Value.ToString();

                    if (strjishu != "")
                    {
                        string strTypeNO = this.cmbType.Text.ToString();
                        string strElement = strTypeNO;
                        IEnumerable<XElement> childElements = xe.Elements(strTypeNO);

                        IEnumerable<XElement> elements = from element in childElements.Elements("各级齿轮参数")
                                                         where element.Element("级数").Value == strjishu
                                                         select element;
                        if (elements.Count() > 0)
                        {
                            XElement newXE = elements.First();
                            //newXE.SetAttributeValue("级数", strjishu);
                            newXE.ReplaceNodes(
                              new XElement("级数", strjishu),
                              new XElement("法向模数", this.dgvParameter.SelectedRows[0].Cells["clmmoshu"].Value.ToString()),
                              new XElement("中心距", this.dgvParameter.SelectedRows[0].Cells["clmzhongxinju"].Value.ToString()),
                              new XElement("齿数1", this.dgvParameter.SelectedRows[0].Cells["clmchishuz1"].Value.ToString()),
                              new XElement("齿数2", this.dgvParameter.SelectedRows[0].Cells["clmchishuz2"].Value.ToString()),
                              new XElement("螺旋角", this.dgvParameter.SelectedRows[0].Cells["clmluoxuanjiao"].Value.ToString()),
                              new XElement("法向压力角", this.dgvParameter.SelectedRows[0].Cells["clmyalijiao"].Value.ToString()),
                              new XElement("法向变位系数1", this.dgvParameter.SelectedRows[0].Cells["clmbianweixishu1"].Value.ToString()),
                              new XElement("法向变位系数2", this.dgvParameter.SelectedRows[0].Cells["clmbianweixishu2"].Value.ToString()),
                              new XElement("齿顶高系数1", this.dgvParameter.SelectedRows[0].Cells["clmchidinggaoxishu1"].Value.ToString()),
                              new XElement("齿顶高系数2", this.dgvParameter.SelectedRows[0].Cells["clmchidinggaoxishu2"].Value.ToString()),
                              new XElement("顶隙系数1", this.dgvParameter.SelectedRows[0].Cells["clmdingxixishu1"].Value.ToString()),
                              new XElement("顶隙系数2", this.dgvParameter.SelectedRows[0].Cells["clmdingxixishu2"].Value.ToString()),
                              new XElement("齿宽", this.dgvParameter.SelectedRows[0].Cells["clmchikuan"].Value.ToString())
                              );
                        }
                        xe.Save(strModelFileName);
                    }
                }
            }
            else
                return;
        }


        //计算齿轮几何尺寸
        private void CalculateDiameter()
        {

        }

        //绘制齿顶圆
        private void DrawChidingyuan()
        {

        }

        #endregion



        private void btnLoad_Click(object sender, EventArgs e)
        {
            string strXML = @"C:\减速器1\XML\干涉检验状态记录.xml";

            XElement xe = XElement.Load(strXML);

            IEnumerable<XElement> gearParaElement = xe.Element(treeNode).Elements("各级齿轮参数");

            IEnumerable<XElement> gearCalElement = xe.Element(treeNode).Elements("尺寸计算");

            foreach (XElement chileElement in gearParaElement)
            {
                string jishu = chileElement.Element("级数").Value; //级数
                string moshu = chileElement.Element("法向模数").Value; //模数
                string zhongxinju = chileElement.Element("中心距").Value; //中心距
                string chishu1 = chileElement.Element("齿数1").Value; //齿数1
                string chishu2 = chileElement.Element("齿数2").Value; //齿数2
                string luoxuanjiao = chileElement.Element("螺旋角").Value; //螺旋角
                string yalijiao = chileElement.Element("法向压力角").Value; //压力角
                string bianweixishu1 = chileElement.Element("法向变位系数1").Value; //变位系数1
                string bianweixishu2 = chileElement.Element("法向变位系数2").Value; //变位系数2
                string chidinggaoxishu1 = chileElement.Element("齿顶高系数1").Value; //齿顶高系数1
                string chidinggaoxishu2 = chileElement.Element("齿顶高系数1").Value; //齿顶高系数2
                string dingxixishu1 = chileElement.Element("顶隙系数1").Value; //顶隙系数1
                string dingxixishu2 = chileElement.Element("顶隙系数1").Value; //顶隙系数1
                string chikuan = chileElement.Element("齿宽").Value; //齿宽

                double[] CalResult = new double[52];

                CalResult = WGeoCal(double.Parse(chishu1), double.Parse(moshu), double.Parse(luoxuanjiao),
                     double.Parse(chishu2), double.Parse(chidinggaoxishu1), double.Parse(chidinggaoxishu2),
                      double.Parse(bianweixishu1), double.Parse(bianweixishu2), double.Parse(dingxixishu1),
                       double.Parse(dingxixishu2), double.Parse(yalijiao), 24, 24, 100);

                double dx = CalResult[8];// = dx;                          //求小齿轮分度圆直径
                double dd = CalResult[9];// = dx;                          //求大齿轮分度圆直径
                double dax = CalResult[13];// = dax;                       //求小齿轮齿顶圆直径              
                double dad = CalResult[14];// = dad;                       //求大齿轮齿顶圆直径
                double dfx = CalResult[15];                       //求小齿轮齿根圆直径
                double dfd = CalResult[16];                                         //求大齿轮齿根圆直径

                IEnumerable<XElement> elements = from element in gearCalElement
                                                 where element.Element("级数").Value == jishu
                                                 select element;
                XElement newXE = elements.First();
                newXE.ReplaceNodes(
                          new XElement("级数", jishu),
                          new XElement("分度圆直径1", dx.ToString("#.###")),
                          new XElement("分度圆直径2", dd.ToString("#.###")),
                          new XElement("齿顶圆直径1", dax.ToString("#.###")),
                          new XElement("齿顶圆直径2", dad.ToString("#.###")),
                          new XElement("齿根圆直径1", dfx.ToString("#.###")),
                          new XElement("齿根圆直径2", dfd.ToString("#.###"))
                          );
                xe.Save(strXML);

            }

            dtdgvDiameter.Rows.Clear();
            IEnumerable<XElement> elmGearDiameter = xe.Element(treeNode).Elements("尺寸计算");
            foreach (XElement elmDiameter in elmGearDiameter)
            {
                DataRow drDiameter = dtdgvDiameter.NewRow();
                drDiameter["jishu"] = elmDiameter.Element("级数").Value.ToString();
                drDiameter["fenduyuan1"] = elmDiameter.Element("分度圆直径1").Value.ToString();
                drDiameter["fenduyuan2"] = elmDiameter.Element("分度圆直径2").Value.ToString();
                drDiameter["chidingyuan1"] = elmDiameter.Element("齿顶圆直径1").Value.ToString();
                drDiameter["chidingyuan2"] = elmDiameter.Element("齿顶圆直径2").Value.ToString();
                drDiameter["chigenyuan1"] = elmDiameter.Element("齿根圆直径1").Value.ToString();
                drDiameter["chigenyuan2"] = elmDiameter.Element("齿根圆直径2").Value.ToString();

                dtdgvDiameter.Rows.Add(drDiameter);
                dtdgvDiameter.AcceptChanges();

            }
            this.dgvDiameter.DataSource = dtdgvDiameter;
            this.dgvDiameter.Update();
            //  xe.Save(strXML);
            // Form1_Load(sender, e);
            //this.dgvDiameter.Update();
        }

        #region 齿轮齿顶圆绘图

        private void btnDraw_Click(object sender, EventArgs e)
        {
            GraphControl.GraphPane.CurveList.Clear();
            PointPairList list1 = new PointPairList();

            list1 = circleCalculate(0, 0, double.Parse(dgvDiameter[3, 0].Value.ToString().Trim()) * 0.5);
            LineItem myCurve = GraphControl.GraphPane.AddCurve("第一级输入齿轮", list1, Color.Blue, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 
            //Make the curve smooth
            myCurve.Line.IsSmooth = true;

            list1 = circleCalculate(double.Parse(dgvParameter[2, 0].Value.ToString().Trim()), 0, double.Parse(dgvDiameter[4, 0].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第一级输出齿轮", list1, Color.Blue, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dgvParameter[2, 0].Value.ToString().Trim()), 0, double.Parse(dgvDiameter[3, 1].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第二级输入齿轮", list1, Color.Red, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dgvParameter[2, 0].Value.ToString().Trim()) + double.Parse(dgvParameter[2, 1].Value.ToString().Trim()), 0,
                double.Parse(dgvDiameter[4, 1].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第二级输出齿轮", list1, Color.Red, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dgvParameter[2, 0].Value.ToString().Trim()) + double.Parse(dgvParameter[2, 1].Value.ToString().Trim()), 0,
                double.Parse(dgvDiameter[3, 2].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第三级输入齿轮", list1, Color.Green, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            list1 = circleCalculate(double.Parse(dgvParameter[2, 0].Value.ToString().Trim()) + double.Parse(dgvParameter[2, 1].Value.ToString().Trim()) +
                double.Parse(dgvParameter[2, 2].Value.ToString().Trim()), 0,
                double.Parse(dgvDiameter[4, 2].Value.ToString().Trim()) * 0.5);
            myCurve = GraphControl.GraphPane.AddCurve("第三级输出齿轮", list1, Color.Green, SymbolType.None);
            myCurve.Line.Width = 2.0F; //设置线宽 

            GraphControl.AxisChange();
            graphPane_AxisChangeEvent(GraphControl.GraphPane);
            GraphControl.Update();
            GraphControl.Refresh();


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



        #endregion


        #region 齿轮几何参数计算
        private void gearCalculateButton_Click(object sender, EventArgs e)
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
        #endregion

        #region 齿轮建模
        IModelDoc2 modDoc;

        private void btnGearModelBuild_Click(object sender, EventArgs e)
        {

            //类变量，调研前要赋值
            //xlmFileName = @"C:\圆柱齿轮.xml";//齿轮的XML文件名字

            XElement xe = XElement.Load(strModelFileName);

            //读取齿轮模型文件名字
            IEnumerable<XElement> elements = from 齿轮名称 in xe.Elements("基本信息")// where PInfo.Attribute("ID").Value == strID
                                             select 齿轮名称;
            //MessageBox.Show(elements.ElementAt(0).Element("齿轮名称").Value);
            string modelFile = elements.ElementAt(0).Element("项目目录").Value + elements.ElementAt(0).Element("齿轮名称").Value;
            //利用齿轮模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(modelFile,
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

            Parameter[] gearparameter = new Parameter[34];

            gearparameter[0].name = "基圆@草图1"; gearparameter[0].value = d_b;
            gearparameter[1].name = "U@草图1"; gearparameter[1].value = u * Math.PI / 180;
            gearparameter[2].name = "分度圆@草图1"; gearparameter[2].value = d;
            gearparameter[3].name = "齿弦厚@草图1"; gearparameter[3].value = s;
            gearparameter[4].name = "齿根圆@草图5"; gearparameter[4].value = d_f;
            gearparameter[5].name = "D3@螺旋线"; gearparameter[5].value = B + 0.001;
            gearparameter[6].name = "D1@齿根圆"; gearparameter[6].value = B + 0.001;
            gearparameter[7].name = "B@草图6"; gearparameter[7].value = B;
            gearparameter[8].name = "D1@齿廓阵列"; gearparameter[8].value = Z;
            gearparameter[9].name = "D1@草图6"; gearparameter[9].value = d_a + 0.002;
            gearparameter[10].name = "D4@螺旋线"; gearparameter[10].value = luoju;
            gearparameter[11].name = "D1@齿顶倒角"; gearparameter[11].value = C_CD;
            gearparameter[12].name = "腹板外径@草图7"; gearparameter[12].value = D_2;
            gearparameter[13].name = "腹板内径@草图7"; gearparameter[13].value = D_1;
            gearparameter[14].name = "D1@草图7"; gearparameter[14].value = (D_1 + D_2) / 2;
            gearparameter[15].name = "D2@草图7"; gearparameter[15].value = (180 / N_FB) * Math.PI / 180;
            gearparameter[16].name = "D1@腹板"; gearparameter[16].value = D_FB;
            gearparameter[17].name = "D1@腹板圆角"; gearparameter[17].value = R_FB;
            gearparameter[18].name = "D1@腹板倒角"; gearparameter[18].value = C_FB;
            gearparameter[19].name = "D1@腹板孔阵列"; gearparameter[19].value = N_FB;
            gearparameter[20].name = "D2@草图6"; gearparameter[20].value = B + 1;
            gearparameter[21].name = "D3@草图7"; gearparameter[21].value = KJ_FB;
            gearparameter[22].name = "D1@齿顶倒角阵列"; gearparameter[22].value = Z;
            gearparameter[23].name = "D1@草图8"; gearparameter[23].value = D_LG;
            gearparameter[24].name = "D2@草图8"; gearparameter[24].value = J_K;
            gearparameter[25].name = "D3@草图8"; gearparameter[25].value = J_S;
            gearparameter[26].name = "D1@轮毂倒角"; gearparameter[26].value = C_LG;
            gearparameter[27].name = "D1@轮毂圆角"; gearparameter[27].value = R_LG;
            gearparameter[28].name = "D1@草图9"; gearparameter[28].value = d_f / 2 - 0.001;
            gearparameter[29].name = "D2@草图9"; gearparameter[29].value = d_a / 2;
            gearparameter[30].name = "D3@草图9"; gearparameter[30].value = B;
            gearparameter[31].name = "D1@草图10"; gearparameter[31].value = d_f;
            gearparameter[32].name = "D3@阵列1"; gearparameter[32].value = (180 / N_FB) * Math.PI / 180;
            gearparameter[33].name = "D1@齿顶倒角1"; gearparameter[33].value = C_CD;
            for (int i = 0; i < gearparameter.Length; i++)
            {
                myDimension = (IDimension)modDoc.Parameter(gearparameter[i].name);
                myDimension.SetSystemValue3(gearparameter[i].value, (int)swInConfigurationOpts_e.swAllConfiguration, null);
            }
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
            modDoc.ViewZoomtofit2();
            modDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, (int)swFileSaveError_e.swGenericSaveError, (int)swFileSaveWarning_e.swFileSaveWarning_RebuildError);
        }

        #endregion

        #region 齿轮轴建模

        private void btnGearShaftModelBuilding_Click(object sender, EventArgs e)
        {
            //类变量，调研前要赋值
            // xlmGearShaftModelFileName = @"C:\齿轮轴.xml";//齿轮的XML文件名字

            XElement xe = XElement.Load(strModelFileName);

            //读取齿轮模型文件名字
            IEnumerable<XElement> elements = from 齿轮轴名称 in xe.Elements("基本信息")// where PInfo.Attribute("ID").Value == strID
                                             select 齿轮轴名称;
            string modelFile = elements.ElementAt(0).Element("项目目录").Value + elements.ElementAt(0).Element("齿轮轴名称").Value;
            //利用齿轮模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(modelFile,
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

            Parameter[] gearparameter = new Parameter[16];
            gearparameter[0].name = "基圆@草图1"; gearparameter[0].value = d_b;
            gearparameter[1].name = "U@草图1"; gearparameter[1].value = u * Math.PI / 180;
            gearparameter[2].name = "分度圆@草图1"; gearparameter[2].value = d;
            gearparameter[3].name = "齿弦厚@草图1"; gearparameter[3].value = s;
            gearparameter[4].name = "齿根圆@草图5"; gearparameter[4].value = d_f;
            gearparameter[5].name = "D3@螺旋线"; gearparameter[5].value = B + 0.001;
            gearparameter[6].name = "D1@齿根圆"; gearparameter[6].value = B + 0.001;
            gearparameter[7].name = "B@草图6"; gearparameter[7].value = B;
            gearparameter[8].name = "D1@齿廓阵列"; gearparameter[8].value = Z;
            gearparameter[9].name = "D1@草图6"; gearparameter[9].value = d_a + 0.002;
            gearparameter[10].name = "D2@草图6"; gearparameter[10].value = B + 1;
            gearparameter[11].name = "D4@螺旋线"; gearparameter[11].value = luoju;
            gearparameter[12].name = "D1@齿顶倒角"; gearparameter[12].value = C_CD;
            gearparameter[13].name = "D1@齿顶倒角阵列"; gearparameter[13].value = Z;
            gearparameter[14].name = "D1@草图7"; gearparameter[14].value = d_a;
            gearparameter[15].name = "D1@齿顶倒角1"; gearparameter[15].value = C_CD;
            for (int i = 0; i < gearparameter.Length; i++)
            {
                myDimension = (IDimension)modDoc.Parameter(gearparameter[i].name);
                myDimension.SystemValue = gearparameter[i].value;
            }



            string zzd;
            XElement zzdEle = xe.Element("左轴段");
            for (int i = 1; i <= numberOfLeftShaft; i++)
            {
                zzd = "左轴段" + i.ToString();
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
            string keySeat;
            for (int i = 1; i <= numberOfKeySeat; i++)
            {
                keySeat = "键槽" + i.ToString();
                elements = from 直径 in keySeatEle.Elements(keySeat)// where PInfo.Attribute("ID").Value == strID
                           select 直径;
                double K = double.Parse(elements.ElementAt(0).Element("宽").Value) / 1000;
                double L = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double C = double.Parse(elements.ElementAt(0).Element("定位").Value) / 1000;
                double S = double.Parse(elements.ElementAt(0).Element("深").Value) / 1000;
                string weizhi = elements.ElementAt(0).Element("位置").Value;
                if (weizhi.Substring(0, 1).Equals("左"))
                {
                    elements = from 直径 in zzdEle.Elements(weizhi)// where PInfo.Attribute("ID").Value == strID
                               select 直径;
                }
                else
                {
                    elements = from 直径 in yzdEle.Elements(weizhi)// where PInfo.Attribute("ID").Value == strID
                               select 直径;
                }
                double D = double.Parse(elements.ElementAt(0).Element("直径").Value) / 1000;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + keySeat);
                myDimension.SystemValue = 0.5 * D - S;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + keySeat + "草图");
                myDimension.SystemValue = K;
                myDimension = (IDimension)modDoc.Parameter(@"D2@" + keySeat + "草图");
                myDimension.SystemValue = L;
                myDimension = (IDimension)modDoc.Parameter(@"D3@" + keySeat + "草图");
                myDimension.SystemValue = C;
            }
            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
            modDoc.ViewZoomtofit2();
            modDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                   (int)swFileSaveError_e.swGenericSaveError,
                (int)swFileSaveWarning_e.swFileSaveWarning_RebuildError);
        }
        #endregion


        #region 工程图公差查询
        private void btnToleranceQuery_Click(object sender, EventArgs e)
        {
            XElement currentElement = XElement.Load(strModelFileName);
            MessageBox.Show(strModelFileName);
            double d = double.Parse(currentElement.Element("尺寸计算").Element("分度圆直径").Value);//分度圆直径

            // 齿距累积公差
            XElement xe = XElement.Load(@"C:\减速器1\公差\齿距累积公差Fp.xml");
            double L = 0.5 * Math.PI * d;
            MessageBox.Show(L.ToString());
            IEnumerable<XElement> elements = from ele in xe.Elements("s")
                                             where ((ele.Element("精度等级").Value == g1.Text)
                                             && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L))
                                             select ele;
            Fp.Text = elements.ElementAt(0).Element("公差").Value;

            /////
            xe = XElement.Load(@"C:\减速器1\公差\齿圈径向跳动公差Fr.xml");
            L = d;
            double m = double.Parse(currentElement.Element("设计参数").Element("法向模数").Value);//法向模数
            //判断模数是否等于1
            if (1.0 == m)
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g1.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (int.Parse(ele.Element("Mmin").Value) == 1))
                           select ele;
                Fr.Text = elements.ElementAt(0).Element("公差").Value;
            }
            else
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g1.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (double.Parse(ele.Element("Mmin").Value) < m) && (double.Parse(ele.Element("Mmax").Value) >= m))
                           select ele;
                Fr.Text = elements.ElementAt(0).Element("公差").Value;
            }


            xe = XElement.Load(@"C:\减速器1\公差\公法线长度变动公差Fw.xml");
            elements = from ele in xe.Elements("s")
                       where ((ele.Element("精度等级").Value == g1.Text)
                       && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L))
                       select ele;
            Fw.Text = elements.ElementAt(0).Element("公差").Value;


            xe = XElement.Load(@"C:\减速器1\公差\齿形公差ff.xml");
            //判断模数是否等于1
            if (1.0 == m)
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g2.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (int.Parse(ele.Element("Mmin").Value) == 1))
                           select ele;
                ff.Text = elements.ElementAt(0).Element("公差").Value;
            }
            else
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g2.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (double.Parse(ele.Element("Mmin").Value) < m) && (double.Parse(ele.Element("Mmax").Value) >= m))
                           select ele;
                ff.Text = elements.ElementAt(0).Element("公差").Value;
            }

            xe = XElement.Load(@"C:\减速器1\公差\齿距极限偏差fpt.xml");
            //判断模数是否等于1
            if (1.0 == m)
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g2.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (int.Parse(ele.Element("Mmin").Value) == 1))
                           select ele;
                fpt.Text = elements.ElementAt(0).Element("公差").Value;
            }
            else
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g2.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (double.Parse(ele.Element("Mmin").Value) < m) && (double.Parse(ele.Element("Mmax").Value) >= m))
                           select ele;
                fpt.Text = elements.ElementAt(0).Element("公差").Value;
            }

            xe = XElement.Load(@"C:\减速器1\公差\基节极限偏差fpb.xml");
            //判断模数是否等于1
            if (1.0 == m)
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g2.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (int.Parse(ele.Element("Mmin").Value) == 1))
                           select ele;
                fpb.Text = elements.ElementAt(0).Element("公差").Value;
            }
            else
            {
                elements = from ele in xe.Elements("s")
                           where ((ele.Element("精度等级").Value == g2.Text)
                           && (double.Parse(ele.Element("Lmin").Value) < L) && (double.Parse(ele.Element("Lmax").Value) >= L)
                           && (double.Parse(ele.Element("Mmin").Value) < m) && (double.Parse(ele.Element("Mmax").Value) >= m))
                           select ele;
                fpb.Text = elements.ElementAt(0).Element("公差").Value;
            }
        }

        #endregion

        private void btnUpdateGearParameter_Click(object sender, EventArgs e)
        {
            string gearFile = xEle.Element(U2Level).Element(fatherLevel).Element("齿轮").Attribute("文件").Value;

            XElement gearElement = XElement.Load(gearFile);

            //齿轮参数
            gearElement.Element("腹板结构").Element("腹板外径D2").Value = textFBWJ.Text;
            gearElement.Element("腹板结构").Element("腹板内径D1").Value = texFBNJ.Text;
            gearElement.Element("腹板结构").Element("腹板孔径dk").Value = textFBKJ.Text;
            gearElement.Element("腹板结构").Element("腹板深").Value = textFBH.Text;
            gearElement.Element("腹板结构").Element("腹板孔数").Value = textFBKS.Text;
            gearElement.Element("腹板结构").Element("腹板圆角").Value = textFBYJ.Text;
            gearElement.Element("腹板结构").Element("腹板倒角").Value = textFBDJ.Text;

            gearElement.Element("尺寸计算").Element("齿顶倒角").Value = textCDDJ.Text;

            gearElement.Element("设计参数").Element("齿宽").Value = textBox13.Text;

            gearElement.Element("轮毂结构").Element("轮毂直径").Value = textLGZJ.Text;
            gearElement.Element("轮毂结构").Element("键宽").Value = textJK.Text;
            gearElement.Element("轮毂结构").Element("键深").Value = textJS.Text;
            gearElement.Element("轮毂结构").Element("轮毂倒角").Value = textZXKDJ.Text;
            gearElement.Element("轮毂结构").Element("轮毂圆角").Value = textBox1.Text;

            if (comboBox1.Text == "左旋")
            {
                gearElement.Element("设计参数").Element("旋向").Value = "L";
            }
            else
            {
                gearElement.Element("设计参数").Element("旋向").Value = "R";
            }

            gearElement.Save(gearFile);
        }

        private void btnUpdataGearAndShaftParameter_Click(object sender, EventArgs e)
        {
            string gearFile = xEle.Element(fatherLevel).Element(currentLevel).Element("齿轮").Attribute("文件").Value;
            string shaftFile = xEle.Element(fatherLevel).Element(currentLevel).Element("轴").Attribute("文件").Value;

            int shaftFileLength = shaftFile.Length;
            XElement gearElement = XElement.Load(gearFile);
            XElement shaftElement = XElement.Load(shaftFile);

            //齿轮参数
            gearElement.Element("设计参数").Element("齿数").Value = text_zd.Text;
            gearElement.Element("设计参数").Element("法向模数").Value = text_mn2.Text;
            gearElement.Element("设计参数").Element("齿宽").Value = text_bd.Text;
            gearElement.Element("设计参数").Element("螺旋角").Value = text_lb12.Text;
            gearElement.Element("设计参数").Element("顶隙系数").Value = text_cad.Text;
            gearElement.Element("设计参数").Element("齿顶高系数").Value = text_had1.Text;
            gearElement.Element("设计参数").Element("法向压力角").Value = text_an2.Text;
            gearElement.Element("设计参数").Element("法向变位系数").Value = text_xnd.Text;
            //gearElement.Element("设计参数").Element("旋向").Value = .Text;
            gearElement.Element("设计参数").Element("全齿高").Value = text_hd.Text;


            gearElement.Element("尺寸计算").Element("分度圆直径").Value = text_dd.Text;
            gearElement.Element("尺寸计算").Element("齿顶圆直径").Value = text_dad.Text;
            gearElement.Element("尺寸计算").Element("齿根圆直径").Value = text_dfd.Text;
            gearElement.Element("尺寸计算").Element("基圆直径").Value = text_dbd.Text;
            gearElement.Element("尺寸计算").Element("中心距").Value = text_a2.Text;
            gearElement.Element("尺寸计算").Element("齿弦厚").Value = text_srnd.Text;
            //gearElement.Element("尺寸计算").Element("齿顶倒角").Value = .Text;

            if (shaftFile.Substring(shaftFileLength - 6, 2) == "轮轴")
            {
                //齿轮轴参数
                shaftElement.Element("设计参数").Element("齿数").Value = txtPinionZ.Text;
                shaftElement.Element("设计参数").Element("法向模数").Value = text_mn2.Text;
                shaftElement.Element("设计参数").Element("齿宽").Value = text_bx.Text;
                shaftElement.Element("设计参数").Element("螺旋角").Value = text_lb12.Text;
                shaftElement.Element("设计参数").Element("顶隙系数").Value = text_cax.Text;
                shaftElement.Element("设计参数").Element("齿顶高系数").Value = text_hax1.Text;
                shaftElement.Element("设计参数").Element("法向压力角").Value = text_an2.Text;
                shaftElement.Element("设计参数").Element("法向变位系数").Value = text_xnx.Text;
                //shaftElement.Element("设计参数").Element("旋向").Value = .Text;
                shaftElement.Element("设计参数").Element("全齿高").Value = text_hx.Text;

                shaftElement.Element("尺寸计算").Element("分度圆直径").Value = text_dx.Text;
                shaftElement.Element("尺寸计算").Element("齿顶圆直径").Value = txtPinionTipDiameter.Text;
                shaftElement.Element("尺寸计算").Element("齿根圆直径").Value = text_dfx.Text;
                shaftElement.Element("尺寸计算").Element("基圆直径").Value = txtPinionBaseDiameter.Text;
                shaftElement.Element("尺寸计算").Element("中心距").Value = text_a2.Text;
                shaftElement.Element("尺寸计算").Element("齿弦厚").Value = text_srnx.Text;
                //shaftElement.Element("尺寸计算").Element("齿顶倒角").Value = .Text;
            }

            shaftElement.Save(shaftFile);

            gearElement.Save(gearFile);
        }

        private void btnShaftModelBuilding_Click(object sender, EventArgs e)
        {

            XElement xe = XElement.Load(strModelFileName);

            XElement chileXElement = xe.Element("基本信息");

            string ModelFileName = chileXElement.Element("项目目录").Value + chileXElement.Element("轴名称").Value;


            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(ModelFileName,
                    (int)swDocumentTypes_e.swDocPART,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
                         "",
                    (int)swFileLoadError_e.swGenericError,
                    (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            int numberOfLeftShaft = int.Parse(xe.Element("设计参数").Element("左轴段数").Value);
            int numberOfRightShaft = int.Parse(xe.Element("设计参数").Element("右轴段数").Value);
            int numberOfKeySeat = int.Parse(xe.Element("设计参数").Element("键槽数").Value);

            IDimension myDimension = null;

            double D = double.Parse(xe.Element("中间轴段").Element("直径").Value) / 1000;
            double L = double.Parse(xe.Element("中间轴段").Element("长度").Value) / 1000;
            double C = double.Parse(xe.Element("中间轴段").Element("倒角").Value) / 1000;

            myDimension = (IDimension)modDoc.Parameter("D1@中间轴段草图");
            myDimension.SetSystemValue3(D, (int)swInConfigurationOpts_e.swAllConfiguration, null);
            myDimension = (IDimension)modDoc.Parameter("D1@中间轴段");
            myDimension.SetSystemValue3(L, (int)swInConfigurationOpts_e.swAllConfiguration, null);
            myDimension = (IDimension)modDoc.Parameter("D1@中间轴段倒角");
            myDimension.SetSystemValue3(C, (int)swInConfigurationOpts_e.swAllConfiguration, null);

            string zzd;
            XElement zzdEle = xe.Element("左轴段");
            for (int i = 1; i <= numberOfLeftShaft; i++)
            {
                zzd = "左轴段" + i.ToString();
                IEnumerable<XElement> elements = from 直径 in zzdEle.Elements(zzd)
                                                 select 直径;
                double ZD = double.Parse(elements.ElementAt(0).Element("直径").Value) / 1000;
                double ZL = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double ZC = double.Parse(elements.ElementAt(0).Element("倒角").Value) / 1000;
                double ZR = double.Parse(elements.ElementAt(0).Element("圆角").Value) / 1000;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd + "草图");
                myDimension.SystemValue = ZD;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd);
                myDimension.SystemValue = ZL;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd + "倒角");
                myDimension.SystemValue = ZC;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + zzd + "圆角");
                myDimension.SystemValue = ZR;
            }


            XElement yzdEle = xe.Element("右轴段");
            string yzd;
            for (int i = 1; i <= numberOfRightShaft; i++)
            {
                yzd = "右轴段" + i.ToString();
                IEnumerable<XElement> elements = from 直径 in yzdEle.Elements(yzd)
                                                 select 直径;
                double YD = double.Parse(elements.ElementAt(0).Element("直径").Value) / 1000;
                double YL = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double YC = double.Parse(elements.ElementAt(0).Element("倒角").Value) / 1000;
                double YR = double.Parse(elements.ElementAt(0).Element("圆角").Value) / 1000;

                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd + "草图");
                myDimension.SystemValue = YD;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd);
                myDimension.SystemValue = YL;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd + "倒角");
                myDimension.SystemValue = YC;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + yzd + "圆角");
                myDimension.SystemValue = YR;
            }

            XElement keySeatEle = xe.Element("键槽");
            string keySeat;
            for (int i = 1; i <= numberOfKeySeat; i++)
            {
                keySeat = "键槽" + i.ToString();
                IEnumerable<XElement> elements = from 直径 in keySeatEle.Elements(keySeat)
                                                 select 直径;
                double JK = double.Parse(elements.ElementAt(0).Element("宽").Value) / 1000;
                double JL = double.Parse(elements.ElementAt(0).Element("长度").Value) / 1000;
                double JC = double.Parse(elements.ElementAt(0).Element("定位").Value) / 1000;
                double JS = double.Parse(elements.ElementAt(0).Element("深").Value) / 1000;
                string weizhi = elements.ElementAt(0).Element("位置").Value;

                if (weizhi.Substring(0, 1).Equals("左"))
                {
                    elements = from 直径 in zzdEle.Elements(weizhi)
                               select 直径;
                }
                else if (weizhi.Substring(0, 1).Equals("中"))
                {
                    elements = from 直径 in xe.Elements("中间轴段")
                               select 直径;
                }
                else
                {
                    elements = from 直径 in yzdEle.Elements(weizhi)
                               select 直径;
                }

                double JD = double.Parse(elements.ElementAt(0).Element("直径").Value) / 1000;

                myDimension = (IDimension)modDoc.Parameter(@"D19@" + keySeat);
                myDimension.SystemValue = 0.5 * JD - JS;
                myDimension = (IDimension)modDoc.Parameter(@"D2@" + keySeat + "草图");
                myDimension.SystemValue = JK;
                myDimension = (IDimension)modDoc.Parameter(@"D1@" + keySeat + "草图");
                myDimension.SystemValue = JL;
                myDimension = (IDimension)modDoc.Parameter(@"D3@" + keySeat + "草图");
                myDimension.SystemValue = JC;
                myDimension = (IDimension)modDoc.Parameter(@"D1@基准面" + i.ToString());
                myDimension.SystemValue = JD - JS;
            }

            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
            modDoc.ViewZoomtofit2();
            modDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                   (int)swFileSaveError_e.swGenericSaveError,
                (int)swFileSaveWarning_e.swFileSaveWarning_RebuildError);
        }

        private void btnUpdateShaftParameter_Click(object sender, EventArgs e)
        {
            XElement shaftElement = XElement.Load(strModelFileName);

            shaftElement.Element("中间轴段").Element("直径").Value = textSZJZZJ.Text;
            shaftElement.Element("中间轴段").Element("长度").Value = textSZJZCD.Text;

            shaftElement.Save(strModelFileName);
        }

        private void btnUpdateDrawing_Click(object sender, EventArgs e)
        {

            XElement currentElement = XElement.Load(strModelFileName);
            // MessageBox.Show(strModelFileName);
            //读取工程图模型文件名字
            XElement childElement = currentElement.Element("基本信息");
            string modelFile = childElement.Element("项目目录").Value + childElement.Element("工程图").Value;
            // MessageBox.Show(modelFile);

            //利用齿轮模型文件名字，读取齿轮工程图      
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(modelFile,
                    (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available.Please check your options and make sure there is a part template selected, or select a new part template.");
            }

            //ModelDoc2 swModel = default(ModelDoc2);
            IModelDocExtension swModelDocExt = modDoc.Extension;// default(ModelDocExtension);
            ICustomPropertyManager swCustProp = swModelDocExt.get_CustomPropertyManager("");// default(CustomPropertyManager);
            childElement = currentElement.Element("设计参数");

            //SW 用户自定义属性 更新
            swCustProp.Set("齿数", childElement.Element("齿数").Value);
            swCustProp.Set("法向模数", childElement.Element("法向模数").Value);
            swCustProp.Set("分度圆螺旋角", childElement.Element("螺旋角").Value);

            if (childElement.Element("旋向").Value == "L")
            {
                swCustProp.Set("螺旋方向", "左旋");
            }
            else
            {
                swCustProp.Set("螺旋方向", "右旋");
            }

            swCustProp.Set("齿形角", "??");//“”childElement.Element("法向模数").Value); 
            swCustProp.Set("法向变位系数", childElement.Element("法向变位系数").Value);
            swCustProp.Set("齿顶高系数", childElement.Element("齿顶高系数").Value);
            swCustProp.Set("全齿高", childElement.Element("全齿高").Value);

            swCustProp.Set("精度等级", g1.Text + "-" + g2.Text + "-" + g3.Text + " " + "?" + " " + "?" + " GB 10095-88");
            swCustProp.Set("周节累积公差", Fp.Text);//childElement.Element("法向变位系数").Value);
            swCustProp.Set("齿顶齿形公差高系数", "??");// childElement.Element("齿顶高系数").Value);
            swCustProp.Set("基节极限偏差", fpb.Text);// childElement.Element("法向变位系数").Value);
            swCustProp.Set("周节极限偏差", fpt.Text);//childElement.Element("齿顶高系数").Value); 
            swCustProp.Set("齿形公差", ff.Text);// childElement.Element("齿顶高系数").Value);
            swCustProp.Set("齿圈径向跳动", Fr.Text);//childElement.Element("法向变位系数").Value);
            swCustProp.Set("公法线长度变动", Fw.Text);// childElement.Element("齿顶高系数").Value);
            swCustProp.Set("中心距", currentElement.Element("尺寸计算").Element("中心距").Value);
            swCustProp.Set("齿向公差", "??");//childElement.Element("齿顶高系数").Value); 
            swCustProp.Set("公法线长度上差", "??");//childElement.Element("齿顶高系数").Value); 
            swCustProp.Set("公法线长度下差", "??");//childElement.Element("齿顶高系数").Value); 
            swCustProp.Set("基准面径向公差", "??");//childElement.Element("齿顶高系数").Value); 
            swCustProp.Set("公法线长度大小", "??");//childElement.Element("齿顶高系数").Value); 

            modDoc.EditRebuild3();
            modDoc.ViewZoomtofit2();

            double fenduyuan = double.Parse(currentElement.Element("尺寸计算").Element("分度圆直径").Value);


            ModelDoc2 swmodel;
            DrawingDoc swdraw;

            IView swview;
            DisplayDimension dispdim;
            Dimension swdim;
            Annotation swann;
            SketchPoint swskpt;
            Sketch swviewsketch;
            Sheet swsheet;


            Coordinate[] pocoor = new Coordinate[30];
            object[] annotations;
            object[] pointsarray;
            double swscale;
            double[] vpos = new double[2];
            int i;
            int j;
            int m;

            double[] voutline = new double[4];
            double[] dimvalue = new double[30];



            swmodel = SwAddin.iSwApp.ActiveDoc;
            swdraw = (DrawingDoc)swmodel;

            //为分度圆赋值
            Dimension mydim;
            mydim = swmodel.Parameter("D4@草图5");
            mydim.SystemValue = fenduyuan / 1000;


            //图纸比例
            swsheet = swdraw.GetCurrentSheet();
            double[] sheetproperties = new double[7];
            sheetproperties = swsheet.GetProperties();

            swdraw.ActivateView("工程图视图1");
            swview = swdraw.ActiveDrawingView;
            annotations = swview.GetDisplayDimensions();

            for (j = 0; j < annotations.Length; j++)
            {
                dispdim = (DisplayDimension)annotations[j];
                swdim = dispdim.GetDimension();
                dimvalue[j] = swdim.GetSystemValue2("");

            }
            double oscale;//根据尺寸算出的原始比例
            oscale = (sheetproperties[5] - 0.2) / dimvalue[18];

            //  MessageBox.Show(oscale.ToString());

            if (oscale >= 1 / 10d && oscale < 1 / 6d)
            {
                oscale = 1 / 10d;
            }
            else if (oscale >= 1 / 6d && oscale < 1 / 5d)
            {
                oscale = 1 / 6d;
            }
            else if (oscale >= 1 / 5d && oscale < 1 / 4d)
            {
                oscale = 1 / 5d;
            }
            else if (oscale >= 1 / 4d && oscale < 1 / 3d)
            {
                oscale = 1 / 4d;
            }
            else if (oscale >= (1 / 3d) && oscale < (1 / 2.5d))
            {
                oscale = 1 / 3d;
            }
            else if (oscale >= 1 / 2.5d && oscale < 1 / 2d)
            {
                oscale = 1 / 2.5d;
            }
            else if (oscale >= 1 / 2d && oscale < 1 / 1.5d)
            {
                oscale = 1 / 2d;
            }
            else if (oscale >= 1 / 1.5d && oscale < 1 / 1d)
            {
                oscale = 1 / 1.5d;
            }
            else if (oscale >= 1 / 1d && oscale < 2d / 1)
            {
                oscale = 1 / 1d;
            }
            else if (oscale >= 2d / 1 && oscale < 2.5d / 1)
            {
                oscale = 2d / 1;
            }
            else if (oscale >= 2.5d / 1 && oscale < 4d / 1)
            {
                oscale = 2.5d / 1;
            }
            else if (oscale >= 4d / 1 && oscale < 5d / 1)
            {
                oscale = 4d / 1;
            }
            else if (oscale >= 5d / 1 && oscale < 10d / 1)
            {
                oscale = 5d / 1;
            }
            else
            {
                oscale = 10d / 1;
            }

            MessageBox.Show(oscale.ToString());

            if (oscale <= 1d / oscale)
            {
                swsheet.SetScale(1d, 1d / oscale, true, true);
            }
            else
            {
                swsheet.SetScale(oscale, 1d, true, true);
            }


            //视图中心点   轮廓坐标   比例

            vpos = swview.Position;
            swscale = (double)swview.ScaleDecimal;
            voutline = swview.GetOutline();

            //设置视图的原点
            double ovpos1;
            double ovpos2;
            ovpos1 = 0.07 + Math.Abs(vpos[0] - voutline[0]);
            ovpos2 = sheetproperties[6] - 0.06 - 0.075 * swscale - Math.Abs(voutline[3] - vpos[1]);
            double[] ovpos = new double[2];
            ovpos[0] = ovpos1;
            ovpos[1] = ovpos2;
            swview.Position = ovpos;
            swdraw.EditRebuild();
            MessageBox.Show(ovpos[0].ToString(), ovpos[1].ToString());

            // 重新得到视图的原点   轮廓的坐标  比例
            vpos = swview.Position;
            swscale = (double)swview.ScaleDecimal;
            voutline = swview.GetOutline();

            //得到尺寸定位点  
            swviewsketch = swview.GetSketch();
            pointsarray = swviewsketch.GetSketchPoints2();
            for (i = 0; i < pointsarray.Length; i++)
            {
                swskpt = (SketchPoint)pointsarray[i];
                pocoor[i].x = swskpt.X;
                pocoor[i].y = swskpt.Y;
            }

            //得到每个尺寸值   为尺寸定位
            for (m = 0; m < annotations.Length; m++)
            {
                switch (m)
                {
                    case 3:   //55.011
                        dispdim = (DisplayDimension)annotations[3];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + (dimvalue[25] + 0.5 * dimvalue[4]) * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 4:     //50
                        dispdim = (DisplayDimension)annotations[4];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + (dimvalue[25] + 0.5 * dimvalue[4]) * swscale, voutline[3] + 0.045 * swscale, 0);
                        break;
                    case 5:       //zuo43
                        dispdim = (DisplayDimension)annotations[5];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + 0.75 * dimvalue[25] * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 6:   //35
                        dispdim = (DisplayDimension)annotations[6];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale - 0.25 * dimvalue[26] * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 7:    //32
                        dispdim = (DisplayDimension)annotations[7];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(voutline[0] - 0.03 * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 8:   //80
                        dispdim = (DisplayDimension)annotations[8];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale - (dimvalue[26] + 0.5 * dimvalue[8]) * swscale, voutline[3] + 0.045 * swscale, 0);
                        break;
                    case 9:   //you 43
                        dispdim = (DisplayDimension)annotations[9];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + (dimvalue[25] + dimvalue[4] + 0.25 * dimvalue[10]) * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 10:  //90
                        dispdim = (DisplayDimension)annotations[10];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + (dimvalue[25] + dimvalue[4] + 0.5 * dimvalue[10]) * swscale, voutline[3] + 0.045 * swscale, 0);
                        break;
                    case 11: //you35
                        dispdim = (DisplayDimension)annotations[11];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(voutline[2] + 0.03 * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 12:  //70
                        dispdim = (DisplayDimension)annotations[12];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale - (dimvalue[26] + 0.5 * dimvalue[8]) * swscale, voutline[3] + 0.03 * swscale, 0);
                        break;
                    case 13:   //5 
                        dispdim = (DisplayDimension)annotations[13];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale - (dimvalue[26] - 0.01) * swscale, voutline[3] + 0.03 * swscale, 0);
                        break;
                    case 14:  //   63.693
                        dispdim = (DisplayDimension)annotations[14];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + (dimvalue[25] + 0.75 * dimvalue[4]) * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    case 18:    //413
                        dispdim = (DisplayDimension)annotations[18];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition((voutline[2] + voutline[0]) / 2, voutline[3] + 0.075 * swscale, 0);
                        break;
                    case 19:    //210
                        dispdim = (DisplayDimension)annotations[19];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale - 0.5 * dimvalue[19] * swscale, voutline[3] + 0.06 * swscale, 0);
                        break;
                    case 20:           //170
                        dispdim = (DisplayDimension)annotations[20];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale + 0.5 * dimvalue[20] * swscale, voutline[3] + 0.06 * swscale, 0);
                        break;
                    case 21:   //35f8
                        dispdim = (DisplayDimension)annotations[21];
                        swann = dispdim.GetAnnotation();
                        swann.SetPosition(vpos[0] + pocoor[6].x * swscale - 0.75 * dimvalue[26] * swscale, vpos[1] + pocoor[6].y * swscale, 0);
                        break;
                    default:
                        break;
                }
            }
            swdraw.EditRebuild();
            // 改变注释位置
            Note swnote;
            swview = swdraw.GetFirstView();
            swnote = swview.GetFirstNote();
            swann = swnote.GetAnnotation();
            swann.SetPosition(0.15, 0.07, 0);

            swnote = swnote.GetNext();
            swann = swnote.GetAnnotation();
            swann.SetPosition(0.29, 0.27, 0);
            swdraw.EditRebuild();

            //调整剖面视图的位置，剖面线与键槽中点重合约束，在键槽中间建立一个点，剖面线草图与其重合，剖面位置对其这个点
            double[] pou = new double[2];
            pou[0] = vpos[0] + pocoor[6].x * swscale - (dimvalue[26] + 0.5 * dimvalue[8]) * swscale;
            pou[1] = 0.1;
            swdraw.ActivateView("剖面视图 J-J");
            swview = swdraw.ActiveDrawingView;
            swview.Position = pou;
            swdraw.EditRebuild();
            //设置块的定位点   在左下角    其坐标与比例有关系    不是在图纸上的定值  instance.position
            //具体求出块的名称在宏文件中     两个放大视图的块 用点重合约束了  不在这里调整

            SketchManager skMgr;
            SketchBlockDefinition pBlock;
            SketchBlockInstance pInst;
            // MathPoint insPt;
            MathUtility swMathutil;
            object[] vBlocks;
            double[] bposition = new double[3];
            MathPoint bpmatgpoint;
            object[] vInstance;

            swMathutil = SwAddin.iSwApp.GetMathUtility();
            skMgr = swmodel.SketchManager;
            swmodel.EditSketch();
            vBlocks = skMgr.GetSketchBlockDefinitions();

            bposition[0] = 0.27 / swscale;
            bposition[1] = 0.07 / swscale;
            bposition[2] = 0;
            bpmatgpoint = swMathutil.CreatePoint(bposition);
            pBlock = (SketchBlockDefinition)vBlocks[0];
            vInstance = pBlock.GetInstances();
            pInst = (SketchBlockInstance)vInstance[0];
            pInst.InstancePosition = bpmatgpoint;

            swdraw.EditRebuild();

            modDoc.EditRebuild3();

            modDoc.ViewZoomtofit2();
            // modDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent,
            // (int)swFileSaveError_e.swGenericSaveError,
            // (int)swFileSaveWarning_e.swFileSaveWarning_RebuildError);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (this.dgvLeftShaft.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的左轴段参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvLeftShaft.SelectedRows.Count == 0)
            {
                //     MessageBox.Show("请选择需要修改的左轴段参数！", "提示", MessageBoxButtons.OK);
            }

            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改左轴段参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                FrmModifyShaftParameters frmshaft = new FrmModifyShaftParameters();

                //往窗口里传值
                int i = this.dgvLeftGearShaft.SelectedCells[0].RowIndex;
                frmshaft.strzhouduan = this.dgvLeftGearShaft[0, i].Value.ToString();
                frmshaft.strzhijing = this.dgvLeftGearShaft[1, i].Value.ToString();
                frmshaft.strchangdu = this.dgvLeftGearShaft[2, i].Value.ToString();
                frmshaft.strdaojiao = this.dgvLeftGearShaft[3, i].Value.ToString();
                frmshaft.stryuanjiao = this.dgvLeftGearShaft[4, i].Value.ToString();

                if (frmshaft.ShowDialog() == DialogResult.OK)
                {

                    //更新dgvParameter中的值
                    this.dgvLeftGearShaft[0, i].Value = frmshaft.strzhouduan;
                    this.dgvLeftGearShaft[1, i].Value = frmshaft.strzhijing;
                    this.dgvLeftGearShaft[2, i].Value = frmshaft.strchangdu;
                    this.dgvLeftGearShaft[3, i].Value = frmshaft.strdaojiao;
                    this.dgvLeftGearShaft[4, i].Value = frmshaft.stryuanjiao;


                    //将修改后的信息存入XML                 

                    string strzhouduan = this.dgvLeftGearShaft[0, i].Value.ToString();

                    if (strzhouduan != "")
                    {
                        string strElement = "左轴段" + strzhouduan;

                        XElement currentElement = XElement.Load(strModelFileName);
                        XElement childElmb = currentElement.Element("左轴段").Element(strElement);

                        childElmb.ReplaceNodes(
                            new XElement("轴段", strzhouduan),
                             new XElement("直径", this.dgvLeftGearShaft[1, i].Value.ToString()),
                             new XElement("长度", this.dgvLeftGearShaft[2, i].Value.ToString()),
                             new XElement("倒角", this.dgvLeftGearShaft[3, i].Value.ToString()),
                             new XElement("圆角", this.dgvLeftGearShaft[4, i].Value.ToString()));

                        currentElement.Save(strModelFileName);
                    }

                }
                else
                    return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dgvRightShaft.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的右轴段参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvRightShaft.SelectedRows.Count == 0)
            {
                //   MessageBox.Show("请选择需要修改的右轴段参数！", "提示", MessageBoxButtons.OK);
            }

            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改右轴段参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                FrmModifyShaftParameters frmshaft = new FrmModifyShaftParameters();

                //往窗口里传值
                int i = this.dgvRightGearShaft.SelectedCells[0].RowIndex;
                frmshaft.strzhouduan = this.dgvRightGearShaft[0, i].Value.ToString();
                frmshaft.strzhijing = this.dgvRightGearShaft[1, i].Value.ToString();
                frmshaft.strchangdu = this.dgvRightGearShaft[2, i].Value.ToString();
                frmshaft.strdaojiao = this.dgvRightGearShaft[3, i].Value.ToString();
                frmshaft.stryuanjiao = this.dgvRightGearShaft[4, i].Value.ToString();

                if (frmshaft.ShowDialog() == DialogResult.OK)
                {

                    //更新dgvParameter中的值
                    this.dgvRightGearShaft[0, i].Value = frmshaft.strzhouduan;
                    this.dgvRightGearShaft[1, i].Value = frmshaft.strzhijing;
                    this.dgvRightGearShaft[2, i].Value = frmshaft.strchangdu;
                    this.dgvRightGearShaft[3, i].Value = frmshaft.strdaojiao;
                    this.dgvRightGearShaft[4, i].Value = frmshaft.stryuanjiao;


                    //将修改后的信息存入XML                 

                    string strzhouduan = this.dgvRightGearShaft[0, i].Value.ToString();

                    if (strzhouduan != "")
                    {
                        string strElement = "右轴段" + strzhouduan;

                        XElement currentElement = XElement.Load(strModelFileName);
                        XElement childElmb = currentElement.Element("右轴段").Element(strElement);

                        childElmb.ReplaceNodes(
                            new XElement("轴段", strzhouduan),
                            new XElement("直径", this.dgvRightGearShaft[1, i].Value.ToString()),
                            new XElement("长度", this.dgvRightGearShaft[2, i].Value.ToString()),
                            new XElement("倒角", this.dgvRightGearShaft[3, i].Value.ToString()),
                            new XElement("圆角", this.dgvRightGearShaft[4, i].Value.ToString())
                            );

                        currentElement.Save(strModelFileName);
                    }

                }
                else
                    return;
            }
        }

        private void tbpShaft_Click(object sender, EventArgs e)
        {

        }

        private void btnjiancao_Click(object sender, EventArgs e)
        {
            if (this.dgvRightGearShaft.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的键槽参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvRightGearShaft.SelectedCells[0].RowIndex == 0)
            {
                //     MessageBox.Show("请选择需要修改的键槽参数！", "提示", MessageBoxButtons.OK);
            }

            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改键槽参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                frmModifyjiancaoParameters frmjiancao = new frmModifyjiancaoParameters();

                //往窗口里传值
                int i = this.dgvjiancao.SelectedCells[0].RowIndex;

                frmjiancao.strjiancaoxuhao = this.dgvjiancao[0, i].Value.ToString();
                frmjiancao.strjiancaoweizhi = this.dgvjiancao[1, i].Value.ToString();
                frmjiancao.strjianchang = this.dgvjiancao[2, i].Value.ToString();
                frmjiancao.strdingwei = this.dgvjiancao[3, i].Value.ToString();
                frmjiancao.strjiankuan = this.dgvjiancao[4, i].Value.ToString();
                frmjiancao.strjianshen = this.dgvjiancao[5, i].Value.ToString();

                if (frmjiancao.ShowDialog() == DialogResult.OK)
                {

                    //更新dgvParameter中的值
                    this.dgvjiancao[0, i].Value = frmjiancao.strjiancaoxuhao;
                    this.dgvjiancao[1, i].Value = frmjiancao.strjiancaoweizhi;
                    this.dgvjiancao[2, i].Value = frmjiancao.strjianchang;
                    this.dgvjiancao[3, i].Value = frmjiancao.strdingwei;
                    this.dgvjiancao[4, i].Value = frmjiancao.strjiankuan;
                    this.dgvjiancao[5, i].Value = frmjiancao.strjianshen;

                    //将修改后的信息存入XML                 

                    string strzhouduan = this.dgvjiancao[0, i].Value.ToString();

                    if (strzhouduan != "")
                    {
                        string strElement = "键槽" + strzhouduan;

                        XElement currentElement = XElement.Load(strModelFileName);
                        XElement childElmb = currentElement.Element("键槽").Element(strElement);

                        childElmb.ReplaceNodes(
                            new XElement("位置", this.dgvjiancao[1, i].Value.ToString()),
                            new XElement("长度", this.dgvjiancao[2, i].Value.ToString()),
                            new XElement("定位", this.dgvjiancao[3, i].Value.ToString()),
                            new XElement("宽", this.dgvjiancao[4, i].Value.ToString()),
                            new XElement("深", this.dgvjiancao[5, i].Value.ToString())
                            );

                        currentElement.Save(strModelFileName);
                    }

                }
                else
                    return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.dgvLeftShaft.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的左轴段参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvLeftShaft.SelectedRows.Count == 0)
            {
                //     MessageBox.Show("请选择需要修改的左轴段参数！", "提示", MessageBoxButtons.OK);
            }

            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改左轴段参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                FrmModifyShaftParameters frmshaft = new FrmModifyShaftParameters();

                //往窗口里传值
                int i = this.dgvLeftShaft.SelectedCells[0].RowIndex;
                frmshaft.strzhouduan = this.dgvLeftShaft[0, i].Value.ToString();
                frmshaft.strzhijing = this.dgvLeftShaft[1, i].Value.ToString();
                frmshaft.strchangdu = this.dgvLeftShaft[2, i].Value.ToString();
                frmshaft.strdaojiao = this.dgvLeftShaft[3, i].Value.ToString();
                frmshaft.stryuanjiao = this.dgvLeftShaft[4, i].Value.ToString();

                if (frmshaft.ShowDialog() == DialogResult.OK)
                {

                    //更新dgvParameter中的值
                    this.dgvLeftShaft[0, i].Value = frmshaft.strzhouduan;
                    this.dgvLeftShaft[1, i].Value = frmshaft.strzhijing;
                    this.dgvLeftShaft[2, i].Value = frmshaft.strchangdu;
                    this.dgvLeftShaft[3, i].Value = frmshaft.strdaojiao;
                    this.dgvLeftShaft[4, i].Value = frmshaft.stryuanjiao;


                    //将修改后的信息存入XML                 

                    string strzhouduan = this.dgvLeftShaft[0, i].Value.ToString();

                    if (strzhouduan != "")
                    {
                        string strElement = "左轴段" + strzhouduan;

                        XElement currentElement = XElement.Load(strModelFileName);
                        XElement childElmb = currentElement.Element("左轴段").Element(strElement);

                        childElmb.ReplaceNodes(
                            new XElement("轴段", strzhouduan),
                             new XElement("直径", this.dgvLeftShaft[1, i].Value.ToString()),
                             new XElement("长度", this.dgvLeftShaft[2, i].Value.ToString()),
                             new XElement("倒角", this.dgvLeftShaft[3, i].Value.ToString()),
                             new XElement("圆角", this.dgvLeftShaft[4, i].Value.ToString()));

                        currentElement.Save(strModelFileName);
                    }

                }
                else
                    return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dgvRightShaft.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的右轴段参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvRightShaft.SelectedRows.Count == 0)
            {
                //     MessageBox.Show("请选择需要修改的右轴段参数！", "提示", MessageBoxButtons.OK);
            }

            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改右轴段参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                FrmModifyShaftParameters frmshaft = new FrmModifyShaftParameters();

                //往窗口里传值
                int i = this.dgvRightShaft.SelectedCells[0].RowIndex;
                frmshaft.strzhouduan = this.dgvRightShaft[0, i].Value.ToString();
                frmshaft.strzhijing = this.dgvRightShaft[1, i].Value.ToString();
                frmshaft.strchangdu = this.dgvRightShaft[2, i].Value.ToString();
                frmshaft.strdaojiao = this.dgvRightShaft[3, i].Value.ToString();
                frmshaft.stryuanjiao = this.dgvRightShaft[4, i].Value.ToString();

                if (frmshaft.ShowDialog() == DialogResult.OK)
                {

                    //更新dgvParameter中的值
                    this.dgvRightShaft[0, i].Value = frmshaft.strzhouduan;
                    this.dgvRightShaft[1, i].Value = frmshaft.strzhijing;
                    this.dgvRightShaft[2, i].Value = frmshaft.strchangdu;
                    this.dgvRightShaft[3, i].Value = frmshaft.strdaojiao;
                    this.dgvRightShaft[4, i].Value = frmshaft.stryuanjiao;


                    //将修改后的信息存入XML                 

                    string strzhouduan = this.dgvRightShaft[0, i].Value.ToString();

                    if (strzhouduan != "")
                    {
                        string strElement = "右轴段" + strzhouduan;

                        XElement currentElement = XElement.Load(strModelFileName);
                        XElement childElmb = currentElement.Element("右轴段").Element(strElement);

                        childElmb.ReplaceNodes(
                            new XElement("轴段", strzhouduan),
                            new XElement("直径", this.dgvRightShaft[1, i].Value.ToString()),
                            new XElement("长度", this.dgvRightShaft[2, i].Value.ToString()),
                            new XElement("倒角", this.dgvRightShaft[3, i].Value.ToString()),
                            new XElement("圆角", this.dgvRightShaft[4, i].Value.ToString())
                            );

                        currentElement.Save(strModelFileName);
                    }

                }
                else
                    return;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.dgvjiancao1.Rows.Count == 0)
            {
                MessageBox.Show("请确定有需要修改的键槽参数！", "提示", MessageBoxButtons.OK);
            }

            if (this.dgvjiancao1.SelectedRows.Count == 0)
            {
                //   MessageBox.Show("请选择需要修改的键槽参数！", "提示", MessageBoxButtons.OK);
            }

            //提示是否确定修改？
            if (MessageBox.Show("您确定要修改键槽参数吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                frmModifyjiancaoParameters frmjiancao = new frmModifyjiancaoParameters();

                //往窗口里传值
                int i = this.dgvjiancao1.SelectedCells[0].RowIndex;

                frmjiancao.strjiancaoxuhao = this.dgvjiancao1[0, i].Value.ToString();
                frmjiancao.strjiancaoweizhi = this.dgvjiancao1[1, i].Value.ToString();
                frmjiancao.strjianchang = this.dgvjiancao1[2, i].Value.ToString();
                frmjiancao.strdingwei = this.dgvjiancao1[3, i].Value.ToString();
                frmjiancao.strjiankuan = this.dgvjiancao1[4, i].Value.ToString();
                frmjiancao.strjianshen = this.dgvjiancao1[5, i].Value.ToString();

                if (frmjiancao.ShowDialog() == DialogResult.OK)
                {

                    //更新dgvParameter中的值
                    this.dgvjiancao1[0, i].Value = frmjiancao.strjiancaoxuhao;
                    this.dgvjiancao1[1, i].Value = frmjiancao.strjiancaoweizhi;
                    this.dgvjiancao1[2, i].Value = frmjiancao.strjianchang;
                    this.dgvjiancao1[3, i].Value = frmjiancao.strdingwei;
                    this.dgvjiancao1[4, i].Value = frmjiancao.strjiankuan;
                    this.dgvjiancao1[5, i].Value = frmjiancao.strjianshen;

                    //将修改后的信息存入XML                 

                    string strzhouduan = this.dgvjiancao1[0, i].Value.ToString();

                    if (strzhouduan != "")
                    {
                        string strElement = "键槽" + strzhouduan;

                        XElement currentElement = XElement.Load(strModelFileName);
                        XElement childElmb = currentElement.Element("键槽").Element(strElement);

                        childElmb.ReplaceNodes(
                            new XElement("位置", this.dgvjiancao1[1, i].Value.ToString()),
                            new XElement("长度", this.dgvjiancao1[2, i].Value.ToString()),
                            new XElement("定位", this.dgvjiancao1[3, i].Value.ToString()),
                            new XElement("宽", this.dgvjiancao1[4, i].Value.ToString()),
                            new XElement("深", this.dgvjiancao1[5, i].Value.ToString())
                            );

                        currentElement.Save(strModelFileName);
                    }

                }
                else
                    return;
            }

        }

        private void btnUpdateGearShaftParameter_Click(object sender, EventArgs e)
        {
            string gearShaftFile = xEle.Element(U2Level).Element(fatherLevel).Element("轴").Attribute("文件").Value;

            XElement gearShaftElement = XElement.Load(gearShaftFile);


            //齿轮参数
            gearShaftElement.Element("设计参数").Element("法向模数").Value = textBox6.Text;
            gearShaftElement.Element("设计参数").Element("法向压力角").Value = textBox5.Text;
            gearShaftElement.Element("设计参数").Element("螺旋角").Value = textBox4.Text;
            gearShaftElement.Element("设计参数").Element("齿数").Value = textBox17.Text;
            gearShaftElement.Element("设计参数").Element("齿顶高系数").Value = textBox8.Text;
            gearShaftElement.Element("设计参数").Element("法向变位系数").Value = textBox16.Text;
            gearShaftElement.Element("设计参数").Element("顶隙系数").Value = textBox10.Text;
            gearShaftElement.Element("设计参数").Element("齿宽").Value = textBox9.Text;



            if (comboBox3.Text == "左旋")
            {
                gearShaftElement.Element("设计参数").Element("旋向").Value = "L";
            }
            else
            {
                gearShaftElement.Element("设计参数").Element("旋向").Value = "R";
            }

            gearShaftElement.Save(gearShaftFile);
        }

        private void btnUpdataAssemlbeModel_Click(object sender, EventArgs e)
        {
            //类变量，调研前要赋值
            // xlmGearShaftModelFileName = @"C:\齿轮轴.xml";//齿轮的XML文件名字

            //读取齿轮装配模型文件名字
            //IEnumerable<XElement> elements = from 齿轮轴名称 in xe.Elements("基本信息")// where PInfo.Attribute("ID").Value == strID
            //                                 select 齿轮轴名称;
            XElement xe = XElement.Load(strModelFileName);
            string modelFile = xe.Element(this.cmbType.Text.ToString()).Attribute("模型文件").Value;

            //利用齿轮装配模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(modelFile,
                    (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }


            //SldWorks swApp;
            // IModelDoc2 swModel = default(IModelDoc2);
            ISelectionMgr swSelMgr = default(ISelectionMgr);

            IDimension myDimension = null;

            swSelMgr = modDoc.SelectionManager as SelectionMgr;



            int jishu = int.Parse(xe.Element(this.cmbType.Text.ToString()).Element("基本信息").Element("传动级数").Value);
            double[] zxj = new double[jishu];
            int j = 1;
            for (int i = 0; i < jishu; i++)
            {
                myDimension = (IDimension)modDoc.Parameter("D" + j + "@草图1");
                myDimension.SystemValue = double.Parse(dgvParameter[2, i].Value.ToString()) / 1000;
                j = j + 1;
            }

            modDoc.EditRebuild3();
            modDoc.ShowNamedView2("*Isometric", (int)swStandardViews_e.swTrimetricView);
            modDoc.ViewZoomtofit2();
            modDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                         (int)swFileSaveError_e.swGenericSaveError,
                         (int)swFileSaveWarning_e.swFileSaveWarning_RebuildError);
        }

        private void btnUpdataAssemlbeDrawing_Click(object sender, EventArgs e)
        {
            //类变量，调研前要赋值
            // xlmGearShaftModelFileName = @"C:\齿轮轴.xml";//齿轮的XML文件名字

            //读取齿轮装配模型文件名字
            //IEnumerable<XElement> elements = from 齿轮轴名称 in xe.Elements("基本信息")// where PInfo.Attribute("ID").Value == strID
            //                                 select 齿轮轴名称;
            XElement xe = XElement.Load(strModelFileName);
            string modelFile = xe.Element(this.cmbType.Text.ToString()).Attribute("工程图文件").Value;

            //利用齿轮装配模型文件名字，读取齿轮模型        
            string partTemplate = SwAddin.iSwApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            if ((partTemplate != null) && (partTemplate != ""))
            {
                modDoc = (IModelDoc2)SwAddin.iSwApp.OpenDoc6(modelFile,
                    (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "",
                    (int)swFileLoadError_e.swGenericError, (int)swFileLoadWarning_e.swFileLoadWarning_AlreadyOpen);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There is no part template available. Please check your options and make sure there is a part template selected, or select a new part template.");
            }


            modDoc.EditRebuild3();

            modDoc.ViewZoomtofit2();

            modDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent,
                   (int)swFileSaveError_e.swGenericSaveError,
                (int)swFileSaveWarning_e.swFileSaveWarning_RebuildError);
        }

        private void dgvRightShaft_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvLeftShaft_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }



    }

    public class SolveFun
    {
        public SolveFun()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /*调用形式
         SolveFun Fun=new SolveFun();
            double c=Fun.Gexianfa(2,1,0.000000000001);
        */

        public double Gexianfa(double rangeL, double rangeR, double eps, double a)
        {
            double t_s = rangeL;
            double t_e = rangeR;
            double y_s = f(t_s, a);
            double y_e = f(t_e, a);
            if ((y_s - y_e) == 0)
            {
                MessageBox.Show("端点函数值不能相等", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }

            int i = 0;
            double alpha = (t_s + t_e) / 2;
            double Error = eps;
            while (Math.Abs(f(alpha, a)) >= Error & i < 1000)
            {
                alpha = t_e - f(t_e, a) * (t_e - t_s) / (f(t_e, a) - f(t_s, a) + eps);
                t_s = t_e;
                t_e = alpha;
                i = i + 1;
            }
            if (i >= 1000)
            {
                MessageBox.Show("迭代失败!!!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
            else
            {

                return alpha;//返回迭代的计算结果
            }

        }
        public double f(double alpha, double a)//此处为求解超越方程——端面截形，在解超越方程时，此处输入超越方程式，返回函数数值
        {
            double result;
            result = Math.Tan(alpha) - alpha - a;
            return result;
        }
    }
}