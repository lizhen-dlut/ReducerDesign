using System;

namespace ReducerDesign.CommonClass
{
    class ModificationCoefficient
    {
        /// <summary>
        /// 小齿数
        /// </summary>
        public double Z1;
        /// <summary>
        /// 大齿数
        /// </summary>
        public double Z2;
        /// <summary>
        /// 模数
        /// </summary>
        public double Mn;
        /// <summary>
        /// 压力角
        /// </summary>
        public double Alpha;
        /// <summary>
        /// 螺旋角
        /// </summary>
        public double Beta;
        /// <summary>
        /// 齿顶高系数
        /// </summary>
        public double Han;
        /// <summary>
        /// 变位中心距
        /// </summary>
        public double A1;
        /// <summary>
        /// 计算好的变位系数用于检验
        /// </summary>
        public double Mc1;

        /// <summary>
        /// 变位系数计算
        /// </summary>
        public double ModificationCalculation()
        {
            //分度圆直径
            double d1 = Z1 * Mn / Math.Cos(Beta * Math.PI / 180);
            double d2 = Z2 * Mn / Math.Cos(Beta * Math.PI / 180);
            //未变位中心距
            double a = Mn * (Z1 + Z2) / (2 * Math.Cos(Beta * Math.PI / 180));
            //端面压力角
            double alpha_t = Math.Atan(Math.Tan(Alpha * Math.PI / 180) / Math.Cos(Beta * Math.PI / 180));
            //中心距变动系数
            double yn = (A1 - a) / Mn;
            //啮合角
            double alpha_t1 = Math.Acos(a * Math.Cos(alpha_t) / A1);
            //基圆直径
            double db1 = d1 * Math.Cos(alpha_t);
            double db2 = d2 * Math.Cos(alpha_t);
            //总变位系数
            double temp = Math.Tan(alpha_t1) - alpha_t1 - (Math.Tan(alpha_t) - alpha_t);
            double xn = (Z1 + Z2) * temp / (2 * Math.Tan(Alpha * Math.PI / 180));
            //变为系数初值
            double x1 = 0.5 * xn;
            double x2 = 0.5 * xn;
            //齿顶高
            double ha1 = (Han + x1 - (xn - yn)) * Mn;
            double ha2 = (Han + x2 - (xn - yn)) * Mn;
            //齿顶圆直径
            double da1 = d1 + 2 * ha1;
            double da2 = d2 + 2 * ha2;
            //齿顶圆压力角
            double alpha_at1 = Math.Acos(db1 / da1);
            double alpha_at2 = Math.Acos(db2 / da2);
            //外啮合最大滑动率
            double etamax1 = (Z1 + Z2) * (Math.Tan(alpha_at2) - Math.Tan(alpha_t1)) / ((Z1 + Z2) * Math.Tan(alpha_t1) - Z2 * Math.Tan(alpha_at2));
            double etamax2 = (Z1 + Z2) * (Math.Tan(alpha_at1) - Math.Tan(alpha_t1)) / ((Z1 + Z2) * Math.Tan(alpha_t1) - Z1 * Math.Tan(alpha_at1));
            //二分法计算
            //给定变位范围
            double[] xlimit = new double[] { 0, xn };
            while (Math.Abs(etamax1 - etamax2) > 0.00001)
            {
                if (etamax1 > etamax2)
                { xlimit[0] = x1; }
                else
                { xlimit[1] = x1; }
                x1 = (xlimit[0] + xlimit[1]) / 2;
                x2 = xn - x1;
                //齿顶高
                ha1 = (Han + x1 - (xn - yn)) * Mn;
                ha2 = (Han + x2 - (xn - yn)) * Mn;
                //齿顶圆直径
                da1 = d1 + 2 * ha1;
                da2 = d2 + 2 * ha2;
                //齿顶圆压力角
                alpha_at1 = Math.Acos(db1 / da1);
                alpha_at2 = Math.Acos(db2 / da2);
                //外啮合最大滑动率
                etamax1 = (Z1 + Z2) * (Math.Tan(alpha_at2) - Math.Tan(alpha_t1)) / ((Z1 + Z2) * Math.Tan(alpha_t1) - Z2 * Math.Tan(alpha_at2));
                etamax2 = (Z1 + Z2) * (Math.Tan(alpha_at1) - Math.Tan(alpha_t1)) / ((Z1 + Z2) * Math.Tan(alpha_t1) - Z1 * Math.Tan(alpha_at1));

            }
            double a2 = etamax1;
            double b = etamax1;

            double result = (xlimit[0] + xlimit[1]) / 2;
            return result;

        }

