using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.Direct3D;

namespace FeralTic.DX11
{
    /// <summary>
    /// Base interface for any geometry
    /// </summary>
    public interface IDX11Geometry : IDxResource, IDisposable
    {
        /// <summary>
        /// Default topology for our geometry
        /// </summary>
        PrimitiveTopology Topology { get; set; }

        InputElement[] InputLayout { get; set; }

        /// <summary>
        /// Creates an input layout 
        /// </summary>
        /// <param name="pass">Effect pass to validate layout on</param>
        /// <param name="layout">Returns validate layout, or null if not valid</param>
        /// <returns>true if layout valid, false otherwise</returns>
        bool ValidateLayout(EffectPass pass,out InputLayout layout);

        void Bind(RenderContext ctx, InputLayout layout);

        void Draw(RenderContext ctx);

        BoundingBox BoundingBox { get; set; }

        bool HasBoundingBox { get; set; }

        IDX11Geometry ShallowCopy();

        string PrimitiveType { get; set; }

        object Tag { get; set; }
    }
}
