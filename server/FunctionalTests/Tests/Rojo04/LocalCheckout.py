"""
TheNexusAvenger

Tests that the git local checkout request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the git commit as a standalone request.
"""
class Rojo04TestGitLocalCheckoutAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20007,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("test")
        self.workspace.writeFile("src/testscript1.server.lua","print(\"Hello world 1!\")")
        self.workspace.writeFile("src/testscript2.client.lua","print(\"Hello world 2!\")")
        self.workspace.writeFile("test/testscript3.lua","print(\"Hello world 3!\")")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git config user.email \"john@doe.com\"")
        self.workspace.runCommand("git config user.name \"John Doe\"")
        self.workspace.runCommand("git add .")
        self.workspace.runCommand("git commit -m \"First commit\"")
        self.workspace.runCommand("git branch second_branch")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20007)
        self.waitForInitialization()

        # Assert the branches are correct.
        self.sendPOSTRequest("/localcheckout","second_branch")
        response = self.sendGETRequest("/localbranches")
        self.assertEquals(response,"[\"master\",\"* second_branch\"]")

        # Write files to the branch.
        self.workspace.writeFile("src/testscript1.server.lua","print(\"Hello world 4!\")")
        self.workspace.writeFile("src/testscript2.client.lua","print(\"Hello world 5!\")")
        self.workspace.writeFile("test/testscript4.lua","print(\"Hello world 6!\")")
        self.workspace.runCommand("git add .")
        self.workspace.runCommand("git commit -m \"Second commit\"")

        # Change branches back and assert the files are back.
        self.sendPOSTRequest("/localcheckout","master")
        response = self.sendGETRequest("/localbranches")
        self.assertEquals(response,"[\"* master\",\"second_branch\"]")
        self.assertEqual(self.workspace.readFile("src/testscript1.server.lua"),"print(\"Hello world 1!\")")
        self.assertEqual(self.workspace.readFile("src/testscript2.client.lua"),"print(\"Hello world 2!\")")
        self.assertEqual(self.workspace.readFile("test/testscript3.lua"),"print(\"Hello world 3!\")")
        self.assertFalse(self.workspace.fileExists("test/testscript4.lua"))
        self.workspace.runCommand("git add .")
        self.workspace.runCommand("git commit -m \"Third commit\"")

        # Change the branch to the second and assert the files are correct.
        self.sendPOSTRequest("/localcheckout","second_branch")
        response = self.sendGETRequest("/localbranches")
        self.assertEquals(response,"[\"master\",\"* second_branch\"]")
        self.assertEqual(self.workspace.readFile("src/testscript1.server.lua"),"print(\"Hello world 4!\")")
        self.assertEqual(self.workspace.readFile("src/testscript2.client.lua"),"print(\"Hello world 5!\")")
        self.assertEqual(self.workspace.readFile("test/testscript3.lua"),"print(\"Hello world 3!\")")
        self.assertEqual(self.workspace.readFile("test/testscript4.lua"),"print(\"Hello world 6!\")")