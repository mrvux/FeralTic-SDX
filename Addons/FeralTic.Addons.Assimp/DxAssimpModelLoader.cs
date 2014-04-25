using Assimp;
using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Addons.AssetImport
{
    public class DxAssimpModelLoader
    {
        private DxDevice device;

        public DxAssimpModelLoader(DxDevice device)
        {
            this.device = device;
        }

        public DX11IndexedGeometry LoadFromMesh(Assimp.Mesh mesh, AssimpLoadInformation loadInfo)
        {
            uint[] inds = mesh.GetIndices();

            if (inds.Length > 0 && mesh.VertexCount > 0)
            {
                int vertexsize;
                var layout = mesh.InputLayout(loadInfo, out vertexsize);

                BoundingBox bb;
                DataStream ds = mesh.LoadVertices(loadInfo, vertexsize, out bb);

                DX11IndexedGeometry geom = new DX11IndexedGeometry(device)
                {
                    HasBoundingBox = true,
                    BoundingBox = bb,
                    IndexBuffer = DX11IndexBuffer.CreateImmutable(device, inds),
                    InputLayout = layout,
                    PrimitiveType = "AssimpModel",
                    Tag = null,
                    Topology = SharpDX.Direct3D.PrimitiveTopology.TriangleList,
                    VertexBuffer = DX11VertexBuffer.CreateImmutable(device, mesh.VertexCount, vertexsize, ds)
                };

                ds.Dispose();
                return geom;
            }

            return null;
        }

        public List<DX11IndexedGeometry> LoadModelsFromFile(string path, AssimpLoadInformation loadInfo)
        {
            Assimp.AssimpImporter importer = new AssimpImporter();
            Scene scene = importer.ImportFile(path, PostProcessPreset.TargetRealTimeQuality | PostProcessPreset.TargetRealTimeQuality);

            List<DX11IndexedGeometry> result = new List<DX11IndexedGeometry>();
            for (int j = 0; j < scene.MeshCount; j++)
            {
                Assimp.Mesh mesh = scene.Meshes[j];

                if (mesh.HasFaces && mesh.HasVertices)
                {
                    result.Add(this.LoadFromMesh(mesh, loadInfo));
                }
            }
            return result;
        }

        public void ConstructFirst(string path, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex)
        {
            Assimp.AssimpImporter importer = new AssimpImporter();
            Scene scene = importer.ImportFile(path, PostProcessPreset.ConvertToLeftHanded | PostProcessPreset.TargetRealTimeQuality);

            Assimp.Mesh mesh = scene.Meshes[0];

            if (mesh.HasFaces && mesh.HasVertices)
            {
                 for (int i= 0; i< mesh.VertexCount;i++)
                 {
                     Vector3D mp = mesh.Vertices[i];
                     Vector3 p = new Vector3(mp.X,mp.Y,mp.Z);

                     Vector3D mn = mesh.Normals[i];
                     Vector3 n = new Vector3(mn.X,mn.Y,mn.Z);
                     appendVertex(p, n, Vector2.Zero);
                 }

                int[] inds = mesh.GetIntIndices();
                for(int i = 0; i < inds.Length / 3 ;i+=3)
                {
                    appendIndex(new Int3(inds[i * 3], inds[i * 3 + 1], inds[i * 3 + 2]));
                }
                 
            }
        }
    }
}
