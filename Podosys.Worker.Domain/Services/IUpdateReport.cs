﻿namespace Podosys.Worker.Domain.Services
{
    public interface IUpdateReport
    {
        Task UpdateReportAsync(DateTime firstdate, DateTime lastdate);
    }
}
