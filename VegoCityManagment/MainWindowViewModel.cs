using MVVMBaseByNH.Domain;
using MVVMBaseByNH.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainNavController _mainNavController;
        public MainNavController NavController => _mainNavController;

        public Action MaximizeAction { get; set; }
        public Action MinimizeAction { get; set; }
        public Action CloseAction { get; set; }

        public MainWindowViewModel()
        {
            _mainNavController = new MainNavController();
            NavController.NavigateToManagmentScreen(NavOptions.None);
        }

        private static Command _maximizeCommand;
        public Command MaximizeCommand
            => _maximizeCommand ??= new Command(
                _ =>
                {
                    MaximizeAction?.Invoke();
                });

        private static Command _minimizeCommand;
        public Command MinimizeCommand
            => _minimizeCommand ??= new Command(
                _ =>
                {
                    MinimizeAction?.Invoke();
                });

        private static Command _closeCommand;
        public Command CloseCommand
            => _closeCommand ??= new Command(
                _ =>
                {
                    CloseAction?.Invoke();
                });
    }
}
