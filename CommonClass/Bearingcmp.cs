using System;

namespace ReducerDesign.CommonClass
{
    class Bearingcmp
    {
        /// <summary>
        /// 速度因数
        /// </summary>
        public double fn;
        /// <summary>
        /// 温度因数
        /// </summary>
        public double ft;
        /// <summary>
        /// 力矩载荷因数
        /// </summary>
        public double fm;
        /// <summary>
        /// 冲击载荷因数
        /// </summary>
        public double fd;
        /// <summary>
        /// 基本额定动载荷
        /// </summary>
        public double c;
        /// <summary>
        /// 当量动载荷
        /// </summary>
        public double p;
        /// <summary>
        /// 轴向力
        /// </summary>
        public double Fa;
        /// <summary>
        /// 径向力
        /// </summary>
        public double Fr;
        /// <summary>
        /// 计算系数
        /// </summary>
        public double Y1;
        /// <summary>
        /// 计算系数
        /// </summary>
        public double Y2;
        /// <summary>
        /// 判定系数
        /// </summary>
        public double e;
        /// <summary>
        /// 转速
        /// </summary>
        public double N;
        /// <summary>
        /// 寿命系数
        /// </summary>
        public double Fh;
        /// <summary>
        /// 寿命因数计算
        /// </summary>
        public double BearingLifeFac()
        {
            double result = fn * ft * c / (fm * fd * p);
            return result;

        }

        /// <summary>
        /// 当量动载荷
        /// </summary>
        public void EquDynLoad()
        {
            if (Fa / Fr <= e)
            {
                p = Fr + Y1 * Fa;

            }
            else
            {
                p = 0.67 * Fr + Y2 * Fa;
            }

        }

        /// <summary>
        /// 滚子轴承速度因数
        /// </summary>
        public double RoBearSpdFac()
        {
            double result = 2.86299985155872 * Math.Pow(N, -0.299972184918814);
            return result; 
        }

        /// <summary>
        /// 球轴承速度因数
        /// </summary>
        public double BaBearSpdFac()
        {
            double result = 3.2194267591373 * Math.Pow(N, -0.333373637212392);
            return result;
        }

        /// <summary>
        /// 滚子轴承寿命
        /// </summary>
        public double RoBearLife()
        {
            double result = Math.Pow(Fh / 0.154938964643228, 1/0.300036847090231);
            return result;
        }

        /// <summary>
        /// 球轴承寿命
        /// </summary>
        public double BaBearLife()
        {
            double result = Math.Pow(Fh/ 0.126023942715267, 1/0.333325610886461);
            return result;
        }
    }
}
