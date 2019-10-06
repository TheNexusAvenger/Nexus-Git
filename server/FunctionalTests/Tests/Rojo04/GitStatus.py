"""
TheNexusAvenger

Tests that the git status requests are valid.
"""

import NexusGitFunctionalTest



"""
Tests the git status as a standalone request.
"""
class Rojo04TestGitStatusAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        # Set up several files.
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
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
        self.setPortNumber(30000)
        self.waitForInitialization()

        # Send a split request and get the response.
        response = self.sendGETRequest("/gitstatus")
        self.assertEquals(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file1.txt\",\"New file: file2.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]")

        # Modify a file and assert it was modified.
        self.workspace.runCommand("git commit file1.txt -m Commit")
        self.workspace.writeFile("file1.txt","Test file 4")
        response = self.sendGETRequest("/gitstatus")
        self.assertEquals(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file2.txt\",\"Modified: file1.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]")