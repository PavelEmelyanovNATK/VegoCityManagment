using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class EditProductWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public EditProductWindowViewModel()
        {
            _vegoApi = new VegoAPI.Domain.VegoAPI();
        }

        private ProductDetailResponse _oldProductInfo;

        private string _productName;
        private CategoryResponse[] _productCategories;
        private CategoryResponse _selectedCategory;
        private string _productDescription = "";
        private string _productPrice;
        private Uri _photoPath;

        public string ProductName { get => _productName; set { _productName = value; PropertyWasChanged(); } }
        public CategoryResponse[] ProductCategories { get => _productCategories; set { _productCategories = value; PropertyWasChanged(); } }
        public CategoryResponse SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; PropertyWasChanged(); } }
        public string ProductDescription { get => _productDescription; set { _productDescription = value; PropertyWasChanged(); } }
        public string ProductPrice { get => _productPrice; set { _productPrice = value; PropertyWasChanged(); } }
        public ImageSource ProductImage
        {
            get
            {
                var converter = new ImageSourceConverter();

                return _photoPath == null
                    ? (ImageSource)converter.ConvertFrom(new Uri("pack://application:,,,/shared/resources/defaultimage.png"))
                    : (ImageSource)converter.ConvertFrom(_photoPath);
            }
        }

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

        public void SetProductId(Guid productId)
        {
            _oldProductInfo = new ProductDetailResponse { Id = productId };
        }

        public async Task LoadProductInfo(Guid productId)
        {
            try
            {
                _oldProductInfo = await _vegoApi.FetchProductDetailsAsync(productId);

                ProductName = _oldProductInfo.Title;
                ProductDescription = _oldProductInfo.Description;
                ProductPrice = _oldProductInfo.Price.ToString();
                SelectedCategory = ProductCategories
                    ?.FirstOrDefault(c => c.Id == _oldProductInfo.CategoryId);
                _photoPath = !string.IsNullOrEmpty(_oldProductInfo.ImagePath) ? new Uri(_oldProductInfo.ImagePath) : null;
                PropertyWasChanged("ProductImage");

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

                        if(changedFields.Count > 0)
                            await _vegoApi.EditProductInfoAsync(new EditEntityWithGuidRequest
                            {
                                EntityId = _oldProductInfo.Id,
                                ChangedFields = changedFields
                            });

                        if (_photoPath.AbsoluteUri != _oldProductInfo.ImagePath)
                        {
                            var photoId = await _vegoApi.AddProductPhotoAsync(new AddProductPhotoRequest
                            {
                                ProductId = _oldProductInfo.Id,
                                Source = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(_photoPath.LocalPath))
                            });

                            await _vegoApi.SetProductMainPhotoAsync(new SetProductMainPhotoRequest { PhotoId = photoId, ProductId = _oldProductInfo.Id });
                        }
                            

                        CloseWindow?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

        private Command _closeCommand;
        public Command CloseCommand
            => _closeCommand ??= new Command(o =>
            {
                CloseWindow?.Invoke();
            });

        private Command _openPhotoDialogCommand;
        public Command OpenPhotoDialogCommand
            => _openPhotoDialogCommand ??= new Command(o =>
            {
                var openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == true)
                {
                    _photoPath = new Uri(openFileDialog.FileName);
                    PropertyWasChanged("ProductImage");
                }
            });
    }
}
