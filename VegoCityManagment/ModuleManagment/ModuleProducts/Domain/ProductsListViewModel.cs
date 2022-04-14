using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.ModuleManagment.Domain;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class ProductsListViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoAPI;
        private DrawerController _drawerController;
        private ProductsNavController _productsNavController;
        public DrawerController DrawerController => _drawerController;

        public ProductsListViewModel()
        {
            _vegoAPI = new VegoAPI.Domain.VegoAPI();
        }

        private ProductLVItem[] _products;
        private CategoryLVItem[] _categories;

        public ProductLVItem[] Products { get => _products; set { _products = value; PropertyWasChanged(); } }
        public CategoryLVItem[] Categories { get => _categories; set { _categories = value; PropertyWasChanged(); } }

        public void Setup(DrawerController drawerController, ProductsNavController navController)
        {
            _drawerController = drawerController;
            _productsNavController = navController;
            PropertyWasChanged("DrawerController");
        }

        public async Task LoadProductsAsync()
        {
            try
            {
                var checkedCategories = Categories
                    ?.Where(c => c.IsChecked)
                    ?.Select(c => c.Id)
                    ?.ToArray();

                var filteredProductsRequest = new FilteredProductsRequest
                {
                    CategoriesIds = checkedCategories,
                    Filter = ""
                };

                var rawProducts = await _vegoAPI.FetchProductsWithFilterAsync(filteredProductsRequest);


                Products = rawProducts.Select(p =>
                new ProductLVItem
                {
                    Id = p.Id,
                    Title = p.Title,
                    Price = p.Price,
                    OnDoubleClickCommand = new Command(async o =>
                    {
                        _productsNavController.NavigateToEditProductScreen(p.Id, _productsNavController);
                    }),
                    ImagePath = p.ImagePath == null || p.ImagePath == ""
                    ? new Uri("pack://application:,,,/shared/resources/defaultimage.png")
                    : new Uri(p.ImagePath),
                    IsActive = p.IsActive
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task LoadCategoriesAsync()
        {
            try
            {
                var rawCategories = await _vegoAPI.FetchAllCategoriesAsync();

                Categories = rawCategories.Select(p =>
                new CategoryLVItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsChecked = Categories?.FirstOrDefault(c => c.Id == p.Id)?.IsChecked ?? false,
                    OnCheckChanged = RefreshCommand,
                    OnDoubleClick = new Command(o =>
                    {
                        new EditCategoryWindow(p.Id).ShowDialog();
                        RefreshCommand.Execute(null);
                    })
                })
                .ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Command _refreshCommand;
        public Command RefreshCommand
        {
            get => _refreshCommand ??= new Command(async o =>
            {
                await LoadCategoriesAsync();
                await LoadProductsAsync();
            });
        }

        private Command _openAddProductWindowCommand;
        public Command OpenAddProductWindowCommand
        {
            get => _openAddProductWindowCommand ??= new Command(o =>
            {
                _productsNavController.NavigateToAddProductScreen(_productsNavController);
            });
        }

        private Command _resetCategoriesCommand;
        public Command ResetCategoriesCommand
        {
            get => _resetCategoriesCommand ??= new Command(o =>
            {
                if (Categories is not null)
                    foreach (var c in Categories)
                        c.IsChecked = false;

                RefreshCommand.Execute(null);
            });
        }

        private Command _openAddCategoryWindowCommand;
        public Command OpenAddCategoryWindowCommand
        {
            get => _openAddCategoryWindowCommand ??= new Command(o =>
            {
                new AddCategoryWindow().ShowDialog();
                RefreshCommand.Execute(null);
            });
        }
    }
}
