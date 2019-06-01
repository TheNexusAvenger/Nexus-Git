/*
 * TheNexusAvenger
 *
 * Base interface for a program action.
 */

using System.Collections.Generic;

namespace NexusGit.ProgramActions
{
    /*
     * Interface for a program action.
     */
    public interface IProgramAction
    {
        /*
         * Returns if a command can be run for the given command.
         */
        bool CanProgramRun(string command);
        
        /*
         * Runs the command.
         */
        void RunCommand(List<string> arguments);
    }
}