using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shaders3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Drawing.Imaging.PixelFormat ImagePixelFormat = System.Drawing.Imaging.PixelFormat.Format64bppArgb;
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += Save;
        }

        void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        // and so on for other encoders (if you want)


        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            //I can just render everything that I want here - I guess you can render any visual
            //How to do transaperency, maybe can apply a subtractive Mask?
            // Maybe this ? very slow though https://stackoverflow.com/questions/26082681/write-transparency-to-bitmap-using-unsafe-with-the-original-colors-preserved
            //Would also need to figure out how to apply shaders to all the image or only parts of it
            bitmap.Render(visual);
            bitmap.Render(InkCanvas);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }

        private void Save(object sender, EventArgs args)
        {
            SaveToPng(BackgroundImage, "C:/test/jk.png");
            //Bitmap img = new Bitmap((int)BackgroundImage.Source.Width, (int)BackgroundImage.Source.Height, ImagePixelFormat);
            //Graphics graphics = Graphics.FromImage(img);
            //var backgroundImag = (BitmapSource)BackgroundImage.Source;
            //var newImg = BitmapSourceToBitmap2(backgroundImag);
            //graphics.DrawImage(newImg, new System.Drawing.Point(0, 0));
            //graphics.Dispose();
            //img.Save(@"C:/test/newImage.png");
        }

        //From https://stackoverflow.com/questions/5689674/c-sharp-convert-wpf-image-source-to-a-system-drawing-bitmap
        //Doesn't work - Makes the image but its jsut a some random black/transparent image
        public static System.Drawing.Bitmap BitmapSourceToBitmap2(BitmapSource srs)
        {
            int width = srs.PixelWidth;
            int height = srs.PixelHeight;
            int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(height * stride);
                srs.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);
                using (var btm = new System.Drawing.Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format64bppArgb, ptr))
                {
                    // Clone the bitmap so that we can dispose it and
                    // release the unmanaged memory at ptr
                    return new System.Drawing.Bitmap(btm);
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        public Bitmap ConvertToBitmap(BitmapSource bitmapSource)
        {
            var width = bitmapSource.PixelWidth;
            var height = bitmapSource.PixelHeight;
            var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, ImagePixelFormat, memoryBlockPointer);
            Marshal.FreeHGlobal(memoryBlockPointer);
            return bitmap;
        }
    }
}