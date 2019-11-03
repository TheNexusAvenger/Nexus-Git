"""
TheNexusAvenger

Tests that the local branches request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the local branches as a standalone request.
"""
class Rojo04TestLocalBranchesAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20006,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git config user.email \"john@doe.com\"")
        self.workspace.runCommand("git config user.name \"John Doe\"")
        self.workspace.runCommand("git add .")
        self.workspace.runCommand("git commit -m \"First commit\"")
        self.workspace.runCommand("git branch branch_1")
        self.workspace.runCommand("git branch branch_2")
        self.workspace.runCommand("git branch branch_3")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20006)
        self.waitForInitialization()

        # Assert the branches are correct.
        response = self.sendGETRequest("/localbranches")
        self.assertEquals(response,"[\"branch_1\",\"branch_2\",\"branch_3\",\"* master\"]")