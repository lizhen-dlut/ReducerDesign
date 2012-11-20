namespace ReducerDesign
{
    partial class MainForm
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
            System.Windows.Forms.Label 级数Label;
            System.Windows.Forms.Label 装配形式Label;
            this.级数TextBox = new System.Windows.Forms.TextBox();
            this.装配形式TextBox = new System.Windows.Forms.TextBox();
            级数Label = new System.Windows.Forms.Label();
            装配形式Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // 级数Label
            // 
            级数Label.AutoSize = true;
            级数Label.Location = new System.Drawing.Point(45, 82);
            级数Label.Name = "级数Label";
            级数Label.Size = new System.Drawing.Size(35, 12);
            级数Label.TabIndex = 1;
            级数Label.Text = "级数:";
            // 
            // 装配形式Label
            // 
            装配形式Label.AutoSize = true;
            装配形式Label.Location = new System.Drawing.Point(48, 116);
            装配形式Label.Name = "装配形式Label";
            装配形式Label.Size = new System.Drawing.Size(59, 12);
            装配形式Label.TabIndex = 3;
            装配形式Label.Text = "装配形式:";
            // 
            // 级数TextBox
            // 
            this.级数TextBox.Location = new System.Drawing.Point(86, 79);
            this.级数TextBox.Name = "级数TextBox";
            this.级数TextBox.Size = new System.Drawing.Size(100, 21);
            this.级数TextBox.TabIndex = 2;
            // 
            // 装配形式TextBox
            // 
            this.装配形式TextBox.Location = new System.Drawing.Point(113, 113);
            this.装配形式TextBox.Name = "装配形式TextBox";
            this.装配形式TextBox.Size = new System.Drawing.Size(100, 21);
            this.装配形式TextBox.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 470);
            this.Controls.Add(装配形式Label);
            this.Controls.Add(this.装配形式TextBox);
            this.Controls.Add(级数Label);
            this.Controls.Add(this.级数TextBox);
            this.Name = "MainForm";
            this.Text = "减速机基本信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox 级数TextBox;
        private System.Windows.Forms.TextBox 装配形式TextBox;
    }
}