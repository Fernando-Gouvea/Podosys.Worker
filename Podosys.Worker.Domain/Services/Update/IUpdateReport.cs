namespace Podosys.Worker.Domain.Services.Update
{
    public interface IUpdateReport
    {
        Task UpdateReportAsync(DateTime firstdate, DateTime lastdate);
    }
}
