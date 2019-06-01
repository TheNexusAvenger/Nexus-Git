﻿/*
 * TheNexusAvenger
 * 
 * Performs remote pushes on a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a remote push action.
     */
    public class RemotePush : RepositoryAction
    {
        private string Command;

        /*
         * Creates a remote pull action object.
         */
        public RemotePush(Repository repository,string remoteRepository,string remoteBranch,bool force) : base(repository)
        {
            this.Command = "push " + remoteRepository + " " + remoteBranch;
            if (force)
            {
                this.Command += " --force";
            }
        }

        /*
         * Returns the command to run.
         */
        public override string GetCommand()
        {
            return this.Command;
        }

        /*
         * Performs the action and returns the response.
         */
        public override GitResponse CompleteAction(ExecutableOutput executableOutput)
        {
            // If everything is up to date, return that as a response.
            if (executableOutput.OutputContainsAtLine(0,"Everything up-to-date"))
            {
                return GitResponse.CreateSingleLineResponse("Nothing to push.");
            }

            // If the push was rejected, return that as a response.
            if (executableOutput.OutputContainsAtLine(1,"(fetch first)"))
            {
                return GitResponse.CreateSingleLineResponse("Push rejected. Fetch first.");
            }

            // Format the response.
            GitResponse newResponse = new GitResponse();
            if (executableOutput.OutputContainsAtLine(1,"(forced update)"))
            {
                newResponse.AddResponse("Force push complete.");
            }
            else if (executableOutput.OutputContainsAtLine(1,"->"))
            {
                newResponse.AddResponse("Push complete.");
            }

            // Return the response.
            return newResponse;
        }
    }
}