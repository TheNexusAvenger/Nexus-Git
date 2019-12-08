"""
TheNexusAvenger

Tests that the get git status requests are valid.
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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
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
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a split request and get the response.
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file1.txt\",\"New file: file2.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]")

        # Modify a file and assert it was modified.
        self.workspace.runCommand("git commit file1.txt -m Commit")
        self.workspace.writeFile("file1.txt","Test file 4")
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: file2.txt\",\"Modified: file1.txt\",\"Untracked files:\",\"file3.txt\",\"rojo.json\"]")

"""
Tests the git status with renamed files as a standalone request.
"""
class Rojo04TestGitStatusRenamedFilesAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        # Set up several files.
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("directory")
        self.workspace.writeFile("directory/file1.txt","Test file 1")
        self.workspace.writeFile("directory/file2.txt","Test file 2")
        self.workspace.writeFile("directory/file3.txt","Test file 3")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git config user.email \"john@doe.com\"")
        self.workspace.runCommand("git config user.name \"John Doe\"")
        self.workspace.runCommand("git add directory/file1.txt")
        self.workspace.runCommand("git add directory/file2.txt")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a split request and get the response.
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: directory/file1.txt\",\"New file: directory/file2.txt\",\"Untracked files:\",\"directory/file3.txt\",\"rojo.json\"]")

        # Rename a file and assert it was modified.
        self.workspace.runCommand("git commit directory/file1.txt -m Commit")
        self.workspace.renameFile("directory/file1.txt","directory/file4.txt")
        self.workspace.runCommand("git add .")
        response = self.sendGETRequest("/getgitstatus")
        self.assertEqual(response,"[\"Current branch:\",\"master\",\"Changes to be committed:\",\"New file: directory/file2.txt\",\"New file: directory/file3.txt\",\"Renamed: directory/file1.txt -> directory/file4.txt\",\"New file: rojo.json\"]")