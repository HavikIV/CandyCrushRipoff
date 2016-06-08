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
            return generateCandyType();
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

        // Generate a random number within the range of 1-5
        // 1 -> Red, 2 -> Blue, 3 -> Yellow, 4 -> Purple, 5 -> Green
        public int generateCandyType()
        {
            return rand.Next(1, 5); // returns a number between 1-5 to randomNumber
        }
    }
}