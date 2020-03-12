/*
 * TheNexusAvenger
 *
 * Tests the ProjectManager class.
 */

using NexusGit.NexusGit.Projects;
using NUnit.Framework;

namespace NexusGitTests.NexusGit.Projects
{
    public class ProjectManagerTests
    {
        /*
         * Tests the GetSupportedProjects method.
         */
        [Test]
        public void GetSupportedProjectsTest()
        {
            Assert.AreEqual(ProjectManager.GetSupportedProjects(),"Rojo 0.5.X, Rojo 0.4.X");
        }
    }
}