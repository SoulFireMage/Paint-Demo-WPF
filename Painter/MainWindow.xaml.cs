using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
  
using Microsoft.Win32;

namespace Painter
   
    
{
    //dreaded utility, take comfort in the fact it's just a drill project
    public static class utility
    {
        public static IEnumerable<int> RangeWithStep(this  int start, int end, int step)
        {
          
            for (int i = start; i <= end; i += step)
                {
                yield return i;
                }
             
            }
        }
    /// <summary>
    /// Demo painter project . Please note, performance is dire on wpf canvas past a few thousand objects :)
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<double> _items = new ObservableCollection<double>();

        public ObservableCollection<double> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = this;
            1.RangeWithStep(10,1).ToList().ForEach(x => Items.Add((double) x / 10));
            10.RangeWithStep(5000, 50).ToList().ForEach(x => Items.Add((double) x)) ;
            this.SizeValue.ItemsSource = Items;
        
            this.ThicknessValue.ItemsSource = Items;
        

        }

       

        // Have a brush type state, instead. 
        private Polyline _lines = new Polyline();
        //Event - generates by entering in Xaml OnMouseDown="Canvans_OnMouseDown" 
        DrawingAttributes inkAttributes = new DrawingAttributes();
        private SolidColorBrush _color = Brushes.Black;

        private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _lines.Stroke = _color;
            _lines.StrokeThickness = 1.0;
            InkCanvas.Children.Add(_lines);
          
        }

        private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            //take this out and you get lines whereever you put the mouse.
            if (e.LeftButton ==MouseButtonState.Pressed)
            {
              
                _lines.Points.Add(Mouse.GetPosition(InkCanvas));
            }
        }

        private void Canvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _lines = new Polyline {Stroke = _color};
            InkCanvas.Children.Add(_lines);
         }
         
      private void SaveCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var saveFile = new SaveFileDialog {Filter = "JPG Image|*.jpg|PNG|*.png|XAML Data|*.xaml"};
           if (saveFile.ShowDialog() == true) InkCanvas.ExportFile(saveFile.FileName, saveFile.FilterIndex);
         }
        
        private void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var openFile = new OpenFileDialog { Filter = "JPG Image|*.jpg|PNG|*.png|XAML Data|*.xaml" };
            if (openFile.ShowDialog() == true)
            {
                InkCanvas.OpenFile(openFile.FileName, openFile.FilterIndex);
            }
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
           // Button btn = (Button)sender;
            if (sender != null)
            {
                var shape = (Shape) sender;
                var col = shape.Fill as SolidColorBrush;
                {
                    if (col != null)
                        inkAttributes.Color = col.Color;
                }
            }
            
            InkCanvas.DefaultDrawingAttributes = inkAttributes;
        }
        
        private void stuff_changed(object sender, RoutedEventArgs e)
        {
            //caught this gap in reasoning! I'd said != null or != null which means run if either is not null.
            if (SizeValue.SelectedValue != null && ThicknessValue.SelectedValue != null)  { 
            circle();
                }
            }

        private void circle()
        {
            Random r = new Random();
            var c = inkAttributes.Color  ;
            drawingStuff.circle(InkCanvas, new Point(600,400) , (double)SizeValue.SelectedValue, (double) ThicknessValue.SelectedValue,c)  ;
            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
 

        }


        private void ComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            ThicknessValue.SelectedIndex = 5;
            SizeValue.SelectedIndex = 9;
            
            }
    }
    

    }
 
