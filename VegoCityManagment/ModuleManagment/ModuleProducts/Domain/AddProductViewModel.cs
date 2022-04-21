using Microsoft.Win32;
using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class AddProductViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;
        private ProductsNavController _productsNavController;

        public AddProductViewModel()
        {
            _vegoApi = new VegoAPI.Domain.VegoAPI();
        }

        private string _productName;
        private CategoryResponse[] _productCategories;
        private CategoryResponse _selectedCategory;
        private string _productDescription = "";
        private string _productPrice;
        private ProductPhotoItem _productMainPhoto;
        private ProductPhotoItem _selectedPhoto;
        private ObservableCollection<ProductPhotoItem> _allProductPhotos;
        private Visibility _saveingIndicatorVisibility = Visibility.Collapsed;
        private ProductPhotoItem[] _allPhotos;

        public string ProductName { get => _productName; set { _productName = value; PropertyWasChanged(); } }
        public CategoryResponse[] ProductCategories { get => _productCategories; set { _productCategories = value; PropertyWasChanged(); } }
        public CategoryResponse SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; PropertyWasChanged(); } }
        public string ProductDescription { get => _productDescription; set { _productDescription = value; PropertyWasChanged(); } }
        public string ProductPrice { get => _productPrice; set { _productPrice = value; PropertyWasChanged(); } }
        public ProductPhotoItem SelectedPhoto { get => _selectedPhoto is null ? new ProductPhotoItem() : _selectedPhoto; set { _selectedPhoto = value; PropertyWasChanged(); } }
        public ObservableCollection<ProductPhotoItem> AllProductPhotos { get => _allProductPhotos; set { _allProductPhotos = value; PropertyWasChanged(); } }
        public Visibility SaveingIndicatorVisibility { get => _saveingIndicatorVisibility; set { _saveingIndicatorVisibility = value; PropertyWasChanged(); } }
        public ProductPhotoItem[] AllPhotos { get => _allPhotos; set { _allPhotos = value; PropertyWasChanged(); } }

        public void Setup(ProductsNavController navController)
        {
            _productsNavController = navController;
        }

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

        private Command _loadViewModel;
        public Command LoadViewModel
            => _loadViewModel ??= new Command(
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

        private Command _saveProductInfoCommand;
        public Command SaveProductInfoCommand
            => _saveProductInfoCommand ??= new Command(
                async o =>
                {
                    try
                    {
                        SaveingIndicatorVisibility = Visibility.Visible;

                        var price = Convert.ToDouble(ProductPrice);
                        if (price < 0)
                            throw new Exception("Цена не может быть отрицательной!");

                        if (SelectedCategory is null)
                            throw new Exception("Выберите категорию.");

                        var product = new AddProductRequest
                        {
                            Title = ProductName,
                            Description = ProductDescription,
                            Price = price,
                            ProductTypeId = SelectedCategory.Id
                        };

                        var productId = await _vegoApi.AddProductAsync(product);

                        var addedPhotos = _allProductPhotos
                        ?.ToList();

                        if (_productMainPhoto is not null)
                            if (addedPhotos is not null && addedPhotos.Contains(_productMainPhoto))
                            {
                                addedPhotos.Remove(_productMainPhoto);

                                var photoId = await _vegoApi.AddProductPhotoAsync(
                                    new UploadProductPhotoRequest
                                    {
                                        ProductId = productId,
                                        Source = _productMainPhoto.HighResPath.IsFile
                                        ? Convert.ToBase64String(await File.ReadAllBytesAsync(_productMainPhoto.HighResPath.LocalPath))
                                        : _productMainPhoto.HighResPath.AbsoluteUri
                                    });

                                await _vegoApi.SetProductMainPhotoAsync(
                                    new SetProductMainPhotoRequest
                                    {
                                        PhotoId = photoId,
                                        ProductId = productId,
                                    });
                            }

                        if (addedPhotos is not null)
                        {
                            var loadPhotosTask = addedPhotos.Select(async photo =>
                                _vegoApi.AddProductPhotoAsync(
                                    new UploadProductPhotoRequest
                                    {
                                        ProductId = productId,
                                        Source = photo.HighResPath.IsFile
                                        ? Convert.ToBase64String(await File.ReadAllBytesAsync(photo.HighResPath.LocalPath))
                                        : photo.HighResPath.AbsoluteUri
                                    }));

                            await Task.WhenAll(loadPhotosTask);
                        }
                            

                        MessageBox.Show("Успешно!");
                        _productsNavController.PopBackStack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        SaveingIndicatorVisibility = Visibility.Collapsed;
                    }
                });

        private Command _backCommand;
        public Command BackCommand
            => _backCommand ??= new Command(o =>
            {
                _productsNavController.PopBackStack();
            });

        private Command _openPhotoDialogCommand;
        public Command OpenPhotoDialogCommand
            => _openPhotoDialogCommand ??= new Command(o =>
            {
                var openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == true)
                {
                    var path = new Uri(openFileDialog.FileName);
                    var photoItem = new ProductPhotoItem
                    {
                        PhotoId = Guid.NewGuid(),
                        LowResPath = path,
                        HighResPath = path
                    };

                    photoItem.FirstButtonCommand = new Command(_ =>
                    {
                        bool wasSelected = SelectedPhoto == photoItem;
                        bool wasMain = _productMainPhoto == photoItem;

                        AllProductPhotos.Remove(photoItem);

                        if (AllProductPhotos.Count > 0)
                        {
                            if (wasMain)
                                _productMainPhoto = AllProductPhotos.First();

                            if (wasSelected)
                                SelectedPhoto = AllProductPhotos.First();
                        }
                        else
                        {
                            _productMainPhoto = null;
                            SelectedPhoto = null;
                        }

                        if (wasSelected)
                            PropertyWasChanged("SelectedPhoto");
                    });

                    photoItem.SecondButtonCommand = new Command(
                            _ =>
                            {
                                _productMainPhoto = photoItem;
                            },
                            _ => _productMainPhoto?.PhotoId is not null
                            ? _productMainPhoto.PhotoId != photoItem.PhotoId
                            : true);

                    photoItem.OnPressCommand = new Command(
                        _ =>
                        {
                            SelectedPhoto = photoItem;
                        },
                        _ => SelectedPhoto?.PhotoId is not null
                        ? SelectedPhoto.PhotoId != photoItem.PhotoId
                        : true);

                    if (AllProductPhotos is null)
                        AllProductPhotos = new ObservableCollection<ProductPhotoItem>();

                    AllProductPhotos.Add(photoItem);

                    if (_productMainPhoto is null)
                        _productMainPhoto = photoItem;

                    if (SelectedPhoto is null || SelectedPhoto.PhotoId == Guid.Empty)
                    {
                        SelectedPhoto = photoItem;
                    }
                }
            });

        private Command _openLinkDialogCommand;
        public Command OpenLinkDialogCommand
            => _openLinkDialogCommand ??= new Command(o =>
            {
                var link = new TextFieldWindow().ShowDialog();

                if (link is null or "")
                    return;

                try
                {
                    SaveingIndicatorVisibility = Visibility.Visible;

                    var photoItem = new ProductPhotoItem
                    {
                        PhotoId = Guid.NewGuid(),
                        LowResPath = new Uri(link),
                        HighResPath = new Uri(link)
                    };

                    photoItem.FirstButtonCommand = new Command(_ =>
                    {
                        bool wasSelected = _selectedPhoto == photoItem;
                        bool wasMain = _productMainPhoto == photoItem;

                        AllProductPhotos.Remove(photoItem);

                        if (AllProductPhotos.Count > 0)
                        {
                            if (wasMain)
                                _productMainPhoto = AllProductPhotos.First();

                            if (wasSelected)
                                SelectedPhoto = AllProductPhotos.First();
                        }
                        else
                        {
                            _productMainPhoto = null;
                            SelectedPhoto = null;
                        }

                        if (wasSelected)
                            PropertyWasChanged("SelectedPhoto");
                    });

                    photoItem.SecondButtonCommand = new Command(
                            _ =>
                            {
                                _productMainPhoto = photoItem;
                            },
                            _ => _productMainPhoto?.PhotoId is not null
                            ? _productMainPhoto.PhotoId != photoItem.PhotoId
                            : true);

                    photoItem.OnPressCommand = new Command(
                        _ =>
                        {
                            SelectedPhoto = photoItem;
                        },
                        _ => SelectedPhoto?.PhotoId is not null
                        ? SelectedPhoto.PhotoId != photoItem.PhotoId
                        : true);

                    if (AllProductPhotos is null)
                        AllProductPhotos = new ObservableCollection<ProductPhotoItem>();
                    AllProductPhotos.Add(photoItem);

                    if (_productMainPhoto is null)
                        _productMainPhoto = photoItem;

                    if (SelectedPhoto.PhotoId == Guid.Empty)
                    {
                        SelectedPhoto = photoItem;
                    }

                    MessageBox.Show("Успешно!");

                    _productsNavController.PopBackStack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    SaveingIndicatorVisibility = Visibility.Collapsed;
                }
            });
    }
}
