"""
TheNexusAvenger

Tests that the git commit request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the git commit as a standalone request.
"""
class Rojo04TestGitCommitAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("test")
        self.workspace.writeFile("src/testscript1.server.lua","print(\"Hello world 1!\")")
        self.workspace.writeFile("src/testscript2.client.lua","print(\"Hello world 2!\")")
        self.workspace.writeFile("test/testscript3.lua","print(\"Hello world 3!\")")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git config user.email \"john@doe.com\"")
        self.workspace.runCommand("git config user.name \"John Doe\"")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(30000)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        self.sendPOSTRequest("/gitadd","[\"src/testscript1.server.lua\",\"test/testscript3.lua\"]")
        response = self.sendPOSTRequest("/gitcommit","{\"message\":\"Test commit\",\"files\":[\"src/testscript1.server.lua\",\"test/testscript3.lua\"]}")
        self.assertEquals(response,"[\"Commit successful.\"]")

        # Assert the status is correct.
        response = self.sendGETRequest("/gitstatus")
        self.assertEquals(response,"[\"Current branch:\",\"master\",\"Untracked files:\",\"rojo.json\",\"src/testscript2.client.lua\"]")