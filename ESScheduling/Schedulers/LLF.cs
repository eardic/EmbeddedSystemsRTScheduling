using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    class LLF : AbstractScheduler
    {
        public LLF(IEnumerable<Job> jobs)
            : base(jobs)
        {
        }
        
        private int Laxity(Job job, int t)
        {
            return job.NextDeadline(t) - (job.RemainingComputationTime + t);
        }

        protected override void SortReadyQueue(int t)
        {
            _readyQueue = _readyQueue.OrderBy(j => Laxity(j, t)).ToList();
        }
    }
}
