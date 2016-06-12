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

//using CandyCrush.Entities;

namespace CandyCrush
{
    //  A class for a grid
    public class gridLayer : CCLayerColor
    {
        private int gridRows, gridColumns;
        private candy[,] grid;
        private Random rand = new Random();

        public gridLayer()
        {
            gridColumns = 9;
            gridRows = 9;
            grid = new candy[gridRows, gridColumns];
            fillGrid(); // fills the grid for the first time
            grid[0, 0].Position = new CCPoint(500, 500);
            AddChild(grid[0, 0]);
        }

        public int getRows()
        {
            return gridRows;
        }

        public int getColumns()
        {
            return gridColumns;
        }

        public void fillGrid()
        {
            for (int i = 0; i < gridRows; i++)
            {
                for (int j = 0; j < gridColumns; j++)
                {
                    assignCandy(i, j, "none"); // assigns a new candy the location [i,j] in the grid
                }
            }
        }

        //  Assigns a candy at the grid location [row, col]
        //  candies should have no stripes when the level is first loaded
        //  dir allows the creation of the striped candies
        //  this method makes generateSpeical() included in candy class redundant, plus this method is more direct
        public void assignCandy(int row, int col, string dir)
        {
            grid[row, col] = new candy(rand, dir, row, col);
        }

        //  Displays the candy at the specific location in the  grid
        public string displayCandy(int row, int col)
        {
            return grid[row, col].getType();
        }

        //  Displays all of the candies in the grid recursively from the first candy to the last, row-wise
        //  Made to display only text-wise and not visually
        public string displaygrid(int row, int col)
        {
            if (col == 8 && row != 8)
            {
                return " " + grid[row, col].getType() + displaygrid(row + 1, 0);
            }
            else if (col != 8)
            {
                return " " + grid[row, col].getType() + displaygrid(row, col + 1);
            }
            return " " + grid[row, col].getType();
        }

        public void findCombos()
        {
            string candyType = grid[0, 0].getType();
            int horizontalLength = 1;
            int row = 0;
            for (int col = 0; col >= 0 && grid[row, col - 1].getType() == candyType; col--)
            {
                horizontalLength++;
            }
        }
    }
}