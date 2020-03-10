/*
 * TheNexusAvenger
 *
 * Displays help information.
 */

using System;
using System.Collections.Generic;
using NexusGit.NexusGit.Projects;

namespace NexusGit.ProgramActions
{
    /*
     * Class representing a help action.
     */
    public class Help : IProgramAction
    {
        /*
         * Returns if a command can be run for the given command.
         */
        public bool CanProgramRun(string command)
        {
            command = command.ToLower();
            return command == "help" || command == "h";
        }
        
        /*
         * Runs the command.
         */
        public void RunCommand(List<string> arguments)
        {
            Console.WriteLine("Nexus Git Version 0.2 Alpha");
            Console.WriteLine("Allows Git actions to be called from Roblox Studio.");
            Console.WriteLine("Nexus Development by TheNexusAvenger");
            Console.WriteLine("");
            Console.WriteLine("Projects supported: " + ProjectManager.GetSupportedProjects());
            Console.WriteLine("");
            Console.WriteLine("Commands:");
            Console.WriteLine("    help      Prints usage information.");
            Console.WriteLine("    detect    Detects the project that can be served in the current directory.");
            Console.WriteLine("    serve     Runs the local server for the Nexus Git Roblox Studio plugin.");
        }
    }
}