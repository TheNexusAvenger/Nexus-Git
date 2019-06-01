/*
 * TheNexusAvenger
 * 
 * Performs commits on a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a commit action.
     */
    public class LocalCommit : RepositoryAction
    {
        private string Command;

        /*
         * Creates a remote pull action object.
         */
        public LocalCommit(Repository repository,FileList files,string message) : base(repository)
        {
            this.Command = "commit " + files.GetFilesAsString() + "-m \"" + message.Replace("\"","\\\"") + "\"";
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
            // If there is nothing to commit, return that as an error.
            if (executableOutput.OutputContains("nothing to commit, working tree clean"))
            {
                return GitResponse.CreateSingleLineResponse("Nothing to commit.");
            }
            if (executableOutput.OutputContains("nothing added to commit but untracked files present") || executableOutput.OutputContains("no changes added to commit"))
            {
                return GitResponse.CreateSingleLineResponse("Nothing to commit. Add first.");
            }

            // Return a success message.
            return GitResponse.CreateSingleLineResponse("Commit successful.");
        }
    }
}