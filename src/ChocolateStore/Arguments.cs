namespace ChocolateStore
{
    class Arguments
    {
        /// <summary>
        /// The directory where the nuget packages and cached files are to be stored
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// The url of the nuget package to cache locally
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Set to true to use relative paths instead of absolute paths in package file
        /// </summary>
        public bool UseRelativePaths { get; set; } = false;
    }
}
