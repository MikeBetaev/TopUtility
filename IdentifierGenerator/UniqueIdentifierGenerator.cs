using System;

namespace Top.IdentifierGenerator
{
    /// <summary>
    /// Class for generating unique identifiers
    /// </summary>
    public class UniqueIdentifierGenerator : IUniqueIdentifierGenerator
    {
        /// <summary>
        /// Next unique identifier
        /// </summary>
        /// <returns>Guid as string</returns>
        public string Next()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
