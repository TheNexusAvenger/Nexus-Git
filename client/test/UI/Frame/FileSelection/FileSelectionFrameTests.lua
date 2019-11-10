--[[
TheNexusAvenger

Tests the FileSelectionFrame class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local FileSelectionFrame = NexusGit:GetResource("UI.Frame.FileSelection.FileSelectionFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = FileSelectionFrame.new({File.new("TestFileName1.lua"),File.new("TestFileName2.lua")})
	UnitTest:AssertEquals(CuT.ClassName,"FileSelectionFrame","Class name is incorrect.")
	UnitTest:AssertEquals(#CuT:GetChildren(),2,"List frames are incorrect.")
end)

--[[
Asserts that the canvas size is correct.
--]]
NexusUnitTesting:RegisterUnitTest("CanvasSize",function(UnitTest)
	--Create several files and directories.
	local File1 = File.new("TestFileName1.lua")
	local File2 = File.new("TestFileName2.lua")
	local File3 = File.new("TestFileName3.lua")
	local File4 = File.new("TestFileName4.lua")
	local Directory1 = Directory.new("TestDirectory1")
	local Directory2 = Directory.new("TestDirectory2")
	local Directory3 = Directory.new("TestDirectory3")
	Directory1:AddFile(File2)
	Directory1:AddFile(Directory2)
	Directory2:AddFile(File3)
	Directory3:AddFile(File4)
	
	--Create the component under testing.
	local CuT = FileSelectionFrame.new({File1,Directory1,Directory3})
	UnitTest:AssertEquals(#CuT:GetChildren(),3,"List frame count is incorrect.")
	UnitTest:AssertEquals(CuT.CanvasSize.Y.Offset,140,"Canvas size Y is incorrect.")
end)

--[[
Tests the GetSelectedFiles method.
--]]
NexusUnitTesting:RegisterUnitTest("GetSelectedFiles",function(UnitTest)
	--Create several files and directories.
	local File1 = File.new("TestFileName1.lua")
	local File2 = File.new("TestFileName2.lua")
	local File3 = File.new("TestFileName3.lua")
	local File4 = File.new("TestFileName4.lua")
	local Directory1 = Directory.new("TestDirectory1")
	local Directory2 = Directory.new("TestDirectory2")
	local Directory3 = Directory.new("TestDirectory3")
	Directory1:AddFile(File2)
	Directory1:AddFile(Directory2)
	Directory2:AddFile(File3)
	Directory3:AddFile(File4)
	
	--Create the component under testing.
	local CuT = FileSelectionFrame.new({File1,Directory1,Directory3})
	CuT.ListFrames[2]:GetCollapsableContainer():GetChildren()[2]:GetCollapsableContainer():GetChildren()[1].CheckedState = NexusEnums.CheckBoxState.Checked
	CuT.ListFrames[3].CheckedState = NexusEnums.CheckBoxState.Checked
	
	--Assert the selected files are correct.
	local SelectedFiles = CuT:GetSelectedFiles()
	UnitTest:AssertEquals(#SelectedFiles,2,"Incorrect amount of files selected.")
	UnitTest:AssertEquals(SelectedFiles[1]:GetFileName(),"TestDirectory1","File name is incorrect.")
	UnitTest:AssertEquals(SelectedFiles[1]:GetFiles()[1]:GetFileName(),"TestDirectory2","File name is incorrect.")
	UnitTest:AssertEquals(SelectedFiles[1]:GetFiles()[1]:GetFiles()[1]:GetFileName(),"TestFileName3.lua","File name is incorrect.")
	UnitTest:AssertEquals(SelectedFiles[2]:GetFileName(),"TestDirectory3","File name is incorrect.")
	UnitTest:AssertEquals(SelectedFiles[2]:GetFiles()[1]:GetFileName(),"TestFileName4.lua","File name is incorrect.")
end)



return true