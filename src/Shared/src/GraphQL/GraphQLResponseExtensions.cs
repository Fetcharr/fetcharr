using GraphQL;

namespace Fetcharr.Shared.GraphQL
{
    public static class GraphQLResponseExtensions
    {
        /// <summary>
        ///   If any errors are present on the response, throw an <see cref="AggregateException"/> with all errors.
        /// </summary>
        /// <param name="response"><see cref="GraphQLResponse{T}"/>-instance to check for errors on.</param>
        public static void ThrowIfErrors<T>(this GraphQLResponse<T> response, string? message = null)
        {
            if(response.Errors is { Length: > 0 })
            {
                throw new AggregateException(
                    message ?? "Error(s) received from GraphQL endpoint.",
                    response.Errors.Select(error => new Exception(error.Message)));
            }
        }
    }
}