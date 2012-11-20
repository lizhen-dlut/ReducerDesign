namespace ReducerDesign.CommonClass
{
    class BearingReaFor
    {
        /// <summary>
        /// A轴承轴向力
        /// </summary>
        public double Fax;
        /// <summary>
        /// A轴承y径向力
        /// </summary>
        public double Fay;
        /// <summary>
        /// A轴承z径向力
        /// </summary>
        public double Faz;
        /// <summary>
        /// B轴承轴向力
        /// </summary>
        public double Fbx;
        /// <summary>
        /// B轴承y径向力
        /// </summary>
        public double Fby;
        /// <summary>
        /// B轴承z径向力
        /// </summary>
        public double Fbz;
        /// <summary>
        /// C点齿轮轴向力
        /// </summary>
        public double Fa1;
        /// <summary>
        /// C点齿轮径向力
        /// </summary>
        public double Fr1;
        /// <summary>
        /// C点齿轮圆周力
        /// </summary>
        public double Ft1;
        /// <summary>
        /// D点齿轮轴向力
        /// </summary>
        public double Fa2;
        /// <summary>
        /// D点齿轮径向力
        /// </summary>
        public double Fr2;
        /// <summary>
        /// D点齿轮圆周力
        /// </summary>
        public double Ft2;
        /// <summary>
        /// E点轴向力
        /// </summary>
        public double Fex;
        /// <summary>
        /// E点y径向力
        /// </summary>
        public double Fey;
        /// <summary>
        /// E点z径向力
        /// </summary>
        public double Fez;
        /// <summary>
        /// H点轴向力
        /// </summary>
        public double Fhx;
        /// <summary>
        /// H点Y径向力
        /// </summary>
        public double Fhy;
        /// <summary>
        /// H点z径向力
        /// </summary>
        public double Fhz;
        /// <summary>
        /// AE距离
        /// </summary>
        public double L1;
        /// <summary>
        /// AC距离
        /// </summary>
        public double L2;
        /// <summary>
        /// AD距离
        /// </summary>
        public double L3;
        /// <summary>
        /// AB距离
        /// </summary>
        public double L4;
        /// <summary>
        /// BH距离
        /// </summary>
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
        /// 中间轴2A轴承Z径向力
        /// </summary>
        public double BearingFAz3()
        {
            double result = (- Fr1 * (L4 - L2) + Fr2 * (L4 - L3) + Fa1*r1+ Fa2*r2 )/L4;
            return result;
        }

        /// <summary>
        /// 中间轴2B轴承Z径向力
        /// </summary>
        public double BearingFBz3()
        {
            double result = Fr2 - Fr1 - BearingFAz3();
            return result;
        }

        /// <summary>
        /// 输入轴A轴承Z径向力
        /// </summary>
        public double BearingFAz1()
        {
            double result = (-Fez*(L1+L4)+Fr2*(L4-L3)+Fa2*r2)/L4;
            return result ;
        }

        /// <summary>
        /// 输入轴B轴承Z径向力
        /// </summary>
        public double BearingFBz1()
        {
            double result = Fr2 - Fez - BearingFAz1();

            return result;
        }

        /// <summary>
        /// 输入轴B轴承Y径向力
        /// </summary>
        public double BearingFBy1()
        {
            double result = -Fey - BearingFAy1() - Ft2;
            return result;
        }

        /// <summary>
        /// 输入轴A轴承y径向力
        /// </summary>
        public double BearingFAy1()
        {
            double result = (-Fey * (L1 + L4) - Ft2 * (L4 - L3) ) / L4;
            return result;
        }

        /// <summary>
        /// 中间轴1A轴承Y径向力
        /// </summary>
        public double BearingFAy2()
        {
            double result = (Ft2 * (L4 - L3) + Ft1 * (L4 - L2)  ) / L4;
            return result;
        }

        /// <summary>
        /// 中间轴2A轴承y径向力
        /// </summary>
        public double BearingFAy3()
        {
            double result = (-Ft1 * (L4 - L2) - Ft2 * (L4 - L3)) / L4;
            return result;
        }

        /// <summary>
        /// 中间轴1A轴承Z径向力
        /// </summary>
        public double BearingFAz2()
        {
            double result = (Fr1 * (L4 - L2) - Fr2 * (L4 - L3)+ Fa1*r1+ Fa2*r2 ) / L4;
            return result;
        }

        /// <summary>
        /// 输出轴A轴承z径向力
        /// </summary>
        public double BearingFAz4()
        {
            double result = (-Fr2 * (L4 - L3) + Fhz * L5 - Fez * (L1 + L4)+ Fa2*r2) / L4;
            return result;
        }

        /// <summary>
        /// 中间轴1B轴承Z径向力
        /// </summary>
        public double BearingFBz2()
        {
            double result = Fr1 - Fr2 - BearingFAz2();
            return result;
        }

        /// <summary>
        /// 输出轴B轴承z径向力
        /// </summary>
        public double BearingFBz4()
        {
            double result = -Fhz - Fr2 - BearingFAz4() - Fez;

            return result;
        }

        /// <summary>
        /// 中间轴1A轴承Y径向力
        /// </summary>
        public double BearingFBy2()
        {
            double result = Ft1 + Ft2 - BearingFAy2();
            return result;
        }

        /// <summary>
        /// 中间轴2B轴承y径向力
        /// </summary>
        public double BearingFBy3()
        {
            double result = -Ft2 - Ft1 - BearingFAy3();
            return result;
        }

        /// <summary>
        /// 输出轴A轴承y径向力
        /// </summary>
        public double BearingFAy4()
        {
            double result = (Fhy * L5 + Ft2 * (L4 - L3) - Fey * (L1 + L4)) / L4;
            return result;
        }

        /// <summary>
        /// 输出轴B轴承y径向力
        /// </summary>
        public double BearingFBy4()
        {
            double result = Ft2 - Fhy - Fey - BearingFAy4();
            return result;
        }

    }
}
