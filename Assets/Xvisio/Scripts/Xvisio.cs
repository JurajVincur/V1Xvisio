using System;
using System.Runtime.InteropServices;

public static class Xvisio
{
    [DllImport("XvisioService.dll")]
    public static extern void CStart(string json);

    [DllImport("XvisioService.dll")]
    public static extern void CStop();

    [DllImport("XvisioService.dll")]
    public static extern IntPtr CGetPose();

    public static void Start(string json = "")
    {
        CStart(json);
    }

    public static void Stop()
    {
        CStop();
    }

    public static bool GetPose(float[] pose)
    {
        IntPtr ptr = CGetPose();
        if (ptr != IntPtr.Zero)
        {
            Marshal.Copy(ptr, pose, 0, 12);
            return true;
        }
        return false;
    }
}
