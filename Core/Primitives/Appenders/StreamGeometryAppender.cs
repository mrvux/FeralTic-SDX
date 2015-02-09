using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class StreamGeometryAppender : IDisposable
    {
        private DataStream vertexStream;
        private DataStream indexStream;

        public DataStream VertexStream
        {
            get { return this.vertexStream; }
        }

        public DataStream IndexStream
        {
            get { return this.indexStream; }
        }

        public StreamGeometryAppender(int verticesCount, int indicesCount)
        {
            int vsize = Marshal.SizeOf(typeof(Pos4Norm3Tex2Vertex));
            this.vertexStream = new DataStream(verticesCount * vsize, true, true);
            this.indexStream = new DataStream(indicesCount * sizeof(int), true, true);
        }

        public void AppendVertex(Vector3 position, Vector3 normal, Vector2 uv)
        {
            Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex()
            {
                Position = new Vector4(position.X, position.Y, position.Z, 1.0f),
                Normals = normal,
                TexCoords = uv
            };
            this.vertexStream.Write(v);
        }

        public void AppendIndex(Int3 index)
        {
            this.indexStream.Write(index);
        }

        public void Dispose()
        {
            this.vertexStream.Dispose();
            this.indexStream.Dispose();
        }
    }
}
