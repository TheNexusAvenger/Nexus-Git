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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20002,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
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
        self.setPortNumber(20002)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        self.sendPOSTRequest("/gitadd","[\"src/testscript1.server.lua\",\"test/testscript3.lua\"]")
        response = self.sendPOSTRequest("/gitcommit","{\"message\":\"Test commit\",\"files\":[\"src/testscript1.server.lua\",\"test/testscript3.lua\"]}")
        self.assertEqual(response,"[\"Commit successful.\"]")

        # Assert the status is correct.
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Untracked files:\",\"rojo.json\",\"src/testscript2.client.lua\"]")

"""
Tests the git commit with renamed files as a standalone request.
"""
class Rojo04TestGitCommitRenamedFilesAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        # Set up several files.
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20018,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.writeFile("file1.txt","Test file 1")
        self.workspace.writeFile("file2.txt","Test file 2")
        self.workspace.writeFile("file3.txt","Test file 3")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git config user.email \"john@doe.com\"")
        self.workspace.runCommand("git config user.name \"John Doe\"")
        self.workspace.runCommand("git add file1.txt")
        self.workspace.runCommand("git add file2.txt")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20018)
        self.waitForInitialization()

        # Send a split request and get the response.
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file1.txt\",\"New file: file2.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]")

        # Modify a file and assert it was modified.
        self.workspace.runCommand("git commit file1.txt -m Commit")
        self.workspace.renameFile("file1.txt","file4.txt")
        self.workspace.runCommand("git add .")

        # Send a standalone request and get the response.
        response = self.sendPOSTRequest("/gitcommit","{\"message\":\"Test commit\",\"files\":[\"file1.txt\",\"file4.txt\"]}")
        self.assertEqual(response,"[\"Commit successful.\"]")
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file2.txt\",\"New file: file3.txt\",\"New file: rojo.json\"]")