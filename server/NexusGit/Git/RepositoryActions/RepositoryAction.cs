/*
 * TheNexusAvenger
 *
 * Abstract class for handling repository actions.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class for handling repository actions.
     */
    public abstract class RepositoryAction
    {
        private Repository GitRepository;

        /*
         * Creates the repository action.
         */
        public RepositoryAction(Repository repository)
        {
            this.GitRepository = repository;
        }
        
        /*
         * Returns the repository used.
         */
        public Repository GetRepository()
        {
            return this.GitRepository;
        }

        /*
         * Creates a common response if one exists.
         */
        private GitResponse GetCommonResponse(ExecutableOutput response)
        {
            // Return an error about no repository if none exists.
            if (response == null)
            {
                return GitResponse.CreateSingleLineResponse("No .git folder.");
            }
            
            // Return an error for the login being cancelled.
            if (response.OutputContainsAtLine(1,"Logon failed"))
            {
                return GitResponse.CreateSingleLineResponse("Login failed.");
            }

            // Return an error about the remote repository not being found. Note
            // that this may be due to the link being incorrect or insufficient permissions.
            if (response.OutputContainsAtLine(1,"remote: Repository not found"))
            {
                return GitResponse.CreateSingleLineResponse("Remote repository not found.");
            }

            // Return an SSH key error if it exists.
            if (response.OutputContainsAtLine(1,": Permission denied"))
            {
                return GitResponse.CreateSingleLineResponse("SSH key password required.");
            }

            // Return null (no common response).
            return null;
        }

        /*
         * Performs the action and returns the response.
         */
        public GitResponse PerformAction()
        {
            // Get the command to run.
            var command = this.GetCommand();

            // Perform the command.
            var executableOutput = this.GitRepository.ExecuteCommand(command);

            // If a common response exists, return the common response.
            var commonResponse = this.GetCommonResponse(executableOutput);
            if (commonResponse != null)
            {
                return commonResponse;
            }

            // Return the main response.
            return this.CompleteAction(executableOutput);
        }

        /*
         * Returns the command to run.
         */
        public abstract string GetCommand();

        /*
         * Performs the action and returns the response.
         */
        public abstract GitResponse CompleteAction(ExecutableOutput executableOutput);
    }
}