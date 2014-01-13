using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Addons.AssetImport
{
    public struct AssimpLoadInformation 
    {
        public bool Bones { get; set; }
        public bool Normals { get; set; }
        public bool Tangents { get; set; }
        public bool TexCoords { get; set; }
        public bool VertexColors { get; set; }

        public static AssimpLoadInformation All
        {
            get
            {
                return new AssimpLoadInformation()
                {
                    Bones = true,
                    Normals = true,
                    Tangents = true,
                    TexCoords = true,
                    VertexColors = true
                };
            }
        }

        public static AssimpLoadInformation AllButBones
        {
            get
            {
                return new AssimpLoadInformation()
                {
                    Bones = false,
                    Normals = true,
                    Tangents = true,
                    TexCoords = true,
                    VertexColors = true
                };
            }
        }
    }
}
