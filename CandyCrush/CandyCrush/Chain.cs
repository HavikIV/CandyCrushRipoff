using System.Collections.Generic;

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
            Horizontal, Vertical
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