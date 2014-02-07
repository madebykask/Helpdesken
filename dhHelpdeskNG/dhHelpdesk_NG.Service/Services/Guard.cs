namespace DH.Helpdesk.Services.Services
{
    using System.Text.RegularExpressions;

    public static class Guard
    {
        public static bool HasValue(object value)
        {
            return value == null;
        }

        public static bool HasValue(string value)
        {
            return !string.IsNullOrEmpty(value) && value.Trim().Length > 0;
        }

        private static Regex _tags = new Regex("<[^>]*(>|$)", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        private static Regex _blacklist = new Regex(@"^<script\s?>$|^</\sscript>$", RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        public static string Sanitize(string html)
        {
            if(string.IsNullOrEmpty(html)) return html;

            string tagname;
            Match tag;

            MatchCollection tags = _tags.Matches(html);
            for(int i = tags.Count - 1; i > -1; i--)
            {
                tag = tags[i];
                tagname = tag.Value.ToLowerInvariant();

                if((_blacklist.IsMatch(tagname)))
                    html = html.Remove(tag.Index, tag.Length);
            }

            return html;
        }
    }
}
