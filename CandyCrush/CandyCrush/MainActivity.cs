using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CandyCrush
{
    [Activity(Label = "CandyCrush", MainLauncher = true, Icon = "@drawable/icon")]

    public class MainActivity : Activity
    {
        int count = 1;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Button btnFindCombos = FindViewById<Button>(Resource.Id.btnClear);
            candyGrid gameGrid = new candyGrid();

            button.Click += delegate {
                // assign candies to the grid
                for (int row = 0; row < gameGrid.getRows(); row++)
                {
                    for (int column = 0; column < gameGrid.getColumns(); column++)
                    {
                        gameGrid.assignCandy(row, column);
                    }
                }

                TextView displayCandies = new TextView(this);

                displayCandies.Text = gameGrid.displaygrid(0, 0);

                LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                layout.AddView(displayCandies);

                button.Text = gameGrid.displayCandy(8,5) + " " + count++;
            };

            btnFindCombos.Click += delegate
            {
                gameGrid.findCombos();

                TextView displayCandies = new TextView(this);

                displayCandies.Text = gameGrid.displaygrid(0, 0);

                LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                layout.AddView(displayCandies);
            };
        }
    }
}