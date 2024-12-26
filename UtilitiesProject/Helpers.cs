using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelsProject;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
[assembly: InternalsVisibleTo("MVCDemoApp")]
namespace UtilitiesProject
{
    public static class Helpers
    {
        public static string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Devexpress2326;User Id=sa;Password=ips12345;TrustServerCertificate=true;";


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


		internal static string ApplyFilter<T>(string baseQuery, object filter)
		{
			try
			{
				int propertyNamePosition = 0, operationPosition = 1, filterPosition = 2;

				if (filter is IList<object> filterList)
				{
					List<string> conditions = new();

					if (filterList[propertyNamePosition].GetType().Name == typeof(string).Name)
					{
						// Single filter
						string condition = PrepareFilterCondition(filterList, propertyNamePosition, operationPosition, filterPosition);
						conditions.Add(condition);
					}
					else if (filterList[propertyNamePosition].GetType().Name == typeof(JArray).Name)
					{
						// Multiple filters
						foreach (var item in filterList)
						{
							if (item.GetType().Name == typeof(string).Name)
							{
								// Single filter with and/or
								string condition = PrepareFilterCondition(filterList, propertyNamePosition, operationPosition, filterPosition);
								conditions.Add(condition);
							}
							else if (item is JArray jArray)
							{
								var itemList = jArray.ToObject<List<object>>();
								string condition = PrepareFilterCondition(itemList,  propertyNamePosition, operationPosition, filterPosition);
								conditions.Add(condition);
							}
						}
					}

					// Append conditions to the query
					if (conditions.Any())
					{
						baseQuery += " WHERE " + string.Join(" AND ", conditions);
					}
				}
			}
			catch (Exception ex)
			{
				// Log or handle the exception as necessary
				Console.WriteLine($"Error applying filter: {ex.Message}");
			}

			return baseQuery;
		}

		private static string PrepareFilterCondition(IList<object> filterList, int propertyNamePosition, int operationPosition, int filterPosition)
		{
            var sb = new StringBuilder();
            var conditions = new List<string>();

			for (int i = 0; i < filterList.Count; i += 3)
			{
				string property = filterList[i].ToString();
				string operation = filterList[i + 1].ToString().ToLower();
				var value = filterList[i + 2];

				string paramName = value.ToString();

				// Map operation to SQL-compatible syntax
				switch (operation)
				{
					case "=":
						conditions.Add($"{property} = {value}");
						break;
					case ">":
						conditions.Add($"{property} > {value}");
						break;
					case ">=":
						conditions.Add($"{property} >= {value}");
						break;
					case "<":
						conditions.Add($"{property} < {value}");
						break;
					case "<=":
						conditions.Add($"{property} <= {value}");
						break;
					case "contains":
						conditions.Add($"{property} LIKE '%{value}%'");
						break;
					case "startswith":
						conditions.Add($"{property} LIKE '{value}%'");
						break;
					case "endswith":
						conditions.Add($"{property} LIKE '%{value}'");
						break;
					case "in":
						var inValues = ((IEnumerable<object>)value).ToList();
						var inParams = string.Join(", ", inValues.Select((_, index) => $"{value}{index}"));
						conditions.Add($"{property} IN ({inParams})");
						break;
					case "notin":
						var notInValues = ((IEnumerable<object>)value).ToList();
						var notInParams = string.Join(", ", notInValues.Select((_, index) => $"{value}{index}"));
						conditions.Add($"{property} NOT IN ({notInParams})");
						break;
					case "isnull":
						conditions.Add($"{property} IS NULL");
						break;
					case "isnotnull":
						conditions.Add($"{property} IS NOT NULL");
						break;
					default:
						throw new NotSupportedException($"Operator '{operation}' is not supported.");
				}
			}

			if (conditions.Any())
			{
                //sb.Append(" WHERE ");
                sb.Append(string.Join(" AND ", conditions));
			}

            return sb.ToString();

            //// Return the condition for the SQL query
            //return $"{propertyName} {operation} {parameterName}";
        }

		#endregion

	}



}
