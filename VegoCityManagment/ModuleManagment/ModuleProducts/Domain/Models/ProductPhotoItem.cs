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
    public class ProductPhotoItem : NotifiedProperties
    {
        private Uri _highResPath;
        private Uri _lowResPath;

        public Guid PhotoId { get; set; }
        public Uri HighResPath { get => _highResPath; set { _highResPath = value; PropertyWasChanged("HighPhoto"); } }
        public Uri LowResPath { get => _lowResPath; set { _lowResPath = value; PropertyWasChanged("LowPhoto"); } }
        public ImageSource LowPhoto
        {
            get
            {
                LowShimmerVisibility = Visibility.Visible;

                var targetUri = LowResPath is null
                    ? new Uri("pack://application:,,,/shared/resources/defaultimage.png")
                    : LowResPath;

                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = targetUri;

                bitmap.DownloadCompleted += (e, s) =>
                {
                    LowShimmerVisibility = Visibility.Collapsed;
                    bitmap.Freeze();
                };

                bitmap.DownloadFailed += (e, s) =>
                {
                    bitmap.UriSource = new Uri("pack://application:,,,/shared/resources/defaultimage.png");
                    bitmap.Freeze();
                };

                bitmap.EndInit();

                if (!bitmap.IsDownloading)
                    LowShimmerVisibility = Visibility.Collapsed;

                return bitmap;
            }
        }

        public ImageSource HighPhoto
        {
            get
            {
                HighShimmerVisibility = Visibility.Visible;

                var targetUri = HighResPath is null
                    ? new Uri("pack://application:,,,/shared/resources/defaultimage.png")
                    : HighResPath;

                var bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = targetUri;

                bitmap.DownloadCompleted += (e, s) =>
                {
                    HighShimmerVisibility = Visibility.Collapsed;
                    bitmap.Freeze();
                };

                bitmap.DownloadFailed += (e, s) =>
                {
                    bitmap.UriSource = new Uri("pack://application:,,,/shared/resources/defaultimage.png");
                    bitmap.Freeze();
                };

                bitmap.EndInit();

                if (!bitmap.IsDownloading)
                    HighShimmerVisibility = Visibility.Collapsed;

                return bitmap;
            }
        }

        private Visibility _lowShimmerVisibility = Visibility.Visible;
        public Visibility LowShimmerVisibility { get => _lowShimmerVisibility; set { _lowShimmerVisibility = value; PropertyWasChanged(); } }

        private Visibility _highShimmerVisibility = Visibility.Visible;
        public Visibility HighShimmerVisibility { get => _highShimmerVisibility; set { _highShimmerVisibility = value; PropertyWasChanged(); } }

        public Command FirstButtonCommand { get; set; }
        public Command SecondButtonCommand { get; set; }
        public Command OnPressCommand { get; set; }
    }
}
