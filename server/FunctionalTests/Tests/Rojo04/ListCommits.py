"""
TheNexusAvenger

Tests that the list commits request acts correctly.
"""

import json
import NexusGitFunctionalTest



"""
Tests the list commits as a standalone request.
"""
class Rojo04TestListCommitsAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
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
        self.workspace.runCommand("git remote add origin https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git")
        self.workspace.runCommand("git fetch")
        self.workspace.runCommand("git config user.email \"john@doe.com\"")
        self.workspace.runCommand("git config user.name \"John Doe\"")
        self.workspace.runCommand("git add src/testscript1.server.lua")
        self.workspace.runCommand("git add src/testscript2.client.lua")
        self.workspace.runCommand("git commit -m \"Test commit 1\"")
        self.workspace.runCommand("git add test/testscript3.lua")
        self.workspace.runCommand("git commit -m \"Test commit 2\nNew line\"")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Assert the commits are correct.
        responseJson = self.sendGETRequest("/listcommits?remote=origin&branch=master")
        response = json.loads(responseJson)
        self.assertEqual(len(response[0]["id"]),40)
        self.assertEqual(response[0]["message"],"Test commit 2\nNew line")
        self.assertEqual(len(response[1]["id"]),40)
        self.assertEqual(response[1]["message"],"Test commit 1")