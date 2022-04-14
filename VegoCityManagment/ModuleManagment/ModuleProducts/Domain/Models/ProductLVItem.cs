using MVVMBaseByNH.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VegoCityManagment.Shared.Domain;

namespace VegoCityManagment.ModuleManagment.ModuleProducts.Domain.Models
{
    public class ProductLVItem : NotifiedProperties
    {
        public Guid Id { get; set; }
        public Uri ImagePath { get; set; }
        public ImageSource Image
        {
            get
            {
                ShimmerVisibility = Visibility.Visible;

                var targetUri = ImagePath is null
                    ? new Uri("pack://application:,,,/shared/resources/defaultimage.png")
                    : ImagePath;

                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = targetUri;

                bitmap.DownloadCompleted += (e, s) =>
                {
                    ShimmerVisibility = Visibility.Collapsed;
                    bitmap.Freeze();
                };

                bitmap.DownloadFailed += (e, s) =>
                {
                    bitmap.UriSource = new Uri("pack://application:,,,/shared/resources/defaultimage.png");
                    bitmap.Freeze();
                };

                bitmap.EndInit();

                if (!bitmap.IsDownloading)
                    ShimmerVisibility = Visibility.Collapsed;

                return bitmap;
            }
        }
        private Visibility _shimmerVisibility = Visibility.Visible;
        public Visibility ShimmerVisibility { get => _shimmerVisibility; set { _shimmerVisibility = value; PropertyWasChanged(); } }
        public double CardOpacity { get => IsActive ? 1d : 0.35d; }
        public string Title { get; set; }
        public double Price { get; set; }
        private bool _isActive;
        public bool IsActive { get => _isActive; set { _isActive = value; PropertyWasChanged(); PropertyWasChanged("CardOpacity"); } }
        public Command OnDoubleClickCommand { get; set; }
    }
}
