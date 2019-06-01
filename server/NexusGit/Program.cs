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
            List<string> arguments = new List<string>(args);
            
            // Run the command.
            ActionRunner actionRunner = new ActionRunner();
            actionRunner.PerformAction(arguments);
        }
    }
}