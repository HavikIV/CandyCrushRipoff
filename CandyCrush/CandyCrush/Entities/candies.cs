using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CocosSharp;

namespace CandyCrush.Entities
{
    // Class that creates one of several different candies,
    // it will contain private variables for it's Name, location, and whether or not it's a special candy
    public class candy : CCNode
    {
        private string candyName;
        private CCSprite candySprite;   // sprite for the candy
        private int row, column, candyType;    // variables to hold the location of the candy in the grid

        // As I've decided to added room for striped candies, I no longer need a bool variable to check if it's striped.
        // As I'm longer going to create an ImageView in this class, there's no need for variables for X and Y locations.
        // Can't create an ImageView in this class as it isn't the main class (I assume), probably for the best anyways.
        //ImageView candyImage = new ImageView(this); // this doesn't work, gives an error, this doesn't exist in current context

        // default constructor
        // generates a random candy
        public candy(Random r, int gRow, int gCol)
        {
            string[] candyTypes = { "Peppermint Swirl", "Blue Jolly Rancher", "Candy Corn", "Purple Nerd", "Green Elliptical" };
            candyType = generateCandyType(r);
            candyName = candyTypes[candyType - 1];
            row = gRow;
            column = gCol;
            switch (candyType - 1)
            {
                case 0: { candySprite = new CCSprite("PeppermintSwirl"); break; }
                case 1: { candySprite = new CCSprite("BlueJollyRancher"); break; }
                case 2: { candySprite = new CCSprite("CandyCorn"); break; }
                case 3: { candySprite = new CCSprite("PurpleNerds"); break; }
                case 4: { candySprite = new CCSprite("GreenElliptical"); break; }
                default: break;
            }

            CCLabel debugLabel = new CCLabel("[" + row +", " + column + "]", "Arial", 30, CCLabelFormat.SystemFont);
            debugLabel.Color = CCColor3B.Black;
            //debugLabel.Position = new CCPoint(10, 10);
            debugLabel.AnchorPoint = new CCPoint(0, 0);
            candySprite.AddChild(debugLabel);
            AddChild(candySprite);
        }

        //  Returns the candy's name
        public int getType()
        {
            return candyType;
        }

        public CCSprite getSprite()
        {
            return candySprite;
        }

        //  Returns which row the candy is in
        public int getRow()
        {
            return row;
        }
        
        //  Returns which column the candy is in
        public int getColumn()
        {
            return column;
        }
        public void setPosition(int ro, int col)
        {
            row = ro;
            column = col;
        }

        // Generate a random number within the range of 1-5
        // 1 -> Red, 2 -> Blue, 3 -> Yellow, 4 -> Purple, 5 -> Green
        public int generateCandyType(Random rand)
        {
            return rand.Next(1, 5); // returns a number between 1-5 to randomNumber
        }
    }
}