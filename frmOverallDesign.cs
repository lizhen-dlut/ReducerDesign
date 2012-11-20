using System;
using System.Windows.Forms;
using System.Xml.Linq;
using ReducerDesign.CommonClass;

namespace ReducerDesign
{

    public partial class FrmOverallDesign : Form
    {
        public FrmOverallDesign()
        {
            InitializeComponent();
        }

        #region 保存对话框

        private void SaveFileDialog()
        {
            //string localFilePath, fileNameExt, newFileName, FilePath; 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //设置文件类型 
            saveFileDialog1.Filter = " txt files(*.txt)|*.txt|All files(*.*)|*.*";

            //设置默认文件类型显示顺序 
            saveFileDialog1.FilterIndex = 2;

            //保存对话框是否记忆上次打开的目录 
            saveFileDialog1.RestoreDirectory = true;

            //点了保存按钮进入 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径 
                //localFilePath = saveFileDialog1.FileName.ToString();

                //获取文件名，不带路径 
                //fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                //获取文件路径，不带文件名 
                //FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));

                //给文件名前加上时间 
                //newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt;

                //在文件名里加字符 
                //saveFileDialog1.FileName.Insert(1,"dameng");

                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile(); //输出文件

                //fs输出带文字或图片的文件，就看需求了 

            }
        }

        #endregion

        private void butProjectFolder_Click(object sender, EventArgs e)
        {
            // FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            //folderBrowserDialog.SelectedPath = @"D:\"; //　设置打开目录选择对话框时默认的目录 
            //folderBrowserDialog.ShowNewFolderButton = true; // false; //是否显示新建文件夹按钮 
            //folderBrowserDialog.Description = "设置项目的目录"; //描述弹出框功能 
            //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer; //.MyDocuments.MyDocuments;　// 打开到我的文档 

            folderBrowserDialog.ShowDialog(); // 打开目录选择对话框 
            SystemConstants.StrProjectPath = folderBrowserDialog.SelectedPath; // 返回用户选择的目录地址 

            #region 复制减速器模版到项目目录

            string[] fileProperties = new string[2];
            fileProperties[0] = @"D:\zip\减速器.zip"; //待解压的文件
            fileProperties[1] = SystemConstants.StrProjectPath + "\\"; // @"D:\unzipped\"; //解压后放置的目标目录
            UnZipClass UnZc = new UnZipClass();
            UnZc.UnZip(fileProperties);

            #endregion

            #region 把减速器基本参数写入 “减速器.xml” 文件

            SystemConstants.XmlProject = XElement.Load(SystemConstants.StrProjectPath + @"\减速器.xml");
            SystemConstants.XmlProject.Element("总体要求").Element("级数").Value = cmbGradeOfReducer.Text;
            SystemConstants.XmlProject.Element("总体要求").Element("结构形式").Value = cmbTypeOfReducer.Text;
            SystemConstants.XmlProject.Element("总体要求").Element("总功率").Value = txtP.Text;
            SystemConstants.XmlProject.Element("总体要求").Element("转速").Value = txtN.Text;
            SystemConstants.XmlProject.Element("总体要求").Element("总速比").Value = txtTotalRate.Text;
            SystemConstants.XmlProject.Save(SystemConstants.StrProjectPath + @"\减速器.xml");

            #endregion

            //MessageBox.Show(strProjectPath);
        }




        private void button2_Click(object sender, EventArgs e)
        {
            //FrmGearCalculate a = new FrmGearCalculate();
           // Test a=new Test();
            //FrmAssemblySketch a = new FrmAssemblySketch();
            //a.Show();

            //string[] FileProperties = new string[2];
            //FileProperties[0] = @"D:\unzipped\"; //待压缩文件目录
            //FileProperties[1] = @"D:\zip\a.zip"; //压缩后的目标文件
            //ZipClass Zc = new ZipClass();
            //Zc.ZipFileMain(FileProperties);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] FileProperties = new string[2];
            FileProperties[0] = @"D:\zip\2.zip"; //待解压的文件
            FileProperties[1] = @"D:\unzipped\"; //解压后放置的目标目录
            UnZipClass UnZc = new UnZipClass();
            UnZc.UnZip(FileProperties);
        }

        private void btnOpenProjectFile_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = SystemConstants.StrProjectPath; // "c:\\";//注意这里写路径时要用c:\\而不是c:\/
            //openFileDialog.Filter = "项目文件|*.xml|C#文件|*.cs|所有文件|*.*";
            //openFileDialog.RestoreDirectory = true;
            //openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog.FileName;
                SystemConstants.StrProjectPath = fName.Substring(0, fName.Length - openFileDialog.SafeFileName.Length);
                SystemConstants.XmlProject = XElement.Load(openFileDialog.FileName);

                XElement xElement = SystemConstants.XmlProject.Element("总体要求");
                if (xElement != null)
                {
                    cmbGradeOfReducer.Text = xElement.Element("级数").Value;
                    cmbTypeOfReducer.Text = xElement.Element("结构形式").Value;
                    txtP.Text = xElement.Element("总功率").Value;
                    txtN.Text = xElement.Element("转速").Value;
                    txtTotalRate.Text = xElement.Element("总速比").Value;
                }
                cmbGradeOfReducer.Enabled = false;
                cmbTypeOfReducer.Enabled = false;
                txtN.Enabled = false;
                txtP.Enabled = false;
                txtTotalRate.Enabled = false;
                //xmlProject.Save(strProjectPath + @"\减速器.xml");
                //  File  fileOpen=new File(fName);
                // bool isFileHaveName = true;
                //richTextBox1.Text=fileOpen.ReadFile();
                //richTextBox1.AppendText("");
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}