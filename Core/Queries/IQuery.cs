using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FeralTic.DX11.Queries
{
    public delegate void DxQueryableDelegate(RenderContext context);

    public interface IDxQuery
    {
        void Start(RenderContext context);
        void Stop(RenderContext context);
        void GetData(RenderContext context);
    }

    public interface IDxQueryable
    {
        event DxQueryableDelegate BeginQuery;
        event DxQueryableDelegate EndQuery;
    }
}
