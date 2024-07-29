// Project Prologue
// Name: Carter Quesenberry
// Date: 7/30/2024
// Purpose: Lab #09 Disk File Line Count
// 
// I declare that the following code was written by me or provided 
// by the instructor for this project. I understand that copying source 
// code from any other source constitutes plagiarism, and that I will receive 
// a zero on this project if I am found in violation of this policy.

using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace LineCounterApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // event handler for the count lines button click:
        private async void OnCountLinesButtonClick(object sender, RoutedEventArgs e)
        {
            // get the file path from the textbox:
            var filePath = FilePathTextBox.Text;
            
            // check if the file path is valid and the file exists:
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                // print an error dialog if invalid:
                await ShowErrorDialog("Invalid file path.");
                return;
            }

            // initialize running and final count displays:
            RunningLineCountTextBlock.Text = "Running Count: 0";
            FinalLineCountTextBlock.Text = "Final Count: 0";

            // start the line counting process and update the UI with the final count:
            int lineCount = await CountLinesAsync(filePath);
            FinalLineCountTextBlock.Text = $"Final Count: {lineCount}";
        }

        // asynchronous function to count lines in a file:
        private async Task<int> CountLinesAsync(string filePath)
        {
            return await Task.Run(async () =>
            {
                // initialize line count:
                int lineCount = 0;
                // open the file for reading:
                using (var reader = new StreamReader(filePath))
                {
                    // read the lines until the end of the file:
                    while (reader.ReadLine() != null)
                    {
                        // increment the line count:
                        lineCount++;

                        // update the running line count on the UI thread:
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            RunningLineCountTextBlock.Text = $"Running Count: {lineCount}";
                        });

                        // add a 1 millisecond delay to make the count visible:
                        await Task.Delay(1);
                    }
                }
                // return the total line count:
                return lineCount;
            });
        }

        // function to show an error dialog with a custom message:
        private async Task ShowErrorDialog(string message)
        {
            var dialog = new Window
            {
                // set the title of the error dialog:
                Title = "Error",

                // display the error message:
                Content = new TextBlock { Text = message, Margin = new Avalonia.Thickness(20) },
                
                // set the width and height of the dialog:
                Width = 400,
                Height = 200
            };
            // show the error dialog as a modal window:
            await dialog.ShowDialog(this);
        }
    }
}
