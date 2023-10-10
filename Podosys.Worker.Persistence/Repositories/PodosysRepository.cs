using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Podosys.Worker.Domain.Models.Podosys;
using Podosys.Worker.Domain.Repositories;

namespace Podosys.Worker.Persistence.Repositories
{
    public class PodosysRepository : IPodosysRepository
    {
        private readonly string _podosysConnectionString;
        private readonly IConfiguration _configuration;

        public PodosysRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _podosysConnectionString = configuration.GetConnectionString("PodosysConnection") ?? "";
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecord(DateTime FirstDate, DateTime LastDate)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var date = DateTime.Now.Date.AddDays(-1);

            string sql = @"SELECT [Id] 
                                 ,[PacientId]
                                 ,[UserId]
                                 ,[PayType]
                                 ,[Value]
                                 ,[Observation]
                                 ,[MedicalRecordDate]
                                 ,[Enabler] 
                             FROM [db_a7ba3c_podosysprd].[dbo].[MedicalRecord_tb] 
                             Where [MedicalRecordDate] >= '" + date.ToString("yyyy-MM-dd") + "'AND [MedicalRecordDate] < '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            var medicalRecords = await db.QueryAsync<MedicalRecord>(sql);
            return medicalRecords;
        }

    }
}