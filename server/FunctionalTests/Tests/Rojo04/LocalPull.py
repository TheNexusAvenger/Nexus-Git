"""
TheNexusAvenger

Tests that the local pull request acts correctly.
"""

import json
import NexusGitFunctionalTest



"""
Tests the local pull as a standalone request with several files.
"""
class Rojo04TestLocalPullAsStandaloneRequestMultipleFile(NexusGitFunctionalTest.NexusGitFunctionalTest):
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

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        responseJson = self.sendGETRequest("/localpull")
        response = json.loads(responseJson)

        self.assertEqual(response["Type"],"Partitions")
        self.assertEqual(len(response["Instances"]),2)
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Properties"]["Name"]["Value"],"NexusGit")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"]),2)
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"]),1)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["ClassName"]["Value"],"Script")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Name"]["Value"],"testscript1")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 1!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"]),0)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["ClassName"]["Value"],"LocalScript")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Name"]["Value"],"testscript2")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Source"]["Value"],"print(\"Hello world 2!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"]),0)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Properties"]["ClassName"]["Value"],"ModuleScript")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Properties"]["Name"]["Value"],"testscript3")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 3!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGitTest"]["Children"][0]["Children"]),0)

"""
Tests the local pull as a standalone request with directories.
"""
class Rojo04TestLocalPullAsStandaloneRequestDirectories(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("src/directory1")
        self.workspace.createDirectory("src/directory2")
        self.workspace.writeFile("src/directory1/testscript1.server.lua","print(\"Hello world 1!\")")
        self.workspace.writeFile("src/directory2/init.client.lua","print(\"Hello world 2!\")")
        self.workspace.writeFile("src/directory2/testscript3.lua","print(\"Hello world 3!\")")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        responseJson = self.sendGETRequest("/localpull")
        response = json.loads(responseJson)

        self.assertEqual(response["Type"],"Partitions")
        self.assertEqual(len(response["Instances"]),1)
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Properties"]["Name"]["Value"],"NexusGit")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"]),2)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["ClassName"]["Value"],"Folder")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Name"]["Value"],"directory1")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"]),1)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Value"],"Script")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["Name"]["Value"],"testscript1")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 1!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"]),0)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["ClassName"]["Value"],"LocalScript")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Name"]["Value"],"directory2")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Properties"]["Source"]["Value"],"print(\"Hello world 2!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"]),1)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Properties"]["ClassName"]["Value"],"ModuleScript")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Properties"]["Name"]["Value"],"testscript3")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 3!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][1]["Children"][0]["Children"]),0)

"""
Tests the local pull as a standalone request with a model file.
"""
class Rojo04TestLocalPullAsStandaloneRequestModelFile(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("src/directory")
        self.workspace.writeFile("src/directory/init.server.lua","print(\"Hello world!\")")
        self.workspace.writeFile("src/directory/hello.model.json","{\"Name\": \"hello\",\"ClassName\": \"Model\",\"Children\": [{\"Name\": \"Some Part\",\"ClassName\": \"Part\"},{\"Name\": \"Some StringValue\",\"ClassName\": \"StringValue\",\"Properties\": {\"Value\": {\"Type\": \"String\",\"Value\": \"Hello, world!\"}}}]}")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        responseJson = self.sendGETRequest("/localpull")
        response = json.loads(responseJson)

        self.assertEqual(response["Type"],"Partitions")
        self.assertEqual(len(response["Instances"]),1)
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Properties"]["Name"]["Value"],"NexusGit")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"]),1)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["ClassName"]["Value"],"Script")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Name"]["Value"],"directory")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world!\")")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"]),1)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Value"],"Model")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Properties"]["Name"]["Value"],"hello")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"]),2)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Value"],"Part")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["Name"]["Value"],"Some Part")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][0]["Children"]),0)

        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["ClassName"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["ClassName"]["Value"],"StringValue")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Name"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Name"]["Value"],"Some StringValue")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Value"]["Type"],"String")
        self.assertEqual(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Value"]["Value"],"Hello, world!")
        self.assertEqual(len(response["Instances"]["ReplicatedStorage.NexusGit"]["Children"][0]["Children"][0]["Children"][1]["Children"]),0)