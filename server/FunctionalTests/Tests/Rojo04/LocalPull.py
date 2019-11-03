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
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20008,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
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
        self.setPortNumber(20008)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        responseJson = self.sendGETRequest("/localpull")
        response = json.loads(responseJson)

        self.assertEqual(response["Type"],"Partitions")
        self.assertEquals(len(response["Instances"]),2)
        self.assertEquals(response["Instances"]["src"]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Properties"]["Name"]["Value"],"NexusGit")
        self.assertEquals(len(response["Instances"]["src"]["Children"]),2)
        self.assertEquals(len(response["Instances"]["test"]["Children"]),1)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["ClassName"]["Value"],"Script")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Name"]["Value"],"testscript1")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 1!\")")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"]),0)

        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["ClassName"]["Value"],"LocalScript")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Name"]["Value"],"testscript2")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Source"]["Value"],"print(\"Hello world 2!\")")
        self.assertEquals(len(response["Instances"]["src"]["Children"][1]["Children"]),0)

        self.assertEquals(response["Instances"]["test"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["test"]["Children"][0]["Properties"]["ClassName"]["Value"],"ModuleScript")
        self.assertEquals(response["Instances"]["test"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["test"]["Children"][0]["Properties"]["Name"]["Value"],"testscript3")
        self.assertEquals(response["Instances"]["test"]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["test"]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 3!\")")
        self.assertEquals(len(response["Instances"]["test"]["Children"][0]["Children"]),0)

"""
Tests the local pull as a standalone request with directories.
"""
class Rojo04TestLocalPullAsStandaloneRequestDirectories(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20009,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
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
        self.setPortNumber(20009)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        responseJson = self.sendGETRequest("/localpull")
        response = json.loads(responseJson)

        self.assertEqual(response["Type"],"Partitions")
        self.assertEquals(len(response["Instances"]),1)
        self.assertEquals(response["Instances"]["src"]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Properties"]["Name"]["Value"],"NexusGit")
        self.assertEquals(len(response["Instances"]["src"]["Children"]),2)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["ClassName"]["Value"],"Folder")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Name"]["Value"],"directory1")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"]),1)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Value"],"Script")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["Name"]["Value"],"testscript1")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 1!\")")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"]),0)

        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["ClassName"]["Value"],"LocalScript")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Name"]["Value"],"directory2")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Properties"]["Source"]["Value"],"print(\"Hello world 2!\")")
        self.assertEquals(len(response["Instances"]["src"]["Children"][1]["Children"]),1)

        self.assertEquals(response["Instances"]["src"]["Children"][1]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Children"][0]["Properties"]["ClassName"]["Value"],"ModuleScript")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Children"][0]["Properties"]["Name"]["Value"],"testscript3")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][1]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world 3!\")")
        self.assertEquals(len(response["Instances"]["src"]["Children"][1]["Children"][0]["Children"]),0)

"""
Tests the local pull as a standalone request with a model file.
"""
class Rojo04TestLocalPullAsStandaloneRequestModelFile(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20010,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("src/directory")
        self.workspace.writeFile("src/directory/init.server.lua","print(\"Hello world!\")")
        self.workspace.writeFile("src/directory/hello.model.json","{\"Name\": \"hello\",\"ClassName\": \"Model\",\"Children\": [{\"Name\": \"Some Part\",\"ClassName\": \"Part\"},{\"Name\": \"Some StringValue\",\"ClassName\": \"StringValue\",\"Properties\": {\"Value\": {\"Type\": \"String\",\"Value\": \"Hello, world!\"}}}]}")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20010)
        self.waitForInitialization()

        # Send a standalone request and get the response.
        responseJson = self.sendGETRequest("/localpull")
        response = json.loads(responseJson)

        self.assertEqual(response["Type"],"Partitions")
        self.assertEquals(len(response["Instances"]),1)
        self.assertEquals(response["Instances"]["src"]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Properties"]["Name"]["Value"],"NexusGit")
        self.assertEquals(len(response["Instances"]["src"]["Children"]),1)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["ClassName"]["Value"],"Script")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Name"]["Value"],"directory")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Source"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Properties"]["Source"]["Value"],"print(\"Hello world!\")")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"]),1)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Value"],"Model")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Properties"]["Name"]["Value"],"hello")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"]),2)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["ClassName"]["Value"],"Part")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][0]["Properties"]["Name"]["Value"],"Some Part")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][0]["Children"]),0)

        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["ClassName"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["ClassName"]["Value"],"StringValue")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Name"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Name"]["Value"],"Some StringValue")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Value"]["Type"],"String")
        self.assertEquals(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Properties"]["Value"]["Value"],"Hello, world!")
        self.assertEquals(len(response["Instances"]["src"]["Children"][0]["Children"][0]["Children"][1]["Children"]),0)