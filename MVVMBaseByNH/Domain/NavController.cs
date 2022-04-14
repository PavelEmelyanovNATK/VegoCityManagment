using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVVMBaseByNH.Domain
{
    public abstract class NavController : NotifiedProperties
    {
        protected readonly List<Page> BackStack = new();
        private Page _currenPage;
        public Page CurrentPage { get => _currenPage; protected set { _currenPage = value; PropertyWasChanged(); } }

        public void PopBackStack()
        {
            if (BackStack.Count > 1)
            {
                BackStack.RemoveAt(BackStack.Count - 1);

                if (BackStack.Count == 1)
                    CurrentPage = BackStack.Last();
                else
                    CurrentPage = null;
            }
            else
                CurrentPage = null;
        }
    }
}
