using System;
using System.Drawing;
using System.Threading;
using WootingNet;

namespace WootingTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Generate a random number
            Random rnd = new Random();

            // Have a manual reset event for resetting the keyboard on exit
            ManualResetEvent syncEvent = new ManualResetEvent(false);

            // Activate the main loop
            bool active = true;

            // Colorchanger loop
            bool Running = true;

            // Check if the keyboard is connected
            if (!RGB.IsConnected() || !Analog.IsConnected())
            {
                Console.WriteLine("Keyboard not connected");
                Environment.Exit(1);
            }

            // Start listening for analog values
            Analog.Start();

            // Check for updates from Analog Key Values
            Analog.OnAnalogUpdate += (Key key, float value) =>
            {

                // If we completely depressed the ESC key, exit the program
                if (key == Key.ESC && value == 1)
                {
                    active = false;
                }

                // Print out the value
                Console.WriteLine($"{key}: {value}");
            };

            // Start the color changer thread
            ThreadPool.QueueUserWorkItem(x =>
            {
                // Have a reference of the color
                Color color;

                // Color looper
                while (Running)
                {
                    // Grab a random key
                    var row = rnd.Next(0, 6);
                    var col = rnd.Next(0, 21);

                    // Check if the key is valid, if not, restart
                    if (!Keys.IsValid(row, col))
                    {
                        continue;
                    }

                    // Get a random color
                    color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));

                    // Set the color
                    RGB.SetKey(row, col, color);

                    // Wait a few ms before updating another random key with random color
                    Thread.Sleep(10);
                }

                // When we exit the color loop, reset all keys
                for (int row = 0; row <= 5; row++)
                {
                    for (int col = 0; col <= 20; col++)
                    {
                        RGB.ResetKey(row, col);
                    }
                }

                // Reset the keyboard
                RGB.Reset();

                // Tell the main thread we're done.
                syncEvent.Set();
            });

            // Main thread
            while (active)
            {
                Thread.Sleep(400);
            }

            // When done, tell the color thread to stop
            Running = false;

            // Wait for the color thread to finish up
            syncEvent.WaitOne();
        }
    }
}
