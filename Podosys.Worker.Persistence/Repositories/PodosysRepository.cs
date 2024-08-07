﻿using Dapper;
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

            var sql = @"SELECT [Id] 
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

        public async Task<IEnumerable<Transaction>> GetTransactionMonth(DateTime date, bool operacionalCost = false)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            date = new DateTime(date.Year, date.Month, 1);

            var ultimoDiaMes = date.AddMonths(1);

            var sql = !operacionalCost ?
                                  @"SELECT [Id] 
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
                             Where [Date] >= '" + date.ToString("yyyy-MM-dd") + "'AND [Date] < '" + ultimoDiaMes.ToString("yyyy-MM-dd") + "'" +
                                     "and (MedicalRecordId is not null " +
                                     "or OrderId is not null " +
                                     "or SaleOffId is not null)" :

                             @"SELECT [Id] 
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
                             Where [Date] >= '" + date.ToString("yyyy-MM-dd") + "'AND [Date] < '" + ultimoDiaMes.ToString("yyyy-MM-dd") + "'" +
                                     "and [TransactionTypeId] = 2";

            return await db.QueryAsync<Transaction>(sql);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionBySaleOffIds(IEnumerable<Guid?>? saleOffIds)
        {
            if (saleOffIds is null || !saleOffIds.Any() || saleOffIds.Any(x => x == null))
                return null;

            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var saleoffId in saleOffIds)
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[SaleOffId] ='" + saleoffId.ToString() + "'";
            }

            var sql = @"SELECT [Id] 
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
                             Where " + ids;

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

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecord(DateTime date, IEnumerable<Guid?> medicalRecordIds = null)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            if (medicalRecordIds != null)
                foreach (var id in medicalRecordIds)
                {
                    ids += ids != string.Empty ? " or " : "";
                    ids += "[Id] ='" + id.ToString() + "'";
                }


            string sql = string.IsNullOrEmpty(ids) ?
                             @"SELECT [Id] 
                                 ,[PacientId]
                                 ,[UserId]
                                 ,[PayType]
                                 ,[Value]
                                 ,[Observation]
                                 ,[MedicalRecordDate]
                                 ,[ProcedurePriceId]
                                 ,[HomeCare]
                                 ,[Enabler] 
                             FROM [db_a7ba3c_podosysprd].[dbo].[MedicalRecord_tb] 
                             Where [MedicalRecordDate] >= '" + date.ToString("yyyy-MM-dd") + "'AND [MedicalRecordDate] < '" + date.AddDays(1).ToString("yyyy-MM-dd") + "'"
                            :
                             @"SELECT [Id] 
                                 ,[PacientId]
                                 ,[UserId]
                                 ,[PayType]
                                 ,[Value]
                                 ,[Observation]
                                 ,[MedicalRecordDate]
                                 ,[ProcedurePriceId]
                                 ,[HomeCare]
                                 ,[Enabler] 
                             FROM [db_a7ba3c_podosysprd].[dbo].[MedicalRecord_tb] 
                             Where" + ids;
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

        public async Task<IEnumerable<SaleOff>> GetSaleOffs(IEnumerable<Guid?>? saleOffIds)
        {
            if (saleOffIds is null || !saleOffIds.Any())
                return null;

            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var id in saleOffIds)
            {
                if (id is null)
                    continue;

                ids += ids != string.Empty ? "or" : "";
                ids += "[Id] ='" + id.ToString() + "'";
            }

            string sql = @"SELECT [Id] 
                                  ,[Password]
                                  ,[BuyerName]
                                  ,[PhoneNumber]
                                  ,[NumberOfSection]
                                  ,[PaymentType]
                                  ,[Value]
                                  ,[Date]
                                  ,[Enabler]
                              FROM [db_a7ba3c_podosysprd].[dbo].[SaleOff_tb]
                              Where " + ids;

            return await db.QueryAsync<SaleOff>(sql);
        }

        public async Task<IEnumerable<Address>> GetAddress(IEnumerable<Guid?> addressIds)
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            var ids = string.Empty;

            foreach (var addressId in addressIds.Where(x => x != null && x != Guid.Empty))
            {
                ids += ids != string.Empty ? "or" : "";
                ids += "[Id] ='" + addressId.ToString() + "'";
            }

            if (string.IsNullOrEmpty(ids))
                return null;

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

        public async Task<IEnumerable<Procedure>> GetAllProcedure()
        {
            await using var db = new SqlConnection(_podosysConnectionString);

            string sql = @"SELECT [Id] 
                                 ,[Name]
                                 ,[Observation]
                                 ,[GroupId]
                             FROM [db_a7ba3c_podosysprd].[dbo].[ProcedurePrices_tb]";

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