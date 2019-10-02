"""
TheNexusAvenger

"Sandbox" for reading and writing files for functional tests.
"""

import os
import subprocess
import tempfile



"""
Class representing a workspace.
"""
class Workspace():
	"""
	Creates a workspace object.
	"""
	def __init__(self):
		self.workspaceDirectory = tempfile.mkdtemp() + "\\"

	"""
	Returns if a file exists.
	"""
	def fileExists(self,fileLocation):
		fileLocation = self.workspaceDirectory + fileLocation
		return os.path.exists(fileLocation) and os.path.isfile(fileLocation)

	"""
	Returns if a directory exists.
	"""
	def directoryExists(self,fileLocation):
		fileLocation = self.workspaceDirectory + fileLocation
		return os.path.exists(fileLocation) and os.path.isdir(fileLocation)

	"""
	Writes a file.
	"""
	def writeFile(self,fileLocation,fileContents):
		fileLocation = self.workspaceDirectory + fileLocation

		with open(fileLocation,"w") as file:
			file.write(fileContents)
			file.truncate()

	"""
	Reads a file.
	"""
	def readFile(self,fileLocation):
		fileLocation = self.workspaceDirectory + fileLocation

		with open(fileLocation) as file:
			return file.read()

	"""
	Creates a directory.
	"""
	def createDirectory(self,fileLocation):
		fileLocation = self.workspaceDirectory + fileLocation
		os.mkdir(fileLocation)

	"""
	Runs a command in the workspace and waits for it to complete.
	"""
	def runCommand(self,command):
		subprocess.Popen(command,cwd=self.workspaceDirectory).wait()