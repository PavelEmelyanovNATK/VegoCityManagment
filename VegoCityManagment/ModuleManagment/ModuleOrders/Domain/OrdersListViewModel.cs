using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.ModuleManagment.ModuleOrders.Domain.Models;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleOrders.Domain
{
    internal class OrdersListViewModel : ViewModelBase
    {
        private DrawerController _drawerController;
        private OrdersNavController _ordersNavController;
        public DrawerController DrawerController => _drawerController;

        private OrderLVItem[] _orders = new[]
        {
            new OrderLVItem
            {
                Id = Guid.NewGuid(),
                RegistrationDate = DateTime.Now,
                ClientName = "Client",
                Status = "Status"
            },
            new OrderLVItem
            {
                Id = Guid.NewGuid(),
                RegistrationDate = DateTime.Now,
                ClientName = "Client",
                Status = "Status"
            },
            new OrderLVItem
            {
                Id = Guid.NewGuid(),
                RegistrationDate = DateTime.Now,
                ClientName = "Client",
                Status = "Status"
            },
            new OrderLVItem
            {
                Id = Guid.NewGuid(),
                RegistrationDate = DateTime.Now,
                ClientName = "Client",
                Status = "Status"
            },
            new OrderLVItem
            {
                Id = Guid.NewGuid(),
                RegistrationDate = DateTime.Now,
                ClientName = "Client",
                Status = "Status"
            }
        };

        private StatusLVItem[] _statuses = new[]
        {
            new StatusLVItem
            {
                Id = 1,
                IsChecked = true,
                Title="1",
                OnClick = new Command(_ =>
                {

                })
            }
        };

        public OrderLVItem[] Orders { get => _orders; set { _orders = value; PropertyWasChanged(); } }

        public void Setup(DrawerController drawerController, OrdersNavController navController)
        {
            _drawerController = drawerController;
            _ordersNavController = navController;
            PropertyWasChanged("DrawerController");
        }

    }
}
