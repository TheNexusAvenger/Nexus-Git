"""
TheNexusAvenger

Tests that the version request acts correctly.
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
		self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"client/src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"client/test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

	"""
	Runs the test.
	"""
	def runTest(self):
		# Wait for it to initialize.
		self.setPortNumber(30000)
		self.waitForInitialization()

		# Send a split request and get the response.
		response = self.sendGETRequest("/version?packet=0&maxPackets=1")
		self.assertEqual(response,"{\"status\":\"success\",\"id\":0,\"currentPacket\":0,\"maxPackets\":0,\"packet\":\"{\\r\\n  \\\"version\\\": \\\"0.1.0 Alpha\\\",\\r\\n  \\\"project\\\": \\\"Rojo 0.4.X\\\"\\r\\n}\"}")

"""
Tests the version as a standalone request.
"""
class Rojo04TestVersionAsStandaloneRequest(NexusGitFunctionalTest.NexusGitFunctionalTest):
	"""
	Setup for the test.
	"""
	def setupTest(self):
		self.workspace.writeFile("rojo.json","{\"name\": \"Nexus Git Repository\",\"servePort\": 30000,\"partitions\": {\"src\": {\"path\": \"client/src\",\"target\": \"ReplicatedStorage.NexusGit\"},\"test\": {\"path\": \"client/test\",\"target\":\"ReplicatedStorage.NexusGitTest\"}}}")

	"""
	Runs the test.
	"""
	def runTest(self):
		# Wait for it to initialize.
		self.setPortNumber(30000)
		self.waitForInitialization()

		# Send a split request and get the response.
		response = self.sendGETRequest("/version")
		self.assertEqual(response,"{\r\n  \"version\": \"0.1.0 Alpha\",\r\n  \"project\": \"Rojo 0.4.X\"\r\n}")