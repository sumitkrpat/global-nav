using System;
using System.Data;

namespace GlobalNavService.Utils.Extensions
{
    /// <summary>
    /// SqlExtensions class
    /// </summary>
    public static class SqlExtensions
    {
        /// <summary>
        /// Determines whether the specified dr has column.
        /// </summary>
        /// <param name="dr">The dr.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}