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
        public bool isLive { get; set; }
        public bool isChcecked { get; set; }
        public Process process { get; set; }
        public String name { get; set; }
        public int id { get; set; }
        public int threads { get; set; }
        public bool isExist { get; set; }
        public int priority { get; set; }
        public String nonPagedSystemMemorySize { get; set; }
        public String pagedSystemMemorySize { get; set; }
        public String privateMemorySize { get; set; }
        public String pagedMemorySize { get; set; }
        public String virtualMemorySize { get; set; }
        public String modulesWarning { get; set; }
        public ProcessThreadCollection threadsList { get; set; }
        public List<String> modulesList { get; set; }
        public String mainModules { get; set; }
        public String modulesWarrning { get; set; }

        public ProcessMy(Process process)
        {
            isLive = false;
            isChcecked = false;
            modulesList = new List<String>();
            var th = new Thread(setModules);
            th.Start();
            refresh(process);
        }

        public void refresh(Process process)
        {
            this.process = process;
            name = process.ProcessName;
            id = process.Id;
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
            try {
                mainModules = process.MainModule.FileName;
            }
            catch (Exception e) {
            }
            try {               
                foreach (ProcessModule module in process.Modules)
                    modulesList.Add(module.FileName);
            }
            catch (Exception e) {
                modulesList.Add("Access is denied");
            }
        }

        public override string ToString()
        {
            return name + " (id: " + id + ")";
        }

        public bool CompareTo(Process obj)
        {
            if (this.id == obj.Id && this.name.Equals(obj.ProcessName))
                return true;
            return false;
        }
    }
}
