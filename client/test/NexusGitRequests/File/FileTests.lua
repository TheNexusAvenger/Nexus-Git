--[[
TheNexusAvenger

Tests the File class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = File.new("TestFileName.lua")
	UnitTest:AssertEquals(CuT.ClassName,"File","Class name is incorrect.")
	UnitTest:AssertEquals(CuT:GetFileName(),"TestFileName.lua","File name is incorrect.")
	UnitTest:AssertTrue(NexusEnums.FileStatus.Untracked:Equals(CuT:GetStatus()),"Default status is incorrect.")
end)

--[[
Tests the GetStatus method.
--]]
NexusUnitTesting:RegisterUnitTest("GetStatus",function(UnitTest)
	--Create the component under testing.
	local CuT = File.new("TestFileName.lua")
	
	--Assert setting the statuses.
	CuT:SetStatus(NexusEnums.FileStatus.Created)
	UnitTest:AssertEquals(CuT:GetStatus(),NexusEnums.FileStatus.Created,"Status is incorrect.")
	CuT:SetStatus(NexusEnums.FileStatus.Deleted)
	UnitTest:AssertEquals(CuT:GetStatus(),NexusEnums.FileStatus.Deleted,"Status is incorrect.")
	CuT:SetStatus(NexusEnums.FileStatus.Modified)
	UnitTest:AssertEquals(CuT:GetStatus(),NexusEnums.FileStatus.Modified,"Status is incorrect.")
	CuT:SetStatus(NexusEnums.FileStatus.Renamed)
	UnitTest:AssertEquals(CuT:GetStatus(),NexusEnums.FileStatus.Renamed,"Status is incorrect.")
	CuT:SetStatus(NexusEnums.FileStatus.Untracked)
	UnitTest:AssertEquals(CuT:GetStatus(),NexusEnums.FileStatus.Untracked,"Status is incorrect.")
end)



return true