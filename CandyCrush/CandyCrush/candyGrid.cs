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
    //  A class for a grid
    public class candyGrid
    {
        private int gridRows, gridColumns;
        private candy[,] grid;
        private Random rand = new Random();

        public candyGrid()
        {
            gridColumns = 9;
            gridRows = 9;
            grid = new candy[gridRows, gridColumns];
        }

        public int getRows()
        {
            return gridRows;
        }

        public int getColumns()
        {
            return gridColumns;
        }

        public void assignCandy(int row, int col)
        {
            grid[row, col] = new candy(rand, "none"); // candies should have no stripes when the level is loaded
        }

        //  Displays the candy at the specific location in the  grid
        public string displayCandy(int row, int col)
        {
            return grid[row, col].getType();
        }

        //  Displays all of the candies in the grid recursively from the first candy to the last, row-wise
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
            bool candyDestroyed = false;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (grid[row, col].getType() == grid[row, col + 1].getType() && grid[row, col].getType() == grid[row, col + 2].getType())
                    {
                        grid[row, col] = null;
                        grid[row, col + 1] = null;
                        grid[row, col + 2] = null;
                        candyDestroyed = true;
                    }
                    //if (row <= 7 && candyDestroyed == false)
                    //{
                    //    if(grid[row, col].getType() == grid[row + 1, col].getType() && grid[row, col].getType() == grid[row + 2, col].getType())
                    //    {
                    //        grid[row, col] = null;
                    //        grid[row + 1, col] = null;
                    //        grid[row + 2, col] = null;
                    //    }
                    //}
                    if (candyDestroyed)
                    {
                        col += 2;
                        candyDestroyed = false;
                    }
                }
            }
        }


    }
}