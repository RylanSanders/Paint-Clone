﻿using ProjectDesigner.ShaderEffects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for ViewPanel.xaml
    /// </summary>
    public partial class ViewPanel : UserControl
    {
        private System.Drawing.Imaging.PixelFormat ImagePixelFormat = System.Drawing.Imaging.PixelFormat.Format64bppArgb;
        public int PicHeight = 422;
        public int PicWidth = 750;
        public string ImagePath = "C:\\test\\picture.jpg";

        public List<FrameworkElement> Layers { get; set; }
        public FrameworkElement ActiveLayer { get; set; }

        //Two things I need to know
        //How can I write transparency to images for the transparent layer
        //How can I make shaders based on the layers

        //Layer

        public ViewPanel()
        {
            System.Drawing.Image image = Bitmap.FromFile(ImagePath);
            PicHeight = image.Height; PicWidth = image.Width;
            InitializeComponent();
            Layers = new List<FrameworkElement>();
            
            InkCanvas transparentInkCanvas = new InkCanvas();
            transparentInkCanvas.Name = "Transparent";
            transparentInkCanvas.Effect = new CheckerboardShader();
            transparentInkCanvas.Background = System.Windows.Media.Brushes.Transparent;
            transparentInkCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            transparentInkCanvas.VerticalAlignment = VerticalAlignment.Stretch;

            System.Windows.Controls.Image imgSrc = new System.Windows.Controls.Image();
            imgSrc.Source = new BitmapImage(new Uri(ImagePath));
            imgSrc.Name = "Image";
            imgSrc.HorizontalAlignment = HorizontalAlignment.Stretch;
            imgSrc.VerticalAlignment = VerticalAlignment.Stretch;

            InkCanvas opaqueCanvas = new InkCanvas();
            opaqueCanvas.Name = "Ink";
            opaqueCanvas.Background = System.Windows.Media.Brushes.Transparent;
            opaqueCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            opaqueCanvas.VerticalAlignment = VerticalAlignment.Stretch;


            Layers.Add(imgSrc);
            Layers.Add(transparentInkCanvas);
            Layers.Add(opaqueCanvas);
            Layers.ForEach(layer => ParentCanvas.Children.Add(layer));
            //TransparentInkCanvas.opaqueInk = DrawingInkCanvas;
            KeyDown += Save;

            ParentCanvas.SizeChanged += ResizeLayers;
            ActiveLayer = opaqueCanvas;
        }

        public void ReorderLayers()
        {
            ParentCanvas.Children.Clear();
            Layers.ForEach(layer => ParentCanvas.Children.Add(layer));
        }

        public void ChangeActiveLayer(FrameworkElement layer)
        {
            ActiveLayer.Focusable = false;
            ActiveLayer.IsHitTestVisible = false;
            ActiveLayer = layer;
            ActiveLayer.Focusable = true;
            ActiveLayer.IsHitTestVisible = true;
        }

        private void ResizeLayers(object sender, EventArgs args) {
            //Layers.ForEach(l => { l.Height = ParentCanvas.Height; l.Width = ParentCanvas.Width; });
            Layers.ForEach(l => l.InvalidateVisual());
        }

        public void ChangeBrushColor(System.Windows.Media.Color color)
        {
            //Use the TransparentInkCanvas which has a shader applied when using 
            if (color == Colors.Transparent)
            {
                //TransparentInkCanvas.Focusable = true;
                //TransparentInkCanvas.IsHitTestVisible = true;
                //DrawingInkCanvas.Focusable = false;
                //DrawingInkCanvas.IsHitTestVisible = false;
            }
            else
            {
                //TransparentInkCanvas.Focusable = false;
                //TransparentInkCanvas.IsHitTestVisible = false;
                //DrawingInkCanvas.Focusable = true;
                //DrawingInkCanvas.IsHitTestVisible = true;

                //DrawingInkCanvas.DefaultDrawingAttributes.Color = color;
            }
        }

        public void ChangeBrushSize(int size)
        {
            //DrawingInkCanvas.DefaultDrawingAttributes.Width = size;
            //DrawingInkCanvas.DefaultDrawingAttributes.Height = size;
            //TransparentInkCanvas.DefaultDrawingAttributes.Width = size;
            //TransparentInkCanvas.DefaultDrawingAttributes.Height = size;
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
            //bitmap.Render(DrawingInkCanvas);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }

        private void Save(object sender, EventArgs args)
        {
            //SaveToPng(BackgroundImage, "C:/test/jk.png");
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
