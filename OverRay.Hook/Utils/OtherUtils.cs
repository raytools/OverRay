using System;

namespace OverRay.Hook.Utils
{
    public static class OtherUtils
    {
        public static Version Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public static string GetVersion = $"{Version.Major}.{Version.Minor}.{Version.Build}";
    }
}