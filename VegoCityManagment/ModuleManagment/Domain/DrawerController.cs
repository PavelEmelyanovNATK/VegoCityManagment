using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.Domain
{
    public class DrawerController : NotifiedProperties
    {
        private bool _isDrawerOpenned = false;
        public bool IsDrawerOpenned { get => _isDrawerOpenned; private set { _isDrawerOpenned = value; PropertyWasChanged(); } }

        public void OpenDrawer()
            => IsDrawerOpenned = true;

        public void CloseDrawer()
            => IsDrawerOpenned = false;

        private Command _openDrawerCommand;
        public Command OpenDrawerCommand
            => _openDrawerCommand ??= new Command(
                _ =>
                {
                    OpenDrawer();
                },
                _ => !IsDrawerOpenned);

        private Command _closeDrawerCommand;
        public Command CloseDrawerCommand
            => _closeDrawerCommand ??= new Command(
                _ =>
                {
                    CloseDrawer();
                },
                _ => IsDrawerOpenned);
    }
}
