"""
TheNexusAvenger

Tests that the local push request acts correctly.
"""

import json
import NexusGitFunctionalTest



"""
Tests the local push as a standalone request with several files.
"""
class Rojo04TestLocalPushAsStandaloneRequestMultipleFile(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("test")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Create the request.
        request = {
            "Type": "Partitions",
            "Instances": {
                "src": {
                    "Properties": {
                        "Name": {
                            "Type": "String",
                            "Value": "NexusGit",
                        },
                        "ClassName": {
                            "Type": "String",
                            "Value": "Folder",
                        },
                    },
                    "Children": [
                        {
                            "Properties": {
                                "Name": {
                                    "Type": "String",
                                    "Value": "testscript1",
                                },
                                "ClassName": {
                                    "Type": "String",
                                    "Value": "Script",
                                },
                                "Source": {
                                    "Type": "String",
                                    "Value": "print(\"Hello world 1!\")",
                                },
                            },
                            "Children": []
                        },
                        {
                            "Properties": {
                                "Name": {
                                    "Type": "String",
                                    "Value": "testscript2",
                                },
                                "ClassName": {
                                    "Type": "String",
                                    "Value": "LocalScript",
                                },
                                "Source": {
                                    "Type": "String",
                                    "Value": "print(\"Hello world 2!\")",
                                },
                            },
                            "Children": []
                        },
                    ]
                },
                "test": {
                    "Properties": {
                        "Name": {
                            "Type": "String",
                            "Value": "NexusGitTests",
                        },
                        "ClassName": {
                            "Type": "String",
                            "Value": "Folder",
                        },
                    },
                    "Children": [
                        {
                            "Properties": {
                                "Name": {
                                    "Type": "String",
                                    "Value": "testscript3",
                                },
                                "ClassName": {
                                    "Type": "String",
                                    "Value": "ModuleScript",
                                },
                                "Source": {
                                    "Type": "String",
                                    "Value": "print(\"Hello world 3!\")",
                                },
                            },
                            "Children": []
                        },
                    ]
                }
            }
        }
        requestJson = json.dumps(request)

        # Send a standalone request and get the response.
        response = self.sendPOSTRequest("/localpush",requestJson)
        self.assertEqual(response,"Local push successful.","Response message is incorrect.")

        # Assert the files are correct.
        self.assertEqual(self.workspace.readFile("src/testscript1.server.lua"),"print(\"Hello world 1!\")","Body is incorrect.")
        self.assertEqual(self.workspace.readFile("src/testscript2.client.lua"),"print(\"Hello world 2!\")","Body is incorrect.")
        self.assertEqual(self.workspace.readFile("test/testscript3.lua"),"print(\"Hello world 3!\")","Body is incorrect.")

"""
Tests the local push as a standalone request with a missing partition.
"""
class Rojo04TestLocalPushAsStandaloneRequestMissingPartition(NexusGitFunctionalTest.NexusGitFunctionalTest):
    """
    Setup for the test.
    """
    def setupTest(self):
        self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")
        self.workspace.createDirectory("src")
        self.workspace.createDirectory("test")

    """
    Runs the test.
    """
    def runTest(self):
        # Wait for it to initialize.
        self.setPortNumber(20001)
        self.waitForInitialization()

        # Create the request.
        request = {
            "Type": "Partitions",
            "Instances": {
                "src": None,
                "test": {
                    "Properties": {
                        "Name": {
                            "Type": "String",
                            "Value": "NexusGitTests",
                        },
                        "ClassName": {
                            "Type": "String",
                            "Value": "Folder",
                        },
                    },
                    "Children": [
                        {
                            "Properties": {
                                "Name": {
                                    "Type": "String",
                                    "Value": "testscript3",
                                },
                                "ClassName": {
                                    "Type": "String",
                                    "Value": "ModuleScript",
                                },
                                "Source": {
                                    "Type": "String",
                                    "Value": "print(\"Hello world 3!\")",
                                },
                            },
                            "Children": []
                        },
                    ]
                }
            }
        }
        requestJson = json.dumps(request)

        # Send a standalone request and get the response.
        response = self.sendPOSTRequest("/localpush",requestJson)
        self.assertEqual(response,"Local push successful.","Response message is incorrect.")

        # Assert the files are correct.
        self.assertFalse(self.workspace.fileExists("src/testscript1.server.lua"))
        self.assertFalse(self.workspace.fileExists("src/testscript2.client.lua"))
        self.assertEqual(self.workspace.readFile("test/testscript3.lua"),"print(\"Hello world 3!\")","Body is incorrect.")