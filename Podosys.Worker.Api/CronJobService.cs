using FluentScheduler;
using Podosys.Worker.Domain.Services;

namespace Podosys.Worker.Api
{
    public class CronJobService
    {
        public class JobRegister : Registry
        {
            public JobRegister()
            {
                Schedule<UpdateReport>().ToRunEvery(1).Days().At(01, 01);
                Schedule<teste>().ToRunEvery(1).Days().At(00, 59);
            }
        }

        public class teste : IJob
        {
            public void Execute()
            {

            }
        }
    }
}