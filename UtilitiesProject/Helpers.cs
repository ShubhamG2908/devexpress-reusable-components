using Microsoft.AspNetCore.Mvc.Rendering;
using ModelsProject;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
[assembly: InternalsVisibleTo("MVCDemoApp")]
namespace UtilitiesProject
{
    public static class Helpers
    {
        #region General Methods or Functions.

        internal static List<string> GetColumnNames<T>(List<T> models)
        {
            if (models == null || !models.Any())
            {
                return new List<string>();
            }

            // Get the type of the model
            Type modelType = typeof(T);

            // Get all public properties of the model
            PropertyInfo[] properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Extract property names
            return properties.Select(p => p.Name).ToList();
        }

        internal static List<string> ParseJArray(string json)
        {
            List<string> result = new List<string>();
            JArray array = JArray.Parse(json);

            foreach (JObject content in array.Children<JObject>())
            {
                List<string> keys = content.Properties().Select(p => p.Name).ToList();
                result.AddRange(keys);
            }

            return result;
        }

        #endregion

        #region Apply Filter on Datagrid

        /// <summary>
        /// Apply Filter on Datagrid
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="query">List of data with query way</param>
        /// <param name="filter">Data Grid with filter</param>
        /// <returns></returns>
        internal static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, object filter)
        {
            IQueryable<T> result;
            try
            {
                int propertyNamePosition = 0, operationPosition = 1, filterPosition = 2;
                if (filter is IList<object> filterList)
                {
                    //it is checking all way.
                    if (filterList[propertyNamePosition].GetType().Name == typeof(string).Name)
                    {
                        //single filter
                        query = PrepareFilterQuery(query, filterList, propertyNamePosition, operationPosition, filterPosition);
                    }
                    else if (filterList[propertyNamePosition].GetType().Name == typeof(JArray).Name)
                    {
                        //it is checking multiple filter.
                        int row = 0;
                        foreach (var item in filterList.ToList())
                        {
                            if (item.GetType().Name == typeof(string).Name)
                            {
                                //single filter with and/or/strings
                                query = PrepareFilterQuery(query, filterList, propertyNamePosition, operationPosition, filterPosition, (row == operationPosition));
                            }
                            else if (item.GetType().Name == typeof(JArray).Name)
                            {
                                if (item is JArray jArray)
                                {
                                    List<object> itemlists = jArray.ToObject<List<object>>();
                                    query = PrepareFilterQuery(query, itemlists, propertyNamePosition, operationPosition, filterPosition);

                                }
                            }
                            row++;
                        }
                    }
                    result = query;
                }
                result = query;
            }
            catch (Exception ex)
            {
                return query;
            }
            // If no filters apply, return the full list
            return result;
        }

        /// <summary>
        /// Logic to Express query with filter multiple as well as single
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filterList"></param>
        /// <param name="propertyNamePosition"></param>
        /// <param name="operationPosition"></param>
        /// <param name="filterPosition"></param>
        /// <param name="oprationKeyWord"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        internal static IQueryable<T> PrepareFilterQuery<T>(IQueryable<T> query, IList<object> filterList, int propertyNamePosition, int operationPosition, int filterPosition, bool oprationKeyWord = false)
        {
            IQueryable<T> returnList = query;
            //single
            if (filterList.Count == 3 && filterList[operationPosition].ToString() is string operationText && oprationKeyWord == false)
            {
                // Extract property, operator, and value
                var propertyName = filterList[propertyNamePosition].ToString();
                var value = filterList[filterPosition];
                // Create the LINQ expression
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, propertyName);
                var constant = Expression.Constant(value);
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var methodStartWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                var methodEndWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                var methodWhere = typeof(IQueryable).GetMethod("Where", new[] { typeof(IQueryable) });
                Expression predicate = operationText.ToLower() switch
                {
                    "=" => Expression.Equal(property, constant),
                    ">" when typeof(DateTime).IsAssignableFrom(property.Type) || typeof(IComparable).IsAssignableFrom(property.Type) => Expression.GreaterThan(property, constant),
                    ">=" when typeof(DateTime).IsAssignableFrom(property.Type) || typeof(IComparable).IsAssignableFrom(property.Type) => Expression.GreaterThanOrEqual(property, constant),
                    "<" when typeof(DateTime).IsAssignableFrom(property.Type) || typeof(IComparable).IsAssignableFrom(property.Type) => Expression.LessThan(property, constant),
                    "<=" when typeof(DateTime).IsAssignableFrom(property.Type) || typeof(IComparable).IsAssignableFrom(property.Type) => Expression.LessThanOrEqual(property, constant),
                    "contains" when typeof(string).IsAssignableFrom(property.Type) => Expression.Call(property, method, constant),
                    "notcontains" when typeof(string).IsAssignableFrom(property.Type) => Expression.Not(Expression.Call(property, method, constant)),
                    "startswith" when typeof(string).IsAssignableFrom(property.Type) => Expression.Call(property, methodStartWith, constant),
                    "endswith" when typeof(string).IsAssignableFrom(property.Type) => Expression.Call(property, methodEndWith, constant),
                    "before" when typeof(DateTime).IsAssignableFrom(property.Type) => Expression.LessThan(property, constant),
                    "after" when typeof(DateTime).IsAssignableFrom(property.Type) => Expression.GreaterThan(property, constant),
                    "in" => Expression.Call(Expression.Constant(((IEnumerable<object>)value).ToList()), typeof(List<object>).GetMethod("Contains"), property),
                    "notin" => Expression.Not(Expression.Call(Expression.Constant(((IEnumerable<object>)value).ToList()), typeof(List<object>).GetMethod("Contains"), property)),
                    "isnull" => Expression.Equal(property, Expression.Constant(null)),
                    "isnotnull" => Expression.NotEqual(property, Expression.Constant(null)),
                    _ => throw new NotSupportedException($"Operator {operationText} is not supported")
                };

                var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                // Choose between IQueryable and IEnumerable dynamically
                if (query is IQueryable<T> iq)
                {
                    returnList = iq.Where(lambda);
                }
                else
                {
                    query = query.Where(lambda.Compile()).AsQueryable();
                }
                returnList = query;
            }
            // Handle compound filters with AND/OR
            else if (filterList.Count > 1 && (filterList[operationPosition].ToString() is string logicalOperator) && oprationKeyWord == true)
            {
                var leftFilter = ApplyFilter(query, filterList[propertyNamePosition]);
                var rightFilter = ApplyFilter(query, filterList[filterPosition]);

                query = logicalOperator switch
                {
                    "and" => leftFilter.Intersect(rightFilter),
                    "or" => leftFilter.Concat(rightFilter),
                    _ => query
                };
                //return result;
                returnList = query;
            }
            return returnList;
        }

        #endregion

    }



}
