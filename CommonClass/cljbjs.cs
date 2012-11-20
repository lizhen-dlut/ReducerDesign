using System;
using System.Windows.Forms;

namespace ReducerDesign.CommonClass
{
    class cljbjs
    {
        public double a, Aa, u, k, T, psi_a,psi_d, sigma_hp;
        public double ZXJ
        {
            get
            {
                a = Aa * (u + 1) * Math.Pow(k * T / (CKXS * u * Math.Pow(sigma_hp, 2)), (double)1 / 3);
                return a;
            }
        }

        public double CKXS
        {
            get { return psi_a; }
            set
            {
                double temp = psi_d/(0.5*(u + 1));
                if (temp <= 0.2)
                {
                    psi_a = 0.2;
                }
                else if (temp <= 0.25)
                {
                    psi_a = 0.25;
                }
                else if (temp <= 0.3)
                {
                    psi_a = 0.3;
                }
                else if (temp <= 0.35)
                {
                    psi_a = 0.35;
                }
                else if (temp <= 0.4)
                {
                    psi_a = 0.4;
                }
                else if (temp <= 0.45)
                {
                    psi_a = 0.45;
                }
                else if (temp <= 0.5)
                {
                    psi_a = 0.5;
                }
                else if (temp <= 0.6)
                {
                    psi_a = 0.6;
                }
                else
                {
                    MessageBox.Show("齿宽系数不符合");
                }
            }
        }
    }
}
