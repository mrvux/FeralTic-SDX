using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if DIRECTX11_2
using DXGIDevice = SharpDX.DXGI.Device2;
using DXGIAdapter = SharpDX.DXGI.Adapter2;
using DXGIFactory = SharpDX.DXGI.Factory2;
using WICFactory = SharpDX.WIC.ImagingFactory2;
using DirectXDevice = SharpDX.Direct3D11.Device2;
using D2DFactory = SharpDX.Direct2D1.Factory1;
#else
#if DIRECTX11_1
using DXGIDevice = SharpDX.DXGI.Device2;
using DXGIAdapter = SharpDX.DXGI.Adapter2;
using DXGIFactory = SharpDX.DXGI.Factory2;
using WICFactory = SharpDX.WIC.ImagingFactory2;
using DirectXDevice = SharpDX.Direct3D11.Device1;
using D2DFactory = SharpDX.Direct2D1.Factory1;
#else
using DXGIDevice = SharpDX.DXGI.Device1;
using DXGIAdapter = SharpDX.DXGI.Adapter1;
using DXGIFactory = SharpDX.DXGI.Factory1;
using WICFactory = SharpDX.WIC.ImagingFactory;
using DirectXDevice = SharpDX.Direct3D11.Device;
#endif
#endif

namespace FeralTic.DX11
{
    public delegate void DeviceDelegate(DxDevice sender);

    /// <summary>
    /// DirectX device small wrapper
    /// </summary>
    public class DxDevice : IDisposable
    {
        /// <summary>
        /// DirectX Device
        /// </summary>
        public DirectXDevice Device { get; private set; }

        /// <summary>
        /// Adapter our device is running on
        /// </summary>
        public DXGIAdapter Adapter { get; private set; }

        public DXGIFactory Factory { get; private set; }

        public WICFactory WICFactory { get; private set; }

        public D2DFactory D2DFactory { get; private set; }

        public bool AutoReset { get; set; }

        public event DeviceDelegate DeviceRemoved;
        public event DeviceDelegate DeviceReset;
        public event DeviceDelegate DeviceDisposing;
        public event DeviceDelegate DeviceDisposed;

        private DeviceCreationFlags creationflags;
        private int adapterindex;

        /// <summary>
        /// Returns true if device is feature Level 11.1 (fully), false otherwise
        /// </summary>
        public bool IsFeatureLevel11_1
        {
            get 
            { 
                #if DIRECTX11_1
                return this.Device.FeatureLevel >= FeatureLevel.Level_11_1; 
                #else
                return false;
                #endif
            }
        }

        public bool IsFeatureLevel11
        {
            get { return this.Device.FeatureLevel >= FeatureLevel.Level_11_0; }
        }


        public bool IsFeatureLevel101
        {
            get { return this.Device.FeatureLevel >= FeatureLevel.Level_10_1; }
        }

        public static implicit operator DirectXDevice(DxDevice device)
        {
            return device.Device;
        }

        public DxDevice(DeviceCreationFlags flags = DeviceCreationFlags.BgraSupport, int adapterindex = 0)
        {
            this.WICFactory = new WICFactory();
            this.D2DFactory = new D2DFactory();
            this.creationflags = flags;
            this.adapterindex = adapterindex;
            this.Initialize();
        }

        #region Initialize
        private void Initialize()
        {
            FeatureLevel[] levels = new FeatureLevel[]
            {
                #if DIRECTX11_1
                 FeatureLevel.Level_11_1,
                #endif
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3
            };

            Device dev;
            if (adapterindex > 0)
            {
                SharpDX.DXGI.Factory f = new SharpDX.DXGI.Factory();
                SharpDX.DXGI.Adapter a = f.GetAdapter(adapterindex);

                dev = new Device(a, this.creationflags, levels);

                f.Dispose();
                a.Dispose();
            }
            else
            {
                dev = new Device(DriverType.Hardware, this.creationflags, levels);
            }

            #if DIRECTX11_1
            this.Device = dev.QueryInterface<DirectXDevice>();
            #else
            this.Device = dev;
            #endif
            
            DXGIDevice dxgidevice = this.Device.QueryInterface<DXGIDevice>();
            
            this.Adapter = dxgidevice.Adapter.QueryInterface<DXGIAdapter>();
            this.Factory = this.Adapter.GetParent<DXGIFactory>();

            this.OnLoad();
        }
        #endregion

        protected virtual void OnLoad() { }
        protected virtual void OnDeviceRemoved() { }
        protected virtual void OnDispose() { }

        internal void NotifyDeviceLost()
        {
            this.OnDeviceRemoved();

            if (this.DeviceRemoved != null) { this.DeviceRemoved(this); }

            if (this.AutoReset)
            {
                this.Initialize();
                if (this.DeviceReset != null) { this.DeviceReset(this); }
            }
        }

        public void Dispose()
        {
            this.OnDispose();

            if (this.DeviceDisposing != null) { this.DeviceDisposing(this); }

            this.Device.Dispose();

            if (this.DeviceDisposed != null) { this.DeviceDisposed(this); }
        }

    }
}