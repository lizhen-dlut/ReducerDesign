namespace ReducerDesign
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.escButton = new System.Windows.Forms.Button();
            this.confirmButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.GraphControl = new ZedGraph.ZedGraphControl();
            this.transmissionSeriesLabel = new System.Windows.Forms.Label();
            this.tbxTransmissionSeries = new System.Windows.Forms.TextBox();
            this.totalTransmissionRatioLabel = new System.Windows.Forms.Label();
            this.tbxTotalTransmissionRatio = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.jishu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moshu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zhongxinju = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chishuz1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chishuz2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.luoxuanjiao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bianweixishu1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bianweixishu2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yalijiao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chikuan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dgvDiameter = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiameter)).BeginInit();
            this.SuspendLayout();
            // 
            // escButton
            // 
            this.escButton.Location = new System.Drawing.Point(351, 603);
            this.escButton.Name = "escButton";
            this.escButton.Size = new System.Drawing.Size(75, 23);
            this.escButton.TabIndex = 34;
            this.escButton.Text = "取消";
            this.escButton.UseVisualStyleBackColor = true;
            this.escButton.Click += new System.EventHandler(this.escButton_Click);
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(212, 603);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(92, 23);
            this.confirmButton.TabIndex = 33;
            this.confirmButton.Text = "绘制齿顶圆";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(455, 603);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 32;
            this.clearButton.Text = "清空";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // GraphControl
            // 
            this.GraphControl.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.GraphControl.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.GraphControl.IsAutoScrollRange = false;
            this.GraphControl.IsEnableHEdit = false;
            this.GraphControl.IsEnableHPan = true;
            this.GraphControl.IsEnableHZoom = true;
            this.GraphControl.IsEnableVEdit = false;
            this.GraphControl.IsEnableVPan = true;
            this.GraphControl.IsEnableVZoom = true;
            this.GraphControl.IsPrintFillPage = true;
            this.GraphControl.IsPrintKeepAspectRatio = true;
            this.GraphControl.IsScrollY2 = false;
            this.GraphControl.IsShowContextMenu = true;
            this.GraphControl.IsShowCopyMessage = true;
            this.GraphControl.IsShowCursorValues = false;
            this.GraphControl.IsShowHScrollBar = false;
            this.GraphControl.IsShowPointValues = false;
            this.GraphControl.IsShowVScrollBar = false;
            this.GraphControl.IsZoomOnMouseCenter = false;
            this.GraphControl.Location = new System.Drawing.Point(619, 60);
            this.GraphControl.Name = "GraphControl";
            this.GraphControl.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.GraphControl.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.GraphControl.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.GraphControl.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.GraphControl.PointDateFormat = "g";
            this.GraphControl.PointValueFormat = "G";
            this.GraphControl.ScrollMaxX = 0D;
            this.GraphControl.ScrollMaxY = 0D;
            this.GraphControl.ScrollMaxY2 = 0D;
            this.GraphControl.ScrollMinX = 0D;
            this.GraphControl.ScrollMinY = 0D;
            this.GraphControl.ScrollMinY2 = 0D;
            this.GraphControl.Size = new System.Drawing.Size(473, 440);
            this.GraphControl.TabIndex = 35;
            this.GraphControl.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.GraphControl.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.GraphControl.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.GraphControl.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.GraphControl.ZoomStepFraction = 0.1D;
            this.GraphControl.Load += new System.EventHandler(this.GraphControl_Load);
            // 
            // transmissionSeriesLabel
            // 
            this.transmissionSeriesLabel.AutoSize = true;
            this.transmissionSeriesLabel.Location = new System.Drawing.Point(13, 22);
            this.transmissionSeriesLabel.Name = "transmissionSeriesLabel";
            this.transmissionSeriesLabel.Size = new System.Drawing.Size(59, 12);
            this.transmissionSeriesLabel.TabIndex = 36;
            this.transmissionSeriesLabel.Text = "传动级数:";
            // 
            // tbxTransmissionSeries
            // 
            this.tbxTransmissionSeries.Location = new System.Drawing.Point(78, 18);
            this.tbxTransmissionSeries.Name = "tbxTransmissionSeries";
            this.tbxTransmissionSeries.Size = new System.Drawing.Size(71, 21);
            this.tbxTransmissionSeries.TabIndex = 37;
            // 
            // totalTransmissionRatioLabel
            // 
            this.totalTransmissionRatioLabel.AutoSize = true;
            this.totalTransmissionRatioLabel.Location = new System.Drawing.Point(158, 22);
            this.totalTransmissionRatioLabel.Name = "totalTransmissionRatioLabel";
            this.totalTransmissionRatioLabel.Size = new System.Drawing.Size(59, 12);
            this.totalTransmissionRatioLabel.TabIndex = 38;
            this.totalTransmissionRatioLabel.Text = "总传动比:";
            // 
            // tbxTotalTransmissionRatio
            // 
            this.tbxTotalTransmissionRatio.Location = new System.Drawing.Point(223, 18);
            this.tbxTotalTransmissionRatio.Name = "tbxTotalTransmissionRatio";
            this.tbxTotalTransmissionRatio.Size = new System.Drawing.Size(91, 21);
            this.tbxTotalTransmissionRatio.TabIndex = 39;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.jishu,
            this.moshu,
            this.zhongxinju,
            this.chishuz1,
            this.chishuz2,
            this.luoxuanjiao,
            this.bianweixishu1,
            this.bianweixishu2,
            this.yalijiao,
            this.chikuan});
            this.dataGridView.Location = new System.Drawing.Point(12, 60);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(555, 211);
            this.dataGridView.TabIndex = 40;
            // 
            // jishu
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.jishu.DefaultCellStyle = dataGridViewCellStyle1;
            this.jishu.HeaderText = "级数";
            this.jishu.Name = "jishu";
            this.jishu.ToolTipText = "减速机的第几级";
            this.jishu.Width = 70;
            // 
            // moshu
            // 
            this.moshu.HeaderText = "模数";
            this.moshu.Name = "moshu";
            this.moshu.Width = 54;
            // 
            // zhongxinju
            // 
            this.zhongxinju.HeaderText = "中心距";
            this.zhongxinju.Name = "zhongxinju";
            this.zhongxinju.Width = 66;
            // 
            // chishuz1
            // 
            this.chishuz1.HeaderText = "齿数 Z1";
            this.chishuz1.Name = "chishuz1";
            this.chishuz1.Width = 72;
            // 
            // chishuz2
            // 
            this.chishuz2.HeaderText = "齿数 Z2";
            this.chishuz2.Name = "chishuz2";
            this.chishuz2.Width = 72;
            // 
            // luoxuanjiao
            // 
            this.luoxuanjiao.HeaderText = "螺旋角";
            this.luoxuanjiao.Name = "luoxuanjiao";
            this.luoxuanjiao.Width = 66;
            // 
            // bianweixishu1
            // 
            this.bianweixishu1.HeaderText = "变位系数 x1";
            this.bianweixishu1.Name = "bianweixishu1";
            this.bianweixishu1.Width = 96;
            // 
            // bianweixishu2
            // 
            this.bianweixishu2.HeaderText = "变位系数 x2";
            this.bianweixishu2.Name = "bianweixishu2";
            this.bianweixishu2.Width = 96;
            // 
            // yalijiao
            // 
            this.yalijiao.HeaderText = "压力角";
            this.yalijiao.Name = "yalijiao";
            this.yalijiao.Width = 66;
            // 
            // chikuan
            // 
            this.chikuan.HeaderText = "齿宽";
            this.chikuan.Name = "chikuan";
            this.chikuan.Width = 54;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(78, 603);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 23);
            this.btnLoad.TabIndex = 41;
            this.btnLoad.Text = "计算减速机参数";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dgvDiameter
            // 
            this.dgvDiameter.AllowUserToAddRows = false;
            this.dgvDiameter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiameter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgvDiameter.Location = new System.Drawing.Point(12, 316);
            this.dgvDiameter.Name = "dgvDiameter";
            this.dgvDiameter.ReadOnly = true;
            this.dgvDiameter.RowHeadersVisible = false;
            this.dgvDiameter.RowTemplate.Height = 23;
            this.dgvDiameter.Size = new System.Drawing.Size(555, 197);
            this.dgvDiameter.TabIndex = 75;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "级数";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "分度圆直径1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "分度圆直径2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "齿顶圆直径1";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "齿顶圆直径2";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 706);
            this.Controls.Add(this.dgvDiameter);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.transmissionSeriesLabel);
            this.Controls.Add(this.tbxTransmissionSeries);
            this.Controls.Add(this.totalTransmissionRatioLabel);
            this.Controls.Add(this.tbxTotalTransmissionRatio);
            this.Controls.Add(this.GraphControl);
            this.Controls.Add(this.escButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.clearButton);
            this.Name = "Form1";
            this.Text = "减速机齿轮几何计算";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiameter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button escButton;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button clearButton;
        private ZedGraph.ZedGraphControl GraphControl;
        private System.Windows.Forms.Label transmissionSeriesLabel;
        private System.Windows.Forms.TextBox tbxTransmissionSeries;
        private System.Windows.Forms.Label totalTransmissionRatioLabel;
        private System.Windows.Forms.TextBox tbxTotalTransmissionRatio;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn jishu;
        private System.Windows.Forms.DataGridViewTextBoxColumn moshu;
        private System.Windows.Forms.DataGridViewTextBoxColumn zhongxinju;
        private System.Windows.Forms.DataGridViewTextBoxColumn chishuz1;
        private System.Windows.Forms.DataGridViewTextBoxColumn chishuz2;
        private System.Windows.Forms.DataGridViewTextBoxColumn luoxuanjiao;
        private System.Windows.Forms.DataGridViewTextBoxColumn bianweixishu1;
        private System.Windows.Forms.DataGridViewTextBoxColumn bianweixishu2;
        private System.Windows.Forms.DataGridViewTextBoxColumn yalijiao;
        private System.Windows.Forms.DataGridViewTextBoxColumn chikuan;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataGridView dgvDiameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;

    }
}