/*
 * TheNexusAvenger
 *
 * Manages projects for the working directory.
 */

using System.Collections.Generic;
using System.IO;
using NexusGit.FileIO;
using NexusGit.NexusGit.Projects.SupportedProjects;

namespace NexusGit.NexusGit.Projects
{
    /*
     * Class representing a project manager.
     */
    public class ProjectManager
    {
        private List<IProject> Projects;
       
        /*
         * Creates a project manager object.
         */
        public ProjectManager()
        {
            // Create the projects.
            this.Projects = new List<IProject>();
            // this.Projects.Add(new Rojo()); // TODO: Add support for Rojo 0.5 Alpha and newer
            this.Projects.Add(new Rojo04());
        }
        
        /*
         * Returns the current project.
         */
        public IProject GetProject()
        {
            // Get the project to use.
            IProject currentProject = null;
            string currentDirectory = Directory.GetCurrentDirectory();
            while (currentDirectory != null)
            {
                // Iterate through the projects and determine if the project is valid.
                foreach (IProject project in this.Projects)
                {
                    // If the project is valid, set it as the current and break the loop.
                    if (project.IsDirectoryValid(currentDirectory))
                    {
                        currentProject = project;
                        break;
                    }
                }
             
                // Break the loop if the project exists.
                if (currentProject != null)
                {
                    break;
                }
                
                // Move up the directory.
                currentDirectory = FileFinder.MoveDirectoryUp(currentDirectory);
            }
            
            // Return the current project.
            return currentProject;
        }
        
        /*
         * Returns the support projected.
         */
        public static string GetSupportedProjects()
        {
            // Create a project manager.
            ProjectManager projectManager = new ProjectManager();
            
            // Create the string of the projects.
            string supportedProjects = "";
            for (int i = 0; i < projectManager.Projects.Count; i++)
            {
                supportedProjects += projectManager.Projects[i].GetName();
                if (i != projectManager.Projects.Count - 1)
                {
                    supportedProjects += ", ";
                }
            }
            
            // Return the supported projects.
            return supportedProjects;
        }
    }
}