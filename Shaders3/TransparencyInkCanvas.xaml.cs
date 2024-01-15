using ProjectDesigner.ShaderEffects;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shaders3
{
    /// <summary>
    /// Interaction logic for TransparencyInkCanvas.xaml
    /// https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/custom-rendering-ink?view=netframeworkdesktop-4.8
    /// </summary>
    public partial class TransparencyInkCanvas : InkCanvas
    {
        public OpaqueInkCanvas opaqueInk;

        public TransparencyInkCanvas() : base()
        {
            // Use the custom dynamic renderer on the
            // custom InkCanvas.
            //this.DynamicRenderer = customRenderer;
            this.Effect = new CheckerboardShader();
            this.opaqueInk = opaqueInk;
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            //TODO Here this should submit a stroke to the associated InkCanvas for normal rendering. The stroke added to the other ink canvas should be transparent but otherwise match
            base.OnStrokeCollected(e);
            opaqueInk.Strokes.Add(e.Stroke);
        }

    }

    //NOTE: NOTHING BELOW THIS LINE IS USED, LEAVING IT HERE FOR REFERENCE FOR LATER

    //The Dynamic Renderer is used to render the ink as it is being drawn
    public class TransparencyDynamicRenderer : DynamicRenderer
    {
        [ThreadStatic]
        static private Brush brush = null;
        [ThreadStatic]
        static private Brush transparentBrush = null;

        [ThreadStatic]
        static private Pen pen = null;

        private Point prevPoint;

        private CheckerboardShader transparentEffect = new CheckerboardShader();

        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            // Allocate memory to store the previous point to draw from.
            prevPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
            base.OnStylusDown(rawStylusInput);
        }

        public void SetBrush(Brush newBrush)
        {
            brush = newBrush;
        }
        protected override void OnDraw(DrawingContext drawingContext,
                                   StylusPointCollection stylusPoints,
                                   Geometry geometry, Brush fillBrush)
        {
            if (brush!=null && brush.ToString() == "#00FFFFFF")
            {
                // Create a new Brush, if necessary.
                transparentBrush ??= new LinearGradientBrush(Colors.Red, Colors.Blue, 20d);

                // Create a new Pen, if necessary.
                pen ??= new Pen(transparentBrush, 2d);

                // Draw linear gradient ellipses between
                // all the StylusPoints that have come in.
                for (int i = 0; i < stylusPoints.Count; i++)
                {
                    Point pt = (Point)stylusPoints[i];
                    Vector v = Point.Subtract(prevPoint, pt);

                    // Only draw if we are at least 4 units away
                    // from the end of the last ellipse. Otherwise,
                    // we're just redrawing and wasting cycles.
                    if (v.Length > 4)
                    {
                        // Set the thickness of the stroke based
                        // on how hard the user pressed.
                        double radius = stylusPoints[i].PressureFactor * 10d;
                        //TODO
                        
                        drawingContext.DrawEllipse(transparentBrush, pen, pt, radius, radius);
                        prevPoint = pt;
                    }
                }
            }
            else
            {
                drawingContext.DrawGeometry(brush, null, geometry);
            }
        }
    }

    // A class for rendering custom strokes
    //This is responsible for the static rendering of the stroke. This can handle hit tests and also store data in the AddPropertyData method
    class CustomStroke : Stroke
    {
        Brush brush;
        Pen pen;

        public CustomStroke(StylusPointCollection stylusPoints)
            : base(stylusPoints)
        {
            // Create the Brush and Pen used for drawing.
            brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 20d);
            pen = new Pen(brush, 2d);
        }

        protected override void DrawCore(DrawingContext drawingContext,
                                         DrawingAttributes drawingAttributes)
        {
            // Allocate memory to store the previous point to draw from.
            Point prevPoint = new Point(double.NegativeInfinity,
                                        double.NegativeInfinity);

            // Draw linear gradient ellipses between
            // all the StylusPoints in the Stroke.
            for (int i = 0; i < this.StylusPoints.Count; i++)
            {
                Point pt = (Point)this.StylusPoints[i];
                Vector v = Point.Subtract(prevPoint, pt);

                // Only draw if we are at least 4 units away
                // from the end of the last ellipse. Otherwise,
                // we're just redrawing and wasting cycles.
                if (v.Length > 4)
                {
                    // Set the thickness of the stroke
                    // based on how hard the user pressed.
                    double radius = this.StylusPoints[i].PressureFactor * 10d;
                    drawingContext.DrawEllipse(brush, pen, pt, radius, radius);
                    prevPoint = pt;
                }
            }
        }
    }
}
