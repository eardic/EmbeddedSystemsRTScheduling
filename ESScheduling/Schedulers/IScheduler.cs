using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    public interface IScheduler
    {
        List<Job> Run(int runTime);

        bool Scheduled { get; set; }
    }
}
