--[[
TheNexusAvenger

Tests the GitStatusRequest class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local GetGitStatusRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetGitStatusRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

--Mock the request.
local MockGitStatusRequest = GetGitStatusRequest:Extend()
function MockGitStatusRequest:SendRequest()
	return {
		GetResponse = function()
			return "[\"Current branch:\","
				.."\"master\","
				.."\"Remote branch:\","
				.."\"origin/master\","
				.."\"Ahead by:\","
				.."\"2\","
				.."\"Changes to be committed:\","
				.."\"Modified: TestFile1.lua\","
				.."\"New file: TestFile2.lua\","
				.."\"Modified: Directory1/TestFile3.lua\","
				.."\"New file: Directory1/TestFile4.lua\","
				.."\"Modified: Directory1/Directory2/TestFile5.lua\","
				.."\"Renamed: Directory1/Directory2/TestFile6.lua\","
				.."\"Deleted: Directory1/Directory2/TestFile7.lua\","
				.."\"Untracked files:\","
				.."\"TestFile8.lua\","
				.."\"Directory1/TestFile9.lua\"]"
		end
	}
end



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = GetGitStatusRequest.new("http://localhost:0000")
	UnitTest:AssertEquals(CuT.ClassName,"GetGitStatusRequest","Class name is incorrect.")
end)

--[[
Tests the StringsToFiles method.
--]]
NexusUnitTesting:RegisterUnitTest("StringsToFiles",function(UnitTest)
	--Create a table of tracked objects.
	local FileStrings = {
		Modified = {
			"TestFile1.lua",
			"Directory1/TestFile3.lua",
			"Directory1/Directory2/TestFile5.lua",
		},
		Created = {
			"TestFile2.lua",
			"Directory1/TestFile4.lua",
		},
		Renamed = {
			"Directory1/Directory2/TestFile6.lua",
		},
		Deleted = {
			"Directory1/Directory2/TestFile7.lua",	
		},
		Untracked = {
			"TestFile8.lua",
			"Directory1/TestFile9.lua",	
		},
	}
		
	--Convert the strings to a list of files.
	local CuT = GetGitStatusRequest.new("http://localhost:0000")
	local Files = CuT:StringsToFiles(FileStrings)
	UnitTest:AssertEquals(#Files,4,"Incorrect amount of files.")
	
	--[[
	Returns the file for the given name.
	--]]
	local function GetFile(FileName)
		for _,File in pairs(Files) do
			if File:GetFileName() == FileName then
				return File
			end
		end
	end
	
	--Assert the modified files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Modified:Equals(GetFile("TestFile1.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Modified:Equals(GetFile("Directory1"):GetFile("TestFile3.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Modified:Equals(GetFile("Directory1"):GetFile("Directory2"):GetFile("TestFile5.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the created files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Created:Equals(GetFile("TestFile2.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Created:Equals(GetFile("Directory1"):GetFile("TestFile4.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the renamed files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Renamed:Equals(GetFile("Directory1"):GetFile("Directory2"):GetFile("TestFile6.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the deleted files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Deleted:Equals(GetFile("Directory1"):GetFile("Directory2"):GetFile("TestFile7.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the untracked files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Untracked:Equals(GetFile("TestFile8.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Untracked:Equals(GetFile("Directory1"):GetFile("TestFile9.lua"):GetStatus()),"File status is incorrect.")
end)

--[[
Tests the GetStatus method.
--]]
NexusUnitTesting:RegisterUnitTest("GetStatus",function(UnitTest)
	--Create the mocked component under testing.
	local CuT = MockGitStatusRequest.new("http://localhost:0000")
	local StatusData = CuT:GetStatus()
	
	--Assert the base response info is correct.
	UnitTest:AssertEquals(StatusData.CurrentBranch,"master","Current branch is incorrect.")
	UnitTest:AssertEquals(StatusData.RemoteBranch,"origin/master","Remote branch is incorrect.")
	UnitTest:AssertEquals(StatusData.AheadBy,2,"Ahead by count is incorrect.")
		
	--[[
	Returns the file for the given name.
	--]]
	local function GetFile(FileName)
		for _,File in pairs(StatusData.Files) do
			if File:GetFileName() == FileName then
				return File
			end
		end
	end
	
	--Assert the modified files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Modified:Equals(GetFile("TestFile1.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Modified:Equals(GetFile("Directory1"):GetFile("TestFile3.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Modified:Equals(GetFile("Directory1"):GetFile("Directory2"):GetFile("TestFile5.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the created files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Created:Equals(GetFile("TestFile2.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Created:Equals(GetFile("Directory1"):GetFile("TestFile4.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the renamed files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Renamed:Equals(GetFile("Directory1"):GetFile("Directory2"):GetFile("TestFile6.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the deleted files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Deleted:Equals(GetFile("Directory1"):GetFile("Directory2"):GetFile("TestFile7.lua"):GetStatus()),"File status is incorrect.")
	
	--Assert the untracked files are correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Untracked:Equals(GetFile("TestFile8.lua"):GetStatus()),"File status is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Untracked:Equals(GetFile("Directory1"):GetFile("TestFile9.lua"):GetStatus()),"File status is incorrect.")
end)



return true