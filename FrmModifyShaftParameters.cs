using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ReducerDesign
{
    public partial class FrmModifyShaftParameters : Form
    {
        //tesffsdf
        public string strzhouduan;//轴段
        public string strzhijing;//直径
        public string strchangdu;//长度
        public string strdaojiao;//倒角
        public string stryuanjiao;//圆角

        public FrmModifyShaftParameters()
        {
            InitializeComponent();
        }

        private void frmModifyShaftParameters_Load(object sender, EventArgs e)
        {

            this.txtzhouduan.Text = strzhouduan;
            this.txtzhijing.Text = strzhijing;
            this.txtchangdu.Text = strchangdu;
            this.txtdaojiao.Text = strdaojiao;
            this.txtyuanjiao.Text = stryuanjiao;

        }


        //确定按钮
        private void btnOK_Click(object sender, EventArgs e)
        {
            //判断输入格式的正确性
            if (CheckInput() == true)
            {
                strzhouduan = this.txtzhouduan.Text;
                strzhijing = this.txtzhijing.Text;
                strchangdu = this.txtchangdu.Text;
                strdaojiao = this.txtdaojiao.Text;
                stryuanjiao = this.txtyuanjiao.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                return;
        }


        //取消按钮
        private void benCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //检查输入的格式是否正确
        private Boolean CheckInput()
        {
            //判断不能为空
            if (this.txtzhijing.Text == "")
            {
                MessageBox.Show("请输入直径");
                return false;
            }
            if (this.txtchangdu.Text == "")
            {
                MessageBox.Show("请输入长度");
                return false;
            }
            if (this.txtdaojiao.Text == "")
            {
                MessageBox.Show("请输入倒角");
                return false;
            }
            if (this.txtyuanjiao.Text == "")
            {
                MessageBox.Show("请输入圆角");
                return false;
            }

            //判断输入的是否为数字
            if (IsNumeric(this.txtzhijing.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的直径");
                return false;
            }
            if (IsNumeric(this.txtchangdu.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的长度");
                return false;
            }

            if (IsNumeric(this.txtdaojiao.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的倒角");
                return false;
            }

            if (IsNumeric(this.txtyuanjiao.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的圆角");
                return false;
            }

            return true;

        }


        #region 判断字符类型

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        public static bool IsUnsign(string value)
        {
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }

        #endregion

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            //判断输入格式的正确性
            if (CheckInput() == true)
            {
                strzhouduan = this.txtzhouduan.Text;
                strzhijing = this.txtzhijing.Text;
                strchangdu = this.txtchangdu.Text;
                strdaojiao = this.txtdaojiao.Text;
                stryuanjiao = this.txtyuanjiao.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                return;
        }

        private void benCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
