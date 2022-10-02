using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowingCommentWindow.Extensions
{
    public static class StringExtension
    {
        private static TextInfo _textInfo = CultureInfo.CurrentCulture.TextInfo;

        public static string? ToTitleCase(this string? self)
        {
            if (self == null)
            {
                return null;
            }

            string outStr = _textInfo.ToTitleCase(self.ToString());

            return outStr;
        }
    }
}
