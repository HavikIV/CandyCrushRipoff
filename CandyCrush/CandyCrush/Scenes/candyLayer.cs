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

//using CandyCrush.Entities;

namespace CandyCrush
{
    //  A class for a grid
    public class candyLayer : CCLayerColor
    {
        private int gridRows, gridColumns;
        private candy[,] grid;
        private Random rand = new Random();

        public candyLayer()
        {
            gridColumns = 9;
            gridRows = 9;
            grid = new candy[gridRows, gridColumns];
            fillGrid(); // fills the grid for the first time
            addCandies(); // adds the candies to the layer to be displayed
        }

        public void fillGrid()
        {
            for (int i = 0; i < gridRows; i++)
            {
                for (int j = 0; j < gridColumns; j++)
                {
                    assignCandy(i, j); // assigns a new candy the location [i,j] in the grid
                }
            }
        }

        //  Assigns a candy at the grid location [row, col]
        //  candies should have no stripes when the level is first loaded
        //  dir allows the creation of the striped candies
        //  this method makes generateSpeical() included in candy class redundant, plus this method is more direct
        public void assignCandy(int row, int col)
        {
            grid[row, col] = new candy(rand, row, col);
        }

        void addCandies()
        {
            for (int i = 0; i < gridRows; i++)
            {
                for (int j = 0; j < gridColumns; j++)
                {
                    grid[i, j].Position = new CCPoint(70 + (62 * i), 810 - (70 * j));
                    AddChild(grid[i, j]);
                }
            }
        }

        //public void findCombos()
        //{
        //    string candyType = grid[0, 0].getType();
        //    int horizontalLength = 1;
        //    int row = 0;
        //    for (int col = 0; col >= 0 && grid[row, col - 1].getType() == candyType; col--)
        //    {
        //        horizontalLength++;
        //    }
        //}
    }
}