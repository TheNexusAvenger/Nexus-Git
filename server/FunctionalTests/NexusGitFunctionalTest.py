"""
TheNexusAvenger

Base class for functional testing Nexus Git.
"""

import os
import subprocess
import unittest
import requests
import Workspace

FILE_LOCATION = os.path.realpath(__file__)
DELETE_FILE_TIMEOUT_MILLISECONDS = 5000
EXECUTABLE_LOCATIONS = [
	FILE_LOCATION + "/../../bin/NexusGit-win-x64.exe"
]



portsUsedByTests = []



"""
Returns the path to the executable.
"""
def getExecutableLocation():
	currentExecutable = None
	currentExecutableLastModifiedTime = 0

	# Iterate through the executables and find the latest.
	for location in EXECUTABLE_LOCATIONS:
		location = os.path.realpath(location)
		if os.path.exists(location):
			lastModifiedTime = os.path.getmtime(location)
			if lastModifiedTime > currentExecutableLastModifiedTime:
				currentExecutable = location
				currentExecutableLastModifiedTime = lastModifiedTime

	# Return the executable.
	return currentExecutable



"""
Abstract class for handling functional tests for Nexus Git.
"""
class NexusGitFunctionalTest(unittest.TestCase):
	"""
	Creates a Nexus Git Functional Test object.
	"""
	def __init__(self,_=None,executableLocation=None,workspace=None):
		super().__init__("performTest")

		# Replace the resources if they are not set.
		if executableLocation is None:
			print("Executable not defined. Getting from known locations.")
			executableLocation = getExecutableLocation()
			if executableLocation is None:
				raise FileNotFoundError("Executable not found.")

		if workspace is None:
			print("Workspace not defined. Creating new workspace.")
			workspace = Workspace.Workspace()

		# Set the resources.
		self.executableLocation = executableLocation
		self.workspace = workspace
		self.serverProcess = None
		self.verbose = True
		self.port = 8000

	"""
	Sets the port number.
	"""
	def setPortNumber(self,port):
		# Raise an error if the port was used.
		global portsUsedByTests
		if port in portsUsedByTests:
			raise RuntimeError("Port already used by another test: " + str(port))

		# Add the port.
		portsUsedByTests.append(port)
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
		self.serverProcess.wait()

		# Call the teardown method.
		self.teardownTest()

	"""
	Performs the test.
	"""
	def performTest(self):
		if self.verbose:
			print("Running " + type(self).__name__)

		self.runTest()