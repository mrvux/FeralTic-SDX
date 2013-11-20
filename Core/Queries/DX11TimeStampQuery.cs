﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;


namespace FeralTic.DX11.Queries
{
    public class DX11TimeStampQuery : IDX11Query
    {
        private Query tstart;
        private Query tend;
        private Query tsdis;

        public float Elapsed;

        public bool hasrun = false;

        private DX11Device device;

        public DX11TimeStampQuery(DX11Device device)
        {
            this.device = device;
            QueryDescription qd = new QueryDescription();
            qd.Type = QueryType.Timestamp;
            qd.Flags = QueryFlags.None;

            this.tstart = new Query(device.Device, qd);
            this.tend = new Query(device.Device, qd);

            QueryDescription qdd = new QueryDescription();
            qdd.Type = QueryType.TimestampDisjoint;
            qdd.Flags = QueryFlags.None;

            this.tsdis = new Query(device.Device, qdd);
        }

        public void Start(DX11RenderContext context)
        {
            context.Context.Begin(this.tsdis);
            context.Context.End(this.tstart);
            //this.hasend = false;
        }

        public void Stop(DX11RenderContext context)
        {
            context.Context.End(this.tend);
            context.Context.End(this.tsdis);
            this.hasrun = true;
        }

        public void GetData(DX11RenderContext context)
        {
            if (this.hasrun == false) { return; }

            DeviceContext ctx = context.Context;

            while (!ctx.IsDataAvailable(this.tstart)) { }
            while (!ctx.IsDataAvailable(this.tend)) { }
            while (!ctx.IsDataAvailable(this.tsdis)) { }

            Int64 startTime = ctx.GetData<Int64>(this.tstart);
            Int64 endTime = ctx.GetData<Int64>(this.tend);
            QueryDataTimestampDisjoint data = ctx.GetData<QueryDataTimestampDisjoint>(this.tsdis);

            float time = 0.0f;
            if (data.Disjoint == false)
            {
                Int64 delta = endTime - startTime;
                float frequency = (float)data.Frequency;
                time = ((float)delta / frequency) * 1000.0f;

                this.Elapsed = time;
            }   
        }
    }
}
