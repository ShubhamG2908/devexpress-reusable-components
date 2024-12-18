using MVCDemoApp.Data;
using System.Reflection;
using UtilitiesProject;

namespace MVCDemoApp.Utilities
{
    public static class ReflectionHelper
    {
        #region Drop down box bind
        /// <summary>
        /// Binding drop own box with list data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listData"></param>
        /// <returns></returns>
        internal static List<string> BindDropDownBoxOnlyColumnNameForDynamicModel<T>(List<T> listData)
        {   
            return  Helpers.GetColumnNames(listData);
        }

        /// <summary>
        /// Apply Filter with Datagrid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, object filter)
        {
            return Helpers.ApplyFilter<T>(query, filter);
        }
        /// <summary>
        /// Get model columns
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<string> GetModelColumns(Type model)
        {

            // Get all public properties of the model
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Extract property names
            return properties.Select(p => p.Name).ToList();
        }
        
        /// <summary>
        /// Get model column name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<string> GetModelColName(this Type model)
        {

            // Get all public properties of the model
            PropertyInfo[] properties = model.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Extract property names
            return properties.Select(p => p.Name).ToList();
        }

        #endregion
    }
}
