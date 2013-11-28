using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FeralTic.DX11.Queries
{
    public delegate void DX11QueryableDelegate(DX11RenderContext context);

    public interface IDX11Query
    {
        void Start(DX11RenderContext context);
        void Stop(DX11RenderContext context);
        void GetData(DX11RenderContext context);
    }

    public interface IDX11Queryable
    {
        event DX11QueryableDelegate BeginQuery;
        event DX11QueryableDelegate EndQuery;
    }
}
