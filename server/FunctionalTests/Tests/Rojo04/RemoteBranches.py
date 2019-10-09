"""
TheNexusAvenger

Tests that the remote branches request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the remote branches as a standalone request.
"""
class Rojo04TestRemoteBranchesAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheNexusAvenger/Nexus-Instance")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")
        self.workspace.runCommand("git checkout -b gh-pages")
        self.workspace.runCommand("git pull origin gh-pages")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(30000)
        self.waitForInitialization()

        # Assert the branches are correct.
        response = self.sendGETRequest("/remotebranches")
        self.assertEquals(response,"[\"origin/gh-pages\",\"origin/master\"]")