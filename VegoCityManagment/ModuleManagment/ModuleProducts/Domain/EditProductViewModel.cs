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
using System.Windows.Media.Imaging;
using VegoAPI.Domain;
using VegoAPI.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models;
using VegoCityManagment.ModuleManagment.ModuleProducts.Presentation.Windows;
using VegoCityManagment.Shared.Components;
using VegoCityManagment.Shared.Domain;
using VegoCityManagment.Shared.Utils;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain
{
    public class EditProductViewModel : ViewModelBase
    {
        private readonly IVegoAPI _vegoApi;
        private ProductsNavController _productsNavController;

        public EditProductViewModel()
        {
            _vegoApi = new VegoAPI.Domain.VegoAPI();
        }

        private ProductDetailResponse _oldProductInfo;

        private string _productName;
        private CategoryResponse[] _productCategories;
        private CategoryResponse _selectedCategory;
        private string _productDescription = "";
        private string _productPrice;
        private ProductPhotoItem _productMainPhoto;
        private ProductPhotoItem _selectedPhoto;
        private ObservableCollection<ProductPhotoItem> _allProductPhotos;
        private Visibility _saveingIndicatorVisibility = Visibility.Collapsed;
        private bool _productIsActive;

        public string ProductName { get => _productName; set { _productName = value; PropertyWasChanged(); } }
        public CategoryResponse[] ProductCategories { get => _productCategories; set { _productCategories = value; PropertyWasChanged(); } }
        public CategoryResponse SelectedCategory { get => _selectedCategory; set { _selectedCategory = value; PropertyWasChanged(); } }
        public string ProductDescription { get => _productDescription; set { _productDescription = value; PropertyWasChanged(); } }
        public string ProductPrice { get => _productPrice; set { _productPrice = value; PropertyWasChanged(); } }
        public ProductPhotoItem SelectedPhoto { get => _selectedPhoto is null ? new ProductPhotoItem() : _selectedPhoto; set { _selectedPhoto = value; PropertyWasChanged(); } }
        public ObservableCollection<ProductPhotoItem> AllProductPhotos { get => _allProductPhotos; set { _allProductPhotos = value; PropertyWasChanged(); } }
        public Visibility SaveingIndicatorVisibility { get => _saveingIndicatorVisibility; set { _saveingIndicatorVisibility = value; PropertyWasChanged();} }
        public bool ProductIsActive { get => _productIsActive; set { _productIsActive = value; PropertyWasChanged(); } }

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

        public void Setup(Guid productId, ProductsNavController navController)
        {
            _oldProductInfo = new ProductDetailResponse { Id = productId };
            _productsNavController = navController;
        }

        public async Task LoadProductInfo(Guid productId)
        {
            try
            {
                _oldProductInfo = await _vegoApi.FetchProductDetailsAsync(productId);

                ProductName = _oldProductInfo.Title;
                ProductIsActive = _oldProductInfo.IsActive;
                ProductDescription = _oldProductInfo.Description;
                ProductPrice = _oldProductInfo.Price.ToString();
                SelectedCategory = ProductCategories
                    ?.FirstOrDefault(c => c.Id == _oldProductInfo.CategoryId);
                AllProductPhotos = _oldProductInfo.Photos.Select(p =>
                {
                    var photoItem = new ProductPhotoItem
                    {
                        PhotoId = p.PhotoId,
                        LowResPath = string.IsNullOrEmpty(p.LowResPath)
                        ? null
                        : new Uri(p.LowResPath),
                        HighResPath = string.IsNullOrEmpty(p.HighResPath)
                        ? null
                        : new Uri(p.HighResPath)
                    };

                    photoItem.FirstButtonCommand = BuildPhotoItemFirstButtonCommand(photoItem);

                    photoItem.SecondButtonCommand = BuildPhotoItemSecondButtonCommand(photoItem);

                    photoItem.OnPressCommand = BuildOnPressPhotoItemCommand(photoItem);

                    return photoItem;
                })
            .ToObservableCollection();

                if (_oldProductInfo.MainPhotoId is not null)
                {
                    var photo = AllProductPhotos.FirstOrDefault(p => p.PhotoId == _oldProductInfo.MainPhotoId);

                    if (photo is not null && photo.HighResPath is not null)
                    {
                        SelectedPhoto = photo;
                        _productMainPhoto = photo;
                    }
                }
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
        private bool _saveButtonEnabled = true;
        public Command SaveProductInfoCommand
            => _saveProductInfoCommand ??= new Command(
                async o =>
                {
                    try
                    {
                        SaveingIndicatorVisibility = Visibility.Visible;

                        var changedFields = new Dictionary<string, string>();

                        if (_oldProductInfo.Title != ProductName)
                            changedFields["Title"] = ProductName;

                        if (_oldProductInfo.Description != ProductDescription)
                            changedFields["Description"] = ProductDescription;

                        if (_oldProductInfo.Price != Convert.ToDouble(ProductPrice))
                            changedFields["Price"] = ProductPrice;

                        if (_oldProductInfo.CategoryId != SelectedCategory.Id)
                            changedFields["CategoryId"] = SelectedCategory.Id.ToString();

                        if(_oldProductInfo.IsActive != ProductIsActive)
                            changedFields["IsActive"] = ProductIsActive.ToString();

                        if (changedFields.Count > 0)
                            await _vegoApi.EditProductInfoAsync(new EditEntityWithGuidRequest
                            {
                                EntityId = _oldProductInfo.Id,
                                ChangedFields = changedFields
                            });

                        var deletedPhotos = _oldProductInfo.Photos
                        .Select(photo => photo.PhotoId)
                        .Except(_allProductPhotos
                        .Select(photo => photo.PhotoId));

                        var addedPhotos = _allProductPhotos
                        .Where(photo => !_oldProductInfo.Photos
                        .Any(oph => photo.PhotoId == oph.PhotoId))
                        .ToList();

                        if(_productMainPhoto is not null && _productMainPhoto.PhotoId != _oldProductInfo.MainPhotoId)
                            if (addedPhotos.Contains(_productMainPhoto))
                            {
                                addedPhotos.Remove(_productMainPhoto);

                                var photoId = await _vegoApi.AddProductPhotoAsync(
                                    new UploadProductPhotoRequest
                                    {
                                        ProductId = _oldProductInfo.Id,
                                        Source = _productMainPhoto.HighResPath.IsFile
                                        ? Convert.ToBase64String(await File.ReadAllBytesAsync(_productMainPhoto.HighResPath.LocalPath))
                                        : _productMainPhoto.HighResPath.AbsoluteUri
                                    });

                                await _vegoApi.SetProductMainPhotoAsync(
                                    new SetProductMainPhotoRequest
                                    {
                                        PhotoId = photoId,
                                        ProductId = _oldProductInfo.Id,
                                    });
                            }
                            else
                            {
                                await _vegoApi.SetProductMainPhotoAsync(
                                    new SetProductMainPhotoRequest
                                    {
                                        PhotoId = _productMainPhoto.PhotoId,
                                        ProductId = _oldProductInfo.Id,
                                    });
                            }

                        var loadPhotosTasks = addedPhotos.Select(async photo =>
                            _vegoApi.AddProductPhotoAsync(
                                new UploadProductPhotoRequest
                                {
                                    ProductId = _oldProductInfo.Id,
                                    Source = photo.HighResPath.IsFile
                                    ? Convert.ToBase64String(await File.ReadAllBytesAsync(photo.HighResPath.LocalPath))
                                    : photo.HighResPath.AbsoluteUri
                                }));

                        var deletePhotosTasks = deletedPhotos.Select(photo =>
                            _vegoApi.DeleteProductPhotoAsync(photo));

                        if(loadPhotosTasks.Count() > 0)
                            await Task.WhenAll(loadPhotosTasks);

                        if(deletePhotosTasks.Count() > 0)
                            await Task.WhenAll(deletePhotosTasks);

                        VegoMessageDialogWindow.ShowDialog("Успешно!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        SaveingIndicatorVisibility = Visibility.Collapsed;
                    }
                },
                _ => _saveButtonEnabled);

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

                    photoItem.FirstButtonCommand = BuildPhotoItemFirstButtonCommand(photoItem);

                    photoItem.SecondButtonCommand = BuildPhotoItemSecondButtonCommand(photoItem);

                    photoItem.OnPressCommand = BuildOnPressPhotoItemCommand(photoItem);

                    AllProductPhotos.Add(photoItem);

                    if (_productMainPhoto is null)
                        _productMainPhoto = photoItem;

                    if (SelectedPhoto.PhotoId == Guid.Empty)
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
                    var photoItem = new ProductPhotoItem
                    {
                        PhotoId = Guid.NewGuid(),
                        LowResPath = new Uri(link),
                        HighResPath = new Uri(link)
                    };

                    photoItem.FirstButtonCommand = BuildPhotoItemFirstButtonCommand(photoItem);

                    photoItem.SecondButtonCommand = BuildPhotoItemSecondButtonCommand(photoItem);

                    photoItem.OnPressCommand = BuildOnPressPhotoItemCommand(photoItem);

                    AllProductPhotos.Add(photoItem);

                    if (_productMainPhoto is null)
                        _productMainPhoto = photoItem;

                    if (SelectedPhoto.PhotoId == Guid.Empty)
                    {
                        SelectedPhoto = photoItem;
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

        private Command BuildPhotoItemFirstButtonCommand(ProductPhotoItem photoItem)
        {
            return new Command(
                _ =>
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
                });
        }

        private Command BuildPhotoItemSecondButtonCommand(ProductPhotoItem photoItem)
        {
            return new Command(
                _ =>
                {
                    _productMainPhoto = photoItem;
                },
                _ => _productMainPhoto?.PhotoId is not null
                ? _productMainPhoto.PhotoId != photoItem.PhotoId
                : true);
        }

        private Command BuildOnPressPhotoItemCommand(ProductPhotoItem photoItem)
        {
            return new Command(
                _ =>
                {
                    SelectedPhoto = photoItem;
                },
                _ => SelectedPhoto?.PhotoId is not null
                ? SelectedPhoto.PhotoId != photoItem.PhotoId
                : true);
        }

        private Command _deleteCommand;
        public Command DeleteCommand
            => _deleteCommand ??= new Command(
                _ =>
                {
                    VegoMessageDialogWindow.ShowDialog("Вы уверены что хотите удалить товар?", "Внимание!");
                });
    }
}
