using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Manager
{
    class ProcessMy
    {
        private Process process;
        private String name;
        private int threads;
        private ProcessThreadCollection threadsList;

        public ProcessMy(Process process)
        {
            this.process = process;
            name = process.ProcessName;
            
            threads = process.Threads.Count;
            ProcessThreadCollection threadsList = process.Threads;

        }

        public override string ToString()
        {
            return name;
        }
    }
}
