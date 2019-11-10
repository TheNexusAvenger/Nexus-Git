/*
 * TheNexusAvenger
 * 
 * Performs remote pulls on a repository.
 */

using NexusGit.FileIO;
using System.Collections.Generic;

namespace NexusGit.Git.RepositoryActions
{
    /*
     * Class representing a remote pull action.
     */
    public class RemotePull : RepositoryAction
    {
        private string Command;

        /*
         * Creates a remote pull action object.
         */
        public RemotePull(Repository repository,string remote,string remoteBranch) : base(repository)
        {
            this.Command = "pull " + remote + " " + remoteBranch;
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
            // If the repository is already up to date, return that as a response.
            if (executableOutput.OutputContainsAtLine(0,"Already up to date."))
            {
                return GitResponse.CreateSingleLineResponse("Already up to date.");
            }

            // Get the modified, created, and deleted files.
            var modifiedFiles = new HashSet<string>();
            var createdFiles = new HashSet<string>();
            var deletedFiles = new HashSet<string>();
            foreach (string line in executableOutput)
            {
                // Add the modified file.
                var barIndex = line.LastIndexOf("| ");
                if (barIndex > 1)
                {
                    modifiedFiles.Add(line.Substring(0,barIndex).Trim());
                }

                // Add the created or deleted file.
                if (line.Length >= 12)
                {
                    // Determine the set to add to.
                    HashSet<string> listToAddTo = null;
                    if (line.Substring(0,12) == "create mode ")
                    {
                        listToAddTo = createdFiles;
                    }
                    else if (line.Substring(0,12) == "delete mode ")
                    {
                        listToAddTo = deletedFiles;
                    }

                    // Add the file.
                    if (listToAddTo != null)
                    {
                        var idSection = line.Substring(13);
                        var fileName = idSection.Substring(idSection.IndexOf(" ") + 1);
                        modifiedFiles.Remove(fileName);
                        listToAddTo.Add(fileName);
                    }
                }
            }

            // Format the response.
            var newResponse = new GitResponse();
            newResponse.AddResponse("Modified:");
            foreach (var fileName in modifiedFiles)
            {
                newResponse.AddResponse(fileName);
            }
            newResponse.AddResponse("Created:");
            foreach (var fileName in createdFiles)
            {
                newResponse.AddResponse(fileName);
            }
            newResponse.AddResponse("Deleted:");
            foreach (var fileName in deletedFiles)
            {
                newResponse.AddResponse(fileName);
            }

            // Return the response.
            return newResponse;
        }
    }
}