using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ProjectDesigner.ShaderEffects
{
    //TODO this needs to take into account Scaling, A setting for Point size and for grid spacing, also would be nice if I could change the colors on the fly

    //Start here for adding DependencyProperties https://bytelanguage.com/2018/11/23/passing-additional-parameters/
    public class CheckerboardShader : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(CheckerboardShader), 0);
        public Brush Input
        {

            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        public CheckerboardShader()
        {
            PixelShader pixelShader = new PixelShader();
            Uri compiledShaderPath = new Uri(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\CheckerboardShader.ps");
            pixelShader.UriSource = compiledShaderPath;

            PixelShader = pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(PixDivProperty);
            UpdateShaderValue(Color1Property);
            UpdateShaderValue(Color2Property);
        }

        public float PixDiv
        {
            get { return (float)GetValue(PixDivProperty); }
            set { SetValue(PixDivProperty, value); }
        }

        public static readonly DependencyProperty PixDivProperty =
        DependencyProperty.Register("PixDiv", typeof(float), typeof(CheckerboardShader), new PropertyMetadata(0.1f, PixelShaderConstantCallback(0)));


        public Color Color1
        {
            get { return (Color)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        //The PixelShaderConstantCallback number should match the one in the shader
        public static readonly DependencyProperty Color1Property =
        DependencyProperty.Register("Color1", typeof(Color), typeof(CheckerboardShader), new PropertyMetadata(Color.FromRgb(232, 231, 230), PixelShaderConstantCallback(1)));

        public Color Color2
        {
            get { return (Color)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        //The PixelShaderConstantCallback number should match the one in the shader
        public static readonly DependencyProperty Color2Property =
        DependencyProperty.Register("Color2", typeof(Color), typeof(CheckerboardShader), new PropertyMetadata(Color.FromRgb(156, 153, 151), PixelShaderConstantCallback(2)));
    }
}
