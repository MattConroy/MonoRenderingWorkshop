using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MonoRenderingWorkshop
{
    internal class ShaderManager : ContentManager
    {
        private const string ContentBuilderPath = @"C:\Program Files (x86)\MSBuild\MonoGame\v3.0\Tools\MGCB.exe";
        private const string RecompileShadersCommand = "recompileShaders.mgcb";

        private readonly string _artifactDirectory;
        private readonly string _sourceDirectory;
        private readonly Action<GameTime> _onShadersReloaded;

        public ShaderManager(ContentManager mainContentManager, Action<GameTime> onShadersReloaded) :
            base(mainContentManager?.ServiceProvider,
                mainContentManager?.RootDirectory)
        {
            _artifactDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
            _sourceDirectory = Path.GetFullPath(Path.Combine(_artifactDirectory, @"..\..\..\..\..\Content"));
            _onShadersReloaded = onShadersReloaded;
        }

        public void Reload(GameTime time)
        {
            if (RecompileShaders())
            {
                Unload();
                _onShadersReloaded(time);
            }
        }

        private bool RecompileShaders()
        {
            var process = StartProcess(_sourceDirectory, ContentBuilderPath,
                $"/@:{RecompileShadersCommand} /outputDir:\"{_artifactDirectory}\"");

            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) =>
                stdOutput.AppendLine(args.Data);

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                var standardOutput = stdOutput.ToString();
                var errorOutput = process.StandardError.ReadToEnd();
                LogProcessOutput("Errors", errorOutput);
                LogProcessOutput("Output", standardOutput);
                return string.IsNullOrWhiteSpace(errorOutput) &&
                    !standardOutput.Contains("error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Shader recompile failed.\n{ex}");
                return false;
            }
        }

        private static void LogProcessOutput(string type, string output)
        {
            if (string.IsNullOrWhiteSpace(output))
                return;

            Console.WriteLine($"Received {type}:\n{output}");
        }

        private static Process StartProcess(string workingDirectory, string fileName, string arguments) =>
            new Process
            {
                StartInfo =
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };
    }
}