using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVMBaseByNH.Domain
{
    public abstract class NotifiedProperties : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void PropertyWasChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
