using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class AddProductWindowViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;

        public AddProductWindowViewModel()
        {
            _vegoApi = new VegoAPI.Domain.VegoAPI();
        }

        private string _productName;
        private CategoryResponse[] _productCategories;
        private CategoryResponse _selectedCategory;
        private string _productDescription = "";
        private string _productPrice;
        private Uri _photoPath;

        public string ProductName { get => _productName; set { _productName = value; PropertyWasChanged(); } }
        public CategoryResponse[] ProductCategories{ get => _productCategories; set { _productCategories = value; PropertyWasChanged(); } }
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

                        var productId = await _vegoApi.AddProductAsync(product);

                        if(_photoPath is not null)
                        {
                            var photoId = await _vegoApi.AddProductPhotoAsync(new AddProductPhotoRequest
                            {
                                ProductId = productId,
                                Source = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(_photoPath.LocalPath))
                            });

                            await _vegoApi.SetProductMainPhotoAsync(new SetProductMainPhotoRequest { PhotoId = photoId, ProductId = productId });
                        }

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

                if(openFileDialog.ShowDialog() == true)
                {
                    _photoPath = new Uri(openFileDialog.FileName);
                    PropertyWasChanged("ProductImage");
                }
            });
    }
}
