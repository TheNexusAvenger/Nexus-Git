/*
 * TheNexusAvenger
 * 
 * Performs adds on a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing an add action.
     */
    public class LocalAdd : RepositoryAction
    {
        private string Command;

        /*
         * Creates a remote pull action object.
         */
        public LocalAdd(Repository repository,FileList files) : base(repository)
        {
            this.Command = "add " + files.GetFilesAsString();
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
            // If nothing was specified, return that as a response.
            if (executableOutput.OutputContainsAtLine(0,"Nothing specified, nothing added."))
            {
                return GitResponse.CreateSingleLineResponse("Nothing specified.");
            }

            // If the file doesn't exist, return that as a response.
            if (executableOutput.OutputContainsAtLine(0,"did not match any files"))
            {
                return GitResponse.CreateSingleLineResponse("File not found.");
            }

            // Format and return the response.
            return GitResponse.CreateSingleLineResponse("Add complete.");
        }
    }
}