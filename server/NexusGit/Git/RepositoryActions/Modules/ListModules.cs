/*
 * TheNexusAvenger
 *
 * Lists the modules in a repository.
 */

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NexusGit.Git.RepositoryActions.Modules
{
    public class SubmoduleData
    {
        public string Name;
        public string Path;
        public string Url;
        public bool Initialized;
    }
    
    public class ListModules
    {
        private Submodules Submodules;

        /*
         * Creates a list remotes action object.
         */
        public ListModules(Repository repository)
        {
            this.Submodules = repository.GetSubmodules();
        }

        /*
         * Returns the submodules.
         */
        public GitResponse PerformAction()
        {
            // Convert the modules to data.
            var submoduleData = new List<SubmoduleData>();
            foreach (var submodule in this.Submodules.Modules)
            {
                var newModule = new SubmoduleData();
                newModule.Name = submodule.Name;
                newModule.Path = submodule.Path;
                newModule.Url = submodule.Url;
                newModule.Initialized = Directory.GetFiles(newModule.Path).Length > 0;
                submoduleData.Add(newModule);
            }
            
            // Return the data.
            return GitResponse.CreateSingleLineResponse(JsonConvert.SerializeObject(submoduleData,Formatting.None));
        }
    }
}