using System;

namespace ReducerDesign.CommonClass
{
    class BasicGeraCalculte
    {/// <summary>
        /// 变位中心距
        /// </summary>
        public double A1;
        /// <summary>
        /// 模数
        /// </summary>
        public double Mn;
        /// <summary>
        /// 小齿轮齿数
        /// </summary>
        public int Z1;
        /// <summary>
        /// 大齿轮齿数
        /// </summary>
        public int Z2;
        /// <summary>
        /// 螺旋角
        /// </summary>
        public double  Lxj;
        /// <summary>
        /// 小齿轮变位系数
        /// </summary>
        public double X1;
        /// <summary>
        /// 压力角
        /// </summary>
        public double An;
        /// <summary>
        /// 大齿轮齿宽
        /// </summary>
        public int B;
        /// <summary>
        /// 端面压力角
        /// </summary>
        private double _dmylj;
        /// <summary>
        /// 啮合角
        /// </summary>
        private double _nhj;
        /// <summary>
        /// 未变位中心距
        /// </summary>
        private double _wbwzxj;
        /// <summary>
        /// 总变位系数
        /// </summary>
        private double _xn;
        /// <summary>
        /// 大齿轮变位系数
        /// </summary>
        private double _x2;
        /// <summary>
        /// 中心距变动系数
        /// </summary>
        private double _zxjbdxs;
        /// <summary>
        /// 小齿轮齿顶高
        /// </summary>
        private double _ha1;
        /// <summary>
        /// 大齿轮齿顶高
        /// </summary>
        private double _ha2;
        /// <summary>
        /// 小齿轮齿根高
        /// </summary>
        private double _hf1;
        /// <summary>
        /// 大齿轮齿根高
        /// </summary>
        private double _hf2;
        /// <summary>
        /// 齿顶高系数
        /// </summary>
        public double H;
        /// <summary>
        /// 顶隙系数
        /// </summary>
        public double C;
        /// <summary>
        /// 小齿轮分度圆直径
        /// </summary>
        private double _d1;
        /// <summary>
        /// 小齿轮齿顶圆直径
        /// </summary>
        private double _da1;
        /// <summary>
        /// 大齿轮分度圆直径
        /// </summary>
        private double _d2;
        /// <summary>
        /// 大齿轮齿顶圆直径
        /// </summary>
        private double _da2;
        /// <summary>
        /// 小齿轮假想齿数
        /// </summary>
        private double _xjz;
        /// <summary>
        /// 大齿轮假象齿数
        /// </summary>
        private double _djz;
        /// <summary>
        /// 小齿轮跨齿数
        /// </summary>
        private int _k1;
        /// <summary>
        /// 大齿轮跨齿数
        /// </summary>
        private int _k2;
        /// <summary>
        /// 小齿轮公法线长度
        /// </summary>
        private double _xclgfx;
        /// <summary>
        /// 大齿轮公法线长度
        /// </summary>
        private double _dclgfx;
        /// <summary>
        /// 小齿轮齿顶圆压力角
        /// </summary>
        private double _xcdyylj;
        /// <summary>
        /// 大齿轮齿顶圆压力角
        /// </summary>
        private double _dcdyylj;
        /// <summary>
        /// 端面重合度
        /// </summary>
        private double _dmchd;
        /// <summary>
        /// 轴向重合度
        /// </summary>
        private double _zxchd;
        /// <summary>
        /// 总重合度
        /// </summary>
        private double _zchd;
        /// <summary>
        /// 齿顶高变动系数
        /// </summary>
        private double _dyn;
        /// <summary>
        /// 小齿根圆直径
        /// </summary>
        private double _df1;
        /// <summary>
        /// 大齿根圆直径
        /// </summary>
        private double df2;

        /// <summary>
        /// 计算大齿轮变位系数
        /// </summary>
        /// 

