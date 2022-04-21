using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.Shared.Domain
{
    public class VegoWindowViewModel : ViewModelBase
    {
        public Action CloseAction { get; set; }
        public Action MinimizeAction { get; set; }
        public Action MaximizeAction { get; set; }

        public Command CloseCommand
            => new Command(_ => CloseAction?.Invoke());

        public Command MinimizeCommand
            => new Command(_ => MinimizeAction?.Invoke());

        public Command MaximizeCommand
            => new Command(_ => MaximizeAction?.Invoke());
    }
}
