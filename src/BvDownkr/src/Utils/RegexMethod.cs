using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BvDownkr.src.Utils {
    public partial class RegexMethod {
        [GeneratedRegex(@"(?<=/)[^/?#]+(?=[^/]*$)gm")]
        private static partial Regex UrlFileNameRegex();
        public static string GetUrlFileName(string downloadUrl) {
            return UrlFileNameRegex().Match(downloadUrl).Value;
        }
    }
}
