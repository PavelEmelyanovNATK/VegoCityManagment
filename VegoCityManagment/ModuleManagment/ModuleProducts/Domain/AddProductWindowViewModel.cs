using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VegoCityManagment.Shared.Domain;
using VegoCityManagment.Shared.Domain.Models;
using VegoCityManagment.Shared.Domain.VegoAPI;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class AddProductWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public AddProductWindowViewModel()
        {
            _vegoApi = new VegoAPI();
        }

        private string _productName;
        private CategoryResponse[] _productCategories;
        private CategoryResponse _selectedCategory;
        private string _productDescription = "";
        private string _productPrice;

        public string ProductName { get => _productName; set { _productName = value; PropertyWasChanged(); } }
        public CategoryResponse[] ProductCategories{ get => _productCategories; set { _productCategories = value; PropertyWasChanged(); } }
        public CategoryResponse SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; PropertyWasChanged(); } }
        public string ProductDescription { get => _productDescription; set { _productDescription = value; PropertyWasChanged(); } }
        public string ProductPrice { get => _productPrice; set { _productPrice = value; PropertyWasChanged(); } }

        public Action CloseWindow { get; set; }

        public async Task LoadCategories()
        {
            try
            {
                ProductCategories = await _vegoApi.FetchAllCategoriesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Command _addProductCommand;
        public Command AddProductCommand
            => _addProductCommand ??= new Command(
                async o =>
                {
                    try
                    {
                        var product = new AddProductRequest
                        {
                            Title = ProductName,
                            Description = ProductDescription,
                            Price = Convert.ToDouble(ProductPrice),
                            ProductTypeId = SelectedCategory.Id
                        };

                        await _vegoApi.AddProductAsync(product);

                        CloseWindow?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

        private Command _loadCategoriesCommand;
        public Command LoadCategoriesCommand
            => _loadCategoriesCommand ??= new Command(
                async o =>
                {
                    try
                    {
                        await LoadCategories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

        
    }
}
