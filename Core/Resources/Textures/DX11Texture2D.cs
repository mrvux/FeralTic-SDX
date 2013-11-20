using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;

namespace FeralTic.DX11.Resources
{
    public class DX11Texture2D : IDX11Texture2D , IDisposable
    {
        public Texture2D Texture { get; private set; }
        public ShaderResourceView ShaderView { get; private set; }
        private Texture2DDescription description;

        public int Width 
        { 
            get { return this.description.Width; }        
        }

        public int Height
        {
            get { return this.description.Height; }
        }

        public Format Format
        {
            get { return this.description.Format; }
        }

        public static DX11Texture2D FromReference(DX11Device device, Texture2D texture, ShaderResourceView view)
        {
            DX11Texture2D result = new DX11Texture2D();
            result.description = texture.Description;
            result.ShaderView = view;
            result.Texture = texture;
            return result;
        }

        public void Dispose()
        {
            if (this.ShaderView != null) { this.ShaderView.Dispose(); }
            if (this.Texture != null) { this.Texture.Dispose(); }
        }
    }
}
