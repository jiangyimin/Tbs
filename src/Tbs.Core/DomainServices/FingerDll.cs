using System.Runtime.InteropServices;

namespace Tbs.DomainServices
{
    public class FingerDll
    {
        [DllImport("F:\\Tbs\\src\\Tbs.Web.Mvc\\bin\\Debug\\netcoreapp1.1\\ARTH_DLL32.dll")]
        public static extern int Match2Fp(byte[] src, byte[] dst);

        [DllImport("C:\\inetpub\\TbsWeb\\ARTH_DLL.dll")]
        //[DllImport("F:\\Tbs\\src\\Tbs.Web.Mvc\\bin\\Debug\\netcoreapp1.1\\ARTH_DLL32.dll")]
        public static extern int UserMatch(byte[] src, byte[] dst, byte secuLevel, ref int matchScore);
    }
}