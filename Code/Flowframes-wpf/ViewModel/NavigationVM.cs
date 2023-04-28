using Flowframes_wpf.Utilities;

using System.Windows.Input;

namespace Flowframes_wpf.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand InterpolationCommand { get; set; }
        public ICommand QuickSettingsCommand { get; set; }
        public ICommand PreviewCommand { get; set; }

        public ICommand AboutCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Interpolation(object obj) => CurrentView = new InterpolationVM();
        private void QuickSettings(object obj) => CurrentView = new QuickSettingsVM();
        private void Preview(object obj) => CurrentView = new PreviewVM();
        private void About(object obj) => CurrentView = new AboutVM();
        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            InterpolationCommand = new RelayCommand(Interpolation);
            QuickSettingsCommand = new RelayCommand(QuickSettings);
            PreviewCommand = new RelayCommand(Preview);
            AboutCommand = new RelayCommand(About);

            CurrentView = new HomeVM();
        }
    }
}
