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
    public class EditProductWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public EditProductWindowViewModel()
        {
            _vegoApi = new VegoAPI();
        }

        private ProductDetailResponse _oldProductInfo;

        private string _productName;
        private CategoryResponse[] _productCategories;
        private CategoryResponse _selectedCategory;
        private string _productDescription = "";
        private string _productPrice;

        public string ProductName { get => _productName; set { _productName = value; PropertyWasChanged(); } }
        public CategoryResponse[] ProductCategories { get => _productCategories; set { _productCategories = value; PropertyWasChanged(); } }
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

        public void SetProductId(int productId)
        {
            _oldProductInfo = new ProductDetailResponse { Id = productId };
        }

        public async Task LoadProductInfo(int productId)
        {
            try
            {
                _oldProductInfo = await _vegoApi.FetchProductDetailsAsync(productId);

                ProductName = _oldProductInfo.Title;
                ProductDescription = _oldProductInfo.Description;
                ProductPrice = _oldProductInfo.Price.ToString();
                SelectedCategory = ProductCategories
                    ?.FirstOrDefault(c => c.Id == _oldProductInfo.CategoryId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Command _loadViewModel;
        public Command LoadViewModel
            => _loadViewModel ??= new Command(
                async o =>
                {
                    try
                    {
                        await LoadCategories();
                        await LoadProductInfo(_oldProductInfo.Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

        private Command _saveProductInfoCommand;
        public Command SaveProductInfoCommand
            => _saveProductInfoCommand ??= new Command(
                async o =>
                {
                    try
                    {
                        var changedFields = new Dictionary<string, string>();

                        if (_oldProductInfo.Title != ProductName)
                            changedFields["Title"] = ProductName;

                        if (_oldProductInfo.Description != ProductDescription)
                            changedFields["Description"] = ProductDescription;

                        if (_oldProductInfo.Price != Convert.ToDouble(ProductPrice))
                            changedFields["Price"] = ProductPrice;

                        if (_oldProductInfo.CategoryId != SelectedCategory.Id)
                            changedFields["CategoryId"] = SelectedCategory.Id.ToString();

                        await _vegoApi.EditProductInfoAsync(new EditEntityRequest
                        {
                            EntityId = _oldProductInfo.Id,
                            ChangedFields = changedFields
                        });

                        CloseWindow?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
    }
}
