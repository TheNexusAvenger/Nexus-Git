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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Assert the branches are correct.
        response = self.sendGETRequest("/listremotebranches")
        self.assertEqual(response,"[\"origin/dummy-branch-1\",\"origin/dummy-branch-2\",\"origin/master\"]")

"""
Tests the remote branches with remote tracking as a standalone request.
"""
class Rojo04TestRemoteBranchesWithTrackingAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")
        self.workspace.runCommand("git branch -u origin/master")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Assert the branches are correct.
        response = self.sendGETRequest("/listremotebranches")
        self.assertEqual(response,"[\"origin/dummy-branch-1\",\"origin/dummy-branch-2\",\"* origin/master\"]")