using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Manager
{
    class ProcessMy
    {
        public Process process;
        public String name { get; set; }
        public int threads { get; set; }
        public int priority { get; set; }
        public String nonPagedSystemMemorySize { get; set; }
        public String pagedSystemMemorySize { get; set; }
        public String privateMemorySize { get; set; }
        public String pagedMemorySize { get; set; }
        public String virtualMemorySize { get; set; }
        public String modulesWarning { get; set; }
        public ProcessThreadCollection threadsList { get; set; }
        public ProcessModuleCollection modulesList { get; set; }
        public String modulesWarrning { get; set; }

        public ProcessMy(Process process)
        {
            var th = new Thread(setModules);
            th.Start();

            this.process = process;
            name = process.ProcessName;        
            threads = process.Threads.Count;
            priority = process.BasePriority;
            nonPagedSystemMemorySize = process.NonpagedSystemMemorySize64 + " b";
            pagedSystemMemorySize = process.PagedSystemMemorySize64 + " b";
            privateMemorySize = process.PrivateMemorySize64 + " b";
            pagedMemorySize = process.PagedMemorySize64 + "/" + process.PeakPagedMemorySize64 + " b";
            virtualMemorySize = process.VirtualMemorySize64 + "/" + process.PeakVirtualMemorySize64 + " b";
            threadsList = process.Threads;
        }

        private void setModules()
        {
            try
            {
                modulesList = process.Modules;
            }
            catch (Exception e)
            {
                modulesWarrning = e.Message;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
