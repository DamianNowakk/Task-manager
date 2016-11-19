using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace Manager
{
    class Controller : INotifyPropertyChanged
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

        public Controller()
        {
            _processMyList = new List<ProcessMy>();
            initProcess();
        }     

        private void initProcess()
        {
            foreach(Process process in Process.GetProcesses())
            {
                _processMyList.Add(new ProcessMy(process));
            }
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
