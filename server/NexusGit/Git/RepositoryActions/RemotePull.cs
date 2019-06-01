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
            HashSet<string> modifiedFiles = new HashSet<string>();
            HashSet<string> createdFiles = new HashSet<string>();
            HashSet<string> deletedFiles = new HashSet<string>();
            foreach (string line in executableOutput)
            {
                // Add the modified file.
                int barIndex = line.LastIndexOf("| ");
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
                        string idSection = line.Substring(13);
                        string fileName = idSection.Substring(idSection.IndexOf(" ") + 1);
                        modifiedFiles.Remove(fileName);
                        listToAddTo.Add(fileName);
                    }
                }
            }

            // Format the response.
            GitResponse newResponse = new GitResponse();
            newResponse.AddResponse("Modified:");
            foreach (string fileName in modifiedFiles)
            {
                newResponse.AddResponse(fileName);
            }
            newResponse.AddResponse("Created:");
            foreach (string fileName in createdFiles)
            {
                newResponse.AddResponse(fileName);
            }
            newResponse.AddResponse("Deleted:");
            foreach (string fileName in deletedFiles)
            {
                newResponse.AddResponse(fileName);
            }

            // Return the response.
            return newResponse;
        }
    }
}