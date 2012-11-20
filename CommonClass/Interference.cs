using System;

namespace ReducerDesign.CommonClass
{
    class Interference
    {
        /// <summary>
        /// 转矩
        /// </summary>
        public double T;
        /// <summary>
        /// 接合直径
        /// </summary>
        public double Df;
        /// <summary>
        /// 接合长度
        /// </summary>
        public double Lf;
        /// <summary>
        /// 摩擦因数
        /// </summary>
        public double Mu;
        /// <summary>
        /// 包容件外径
        /// </summary>
        public double Da;
        /// <summary>
        /// 被包容件内径
        /// </summary>
        public double Di;
        /// <summary>
        /// 包容件泊松比
        /// </summary>
        public double Va;
        /// <summary>
        /// 包容件弹性模量
        /// </summary>
        public double Ea;
        /// <summary>
        /// 被包容件弹性模量
        /// </summary>
        public double Ei;
        /// <summary>
        /// 包容件材料屈服点
        /// </summary>
        public double Sigmasa;
        /// <summary>
        /// 被包容件材料屈服点
        /// </summary>
        public double Sigmasi;
        /// <summary>
        /// 被包容件泊松比
        /// </summary>
        public double Vi;

        /// <summary>
        /// 传递载荷所需要的最小结合压强
        /// </summary>
        public double Pfmin()
        {
            double result = 2 * T / (Math.PI * Math.Pow(Df, 2) * Lf * Mu);
            return result;

        }

        /// <summary>
        /// 包容件直径比
        /// </summary>
        public double Qa()
        {
            double result = Df / Da;
            return result;
        }

        /// <summary>
        /// 被包容件直径比
        /// </summary>
        public double Qi()
        {
            double result = Di / Df;
            return result;
        }

        /// <summary>
        /// 包容件传递载荷需要的最小直径变化量
        /// </summary>
        public double Eamin()
        {
            double ca = (1 + Math.Pow(Qa(), 2)) / (1 - Math.Pow(Qa(), 2)) + Va;
            double result = Pfmin() * Df * ca / Ea;
            return result;
        }

        /// <summary>
        /// 被包容件传递载荷需要的最小直径变化量
        /// </summary>
        public double Eimin()
        {
            double ci =(1+Math.Pow(Qi(),2))/(1-Math.Pow(Qi(),2))-Vi;
            double result = Pfmin() * Df * ci / Ei;
            return result;
        }

        /// <summary>
        /// 传递载荷缩需要的最小有效过盈量
        /// </summary>
        public double Deltaemin()
        {
            double result = Eamin() + Eimin();
            return result;
        }

        /// <summary>
        /// 胀缩法转配考虑压平量的多需要最小过盈量
        /// </summary>
        public double Deltamin()
        {
            double result = Deltaemin();
            return result;
            
        }

        /// <summary>
        /// 塑性材料包容件不产生塑性变形允许的最大结合压强
        /// </summary>
        public double Pfamax()
        {
            double a = (1 - Math.Pow(Qa(), 2)) / Math.Sqrt(3 + Math.Pow(Qa(), 4));
            double result = a * Sigmasa;
            return result;

        }

        /// <summary>
        /// 塑性材料被包容件不产生塑性变形允许的最大结合压强
        /// </summary>
        public double Pfimax()
        {
            double c = (1 - Math.Pow(Qi(), 2)) / 2;
            double result = c * Sigmasi;
            return result;
        }

        /// <summary>
        /// 被联接件不产生塑性变形允许的最大结合压强
        /// </summary>
        public double Pfmax()
        {
            double result = Math.Min(Pfamax(), Pfimax());
            return result;
            
        }

        /// <summary>
        /// 被联接件不产生塑性变形的传递力
        /// </summary>
        public double Ft()
        {
            double result = Pfmax() * Math.PI * Df * Lf * Mu;
            return result;
        }

        /// <summary>
        /// 包容件不产生塑性变形允许的最大直径变化量
        /// </summary>
        public double Eamax()
        {
            double ca = (1 + Math.Pow(Qa(), 2)) / (1 - Math.Pow(Qa(), 2)) + Va;
            double result = Pfmax() * Df * (ca / Ea);
            return result;
        }

        /// <summary>
        /// 被包容件不产生塑性变形允许的最大直径变化量
        /// </summary>
        public double Eimax()
        {
            double ci = (1 + Math.Pow(Qi(), 2)) / (1 - Math.Pow(Qi(), 2)) - Vi;
            double result = Pfmax() * Df * (ci / Ei);
            return result;
        }

        /// <summary>
        /// 被联接件不产生塑性变形所容许的最大有效过盈量
        /// </summary>
        public double Deltaemax()
        {
            double result = Eamax() + Eimax();
            return result;
        }

        /// <summary>
        /// 初选基本过盈量
        /// </summary>
        public double Deltab()
        {
            double result = (Deltamin() + Deltaemax()) / 2;
            return result;
        }
    }
}
