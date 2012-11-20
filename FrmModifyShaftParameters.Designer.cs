namespace ReducerDesign
{
    partial class FrmModifyShaftParameters
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
            this.btnOK = new System.Windows.Forms.Button();
            this.benCancel = new System.Windows.Forms.Button();
            this.txtyuanjiao = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtdaojiao = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtchangdu = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtzhijing = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtzhouduan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(90, 225);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 50;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // benCancel
            // 
            this.benCancel.Location = new System.Drawing.Point(231, 225);
            this.benCancel.Name = "benCancel";
            this.benCancel.Size = new System.Drawing.Size(75, 23);
            this.benCancel.TabIndex = 51;
            this.benCancel.Text = "取消";
            this.benCancel.UseVisualStyleBackColor = true;
            this.benCancel.Click += new System.EventHandler(this.benCancel_Click_1);
            // 
            // txtyuanjiao
            // 
            this.txtyuanjiao.Location = new System.Drawing.Point(288, 137);
            this.txtyuanjiao.Name = "txtyuanjiao";
            this.txtyuanjiao.Size = new System.Drawing.Size(75, 21);
            this.txtyuanjiao.TabIndex = 49;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(213, 140);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 48;
            this.label14.Text = "圆角（°）";
            // 
            // txtdaojiao
            // 
            this.txtdaojiao.Location = new System.Drawing.Point(288, 92);
            this.txtdaojiao.Name = "txtdaojiao";
            this.txtdaojiao.Size = new System.Drawing.Size(75, 21);
            this.txtdaojiao.TabIndex = 47;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(213, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 46;
            this.label13.Text = "倒角（°）";
            // 
            // txtchangdu
            // 
            this.txtchangdu.Location = new System.Drawing.Point(109, 137);
            this.txtchangdu.Name = "txtchangdu";
            this.txtchangdu.Size = new System.Drawing.Size(75, 21);
            this.txtchangdu.TabIndex = 45;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "长度（mm）";
            // 
            // txtzhijing
            // 
            this.txtzhijing.Location = new System.Drawing.Point(109, 92);
            this.txtzhijing.Name = "txtzhijing";
            this.txtzhijing.Size = new System.Drawing.Size(75, 21);
            this.txtzhijing.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 42;
            this.label2.Text = "直径（mm）";
            // 
            // txtzhouduan
            // 
            this.txtzhouduan.Enabled = false;
            this.txtzhouduan.Location = new System.Drawing.Point(109, 32);
            this.txtzhouduan.Name = "txtzhouduan";
            this.txtzhouduan.Size = new System.Drawing.Size(75, 21);
            this.txtzhouduan.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "轴段";
            // 
            // frmModifyShaftParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(398, 284);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.benCancel);
            this.Controls.Add(this.txtyuanjiao);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtdaojiao);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtchangdu);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtzhijing);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtzhouduan);
            this.Controls.Add(this.label1);
            this.Name = "FrmModifyShaftParameters";
            this.Text = "frmModifyShaftParameters";
            this.Load += new System.EventHandler(this.frmModifyShaftParameters_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button benCancel;
        private System.Windows.Forms.TextBox txtyuanjiao;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtdaojiao;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtchangdu;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtzhijing;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtzhouduan;
        private System.Windows.Forms.Label label1;
    }
}