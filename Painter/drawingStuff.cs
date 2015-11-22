
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Painter
    {
  public  static class drawingStuff
  {
        
      private static SolidColorBrush brushC;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InkCanvas"></param>
        /// <param name="startXY"></param>
        /// <param name="size"></param>
        /// <param name="thickness"></param>
        /// <param name="c"></param>
      public static void circle(InkCanvas InkCanvas, Point startXY, double size, double thickness, Color c)
      {
            brushC =  new SolidColorBrush(c);
            double radius = size/2;
            Point _start = new Point(startXY.X, startXY.Y - radius);
            Func<Point, double, Point> setPixel = (Point PP, double degree) =>
            {
                PP.X = PP.X + radius*Math.Cos(degree);
                PP.Y = PP.Y + radius*Math.Sin(degree);
                return PP;
            };
            List<Point> points = new List<Point>();
            double circumference = size * Math.PI;
            double step = 360/circumference;
            for (double i = 0; i <= circumference; i += step)
            {
                points.Add(setPixel(_start, i));
             }
            brushC.Freeze();//does this speed things up by not keep making this?
           foreach (var point in points)
           {
                Rectangle r = new Rectangle() { Width = 1, Height = 1, Stroke = brushC }; //can I in fact clone this object - flyweight pattern?
                InkCanvas.SetTop(r,point.Y);
                InkCanvas.SetLeft(r, point.X);
                InkCanvas.Children.Add(r);
                }
      }

      public static void spiral(InkCanvas InkCanvas, double size, Color c)
      {
          double ratioX = InkCanvas.ActualWidth/InkCanvas.ActualHeight;
          double ratioy = InkCanvas.ActualHeight/InkCanvas.ActualWidth;
          Point acc = new Point((InkCanvas.ActualWidth - (size/ratioX))/2, (InkCanvas.ActualHeight - (size*ratioy))/2);
          Polyline p = new Polyline {Stroke = new SolidColorBrush(c), StrokeThickness = 0.1};
          for (double x = 0; x <= 360; x += 1)
          {
              double x2 = (size*Math.Cos(x))/0.99;
              double y2 = (size*Math.Sin(x))/0.99;
              p.Points.Add(new Point(acc.X, acc.Y));
              acc.X += x2;
              acc.Y += y2;
              // InkCanvas.Children.Add(drawingStuff.AngleLine(5, acc, 100, Colors.Black));
          }
          InkCanvas.Children.Add(p);
      }

      /// <summary>
        /// Simple route, can I define a new line using a start, angle and length?
        /// First attempt just worked! :).
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="StartxY"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Line AngleLine(int degree, Point StartxY, double length, Color c)
        {
            Line l = new Line
            {
                X1 = StartxY.X,
                X2 = (StartxY.X/Math.Sin(degree))*length,
                Y1 = StartxY.Y,
                Y2 = (StartxY.Y/Math.Sin(degree))*length
            };
            l.Stroke = new SolidColorBrush(c);
            return l;
        }

      public static Point GetPoint(int degree, Point StartXY, double length)
      {
           return new Point() { X = (StartXY.X/Math.Sin(degree)) * length,Y = (StartXY.Y/Math.Sin(degree))* length };
       }
        /// <summary>
        /// Make a square with a colour and a size
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Polyline Square(int x, int y, int size, Color c )
        {
            Point TopLeft = new Point(x, y);
            Point TopRight = new Point(x*size, y);
            Point BottomRight = new Point(x * size, y * size);
            Point BottomLeft = new Point(x, y * size);
            
            Polyline p = P(c);
            p.Points = new PointCollection() { TopLeft, TopRight, BottomRight, BottomLeft,TopLeft };
            return p;
            }

        /// <summary>
        /// random collapsing squares
        /// </summary>
        /// <param name="InkCanvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
      public static void randomSqares(InkCanvas InkCanvas, int x, int y)
      {
          Random r = new Random();
          for (int i = 0; i <= 100; i ++)   
          {
             byte c = (byte) i;

              InkCanvas.Children.Add(drawingStuff.Square(10, 10, i,
                  Color.FromRgb((byte) r.Next(255), (byte) r.Next(255), (byte) r.Next(255))));
          }
      }
       
        /// <summary>
        /// Convenience routine only
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static Polyline P(Color C)
                    {
                    Polyline p = new Polyline
                    {
                        StrokeThickness = 2,
                        Stroke = new SolidColorBrush(C)
                    };
                    return p;
                    }
        
        public static Line gridLineX(int x, int y, Brush c)
            {
            Line l = new Line();
            l.X1 = x;
            l.Y1 = y;
            l.X2 = x;
            l.Y2 = y + 100;
            l.Stroke = Brushes.Black;
            return l;
            }

        public static Line gridLineY(int x, int y, Brush c)
            {
            Line l = new Line();
            l.X1 = x;
            l.Y1 = y;
            l.X2 = x + 100;
            l.Y2 = y;
            l.Stroke = Brushes.Black;
            return l;
            }
        }
    }
