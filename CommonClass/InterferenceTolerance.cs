using System.Windows.Forms;

namespace ReducerDesign.CommonClass
{
    class InterferenceTolerance
    {/// <summary>
        /// 查询轴的基本偏差
        /// </summary>
        public double getTolerance(double dBaseDimention, string sBaseDiv)
        {
            double returnResult = 0;

            #region 轴的基本偏差数组
            // 轴的基本偏差，GB/T 1800.3-1998，不包括js的公差，数值大小 41*30 
            double[,] dBasicDeviation ={
                                        {-270,-140,-60,-34,-20,-14,-10,-6,-4,-2,0,-2,-4,-6,0,0,2,4,6,10,14,9999,18,9999,20,9999,26,32,40,60},
                                        {-270,-140,-70,-46,-30,-20,-14,-10,-6,-4,0,-2,-4,9999,1,0,4,8,12,15,19,9999,23,9999,28,9999,35,42,50,80},
                                        {-280,-150,-80,-56,-40,-25,-18,-13,-8,-5,0,-2,-5,9999,1,0,6,10,15,19,23,9999,28,9999,34,9999,45,52,67,97},
                                        {-290,-150,-95,9999,-50,-32,9999,-16,9999,-6,0,-3,-6,9999,1,0,7,12,18,23,28,9999,33,9999,40,9999,50,64,90,130},
                                        {-290,-150,-95,9999,-50,-32,9999,-16,9999,-6,0,-3,-6,9999,1,0,7,12,18,23,28,9999,33,39,45,9999,60,77,108,150},
                                        {-300,-160,-110,9999,-65,-40,9999,-20,9999,-7,0,-4,-8,9999,2,0,8,15,22,28,35,9999,41,47,54,63,73,98,136,188},
                                        {-300,-170,-110,9999,-65,-40,9999,-20,9999,-7,0,-4,-8,9999,2,0,8,15,22,28,35,41,48,55,64,75,88,118,160,218},
                                        {-310,-180,-120,9999,-80,-50,9999,-25,9999,-9,0,-5,-10,9999,2,0,9,17,26,34,43,48,60,68,80,94,112,148,200,274},
                                        {-320,-190,-130,9999,-80,-50,9999,-25,9999,-9,0,-5,-10,9999,2,0,9,17,26,34,43,54,70,81,97,114,136,180,242,325},
                                        {-340,-200,-140,9999,-100,-60,9999,-30,9999,-10,0,-7,-12,9999,2,0,11,20,32,41,53,66,87,102,122,144,172,226,300,405},
                                        {-360,-220,-150,9999,-100,-60,9999,-30,9999,-10,0,-7,-12,9999,2,0,11,20,32,43,59,75,102,120,146,174,210,274,360,480},
                                        {-380,-240,-170,9999,-120,-72,9999,-36,9999,-12,0,-9,-15,9999,3,0,13,23,37,51,71,91,124,146,178,214,258,335,445,585},
                                        {-410,-260,-180,9999,-120,-72,9999,-36,9999,-12,0,-9,-15,9999,3,0,13,23,37,54,79,104,144,172,210,254,310,400,525,690},
                                        {-460,-280,-200,9999,-145,-85,9999,-43,9999,-14,0,-11,-18,9999,3,0,15,27,43,63,92,122,170,202,248,300,365,470,620,800},
                                        {-520,-310,-210,9999,-145,-85,9999,-43,9999,-14,0,-11,-18,9999,3,0,15,27,43,65,100,134,190,228,280,340,415,535,700,900},
                                        {-580,-340,-230,9999,-145,-85,9999,-43,9999,-14,0,-11,-18,9999,3,0,15,27,43,68,108,146,210,252,310,380,465,600,780,1000},
                                        {-660,-380,-240,9999,-170,-100,9999,-50,9999,-15,0,-13,-21,9999,4,0,17,31,50,77,122,166,236,284,350,425,520,670,880,1150},
                                        {-740,-420,-260,9999,-170,-100,9999,-50,9999,-15,0,-13,-21,9999,4,0,17,31,50,80,130,180,258,310,385,470,575,740,960,1250},
                                        {-820,-430,-280,9999,-170,-100,9999,-50,9999,-15,0,-13,-21,9999,4,0,17,31,50,84,140,196,284,340,425,520,640,820,1050,1350},
                                        {-920,-480,-300,9999,-190,-110,9999,-56,9999,-17,0,-16,-26,9999,4,0,20,34,56,94,158,218,315,385,475,580,710,920,1200,1550},
                                        {-1050,-540,-330,9999,-190,-110,9999,-56,9999,-17,0,-16,-26,9999,4,0,20,34,56,98,170,240,350,425,525,650,790,1000,1300,1700},
                                        {-1200,-600,-360,9999,-210,-125,9999,-62,9999,-18,0,-18,-28,9999,4,0,21,37,62,108,190,268,390,475,590,730,900,1150,1500,1900},
                                        {-1350,-680,-400,9999,-210,-125,9999,-62,9999,-18,0,-18,-28,9999,4,0,21,37,62,114,208,294,435,530,660,820,1000,1300,1650,2100},
                                        {-1500,-760,-440,9999,-230,-135,9999,-68,9999,-20,0,-20,-32,9999,5,0,23,40,68,126,232,330,490,595,740,920,1100,1450,1850,2400},
                                        {-1650,-840,-480,9999,-230,-135,9999,-68,9999,-20,0,-20,-32,9999,5,0,23,40,68,132,252,360,540,660,820,1000,1250,1600,2100,2600},
                                        {9999,9999,9999,9999,-260,-145,9999,-76,9999,-22,0,9999,9999,9999,0,0,26,44,78,150,280,400,600,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-260,-145,9999,-76,9999,-22,0,9999,9999,9999,0,0,26,44,78,155,310,450,660,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-290,-160,9999,-80,9999,-24,0,9999,9999,9999,0,0,30,50,88,175,340,500,740,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-290,-160,9999,-80,9999,-24,0,9999,9999,9999,0,0,30,50,88,185,380,560,840,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-320,-170,9999,-86,9999,-26,0,9999,9999,9999,0,0,34,56,100,210,430,620,940,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-320,-170,9999,-86,9999,-26,0,9999,9999,9999,0,0,34,56,100,220,470,680,1050,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-350,-195,9999,-98,9999,-28,0,9999,9999,9999,0,0,40,66,120,250,520,780,1150,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-350,-195,9999,-98,9999,-28,0,9999,9999,9999,0,0,40,66,120,260,580,840,1300,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-390,-220,9999,-110,9999,-30,0,9999,9999,9999,0,0,48,78,140,300,640,960,1450,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-390,-220,9999,-110,9999,-30,0,9999,9999,9999,0,0,48,78,140,330,720,1050,1600,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-430,-240,9999,-120,9999,-32,0,9999,9999,9999,0,0,58,92,170,370,820,1200,1850,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-430,-240,9999,-120,9999,-32,0,9999,9999,9999,0,0,58,92,170,400,920,1350,2000,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-480,-260,9999,-130,9999,-34,0,9999,9999,9999,0,0,68,110,195,440,1000,1500,2300,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-480,-260,9999,-130,9999,-34,0,9999,9999,9999,0,0,68,110,195,460,1100,1650,2500,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-520,-290,9999,-145,9999,-38,0,9999,9999,9999,0,0,76,135,240,550,1250,1900,2900,9999,9999,9999,9999,9999,9999,9999},
                                        {9999,9999,9999,9999,-520,-290,9999,-145,9999,-38,0,9999,9999,9999,0,0,76,135,240,580,1400,2100,3200,9999,9999,9999,9999,9999,9999,9999}
                                        };
            #endregion


            #region 确定基本偏差的行坐标

            int iBasicDeviation = 0, jBasicDeviation = 0; // dBasicDeviation 数组的位置

            if ((dBaseDimention > 0) && (dBaseDimention <= 3))
                iBasicDeviation = 0;
            else if ((dBaseDimention > 3) && (dBaseDimention <= 6))
                iBasicDeviation = 1;
            else if ((dBaseDimention > 6) && (dBaseDimention <= 10))
                iBasicDeviation = 2;
            else if ((dBaseDimention > 10) && (dBaseDimention <= 14))
                iBasicDeviation = 3;
            else if ((dBaseDimention > 14) && (dBaseDimention <= 18))
                iBasicDeviation = 4;
            else if ((dBaseDimention > 18) && (dBaseDimention <= 24))
                iBasicDeviation = 5;
            else if ((dBaseDimention > 24) && (dBaseDimention <= 30))
                iBasicDeviation = 6;
            else if ((dBaseDimention > 30) && (dBaseDimention <= 40))
                iBasicDeviation = 7;
            else if ((dBaseDimention > 40) && (dBaseDimention <= 50))
                iBasicDeviation = 8;
            else if ((dBaseDimention > 50) && (dBaseDimention <= 65))
                iBasicDeviation = 9;
            else if ((dBaseDimention > 65) && (dBaseDimention <= 80))
                iBasicDeviation = 10;
            else if ((dBaseDimention > 80) && (dBaseDimention <= 100))
                iBasicDeviation = 11;
            else if ((dBaseDimention > 100) && (dBaseDimention <= 120))
                iBasicDeviation = 12;
            else if ((dBaseDimention > 120) && (dBaseDimention <= 140))
                iBasicDeviation = 13;
            else if ((dBaseDimention > 140) && (dBaseDimention <= 160))
                iBasicDeviation = 14;
            else if ((dBaseDimention > 160) && (dBaseDimention <= 180))
                iBasicDeviation = 15;
            else if ((dBaseDimention > 180) && (dBaseDimention <= 200))
                iBasicDeviation = 16;
            else if ((dBaseDimention > 200) && (dBaseDimention <= 225))
                iBasicDeviation = 17;
            else if ((dBaseDimention > 225) && (dBaseDimention <= 250))
                iBasicDeviation = 18;
            else if ((dBaseDimention > 250) && (dBaseDimention <= 280))
                iBasicDeviation = 19;
            else if ((dBaseDimention > 280) && (dBaseDimention <= 315))
                iBasicDeviation = 20;
            else if ((dBaseDimention > 315) && (dBaseDimention <= 355))
                iBasicDeviation = 21;
            else if ((dBaseDimention > 355) && (dBaseDimention <= 400))
                iBasicDeviation = 22;
            else if ((dBaseDimention > 400) && (dBaseDimention <= 450))
                iBasicDeviation = 23;
            else if ((dBaseDimention > 450) && (dBaseDimention <= 500))
                iBasicDeviation = 24;
            else if ((dBaseDimention > 500) && (dBaseDimention <= 560))
                iBasicDeviation = 25;
            else if ((dBaseDimention > 560) && (dBaseDimention <= 630))
                iBasicDeviation = 26;
            else if ((dBaseDimention > 630) && (dBaseDimention <= 710))
                iBasicDeviation = 27;
            else if ((dBaseDimention > 710) && (dBaseDimention <= 800))
                iBasicDeviation = 28;
            else if ((dBaseDimention > 800) && (dBaseDimention <= 900))
                iBasicDeviation = 29;
            else if ((dBaseDimention > 900) && (dBaseDimention <= 1000))
                iBasicDeviation = 30;
            else if ((dBaseDimention > 1000) && (dBaseDimention <= 1120))
                iBasicDeviation = 31;
            else if ((dBaseDimention > 1120) && (dBaseDimention <= 1250))
                iBasicDeviation = 32;
            else if ((dBaseDimention > 1250) && (dBaseDimention <= 1400))
                iBasicDeviation = 33;
            else if ((dBaseDimention > 1400) && (dBaseDimention <= 1600))
                iBasicDeviation = 34;
            else if ((dBaseDimention > 1600) && (dBaseDimention <= 1800))
                iBasicDeviation = 35;
            else if ((dBaseDimention > 1800) && (dBaseDimention <= 2000))
                iBasicDeviation = 36;
            else if ((dBaseDimention > 2000) && (dBaseDimention <= 2240))
                iBasicDeviation = 37;
            else if ((dBaseDimention > 2240) && (dBaseDimention <= 2500))
                iBasicDeviation = 38;
            else if ((dBaseDimention > 2500) && (dBaseDimention <= 2800))
                iBasicDeviation = 39;
            else if ((dBaseDimention > 2800) && (dBaseDimention <= 3150))
                iBasicDeviation = 40;
            else
                MessageBox.Show("基本尺寸超出范围。");

            #endregion


            #region 确定基本偏差的纵坐标
            double BasicDeviation;

            if (sBaseDiv.Equals("m"))
            {
                jBasicDeviation = 16;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                returnResult = BasicDeviation;

            }

            else if (sBaseDiv.Equals("n"))
            {
                jBasicDeviation = 17;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                returnResult = BasicDeviation;

            }

            else if (sBaseDiv.Equals("p"))
            {
                jBasicDeviation = 18;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                returnResult = BasicDeviation;

            }
            else if (sBaseDiv.Equals("r"))
            {
                jBasicDeviation = 19;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                returnResult = BasicDeviation;

            }
            else if (sBaseDiv.Equals("s"))
            {
                jBasicDeviation = 20;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                returnResult = BasicDeviation;

            }
            else if (sBaseDiv.Equals("t"))
            {
                jBasicDeviation = 21;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("u"))
            {
                jBasicDeviation = 22;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                returnResult = BasicDeviation;

            }
            else if (sBaseDiv.Equals("v"))
            {
                jBasicDeviation = 23;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("x"))
            {
                jBasicDeviation = 24;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("y"))
            {
                jBasicDeviation = 25;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("z"))
            {
                jBasicDeviation = 26;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("za"))
            {
                jBasicDeviation = 27;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("zb"))
            {
                jBasicDeviation = 28;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else if (sBaseDiv.Equals("zc"))
            {
                jBasicDeviation = 29;
                BasicDeviation = dBasicDeviation[iBasicDeviation, jBasicDeviation];
                if (BasicDeviation != 9999)
                {
                    returnResult = BasicDeviation;

                }
                else
                {

                    returnResult = 9999;
                }
            }
            else
                MessageBox.Show("基本偏差等级错误。");

            //计算完毕，返回结果
            return returnResult;
            #endregion
        }
        /// <summary>
        /// 查询标准公差值
        /// </summary>
        public double getStandardOfTolerance(double dBaseDimention, int iGrade)
        {
            #region 标准公差数值，GB/T 1800.3-1998，数值大小 21*18

            double[,] dStandardOfTolerance = { 
                             {0.8,1.2,2,3,4,6,10,14,25,40,60,100,140,250,400,600,1000,1400},
                             {1,1.5,2.5,4,5,8,12,18,30,48,75,120,180,300,480,750,1200,1800},
                             {1,1.5,2.5,4,6,9,15,22,36,58,90,150,220,360,580,900,1500,2200},
                             {1.2,2,3,5,8,11,18,27,43,70,110,180,270,430,700,1100,1800,2700},
                             {1.5,2.5,4,6,9,13,21,33,52,84,130,210,330,520,840,1300,2100,3300},
                             {1.5,2.5,4,7,11,16,25,39,62,100,160,250,390,620,1000,1600,2500,3900},
                             {2,3,5,8,13,19,30,46,74,120,190,300,460,740,1200,1900,3000,4600},
                             {2.5,4,6,10,15,22,35,54,87,140,220,350,540,870,1400,2200,3500,5400},
                             {3.5,5,8,12,18,25,40,63,100,160,250,400,630,1000,1600,2500,4000,6300},
                             {4.5,7,10,14,20,29,46,72,115,185,290,460,720,1150,1850,2900,4600,7200},
                             {6,8,12,16,23,32,52,81,130,210,320,520,810,1300,2100,3200,5200,8100},
                             {7,9,13,18,25,36,57,89,140,230,360,570,890,1400,2300,3600,5700,8900},
                             {8,10,15,20,27,40,63,97,155,250,400,630,970,1550,2500,4000,6300,9700},
                             {9,11,16,22,32,44,70,110,175,280,440,700,1100,1750,2800,4400,7000,11000},
                             {10,13,18,25,36,50,80,125,200,320,500,800,1250,2000,3200,5000,8000,12500},
                             {11,15,21,28,40,56,90,140,230,360,560,900,1400,2300,3600,5600,9000,14000},
                             {13,18,24,33,47,66,105,165,260,420,660,1050,1650,2600,4300,6600,10500,16500},
                             {15,21,29,39,55,78,125,195,310,500,780,1250,1950,3100,5000,7800,12500,19500},
                             {18,25,35,46,65,92,150,230,370,600,920,1500,2300,3700,6000,9200,15000,23000}, 
                             {22,30,41,55,78,110,175,280,440,700,1100,1750,2800,4400,7000,11000,17500,28000},
                             {26,36,50,68,96,135,210,330,540,860,1350,2100,3300,5400,8600,13500,21000,33000}                            
                             };
            # endregion

            #region  确定公差等级横坐标
            int iStandardOfTolerance = 0, jStandardOfTolerance = 0; // dStandardOfTolerance 数组的位置
            if ((dBaseDimention > 0) && (dBaseDimention <= 3))
                iStandardOfTolerance = 0;
            else if ((dBaseDimention > 3) && (dBaseDimention <= 6))
                iStandardOfTolerance = 1;
            else if ((dBaseDimention > 6) && (dBaseDimention <= 10))
                iStandardOfTolerance = 2;
            else if ((dBaseDimention > 10) && (dBaseDimention <= 18))
                iStandardOfTolerance = 3;
            else if ((dBaseDimention > 18) && (dBaseDimention <= 30))
                iStandardOfTolerance = 4;
            else if ((dBaseDimention > 30) && (dBaseDimention <= 50))
                iStandardOfTolerance = 5;
            else if ((dBaseDimention > 50) && (dBaseDimention <= 80))
                iStandardOfTolerance = 6;
            else if ((dBaseDimention > 80) && (dBaseDimention <= 120))
                iStandardOfTolerance = 7;
            else if ((dBaseDimention > 120) && (dBaseDimention <= 180))
                iStandardOfTolerance = 8;
            else if ((dBaseDimention > 180) && (dBaseDimention <= 250))
                iStandardOfTolerance = 9;
            else if ((dBaseDimention > 250) && (dBaseDimention <= 315))
                iStandardOfTolerance = 10;
            else if ((dBaseDimention > 315) && (dBaseDimention <= 400))
                iStandardOfTolerance = 11;
            else if ((dBaseDimention > 400) && (dBaseDimention <= 500))
                iStandardOfTolerance = 12;
            else if ((dBaseDimention > 500) && (dBaseDimention <= 630))
                iStandardOfTolerance = 13;
            else if ((dBaseDimention > 630) && (dBaseDimention <= 800))
                iStandardOfTolerance = 14;
            else if ((dBaseDimention > 800) && (dBaseDimention <= 1000))
                iStandardOfTolerance = 15;
            else if ((dBaseDimention > 1000) && (dBaseDimention <= 1250))
                iStandardOfTolerance = 16;
            else if ((dBaseDimention > 1250) && (dBaseDimention <= 1600))
                iStandardOfTolerance = 17;
            else if ((dBaseDimention > 1600) && (dBaseDimention <= 2000))
                iStandardOfTolerance = 18;
            else if ((dBaseDimention > 2000) && (dBaseDimention <= 2500))
                iStandardOfTolerance = 19;
            else if ((dBaseDimention > 2500) && (dBaseDimention <= 3150))
                iStandardOfTolerance = 20;
            else
                MessageBox.Show("基本尺寸超出范围。");

            switch (iGrade)
            {
                case 1:
                    jStandardOfTolerance = 0;
                    break;
                case 2:
                    jStandardOfTolerance = 1;
                    break;
                case 3:
                    jStandardOfTolerance = 2;
                    break;
                case 4:
                    jStandardOfTolerance = 3;
                    break;
                case 5:
                    jStandardOfTolerance = 4;
                    break;
                case 6:
                    jStandardOfTolerance = 5;
                    break;
                case 7:
                    jStandardOfTolerance = 6;
                    break;
                case 8:
                    jStandardOfTolerance = 7;
                    break;
                case 9:
                    jStandardOfTolerance = 8;
                    break;
                case 10:
                    jStandardOfTolerance = 9;
                    break;
                case 11:
                    jStandardOfTolerance = 10;
                    break;
                case 12:
                    jStandardOfTolerance = 11;
                    break;
                case 13:
                    jStandardOfTolerance = 12;
                    break;
                case 14:
                    jStandardOfTolerance = 13;
                    break;
                case 15:
                    jStandardOfTolerance = 14;
                    break;
                case 16:
                    jStandardOfTolerance = 15;
                    break;
                case 17:
                    jStandardOfTolerance = 16;
                    break;
                case 18:
                    jStandardOfTolerance = 17;
                    break;
            }
            return dStandardOfTolerance[iStandardOfTolerance, jStandardOfTolerance];
            #endregion
        }
           
    }
}
