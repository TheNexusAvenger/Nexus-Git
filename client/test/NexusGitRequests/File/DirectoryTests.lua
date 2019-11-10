--[[
TheNexusAvenger

Tests the Directory class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = Directory.new("DirectoryName")
	UnitTest:AssertEquals(CuT.ClassName,"Directory","Class name is incorrect.")
	UnitTest:AssertEquals(CuT:GetFileName(),"DirectoryName","Directory name is incorrect.")
end)

--[[
Tests the FilterFiles method.
--]]
NexusUnitTesting:RegisterUnitTest("FilterFiles",function(UnitTest)
	--Create several files and directories.
	local File1,File2,File3 = File.new("TestFile1.lua"),File.new("TestFile2.lua"),File.new("TestFile3.lua")
	local File4,File5,File6 = File.new("TestFile4.lua"),File.new("TestFile5.lua"),File.new("TestFile6.lua")
	local Directory1,Directory2,Directory3 = Directory.new("Directory1"),Directory.new("Directory2"),Directory.new("Directory3")
	File1:SetStatus(NexusEnums.FileStatus.Created)
	File2:SetStatus(NexusEnums.FileStatus.Modified)
	File3:SetStatus(NexusEnums.FileStatus.Modified)
	File4:SetStatus(NexusEnums.FileStatus.Renamed)
	File5:SetStatus(NexusEnums.FileStatus.Deleted)
	File6:SetStatus(NexusEnums.FileStatus.Created)
	Directory2:AddFile(File3)
	Directory2:AddFile(File4)
	Directory2:AddFile(Directory3)
	Directory3:AddFile(File5)
	Directory3:AddFile(File6)
	
	--Filter the files.
	local FilteredFiles = Directory.FilterFiles({File1,File2,Directory1,Directory2},{NexusEnums.FileStatus.Modified,NexusEnums.FileStatus.Renamed})
	
	--Assert the files are correct.
	UnitTest:AssertEquals(FilteredFiles[1]:GetFileName(),"TestFile2.lua","File is incorrect.")
	UnitTest:AssertEquals(FilteredFiles[2]:GetFileName(),"Directory2","File is incorrect.")
	UnitTest:AssertEquals(FilteredFiles[2]:GetFiles()[1]:GetFileName(),"TestFile3.lua","File is incorrect.")
	UnitTest:AssertEquals(FilteredFiles[2]:GetFiles()[2]:GetFileName(),"TestFile4.lua","File is incorrect.")
end)

--[[
Tests the FromStrings method.
--]]
NexusUnitTesting:RegisterUnitTest("FromStrings",function(UnitTest)
	--Create a list of files.
	local FilesList = {
		"TestFile1.lua",
		"TestFile2.lua",
		"TestDirectory1/TestFile3.lua",
		"TestDirectory1/TestDirectory2/TestFile4.lua",
		"TestDirectory1/TestDirectory2/TestFile5.lua",
		"TestDirectory1/TestDirectory2/TestFile5.lua/TestFile6.lua",
	}
	
	--Create the list of files and assert they are correct.
	local CuTs = Directory.FromStrings(FilesList)
	UnitTest:AssertEquals(CuTs[1]:GetFileName(),"TestFile1.lua","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[2]:GetFileName(),"TestFile2.lua","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFileName(),"TestDirectory1","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFiles()[1]:GetFileName(),"TestFile3.lua","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFiles()[2]:GetFileName(),"TestDirectory2","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFiles()[2]:GetFiles()[1]:GetFileName(),"TestFile4.lua","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFiles()[2]:GetFiles()[2]:GetFileName(),"TestFile5.lua","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFiles()[2]:GetFiles()[3]:GetFileName(),"TestFile5.lua","File name is incorrect.")
	UnitTest:AssertEquals(CuTs[3]:GetFiles()[2]:GetFiles()[3]:GetFiles()[1]:GetFileName(),"TestFile6.lua","File name is incorrect.")
end)

--[[
Tests the GetFiles and GetFile methods.
--]]
NexusUnitTesting:RegisterUnitTest("GetFiles",function(UnitTest)
	--Create the components under testing.
	local CuT1 = Directory.new("DirectoryName1")
	local CuT2 = Directory.new("DirectoryName2")
	CuT1:AddFile(CuT2)
	
	--Create and add files.
	local File1,File2,File3 = File.new("TestFile1.lua"),File.new("TestFile2.lua"),File.new("TestFile3.lua")
	CuT1:AddFile(File1)
	CuT2:AddFile(File2)
	CuT2:AddFile(File3)
	
	--Assert the file lists are correct.
	UnitTest:AssertEquals(CuT1:GetFiles(),{CuT2,File1},"File list is incorrect.")
	UnitTest:AssertEquals(CuT2:GetFiles(),{File2,File3},"File list is incorrect.")
	
	--Assert the file searches are correct.
	UnitTest:AssertSame(CuT1:GetFile("DirectoryName2"),CuT2,"Directory not found.")
	UnitTest:AssertSame(CuT1:GetFile("TestFile1.lua"),File1,"File not found.")
	UnitTest:AssertNil(CuT1:GetFile("TestFile2.lua"),"File found.")
	UnitTest:AssertNil(CuT1:GetFile("TestFile3.lua"),"File found.")
	UnitTest:AssertSame(CuT2:GetFile("TestFile2.lua"),File2,"File not found.")
	UnitTest:AssertSame(CuT2:GetFile("TestFile3.lua"),File3,"File not found.")
end)

--[[
Tests the GetStatus method.
--]]
NexusUnitTesting:RegisterUnitTest("GetStatus",function(UnitTest)
	--Create the component under testing and several files.
	local CuT = Directory.new("DirectoryName")
	local File1,File2,File3,File4,File5 = File.new("TestFile1.lua"),File.new("TestFile2.lua"),File.new("TestFile3.lua"),File.new("TestFile4.lua"),File.new("TestFile5.lua")
	CuT:AddFile(File1)
	CuT:AddFile(File2)
	CuT:AddFile(File3)
	
	--Asserrt the directory status is correct.
	UnitTest:AssertTrue(NexusEnums.FileStatus.Untracked:Equals(CuT:GetStatus()),"Status is incorrect.")
	
	--Assert that unform file states change the state.
	File1:SetStatus(NexusEnums.FileStatus.Created)
	File2:SetStatus(NexusEnums.FileStatus.Created)
	File3:SetStatus(NexusEnums.FileStatus.Created)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Created:Equals(CuT:GetStatus()),"Status is incorrect.")
	File1:SetStatus(NexusEnums.FileStatus.Deleted)
	File2:SetStatus(NexusEnums.FileStatus.Deleted)
	File3:SetStatus(NexusEnums.FileStatus.Deleted)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Deleted:Equals(CuT:GetStatus()),"Status is incorrect.")
	File1:SetStatus(NexusEnums.FileStatus.Renamed)
	File2:SetStatus(NexusEnums.FileStatus.Renamed)
	File3:SetStatus(NexusEnums.FileStatus.Renamed)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Renamed:Equals(CuT:GetStatus()),"Status is incorrect.")
	
	--Assert that unmodified statuses are shown.
	File1:SetStatus(NexusEnums.FileStatus.Created)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Unmodified:Equals(CuT:GetStatus()),"Status is incorrect.")
	File1:SetStatus(NexusEnums.FileStatus.Deleted)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Unmodified:Equals(CuT:GetStatus()),"Status is incorrect.")
	File1:SetStatus(NexusEnums.FileStatus.Modified)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Unmodified:Equals(CuT:GetStatus()),"Status is incorrect.")
	File1:SetStatus(NexusEnums.FileStatus.Untracked)
	UnitTest:AssertTrue(NexusEnums.FileStatus.Unmodified:Equals(CuT:GetStatus()),"Status is incorrect.")
end)



return true