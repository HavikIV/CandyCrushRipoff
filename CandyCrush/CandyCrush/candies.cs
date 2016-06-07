using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CandyCrush
{
    // Class that creates one of several different candies,
    // it will contain private variables for it's Name, location, and whether or not it's a special candy
    public class candy
    {
        private string candyType, stripeDirection;
        private Random rand;
        // As I've decided to added room for striped candies, I no longer need a bool variable to check if it's striped.
        // As I'm longer going to create an ImageView in this class, there's no need for variables for X and Y locations.
        // Can't create an ImageView in this class as it isn't the main class (I assume), probably for the best anyways.
        //ImageView candyImage = new ImageView(this); // this doesn't work, gives an error, this doesn't exist in current context

        // default constructor
        // generates a random candy
        public candy(Random r, string sDirection)
        {
            rand = r;
            //string[] candyTypes = { "Peppermint Swirl", "Blue Jolly Rancher", "Candy Corn", "Purple Nerd", "Green Elliptical" };
            string[] candyTypes = { "red", "blue", "yellow", "purple", "green" };
            int candyNumber = generateCandy(); // gets the candy number
            stripeDirection = sDirection; // by default the candy has no stripeDirection, only if it's a special candy
            //  assign what candy it is based on the candyNumber
            candyType = candyTypes[candyNumber];
            stripeDirection = sDirection;   // Assigns the direction of the stripes
        }

        // Generates a candy by generating a random number 
        public int generateCandy()
        {
            return generateNumber();
        }

        //  Returns the candy's name
        public string getType()
        {
            return candyType;
        }

        //  Returns the candy's stripe direction
        public string getDirection()
        {
            return stripeDirection;
        }

        // Should have a way to create a special candy for when one of several certain combinations are met
        // create the function here
        public void generateSpecial(string candy, string direction)
        {
            candyType = candy;
            stripeDirection = direction;
        }

        // Generate a random number within the range of 1-750
        // will return a number that corresponds with a candy after the random number is evaluated
        // the first 500 numbers will used to candies of being red, blue, yellow, purple, and green
        // 1-150 -> Red, 151-300 -> Blue, 301-450 -> Yellow, 451-600 -> Purple, 601-750 -> Green
        // rest of the numbers will be used to generate special candies.
        // Maybe use a second number for special candies, in either case special candies should have
        // of being created.
        public int generateNumber()
        {
            //Random rand = new Random(); // declares a random object
            int randomNumber; // variable to hold a random number
            randomNumber = rand.Next(1, 750); // assigns a number between 1-750 to randomNumber

            // determine what candy is generated
            if (randomNumber <= 150)
            {
                // Red candy is generated
                return 0;
            }
            else if (randomNumber > 151 && randomNumber <= 300)
            {
                // Blue candy is generated
                return 1;
            }
            else if (randomNumber > 301 && randomNumber <= 450)
            {
                // Yellow candy is generated
                return 2;
            }
            else if (randomNumber > 450 && randomNumber <= 600)
            {
                // Purple candy is generated
                return 3;
            }
            else if (randomNumber > 600 && randomNumber <= 750)
            {
                // Green candy is generated
                return 4;
            }
            return 0; // returns a red candy by default
        }
    }
}