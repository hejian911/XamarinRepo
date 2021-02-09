using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileX
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public App()
        {
            InitializeComponent();

       	   if (!IsUserLoggedIn) {
				MainPage = new NavigationPage (new LoginPage ());
			} else {
				MainPage = new NavigationPage (new MainPage ());
			}
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
