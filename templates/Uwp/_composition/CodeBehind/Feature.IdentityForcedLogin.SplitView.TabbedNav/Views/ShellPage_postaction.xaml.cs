//{[{
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Models;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private WinUI.NavigationViewItem _selected;
//{[{
        private UserData _user;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
//}]}
        public WinUI.NavigationViewItem Selected
        {
        }
//^^
//{[{
        public UserData User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
//}]}

        public ShellPage()
        {
        }

        private void Initialize()
        {
//^^
//{[{
            IdentityService.LoggedOut += OnLoggedOut;
//}]}
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{
            User = await UserDataService.GetUserFromCacheAsync();
            User = await UserDataService.GetUserFromGraphApiAsync();
            if (User == null)
            {
                User = UserDataService.GetDefaultUserData();
            }
//}]}
        }
//{[{

        private void OnLoggedOut(object sender, EventArgs e)
        {
            NavigationService.Navigated -= Frame_Navigated;
            navigationView.BackRequested -= OnBackRequested;
            IdentityService.LoggedOut -= OnLoggedOut;
        }

        private void OnUserProfile(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate<SettingsPage>();
        }
//}]}
    }
}
