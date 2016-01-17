using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    // Non preemptive and static priority earliest due date algorithm
    class EDD : AbstractScheduler
    {
        public EDD(IEnumerable<Job> jobs) : base(jobs, true)
        {}

        protected override void SortReadyQueue(int t)
        {
            // Sort the ready queue in increasing order according to deadlines of jobs
            _readyQueue = _readyQueue.OrderBy(j => j.Deadline).ToList();
        }
    }
}
