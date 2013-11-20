using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic
{
    public interface IDX11Resource
    {
    }

    public interface IDX11ShaderResource : IDX11Resource
    {
        ShaderResourceView ShaderView { get; }
    }

    public interface IDX11UnorderedResource : IDX11ShaderResource
    {
        UnorderedAccessView UnorderedView { get; }
    }

    public interface IDX11Texture3D : IDX11ShaderResource
    {
        Texture3D Texture { get; }
        SharpDX.DXGI.Format Format { get; }
        int Width { get; }
        int Height { get; }
        int Depth { get; }
    }

    public interface IDX11Texture2D : IDX11ShaderResource
    {
        Texture2D Texture { get; }
        SharpDX.DXGI.Format Format { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface IDX11Texture1D : IDX11ShaderResource
    {
        Texture1D Texture { get; }
        SharpDX.DXGI.Format Format { get; }
        int Width { get; }
    }


    public interface IDX11DepthStencil : IDX11ShaderResource
    {
        DepthStencilView DepthView { get; }
        DepthStencilView ReadOnlyView { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface IDX11RenderTarget : IDisposable
    {
        RenderTargetView RenderView { get; }
        int Width { get; }
        int Height { get; }
    }
}
