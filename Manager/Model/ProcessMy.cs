using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Management;

namespace Manager
{
    class ProcessMy
    {
        public bool isLive { get; set; }
        public String command { get; set; }
        public bool isChcecked { get; set; }
        public Process process { get; set; }
        public String name { get; set; }
        public int id { get; set; }
        public int threads { get; set; }
        public int priority { get; set; }
        public String nonPagedSystemMemorySize { get; set; }
        public String pagedSystemMemorySize { get; set; }
        public String privateMemorySize { get; set; }
        public String pagedMemorySize { get; set; }
        public String virtualMemorySize { get; set; }
        public ProcessThreadCollection threadsList { get; set; }
        public List<String> modulesList { get; set; }
        public String mainModules { get; set; }

        public ProcessMy(Process process)
        {
            isLive = false;
            isChcecked = false;
            modulesList = new List<String>();         
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

        public void setModules()
        {             
            try {
                mainModules = process.MainModule.FileName;
                foreach (ProcessModule module in process.Modules)
                    modulesList.Add(module.FileName);
                setCommandLine();
            }
            catch{
                modulesList.Clear();
                modulesList.Add("Access is denied");
            }
        }

        private void setCommandLine()
        {
            var commandLine = new StringBuilder();
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"]);
                }
            }
            command = commandLine.ToString().Substring(process.MainModule.FileName.Length + 3);
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
