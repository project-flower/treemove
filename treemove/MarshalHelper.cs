using System;
using System.Runtime.InteropServices;

namespace treemove
{
    public abstract class ComSupporter
    {
        public static T GetObjectForIUnknown<T>(IntPtr pUnk)
        {
            // Use GetTypedObjectForIUnknown to ensure the returned RCW is typed to the requested interface
            return (T)Marshal.GetTypedObjectForIUnknown(pUnk, typeof(T));
        }

        public static void ThrowIfFailed(int hResult)
        {
            if (hResult != 0) Marshal.ThrowExceptionForHR(hResult);
        }
    }

    public static class MarshalHelper
    {
        public static void Release(this IntPtr pointer)
        {
            if (pointer != IntPtr.Zero) Marshal.Release(pointer);
        }

        public static void Release(this object comObject)
        {
            if (comObject != null) Marshal.ReleaseComObject(comObject);
        }
    }
}
