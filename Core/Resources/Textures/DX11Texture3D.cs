using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;
using System.Runtime.InteropServices;

namespace FeralTic.DX11.Resources
{
    public unsafe class DX11Texture3D : IDxTexture3D , IDisposable
    {
        public Texture3D Texture { get; private set; }
        public ShaderResourceView ShaderView { get; private set; }
        private Texture3DDescription description;

        public int Width 
        { 
            get { return this.description.Width; }        
        }

        public int Height
        {
            get { return this.description.Height; }
        }

        public int Depth
        {
            get { return this.description.Depth; }
        }

        public Format Format
        {
            get { return this.description.Format; }
        }

        public static DX11Texture3D FromReference(DxDevice device, Texture3D texture, ShaderResourceView view)
        {
            DX11Texture3D result = new DX11Texture3D();
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
