using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    // Earliest Deadline First algorithm
    class EDF : AbstractScheduler
    {
        public EDF(IEnumerable<Job> jobs)
            : base(jobs)
        { }

        protected override void SortReadyQueue(int t)
        {
            // Sort the ready queue in increasing order according to deadlines of jobs
            _readyQueue = _readyQueue.OrderBy(j => j.NextDeadline(t)).ToList();
        }
    }
}
