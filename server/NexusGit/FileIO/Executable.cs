/*
 * TheNexusAvenger
 * 
 * Executes commands without creating windows.
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace NexusGit.FileIO
{
    /*
     * Class representing an executable's output.
     */
    public class ExecutableOutput : List<string>
    {
        /*
         * Returns if the output contains a string.
         */
        public bool OutputContains(string contents)
        {
            // Return true if a line contains the contents.
            foreach (var line in this) {
                if (line.Contains(contents))
                {
                    return true;
                }
            }

            // Return false (none found).
            return false;
        }

        /*
         * Returns if the output at a given line contains a string.
         */
        public bool OutputContainsAtLine(int lineNumber,string contents)
        {
            // Return false if the line doesn't exist.
            if (lineNumber < 0 || this.Count - 1 < lineNumber)
            {
                return false;
            }

            // Return if the line contains the string.
            return this[lineNumber].Contains(contents);
        }
    }

    /*
     * Class representing an executable.
     */
    public class Executable
    {
        private Process Process;
        private StringBuilder Output;
        private StringBuilder Error;

        /*
         * Creates a process object.
         */
        public Executable(string executable,string arguments)
        {
            // Create the process.
            this.Process = new Process();
            this.Process.StartInfo.UseShellExecute = false;
            this.Process.StartInfo.FileName = executable;
            this.Process.StartInfo.Arguments = arguments;
            this.Process.StartInfo.RedirectStandardOutput = true;
            this.Process.StartInfo.RedirectStandardError = true;
            this.Process.StartInfo.CreateNoWindow = true;
            
            // Create the output and error logging.
            this.Output = new StringBuilder();
            this.Error = new StringBuilder();

            using (var outputWaitHandle = new AutoResetEvent(false))
            {
                using (var errorWaitHandle = new AutoResetEvent(false))
                {
                    // Add output logging.
                    this.Process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            if (!this.Process.HasExited)
                            {
                                outputWaitHandle.Set();
                            }
                        }
                        else
                        {
                            this.Output.AppendLine(e.Data);
                        }
                    };

                    // Add error logging.
                    this.Process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            if (!this.Process.HasExited)
                            {
                                errorWaitHandle.Set();
                            }
                        }
                        else
                        {
                            this.Error.AppendLine(e.Data);
                        }
                    };
                }
            }
        }
        
        /*
         * Sets the working directory of the executable.
         */
        public void SetWorkingDirectory(string workingDirectory)
        {
            this.Process.StartInfo.WorkingDirectory = workingDirectory;
        }

        /*
         * Starts the executable.
         */
        public void Start()
        {
            this.Process.Start();
            this.Process.BeginOutputReadLine();
            this.Process.BeginErrorReadLine();
        }

        /*
         * Waits for the executable to complete.
         */
        public void WaitForCompletion()
        {
            this.Process.WaitForExit();
        }

        /*
         * Returns the output of the executable.
         */
        public ExecutableOutput GetOutput()
        {
            // Store the output.
            var output = new ExecutableOutput();
            foreach (var line in this.Output.ToString().Split('\n'))
            {
                output.Add(line.Trim());
            }
            
            foreach (var line in this.Error.ToString().Split('\n'))
            {
                output.Add(line.Trim());
            }

            // Return the output.
            return output;
        }
    }
}