        /// <summary>
        /// 最小变位系数
        /// </summary>
        public double MinModification1()
        {     //端面压力角
            double alpha_t = Math.Atan(Math.Tan(Alpha * Math.PI / 180) / Math.Cos(Beta * Math.PI / 180));
            double result = Han - Z1 * Math.Pow(Math.Sin(alpha_t), 2) / 2;
            return result;
        }

        /// <summary>
        /// 变位系数检验
        /// </summary>
        public double[] Checking()
        {
            //分度圆直径
            double d1 = Z1 * Mn / Math.Cos(Beta * Math.PI / 180);
            double d2 = Z2 * Mn / Math.Cos(Beta * Math.PI / 180);
            //未变位中心距
            double a = Mn * (Z1 + Z2) / (2 * Math.Cos(Beta * Math.PI / 180));
            //端面压力角
            double alpha_t = Math.Atan(Math.Tan(Alpha * Math.PI / 180) / Math.Cos(Beta * Math.PI / 180));
            //中心距变动系数
            double yn = (A1 - a) / Mn;
            //啮合角
            double alpha_t1 = Math.Acos(a * Math.Cos(alpha_t) / A1);
            //基圆直径
            double db1 = d1 * Math.Cos(alpha_t);
            double db2 = d2 * Math.Cos(alpha_t);
            //总变位系数
            double temp = Math.Tan(alpha_t1) - alpha_t1 - (Math.Tan(alpha_t) - alpha_t);
            double xn = (Z1 + Z2) * temp / (2 * Math.Tan(Alpha * Math.PI / 180));
            double x2 = xn - Mc1;

            //齿顶高
            double ha1 = (Han + Mc1 - (xn - yn)) * Mn;
            double ha2 = (Han + x2 - (xn - yn)) * Mn;
            //齿顶圆直径
            double da1 = d1 + 2 * ha1;
            double da2 = d2 + 2 * ha2;
            //齿顶圆压力角
            double alpha_at1 = Math.Acos(db1 / da1);
            double alpha_at2 = Math.Acos(db2 / da2);
            //检验条件
            double[] result = new double[4];
            //齿顶厚 
            result[0] = da1 * (Math.PI / (2 * Z1) + 2 * Mc1 * Math.Tan(alpha_t) / Z1 + Math.Tan(alpha_t) - alpha_t - (Math.Tan(alpha_at1) - alpha_at1));
            //重合度
            result[1] = (Z1 * (Math.Tan(alpha_at1) - Math.Tan(alpha_t1)) + Z2 * (Math.Tan(alpha_at2) - Math.Tan(alpha_t1))) / (2 * Math.PI);
            //小齿根与大齿轮齿顶不产生干涉条件
            result[2] = Math.Tan(alpha_t1) - Z2 * (Math.Tan(alpha_at2) - Math.Tan(alpha_t1)) / Z1 - Math.Tan(alpha_t) + 4 * (Han - Mc1) / (Z1 * Math.Sin(2 * alpha_t));
            //大齿根与小齿轮齿顶不产生干涉条件
            result[3] = Math.Tan(alpha_t1) - Z1 * (Math.Tan(alpha_at1) - Math.Tan(alpha_t1)) / Z2 - Math.Tan(alpha_t) + 4 * (Han - x2) / (Z2 * Math.Sin(2 * alpha_t));
            return result;
        }

    }
}
