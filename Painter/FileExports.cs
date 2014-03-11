using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Painter
{
    public static class FileExports
    {
        
        private static InkCanvas surface { get; set; }

        public static void ExportFile(this InkCanvas canvas, string filename, int format)
        {
            surface = canvas;
            switch (format)
            {
                case 1: ExportToPng(new Uri( filename), new PngBitmapEncoder());
                    break;
                case 2: ExportToPng(new Uri(filename), new JpegBitmapEncoder());
                    break;
                case 3: SerializeToXML( filename);
                    break;
            }
        }


        public static void OpenFile(this InkCanvas canvas, string filename, int format)
        {
            surface = canvas;
            switch (format)
            {
                case 1: OpenPng(  new BitmapImage( new Uri(filename, UriKind.Relative))  );
                    break;
                case 2:
                    OpenPng(new BitmapImage(new Uri(filename, UriKind.Relative)));
                    break;
                case 3: OpenXML(filename);
                    break;
            }
        }

        private static void OpenXML(string filename)
        {
            var mystrXAML = XamlReader.Load(new FileStream(filename,FileMode.Open));
            surface.DataContext = mystrXAML;
        }

        private static void OpenPng(BitmapImage bitmapImage)
        {

            var brush = new ImageBrush {ImageSource = bitmapImage};  
            
            surface.Background = brush;
        }
        private static void SerializeToXML( string  filename  )
        {
            string mystrXAML = XamlWriter.Save(surface);
            FileStream filestream = File.Create(filename);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.Write(mystrXAML);
            streamwriter.Close();
            filestream.Close();
        }
        private static void ExportToPng(  Uri path, BitmapEncoder encoder)
        {
            if (path == null) return;

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;
            double w = surface.Width.CompareTo(double.NaN) == 0 ? surface.ActualWidth : surface.Width;
            double h = surface.Height.CompareTo(double.NaN) == 0 ? surface.ActualHeight : surface.Height;
            // Get the size of canvas
            Size size = new Size(w, h);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
            {
                // Use png encoder for our data -imjected the dependency
            //    PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }

            // Restore previously saved layout
            surface.LayoutTransform = transform;

        }
    }
}
