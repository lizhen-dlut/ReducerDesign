using System;

namespace ReducerDesign.CommonClass
{
    class ShaftStrength
    {
        public double Fex;
        public double Fey;
        public double Fez;
        public double Fax;
        public double Fay;
        public double Faz;
        public double Fbx;
        public double Fby;
        public double Fbz;
        /// <summary>
        /// D点齿轮圆周力
        /// </summary>
        public double Ft2;
        public double Fr2;
        public double Fa2;
        /// <summary>
        /// C点齿轮圆周力
        /// </summary>
        public double Ft1;
        public double Fr1;
        public double Fa1;
        public double Fhz;
        public double Fhy;
        public double Fhx;
        public double L1;
        public double L2;
        public double L3;
        public double L4;
        public double L5;
        /// <summary>
        /// C点齿轮分度圆半径
        /// </summary>
        public double r1;
        /// <summary>
        /// D点齿轮分度圆半径
        /// </summary>
        public double r2;
        /// <summary>
        /// 键槽参数深
        /// </summary>
        

        /// <summary>
        /// 输入轴水平面（xoz）D剖面右侧弯矩
        /// </summary>
        public double ShaftMdzR1()
        {
            double result = -Fbz * (L4 - L3);
            return result;
        }

        /// <summary>
        /// 输入轴水平面D剖面左侧弯矩
        /// </summary>
        public double ShaftMdzL1()
        {
            double result = ShaftMdzR1() - Fa2 * r2;
            return result;

        }

        /// <summary>
        /// 中间轴1水平面D剖面左侧弯矩
        /// </summary>
        public double ShaftMdzL2()
        {
            double result = ShaftMdzR2() - Fa2 * r2;
            return result;
        }

        /// <summary>
        /// 输入轴水平面A剖面弯矩
        /// </summary>
        public double ShaftMaz1()
        {
            double result = -Fez * L1;
            return result;
        }

        /// <summary>
        /// 输入轴垂直面(xoy)D剖面弯矩
        /// </summary>
        public double ShaftMdy1()
        {
            double result = Fby * (L4 - L3);
            return result;

        }

        /// <summary>
        /// 输入轴垂直面A剖面弯矩
        /// </summary>
        public double ShaftMay1()
        {
            double result = Fey * L1;
            return result;

        }

        /// <summary>
        /// 中间轴1水平面D剖面右侧弯矩
        /// </summary>
        public double ShaftMdzR2()
        {
            double result = -Fbz * (L4 - L3);
            return result;
        }

        /// <summary>
        /// 中间轴1水平面C剖面左侧弯矩
        /// </summary>
        public double ShaftMczL2()
        {
            double result = -Faz * L2;
            return result;
        }

        /// <summary>
        /// 中间轴1水平面C剖面右侧弯矩
        /// </summary>
        public double ShaftMczR2()
        {
            double result = ShaftMczL2() + Fa1 * r1;
            return result;
        }

        /// <summary>
        /// 中间轴1垂直面C剖面弯矩
        /// </summary>
        public double ShaftMcy2()
        {
            double result = Fay * L2;
            return result;
        }

        /// <summary>
        /// 中间轴1垂直面D剖面弯矩
        /// </summary>
        public double ShaftMdy2()
        {
            double result = Fby * (L4 - L3);
            return result;
        }

        /// <summary>
        /// 中间轴2水平面D剖面右侧弯矩
        /// </summary>
        public double ShaftMdzR3()
        {
            double result = -Fbz * (L4 - L3);
            return result;
        }

        /// <summary>
        /// 中间轴2水平面D剖面左侧弯矩
        /// </summary>
        public double ShaftMdzL3()
        {
            double result = ShaftMdzR3() - Fa2 * r2;
            return result;
        }

        /// <summary>
        /// 中间轴2水平面C剖面左侧弯矩
        /// </summary>
        public double ShaftMczL3()
        {
            double result = -Faz * L2;
            return result;
        }

