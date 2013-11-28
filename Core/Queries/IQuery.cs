using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FeralTic.DX11.Queries
{
    public delegate void DxQueryableDelegate(DX11RenderContext context);

    public interface IDxQuery
    {
        void Start(DX11RenderContext context);
        void Stop(DX11RenderContext context);
        void GetData(DX11RenderContext context);
    }

    public interface IDxQueryable
    {
        event DxQueryableDelegate BeginQuery;
        event DxQueryableDelegate EndQuery;
    }
}
