using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anderson.PackageAudit.Tests
{
    public class AzureFunctionHost : IDisposable
    {
        private Process _process;
        private readonly int _port;
        private readonly string _workingDirectory;
        readonly ManualResetEvent _manualReset = new ManualResetEvent(false);

        public AzureFunctionHost(string workingDirectory, int port)
        {
            _workingDirectory = workingDirectory;
            _port = port;
        }

        public async Task StartAsync(Dictionary<string, string> environmentVariables)
        {
            await Task.Yield();

            if (_process == null || _process.HasExited)
            {
                (string filename, string args) = GetCommandLineOptions();

                var processStartInfo = CreateProcessStartInfo(environmentVariables, filename, args);

                _process = new Process
                {
                    StartInfo = processStartInfo,
                };
                _process.OutputDataReceived += Process_OutputDataReceived;
                _process.ErrorDataReceived += Process_OutputDataReceived;

                CancellationTokenSource source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                await Task.WhenAny(
                    WaitForHostReady(), 
                    WaitForTimeout(source));
            }
        }

        private static async Task WaitForTimeout(CancellationTokenSource source)
        {
            while (!source.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), source.Token);
            }
        }

        private async Task WaitForHostReady()
        {
            await Task.Yield();

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _manualReset.WaitOne();
        }

        private (string filename, string args) GetCommandLineOptions()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    return ("sh", $"-c \"func host start --port {_port}\"");
                case PlatformID.Win32NT:
                    return ("cmd.exe", $"/k func host start --port {_port}");
                default:
                    throw new InvalidOperationException("Unsupported operating system");
            }
        }

        private void Process_OutputDataReceived(
            object sender,
            DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data) &&
                e.Data.Contains($"Listening on http://0.0.0.0:{_port}/"))
            {
                _manualReset.Set();
            }
        }

        private ProcessStartInfo CreateProcessStartInfo(
            Dictionary<string, string> environmentVariables,
            string filename,
            string args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = args,
                WorkingDirectory = _workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            foreach (var kvp in environmentVariables)
            {
                processStartInfo.EnvironmentVariables[kvp.Key] = kvp.Value;
            }

            return processStartInfo;
        }

        public void Dispose()
        {
            var ps = Process.GetProcesses();
            ps.Where(x => x.ProcessName == "func").ToList().ForEach(proc =>
            {
                proc.Kill();
            });
            _manualReset?.Dispose();
            _process.Kill();

        }
    }
}
