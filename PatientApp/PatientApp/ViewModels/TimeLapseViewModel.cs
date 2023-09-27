using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using PatientApp.DataModel.SqlEntities;
using PatientApp.Interfaces;
using PatientApp.Settings;
using System.Collections.ObjectModel;
using PatientApp.Views;
using PatientApp.Services;
using System.Threading;
using PatientApp.Utilities;
using System.IO;
using Xamarin.Essentials;

namespace PatientApp.ViewModels
{
    /// <summary>
    /// ViewModel class used to display and select an album and store video file reference
    /// </summary>
    public class TimeLapseAlbum : ObservableObject
    {
        string _albumName;
        public string AlbumName
        {
            get { return _albumName; }
            set { SetProperty(ref _albumName, value); }
        }

        string _previewImagePath;
        public string PreviewImagePath
        {
            get { return _previewImagePath; }
            set { SetProperty(ref _previewImagePath, value); }
        }

        int _imagesCount;
        public int ImagesCount
        {
            get { return _imagesCount; }
            set { SetProperty(ref _imagesCount, value); }
        }

        string _videoFullPath;
        public string VideoFullPath
        {
            get { return _videoFullPath; }
            set
            {
                SetProperty(ref _videoFullPath, value);
                OnPropertyChanged(nameof(HasBuiltVideo));
                OnPropertyChanged(nameof(VideoCreatedOnFormatted));
            }
        }

        public string VideoCreatedOnFormatted
        {
            get
            {
                DateTime? createdOn = null;
                if (!string.IsNullOrEmpty(VideoFullPath))
                {
                    try
                    {
                        createdOn = DateTime.ParseExact(Path.GetFileNameWithoutExtension(VideoFullPath), "yyyyMMdd-HHmmss", null);
                    }
                    catch { }
                }
                return createdOn?.ToString("MM/dd/yyyy hh:mm tt");
            }
        }

        public bool HasBuiltVideo
        {
            get { return !string.IsNullOrEmpty(this.VideoFullPath); }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

    }

    /// <summary>
    /// ViewModel class used to display and select the image thumbnail 
    /// </summary>
    public class TimeLapseImage : ObservableObject
    {
        string _imageTitle;
        public string ImageTitle
        {
            get { return _imageTitle; }
            set { SetProperty(ref _imageTitle, value); }
        }

        DateTime _CreatedOn;
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { SetProperty(ref _CreatedOn, value); }
        }

        public string CreatedOnFormatted
        {
            get { return CreatedOn.ToString("MM/dd/yyyy hh:mm tt"); }
        }

        string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }

    /// <summary>
    /// ViewModel used in timelapse views 
    /// </summary>
    public class TimeLapseViewModel : BaseViewModel
    {
        private ITakePhotoWithOverlay _photoService = null;
        private IStopMotion _stopMotionService = null;
        private IShareMediaService _shareMediaService = null;

        public Command BeginEditModeCommand { get; set; }
        public Command EndEditModeCommand { get; set; }
        public Command AlbumSelectedCommand { get; set; }
        public Command CreateAlbumCommand { get; set; }
        public Command ImageTappedCommand { get; set; }
        public Command LastPhotoTappedCommand { get; set; }
        public Command VideoPreviewTappedCommand { get; set; }
        public Command TakePhotoCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command BuildVideoCommand { get; set; }
        public Command ShareCommand { get; set; }

        /// <summary>
        /// The list of saved albums
        /// </summary>
        public ObservableCollection<TimeLapseAlbum> Albums { get; protected set; }

        /// <summary>
        /// The list of saved images in a album
        /// </summary>
        public ObservableCollection<TimeLapseImage> Images { get; protected set; }

        public bool CanEdit
        {
            get { return HasImages || (SelectedAlbum != null && SelectedAlbum.HasBuiltVideo); }
        }

        bool _isInEditMode;
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set { SetProperty(ref _isInEditMode, value); }
        }

        TimeLapseAlbum _selectedAlbum;
        public TimeLapseAlbum SelectedAlbum
        {
            get { return _selectedAlbum; }
            set
            {
                SetProperty(ref _selectedAlbum, value);
            }
        }

        public bool HasAlbums
        {
            get { return Albums != null & Albums.Any(); }
        }

        public bool HasImages
        {
            get { return LastTakenPhoto != null || (Images != null & Images.Any()); }
        }

