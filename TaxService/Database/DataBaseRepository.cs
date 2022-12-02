using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using TaxService.Models;
using Microsoft.Extensions.Configuration;

namespace TaxService.Database
{
    public class DataBaseRepository : IDataBaseRepository
    {
        private readonly ILogger<DataBaseRepository> _logger;
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;

        public DataBaseRepository(ILogger<DataBaseRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async System.Threading.Tasks.Task<List<TaxDetails>> FetchTaxDetailsFromDB()
        {
            var taxDetailsList = new List<TaxDetails>();
            try
            {
                string connectionString = _configuration.GetValue<string>("ConnectionString");
                using (_connection = new SqlConnection(connectionString))
                {
                    _connection.Open();
                    string query = "SELECT * FROM Tax";
                    using (SqlCommand command = new SqlCommand(query, _connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var taxDetails = new TaxDetails();
                                taxDetails.TaxID = (int)reader["TaxID"];
                                taxDetails.Municipality = reader["Municipality"].ToString();
                                taxDetails.TaxType = reader["TaxType"].ToString();
                                taxDetails.TaxRule = (int)reader["TaxRule"];
                                taxDetails.FromDate = reader["FromDate"].ToString().Length > 0 ? (DateTime?)reader["FromDate"] : null;
                                taxDetails.ToDate = reader["ToDate"].ToString().Length > 0 ? (DateTime?)reader["ToDate"] : null;
                                taxDetails.IndividualDates = reader["IndividualDates"].ToString();
                                taxDetails.TaxApplied = (decimal)reader["TaxApplied"];
                                taxDetailsList.Add(taxDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while Reading from DB. Error Message: {ex.ToString()}");
            }
            finally
            {
                if (_connection != null && _connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
            return taxDetailsList;
        }
    }
}
