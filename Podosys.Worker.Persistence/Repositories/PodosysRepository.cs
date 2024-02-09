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

        public async Task<IEnumerable<Transaction>> GetTransaction(DateTime FirstDate)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            string sql = @"SELECT [Id] 
                                 ,[Description]
                                 ,[FlowType]
                                 ,[PaymentType]
                                 ,[Value]
                                 ,[Date]
                                 ,[CashFlowId]
                                 ,[MedicalRecordId]
                                 ,[OrderId]
                                 ,[SaleOffId]
                                 ,[TransactionTypeId]
                                 ,[PaymentTypeId]
                                 ,[TransactionCategoryId]
                             FROM [db_a7ba3c_podosysprd].[dbo].[Transaction_tb]
                             Where [Date] >= '" + FirstDate.ToString("yyyy-MM-dd") + "'AND [Date] < '" + FirstDate.AddDays(1).ToString("yyyy-MM-dd") + "'";

            return await db.QueryAsync<Transaction>(sql);
        }

        public async Task<IEnumerable<TransactionCategory>> GetAllTransactionCategory()
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            string sql = @"SELECT [Id] 
                                 ,[Name]
                             FROM [db_a7ba3c_podosysprd].[dbo].[TransactionCategory_tb]";

            return await db.QueryAsync<TransactionCategory>(sql);
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecord(IEnumerable<Guid> medicalRecordIds)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var medicalRecordId in medicalRecordIds)
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[Id] ='" + medicalRecordId.ToString() + "'";
            }

            string sql = @"SELECT [Id] 
                                 ,[PacientId]
                                 ,[UserId]
                                 ,[PayType]
                                 ,[Value]
                                 ,[Observation]
                                 ,[MedicalRecordDate]
                                 ,[Enabler] 
                             FROM [db_a7ba3c_podosysprd].[dbo].[MedicalRecord_tb] 
                             Where " + ids;
            try
            {
                return await db.QueryAsync<MedicalRecord>(sql);

            }
            catch (Exception ex)
            {


            }
            return null;
        }

        public async Task<IEnumerable<Pacient>> GetPacient(IEnumerable<Guid> pacientIds)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var pacientId in pacientIds)
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[Id] ='" + pacientId.ToString() + "'";
            }

            string sql = @"SELECT [Id] 
                                  ,[Name]
                                  ,[Surgery]
                                  ,[BirthDate]
                                  ,[PrimaryPhone]
                                  ,[SecondaryPhone]
                                  ,[Allergies]
                                  ,[AddressId]
                                  ,[RegisterDate]
                                  ,[Enabler]
                                  ,[CommunicationChannelId]
                                  ,[Occupation]
                                  ,[Sport]
                                  ,[Standing]
                                  ,[Medicine]
                                  ,[Shoe]
                                  ,[Observation]
                              FROM [db_a7ba3c_podosysprd].[dbo].[Pacient_tb]
                              Where " + ids;

            return await db.QueryAsync<Pacient>(sql);
        }

        public async Task<IEnumerable<Address>> GetAddress(IEnumerable<Guid> addressIds)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var addressId in addressIds)
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[Id] ='" + addressId.ToString() + "'";
            }

            string sql = @"SELECT 
                               [Id] 
                              ,[Street]
                              ,[Number]
                              ,[Complement]
                              ,[Neighborhood]
                              ,[City]
                              ,[State]
                              ,[PostalCode]
                              ,[Country]
                              ,[Latitude]
                              ,[Longitude]
                            FROM [db_a7ba3c_podosysprd].[dbo].[Address_tb]
                            Where " + ids;

            return await db.QueryAsync<Address>(sql);
        }


        public async Task<IEnumerable<Pacient>> GetPacientByDate(DateTime date)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var lastdate = date.AddDays(1);

            string sql = @"SELECT [Id] 
                                  ,[Name]
                                  ,[Surgery]
                                  ,[BirthDate]
                                  ,[PrimaryPhone]
                                  ,[SecondaryPhone]
                                  ,[Allergies]
                                  ,[AddressId]
                                  ,[RegisterDate]
                                  ,[Enabler]
                                  ,[CommunicationChannelId]
                                  ,[Occupation]
                                  ,[Sport]
                                  ,[Standing]
                                  ,[Medicine]
                                  ,[Shoe]
                                  ,[Observation]
                              FROM [db_a7ba3c_podosysprd].[dbo].[Pacient_tb]
                              Where [RegisterDate] >= '" + date.ToString("yyyy-MM-dd") + "'AND [RegisterDate] < '" + lastdate.ToString("yyyy-MM-dd") + "'";

            return await db.QueryAsync<Pacient>(sql);
        }

        public async Task<IEnumerable<Procedure>> GetProcedure(IEnumerable<Guid> medicalRecordIds)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var medicalRecordId in medicalRecordIds)
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[MedicalRecordId] ='" + medicalRecordId.ToString() + "'";
            }

            string sql = @"SELECT [Id] 
                                 ,[ProcedureType]
                                 ,[MedicalRecordId]
                              FROM [db_a7ba3c_podosysprd].[dbo].[Procedure_tb]
                              Where " + ids;

            return await db.QueryAsync<Procedure>(sql);
        }

        public async Task<IEnumerable<Professional>> GetProfessional(IEnumerable<Guid> professionalIds)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var professionalId in professionalIds)
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[Id] ='" + professionalId.ToString() + "'";
            }

            string sql = @"SELECT [Id] 
                                 ,[Name]
                              FROM [db_a7ba3c_podosysprd].[dbo].[User_tb]
                              Where " + ids;

            return await db.QueryAsync<Professional>(sql);
        }

        public async Task<IEnumerable<PodosysCommunicationChannel>> GetAllCommunicationChannel()
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            string sql = @"SELECT [Id]
                                 ,[Name]
                                  FROM [db_a7ba3c_podosysprd].[dbo].[CommunicationChannel_tb]";

            return await db.QueryAsync<PodosysCommunicationChannel>(sql);
        }
    }
}