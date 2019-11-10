/*
 * TheNexusAvenger
 * 
 * Runs the Nexus Git application.
 */

using System.Collections.Generic;
using NexusGit.ProgramActions;

namespace NexusGit
{
    /*
     * Class representing a program.
     */
    public class Program
    {
        /*
         * Runs the program.
         */
        public static void Main(string[] args)
        {
            // Convert the arguments to a list.
            var arguments = new List<string>(args);
            
            // Run the command.
            var actionRunner = new ActionRunner();
            actionRunner.PerformAction(arguments);
        }
    }
}