/*
 * TheNexusAvenger
 * 
 * Gets the status of a repository.
 */

using NexusGit.FileIO;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a status action.
     */
    public class LocalStatus : RepositoryAction
    {
        private string Command;

        /*
         * Creates a remote pull action object.
         */
        public LocalStatus(Repository repository) : base(repository)
        {
            this.Command = "status";
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
            // Format the response.
            GitResponse newResponse = new GitResponse();
            bool writeUntrackedFiles = false;
            foreach (string line in executableOutput)
            {
                // Handle the current local branch.
                if (line.Length >= 11 && line.Substring(0, 9) == "On branch")
                {
                    newResponse.AddResponse("Current branch:");
                    newResponse.AddResponse(line.Substring(10));
                }

                // Handle the remote branch with no commits.
                if (line.Length >= 32 && line.Substring(0, 30) == "Your branch is up to date with")
                {
                    int remoteStart = line.IndexOf("'");
                    int remoteEnd = line.LastIndexOf("'");

                    newResponse.AddResponse("Remote branch:");
                    newResponse.AddResponse(line.Substring(remoteStart + 1, remoteEnd - remoteStart - 1));
                    newResponse.AddResponse("Ahead by:");
                    newResponse.AddResponse("0");
                }

                // Handle the remote branch with commits.
                if (line.Length >= 32 && line.Substring(0, 23) == "Your branch is ahead of")
                {
                    int remoteStart = line.IndexOf("'");
                    int remoteEnd = line.LastIndexOf("'");

                    newResponse.AddResponse("Remote branch:");
                    newResponse.AddResponse(line.Substring(remoteStart + 1, remoteEnd - remoteStart - 1));
                    newResponse.AddResponse("Ahead by:");
                    newResponse.AddResponse(line.Substring(remoteEnd + 5, line.LastIndexOf(" ") - remoteEnd - 5));
                }

                // Handle header.
                if (line.Contains("Changes to be committed:"))
                {
                    newResponse.AddResponse(line);
                }

                // Handle tracked file changes.
                if (line.Contains("renamed") || line.Contains("modified") || line.Contains("new file") || line.Contains("deleted"))
                {
                    string command = line.Substring(0, line.IndexOf(":"));
                    string file = line.Substring(line.IndexOf(":") + 1).Trim();
                    command = command.Substring(0, 1).ToUpper() + command.Substring(1);
                    newResponse.AddResponse(command + ": " + file);
                }

                // Handle starting untracked files.
                if (line.Contains("Untracked files:"))
                {
                    writeUntrackedFiles = true;
                }

                // Write untracked files (includes the header).
                if (writeUntrackedFiles && line != "" && !line.Contains("include in what will be committed") && !line.Contains("nothing added to commit but untracked files present"))
                {
                    newResponse.AddResponse(line);
                }
            }

            // Return the response.
            return newResponse;
        }
    }
}