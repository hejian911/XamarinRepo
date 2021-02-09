﻿using System.Net;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MobileX.Droid.DomainModel;
using Newtonsoft.Json;

namespace MobileX.Droid
{
    [Activity(Label = "UserProfileActivity")]
    public class UserProfileActivity : Activity
    {
        private UserProfile _userProfile;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_user_profile);

            GetLoginResult(savedInstanceState);

            DisplayProfileInfo();
        }

        private void GetLoginResult(Bundle savedInstanceState)
        {
            string loginResultAsJson = Intent.GetStringExtra("LoginResult") ?? string.Empty;
            _userProfile = JsonConvert.DeserializeObject<UserProfile>(loginResultAsJson);
        }
        private void DisplayProfileInfo()
        {
                FindViewById<TextView>(Resource.Id.text1).Text = _userProfile.Name;
                FindViewById<TextView>(Resource.Id.text2).Text = _userProfile.Email;

                var imageBitmap = GetImageBitmapFromUrl(_userProfile.ProfilePictureUrl);
                FindViewById<ImageView>(Resource.Id.userProfileImageView).SetImageBitmap(imageBitmap);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

    }
}