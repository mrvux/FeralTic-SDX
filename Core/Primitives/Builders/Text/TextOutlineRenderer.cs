using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D2DFactory = SharpDX.Direct2D1.Factory1;
using D2DGeometry = SharpDX.Direct2D1.Geometry;

namespace FeralTic.Primitives.Builders.Text
{
    public class TextOutlineRenderer : SharpDX.DirectWrite.TextRendererBase
    {
        private readonly D2DFactory factory;
        private SharpDX.Direct2D1.Geometry geometry = null;

        public TextOutlineRenderer(D2DFactory factory)
        {
            this.factory = factory;
        }


        public override SharpDX.Result DrawGlyphRun(object clientDrawingContext, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode, GlyphRun glyphRun, GlyphRunDescription glyphRunDescription, SharpDX.ComObject clientDrawingEffect)
        {
            PathGeometry pg = new PathGeometry(this.factory);

            GeometrySink sink = pg.Open();

            glyphRun.FontFace.GetGlyphRunOutline(glyphRun.FontSize, glyphRun.Indices, glyphRun.Advances, glyphRun.Offsets, glyphRun.IsSideways, glyphRun.BidiLevel % 2 == 1, sink as SimplifiedGeometrySink);

            sink.Close();

            TransformedGeometry tg = new TransformedGeometry(this.factory, pg, Matrix3x2.Translation(baselineOriginX, baselineOriginY));
            pg.Dispose();

            //Transform from baseline

            this.AddGeometry(tg);

            return SharpDX.Result.Ok;
        }

        public override SharpDX.Mathematics.Interop.RawMatrix3x2 GetCurrentTransform(object clientDrawingContext)
        {
            return SharpDX.Matrix3x2.Identity;
        }

        public override bool IsPixelSnappingDisabled(object clientDrawingContext)
        {
            return true;
        }

        public override float GetPixelsPerDip(object clientDrawingContext)
        {
            return 1.0f;
        }

        public SharpDX.Direct2D1.Geometry GetGeometry()
        {
            return this.geometry;
        }

        protected void AddGeometry(D2DGeometry geom)
        {
            if (this.geometry == null)
            {
                this.geometry = geom;
            }
            else
            {
                PathGeometry pg = new PathGeometry(this.factory);

                GeometrySink sink = pg.Open();

                this.geometry.Combine(geom, CombineMode.Union, sink);

                sink.Close();

                this.geometry = pg;
            }
        }
    }
}
