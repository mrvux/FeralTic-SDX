using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FeralTic.Utils
{
    public class MergeUtils
    {
        public DataStream Interleave<T1,T2>(T1[] data1, T2[] data2) where T1 : struct where T2 : struct
        {
            if (data1.Length != data2.Length)
            {
                throw new ArgumentException("Array Length Mismatch");
            }
            int s1 = Marshal.SizeOf(typeof(T1));
            int s2 = Marshal.SizeOf(typeof(T2));

            DataStream ds = new DataStream((s1 + s2) * data1.Length,true,true);
            for (int i = 0; i < data1.Length; i++)
            {
                ds.Write<T1>(data1[i]);
                ds.Write<T2>(data2[i]);
            }

            ds.Position = 0;
            return ds;
        }
    }
}
