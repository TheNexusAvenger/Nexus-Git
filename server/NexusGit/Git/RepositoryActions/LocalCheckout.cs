/*
 * TheNexusAvenger
 * 
 * Performs local checkouts on a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing an add action.
     */
    public class LocalCheckout : RepositoryAction
    {
        private string Command;

        /*
         * Creates a local checkout action object.
         */
        public LocalCheckout(Repository repository,string branch) : base(repository)
        {
            this.Command = "checkout " + branch;
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
            // Format and return the response.
            return GitResponse.CreateSingleLineResponse("Checkout complete.");
        }
    }
}