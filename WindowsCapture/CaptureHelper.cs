using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using Windows.Graphics.Capture;
using WinRT;

namespace WindowsCapture;

public static class CaptureHelper
{
  //  static readonly Guid GraphicsCaptureItemGuid = new Guid("79C3F95B-31F7-4EC2-A464-632EF5D30760");

    [ComImport]
    [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface IInitializeWithWindow
    {
        void Initialize(
            IntPtr hwnd);
    }

    public static void SetWindow(this GraphicsCapturePicker picker, IntPtr hwnd)
    {
        var interop = picker.As<IInitializeWithWindow>();
        interop.Initialize(hwnd);
    }





    [ComImport]
    [Guid("3628E81B-3CAC-4C60-B7F4-23CE0E0C3356")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface IGraphicsCaptureItemInterop
    {
        IntPtr CreateForWindow(
            [In] IntPtr window,
            [In] ref Guid iid);

        IntPtr CreateForMonitor(
            [In] IntPtr monitor,
            [In] ref Guid iid);
    }
    private static readonly IGraphicsCaptureItemInterop interop = GraphicsCaptureItem.As<IGraphicsCaptureItemInterop>();

    internal static GraphicsCaptureItem CreateForWindow(IntPtr hWindow)
    {
        var GraphicsCaptureItemGuid = new Guid("79C3F95B-31F7-4EC2-A464-632EF5D30760");
        var ptr = interop.CreateForWindow(hWindow, ref GraphicsCaptureItemGuid);
        try
        {
            return GraphicsCaptureItem.FromAbi(ptr);
        }
        finally
        {
            Marshal.Release(ptr);
        }
    }
    public static GraphicsCaptureItem CreateItemForMonitor(IntPtr hmon)
    {
        var GraphicsCaptureItemGuid = new Guid("79C3F95B-31F7-4EC2-A464-632EF5D30760");
        var ptr = interop.CreateForMonitor(hmon, ref GraphicsCaptureItemGuid);
        try
        {
            return GraphicsCaptureItem.FromAbi(ptr);
        }
        finally
        {
            Marshal.Release(ptr);
        }

    }

    //[SupportedOSPlatform("windows")]
    //public static GraphicsCaptureItem CreateItemForWindow(IntPtr hwnd)
    //{
    //    var factory = WindowsRuntimeMarshal.GetActivationFactory(typeof(GraphicsCaptureItem));
    //    var interop = (IGraphicsCaptureItemInterop)factory;
    //    var temp = typeof(GraphicsCaptureItem);
    //    var itemPointer = interop.CreateForWindow(hwnd, GraphicsCaptureItemGuid);
    //    var item = Marshal.GetObjectForIUnknown(itemPointer) as GraphicsCaptureItem;
    //    Marshal.Release(itemPointer);

    //    return item;
    //}

    //[SupportedOSPlatform("windows")]
    //public static GraphicsCaptureItem CreateItemForMonitor(IntPtr hmon)
    //{
    //   var factory = WindowsRuntimeMarshal.GetActivationFactory(typeof(GraphicsCaptureItem));
    //    var interop = (IGraphicsCaptureItemInterop)factory;
    //    var temp = typeof(GraphicsCaptureItem);
    //    var itemPointer = interop.CreateForMonitor(hmon, GraphicsCaptureItemGuid);
    //    var item = Marshal.GetObjectForIUnknown(itemPointer) as GraphicsCaptureItem;
    //    Marshal.Release(itemPointer);

    //    return item;
    //}
}
