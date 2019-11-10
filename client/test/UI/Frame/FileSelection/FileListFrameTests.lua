--[[
TheNexusAvenger

Tests the FileListFrame class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local FileListFrame = NexusGit:GetResource("UI.Frame.FileSelection.FileListFrame")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = FileListFrame.new(File.new("TestFileName.lua"))
	UnitTest:AssertEquals(CuT.ClassName,"FileListFrame","Class name is incorrect.")
	UnitTest:AssertEquals(CuT:GetWrappedInstance().Name,"TestFileName.lua","Name is incorrect.")
	UnitTest:AssertEquals(#CuT:GetChildren(),0,"Contents aren't hidden.")
	UnitTest:AssertEquals(#CuT:GetMainContainer():GetChildren(),0,"Contents aren't hidden.")
	UnitTest:AssertEquals(#CuT:GetCollapsableContainer():GetChildren(),0,"Contents aren't hidden.")
	
	CuT = FileListFrame.new(Directory.new("TestFileName.lua"))
	UnitTest:AssertEquals(CuT.ClassName,"FileListFrame","Class name is incorrect.")
	UnitTest:AssertEquals(CuT:GetWrappedInstance().Name,"TestFileName.lua","Name is incorrect.")
	UnitTest:AssertEquals(#CuT:GetChildren(),0,"Contents aren't hidden.")
	UnitTest:AssertEquals(#CuT:GetMainContainer():GetChildren(),0,"Contents aren't hidden.")
	UnitTest:AssertEquals(#CuT:GetCollapsableContainer():GetChildren(),0,"Contents aren't hidden.")
end)

--[[
Tests the FromFile method.
--]]
NexusUnitTesting:RegisterUnitTest("FromFile",function(UnitTest)
	--Create the directories and files.
	local Directory1 = Directory.new("TestDirectory1")
	local Directory2 = Directory.new("TestDirectory2")
	local File1 = File.new("TestFile1")
	local File2 = File.new("TestFile2")
	local File3 = File.new("TestFile3")
	Directory1:AddFile(Directory2)
	Directory1:AddFile(File1)
	Directory1:AddFile(File2)
	Directory2:AddFile(File3)
	
	--Create the list frame and assert it was set up correctly.
	local CuT = FileListFrame.FromFile(Directory1)
	UnitTest:AssertSame(CuT.File,Directory1,"File is incorrect.")
	UnitTest:AssertTrue(CuT.Expanded,"List frame isn't expanded.")
	UnitTest:AssertEquals(#CuT:GetCollapsableContainer():GetChildren(),3,"Child frame count is incorrect.")
	
	--Assert the children are correct.
	UnitTest:AssertSame(CuT:GetCollapsableContainer():GetChildren()[1].File,Directory2,"File is incorrect.")
	UnitTest:AssertSame(CuT:GetCollapsableContainer():GetChildren()[2].File,File1,"File is incorrect.")
	UnitTest:AssertSame(CuT:GetCollapsableContainer():GetChildren()[3].File,File2,"File is incorrect.")
	UnitTest:AssertSame(CuT:GetCollapsableContainer():GetChildren()[1]:GetCollapsableContainer():GetChildren()[1].File,File3,"File is incorrect.")
end)

--[[
Tests that changes to and from the check box are replicated.
--]]
NexusUnitTesting:RegisterUnitTest("CheckedReplication",function(UnitTest)
	local CuT = FileListFrame.new(File.new("TestFileName.lua"))
	local CheckBox = CuT.CheckBox
	
	--Assert changes are replicated to the checkbox.
	CuT.CheckedState = NexusEnums.CheckBoxState.Checked
	UnitTest:AssertEquals(CheckBox.BoxState,NexusEnums.CheckBoxState.Checked,"Checked state not replicated.")
	
	--Assert changes are replicated from the checkbox.
	CheckBox.BoxState = NexusEnums.CheckBoxState.Unchecked
	UnitTest:AssertEquals(CuT.CheckedState,NexusEnums.CheckBoxState.Unchecked,"Checked state not replicated.")
end)

--[[
Tests that a structure is properly replicationed.
--]]
NexusUnitTesting:RegisterUnitTest("StructureReplication",function(UnitTest)
	--Create several components under testing.
	local CuT1 = FileListFrame.new(Directory.new("TestFileName1.lua"))
	local CuT2 = FileListFrame.new(Directory.new("TestFileName2.lua"))
	local CuT3 = FileListFrame.new(File.new("TestFileName3.lua"))
	local CuT4 = FileListFrame.new(File.new("TestFileName4.lua"))
	local CuT5 = FileListFrame.new(File.new("TestFileName5.lua"))
	CuT3.CheckedState = NexusEnums.CheckBoxState.Checked
	CuT4.CheckedState = NexusEnums.CheckBoxState.Checked
	CuT2.Parent = CuT1:GetCollapsableContainer()
	CuT3.Parent = CuT1:GetCollapsableContainer()
	CuT4.Parent = CuT2:GetCollapsableContainer()
	
	--Assert the checked states are correct.
	UnitTest:AssertEquals(tostring(CuT1.CheckedState),tostring(NexusEnums.CheckBoxState.Checked),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT2.CheckedState),tostring(NexusEnums.CheckBoxState.Checked),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT3.CheckedState),tostring(NexusEnums.CheckBoxState.Checked),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT4.CheckedState),tostring(NexusEnums.CheckBoxState.Checked),"Checked state is incorrect.")
	
	--Assert adding a new unchecked list frame changes the states.
	CuT5.Parent = CuT2:GetCollapsableContainer()
	UnitTest:AssertEquals(tostring(CuT1.CheckedState),tostring(NexusEnums.CheckBoxState.Mixed),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT2.CheckedState),tostring(NexusEnums.CheckBoxState.Mixed),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT3.CheckedState),tostring(NexusEnums.CheckBoxState.Checked),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT4.CheckedState),tostring(NexusEnums.CheckBoxState.Checked),"Checked state is incorrect.")
	UnitTest:AssertEquals(tostring(CuT5.CheckedState),tostring(NexusEnums.CheckBoxState.Unchecked),"Checked state is incorrect.")
	
	--Assert checking a lower list frame replicates upward.
	CuT5.CheckedState = NexusEnums.CheckBoxState.Checked
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Checked:Equals(CuT1.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Checked:Equals(CuT2.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Checked:Equals(CuT3.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Checked:Equals(CuT4.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Checked:Equals(CuT5.CheckedState),"Checked state is incorrect.")
	
	--Assert checking an upper list frame replicates downward.
	CuT2.CheckedState = NexusEnums.CheckBoxState.Unchecked
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Mixed:Equals(CuT1.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT2.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Checked:Equals(CuT3.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT4.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT5.CheckedState),"Checked state is incorrect.")
	
	--Assert removing a list frame changes the list frame.
	CuT3:Destroy()
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT1.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT2.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT4.CheckedState),"Checked state is incorrect.")
	UnitTest:AssertTrue(NexusEnums.CheckBoxState.Unchecked:Equals(CuT5.CheckedState),"Checked state is incorrect.")
end)

--[[
Tests the GetBoundingSizeX method.
--]]
NexusUnitTesting:RegisterUnitTest("GetBoundingSizeX",function(UnitTest)
	--Set up the ScreenGui to make TextBounds work.
	local ScreenGui = Instance.new("ScreenGui")
	ScreenGui.Parent = game:GetService("Lighting")
	delay(0.5,function()
		if ScreenGui.Parent then
			ScreenGui:Destroy()
		end
	end)
	
	--Create several components under testing.
	local CuT1 = FileListFrame.new(Directory.new("TestFileName.lua"))
	CuT1.Parent = ScreenGui
	local CuT2 = FileListFrame.new(Directory.new("TestFileName.lua"))
	local CuT3 = FileListFrame.new(File.new("TestFileName.lua"))
	local CuT4 = FileListFrame.new(File.new("TestFileName.lua"))
	local CuT5 = FileListFrame.new(File.new("TestFileName.lua"))
	
	
	--Assert the bounding size is correct.
	local BaseBoundingSizeX = CuT1:GetBoundingSizeX()
	UnitTest:AssertTrue(BaseBoundingSizeX > 22,"Bounding size is below 20 pixels.")
	
	--Assert adding a child frame changes the bounding size.
	CuT2.Parent = CuT1:GetCollapsableContainer()
	UnitTest:AssertEquals(CuT1:GetBoundingSizeX(),BaseBoundingSizeX + 20,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT2:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	
	--Assert adding a child frame changes the bounding size.
	CuT3.Parent = CuT2:GetCollapsableContainer()
	UnitTest:AssertEquals(CuT1:GetBoundingSizeX(),BaseBoundingSizeX + 40,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT2:GetBoundingSizeX(),BaseBoundingSizeX + 20,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT3:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	
	--Assert adding a child frame of the same width doesn't change the bounding size.
	CuT4.Parent = CuT2:GetCollapsableContainer()
	UnitTest:AssertEquals(CuT1:GetBoundingSizeX(),BaseBoundingSizeX + 40,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT2:GetBoundingSizeX(),BaseBoundingSizeX + 20,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT3:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT4:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	
	--Assert adding a child frame of the same width doesn't change the bounding size.
	CuT5.Parent = CuT1:GetCollapsableContainer()
	UnitTest:AssertEquals(CuT1:GetBoundingSizeX(),BaseBoundingSizeX + 40,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT2:GetBoundingSizeX(),BaseBoundingSizeX + 20,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT3:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT4:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	UnitTest:AssertEquals(CuT5:GetBoundingSizeX(),BaseBoundingSizeX,"Bounding size is incorrect.")
	
	--Destroy the ScreenGui.
	ScreenGui:Destroy()
end)

--[[
Tests the GetSelectedFiles method.
--]]
NexusUnitTesting:RegisterUnitTest("GetSelectedFiles",function(UnitTest)
	--Create several components under testing.
	local CuT1 = FileListFrame.new(Directory.new("TestFileName1.lua"))
	local CuT2 = FileListFrame.new(Directory.new("TestFileName2.lua"))
	local CuT3 = FileListFrame.new(File.new("TestFileName3.lua"))
	local CuT4 = FileListFrame.new(File.new("TestFileName4.lua"))
	local CuT5 = FileListFrame.new(File.new("TestFileName5.lua"))
	CuT4.CheckedState = NexusEnums.CheckBoxState.Checked
	CuT2.Parent = CuT1:GetCollapsableContainer()
	CuT3.Parent = CuT1:GetCollapsableContainer()
	CuT4.Parent = CuT2:GetCollapsableContainer()
	CuT5.Parent = CuT1:GetCollapsableContainer()
	
	--Assert the right files are returned.
	local SelectedFile = CuT1:GetSelectedFiles()
	UnitTest:AssertEquals(#SelectedFile:GetFiles(),1,"Incorrect amount of files selected.")
	UnitTest:AssertEquals(SelectedFile:GetFileName(),"TestFileName1.lua","Directory name is incorrect.")
	UnitTest:AssertEquals(SelectedFile:GetFiles()[1]:GetFileName(),"TestFileName2.lua","Directory name is incorrect.")
	UnitTest:AssertEquals(SelectedFile:GetFiles()[1]:GetFiles()[1]:GetFileName(),"TestFileName4.lua","File name is incorrect.")
	
	--Assert unselecting the root list makes the selected files nil.
	CuT1.CheckedState = NexusEnums.CheckBoxState.Unchecked
	UnitTest:AssertNil(CuT1:GetSelectedFiles(),"File was selected.")
end)

--[[
Tests that the bounding size is correct.
--]]
NexusUnitTesting:RegisterUnitTest("ContentsBoundingSize",function(UnitTest)
	--Create several components under testing.
	local CuT1 = FileListFrame.new(Directory.new("TestFileName1.lua"))
	local CuT2 = FileListFrame.new(Directory.new("TestFileName2.lua"))
	local CuT3 = FileListFrame.new(File.new("TestFileName3.lua"))
	local CuT4 = FileListFrame.new(File.new("TestFileName4.lua"))
	local CuT5 = FileListFrame.new(File.new("TestFileName5.lua"))
	CuT4.CheckedState = NexusEnums.CheckBoxState.Checked
	CuT2.Parent = CuT1:GetCollapsableContainer()
	CuT3.Parent = CuT1:GetCollapsableContainer()
	CuT4.Parent = CuT2:GetCollapsableContainer()
	CuT5.Parent = CuT1:GetCollapsableContainer()
	
	--Assert the bounding size is correct.
	UnitTest:AssertEquals(CuT1:GetWrappedInstance().AbsoluteSize.Y,100,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT2:GetWrappedInstance().AbsoluteSize.Y,40,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT3:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT4:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT5:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	
	--Assert collapsing an inner frame changes the sizes.
	CuT2.Expanded = false
	UnitTest:AssertEquals(CuT1:GetWrappedInstance().AbsoluteSize.Y,80,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT2:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT3:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT4:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT5:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	
	--Assert collapsing the outer frame changes the size.
	CuT1.Expanded = false
	UnitTest:AssertEquals(CuT1:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT2:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT3:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT4:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
	UnitTest:AssertEquals(CuT5:GetWrappedInstance().AbsoluteSize.Y,20,"Bounding size isn't correct.")
end)



return true