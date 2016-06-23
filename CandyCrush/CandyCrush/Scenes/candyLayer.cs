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
        readonly CCNode drawNodeRoot;

        public candyLayer()
        {
            gridColumns = 9;
            gridRows = 9;
            grid = new candy[gridRows, gridColumns];
            fillGrid(); // fills the grid for the first time
            addCandies(); // adds the candies to the layer to be displayed
            addDebug();
            drawNodeRoot = new CCNode { PositionX = 500f, PositionY = 350f };
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

        private void addDebug()
        {
            debugLabel = new CCLabel("Debug info shows here...", "Arial", 30, CCLabelFormat.SystemFont);
            debugLabel.Color = CCColor3B.Black;
            //debugLabel.Position = new CCPoint(10, 10);
            debugLabel.AnchorPoint = new CCPoint(0, 0);
            AddChild(debugLabel);
        }

        //  Assigns a candy at the grid location [row, col]
        //  candies should have no stripes when the level is first loaded
        public void assignCandy(int row, int col)
        {
            candy newCandy = new candy(rand, row, col);
            while ((col >= 2 && grid[row, col - 1].getType() == newCandy.getType() && grid[row, col - 2].getType() == newCandy.getType())
                || (row >= 2 && grid[row - 1, col].getType() == newCandy.getType() && grid[row - 2, col].getType() == newCandy.getType()))
            {
                newCandy = new candy(rand, row, col);
            }
            grid[row, col] = newCandy;
        }

        //  Adds the candies to the layer and positions them on screen
        //  based on their position in the grid
        private void addCandies()
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

        //  gets the candy that's at the given [row, col] position in the grid
        public candy candyAt(int row, int col)
        {
            return grid[row, col];
        }

        //  Check to see if the touch location is within the grid and if it is
        //  then returns true and the row and column position of the candy
        public bool convertToPoint(CCPoint location, ref int row, ref int col)
        {
            if (location.X >= 38 && location.X < 598 && location.Y >= 216 && location.Y < 846)
            {
                //debugLabel.Text = "Touch was within the grid.";
                row = convertYToRow(location.Y);   //(846 - Convert.ToInt32(location.Y)) / 70;
                col = convertXToColumn(location.X);    //(Convert.ToInt32(location.X) - 38) / 62;
                candy debugCandy = candyAt(row, col);
                debugLabel.Text = "Touched the candy at [" + debugCandy.getRow() + ", " + debugCandy.getColumn() + "]";
                return true;
            }
            else
            {
                return false;
            }
        }

        //  Checks to see if a swap is possible, if it is then it will do so
        //  otherwise it will call for a failed swap animation
        public void trySwap(int horzDelta, int vertDelta, int fromRow, int fromCol)
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

            animateSwap(fromCandy, toCandy); // complete and animate the swap
        }

        //  Visually animates the swap using the CCMoveTo function provided by CocosSharp,
        //  also updates the grid location of the candies
        private void animateSwap(candy fromCandy, candy toCandy)
        {
            const float timeToTake = 0.5f; // in seconds
            CCFiniteTimeAction coreAction = null;

            //  Store the positions of the candies to be used to swap them
            CCPoint positionA = new CCPoint(fromCandy.Position);
            CCPoint positionB = new CCPoint(toCandy.Position);

            //  Animate the swapping of the candies
            coreAction = new CCMoveTo(timeToTake, positionB);
            fromCandy.AddAction(coreAction);
            coreAction = new CCMoveTo(timeToTake, positionA);
            toCandy.AddAction(coreAction);

            //  Update the row and column positions for each candy
            fromCandy.setPosition(convertYToRow(positionB.Y), convertXToColumn(positionB.X));
            toCandy.setPosition(convertYToRow(positionA.Y), convertXToColumn(positionA.X));

            //  Update the position of the candies within the grid
            grid[fromCandy.getRow(), fromCandy.getColumn()] = fromCandy;
            grid[toCandy.getRow(), toCandy.getColumn()] = toCandy;
        }

        private int convertYToRow(float y)
        {
            return (846 - Convert.ToInt32(y)) / 70;
        }

        private int convertXToColumn(float x)
        {
            return (Convert.ToInt32(x) - 38) / 62;
        }

        
    }
}