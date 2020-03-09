"""
TheNexusAvenger

Tests that the get version request acts correctly.
"""

import NexusGitFunctionalTest



"""
Tests the version as a split request.
"""
class Rojo04TestVersionAsSplitRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
	"""
	Setup for the test.
	"""
	def setupTest(self):
		self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

	"""
	Runs the test.
	"""
	def runTest(self):
		# Wait for it to initialize.
		self.setPortNumber(20001)
		self.waitForInitialization()

		# Send a split request and get the response.
		response = self.sendGETRequest("/getversion?packet=0&maxPackets=1")
		self.assertEqual(response,"{\"status\":\"success\",\"id\":0,\"currentPacket\":0,\"maxPackets\":1,\"packet\":\"{\\r\\n  \\\"version\\\": \\\"0.1.0 Alpha\\\",\\r\\n  \\\"project\\\": \\\"Rojo 0.4.X\\\"\\r\\n}\"}")

"""
Tests the version as a standalone request.
"""
class Rojo04TestVersionAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
	"""
	Setup for the test.
	"""
	def setupTest(self):
		self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 20001,\"partitions\": {\"src\": {\"path\": \"client/src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"client/test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

	"""
	Runs the test.
	"""
	def runTest(self):
		# Wait for it to initialize.
		self.setPortNumber(20001)
		self.waitForInitialization()

		# Send a split request and get the response.
		response = self.sendGETRequest("/getversion")
		self.assertEqual(response,"{\r\n  \"version\": \"0.1.0 Alpha\",\r\n  \"project\": \"Rojo 0.4.X\"\r\n}")