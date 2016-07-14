using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CandyCrush
{
    class Level
    {
        public int[,] tiles { get; set; }
        public int targetScore { get; set; }
        public int moves { get; set; }
    }
}