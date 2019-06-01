/*
 * TheNexusAvenger
 *
 * Starts a server for a client.
 */

using System;
using System.Collections.Generic;
using NexusGit.NexusGit;

namespace NexusGit.ProgramActions
{
    /*
     * Class representing a serve action.
     */
    public class Serve : IProgramAction
    {
        /*
         * Returns if a command can be run for the given command.
         */
        public bool CanProgramRun(string command)
        {
            command = command.ToLower();
            return command == "serve" || command == "s";
        }
        
        /*
         * Runs the command.
         */
        public void RunCommand(List<string> arguments)
        {
            // Create the server.
            NexusGitServer server = NexusGitServer.GetServer();
            
            // Output an error if the server doesn't exist (no project found).
            if (server == null)
            {
                Console.WriteLine("Unable to start server: no project found.");
                return;
            }
            
            // Start the server.
            Console.WriteLine("Serving on port " + server.GetPort());
            server.Start();
        }
    }
}