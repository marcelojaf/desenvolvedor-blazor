namespace VelozientComputers.Infrastructure.Data
{
    /// <summary>
    /// Constants used for database configuration and entity properties
    /// </summary>
    public static class DbConstants
    {
        /// <summary>
        /// Maximum length for name fields (first name, last name)
        /// </summary>
        public const int NameLength = 100;

        /// <summary>
        /// Maximum length for email addresses
        /// </summary>
        public const int EmailLength = 256;

        /// <summary>
        /// Maximum length for URLs (image URLs)
        /// </summary>
        public const int UrlLength = 2048;

        /// <summary>
        /// Maximum length for serial numbers
        /// </summary>
        public const int SerialNumberLength = 50;

        /// <summary>
        /// Maximum length for specifications text
        /// </summary>
        public const int SpecificationsLength = 4000;

        /// <summary>
        /// Maximum length for manufacturer names
        /// </summary>
        public const int ManufacturerNameLength = 100;

        /// <summary>
        /// Maximum length for status names
        /// </summary>
        public const int StatusNameLength = 50;

        /// <summary>
        /// Maximum length for regular expression patterns
        /// </summary>
        public const int RegexPatternLength = 500;
    }
}