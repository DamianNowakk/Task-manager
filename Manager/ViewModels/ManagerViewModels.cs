using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

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
                _processMyList = value.OrderBy(o => o.name).ToList(); ;
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
            _selectedProcessMy = _processMyList[0];
        }     

        private void initProcess()
        {
            List<ProcessMy> list = new List<ProcessMy>();
            foreach (Process process in Process.GetProcesses())
            {
                list.Add(new ProcessMy(process));
            }
            ProcessMyList = list;
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
