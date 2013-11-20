using FeralTic.DX11.Resources;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class TextureUtils
    {
        public static DX11Texture2D SolidColor(DX11Device device, Color4 color)
        {
            DataStream ds = new DataStream(4, true, true);
            ds.Write<int>(color.ToRgba());

            Texture2DDescription td = new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.R8G8B8A8_UNorm,
                Height = 1,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Immutable,
                Width = 1
            };

            DataBox db = new DataBox(ds.DataPointer);
            db.RowPitch = 4;
            Texture2D texture = new Texture2D(device.Device, td, new DataBox[1] { db });
            ds.Dispose();
            ShaderResourceView srv = new ShaderResourceView(device.Device, texture);
            return DX11Texture2D.FromReference(device, texture, srv);
        }
    }
}
