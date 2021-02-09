using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Auth0.OidcClient;
using System.Threading.Tasks;
using MobileX.Droid.DomainModel;
using Android.Content;
using Newtonsoft.Json;

namespace MobileX.Droid
{
    [Activity(Label = "MobileX", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
 
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "com.companyname.MobileX",   
        DataHost = "dev-b30q6rye.au.auth0.com",
        DataPathPrefix = "/android/YOUR_ANDROID_PACKAGE_NAME/callback")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

          private Auth0Client _auth0Client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _auth0Client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "dev-b30q6rye.au.auth0.com",
                ClientId = "1LI6lgyfqvAnQ3Kk1DgDKvrSOD01eFdG"
            });
             SetContentView(Resource.Layout.activity_main);
             var loginButton = FindViewById<Button>(Resource.Id.button1);
            loginButton.Click += LoginButton_Click;
          //  LoadApplication(new App());
        }

      private async void LoginButton_Click(object sender, EventArgs e)
        {
            await LoginAsync();
        }
        private async Task LoginAsync()
        {
            var loginResult = await _auth0Client.LoginAsync();

            if (!loginResult.IsError)
            {
                var name = loginResult.User.FindFirst(c => c.Type == "name")?.Value;
                var email = loginResult.User.FindFirst(c => c.Type == "email")?.Value;
                var image = loginResult.User.FindFirst(c => c.Type == "picture")?.Value;

                var userProfile = new UserProfile
                {
                    Email = email,
                    Name = name,
                    ProfilePictureUrl = image
                };

                var intent = new Intent(this, typeof(UserProfileActivity));
                var serializedLoginResponse = JsonConvert.SerializeObject(userProfile);
                intent.PutExtra("LoginResult", serializedLoginResponse);
                StartActivity(intent);
            }
            else
            {
                Console.WriteLine($"An error occurred during login: {loginResult.Error}");
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}