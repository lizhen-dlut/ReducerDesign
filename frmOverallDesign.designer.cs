namespace ReducerDesign
{
    partial class FrmOverallDesign
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtN = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtP = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTotalRate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cmbTypeOfReducer = new System.Windows.Forms.ComboBox();
            this.cmbGradeOfReducer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnOpenProjectFile = new System.Windows.Forms.Button();
            this.butProjectFolder = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtN);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtP);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtTotalRate);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 185);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "总体要求";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(197, 88);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 12);
            this.label18.TabIndex = 3;
            this.label18.Text = "rpm";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(197, 31);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 12);
            this.label17.TabIndex = 3;
            this.label17.Text = "Kw";
            // 
            // txtN
            // 
            this.txtN.Location = new System.Drawing.Point(91, 84);
            this.txtN.Name = "txtN";
            this.txtN.Size = new System.Drawing.Size(100, 21);
            this.txtN.TabIndex = 1;
            this.txtN.Text = "1000";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 140);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "总速比：";
            // 
            // txtP
            // 
            this.txtP.Location = new System.Drawing.Point(91, 27);
            this.txtP.Name = "txtP";
            this.txtP.Size = new System.Drawing.Size(100, 21);
            this.txtP.TabIndex = 0;
            this.txtP.Text = "30";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.Control;
            this.label14.Location = new System.Drawing.Point(20, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "总功率：";
            // 
            // txtTotalRate
            // 
            this.txtTotalRate.Location = new System.Drawing.Point(91, 136);
            this.txtTotalRate.Name = "txtTotalRate";
            this.txtTotalRate.Size = new System.Drawing.Size(100, 21);
            this.txtTotalRate.TabIndex = 2;
            this.txtTotalRate.Text = "48";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "转  速：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.btnOpenProjectFile);
            this.splitContainer1.Panel2.Controls.Add(this.butProjectFolder);
            this.splitContainer1.Size = new System.Drawing.Size(602, 303);
            this.splitContainer1.SplitterDistance = 187;
            this.splitContainer1.TabIndex = 42;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cmbTypeOfReducer);
            this.splitContainer2.Panel1.Controls.Add(this.cmbGradeOfReducer);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(602, 187);
            this.splitContainer2.SplitterDistance = 286;
            this.splitContainer2.TabIndex = 0;
            // 
            // cmbTypeOfReducer
            // 
            this.cmbTypeOfReducer.FormattingEnabled = true;
            this.cmbTypeOfReducer.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.cmbTypeOfReducer.Location = new System.Drawing.Point(133, 85);
            this.cmbTypeOfReducer.Name = "cmbTypeOfReducer";
            this.cmbTypeOfReducer.Size = new System.Drawing.Size(65, 20);
            this.cmbTypeOfReducer.TabIndex = 1;
            this.cmbTypeOfReducer.Text = "A";
            // 
            // cmbGradeOfReducer
            // 
            this.cmbGradeOfReducer.FormattingEnabled = true;
            this.cmbGradeOfReducer.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.cmbGradeOfReducer.Location = new System.Drawing.Point(133, 31);
            this.cmbGradeOfReducer.Name = "cmbGradeOfReducer";
            this.cmbGradeOfReducer.Size = new System.Drawing.Size(65, 20);
            this.cmbGradeOfReducer.TabIndex = 1;
            this.cmbGradeOfReducer.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "减速器结构形式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "减速器级数：";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(322, 44);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "确定";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(450, 44);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "zip压缩测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnOpenProjectFile
            // 
            this.btnOpenProjectFile.Location = new System.Drawing.Point(193, 44);
            this.btnOpenProjectFile.Name = "btnOpenProjectFile";
            this.btnOpenProjectFile.Size = new System.Drawing.Size(97, 23);
            this.btnOpenProjectFile.TabIndex = 1;
            this.btnOpenProjectFile.Text = "打开项目";
            this.btnOpenProjectFile.UseVisualStyleBackColor = true;
            this.btnOpenProjectFile.Click += new System.EventHandler(this.btnOpenProjectFile_Click);
            // 
            // butProjectFolder
            // 
            this.butProjectFolder.Location = new System.Drawing.Point(71, 44);
            this.butProjectFolder.Name = "butProjectFolder";
            this.butProjectFolder.Size = new System.Drawing.Size(97, 23);
            this.butProjectFolder.TabIndex = 0;
            this.butProjectFolder.Text = "新建项目";
            this.butProjectFolder.UseVisualStyleBackColor = true;
            this.butProjectFolder.Click += new System.EventHandler(this.butProjectFolder_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "减速器.xml";
            this.openFileDialog.Filter = "项目文件|*.xml|C#文件|*.cs|所有文件|*.*";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "设置项目的目录";
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog.SelectedPath = "D:\\";
            // 
            // FrmOverallDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 303);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmOverallDesign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "减速器选型";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtN;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTotalRate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox cmbTypeOfReducer;
        private System.Windows.Forms.ComboBox cmbGradeOfReducer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butProjectFolder;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnOpenProjectFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnClose;
    }
}