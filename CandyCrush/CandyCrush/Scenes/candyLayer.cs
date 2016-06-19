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
        private CCLabel debugLabel;

        public candyLayer()
        {
            gridColumns = 9;
            gridRows = 9;
            grid = new candy[gridRows, gridColumns];
            fillGrid(); // fills the grid for the first time
            addCandies(); // adds the candies to the layer to be displayed
            addDebug();
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

        void addDebug()
        {
            debugLabel = new CCLabel("Debug info shows here...", "Arial", 30, CCLabelFormat.SystemFont);
            debugLabel.Color = CCColor3B.Black;
            //debugLabel.Position = new CCPoint(10, 10);
            debugLabel.AnchorPoint = new CCPoint(0, 0);
            AddChild(debugLabel);
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
                    grid[i, j].Position = new CCPoint(70 + (62 * j), 810 - (70 * i));
                    AddChild(grid[i, j]);
                }
            }
        }

        public candy candyAt(int row, int col)
        {
            return grid[row, col];
        }

        public bool convertToPoint(CCPoint location, ref int row, ref int col)
        {
            if (location.X >= 38 && location.X < 598 && location.Y >= 216 && location.Y < 846)
            {
                debugLabel.Text = "Touch was within the grid.";
                row = (846 - Convert.ToInt32(location.Y)) / 70;
                col = (Convert.ToInt32(location.X) - 38) / 62;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void trySwapHorizontal(int horzDelta, int vertDelta, int fromRow, int fromCol)
        {
            debugLabel.Text = "checking to see if a swap is possible.";
            int toRow = fromRow + vertDelta;
            int toCol = fromCol + horzDelta;

            if (toRow < 0 || toRow >= gridRows)
            {
                return;
            }
            if (toCol < 0 || toCol >= gridColumns)
            {
                return;
            }

            candy toCandy = candyAt(toRow, toCol);
            candy fromCandy = candyAt(fromRow, fromCol);
            debugLabel.Text = "Switching candy at [" + fromRow + ", " + fromCol + "] with candy at [" + toRow + ", " + toCol + "].";

            //toCandy.Position = new CCPoint(70 + (62 * toRow), 810 - (70 * toCol));
            ////grid[toCandy.getRow(), toCandy.getColumn()].Position = new CCPoint(70 + (62 * toCandy.getRow()), 810 - (70 * toCandy.getColumn()));
            //toCandy.setPosition(toRow, toCol);
            //fromCandy.Position = new CCPoint(70 + (62 * fromRow), 810 - (70 * fromCol));
            ////grid[fromCandy.getRow(), fromCandy.getColumn()].Position = new CCPoint(70 + (62 * fromCandy.getRow()), 810 - (70 * fromCandy.getColumn()));
            //fromCandy.setPosition(fromRow, fromCol);
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