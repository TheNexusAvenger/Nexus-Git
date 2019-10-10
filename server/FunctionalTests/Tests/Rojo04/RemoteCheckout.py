"""
TheNexusAvenger

Tests that the remote checkout request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the remote checkout as a standalone request.
"""
class Rojo04TestRemoteCheckoutAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheNexusAvenger/Nexus-Instance")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(30000)
        self.waitForInitialization()

        # Checkout the gh-pages branch and assert the files are correct.
        self.sendPOSTRequest("/remotecheckout","{\"localBranch\":\"test_branch\",\"remote\":\"origin\",\"branch\":\"gh-pages\"}")
        self.assertTrue(self.workspace.fileExists("index.html"))
        self.assertFalse(self.workspace.fileExists("README.md"))
        response = self.sendGETRequest("/localbranches")
        self.assertEquals(response,"[\"master\",\"* test_branch\"]")