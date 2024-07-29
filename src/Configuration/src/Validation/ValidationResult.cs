namespace Fetcharr.Configuration.Validation
{
    public class ValidationResult(string? errorMessage)
    {
        /// <summary>
        ///   Gets a successful representation of <see cref="ValidationResult" />.
        /// </summary>
        public static readonly ValidationResult Success = new(null);

        /// <summary>
        ///   Gets the error message of the validation result, if any.
        /// </summary>
        public readonly string? ErrorMessage = errorMessage;

        /// <summary>
        ///   Gets the names of the members which have validation errors, if any.
        /// </summary>
        public readonly IEnumerable<string> InvalidMemberNames = [];

        /// <summary>
        ///   Gets whether the result is sucessful.
        /// </summary>
        public bool IsSuccess => this.ErrorMessage is null;

        public ValidationResult(string errorMessage, IEnumerable<string> invalidMembers)
            : this(errorMessage)
            => this.InvalidMemberNames = invalidMembers;
    }
}