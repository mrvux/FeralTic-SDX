using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public class DepthStencilStates : RenderStates<DepthStencilState>
    {
        private DxDevice device;

        public DepthStencilState Disabled { get; private set; }
        public DepthStencilState LessReadWrite { get; private set; }
        public DepthStencilState LessReadOnly { get; private set; }
        public DepthStencilState LessEqualReadWrite { get; private set; }
        public DepthStencilState LessEqualReadOnly{ get; private set; }
        public DepthStencilState WriteOnly { get; private set; }
        public DepthStencilState LessROStencilIncrement { get; private set; }
        public DepthStencilState LessStencilIncrement { get; private set; }
        public DepthStencilState StencilDepthLessEqual { get; private set; }
        public DepthStencilState StencilDepthLessRW { get; private set; }
        public DepthStencilState StencilLess { get; private set; }
        public DepthStencilState StencilGreater { get; private set; }
        public DepthStencilState StencilLessEqual { get; private set; }
        public DepthStencilState StencilIncrement { get; private set; }
        public DepthStencilState StencilInvert { get; private set; }
        public DepthStencilState StencilZero { get; private set; }
        public DepthStencilState StencilNotEqual { get; private set; }


        public DepthStencilStates(DxDevice device)
        {
            this.device = device;
            this.Initialize();
        }

        public override string EnumName
        {
            get
            {
                return "DX11.DepthStencilPresets";
            }
        }

        protected override void Initialize()
        {
            this.CreateLessReadOnly();
            this.CreateLessRW();
            this.CreateNoDepth();
            this.CreateLessEqualReadOnly();
            this.CreateLessEqualRW();
            this.CreateWriteOnly();
            this.CreateLessStencilIncrement();
            this.CreateLessROStencilIncrement();
            this.CreateStencilLess();
            this.CreateStencilLessEqual();
            this.CreateStencilGreater();
            this.CreateStencilIncrement();
            this.CreateStencilInvert();
            this.CreateLessStencilZero();

        }



        private void CreateNoDepth()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always
            };

            this.Disabled = new DepthStencilState(device.Device, ds);
            this.AddState("NoDepth", this.Disabled);
        }

        private void CreateLessReadOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Less
            };

            this.LessReadOnly = new DepthStencilState(device.Device, ds);
            this.AddState("LessRead",this.LessReadOnly );
        }

        private void CreateLessEqualReadOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.LessEqual
            };

            this.LessEqualReadOnly = new DepthStencilState(device.Device, ds);
            this.AddState("LessEqualRead", this.LessEqualReadOnly);
        }

        private void CreateLessRW()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less
            };
            this.LessReadWrite = new DepthStencilState(device.Device, ds);
            this.AddState("LessReadWrite", this.LessReadWrite);
        }

        private void CreateLessEqualRW()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.LessEqual
            };

            this.LessEqualReadWrite = new DepthStencilState(device.Device, ds);
            this.AddState("LessEqualReadWrite", this.LessEqualReadWrite);
        }

        private void CreateWriteOnly()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Always
            };

            this.WriteOnly = new DepthStencilState(device.Device, ds);
            this.AddState("WriteOnly", this.WriteOnly);
        }

        private void CreateLessStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };

            this.LessStencilIncrement = new DepthStencilState(device.Device, ds);
            this.AddState("LessReadStencilIncrement", this.LessStencilIncrement);
        }

        private void CreateLessROStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };

            this.LessROStencilIncrement = new DepthStencilState(device.Device, ds);
            this.AddState("LessROStencilIncrement", this.LessROStencilIncrement);
        }

        private void CreateLessStencilZero()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Zero
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Zero
                }
            };

            this.StencilZero = new DepthStencilState(device.Device, ds);
            this.AddState("LessReadStencilZero", this.StencilZero);
        }

        private void CreateStencilLess()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            this.StencilLess = new DepthStencilState(device.Device, ds);
            this.AddState("StencilLess", this.StencilLess);
       }

        private void CreateStencilLessEqual()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.LessEqual,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.LessEqual,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.LessEqual,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            this.StencilDepthLessEqual = new DepthStencilState(device.Device, ds);
            this.AddState("StencilLessEqual", this.StencilDepthLessEqual);
        }

        private void CreateDepthStencilLess()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Less,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            this.StencilDepthLessRW = new DepthStencilState(device.Device, ds);
            this.AddState("StencilDepthLessRW", this.StencilDepthLessRW);
        }

        private void CreateStencilGreater()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Greater,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Greater,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            this.StencilGreater = new DepthStencilState(device.Device, ds);

            ds.FrontFace.Comparison = Comparison.LessEqual;
            ds.BackFace.Comparison = Comparison.LessEqual;

            this.StencilLessEqual = new DepthStencilState(device.Device, ds);

            this.AddState("StencilGreater", this.StencilGreater);

            ds.FrontFace.Comparison = Comparison.NotEqual;
            ds.BackFace.Comparison = Comparison.NotEqual;
            this.StencilNotEqual = new DepthStencilState(device.Device, ds);
        }


        private void CreateStencilIncrement()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.IncrementAndClamp
                }
            };

            this.StencilIncrement = new DepthStencilState(device.Device, ds);
            this.AddState("StencilIncrement", this.StencilIncrement);
        }

        private void CreateStencilInvert()
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = 255,
                StencilWriteMask = 255,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Invert
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Invert
                }
            };

            this.StencilInvert = new DepthStencilState(device.Device, ds);
            this.AddState("StencilInvert", this.StencilInvert);
        }

        public DepthStencilState LessWriteMask(byte mask)
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                StencilReadMask = 0,
                StencilWriteMask = mask,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Replace
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Replace
                }
            };

            return new DepthStencilState(device.Device, ds);
        }

        public DepthStencilState ReadMask(byte mask)
        {
            DepthStencilStateDescription ds = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                IsStencilEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.Always,
                StencilReadMask = mask,
                StencilWriteMask = 0,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Equal,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = Comparison.Equal,
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep
                }
            };

            return new DepthStencilState(device.Device, ds);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.StencilLessEqual.Dispose();
        }
    }
}
