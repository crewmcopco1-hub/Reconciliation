using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDSReconciliation.Infrastructure;

namespace VDSReconciliation.Repositories.SQL
{

    public class DetailRepository : IDetailRepository
    {
        private readonly IConfiguration _config;

        public DetailRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<DetailRecord>> GetDetailsByMasterIdAsync(long BatchID)
        {
            using var conn =
                new SqlConnection(_config["SqlConnection"]);
            var result = new List<DetailRecord>();

            //using var conn = _connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(
                @"SELECT ID, EID, GLIN, Status
              FROM VDS_BatchDetails
              WHERE BatchID = @BatchID", conn);

            cmd.Parameters.AddWithValue("@BatchID", BatchID);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new DetailRecord
                {
                    ID = reader.GetInt64(0),
                    EID = reader.GetString(1),
                    GLIN = reader.GetString(2),
                    Status = reader.GetInt32(3)
                });
            }

            return result;
        }

        public async Task InsertBulkAsync(long BatchID, List<DetailRecord> records)
        {
            using var conn = new SqlConnection(_config["SqlConnection"]); 
            await conn.OpenAsync();

            foreach (var record in records)
            {
                using var cmd = new SqlCommand(
                    @"INSERT INTO VDS_BatchDetails (BatchID, EID, GLIN, RowInsertDate, Status)
                  VALUES (@BatchID, @EID, @GLIN, GETUTCDATE(), 1);
                  SELECT SCOPE_IDENTITY();", conn);

                cmd.Parameters.AddWithValue("@BatchID", BatchID);
                cmd.Parameters.AddWithValue("@EID", record.EID);
                cmd.Parameters.AddWithValue("@GLIN", record.GLIN);

                record.ID = Convert.ToInt64(await cmd.ExecuteScalarAsync());
            }
        }

        public async Task UpdateStatusAsync(long ID, int status, string? errorMessage)
        {
            using var conn = new SqlConnection(_config["SqlConnection"]);
            using var cmd = new SqlCommand(
                @"UPDATE VDS_BatchDetails
              SET Status = @Status,
              WHERE ID = @ID", conn);

            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@ID", ID);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<ProcessingSummary> GetProcessingSummaryAsync(long BatchID)
        {
            using var conn = new SqlConnection(_config["SqlConnection"]);
            using var cmd = new SqlCommand(
                @"SELECT 
                COUNT(*) AS Total,
                SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END) AS SuccessCount,
                SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END) AS FailedCount
              FROM VDS_BatchDetails
              WHERE BatchID = @BatchID", conn);

            cmd.Parameters.AddWithValue("@BatchID", BatchID);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();

            return new ProcessingSummary
            {
                TotalRecords = reader.GetInt32(0),
                SuccessCount = reader.GetInt32(1),
                FailedCount = reader.GetInt32(2)
            };
        }
    }
}
