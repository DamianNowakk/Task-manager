using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading;

namespace Manager
{
    class ManagerViewModels : INotifyPropertyChanged
    {
        private List<ProcessMy> _processMyList;
        public List<ProcessMy> ProcessMyList
        {
            get
            {
                return _processMyList;
            }
            set
            {
                _processMyList = value;
                OnPropertyChanged("ProcessMyList");
            }
        }

        private ProcessMy _selectedProcessMy;
        public ProcessMy SelectedProcessMy
        {
            get
            {
                return _selectedProcessMy;
            }
            set
            {
                _selectedProcessMy = value;
                OnPropertyChanged("SelectedProcessMy");
            } 
        }

        public ManagerViewModels()
        {                 
            initProcess();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += initProcessTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void initProcess()
        {
            int index = -1;
            if (SelectedProcessMy != null)
                index = ProcessMyList.IndexOf(SelectedProcessMy);
            List<ProcessMy> list = new List<ProcessMy>();
            foreach (Process process in Process.GetProcesses())
            {
                list.Add(new ProcessMy(process));
            }          
            ProcessMyList = list.OrderBy(o => o.name).ThenBy(x => x.id).ToList();           
            if(index != -1)
                SelectedProcessMy = ProcessMyList[index];
        }

        private void initProcessTimer_Tick(object sender, EventArgs e)
        {
            initProcess();
        }

        public ICommand killProcess { get { return new RelayCommand(killProcessExcute); } }
        public void killProcessExcute()
        {
            SelectedProcessMy.process.Kill();
            initProcess();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
