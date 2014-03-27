namespace DH.Helpdesk.Common.Extensions.String
{
    using System;

    public static class SplitExtension
    {
        public static string[] Split(this string source, string separator)
        {
            return source.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}