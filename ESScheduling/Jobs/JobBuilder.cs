using System.Collections.Generic;
using System.IO;

namespace ESScheduling.Jobs
{
    class JobBuilder
    {
        public IEnumerable<Job> GetJobsFromFile(string path)
        {
            var fileStream = new StreamReader(path);
            string jobLine;
            bool firstLine = true;
            while ((jobLine = fileStream.ReadLine()) != null)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }
                var tokens = jobLine.Split(';');
                yield return new Job
                {
                    Id = int.Parse(tokens[0]),
                    ArrivalTime = int.Parse(tokens[1]),
                    Deadline = int.Parse(tokens[2]),
                    ComputationTime = int.Parse(tokens[3]),
                    Type = tokens[4] == "A" ? JobType.Aperiodic : JobType.Periodic
                };
            }
        } 
    }
}
