using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Data;
using System.Threading;
using ZedGraph;
//test

namespace ReducerDesign
{
	/// <summary>
    /// </summary>
	public class MyPlot
	{
		public MyPlot()
		{
		}
        #region ����ͼ��


        /// <summary>
        /// 
        /// </summary>
        /// <param name="myGraphControl"></param>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <param name="title"></param>
        /// <param name="Xtitle"></param>
        /// <param name="Ytitle"></param>
        public void MyDrawPic2(ZedGraph.ZedGraphControl myGraphControl, double[] X1, double[] Y1, double[] X2, double[] Y2, 
            string title, string Xtitle, string Ytitle)
        {
            GraphPane myPane = myGraphControl.GraphPane;
            myPane.Legend.IsVisible = false;
            myPane.CurveList.Clear();
            myPane.Title.Text = title;
            myPane.XAxis.Title.Text = Xtitle;
            myPane.YAxis.Title.Text = Ytitle;

            PointPairList listr1 = new PointPairList();
            PointPairList listr2 = new PointPairList();
            PointPairList listr3 = new PointPairList(); 
            for (int i = 0; i < X1.Length; i++)
            {
                listr1.Add(Convert.ToDouble(X1[i].ToString("f6")), Convert.ToDouble(Y1[i].ToString("f6")));
            }
            for (int i = 0; i < X2.Length; i++)
            {
                listr2.Add(Convert.ToDouble(X2[i].ToString("f6")), Convert.ToDouble(Y2[i].ToString("f6")));
            }

            //add rack curve
            LineItem myCurveR1 = myPane.AddCurve("title", listr1, Color.Blue, SymbolType.None);
            LineItem myCurveR2 = myPane.AddCurve("title", listr2, Color.Blue, SymbolType.None);
            LineItem myCurveR3 = myPane.AddCurve("title", listr3, Color.Blue, SymbolType.None);
            myCurveR1.Line.Width = 2;
            myCurveR2.Line.Width = 2;
            myCurveR3.Line.Width = 2;
            myCurveR1.Symbol.Fill = new Fill(Color.White);
            myCurveR2.Symbol.Fill = new Fill(Color.White);
            myCurveR3.Symbol.Fill = new Fill(Color.White);
            myCurveR1.IsY2Axis = true;
            myCurveR2.IsY2Axis = true;
            myCurveR3.IsY2Axis = true;
            myPane.XAxis.MajorGrid.IsVisible = true;

            myPane.YAxis.Scale.FontSpec.FontColor = Color.Black;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Black;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.Min = -30;
            myPane.YAxis.Scale.Max = 30;

            myPane.Y2Axis.IsVisible = true;
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Black;
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Black;
            myPane.Y2Axis.MajorTic.IsOpposite = false;
            myPane.Y2Axis.MinorTic.IsOpposite = false;
            myPane.Y2Axis.MajorGrid.IsVisible = true;
            myPane.Y2Axis.Scale.Align = AlignP.Inside;
            myPane.Chart.Fill = new Fill(Color.White, Color.White, 45.0f);
            myGraphControl.IsShowPointValues = true;
            myGraphControl.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(MyPointValueHandler);

            myGraphControl.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(MyZoomEvent);

            myGraphControl.AxisChange();
            myGraphControl.RestoreScale(myPane);
            myGraphControl.Invalidate();
            Application.DoEvents();
            Thread.Sleep(10);//��ͣ0.05��
        }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="myGraphControl"></param>
     /// <param name="coor_x"></param>
     /// <param name="coor_y"></param>
     /// <param name="title"></param>
     /// <param name="Xtitle"></param>
     /// <param name="Ytitle"></param>
        public void MyDrawPic1(ZedGraph.ZedGraphControl myGraphControl, double[] coor_x, double[] coor_y, string title, string Xtitle, string Ytitle)
        {
            GraphPane myPane = myGraphControl.GraphPane;
            myPane.Legend.IsVisible = false;
           // myPane.CurveList.Clear();
            //myPane.Title.Text = title;
            //myPane.XAxis.Title.Text = Xtitle;
            //myPane.YAxis.Title.Text = Ytitle;
            PointPairList list = new PointPairList();
            int N = coor_x.Length;
            for (int i = 0; i < N; i++)
            {
                double x = coor_x[i];
                double y = coor_y[i];
                list.Add(x, y);//��������
            }
            LineItem myCurve = myPane.AddCurve(title,list, Color.Black, SymbolType.None);
            myCurve.Symbol.Fill = new Fill(Color.White);
            myCurve.Line.Width = 2;
            //myCurve.IsX2Axis = false;
            //myCurve.IsY2Axis = false;
                      
            myPane.XAxis.MajorGrid.IsVisible = false;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Black;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Black;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.Min = -30;
            myPane.YAxis.Scale.Max = 30;
            myPane.Y2Axis.IsVisible = false;
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Black;
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Black;
            myPane.Y2Axis.MajorTic.IsOpposite = false;
            myPane.Y2Axis.MinorTic.IsOpposite = false;
            myPane.Y2Axis.MajorGrid.IsVisible = false;
            myPane.Y2Axis.Scale.Align = AlignP.Inside;
            
            myPane.Chart.Fill = new Fill(Color.White, Color.White, 45.0f);
            myGraphControl.IsShowPointValues = true;
            myGraphControl.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(MyPointValueHandler);
            myGraphControl.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(MyZoomEvent);
            myGraphControl.AxisChange();


           


            myGraphControl.RestoreScale(myPane);
            myGraphControl.Invalidate();
        }
             
       
       /// <summary>
       /// 
       /// </summary>
       /// <param name="control"></param>
       /// <param name="pane"></param>
       /// <param name="curve"></param>
       /// <param name="iPt"></param>
       /// <returns></returns>
        private string MyPointValueHandler(ZedGraph.ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];

            return curve.Label.Text + "   ������Ϊ��" + pt.X.ToString("f4") + " ������Ϊ�� " + pt.Y.ToString("f4") + " (mm)";


        }
        private void MyZoomEvent(ZedGraph.ZedGraphControl control, ZoomState oldState,
                    ZoomState newState)
        {
        }

            
        #endregion ����ͼ��
	}
}
