"""
TheNexusAvenger

Runs functional tests.
"""

import importlib.machinery
import os
import sys
import unittest
import Workspace
import NexusGitFunctionalTest



"""
Imports a Python module from a file location.
From: https://stackoverflow.com/questions/301134/dynamic-module-import-in-python
"""
def importFromFile(filePath):
	pythonModule = None
	moduleName,fileExtension = os.path.splitext(os.path.split(filePath)[-1])

	# Local the source or compiled.
	if fileExtension.lower() == ".py":
		loader = importlib.machinery.SourceFileLoader(moduleName,filePath)
		pythonModule = loader.load_module()

	# Return the module.
	return pythonModule

"""
Returns the functional tests to run.
"""
def getFunctionalTests(directory = None):
	# Set the directory as root if not specified.
	if directory is None:
		directory = os.path.abspath(__file__ + "\\..")

	# Import the files.
	testClasses = []
	for subFile in os.listdir(directory):
		subFilePath = os.path.abspath(directory + "\\" + subFile)

		if subFilePath != os.path.abspath(__file__):
			if os.path.isdir(subFilePath):
				for testClass in getFunctionalTests(subFilePath + "\\"):
					testClasses.append(testClass)
			else:
				module = importFromFile(subFilePath)
				if module is not None:
					for subClassName in dict([(name, cls) for name, cls in module.__dict__.items() if isinstance(cls, type)]):
						subClass = getattr(module,subClassName)
						if issubclass(subClass,NexusGitFunctionalTest.NexusGitFunctionalTest) and subClass != NexusGitFunctionalTest.NexusGitFunctionalTest:
							testClasses.append(subClass)


	# Return the tests.
	return testClasses

"""
Runs the functional tests.
"""
def runFunctionalTests(executableLocation=None):
	# Create the tests.
	suite = unittest.TestSuite()
	for testClass in getFunctionalTests():
		workspace = Workspace.Workspace()
		suite.addTest(testClass(executableLocation=executableLocation,workspace=workspace))

	# Run the tests.
	runner = unittest.TextTestRunner()
	runner.run(suite)

"""
Runs the script.
"""
def main():
	# Error check the argument.
	if len(sys.argv) < 2:
		print("Executable location not given. All tests will try to detect it.")
		runFunctionalTests()
		return
	elif not os.path.exists(sys.argv[1]):
		raise RuntimeError("Executable doesn't exist: " + sys.argv[1])

	# Run the functional tests.
	runFunctionalTests(sys.argv[1])


# Run the script.
if __name__ == '__main__':
	main()