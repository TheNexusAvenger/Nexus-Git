"""
TheNexusAvenger

Tests that the list remotes request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the list remotes as a standalone request.
"""
class Rojo04TestListRemotesAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20005,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin1 https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git")
        self.workspace.runCommand("git remote add origin2 https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20005)
        self.waitForInitialization()

        # Assert the remotes are correct.
        response = self.sendGETRequest("/listremotes")
        self.assertEquals(response,"[\"origin1\",\"origin2\"]")