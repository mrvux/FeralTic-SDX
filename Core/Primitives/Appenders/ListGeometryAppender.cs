using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class ListGeometryAppender
    {
        private List<Pos4Norm3Tex2Vertex> vertices = new List<Pos4Norm3Tex2Vertex>();
        private List<int> indices = new List<int>();

        public List<Pos4Norm3Tex2Vertex> Vertices { get { return this.vertices; } }
        public List<int> Indices { get { return this.indices; } }

        public void AppendVertex(Vector3 position, Vector3 normal, Vector2 uv)
        {
            Pos4Norm3Tex2Vertex v = new Pos4Norm3Tex2Vertex()
            {
                Position = new Vector4(position.X, position.Y, position.Z, 1.0f),
                Normals = normal,
                TexCoords = uv
            };
            this.vertices.Add(v);
        }

        public void AppendIndex(Int3 index)
        {
            this.indices.Add(index.X);
            this.indices.Add(index.Y);
            this.indices.Add(index.Z);
        }
    }
}
