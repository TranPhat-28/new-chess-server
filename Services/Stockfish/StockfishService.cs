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
        // private readonly ObservableCollection<string> output = new ObservableCollection<string>();
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

            ExpectOutput("Stockfish 16 by the Stockfish developers", "Stockfish started");

            // Start the process and listening to output
            process.Start();
            process.BeginOutputReadLine();
        }

        private void ExecuteCommand(string command)
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
        }

        public void NewGame()
        {
            ExecuteCommand("ucinewgame");
            ExecuteCommand("isready");

            ExpectOutput("readyok", "Stockfish is ready");
        }

        private void ExpectOutput(string expectedOutput, string? logValue)
        {
            DataReceivedEventHandler handler = null;
            handler = new DataReceivedEventHandler((sender, e) =>
            {
                if (e.Data is not null && e.Data.Trim().Contains(expectedOutput))
                {
                    if (logValue is not null)
                    {
                        Console.WriteLine(logValue);
                    }
                    process.OutputDataReceived -= handler;
                }
            });

            process.OutputDataReceived += handler;
        }
    }
}