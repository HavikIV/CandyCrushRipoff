using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using CocosSharp;

namespace CandyCrush
{
    [Activity(Label = "CandyCrush", MainLauncher = true, Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        Theme = "@android:style/Theme.NoTitleBar",
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our game view from the layout resource,
            // and attach the view created event to it
            CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);
            gameView.ViewCreated += LoadGame;
        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null)
            {
                var contentSearchPaths = new List<string>() { "Fonts", "Sounds" };
                CCSizeI viewSize = gameView.ViewSize;

                int width = 768;
                int height = 1024;

                // Set world dimensions
                gameView.DesignResolution = new CCSizeI(width, height);
                

                // Determine whether to use the high or low def versions of our images
                // Make sure the default texel to content size ratio is set correctly
                // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
                if (width < viewSize.Width)
                {
                    contentSearchPaths.Add("Images/Hd");
                    CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
                }
                else
                {
                    contentSearchPaths.Add("Images/Ld");
                    CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
                }

                gameView.ContentManager.SearchPaths = contentSearchPaths;

                //CCScene gameScene = new CCScene(gameView);
                //gameScene.AddLayer(new GameLayer());
                //gameScene.AddLayer(new gridLayer());
                //gameScene.AddLayer(new tileLayer());
                CCScene gameScene = new GameScene(gameView);
                gameView.RunWithScene(gameScene);
            }
        }
    }
}

