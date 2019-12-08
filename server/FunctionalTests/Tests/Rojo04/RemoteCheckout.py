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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20013,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

        # Initialize the git repository.
        self.workspace.runCommand("git init")
        self.workspace.runCommand("git remote add origin https://github.com/TheBotAvenger/Initialized-Dummy-Repository.git")
        self.workspace.runCommand("git checkout -b master")
        self.workspace.runCommand("git pull origin master")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20013)
        self.waitForInitialization()

        # Checkout the gh-pages branch and assert the files are correct.
        self.sendPOSTRequest("/remotecheckout","{\"localBranch\":\"test_branch\",\"remote\":\"origin\",\"branch\":\"dummy-branch-2\"}")
        self.assertTrue(self.workspace.fileExists("DummyFile6"))
        self.assertFalse(self.workspace.fileExists("DummyFile3"))
        response = self.sendGETRequest("/listlocalbranches")
        self.assertEqual(response,"[\"master\",\"* test_branch\"]")