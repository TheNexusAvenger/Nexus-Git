"""
TheNexusAvenger

Tests that the list remote branches request acts correctly.
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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20012,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheNexusAvenger/Nexus-Instance")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20012)
        self.waitForInitialization()

        # Assert the branches are correct.
        response = self.sendGETRequest("/listremotebranches")
        self.assertEquals(response,"[\"origin/gh-pages\",\"origin/master\"]")

"""
Tests the remote branches with remote tracking as a standalone request.
"""
class Rojo04TestRemoteBranchesWithTrackingAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20016,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheNexusAvenger/Nexus-Instance")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")
        self.workspace.runCommand("git branch -u origin/master")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20016)
        self.waitForInitialization()

        # Assert the branches are correct.
        response = self.sendGETRequest("/listremotebranches")
        self.assertEquals(response,"[\"origin/gh-pages\",\"* origin/master\"]")