using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace FeralTic.DX11
{
    public interface IDxResource : IDisposable
    {
    }

    public interface IDxBuffer : IDxResource
    {
        Buffer Buffer { get; }
    }

    public interface IDxShaderResource : IDxResource
    {
        ShaderResourceView ShaderView { get; }
    }

    public interface IDxUnorderedResource : IDxShaderResource
    {
        UnorderedAccessView UnorderedView { get; }
    }

    public interface IDxTexture3D : IDxShaderResource
    {
        Texture3D Texture { get; }
        SharpDX.DXGI.Format Format { get; }
        int Width { get; }
        int Height { get; }
        int Depth { get; }
    }

    public interface IDxTexture2D : IDxShaderResource
    {
        Texture2D Texture { get; }
        SharpDX.DXGI.Format Format { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface IDxTexture1D : IDxShaderResource
    {
        Texture1D Texture { get; }
        SharpDX.DXGI.Format Format { get; }
        int Width { get; }
    }


    public interface IDxDepthStencil : IDxShaderResource
    {
        DepthStencilView DepthView { get; }
        DepthStencilView ReadOnlyView { get; }
        int Width { get; }
        int Height { get; }
    }

    public interface IDxRenderTarget : IDxResource, IDisposable
    {
        RenderTargetView RenderView { get; }
        int Width { get; }
        int Height { get; }
    }
}
