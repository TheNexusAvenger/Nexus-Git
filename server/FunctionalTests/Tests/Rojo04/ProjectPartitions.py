"""
TheNexusAvenger

Tests that the project partitions request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the project partitions as a standalone request.
"""
class Rojo04TestProjectPartitionsAsStandaloneRequestMultipleFile(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(30000)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        response = self.sendGETRequest("/projectpartitions")
        self.assertEquals(response,"{\"src\":\"ReplicatedStorage.NexusGit\",\"test\":\"ReplicatedStorage.NexusGitTest\"}")