"""
TheNexusAvenger

Tests that the git add request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the git add as a standalone request.
"""
class Rojo04TestGitAddAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("test")
        self.workspace.writeFile("src/testscript1.server.lua","print(\"Hello world 1!\")")
        self.workspace.writeFile("src/testscript2.client.lua","print(\"Hello world 2!\")")
        self.workspace.writeFile("test/testscript3.lua","print(\"Hello world 3!\")")

        # Initialize the git repository.
        self.workspace.runCommand("git init")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        response = self.sendPOSTRequest("/gitadd","[\"src/testscript1.server.lua\",\"test/testscript3.lua\"]")
        self.assertEquals(response,"[\"Add complete.\"]")

        # Assert the status is correct.
        response = self.sendGETRequest("/gitstatus")
        self.assertEquals(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: src/testscript1.server.lua\",\"New file: test/testscript3.lua\",\"Untracked files:\",\"rojo.json\",\"src/testscript2.client.lua\"]")