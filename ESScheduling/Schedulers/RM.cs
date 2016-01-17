using System.Collections.Generic;
using System.Linq;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    class RM : AbstractScheduler
    {
        public RM(IEnumerable<Job> jobs) : base(jobs)
        {
        }

        protected override void UpdateReadyQueue(int t)
        {
            foreach (var job in _jobs.Where(j => !j.IsFinished() && j.Type == JobType.Periodic))
            {
                // Job is arrived or period has come
                if (job.ArrivalTime == t || (t - job.ArrivalTime) % job.Period == 0)
                {
                    _readyQueue.Add(job.Clone());
                }
            }
        }
        
        protected override void SortReadyQueue(int t)
        {
            _readyQueue = _readyQueue.OrderBy(j => j.Period).ToList();
        }
    }
}
