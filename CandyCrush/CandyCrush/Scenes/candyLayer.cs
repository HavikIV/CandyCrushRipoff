using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private CCLabel debugLabel, scoreLabel;
        private List<Swap> possibleSwaps;
        private List<Chain> deleteChains;
        private bool dropped, filledAgain, finishedRemoving;
        private bool pointGone;

        public candyLayer()
        {
            gridColumns = 9;
            gridRows = 9;
            possibleSwapCount = 0;
            grid = new candy[gridRows, gridColumns];
            shuffle(); // fills the grid for the first time
            addCandies(); // adds the candies to the layer to be displayed
            addDebug();
            detectPossibleSwap();
            scoreLabel = new CCLabel("0", "Arial", 130, CCLabelFormat.SystemFont);
            scoreLabel.Color = CCColor3B.Green;
            scoreLabel.AnchorPoint = new CCPoint(0, 0);
            scoreLabel.Position = new CCPoint(540, 1000);
            AddChild(scoreLabel);
        }

        //  This method keep on filling up the grid until there's at least one possible swap that can be made
        public void shuffle()
        {
            do
            {
                fillGrid();
                detectPossibleSwap();
                possibleSwapCount = possibleSwaps.Count;
            }
            while (possibleSwapCount == 0);
        }

        //  Fills the grid up with new candies
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
                if ((item.candyA == swap.candyA && item.candyB == swap.candyB) || (item.candyA == swap.candyB && item.candyB == swap.candyA))
                {
                    return true;
                }
            }
            return false;
        }

        //  See if there's a chain at the given location in the grid
        public bool hasChainAt(int row, int col)
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
        public async void trySwap(int horzDelta, int vertDelta, int fromRow, int fromCol)
        {
            debugLabel.Text = "checking to see if a swap is possible.";
            int toRow = fromRow + vertDelta;
            int toCol = fromCol + horzDelta;

            //  Make sure that the user didn't swipe out of the grid as there isn't any candies to swap with out there
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
                await Task.Delay(300);  // Wait for the swap animation to finish before continuing
                dropped = false;    // Sets dropped to false, it will be used to check if the game finished dropping all of the candies
                filledAgain = false;
                finishedRemoving = false;
                do
                {
                    //  My reason to add the while loops with the awaits is that the App seems to come back to this do while even before the
                    //  the method completely finish running. I'm guessing that awaits in the methods is forcing the App to continue running while it's awaiting
                    //  to continue within the method. It's possible that the called methods are running on a separate threads from the thread that is running this
                    //  method, so while those threads are put on hold, the App jumps back to this thread. After putting in while loops the app does seems to work like
                    //  I want it to so I'm probably on the right track, thought there must be a better way to accomplish as the current way looks ugly.

                    removeMatches();        // Remove the matches
                    while (!finishedRemoving)
                    {
                        await Task.Delay(50);
                    }
                    dropCandies();          // Move the candies down
                    while (!dropped)        // As long as the dropCandies method isn't finished it will keep adding an await
                    {
                        await Task.Delay(50);
                    }
                    fillUpColumns();        // Fill the grid back up
                    while (!filledAgain)
                    {
                        await Task.Delay(50);
                    }
                    detectPossibleSwap();   // Need to update the list of possible swaps
                    await Task.Delay(300);
                }
                while (deleteChains.Count != 0);
            }
            else
            {
                //  Swap is not possible so run the failed swap animation
                failedSwapAnimation(swap);
                //  Waiting to make sure the animation has been completed
                await Task.Delay(300);

            }
            //  Turn user interaction back on as all of the matches were removed and the grid filled back up
            enableListeners();
        }

        public void disableListeners()
        {
            PauseListeners();
        }

        public void enableListeners()
        {
            ResumeListeners();
        }

        //  Visually animates the swap using the CCMoveTo function provided by CocosSharp,
        //  also updates the grid location of the candies
        private void animateSwap(Swap swap)
        {
            const float timeToTake = 0.3f; // in seconds
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
        private async void failedSwapAnimation(Swap swap)
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

            //  Wait for the animation to complete before moving on
            await Task.Delay(300);
        }

        //  Method to find all chains in the grid 
        private void removeMatches()
        {
            List<Chain> horizontalChains = detectHorizontalMatches();
            List<Chain> verticalChains = detectVerticalMatches();

            // Logic to remove the candies from the grid goes here, possibly call a method that takes the list of chains to work with
            // Don't forget that candies have to be removed from the grid and then afterwards the sprites need to be removed from the screen separately
            // which can be handle by another method
            foreach (Chain item in verticalChains)
            {
                horizontalChains.Add(item);
            }
            deleteChains = horizontalChains;
            removeCandies(horizontalChains);
        }

        //  Remove the candy objects from the screen and the grid
        private async void removeCandies(List<Chain> chains)
        {
            if (finishedRemoving != false)
            {
                finishedRemoving = false;
            }

            foreach (Chain chain in chains)
            {
                foreach (candy candy in chain.candies)
                {
                    //  Remove the candy from the grid
                    grid[candy.getRow(), candy.getColumn()] = null;
                    CCSprite removeCandy = candy.getSprite();
                    if (removeCandy != null)
                    {
                        const float timeToTake = 0.3f; // in seconds
                        CCFiniteTimeAction coreAction = null;
                        CCAction easing = null;

                        coreAction = new CCScaleTo(timeToTake, 0.3f);
                        easing = new CCEaseOut(coreAction, 0.1f);
                        removeCandy.RunAction(coreAction);

                        await Task.Delay(50);   // Wait for the scaling animation to show a bit before continuing on to remove the candy
                        pointGone = false;
                        pointLabel(candy.getRow(), candy.getColumn());
                        while (!pointGone)
                        {
                            await Task.Delay(30);
                        }
                        removeCandy.RemoveFromParent(); // This should remove the candy from the screen
                        handlePoints();
                    }
                }
                //  Wait for all of the candies to be removed before moving on to the next chain in the list of chains
                await Task.Delay(300);
            }
            //  Since the method is finished removing all of chains, needed to set the finishedRemoving bool variable to true
            //  so the calling method can get out of it's await loop
            finishedRemoving = true;
        }

        private async void pointLabel(int row, int col)
        {
            var point = new CCLabel("+10", "Arial", 50, CCLabelFormat.SystemFont);
            point.Color = CCColor3B.Green;
            point.AnchorPoint = new CCPoint(0, 0);
            point.Position = new CCPoint(70 + (62 * col), 810 - (70 * row) + 50);
            AddChild(point);
            point.AddAction(new CCMoveTo(0.2f, new CCPoint(500, 1000)));
            await Task.Delay(200);
            point.RemoveFromParent();
            pointGone = true;
        }

        //  Drops the candies down 
        private async void dropCandies()
        {
            // Makes sure that dropped bool variable is set false before continuing
            if (dropped != false)
            {
                dropped = false;
            }
            for (int col = 0; col < gridColumns; col++)
            {
                for (int row = 8; row > 0; row--)
                {
                    candy Candy = candyAt(row, col);
                    if (Candy == null)
                    {
                        // Find which row number to drop the candy from
                        int tempRow = row - 1;
                        while (tempRow >= 0 && grid[tempRow, col] == null)
                        {
                            tempRow--;
                        }
                        //  Only runs if there's a row that has a candy in it
                        if (tempRow >= 0)
                        {
                            CCPoint position = new CCPoint(70 + (62 * col), 810 - (70 * row));
                            Candy = candyAt(tempRow, col);
                            Candy.AddAction(new CCEaseOut(new CCMoveTo(0.3f, position), 0.3f));
                            Candy.setPosition(row, col);    // Update the row and column of the candy
                            grid[row, col] = Candy;             // Update the position of the candy within the grid
                            grid[tempRow, col] = null;
                            //  Wait for the candy to drop before moving to on the next candy
                            await Task.Delay(50);
                        }
                    }
                }
            }

            // Since the method should have gone through the entire grid and finished dropping the candies
            // need to set dropped to true so the calling method can get out of the await loop
            dropped = true;
        }

        //  Fill the holes at the top of the of each column
        private void fillUpColumns()
        {
            int candyType = 0;
            if (filledAgain != false)
            {
                filledAgain = false;
            }
            for (int col = 0; col < gridColumns; col++)
            {
                //  Starting the top and working downwards, add a new candy where it's needed
                for (int row = 0; row < gridRows && grid[row, col] == null; row++)
                {
                    int newCandyType = 0;
                    //  Have to first create a new candy outside of the while loop or otherwise the IDE won't let me use the variable newCandy
                    //  as it will be seen as using an unassigned variable, even though it will be assigned a new candy in the while loop
                    candy newCandy = new candy(rand, row, col);
                    newCandyType = newCandy.getType();
                    //  Make sure that each candy that is being added isn't the same as the one that was added previously
                    while (newCandyType == candyType)
                    {
                        newCandy = new candy(rand, row, col);
                        newCandyType = newCandy.getType();
                    }
                    candyType = newCandyType;
                    grid[row, col] = newCandy;

                    // Once all of the candy is created to fill the grid back up
                    // Use an animation to add it to the screen
                    animateAddingNewCandies(row, col);
                }
            }
            //  Since the entire grid was filled back up with candies, need to set the filledAgain bool variable to true
            //  so the calling method can get out the await loop
            filledAgain = true;
        }

        private void handlePoints()
        {
            var points = Convert.ToInt32(scoreLabel.Text);
            points += 10;
            if (points < 100)
            {
                scoreLabel.Position = new CCPoint(480, 1000);
            }
            else if (points >= 100 && points < 1000)
            {
                scoreLabel.Position = new CCPoint(400, 1000);
            }
            else if (points >= 1000 && points < 10000)
            {
                scoreLabel.Position = new CCPoint(320, 1000);
            }
            scoreLabel.Text = Convert.ToString(points);
        }

        //  Using an animation to add all of the new candies to screen
        private async void animateAddingNewCandies(int row, int col)
        {
            // Starting position for the candy to be added
            CCPoint beginningPosition = new CCPoint(70 + (62 * col), 810 - (70 * row) + 50);
            // The final position of where the candy goes
            CCPoint endPosition = new CCPoint(70 + (62 * col), 810 - (70 * row));
            grid[row, col].Position = beginningPosition;
            AddChild(grid[row, col]);   // Add the candy to the screen
            // Animation to move the candy into it's proper position
            grid[row, col].AddAction(new CCMoveTo(0.3f, endPosition));
            // Wait for the animation before continuing
            await Task.Delay(300);
        }

        //  Detects any and all horizontal chains
        private List<Chain> detectHorizontalMatches()
        {
            var horzList = new List<Chain>();
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns - 2;)
                {
                    //  Makes sure that location in the grid isn't empty
                    if (grid[row, col] != null)
                    {
                        int matchType = grid[row, col].getType();
                        //  If the next two candies match than there's a chain here
                        if (grid[row, col + 1].getType() == matchType && grid[row, col + 2].getType() == matchType)
                        {
                            //  Create a new chain
                            var chain = new Chain();
                            chain.chainType = Chain.ChainType.Horizontal;
                            //  Add all of the candies within the chain to chain variable
                            do
                            {
                                chain.addCandy(candyAt(row, col));
                                col += 1;
                            }
                            while (col < gridColumns && grid[row, col].getType() == matchType);
                            horzList.Add(chain);    // Add the chain to the list of horizontal chains
                        }
                    }
                    col += 1;
                }
            }
            return horzList;
        }

        //  Detects any and all vertical chains
        private List<Chain> detectVerticalMatches()
        {
            var vertList = new List<Chain>();
            for (int col = 0; col < gridColumns; col++)
            {
                for (int row = 0; row < gridRows - 2;)
                {
                    //  Makes sure that the location in the grid isn't empty
                    if (grid[row, col] != null)
                    {
                        int matchType = grid[row, col].getType();
                        //  If the two candies below it matches, then there's a chain
                        if (grid[row + 1, col].getType() == matchType && grid[row + 2, col].getType() == matchType)
                        {
                            //  Create a new chain
                            var chain = new Chain();
                            chain.chainType = Chain.ChainType.Vertical;
                            //  Add all of the candies within the chain to the chain variable
                            do
                            {
                                chain.addCandy(candyAt(row, col));
                                row += 1;
                            }
                            while (row < gridRows && grid[row, col].getType() == matchType);
                            vertList.Add(chain);    // Add the chain to the list of vertical chains
                        }
                    }
                    row += 1;
                }
            }
            return vertList;
        }

        //  Get the row from the Y location
        private int convertYToRow(float y)
        {
            return (846 - Convert.ToInt32(y)) / 70;
        }

        //  Get the column from the X location
        private int convertXToColumn(float x)
        {
            return (Convert.ToInt32(x) - 38) / 62;
        }

        
    }
}