        public bool CanBuild
        {
            get { return Images != null & Images.Count >= 2; }
        }


        TimeLapseImage _selectedImage;
        public TimeLapseImage SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                SetProperty(ref _selectedImage, value);
            }
        }

        public bool HasSelection
        {
            get
            {
                return (SelectedImages != null && SelectedImages.Any()) ||
                       (LastTakenPhoto != null && LastTakenPhoto.IsSelected) ||
                       (VideoPreviewPhoto != null && VideoPreviewPhoto.IsSelected);
            }
        }


        TimeLapseImage _zoomImage;
        /// <summary>
        /// Image displayed in the zoom view
        /// </summary>
        public TimeLapseImage ZoomImage
        {
            get { return _zoomImage; }
            set
            {
                SetProperty(ref _zoomImage, value);
            }
        }

        public IEnumerable<TimeLapseImage> SelectedImages
        {
            get { return this.Images.Where(i => i.IsSelected); }
        }


        private TimeLapseImage _lastTakenPhoto = null;
        public TimeLapseImage LastTakenPhoto
        {
            get { return _lastTakenPhoto; }
            set
            {
                SetProperty(ref _lastTakenPhoto, value);
                OnPropertyChanged(nameof(HasLastPhoto));
            }
        }

        public bool HasLastPhoto
        {
            get { return LastTakenPhoto != null; }
        }

        private TimeLapseImage _VideoPreviewPhoto = null;
        public TimeLapseImage VideoPreviewPhoto
        {
            get { return _VideoPreviewPhoto; }
            set
            {
                SetProperty(ref _VideoPreviewPhoto, value);
                OnPropertyChanged(nameof(HasVideoPreview));
            }
        }

        public bool HasVideoPreview
        {
            get { return VideoPreviewPhoto != null; }
        }

        string _buildStatus;
        public string BuildStatus
        {
            get { return _buildStatus; }
            set { SetProperty(ref _buildStatus, value); }
        }


        public TimeLapseViewModel(ILocalDatabaseService dbService, ISystemUtility sysUtility) : base(dbService, null, sysUtility)
        {
            _photoService = DependencyService.Get<ITakePhotoWithOverlay>();
            _stopMotionService = DependencyService.Get<IStopMotion>();
            _shareMediaService = DependencyService.Get<IShareMediaService>();

            Albums = new ObservableCollection<TimeLapseAlbum>();

            AlbumSelectedCommand = new Command(AlbumSelectedCommandExecute);
            CreateAlbumCommand = new Command(CreateAlbumCommandExecute);

            BeginEditModeCommand = new Command(BeginEditModeCommandExecute);
            EndEditModeCommand = new Command(EndEditModeCommandExecute);
            ImageTappedCommand = new Command(ImageTappedCommandExecute);
            LastPhotoTappedCommand = new Command(LastPhotoTappedCommandExecute, () => { return LastTakenPhoto != null; });
            VideoPreviewTappedCommand = new Command(VideoPreviewTappedCommandExecute);
            TakePhotoCommand = new Command(TakePhotoCommandExecute);
            BuildVideoCommand = new Command(BuildVideoCommandExecute, () => { return this.CanBuild; });
            DeleteCommand = new Command(DeleteCommandExecute, () => HasSelection);
            ShareCommand = new Command(ShareCommandExecute, () => HasSelection);

            Images = new ObservableCollection<TimeLapseImage>();

            MessagingCenter.Subscribe<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE, (page) =>
            {
                if (page is TimeLapseAlbumsPage)
                {
                    IsInEditMode = false;
                    LoadAlbums();
                }
                else if (page is TimeLapseImagesPage)
                {
                    IsInEditMode = false;

                    if (SelectedAlbum != null)
                    {
                        LoadImages();
                    }
                    else
                    {
                        // This would never happen
                        App.NavigationController.NavigateBack();
                    }
                }
            });
        }

        private void BeginEditModeCommandExecute()
        {
            IsInEditMode = true;
        }

        private void EndEditModeCommandExecute()
        {
            UnselectAll();
        }

        private async void AlbumSelectedCommandExecute()
        {
            if (SelectedAlbum != null)
            {
                if (IsInEditMode)
                {
                    // Delete album
                    var confirm = await App.Current.MainPage.DisplayAlert(PatientApp.Resources.PatientApp.LblMyTimeLapseTitle,
                                           string.Format(PatientApp.Resources.PatientApp.AlertConfirmDeleteAlbum, SelectedAlbum.AlbumName),
                                            PatientApp.Resources.PatientApp.BtnOK, PatientApp.Resources.PatientApp.BtnCancel);
                    if (confirm)
                    {
                        DeleteSelectedAlbum();
                    }
                }
                else
                {
                    App.NavigationController.NavigateTo(NavigationController.TIME_LAPSE_IMAGES);
                }
            }
        }

        private void CreateAlbumCommandExecute()
        {
            string albumName = "";

            EntryPopup popup = null;

            popup = new EntryPopup(PatientApp.Resources.PatientApp.LblCreateAlbumTitle,
                                   PatientApp.Resources.PatientApp.LblCreateAlbumHint,
                                   albumName,
                                   PatientApp.Resources.PatientApp.BtnOK,
                                   PatientApp.Resources.PatientApp.BtnCancel)
            {
                MaxLength = 20
            };

            popup.PopupClosed += async (o, closedArgs) =>
            {
                if (closedArgs.ButtonIndex == 0)
                {
                    albumName = closedArgs.Text;
                    if (!string.IsNullOrEmpty(albumName))
                    {
                        // Temp folder in reserved by CrossMedia plugin (pickPhotoAsync method)
                        if (albumName.Equals("temp", StringComparison.CurrentCultureIgnoreCase))
                            albumName = string.Concat(albumName, "_");

                        if (!Albums.Any(a => a.AlbumName.ToLower() == albumName.ToLower()))
                        {
                            _photoService.CreateFolder(albumName);
                            LoadAlbums();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(
                                PatientApp.Resources.PatientApp.LblMyTimeLapseTitle,
                                PatientApp.Resources.PatientApp.ErrorDuplicateAlbumName,
                                PatientApp.Resources.PatientApp.BtnOK);
                            // Show popup again
                            popup.Show();
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                               PatientApp.Resources.PatientApp.LblMyTimeLapseTitle,
                               PatientApp.Resources.PatientApp.ErrorEmptyAlbumName,
                               PatientApp.Resources.PatientApp.BtnOK);
                        // Show popup again
                        popup.Show();
                    }
                }
            };
            popup.Show();
        }

        private void ImageTappedCommandExecute()
        {
            if (IsInEditMode)
                ToggleImageSelection(SelectedImage);
            else
                ShowImageZoomed(SelectedImage);
        }

        private void LastPhotoTappedCommandExecute()
        {
            if (IsInEditMode)
                ToggleImageSelection(LastTakenPhoto);
            else
                ShowImageZoomed(LastTakenPhoto);
        }

        private void VideoPreviewTappedCommandExecute()
        {
            if (IsInEditMode)
                ToggleImageSelection(VideoPreviewPhoto);
            else if (this.SelectedAlbum.HasBuiltVideo)
                App.NavigationController.NavigateTo(NavigationController.TIME_LAPSE_VIDEO);
        }

        private async void TakePhotoCommandExecute()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                ShowErrorMessage(Resources.PatientApp.LblMyTimeLapseTitle, Resources.PatientApp.LblTakePhoto_PinSiteCare_Message);
                return;
            }

            //status = await Permissions.RequestAsync<Permissions.StorageRead>();
            //if (status != PermissionStatus.Granted)
            //{
            //    ShowErrorMessage(Resources.PatientApp.LblMyTimeLapseTitle, Resources.PatientApp.LblTakePhoto_PinSiteCare_Message);
            //    return;
            //}

            var fileName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".jpg";
            _photoService.TakePhoto(SelectedAlbum.AlbumName, fileName, "time_lapse_overlay.png", 0.3f, TakePhotoSuccessCallback, TakePhotoCancelCallback);
        }


        /// <summary>
        /// Manage deletion of selected images and video
        /// </summary>
        private async void DeleteCommandExecute()
        {
            if (this.HasSelection)
            {
                var result = await App.Current.MainPage.DisplayAlert(PatientApp.Resources.PatientApp.LblMyTimeLapseTitle,
                                                                     PatientApp.Resources.PatientApp.AlertTimeLapseDeleteImages,
                                                                     PatientApp.Resources.PatientApp.BtnOK,
                                                                     PatientApp.Resources.PatientApp.BtnCancel);
                if (result)
                {
                    IsBusy = true;

                    foreach (var image in SelectedImages)
                    {
                        try
                        {
                            _photoService.DeletePhoto(image.ImagePath);
                        }
                        catch
                        {
                        }
                    }

                    if (LastTakenPhoto != null && LastTakenPhoto.IsSelected)
                    {
                        try
                        {
                            _photoService.DeletePhoto(LastTakenPhoto.ImagePath);
                        }
                        catch
                        {
                        }
                    }

                    if (VideoPreviewPhoto != null && VideoPreviewPhoto.IsSelected)
                    {
                        try
                        {
                            _stopMotionService.DeleteVideoFile(this.SelectedAlbum.VideoFullPath);
                            SelectedAlbum.VideoFullPath = null;
                        }
                        catch
                        {
                        }
                    }

                    IsBusy = false;
                    LoadImages();
                    UnselectAll();
                }
            }
        }

        /// <summary>
        /// Manage sharing of selected images and video
        /// </summary>
        private void ShareCommandExecute()
        {
            var shareImages = new List<string>();
            var shareVideos = new List<string>();

            // Create two lists of files (images and video) to share
            shareImages.AddRange(SelectedImages.Select(i => i.ImagePath));

            if (LastTakenPhoto != null && LastTakenPhoto.IsSelected)
                shareImages.Add(this.LastTakenPhoto.ImagePath);

            if (VideoPreviewPhoto != null && VideoPreviewPhoto.IsSelected && !string.IsNullOrEmpty(this.SelectedAlbum.VideoFullPath))
                shareVideos.Add(this.SelectedAlbum.VideoFullPath);

            if (shareImages.Any() || shareVideos.Any())
            {
                try
                {
                    _shareMediaService.ShareImagesAndVideos(shareImages, shareVideos,
                        () =>
                        {
                            UnselectAll();
                        });
                }
                catch (Exception ex)
                {

                }
            }

        }

        /// <summary>
        /// Toggle the selection of the argument image
        /// </summary>
        /// <param name="image"></param>
        private void ToggleImageSelection(TimeLapseImage image)
        {
            if (image != null)
            {
                image.IsSelected = !image.IsSelected;

                ShareCommand.ChangeCanExecute();
                DeleteCommand.ChangeCanExecute();
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        private void UnselectAll()
        {
            IsInEditMode = false;
            if (Albums != null)
            {
                foreach (var album in Albums)
                    album.IsSelected = false;
            }
            if (Images != null)
            {
                foreach (var image in SelectedImages)
                    image.IsSelected = false;
            }

            if (LastTakenPhoto != null)
                LastTakenPhoto.IsSelected = false;
            if (VideoPreviewPhoto != null)
                VideoPreviewPhoto.IsSelected = false;

            OnPropertyChanged(nameof(HasSelection));
        }

        private void ShowImageZoomed(TimeLapseImage image)
        {
            ZoomImage = image;
            App.NavigationController.NavigateTo(NavigationController.TIME_LAPSE_ZOOM, true);
        }

        /// <summary>
        /// Populates the list of saved album 
        /// </summary>
        private void LoadAlbums()
        {
            Albums.Clear();
            foreach (var folderName in _photoService.GetFolders())
            {
                var images = _photoService.GetTakenPhotoes(folderName).OrderBy(p => p);
                var videos = _stopMotionService.GetBuiltVideos(folderName);
                var album = new TimeLapseAlbum()
                {
                    AlbumName = folderName,
                    ImagesCount = images.Count(),
                    PreviewImagePath = images != null && images.Any() ? images.First() : null,
                    VideoFullPath = videos != null && videos.Any() ? videos.First() : null,
                };

                Albums.Add(album);
            }

            OnPropertyChanged(nameof(HasAlbums));
        }

        /// <summary>
        /// Delete the selected album folder and its content
        /// </summary>
        private void DeleteSelectedAlbum()
        {
            if (SelectedAlbum != null)
            {
                try
                {
                    _photoService.DeleteFolder(SelectedAlbum.AlbumName);
                    SelectedAlbum = null;
                    LoadAlbums();
                    if (IsInEditMode && !this.Albums.Any())
                        UnselectAll();
                }
                catch (Exception ex)
                {
                    // TODO: Display error
                    App.Current.MainPage.DisplayAlert(PatientApp.Resources.PatientApp.LblMyTimeLapseTitle, "Error deleting album", "OK");
                }
            }
        }

        /// <summary>
        /// Populates the list of images contained in the selected album
        /// </summary>
        private void LoadImages()
        {
            IsBusy = true;
            Images.Clear();
            var savedImages = _photoService.GetTakenPhotoes(SelectedAlbum.AlbumName).OrderBy(p => p);
            if (savedImages.Any())
            {
                bool isFirst = true;
                foreach (var imagePath in savedImages.Reverse())
                {
                    var timeLapseImage = new TimeLapseImage()
                    {
                        ImagePath = imagePath,
                        ImageTitle = Path.GetFileNameWithoutExtension(imagePath),
                        CreatedOn = DateTime.ParseExact(Path.GetFileNameWithoutExtension(imagePath), "yyyyMMdd-HHmmss", null)
                    };

                    if (isFirst)
                    {
                        LastTakenPhoto = timeLapseImage;
                        isFirst = false;
                    }
                    else
                        Images.Add(timeLapseImage);
                }
            }
            else
            {
                LastTakenPhoto = null;
            }

            // Create placeholder for video preview
            if (SelectedAlbum.HasBuiltVideo)
            {
                VideoPreviewPhoto = new TimeLapseImage()
                {
                    ImagePath = "video_placeholder"
                };
            }
            else
                VideoPreviewPhoto = null;

            OnPropertyChanged(nameof(HasLastPhoto));
            OnPropertyChanged(nameof(HasImages));
            OnPropertyChanged(nameof(CanBuild));
            OnPropertyChanged(nameof(HasVideoPreview));
            OnPropertyChanged(nameof(CanEdit));
            BuildVideoCommand.ChangeCanExecute();
            IsBusy = false;
        }

        private async void TakePhotoSuccessCallback(string filePath)
        {
            LoadImages();

            // build time lapse video after taking 3 photoes (1 last + 2 in the gridview) without a saved built video 
            if (this.CanBuild && !SelectedAlbum.HasBuiltVideo)
            {
                BuildVideoCommand.Execute(null);
            }

        }

        private void TakePhotoCancelCallback(string error)
        {
            App.Current.MainPage.DisplayAlert("Error", error, "OK");
        }

        private async void BuildVideoCommandExecute()
        {
            if (!this.CanBuild)
            {
                App.Current.MainPage.DisplayAlert(PatientApp.Resources.PatientApp.LblMyTimeLapseTitle, PatientApp.Resources.PatientApp.AlertTimeLapseMinPhoto, PatientApp.Resources.PatientApp.BtnOK);
                return;
            }

            var confirm = await App.Current.MainPage.DisplayAlert(
                             PatientApp.Resources.PatientApp.LblMyTimeLapseTitle,
                             (SelectedAlbum.HasBuiltVideo
                                 ? Resources.PatientApp.AlertTimeLapseRebuild
                                 : Resources.PatientApp.AlertTimeLapseBuild),
                             Resources.PatientApp.BtnOK, Resources.PatientApp.BtnCancel);

            if (confirm)
            {
                // Delete old video
                if (!string.IsNullOrEmpty(this.SelectedAlbum.VideoFullPath) && _stopMotionService.VideoFileExists(this.SelectedAlbum.VideoFullPath))
                    _stopMotionService.DeleteVideoFile(this.SelectedAlbum.VideoFullPath);

                IsBusy = true;
                List<string> sourceImages = new List<string>();
                sourceImages.AddRange(this.Images.OrderBy(i => i.CreatedOn).Select(i => i.ImagePath));
                sourceImages.Add(this.LastTakenPhoto.ImagePath);
                var videoFileName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".mp4";
                _stopMotionService.StartBuildVideo(sourceImages, SelectedAlbum.AlbumName, videoFileName, 1000, BuildProcessCallback, BuildCompletedCallback, BuildErrorCallback);
            }

        }

        private void BuildProcessCallback(long processIndex, long total)
        {
            BuildStatus = string.Format("Processing image {0} of {1}...", processIndex, total);
        }

        private void BuildCompletedCallback(string videoFilePath)
        {
            IsBusy = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                BuildStatus = string.Empty;
                SelectedAlbum.VideoFullPath = videoFilePath;
                if (SelectedAlbum.HasBuiltVideo)
                    App.NavigationController.NavigateTo(NavigationController.TIME_LAPSE_VIDEO);
            });

        }

        private void BuildErrorCallback(string error)
        {
            IsBusy = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Error", String.Format("Build failed {0}", error), "OK");
            });
        }
    }
}

