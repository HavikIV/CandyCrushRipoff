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
        private string name, stripeDirection;
        private Random rand;
        // As I've decided to added room for striped candies, I no longer need a bool variable to check if it's striped.
        // As I'm longer going to create an ImageView in this class, there's no need for variables for X and Y locations.
        // Can't create an ImageView in this class as it isn't the main class (I assume), probably for the best anyways.
        //ImageView candyImage = new ImageView(this); // this doesn't work, gives an error, this doesn't exist in current context

        // default constructor
        // generates a random candy
        public candy(Random r)
        {
            rand = r;
            int candyNumber = generateCandy(); // gets the candy number
            stripeDirection = "None"; // by default the candy has no stripeDirection, only if it's a special candy
            //  determine what candy it is based on the candyNumber and assign it's name
            if (candyNumber == 1)
            {
                name = "Peppermint Swirl";

            }
            else if (candyNumber == 2)
            {
                name = "Blue Jolly Rancher";

            }
            else if (candyNumber == 3)
            {
                name = "Candy Corn";
            }
            else if (candyNumber == 4)
            {
                name = "Purple Nerd";
            }
            else if (candyNumber == 5)
            {
                name = "Green Elliptical";
            }
            else if (candyNumber == 6)
            {
                name = "Striped Peppermint Swirl";
            }
            else if (candyNumber == 7)
            {
                name = "Striped Blue Jolly Rancher";
            }
            else if (candyNumber == 8)
            {
                name = "Striped Candy Corn";
            }
            else if (candyNumber == 9)
            {
                name = "Striped Purple Nerd";
            }
            else if (candyNumber == 10)
            {
                name = "Striped Green Elliptical";
            }

            //  assign stripeDirection
            if (candyNumber > 5 && candyNumber <= 10)
            {
                stripeDirection = generateDirecction();
            }
        }

        // Generates a candy by generating a random number 
        public int generateCandy()
        {
            return generateNumber();
        }

        //  Returns the candy's name
        public string getName()
        {
            return name;
        }

        //  Returns the candy's stripe direction
        public string getDirection()
        {
            return stripeDirection;
        }

        //  returns a direction for stripes on the candy
        public string generateDirecction()
        {
            Random randDirection = new Random(); // declares a random object
            int randNumber = randDirection.Next(1, 2); // assigns either 1 or 2 to randNumber
           if (randNumber == 1)
            {
                return "Horizontal";
            }
           else
            {
                return "Vertical";
            }
        }

        // Should have a way to create a special candy for when one of several certain combinations are met
        // create the function here
        public void generateSpecial(string candy, string direction)
        {
            name = candy;
            stripeDirection = direction;
        }

        // Generate a random number within the range of 1-1000
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
            randomNumber = rand.Next(1, 750); // assigns a number between 1-500 to randomNumber

            // determine what candy is generated
            if (randomNumber <= 150)
            {
                // Red candy is generated
                return 1;
            }
            else if (randomNumber > 151 && randomNumber <= 300)
            {
                // Blue candy is generated
                return 2;
            }
            else if (randomNumber > 301 && randomNumber <= 450)
            {
                // Yellow candy is generated
                return 3;
            }
            else if (randomNumber > 450 && randomNumber <= 600)
            {
                // Purple candy is generated
                return 4;
            }
            else if (randomNumber > 600 && randomNumber <= 750)
            {
                // Green candy is generated
                return 5;
            }
            else if (randomNumber > 751 && randomNumber <= 780)
            {
                // striped red candy
                return 6;
            }
            else if (randomNumber > 781 && randomNumber <= 810)
            {
                // striped blue candy
                return 7;
            }
            else if (randomNumber > 811 && randomNumber <= 840)
            {
                // striped yellow candy
                return 8;
            }
            else if (randomNumber > 841 && randomNumber <= 870)
            {
                // striped purple candy
                return 9;
            }
            else if (randomNumber > 871 && randomNumber <= 900)
            {
                // striped green candy
                return 10;
            }
            // add the other candies here
            // the last 10% of the numbers should be used to create other special candies
            return 1; // returns a red candy by default
        }
    }
}