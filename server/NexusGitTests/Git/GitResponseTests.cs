/*
 * TheNexusAvenger
 * 
 * Test the NexusGit.Git.GitResponse class.
 */

using NexusGit.Git;
using NUnit.Framework;

namespace NexusGitTests.Git
{
    [TestFixture]
    public class GitResponseTests
    {
        /*
         * Tests the ToJSON method.
         */
        [Test]
        public void ToJsonTest()
        {
            // Create the component under testing.
            GitResponse CuT = new GitResponse();

            // Add 3 responses.
            CuT.AddResponse("Test 1");
            CuT.AddResponse("Test 2");
            CuT.AddResponse("Test \"3\"");

            // Assert it is serialized correctly.
            Assert.AreEqual(CuT.ToJson(),"[\"Test 1\",\"Test 2\",\"Test \\\"3\\\"\"]");
        }
    }
}