        /// <summary>
        /// 中间轴2水平面C剖面右侧弯矩
        /// </summary>
        public double ShaftMczR3()
        {
            double result = ShaftMczL3() +Fa1 * r1;
            return result;
        }

        /// <summary>
        /// 中间轴2垂直面C剖面弯矩
        /// </summary>
        public double ShaftMcy3()
        {
            double result = Fay * L2;
            return result;
        }

        /// <summary>
        /// 中间轴2垂直面D剖面弯矩
        /// </summary>
        public double ShaftMdy3()
        {
            double result = Fby * (L4 - L3);
            return result;
        }

        /// <summary>
        /// 输出轴水平面D剖面右侧弯矩
        /// </summary>
        public double ShaftMdzR4()
        {
            double result = -Fhz * (L5 + L4 - L3) - Fbz * (L4 - L3);
            return result;
        }

        /// <summary>
        /// 输出轴水平面D剖面左侧弯矩
        /// </summary>
        public double ShaftMdzL4()
        {
            double result = ShaftMdzR4() - Fa2 * r2;
            return result;
        }

        /// <summary>
        /// 输出轴水平面B剖面弯矩
        /// </summary>
        public double ShaftMbz4()
        {
            double result = -Fhz * L5;
            return result;
        }

        /// <summary>
        /// 输出轴水平面A剖面弯矩
        /// </summary>
        public double ShaftMaz4()
        {
            double result = -Fez * L1;
            return result;
        }

        /// <summary>
        /// 输出轴垂直面D剖面弯矩
        /// </summary>
        public double ShaftMdy4()
        {
            double result = Fby * (L4 - L3) + Fhy * (L4 - L3 + L5);
            return result;
        }

        /// <summary>
        /// 输出轴垂直面B剖面弯矩
        /// </summary>
        public double ShaftMby4()
        {
            double result = Fhy * L5;
            return result;
        }

        /// <summary>
        /// 输出轴垂直面A剖面弯矩
        /// </summary>
        public double ShaftMay4()
        {
            double result = Fey * L1;
            return result;
        }

        /// <summary>
        /// 合成弯矩
        /// </summary>
        public double SyntheticMoment(double a,double b)
        {
            double result = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            return result;
        }

        /// <summary>
        /// 实心轴弯曲应力
        /// </summary>
        public double CircleSecBendStre(double d,double alpha,double m,double t )
        {  //d 截面直径  a折合系数 m 合成弯矩  t转矩
            double w = Math.PI * Math.Pow(d, 3) / 32;//抗弯截面系数
            double result = Math.Sqrt(Math.Pow(m, 2) + Math.Pow(alpha * t, 2)) / w;
            return result;
        }

        /// <summary>
        /// 单键槽弯曲阴历
        /// </summary>
        public double SingleGroBendStre(double d, double alpha, double m, double t,double b,double ht)
        {   //d 截面直径  a折合系数 m 合成弯矩  t转矩  b 键槽宽  ht键槽深度参数
            double w = Math.PI * Math.Pow(d, 3) / 32 - b*ht*Math.Pow(d-ht,2)/(2*d)  ;//抗弯截面系数
            double wt = Math.PI * Math.Pow(d, 3) / 16 - b * ht * Math.Pow(d - ht, 2) / (2 * d);//抗扭截面系数
            double sigma = m / w;//弯曲应力
            double tau = t / wt;//
            double result = Math.Sqrt(Math.Pow(sigma, 2) + 4 * Math.Pow(alpha * tau, 2));
                return result ;

        }

        /// <summary>
        /// 扭转应力
        /// </summary>
        public double CircleSecTorStre(double d,  double t)
        {//d 截面直径   t转矩
            double wt = Math.PI * Math.Pow(d, 3) / 16;//扭转截面系数
            double result = t / wt;
            return result;
            
        }
    }
}
