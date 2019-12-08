"""
TheNexusAvenger

Tests that the get project partitions request acts correctly.
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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        response = self.sendGETRequest("/getprojectpartitions")
        self.assertEqual(response,"{\"src\":\"ReplicatedStorage.NexusGit\",\"test\":\"ReplicatedStorage.NexusGitTest\"}")