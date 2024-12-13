namespace AddressManagement.Middlewares
{
    /// <summary>
    /// Class to store the paths and methods that are excluded from JWT validation.
    /// </summary>
    public static class PathSettings
    {
        public static readonly List<(string path, string method)> ExcludedPaths = new()
        {
            ("/Authentication", "POST"),
            ("/User", "POST"),
            ("/User/forgot-password", "POST"),
            ("/User/validate-code", "POST"),
            ("/User/reset-password", "POST")
        };

        /// <summary>
        /// Validates if the current request path and method do not need authentication.
        /// </summary>
        /// <param name="context">The current HTTP context</param>
        /// <returns>True if the path and method are excluded from authentication, otherwise false</returns>
        public static bool IsExcludedPath(HttpContext context)
        {
            return ExcludedPaths.Any(p => context.Request.Path.StartsWithSegments(p.path) && context.Request.Method == p.method);
        }
    }
}
