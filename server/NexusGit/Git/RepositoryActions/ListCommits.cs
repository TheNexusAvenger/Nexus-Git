/*
 * TheNexusAvenger
 * 
 * Gets the unpushed commits in a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a list commits action.
     */
    public class ListCommits : RepositoryAction
    {
        private string Command;

        /*
         * Creates a list commits action object.
         */
        public ListCommits(Repository repository,string remote,string branch) : base(repository)
        {
            this.Command = "log " + remote + "/" + branch + "..HEAD";
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
            // Create the response.
            var response = new GitResponse();
            
            // Add the remotes.
            foreach (var line in executableOutput)
            {
                if (line != "")
                {
                    response.AddResponse(line);
                }
            }

            // Return the response.
            return response;
        }
    }
}