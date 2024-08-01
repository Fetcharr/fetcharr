using System.Globalization;

using static DotNet.Testcontainers.Guard;

namespace Fetcharr.Testing.Templates.Extensions
{
    public static partial class ArgumentInfoExtensions
    {
        public static ref readonly ArgumentInfo<string> HasLength(
            this in ArgumentInfo<string> argument,
            int length,
            string? exceptionMessage = null)
        {
            if(argument.Value.Length == length)
            {
                return ref argument;
            }

            throw new ArgumentException(
                exceptionMessage ??
                string.Format(
                    CultureInfo.InvariantCulture,
                    "'{0}' must have length of {1}.",
                    argument.Name,
                    length),
                argument.Name);
        }
    }
}