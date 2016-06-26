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

using CocosSharp;
using CandyCrush.Entities;

namespace CandyCrush
{
    class Chain : CCNode
    {
        public List<candy> candies;
        public ChainType chainType;

        public enum ChainType
        {
            ChainTypeHorizontal, ChainTypeVertical
        };

        public Chain()
        {
            candies = new List<candy>();
        }

        public void addCandy(candy candy)
        {
            candies.Add(candy);
        }
    }
}