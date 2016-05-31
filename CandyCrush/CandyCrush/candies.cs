using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CandyCrush.candies
{
    // Class that creates one of several different candies,
    // it will contain private variables for it's Name, location, and whether or not it's a special candy
    public class candy
    {
        private string name; //, stripeDirection;
        //private int locationX, locationY;
        //private bool striped;

        // default constructor
        // generates a random candy
        public candy()
        {
            int candyNumber = generateCandy();
            if (candyNumber == 0)
            {
                name = "RED";
                //stripeDirection = "";
                //locationX = 0;
                //locationY = 0;
            }
            else if (candyNumber == 1)
            {
                name = "BLUE";
                //stripeDirection = "";
                //locationX = 0;
                //locationY = 0;
            }
            else if (candyNumber == 2)
            {
                name = "YELLOW";
                //stripeDirection = "";
                //locationX = 0;
                //locationY = 0;
            }
            else if (candyNumber == 3)
            {
                name = "PURPLE";
                //stripeDirection = "";
                //locationX = 0;
                //locationY = 0;
            }
            else if (candyNumber == 4)
            {
                name = "GREEN";
                //stripeDirection = "";
                //locationX = 0;
                //locationY = 0;
            }
        }

        // Generates a candy by generating a random number 
        public int generateCandy()
        {
            return generateNumber();
        }

        public string getName()
        {
            return name;
        }

        // Generate a random number within the range of 1-1000
        // will return a number that corresponds with a candy after the random number is evaluated
        // the first 500 numbers will used to candies of being red, blue, yellow, purple, and green
        // 1-100 -> Red, 101-200 -> Blue, 201-300 -> Yellow, 301-400 -> Purple, 401-500 -> Green
        // rest of the numbers will be used to generate special candies.
        // Maybe use a second number for special candies, in either case special candies should have
        // of being created.
        public int generateNumber()
        {
            Random rand = new Random(); // declares a random object
            int randomNumber; // variable to hold a random number
            randomNumber = rand.Next(1, 500); // assigns a number between 1-500 to randomNumber

            // determine what candy is generated
            if (randomNumber <= 100)
            {
                // Red candy is generated
                return 0;
            }
            else if (randomNumber > 100 && randomNumber <= 200)
            {
                // Blue candy is generated
                return 1;
            }
            else if (randomNumber > 200 && randomNumber <= 300)
            {
                // Yellow candy is generated
                return 2;
            }
            else if (randomNumber > 300 && randomNumber <= 400)
            {
                // Purple candy is generated
                return 3;
            }
            else if (randomNumber > 400)
            {
                // Green candy is generated
                return 4;
            }
            return 0; // returns a red candy by default
        }
    }
}