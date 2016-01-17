using System;

namespace ESScheduling.Jobs
{
    public class Job
    {
        private int _runTime;// How much unit of time the job has run
        public int Id { get; set; }// Identity of the job
        public int Deadline { get; set; }// Deadline time of the job
        public int ArrivalTime { get; set; }// The time When the job arrived
        public int ComputationTime { get; set; }// Total computation time
        public JobType Type { get; set; }// Periodic or Aperiodic

        public int Period
        {
            get { return Deadline - ArrivalTime; }
        }

        public int RemainingComputationTime
        {
            get { return ComputationTime - RunTime; }
        }

        public int RunTime
        {
            get { return _runTime; }
            set
            {
                if (value > ComputationTime)
                    throw new InvalidOperationException("Run time cannot be larger than computation time.");
                _runTime = value;
            }
        }

        public int NextDeadline(int t)
        {
            if (Type == JobType.Periodic)
            {
                for (var i = ArrivalTime + Period; ; i += Period)
                {
                    if (i > t)
                    {
                        return i;
                    }
                }
            }
            else
            {
                return Deadline;
            }
        }

        public void Reset()
        {
            RunTime = 0;
        }

        public bool IsFinished()
        {
            return ComputationTime <= RunTime;
        }

        public Job Clone()
        {
            return this.MemberwiseClone() as Job;
        }
    }
}
