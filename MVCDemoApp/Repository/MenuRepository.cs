using Dapper;
using ModelsProject;
using System.Data;
using System;
using Microsoft.Data.SqlClient;
using UtilitiesProject;
using MVCDemoApp.Utilities;

namespace MVCDemoApp.Repository
{
    public class MenuRepository 
    {
        public readonly string _connectionString;

        internal  MenuRepository()
        {
            _connectionString = Helpers.ConnectionString;
        }

        internal IEnumerable<MenuItem> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<MenuItem>("SELECT * FROM MenuItems").AsEnumerable();
            }            
        }

        internal MenuItem GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.QueryFirst<MenuItem>("SELECT * FROM MenuItems WHERE Id = @Id", new { Id = id });
            }
        }
		internal async Task<MenuItem> GetByIdAsync(int id)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				return await db.QueryFirstAsync<MenuItem>("SELECT * FROM MenuItems WHERE Id = @Id", new { Id = id });
			}
		}

		internal IEnumerable<MenuItem> GetDataByFilter(object filter)
		{
			using (IDbConnection db = new SqlConnection(_connectionString))
			{
				return db.Query<MenuItem>(QueryBuilderForFilter(filter)).AsEnumerable();
			}
		}
		internal string QueryBuilderForFilter(object filter)
        {
			string baseQuery = "SELECT * FROM MenuItems ";
			string finalQuery = ReflectionHelper.ApplyFilter<MenuItem>(baseQuery, filter);
            return finalQuery;
		}

		#region For Add

		internal int Add(MenuItem menuItem)
        {
            int createdRecords = 0;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = QueryBuilderForAdd();
                createdRecords = db.Execute(sql, menuItem);
            }
            return createdRecords;
        }

        internal async Task<int> AddAsync(MenuItem menuItem)
        {
            int createdRecords = 0;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = QueryBuilderForAdd();
                createdRecords = await db.ExecuteAsync(sql, menuItem);
            }
            return createdRecords;
        }
        private string QueryBuilderForAdd()
        {
            return "INSERT INTO MenuItems (Text, Icon, Disabled, Url) VALUES (@Text, @Icon, @Disabled,@Url)";
        }

        #endregion
        #region For Update

        internal int Update(MenuItem menuItem)
        {
            int updatedRecords = 0;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = QueryBuilderForUpdate();
                updatedRecords = db.Execute(sql, menuItem);
            }
            return updatedRecords;
        }
        internal async Task<int> UpdateAsync(MenuItem menuItem)
        {
            int updatedRecords = 0;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = QueryBuilderForUpdate();
                updatedRecords = await db.ExecuteAsync(sql, menuItem);
            }
            return updatedRecords;
        }
        private string QueryBuilderForUpdate()
        {
            return "UPDATE MenuItems SET Text = @Text, Icon = @Icon, Disabled = @Disabled, Url= @Url WHERE Id = @Id";

        }

        #endregion
        internal bool Delete(int id)
        {
            bool deletedRecords = false;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM MenuItems WHERE Id = @Id";
                deletedRecords = Convert.ToBoolean(db.Execute(sql, new { Id = id }));
            }
            return deletedRecords;
        }
    }
}
