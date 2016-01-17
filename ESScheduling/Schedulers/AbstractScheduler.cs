using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESScheduling.Jobs;

namespace ESScheduling.Schedulers
{
    abstract class AbstractScheduler : IScheduler
    {
        protected ICollection<Job> _readyQueue;
        protected IEnumerable<Job> _jobs;
        protected readonly bool _static;

        public bool Scheduled { get; set; }

        protected AbstractScheduler(IEnumerable<Job> jobs, bool isStatic = false)
        {
            // Copy jobs list to prevent modification on original job list
            _jobs = jobs.Select(t => t.Clone()).ToList();
            _readyQueue = new List<Job>();
            _static = isStatic;// If static is true than ready queue is only created and sorted at time 0.
        }

        protected virtual void UpdateReadyQueue(int t)
        {
            // For each unfinished job
            foreach (var job in _jobs.Where(j => !j.IsFinished()))
            {
                // If the job is aperiodic and its arrival time has come, then add it to ready queue
                if (job.Type == JobType.Aperiodic && job.ArrivalTime == t)
                {
                    // Clone the job to make sure that properties of it is not changed in original job list
                    _readyQueue.Add(job.Clone());
                }
                // If the job is periodic and its period has come
                if (job.Type == JobType.Periodic && (t - job.ArrivalTime) % job.Period == 0)
                {
                    // Clone the job to make sure that properties of it is not changed in original job list
                    _readyQueue.Add(job.Clone());
                }
            }
        }

        protected abstract void SortReadyQueue(int t);

        private void CheckSchedule(List<Job> schedule)
        {
            Scheduled = true;

            // Check if all deadlines are met
            foreach (var job in _jobs)
            {
                // If the job is not runned at all
                if (!schedule.Exists(t => t != null && t.Id == job.Id))
                {
                    Scheduled = false;
                    break;
                }
                else// Check if the job meets deadline
                {
                    if (job.Type == JobType.Aperiodic)
                    {
                        // Index indicates runtime of job in schedule list, 
                        // Example: if index of job is 0 then the job is runned from 0 to 1.
                        var lastRun = schedule.FindLastIndex(x => x != null && x.Id == job.Id) + 1;
                        if (lastRun > job.Deadline)
                        {
                            Scheduled = false;
                            break;
                        }
                    }
                    else
                    {// Periodic
                        var i = job.ArrivalTime;
                        while (i < schedule.Count)
                        {
                            // Calculate runtime between period
                            var periodOrRunTime = i + job.Period < schedule.Count ? job.Period : schedule.Count - i;
                            var runTime = schedule.GetRange(i, periodOrRunTime).Count(x => x != null && x.Id == job.Id);
                            // If the job didnt run for comp. time then it missed deadline.
                            if (runTime != job.ComputationTime)
                            {
                                Scheduled = false;
                                break;
                            }
                            // Next arrival time
                            i += job.Period;
                        }
                    }
                }
            }
        }

        public virtual List<Job> Run(int runTime)
        {
            // The job at index i represents that the job is scheduled between from i to i+1 unit of time. 
            var schedule = new List<Job>();
            // Clear ready queue and reset jobs
            _readyQueue.Clear();
            foreach (var j in _jobs) { j.Reset(); }
            // Scheduling skeleton
            for (var t = 0; t < runTime; ++t)
            {
                // Add arrived jobs to queue, if static then run only at time 0
                if (_static) { if (t == 0) { UpdateReadyQueue(t); } }
                else { UpdateReadyQueue(t); }

                Job runningJob = null;
                if (_readyQueue.Count > 0)
                {
                    // Sort ready queue by algorithm implemented in derived class
                    // If static then run only at time 0.
                    if (_static) { if (t == 0) { SortReadyQueue(t); } }
                    else { SortReadyQueue(t); }
                    // Get first job from queue to run
                    runningJob = _readyQueue.First();
                    // Run the job for 1 unit of time
                    runningJob.RunTime += 1;
                    // Remove the job from ready queue if the job finishes
                    if (runningJob.IsFinished())
                    {
                        _readyQueue.Remove(runningJob);
                    }
                }
                // Add the job to the schedule list
                schedule.Add(runningJob);
            }
            // Check if all deadlines are met
            CheckSchedule(schedule);
            return schedule;
        }
    }
}
