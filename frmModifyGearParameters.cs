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
    public partial class frmModifyGearParameters : Form
    {
        public string strjishu;//级数
        public string strmoshu;//法向模数
        public string strzhongxinju;//中心距
        public string strchishu1;//齿数1
        public string strchishu2;//齿数2
        public string strluoxuanjiao;//螺旋角
        public string stryalijiao;//法向压力角
        public string strbianweixishu1;//法向变位系数1
        public string strbianweixishu2;//法向变位系数2
        public string strchidinggaoxishu1;//齿顶高系数1
        public string strchidinggaoxishu2;//齿顶高系数2
        public string strdingxixishu1;//顶隙系数1
        public string strdingxixishu2;//顶隙系数2
        public string strchikuan;//齿宽

        public frmModifyGearParameters()
        {
            InitializeComponent();

        }

        private void frmModifyGearParameters_Load(object sender, EventArgs e)
        {

            this.txtjishu.Text = strjishu;
            this.txtmoshu.Text = strmoshu;
            this.txtzhongxinju.Text = strzhongxinju;
            this.txtchishu1.Text = strchishu1;
            this.txtchishu2.Text = strchishu2;
            this.txtluoxuanjiao.Text = strluoxuanjiao;
            this.txtyalijiao.Text = stryalijiao;
            this.txtbianweixishu1.Text = strbianweixishu1;
            this.txtbianweixishu2.Text = strbianweixishu2;
            this.txtchidinggaoxishu1.Text = strchidinggaoxishu1;
            this.txtchidinggaoxishu2.Text = strchidinggaoxishu2;
            this.txtdingxixishu1.Text = strdingxixishu1;
            this.txtdingxixishu2.Text = strdingxixishu2;
            this.txtchikuan.Text = strchikuan;


        }

        //确定
        private void btnOK_Click(object sender, EventArgs e)
        {
            //判断输入格式的正确性
            if (CheckInput() == true)
            {
                strjishu = this.txtjishu.Text.ToString();
                strmoshu = this.txtmoshu.Text.ToString();
                strzhongxinju = this.txtzhongxinju.Text.ToString();
                strchishu1 = this.txtchishu1.Text.ToString();
                strchishu2 = this.txtchishu2.Text.ToString();
                strluoxuanjiao = this.txtluoxuanjiao.Text.ToString();
                stryalijiao = this.txtyalijiao.Text.ToString();
                strbianweixishu1 = this.txtbianweixishu1.Text.ToString();
                strbianweixishu2 = this.txtbianweixishu2.Text.ToString();
                strchidinggaoxishu1 = this.txtchidinggaoxishu1.Text.ToString();
                strchidinggaoxishu2 = this.txtchidinggaoxishu2.Text.ToString();
                strdingxixishu1 = this.txtdingxixishu1.Text.ToString();
                strdingxixishu2 = this.txtdingxixishu2.Text.ToString();
                strchikuan = this.txtchikuan.Text.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                return;
        }


        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //检查输入的格式是否正确
        private Boolean CheckInput()
        {
            //判断不能为空
            //pandu判断
            if (this.txtmoshu.Text == "")
            {
                MessageBox.Show("请输入法向模数");
                return false;
            }
            if (this.txtzhongxinju.Text == "")
            {
                MessageBox.Show("请输入中心距");
                return false;
            }
            if (this.txtchishu1.Text == "")
            {
                MessageBox.Show("请输入齿数1");
                return false;
            }
            if (this.txtchishu2.Text == "")
            {
                MessageBox.Show("请输入齿数2");
                return false;
            }
            if (this.txtluoxuanjiao.Text == "")
            {
                MessageBox.Show("请输入螺旋角");
                return false;
            }
            if (this.txtyalijiao.Text == "")
            {
                MessageBox.Show("请输入法向压力角");
                return false;
            }
            if (this.txtbianweixishu1.Text == "")
            {
                MessageBox.Show("请输入法向变位系数1");
                return false;
            }
            if (this.txtbianweixishu2.Text == "")
            {
                MessageBox.Show("请输入法向变位系数2");
                return false;
            }
            if (this.txtchidinggaoxishu1.Text == "")
            {
                MessageBox.Show("请输入齿顶高系数1");
                return false;
            }
            if (this.txtchidinggaoxishu2.Text == "")
            {
                MessageBox.Show("请输入齿顶高系数2");
                return false;
            }
            if (this.txtdingxixishu1.Text == "")
            {
                MessageBox.Show("请输入顶隙系数1");
                return false;
            }
            if (this.txtdingxixishu2.Text == "")
            {
                MessageBox.Show("请输入顶隙系数2");
                return false;
            }
            if (this.txtchikuan.Text == "")
            {
                MessageBox.Show("请输入齿宽");
                return false;
            }

            //判断输入的是否为数字
            if (IsNumeric(this.txtmoshu.Text.ToString()) == false)
            {
                MessageBox.Show("请输入正确的法向模数");
                return false;
            }
            if (IsNumeric(this.txtzhongxinju.Text.ToString()) == false)
            {
                MessageBox.Show("请输入中心距");
                return false;
            }
            if (IsNumeric(this.txtchishu1.Text.ToString()) == false)
            {
                MessageBox.Show("请输入齿数1");
                return false;
            }
            if (IsNumeric(this.txtchishu2.Text.ToString()) == false)
            {
                MessageBox.Show("请输入齿数2");
                return false;
            }
            if (IsNumeric(this.txtluoxuanjiao.Text.ToString()) == false)
            {
                MessageBox.Show("请输入螺旋角");
                return false;
            }
            if (IsNumeric(this.txtyalijiao.Text.ToString()) == false)
            {
                MessageBox.Show("请输入法向压力角");
                return false;
            }
            if (IsNumeric(this.txtbianweixishu1.Text.ToString()) == false)
            {
                MessageBox.Show("请输入法向变位系数1");
                return false;
            }
            if (IsNumeric(this.txtbianweixishu2.Text.ToString()) == false)
            {
                MessageBox.Show("请输入法向变位系数2");
                return false;
            }
            if (IsNumeric(this.txtchidinggaoxishu1.Text.ToString()) == false)
            {
                MessageBox.Show("请输入齿顶高系数1");
                return false;
            }
            if (IsNumeric(this.txtchidinggaoxishu2.Text.ToString()) == false)
            {
                MessageBox.Show("请输入齿顶高系数2");
                return false;
            }
            if (IsNumeric(this.txtdingxixishu1.Text.ToString()) == false)
            {
                MessageBox.Show("请输入顶隙系数1");
                return false;
            }
            if (IsNumeric(this.txtdingxixishu2.Text.ToString()) == false)
            {
                MessageBox.Show("请输入顶隙系数2");
                return false;
            }
            if (IsNumeric(this.txtchikuan.Text.ToString()) == false)
            {
                MessageBox.Show("请输入齿宽");
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
