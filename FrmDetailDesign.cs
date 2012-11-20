using System;
using System.Windows.Forms;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{
    public partial class FrmDetailDesign : Form
    {
        public FrmDetailDesign()
        {
            InitializeComponent();
        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "第1级轴串")
            {
                pictureBox1.Load(SystemConstants.StrProjectPath + SystemConstants.XmlProject.Element("第1级轴串").Element("轴串图").Value);
                pictureBox2.Load(SystemConstants.StrProjectPath + SystemConstants.XmlProject.Element("第2级轴串").Element("轴串图").Value);

                button1.Visible = false;
                button3.Visible = false;
                //SecondPartForm frm = new SecondPartForm();
                //frm.Show();
                //this.Hide();
            }
            if (e.Node.Text == "第2级轴串")
            {
                pictureBox1.Load(SystemConstants.StrProjectPath + SystemConstants.XmlProject.Element("第2级轴串").Element("轴串图").Value);
                pictureBox2.Load(SystemConstants.StrProjectPath + SystemConstants.XmlProject.Element("第4级轴串").Element("轴串图").Value);
                button1.Visible = false;
                button3.Visible = false;
                //SecondPartForm frm = new SecondPartForm();
                //frm.Show();
                //this.Hide();
            }
            if (e.Node.Text == "第3级轴串")
            {
                pictureBox1.Load(SystemConstants.StrProjectPath + SystemConstants.XmlProject.Element("第3级轴串").Element("轴串图").Value);
                button1.Visible = false;
                button3.Visible = false;
                //SecondPartForm frm = new SecondPartForm();
                //frm.Show();
                //this.Hide();
            }
            if (e.Node.Text == "第4级轴串")
            {
                pictureBox1.Load(SystemConstants.StrProjectPath + SystemConstants.XmlProject.Element("第4级轴串").Element("轴串图").Value);
                button1.Visible = true;
                button3.Visible = true;
                //SecondPartForm frm = new SecondPartForm();
                //frm.Show();
                //this.Hide();
            }
        }


        //递归调用，建立TreeView结构树
        //private void ConvertXmlNodeToTreeNode(XmlNodeList xmlNodes, TreeNodeCollection treeNodes)
        //{
        //    foreach (XmlNode xmlNode in xmlNodes)
        //    {
        //        string nodeText = xmlNode.Name.ToString();

        //        string nodeTreeNode = "";
        //        string nodeValue = "";

        //        if (xmlNode.ChildNodes.Count == 1)
        //        {
        //            nodeValue = xmlNode.InnerText.ToString();
        //            nodeTreeNode = nodeValue;
        //        }
        //        //if (xmlNode.Value == null)

        //        else
        //        {
        //            nodeTreeNode = nodeText;
        //        }

        //        TreeNode newTreeNode = new TreeNode(nodeTreeNode);
        //        newTreeNode.Tag = nodeValue;
        //        if (xmlNode.ChildNodes.Count != 1)
        //        {
        //            this.ConvertXmlNodeToTreeNode(xmlNode.ChildNodes, newTreeNode.Nodes);
        //        }
        //        treeNodes.Add(newTreeNode);
        //    }
        //}

        private void BuildAssemblyTreeNode()
        {
            treeView1.LabelEdit = true;//可编辑状态。   
            treeView1.Nodes.Clear();
            //这个结点是根节点。    
            //TreeNode node = new TreeNode();   
            //node.Text = "hope";
            //treeView1.Nodes.Add(node);   
            //TreeNode node1 = new TreeNode();     
            //node1.Text = "hopeone";      
            //TreeNode node11 = new TreeNode();     
            //node11.Text = "hopeoneone";   
            //TreeNode node2 = new TreeNode();    
            //node2.Text = "hopetwo";      
            //node1.Nodes.Add(node11);//在node1下面在添加一个结点。    
            //node.Nodes.Add(node1);//node下的两个子节点。     
            //node.Nodes.Add(node2);           
            //TreeNode t = new TreeNode("basil");
            ////作为根节点。     
            //treeView1.Nodes.Add(t);   
            //TreeNode t1 = new TreeNode("basilone");  
            //t.Nodes.Add(t1);     
            //TreeNode t2 = new TreeNode("basiltwo"); 
            //t.Nodes.Add(t2);  
            int iGrade = 3;// = Convert.ToInt32(SystemConstants.XmlProject.Element("总体要求").Element("级数").Value);
            //TreeNode newTreeNode = new TreeNode();

            for (int i = 1; i <= iGrade + 1; i++)
            {
                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = "第" + i + "级轴串";
                treeView1.Nodes.Add(newTreeNode);
                TreeNode childTreeNode1 = new TreeNode();
                childTreeNode1.Text = "齿轮轴";
                newTreeNode.Nodes.Add(childTreeNode1);
                if (i != 1)
                {
                    TreeNode childTreeNode2 = new TreeNode();
                    childTreeNode2.Text = "齿轮";
                    newTreeNode.Nodes.Add(childTreeNode2);
                }
            }
            //foreach (XmlNode xmlNode in xmlNodes)
            //{
            //    string nodeText = xmlNode.Name.ToString();

            //    string nodeTreeNode = "";
            //    string nodeValue = "";

            //    if (xmlNode.ChildNodes.Count == 1)
            //    {
            //        nodeValue = xmlNode.InnerText.ToString();
            //        nodeTreeNode = nodeValue;
            //    }
            //    //if (xmlNode.Value == null)

            //    else
            //    {
            //        nodeTreeNode = nodeText;
            //    }


            //    newTreeNode.Tag = nodeValue;
            //    if (xmlNode.ChildNodes.Count != 1)
            //    {
            //        this.ConvertXmlNodeToTreeNode(xmlNode.ChildNodes, newTreeNode.Nodes);
            //    }
            //    treeNodes.Add(newTreeNode);
        }

        private void InputPartForm_Load(object sender, EventArgs e)
        {
            BuildAssemblyTreeNode();
        }

        private void InputPartForm_Load_1(object sender, EventArgs e)
        {
            BuildAssemblyTreeNode();
        }

        private void btForceForm_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            if (i == 1)
            {
                FrmBearForceAnalysis1 childFrame = new FrmBearForceAnalysis1(i);
                childFrame.Show();
            }
            if (i == 2)
            {
                FrmBearForceAnalysis2 childFrame = new FrmBearForceAnalysis2(i);
                childFrame.Show();
            }
            if (i == 3)
            {
                FrmBearForceAnalysis3 childFrame = new FrmBearForceAnalysis3(i);
                childFrame.Show();
            }
            if (i == 4)
            {
                Form11 childFrame = new Form11(i);
                childFrame.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            if (i == 1)
            {
                FrmStrengthCalculation1 childFrame = new FrmStrengthCalculation1(i);
                childFrame.Show();
            }
            if (i == 2)
            {
                FrmStrengthCalculation2 childFrame = new FrmStrengthCalculation2(i);
                childFrame.Show();
            }
            if (i == 3)
            {
                FrmStrengthCalculation2 childFrame = new FrmStrengthCalculation2(i);
                childFrame.Show();
            }
            if (i == 4)
            {
                Form19 childFrame = new Form19(i);
                childFrame.Show();
            }
        }

        private void btInterferenceForm_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            double t = 0.0;
            double d = 0.0;
            if (i == 1)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段6直径").Value);
                FrmInterferenceConnection1 childFrame = new FrmInterferenceConnection1(i, t, d);
                childFrame.Show();
            }
            if (i == 2)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段5直径").Value);
                FrmInterferenceConnection1 childFrame = new FrmInterferenceConnection1(i, t, d);
                childFrame.Show();
            }
            if (i == 3)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段5直径").Value);
                FrmInterferenceConnection1 childFrame = new FrmInterferenceConnection1(i, t, d);
                childFrame.Show();
            }
            if (i == 4)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("受力计算参数").Element("T1").Value);
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段1直径").Value);
                FrmInterferenceConnection1 childFrame = new FrmInterferenceConnection1(i, t, d);
                childFrame.Show();
            }
        }

        private void btKeyForm_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            double t = 0.0;
            double d = 0.0;
            double b = 0.0;
            double h = 0.0;
            double l = 0.0;

            if (i == 1)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                b = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽b").Value);
                h = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽h").Value); //键宽
                l = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽L").Value);//键长度
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段6直径").Value);
                FrmKeyStrength1 frm = new FrmKeyStrength1(i, t, d, b, h, l);
                frm.Show();
            }
            if (i == 2)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                b = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽b").Value);
                h = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽h").Value); //键宽
                l = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽L").Value);//键长度
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段5直径").Value);
                FrmKeyStrength1 frm = new FrmKeyStrength1(i, t, d, b, h, l);
                frm.Show();
            }
            if (i == 3)
            {
                t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                b = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽b").Value);
                h = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽h").Value); //键宽
                l = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("键槽L").Value);//键长度
                d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段5直径").Value);
                FrmKeyStrength1 frm = new FrmKeyStrength1(i, t, d, b, h, l);
                frm.Show();
            }
            if (i == 4)
            {
                KeyStrength4 frm = new KeyStrength4(i);
                frm.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            FrmBearingLife frm = new FrmBearingLife(i);
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            if (i == 4)
            {
                double t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("动力参数").Element("扭矩").Value);
                double d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段5直径").Value);
                FrmInterferenceConnection1 childFrame = new FrmInterferenceConnection1(i, t, d);
                childFrame.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = treeView1.SelectedNode.Text;
            int i = Convert.ToInt32(str.Length == 5 ? str.Substring(1, 1) : str.Substring(1, 2));
            if (i == 4)
            {
                double t = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("受力计算参数").Element("T2").Value);
                double d = double.Parse(SystemConstants.XmlProject.Element("第" + i + "级轴串").Element("齿轮轴").Element("轴段7直径").Value);
                FrmInterferenceConnection1 childFrame = new FrmInterferenceConnection1(i, t, d);
                childFrame.Show();
            }
        }
    }
}




