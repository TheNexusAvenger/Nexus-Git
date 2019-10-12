/*
 * TheNexusAvenger
 * 
 * Performs remote checkouts on a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing an add action.
     */
    public class RemoteCheckout : RepositoryAction
    {
        private string Command;

        /*
         * Creates a remote checkout action object.
         */
        public RemoteCheckout(Repository repository,string localBranch,string remote,string branch) : base(repository)
        {
            this.GetRepository().ExecuteCommand("fetch");
            this.Command = "checkout -b " + localBranch + " " + remote + "/" + branch;
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