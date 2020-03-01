using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoRenderingWorkshop.Input;
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

        public event Action<GameTime> ShadersReloaded;

        private readonly KeyboardController _keyboard;

        private readonly string _artifactDirectory;
        private readonly string _sourceDirectory;

        public ShaderManager(ContentManager mainContentManager, KeyboardController keyboard) :
            base(mainContentManager?.ServiceProvider,
                mainContentManager?.RootDirectory)
        {
            _artifactDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
            _sourceDirectory = Path.GetFullPath(Path.Combine(_artifactDirectory, @"..\..\..\..\..\Content"));
            _keyboard = keyboard;
        }

        public void Load() => Reload(new GameTime());

        public void Update(GameTime time)
        {
            if (_keyboard.WasPressed(Keys.F5))
                Reload(time);
        }

        private void Reload(GameTime time)
        {
            if (RecompileShaders())
            {
                Unload();
                OnShadersReloaded(time);
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

        private void OnShadersReloaded(GameTime time)
        {
            ShadersReloaded?.Invoke(time);
        }
    }
}