        public double X2
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = Zbwxs - X1;
            }
        }

        /// <summary>
        /// 中心距变动系数
        /// </summary>
        public double Yn
        {
            get
            {
                return _zxjbdxs;
            }
            set
            {
                _zxjbdxs = (A1 - A) / Mn;
            }
        }

        /// <summary>
        /// 端面压力角计算
        /// </summary>
        public double AlphaT
        {
            get
            {
                return _dmylj;
            }
            set
            {     //端面压力角
                _dmylj = Math.Atan(Math.Tan(An * Math.PI / 180) / Math.Cos(Lxj * Math.PI / 180));
            }
        }

        /// <summary>
        /// 啮合角计算
        /// </summary>
        public double AlphaT1
        {
            get
            {
                return _nhj;
            }
            set
            {//啮合角a
                _nhj = Math.Acos(A * Math.Cos(AlphaT) / A1);
            }
        }

        /// <summary>
        /// 未变位中心距计算
        /// </summary>
        public double A
        {
            get
            {
                return _wbwzxj;
            }
            set
            {
                _wbwzxj = Mn * (Z1 + Z2) / (2 * Math.Cos(Lxj * Math.PI / 180));//未变位中心距
            }
        }

        /// <summary>
        /// 总变位系数计算
        /// </summary>
        public double Zbwxs
        {
            get
            {
                return _xn;
            }
            set
            {
                double temp = Math.Tan(AlphaT1) - AlphaT1 - (Math.Tan(AlphaT) - AlphaT);
                _xn = (Z1 + Z2) * temp / (2 * Math.Tan(An * Math.PI / 180));
            }
        }

        /// <summary>
        /// 小齿轮齿顶高
        /// </summary>
        public double Ha1
        {
            get
            {
                return _ha1;
            }
            set
            {
                _ha1 = (H + X1 - (Zbwxs - Yn)) * Mn;
            }
        }

        /// <summary>
        /// 大齿轮齿顶高
        /// </summary>
        public double Ha2
        {
            get
            {
                return _ha2;
            }
            set
            {
                _ha2 = (H + X2 - (Zbwxs - Yn)) * Mn;
            }
        }

        /// <summary>
        /// 小齿轮齿根高
        /// </summary>
        public double Hf1
        {
            get
            {
                return _hf1;
            }
            set
            {
                _hf1 = (H + C - X1) * Mn;
            }
        }

        /// <summary>
        /// 大齿轮齿根高
        /// </summary>
        public double Hf2
        {
            get
            {
                return _hf2;
            }
            set
            {
                _hf2 = (H + C - X2) * Mn;
            }
        }

        /// <summary>
        /// 小齿轮分度圆直径
        /// </summary>
        public double D1
        {
            get
            {
                return _d1;
            }
            set
            {
                _d1 = Z1 * Mn / Math.Cos(Lxj * Math.PI / 180);
            }
        }

        /// <summary>
        /// 小齿轮齿顶圆直径
        /// </summary>
        public double Da1
        {
            get
            {
                return _da1;
            }
            set
            {
                _da1 = D1 + 2 * Ha1;
            }
        }

        /// <summary>
        /// 大齿轮分度圆直径
        /// </summary>
        public double D2
        {
            get
            {
                return _d2;
            }
            set
            {
                _d2 = Z2 * Mn / Math.Cos(Lxj * Math.PI / 180);
            }
        }

        /// <summary>
        /// 大齿轮齿顶圆直径
        /// </summary>
        public double Da2
        {
            get
            {
                return _da2;
            }
            set
            {
                _da2 = D2 + 2 * Ha2;
            }
        }

        /// <summary>
        /// 小齿轮假象齿数计算
        /// </summary>
        private double Xjz
        {
            get
            {
                return _xjz;
            }
            set
            {
                _xjz = Z1 * (Math.Tan(AlphaT) - AlphaT) / (Math.Tan(An * Math.PI / 180) - An * Math.PI / 180);
            }
        }

        /// <summary>
        /// 大齿轮假象齿数计算
        /// </summary>
        private double Djz
        {
            get
            {
                return _djz;
            }
            set
            {
                _djz = Z2 * (Math.Tan(AlphaT) - AlphaT) / (Math.Tan(An * Math.PI / 180) - An * Math.PI / 180);
            }
        }

        /// <summary>
        /// 小齿轮跨齿数计算
        /// </summary>
        public int K1
        {
            get
            {
                return _k1;
            }
            set
            {
                _k1 = (int)Math.Round(An * Xjz / 180 + 0.5 + 2 * X1 / (Math.Tan(An * Math.PI / 180) * Math.PI));
            }
        }

        /// <summary>
        /// 大齿轮跨齿数计算
        /// </summary>
        public int K2
        {
            get
            {
                return _k2;
            }
            set
            {
                _k2 = (int)Math.Round(An * Djz / 180 + 0.5 + 2 * X2 / (Math.Tan(An * Math.PI / 180) * Math.PI));
            }
        }

        /// <summary>
        /// 小齿轮公法线计算
        /// </summary>
        public double W1
        {
            get
            {
                return _xclgfx;
            }
            set
            {
                _xclgfx = Mn * Math.Cos(An * Math.PI / 180) * (Math.PI * (K1 - 0.5) + Xjz * (Math.Tan(An * Math.PI / 180) - An * Math.PI / 180) + 2 * X1 * Math.Tan(An * Math.PI / 180));
            }
        }

        /// <summary>
        /// 大齿轮公法线计算
        /// </summary>
        public double W2
        {
            get
            {
                return _dclgfx;
            }
            set
            {
                _dclgfx = Mn * Math.Cos(An * Math.PI / 180) * (Math.PI * (K2 - 0.5) + Djz * (Math.Tan(An * Math.PI / 180) - An * Math.PI / 180) + 2 * X2 * Math.Tan(An * Math.PI / 180));
            }
        }

        /// <summary>
        /// 小齿轮齿顶圆压力角计算
        /// </summary>
        private double AlphaAt1
        {
            get
            {
                return _xcdyylj;
            }
            set
            {
                _xcdyylj = Math.Acos(D1 * Math.Cos(AlphaT) / Da1);
            }
        }

        /// <summary>
        /// 大齿轮齿顶圆压力角计算
        /// </summary>
        private double AlphaAt2
        {
            get
            {
                return _dcdyylj;
            }
            set
            {
                _dcdyylj = Math.Acos(D2 * Math.Cos(AlphaT) / Da2);
            }
        }

        /// <summary>
        /// 端面重合度计算
        /// </summary>
        public double Ea
        {
            get
            {
                return _dmchd;
            }
            set
            {
                _dmchd = (Z1 * (Math.Tan(AlphaAt1) - Math.Tan(AlphaT1)) + Z2 * (Math.Tan(AlphaAt2) - Math.Tan(AlphaT1))) / (2 * Math.PI);
            }
        }

        /// <summary>
        /// 轴向重合度
        /// </summary>
        public double Eb
        {
            get
            {
                return _zxchd;
            }
            set
            {
                _zxchd = B * Math.Sin(Lxj * Math.PI / 180) / (Math.PI * Mn);
            }
        }

        /// <summary>
        /// 总重合度计算
        /// </summary>
        public double E
        {
            get
            {
                return _zchd;
            }
            set
            {
                _zchd = Ea + Eb;
            }
        }

        /// <summary>
        /// 齿顶高变动系数
        /// </summary>
        public double Dyn
        {
            get
            {
                return _dyn;
            }
            set
            {
                _dyn = Zbwxs - Yn;
            }
        }

        /// <summary>
        /// 小齿轮齿根圆直径
        /// </summary>
        public double Df1
        {
            get
            {
                return _df1;
            }
            set
            {
                _df1 = D1 - 2 * Hf1;
            }
        }

        /// <summary>
        /// 大齿根圆直径
        /// </summary>
        public double Df2
        {
            get
            {
                return df2;
            }
            set
            {
                df2 = D2 - 2 * _hf2;
            }
        }

        /// <summary>
        /// 齿轮中间变量计算
        /// </summary>
        public void Nbjs()
        {
            A = 1;
            AlphaT = 1;
            AlphaT1 = 1;
            Zbwxs = 1;
            Yn = 1;
            X2 = 1;
            Dyn = 1;
            D1 = 1;
            D2 = 1;
            Ha1 = 1;
            Ha2 = 1;
            Da1 = 1;
            Da2 = 1;
            Hf1 = 1;
            Hf2 = 1;
            Df1 = 1;
            Df2 = 1;
            Xjz = 1;
            Djz = 1;
            K1 = 1;
            K2 = 1;
            W1 = 1;
            W2 = 1;
            AlphaAt1 = 1;
            AlphaAt2 = 1;
            Ea = 1;
            Eb = 1;
            E = 1;
        }






    
    
    }
}
