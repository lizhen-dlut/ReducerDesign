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
    public partial class frmModifyjiancaoParameters : Form
    {

        public string strjiancaoxuhao;//键槽序号
        public string strjiancaoweizhi;//键槽位置
        public string strjianchang;//长度
        public string strdingwei;//定位
        public string strjiankuan;//倒角
        public string strjianshen;//圆角

        public frmModifyjiancaoParameters()
        {
            InitializeComponent();
        }

        private void frmModifyjiancaoParameters_Load(object sender, EventArgs e)
        {
            this.txtjiancaoxuhao.Text = strjiancaoxuhao;
            this.txtjiancaoweizhi.Text = strjiancaoweizhi;
            this.txtjianchang.Text = strjianchang;
            this.txtdingwei.Text = strdingwei;
            this.txtjiankuan.Text = strjiankuan;
            this.txtjianshen.Text = strjianshen;

        }


        #region 确定

        private void btnOK_Click(object sender, EventArgs e)
        {
            //判断输入格式的正确性
            if (CheckInput() == true)
            {
                strjiancaoxuhao = this.txtjiancaoxuhao.Text;
                strjiancaoweizhi = this.txtjiancaoweizhi.Text;
                strjianchang = this.txtjianchang.Text;
                strdingwei = this.txtdingwei.Text;
                strjiankuan = this.txtjiankuan.Text;
                strjianshen = this.txtjianshen.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                return;
        }

        #endregion


        #region 取消

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion


        //检查输入的格式是否正确
        private Boolean CheckInput()
        {
            //判断不能为空
            if (this.txtjiancaoxuhao.Text == "")
            {
                MessageBox.Show("请输入键槽序号！");
                return false;
            }
            if (this.txtjiancaoweizhi.Text == "")
            {
                MessageBox.Show("请输入键槽位置！");
                return false;
            }
            if (this.txtjianchang.Text == "")
            {
                MessageBox.Show("请输入键长!");
                return false;
            }
            if (this.txtdingwei.Text == "")
            {
                MessageBox.Show("请输入定位!");
                return false;
            }

            if (this.txtjiankuan.Text == "")
            {
                MessageBox.Show("请输入键宽！");
                return false;
            }

            if (this.txtjianshen.Text == "")
            {
                MessageBox.Show("请输入键深！");
                return false;
            }

            //判断输入的是否为数字
            if (IsNumeric(this.txtjianchang.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的键长！");
                return false;
            }
            if (IsNumeric(this.txtjiankuan.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的键宽！");
                return false;
            }
            if (IsNumeric(this.txtdingwei.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的定位！");
                return false;
            }
            if (IsNumeric(this.txtjianshen.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的键深！");
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

     



    }
}
