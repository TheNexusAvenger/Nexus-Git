/*
 * TheNexusAvenger
 *
 * Runs actions based on the command.
 */

using System;
using System.Collections.Generic;

namespace NexusGit.ProgramActions
{
    /*
     * Class representing an action runner.
     */
    public class ActionRunner
    {
        private List<IProgramAction> Actions;
        
        /*
         * Creates an action runner action.
         */
        public ActionRunner()
        {
            // Create the actions.
            this.Actions = new List<IProgramAction>();
            this.Actions.Add(new Help());
            this.Actions.Add(new Detect());
            this.Actions.Add(new Serve());
        }
        
        /*
         * Performs an action. Runs the help action if none can run.
         */
        public void PerformAction(List<string> arguments)
        {
            // Get the command to run.
            IProgramAction action = null;
            if (arguments.Count >= 1)
            {
                string command = arguments[0];
                foreach (IProgramAction newAction in this.Actions)
                {
                    if (newAction.CanProgramRun(command))
                    {
                        action = newAction;
                        break;
                    }
                }
            }
            
            // Run the action.
            if (action != null)
            {
                action.RunCommand(arguments);
            }
            else
            {
                // Determine the diagnostic message.
                if (arguments.Count >= 1)
                {
                    Console.WriteLine("Unknown command \"" + arguments[0] + "\"; defaulting to \"help\".");
                }
                else
                {
                    Console.WriteLine("No command specified; defaulting to \"help\".");
                }

                // Run the help command.
                new Help().RunCommand(arguments);
            }
        }
    }
}