using Microsoft.AspNetCore.Mvc.Razor;

namespace DevIO.App.Extensions
{
    public static class RazorExtensions
    {
        public static string DocumentFormatter(this RazorPage page, int type, string document)
        {
            return type == 1
                ? Convert.ToUInt64(document).ToString(@"000\.000\.000\-00")
                : Convert.ToUInt64(document).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}
