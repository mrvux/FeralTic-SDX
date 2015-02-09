using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class MultiListGeometryAppender
    {
        private List<Vector3> positions = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();

        private List<Int3> indices = new List<Int3>();

        public List<Vector3> Positions { get { return this.positions; } }
        public List<Vector3> Normals { get { return this.normals; } }
        public List<Vector2> UVs { get { return this.uvs; } }

        public List<Int3> Indices { get { return this.indices; } }

        public void AppendVertex(Vector3 position, Vector3 normal, Vector2 uv)
        {
            this.positions.Add(position);
            this.normals.Add(normal);
            this.uvs.Add(uv);
        }

        public void AppendIndex(Int3 index)
        {
            this.indices.Add(index);
        }
    }
}
