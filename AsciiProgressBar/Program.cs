using System;
using System.Text;
using System.Threading;

namespace AsciiProgressBar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int totalFiles = 1402;
            int cursorTop = Console.CursorTop; // Save the initial cursor position
            int progressBarLine = cursorTop; // This will keep track of the progress bar line

            for (int i = 0; i <= totalFiles; i++)
            {
                // Set the cursor position to the saved position
                Console.SetCursorPosition(0, progressBarLine);

                WriteProgressBar(i, totalFiles, progressBarLine);

                // Prevent console from writing on the last line
                if (Console.CursorTop == Console.WindowTop + Console.WindowHeight - 1)
                {
                    Console.SetCursorPosition(0, Console.WindowTop + Console.WindowHeight - 2);
                }

                Console.WriteLine("Processing file {0} of {1}", i, totalFiles);

                // Save the current position after writing the additional information
                if (i == 0) // Do this only once
                {
                    cursorTop = Console.CursorTop;
                }

                Thread.Sleep(20); // simulate some work
            }

            Console.SetCursorPosition(0, cursorTop); // Adjust the final position
            Console.WriteLine("Process completed."); // Final message after completion
        }

        static void WriteProgressBar(int progress, int total, int progressBarLine)
        {
            // Calculate width for progress bar and file count display
            string fileCountDisplay = String.Format("[{0}/{1}]", progress, total);
            int fileCountDisplayWidth = fileCountDisplay.Length;
            int totalWidth = Console.WindowWidth - 10 - fileCountDisplayWidth; // Adjusted for padding and file count display

            double ratio = (double)progress / total; // Progress ratio between 0 and 1
            int filledWidth = (int)(ratio * totalWidth); // Number of characters to fill

            // Shades of green for the progress bar
            string[] greenShades = new string[]
            {
                "\u001b[48;5;22m", // Dark green
                "\u001b[48;5;28m", // Medium green
                "\u001b[48;5;34m", // Light green
                "\u001b[48;5;40m", // Lighter green
                "\u001b[48;5;46m", // Bright green
            };

            string colorReset = "\u001b[0m"; // Reset to default colors

            // Build the gradient bar
            var progressBar = new StringBuilder();
            for (int j = 0; j < totalWidth; j++)
            {
                if (j < filledWidth)
                {
                    // Determine the shade of green based on the position
                    int shadeIndex = (int)((double)j / filledWidth * (greenShades.Length - 1));
                    progressBar.Append(greenShades[shadeIndex]);
                    progressBar.Append(' '); // Append space for filled portion
                }
                else
                {
                    progressBar.Append(colorReset); // Reset color for unfilled portion
                    progressBar.Append(' '); // Append space for unfilled portion
                }
            }
            progressBar.Append(colorReset); // Reset color at the end of the bar

            // Print the progress bar with the rounded percentage and file count
            Console.SetCursorPosition(0, progressBarLine); // Ensure we're on the correct line
            Console.Write("[{0}] {1:F2}% {2}", progressBar.ToString(), ratio * 100, fileCountDisplay);
        }
    }
}
