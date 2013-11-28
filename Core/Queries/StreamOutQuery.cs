using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;

namespace FeralTic.DX11.Queries
{

    public class StreamOutQuery : IDxQuery
    {
        private DxDevice device;

        private Query query;

        public bool hasrun = false;

        private readonly int WAIT_MAX = 1024;

        public StreamOutputStatistics Statistics { get; protected set; }

        public StreamOutQuery(DxDevice device)
        {
            this.device = device;

            QueryDescription qd = new QueryDescription();
            qd.Flags = QueryFlags.None;
            qd.Type = QueryType.StreamOutputStatistics;

            this.query = new Query(device.Device, qd);
        }

        public void Start(RenderContext context)
        {
            context.Context.Begin(query);
        }

        public void Stop(RenderContext context)
        {
            context.Context.End(query);
            this.hasrun = true;
        }

        public void GetData(RenderContext context)
        {
            if (this.hasrun == false) { return; }

            for (int i = 0; i < WAIT_MAX; i++ )
            {
                if (context.Context.IsDataAvailable(this.query))
                {
                    this.Statistics = context.Context.GetData<StreamOutputStatistics>(this.query);
                    return;
                }
            }
        }


        public void Dispose()
        {
            this.query.Dispose();
        }

    }
}
