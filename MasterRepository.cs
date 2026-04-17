using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VDSReconciliation.Infrastructure;

namespace VDSReconciliation.Repositories.SQL
{
    public class MasterRepository : IMasterRepository
    {
        private readonly IConfiguration _config;
        public MasterRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task UpdateStatusAsync(
            long batchID,
            int total,
            int success,
            int failed)
        {
            int status;
            var Comments = string.Empty;
            if (total <= 0)
            {
                status = 0; // No records / Not started
                Comments = "No records found";
            }
            else if (total == success && failed == 0)
            {
                status = 3; // All Success
                Comments = "All records processed successfully";
            }
            else if (total == failed && success == 0)
            {
                status = 2; // All Failed
                Comments = "All records failed";
            }
            else if (total != success && failed != 0)
            {
                status = 4; // Partial / Mixed
                Comments = "Records processed partially";
            }
            else
            {
                status = 1; // Fallback / In Progress
                Comments = "Fallback";
            }

            using var conn = new SqlConnection(_config["SqlConnection"]);
            using var cmd = new SqlCommand(
               @"update VDS_BatchMaster 
                 set TotalRecords=@TotalRecords, ProcessedRecords=@ProcessedRecords, 
                 UpdatedBy='VDS Reconcilation Function', UpdatedDate=GETUTCDATE() ,Status=@Status,
                 Comments=@Comments
              WHERE ID = @ID", conn);
            cmd.Parameters.AddWithValue("@TotalRecords", total);
            cmd.Parameters.AddWithValue("@ProcessedRecords", success);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Comments", Comments);
            cmd.Parameters.AddWithValue("@ID", batchID);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
