using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VegoCityManagment.Shared.Domain
{
    public abstract class NavController : NotifiedProperties
    {
        private Page _currentPage;
        public virtual Page CurrentPage { get => _currentPage; protected set { _currentPage = value; PropertyWasChanged(); } }
    }
}
