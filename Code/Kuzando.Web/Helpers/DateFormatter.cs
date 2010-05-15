using System;
using System.Globalization;
using System.Web;

namespace Kuzando.Web.Helpers
{
    public static class DateFormatter
    {
        public static string Format(DateTime date)
        {
            // todo
            return date.ToString("dd/MM/yy");

//            var culture = ResolveCulture();
//            if (culture == null)
//                return date.ToString("dd/MM/yy");
//
//            return date.ToString(culture.DateTimeFormat.ShortDatePattern);
        }

//        public static CultureInfo ResolveCulture()
//        {
//            var languages = HttpContext.Current.Request.UserLanguages;
//
//            if (languages == null || languages.Length == 0)
//                return null;
//
//            try
//            {
//                string language = languages[0].ToLowerInvariant().Trim();
//                return CultureInfo.CreateSpecificCulture(language);
//            }
//            catch (ArgumentException)
//            {
//                return null;
//            }
//        }
    }
}
