using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;


namespace FeralTic.DX11.Resources
{
    public abstract class DX11BaseGeometry : IDxGeometry
    {
        protected DxDevice device;

        public DX11BaseGeometry(DxDevice device, bool resourceowner = true)
        {
            this.device = device;
        }

        internal DX11BaseGeometry()
        {

        }

        /// <summary>
        /// Vertex Input Layout
        /// </summary>
        public virtual InputElement[] InputLayout { get; set; }


        public PrimitiveTopology Topology
        {
            get; set;
        }

        public bool ValidateLayout(ShaderSignature inputSignature, out InputLayout layout)
        {
            layout = null;
            if (inputSignature == null)
            {
                return true;
            }
            try
            {
                
                /*bool allownull = true;
                if (pass.VertexShaderDescription.Variable != null)
                {
                    EffectShaderVariable shader = pass.VertexShaderDescription.Variable;
                    if (shader.IsValid)
                    {
                        for (int i = 0; i < shader.GetShaderDescription(0).InputParameterCount; i++)
                        {
                            if (shader.GetInputSignatureElementDescription(0, i).SystemValueType == SharpDX.D3DCompiler.SystemValueType.Undefined)
                            {
                                allownull = false;
                            }
                        }
                    }

                    try
                    {
                        var sb = pass.Description.Signature;
                    }
                    catch
                    {
                        //If no signature, assume it's compute only
                        allownull = true;
                    }

                }

                if (allownull)
                {
                    layout = null;
                    return true;
                }*/
                layout = new InputLayout(device.Device, inputSignature, this.InputLayout);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BoundingBox BoundingBox { get; set; }

        public bool HasBoundingBox { get; set; }

        public abstract void Draw(RenderContext context);

        public abstract void Bind(RenderContext context, InputLayout layout);

        public abstract void Dispose();

        public abstract IDxGeometry ShallowCopy();

        public object Tag { get; set; }

        public string PrimitiveType { get; set; }
    }
}
