using System;

namespace RedDog.Messenger.Utils
{
    internal static class StringExtensions
    {
        public static bool NotEmpty(this string text)
        {
            return !String.IsNullOrEmpty(text);
        }
    }
}