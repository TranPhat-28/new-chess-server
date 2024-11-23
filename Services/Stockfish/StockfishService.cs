using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Services.Stockfish
{
    public class StockfishService : IStockfishService
    {
        private readonly Process process = new Process();
        public StockfishService()
        {
            process.StartInfo = new ProcessStartInfo
            {
                // -----------CHANGE FOR DEPLOYMENT----------------
                // Window Stockfish
                FileName = $@"{System.IO.Directory.GetCurrentDirectory().ToString()}\Stockfish\stockfish.exe",
                // Docker - Ubuntu Stockfish
                // FileName = @"/App/Stockfish/stockfish-ubuntu",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            // Start the process and listening to output
            process.Start();
            process.BeginOutputReadLine();
        }

        // Public interface method
        public async Task<string> NewGame()
        {
            ExecuteCommand("ucinewgame");
            ExecuteCommand("isready");

            string output = await ExpectOutput("readyok");
            Console.WriteLine("[EXPECT]: " + output);
            return output;
        }

        // Private method
        private void ExecuteCommand(string command)
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
        }

        private async Task<string> ExpectOutput(string expectedOutput)
        {
            var tcs = new TaskCompletionSource<string>();
            DataReceivedEventHandler handler = null;
            handler = (sender, e) =>
            {
                if (e.Data is not null && e.Data.Contains(expectedOutput))
                {
                    tcs.TrySetResult(e.Data); // Signal the waiting task
                    process.OutputDataReceived -= handler; // Unsubscribe from the event
                }
            };

            process.OutputDataReceived += handler;

            // Add a timeout to avoid indefinite waiting
            var timeoutTask = Task.Delay(5000);
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

            // Check if the timeout expired
            if (completedTask == timeoutTask)
            {
                process.OutputDataReceived -= handler; // Clean up
                throw new Exception($"[Stockfish] Expected output '{expectedOutput}' not received within 5000s.");
            }

            return await tcs.Task;
        }
    }
}