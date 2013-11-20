using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Queries
{

    public class DX11StreamOutQuery : IDX11Query
    {
        private DX11Device device;

        private Query query;

        public bool hasrun = false;


        public StreamOutputStatistics Statistics { get; protected set; }

        public DX11StreamOutQuery(DX11Device device)
        {
            this.device = device;

            QueryDescription qd = new QueryDescription();
            qd.Flags = QueryFlags.None;
            qd.Type = QueryType.StreamOutputStatistics;

            this.query = new Query(device.Device, qd);
        }

        public void Start(DX11RenderContext context)
        {
            context.Context.Begin(query);
        }

        public void Stop(DX11RenderContext context)
        {
            context.Context.End(query);
            this.hasrun = true;
        }

        public void GetData(DX11RenderContext context)
        {
            if (this.hasrun == false) { return; }

            while (!context.Context.IsDataAvailable(this.query)) { }

            this.Statistics = context.Context.GetData<StreamOutputStatistics>(this.query);
        }

    }
}
