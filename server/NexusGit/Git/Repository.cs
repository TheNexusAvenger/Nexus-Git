/*
 * TheNexusAvenger
 * 
 * Acts as a proxy for remote Git repositories.
 */

using NexusGit.FileIO;
using NexusGit.Git.RepositoryActions;

namespace NexusGit.Git
{
    /*
     * Class representing a remote repository.
     */
    public class Repository
    {
        /*
         * Returns the Git repository directory location.
         */
        public static string GetGitDirectory()
        {
            return FileFinder.GetParentDirectoryOfDirectory(".git");
        }
        
        /*
         * Returns if a Git repository exists.
         */
        public static bool GitDirectoryExists()
        {
            return GetGitDirectory() != null;
        }
        
        /*
         * Returns the submodules of the repository.
         */
        public Submodules GetSubmodules()
        {
            return Submodules.FromDirectory(GetGitDirectory());
        }

        /*
         * Executes a Git command. Returns a list of the output. Returns
         * null if there is no repository set up.
         */
        public ExecutableOutput ExecuteCommand(string command)
        {
            // Return null if there is no Git repository.
            if (!GitDirectoryExists())
            {
                return null;
            }

            // Create and run the executable.
            var gitExutable = new Executable("git", command);
            gitExutable.SetWorkingDirectory(GetGitDirectory());
            gitExutable.Start();
            gitExutable.WaitForCompletion();

            // Return the output.
            return gitExutable.GetOutput();
        }

        /*
         * Pulls from the remote repository. Returns a response object.
         */
        public GitResponse RemotePull(string remote,string branch)
        {
            return new RemotePull(this,remote,branch).PerformAction();
        }

        /*
         * Pushes to the remote repository. Returns a response object.
         */
        public GitResponse RemotePush(string repository,string branch,bool force)
        {
            return new RemotePush(this,repository,branch,force).PerformAction();
        }

        /*
         * Adds a file to commit. Returns a response object.
         */
        public GitResponse Add(FileList file)
        {
            return new LocalAdd(this,file).PerformAction();
        }

        /*
         * Commits the added files. Returns a response object.
         */
        public GitResponse Commit(FileList files,string message)
        {
            return new LocalCommit(this,files,message).PerformAction();
        }

        /*
         * Checks the status. Returns a response object.
         */
        public GitResponse Status()
        {
            return new LocalStatus(this).PerformAction();
        }
        
        /*
         * Gets the local branches. Returns a response object.
         */
        public GitResponse LocalBranches()
        {
            return new LocalBranches(this).PerformAction();
        }
        
        /*
         * Gets the remote branches. Returns a response object.
         */
        public GitResponse RemoteBranches()
        {
            return new RemoteBranches(this).PerformAction();
        }
        
        /*
         * Checksout a local branch. Returns a response object.
         */
        public GitResponse LocalCheckout(string branch) {
            return new LocalCheckout(this,branch).PerformAction();
        }
        
        /*
         * Checksout a remote branch. Returns a response object.
         */
        public GitResponse RemoteCheckout(string localBranch,string remote,string branch) {
            return new RemoteCheckout(this,localBranch,remote,branch).PerformAction();
        }
        
        /*
         * Gets the remotes. Returns a response object.
         */
        public GitResponse ListRemotes()
        {
            return new ListRemotes(this).PerformAction();
        }
        
        /*
         * Gets the commits. Returns a response object.
         */
        public GitResponse ListCommits(string remote,string branch)
        {
            return new ListCommits(this,remote,branch).PerformAction();
        }
    }
}