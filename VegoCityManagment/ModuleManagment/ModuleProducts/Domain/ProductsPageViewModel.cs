using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows;
using VegoCityManagment.ModuleProducts.Presentation.Windows;
using VegoCityManagment.Shared.Domain;
using VegoCityManagment.Shared.Domain.Models;
using VegoCityManagment.Shared.Domain.VegoAPI;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoAPI;

        public ProductsPageViewModel()
        {
            _vegoAPI = new VegoAPI();
        }

        private ProductLVItem[] _products;
        private Category[] _categories;

        public ProductLVItem[] Products { get => _products; set { _products = value; PropertyWasChanged(); } }
        public Category[] Categories { get => _categories; set { _categories = value; PropertyWasChanged(); } }

        public async Task LoadProductsAsync()
        {
            try
            {
                var checkedCategories = Categories
                    .Where(c => c.IsChecked)
                    .Select(c => c.Id)
                    .ToArray();

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
                        new EditProductWindow(p.Id).ShowDialog();
                        await LoadProductsAsync();
                    }),
                    ImagePath = new Uri("https://web.archive.org/web/20190502184842if_/https://2ch.hk/rf/src/3366980/15568158108720.png")
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
                new Category
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsChecked = Categories?.FirstOrDefault(c => c.Id == p.Id)?.IsChecked ?? false,
                    OnCheckChanged = RefreshCommand
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
            get => _openAddProductWindowCommand ??= new Command(async o =>
            {
                new AddProductWindow().ShowDialog();
                await LoadProductsAsync();
            });
        }

        private Command _resetCategoriesCommand;
        public Command ResetCategoriesCommand
        {
            get => _resetCategoriesCommand ??= new Command(async o =>
            {
                foreach (var c in Categories)
                    c.IsChecked = false;

                RefreshCommand.Execute(null);
            });
        }
    }
}
