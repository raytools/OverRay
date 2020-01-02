namespace OverRay.Hook.Utils
{
    public static class Text
    {
        public static string Red(this string s) => "/O200:" + s + "/O0:";
        public static string Yellow(this string s) => "/O400:" + s + "/O0:";

        public static string NL(this string s) => s + "/l:";
        public static string Arrow = "\\";

        public static string Float(this float f) => f.ToString("0.000");
        public static string Double(this double d) => d.ToString("0.000");
        public static string KeyValue(this string k, string v) => k.Red() + Arrow + v;
    }
}