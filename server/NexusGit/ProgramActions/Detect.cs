/*
 * TheNexusAvenger
 *
 * Detects the project for the current directory.
 */

using System;
using System.Collections.Generic;
using NexusGit.NexusGit.Projects;

namespace NexusGit.ProgramActions
{
    /*
     * Class representing a serve action.
     */
    public class Detect : IProgramAction
    {
        /*
         * Returns if a command can be run for the given command.
         */
        public bool CanProgramRun(string command)
        {
            command = command.ToLower();
            return command == "detect" || command == "d";
        }
        
        /*
         * Runs the command.
         */
        public void RunCommand(List<string> arguments)
        {
            // Create a project manager.
            ProjectManager projectManager = new ProjectManager();
            
            // Detect the project and output the project.
            IProject project = projectManager.GetProject();
            if (project == null)
            {
                Console.WriteLine("No project detected.");
            } else
            {
                Console.WriteLine("Project detected: " + project.GetName());
            }
        }
    }
}