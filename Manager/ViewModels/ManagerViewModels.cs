using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows;

namespace Manager
{
    class ManagerViewModels : INotifyPropertyChanged
    {
        private ObservableCollection<ProcessMy> _processMyList;
        public ObservableCollection<ProcessMy> ProcessMyList
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

        private ProcessThread _selectedThreadsMy;
        public ProcessThread SelectedThreadMy
        {
            get
            {
                return _selectedThreadsMy;
            }
            set
            {
                _selectedThreadsMy = value;
                OnPropertyChanged("SelectedThreadMy");
            }
        }

        public ManagerViewModels()
        {
            _processMyList = new ObservableCollection<ProcessMy>();
            initProcess();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += initProcessTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void initProcess()
        {
            var processes = Process.GetProcesses();
            for(int i=0; i < processes.Length; i++) {
                ProcessMyList.Add(new ProcessMy(processes[i]));
            }
        }

        private void update()
        {
            ProcessThread tmp = _selectedThreadsMy;
            var processes = Process.GetProcesses();
            for (int i = 0; i < processes.Length; i++) {
                if (ProcessMyList[i].CompareTo(processes[i]))
                    ProcessMyList[i].refresh(processes[i]);
                else
                    ProcessMyList.Insert(i, new ProcessMy(processes[i]));
            }
            bool add;
            int oldd = ProcessMyList.Count;
            int neww = processes.Length;
            for (int j = 0; j < neww; j++) {
                add = true;
                for (int i = 0; i < oldd; i++) {
                    if (ProcessMyList[i].CompareTo(processes[j])) {
                        ProcessMyList[i].isChcecked = true;
                        add = false;
                        break;
                    }
                }
                if (add) {
                    ProcessMyList.Add(new ProcessMy(processes[j]));
                    ProcessMyList[ProcessMyList.Count - 1].isChcecked = true;
                }
            }
            List<int> remove = new List<int>();
            for (int i = 0; i < ProcessMyList.Count; i++) {        
                if (ProcessMyList[i].isChcecked == false)
                    remove.Add(i);
                else 
                    ProcessMyList[i].isChcecked = false;               
            }
            for (int i = remove.Count - 1; i >= 0; i--) {
                ProcessMyList.RemoveAt(remove[i]);
            }

            OnPropertyChanged("SelectedProcessMy");
        }

        private void initProcessTimer_Tick(object sender, EventArgs e)
        {
            update();
        }

        public ICommand killProcess { get { return new RelayCommand<object>(killProcessExcute); } }
        public void killProcessExcute(object obj)
        {
            try {
                SelectedProcessMy.process.Kill();
            }
            catch(Exception e) {
                MessageBox.Show(e.Message, "Error");
            }
            update();
        }

        public ICommand changePriority { get { return new RelayCommand<string>(changePriorityExcute); } }
        public void changePriorityExcute(string i)
        {        
            try {
                ProcessPriorityClass priority;
                switch (i)
                {
                    case "Realtime":
                        priority = ProcessPriorityClass.RealTime; break;
                    case "High":
                        priority = ProcessPriorityClass.High; break;
                    case "Above normal":
                        priority = ProcessPriorityClass.AboveNormal; break;
                    case "Normal":
                        priority = ProcessPriorityClass.Normal; break;
                    case "Below normal":
                        priority = ProcessPriorityClass.BelowNormal; break;
                    case "Low":
                        priority = ProcessPriorityClass.Idle; break;
                    default:
                        priority = ProcessPriorityClass.Normal; break;
                }
                SelectedProcessMy.process.PriorityClass = priority;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message, "Error");
            }
            update();
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
