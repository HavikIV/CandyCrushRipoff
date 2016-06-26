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
        private int gridRows, gridColumns, possibleSwapCount;
        private candy[,] grid;
        private Random rand = new Random();
        private CCLabel debugLabel;
        private List<Swap> possibleSwaps;

        public candyLayer()
        {
            gridColumns = 9;
            gridRows = 9;
            grid = new candy[gridRows, gridColumns];
            fillGrid(); // fills the grid for the first time
            addCandies(); // adds the candies to the layer to be displayed
            addDebug();
            //possibleSwaps = new List<Swap>();
            detectPossibleSwap();
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

        //  Detects how many swaps are possible, and adds the possible swaps in the possibleSwaps array
        //  Will return the number of possible swaps that were detected in the grid
        private void detectPossibleSwap()
        {
            possibleSwaps = new List<Swap>();
            // for loop to go through the grid to find possible swaps
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns; col++)
                {
                    //  Grab the candy from grid
                    candy checkCandy = grid[row, col];

                    //  Make sure that there's a candy at the given grid location
                    if (checkCandy != null)
                    {
                        //  See if it's possible to swap to the right
                        if (col < gridColumns - 1)
                        {
                            //  Grab the candy to the right from the checkCandy
                            candy otherCandy = grid[row, col + 1];
                            if (otherCandy != null)
                            {
                                //  Swap the candies
                                grid[row, col] = otherCandy;
                                grid[row, col + 1] = checkCandy;

                                //  Check to see if either one of the swapped candies is now part of a chain
                                if (hasChainAt(row, col + 1) || hasChainAt(row, col))
                                {
                                    Swap swap = new Swap();
                                    swap.candyA = checkCandy;
                                    swap.candyB = otherCandy;

                                    //  Add the candies to the array of possibleSwaps
                                    possibleSwaps.Add(swap);
                                }

                                //  Swap the candies back to their original positions
                                grid[row, col] = checkCandy;
                                grid[row, col + 1] = otherCandy;
                            }
                        }

                        //  See if it's possible to swap below
                        if (row < gridRows - 1)
                        {
                            //  Grab the candy to the right from the checkCandy
                            candy otherCandy = grid[row + 1, col];
                            if (otherCandy != null)
                            {
                                //  Swap the candies
                                grid[row, col] = otherCandy;
                                grid[row + 1, col] = checkCandy;

                                //  Check to see if either one of the swapped candies is now part of a chain
                                if (hasChainAt(row + 1, col) || hasChainAt(row, col))
                                {
                                    Swap swap = new Swap();
                                    swap.candyA = checkCandy;
                                    swap.candyB = otherCandy;

                                    //  Add the candies to the array of possibleSwaps
                                    possibleSwaps.Add(swap);
                                }

                                //  Swap the candies back to their original positions
                                grid[row, col] = checkCandy;
                                grid[row + 1, col] = otherCandy;
                            }
                        }
                    }
                }
            }
        }

        //  Check to see if a swap is possible
        private bool isSwapPossible(Swap swap)
        {
            foreach (var item in possibleSwaps)
            {
                if ((item.candyA == swap.candyA && item.candyB == swap.candyB) ||(item.candyA == swap.candyB && item.candyB == swap.candyA))
                {
                    return true;
                }
            }
            return false;
        }

        //  See if there's a chain at the given location in the grid
        private bool hasChainAt(int row, int col)
        {
            int cookieType = grid[row, col].getType();

            //  Check to see if there's a chain in the row on either side of the candy
            int horzLenght = 1;
            for (int i = col - 1; i >= 0 && grid[row, i].getType() == cookieType; i--)
            {
                horzLenght++;
            }
            for (int i = col + 1; i < gridColumns && grid[row, i].getType() == cookieType; i++)
            {
                horzLenght++;
            }

            //  Returns true if there's a chain in the row
            if (horzLenght >= 3)
            {
                return true;
            }

            //  Check to see if there's a chain in the column either above/below the candy
            int vertLength = 1;
            for (int i = row - 1; i >= 0 && grid[i, col].getType() == cookieType; i--)
            {
                vertLength++;
            }
            for (int i = row + 1; i < gridRows && grid[i, col].getType() == cookieType; i++)
            {
                vertLength++;
            }

            //  Returns true if there's a chain in the column
            //  This also becomes the default return for the method
            return (vertLength >= 3);
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

            Swap swap = new Swap();
            swap.candyA = fromCandy;
            swap.candyB = toCandy;

            if (isSwapPossible(swap))
            {
                // Swap them
                animateSwap(swap);
                detectPossibleSwap();
            }
            else
            {
                //  Swap is not possible so run the failed swap animation
                failedSwapAnimation(swap);
            }
            //animateSwap(fromCandy, toCandy); // complete and animate the swap
            //animateSwap(swap);
        }

        //  Visually animates the swap using the CCMoveTo function provided by CocosSharp,
        //  also updates the grid location of the candies
        private void animateSwap(Swap swap) //candy fromCandy, candy toCandy
        {
            const float timeToTake = 0.5f; // in seconds
            CCFiniteTimeAction coreAction = null;

            //  Store the positions of the candies to be used to swap them
            CCPoint positionA = new CCPoint(swap.candyA.Position);
            CCPoint positionB = new CCPoint(swap.candyB.Position);

            //  Animate the swapping of the candies
            coreAction = new CCMoveTo(timeToTake, positionB);
            swap.candyA.AddAction(coreAction);
            coreAction = new CCMoveTo(timeToTake, positionA);
            swap.candyB.AddAction(coreAction);

            //  Update the row and column positions for each candy
            swap.candyA.setPosition(convertYToRow(positionB.Y), convertXToColumn(positionB.X));
            swap.candyB.setPosition(convertYToRow(positionA.Y), convertXToColumn(positionA.X));

            //  Update the position of the candies within the grid
            grid[swap.candyA.getRow(), swap.candyA.getColumn()] = swap.candyA;
            grid[swap.candyB.getRow(), swap.candyB.getColumn()] = swap.candyB;
        }

        //  Animation for a failed swap
        private void failedSwapAnimation(Swap swap)
        {
            const float timeToTake = 0.1f; // in seconds
            CCFiniteTimeAction coreAction = null;
            CCFiniteTimeAction secondAction = null;

            //  Store the positions of the candies to be used to swap them
            CCPoint positionA = new CCPoint(swap.candyA.Position);
            CCPoint positionB = new CCPoint(swap.candyB.Position);

            //  Animate moving the candies back and forth
            coreAction = new CCMoveTo(timeToTake, positionB);
            secondAction = new CCMoveTo(timeToTake, positionA);
            swap.candyA.RunActions(coreAction, secondAction);
            coreAction = new CCMoveTo(timeToTake, positionA);
            secondAction = new CCMoveTo(timeToTake, positionB);
            swap.candyB.RunActions(coreAction, secondAction);
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