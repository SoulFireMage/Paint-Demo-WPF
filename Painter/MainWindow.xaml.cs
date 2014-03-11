using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
 

// Note to Daniel - Get yourself a trial of Resharper as well as visual studio 2013 pro 
using Microsoft.Win32;

namespace Painter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
         
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        //This is cheating, I'm casting an object to a Button to access it's background colour.
            //Quick n dirty basically as casts never feel right to me.
            Button btn = (Button) sender;
            var solidColorBrush = btn.Background as SolidColorBrush;
            if (solidColorBrush != null)
            {
                inkAttributes.Color = solidColorBrush.Color;
            }
            InkCanvas.DefaultDrawingAttributes = inkAttributes;
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
      
    }
    

    }
 
