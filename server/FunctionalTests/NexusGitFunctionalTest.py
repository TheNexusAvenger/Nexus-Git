"""
TheNexusAvenger

Base class for functional testing Nexus Git.
"""

DELETE_FILE_TIMEOUT_MILLISECONDS = 5000

import subprocess
import unittest
import requests


"""
Abstract class for handling functional tests for Nexus Git.
"""
class NexusGitFunctionalTest(unittest.TestCase):
	"""
	Creates a Nexus Git Functional Test object.
	"""
	def __init__(self,executableLocation,workspace):
		super().__init__("performTest")

		self.executableLocation = executableLocation
		self.workspace = workspace
		self.serverProcess = None
		self.verbose = True
		self.port = 8000

	"""
	Sets the port number.
	"""
	def setPortNumber(self,port):
		self.port = port

	"""
	Waits for the server to application to be online.
	"""
	def waitForInitialization(self):
		self.sendGETRequest("/Version")

	"""
	Sends a GET request and return the contents.
	"""
	def sendGETRequest(self,URL):
		response = requests.get("http://localhost:" + str(self.port) + URL)
		return response.content.decode()

	"""
	Sends a POST request and return the contents.
	"""
	def sendPOSTRequest(self,URL,body):
		response = requests.post("http://localhost:" + str(self.port) + URL,body)
		return response.content.decode()

	"""
	Setup for the test.
	"""
	def setupTest(self):
		pass

	"""
	Runs the test.
	"""
	def runTest(self):
		pass

	"""
	Teardown for the test.
	"""
	def teardownTest(self):
		pass

	"""
	Sets up the unit test.
	"""
	def setUp(self):
		if self.verbose:
			print("Setting up " + type(self).__name__ + " using " + self.workspace.workspaceDirectory)

		# Call the setup method.
		self.setupTest()

		# Start the server in a thread.
		self.serverProcess = subprocess.Popen(self.executableLocation + " serve",cwd=self.workspace.workspaceDirectory)

	"""
	Tears down the unit test.
	"""
	def tearDown(self):
		if self.verbose:
			print("Tearing down " + type(self).__name__ + "\n\r\n\r\n\r")

		# End the server thread.
		self.serverProcess.kill()

		# Call the teardown method.
		self.teardownTest()

	"""
	Performs the test.
	"""
	def performTest(self):
		if self.verbose:
			print("Running " + type(self).__name__)

		self.runTest()