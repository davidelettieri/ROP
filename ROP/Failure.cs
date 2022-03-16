using System.Diagnostics.CodeAnalysis;

namespace ROP
{
    public class Failure
    {
        /// <summary>
        /// ReasonId is meant to be a code to be used internally to debug and troubleshoot issues
        /// </summary>
        public string ReasonId { get; }

        /// <summary>
        /// Message is a description of the error, it is meant to be shown to the end user. Possibly localized in the user
        /// language
        /// </summary>
        public string Message { get; }

        public Failure([NotNull] string reasonId, [NotNull] string message)
        {
            ReasonId = reasonId;
            Message = message;
        }
    }
}