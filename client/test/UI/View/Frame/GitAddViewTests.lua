--[[
TheNexusAvenger

Tests the GitAddView class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")
local DependencyInjector = NexusUnitTesting.Util.DependencyInjector

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local File = NexusGit:GetResource("NexusGitRequest.File.File")
local GitAddView = NexusGit:GetResource("UI.View.Frame.GitAddView")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = GitAddView.new({File.new("TestFileName.lua")},"http://localhost.com:8000")
	UnitTest:AssertEquals(CuT.ClassName,"GitAddView","Class name is incorrect.")
end)

--[[
Tests the AddFiles method.
--]]
NexusUnitTesting:RegisterUnitTest("AddFiles",function(UnitTest)
	--Create several files.
	local Directory1,Directory2 = Directory.new("TestDirectory1"),Directory.new("TestDirectory1")
	local File1,File2,File3 = File.new("TestFileName1.lua"),File.new("TestFileName2.lua"),File.new("TestFileName3.lua")
	local File4,File5,File6 = File.new("TestFileName4.lua"),File.new("TestFileName5.lua"),File.new("TestFileName6.lua")
	Directory1:AddFile(File3)
	Directory1:AddFile(File4)
	Directory2:AddFile(File5)
	Directory2:AddFile(File6)
	File1:SetStatus(NexusEnums.FileStatus.Untracked)
	File2:SetStatus(NexusEnums.FileStatus.Modified)
	File3:SetStatus(NexusEnums.FileStatus.Untracked)
	File4:SetStatus(NexusEnums.FileStatus.Modified)
	File5:SetStatus(NexusEnums.FileStatus.Created)
	File6:SetStatus(NexusEnums.FileStatus.Modified)
	
	--Create a mock class.
	local MockGitAddView = GitAddView:Extend()
	local AddsCalled,StatusBarUpdatesCalled = {},{}
	function MockGitAddView:SendAddRequest(Files)
		for _,String in pairs(Files) do
			AddsCalled[String] = true
		end
	end
	function MockGitAddView:UpdateAddStatus(Percent)
		table.insert(StatusBarUpdatesCalled,Percent)
	end
	
	--Create the component under testing.
	local CuT = MockGitAddView.new({File1,File2,Directory1,Directory2})
	for _,ListFrame in pairs(CuT.ListFrame:GetChildren()) do
		ListFrame.CheckedState = NexusEnums.CheckBoxState.Checked
	end
	CuT:AddFiles(CuT.ListFrame:GetSelectedFiles())
	
	--Assert the methods were called correctly.
	UnitTest:AssertEquals(AddsCalled["TestFileName1.lua"],true,"File was not added.")
	UnitTest:AssertEquals(AddsCalled["TestFileName2.lua"],nil,"File was added.")
	UnitTest:AssertEquals(AddsCalled["TestDirectory1/TestFileName3.lua"],true,"File was not added.")
	UnitTest:AssertEquals(AddsCalled["TestDirectory1/TestFileName4.lua"],nil,"File was added.")
	UnitTest:AssertEquals(AddsCalled["TestDirectory1/TestDirectory2/TestFileName5.lua"],nil,"File was added.")
	UnitTest:AssertEquals(AddsCalled["TestDirectory1/TestDirectory2/TestFileName6.lua"],nil,"File was added.")
	UnitTest:AssertEquals(StatusBarUpdatesCalled,{0,0.5,true},"Progress bar was not called correctly.")
end)

--[[
Tests the AddFiles method with an error.
--]]
NexusUnitTesting:RegisterUnitTest("AddFilesWithError",function(UnitTest)
	--Silence the warnings.
	local Injector = DependencyInjector.CreateOverrider()
	Injector:WhenCalled("warn"):ThenReturn(nil)
	local GitAddView = DependencyInjector.Require(NexusGit:GetObjectReference("UI.View.Frame.GitAddView"),Injector)
	
	--Create several files.
	local Directory1,Directory2 = Directory.new("TestDirectory1"),Directory.new("TestDirectory1")
	local File1,File2,File3 = File.new("TestFileName1.lua"),File.new("TestFileName2.lua"),File.new("TestFileName3.lua")
	local File4,File5,File6 = File.new("TestFileName4.lua"),File.new("TestFileName5.lua"),File.new("TestFileName6.lua")
	Directory1:AddFile(File3)
	Directory1:AddFile(File4)
	Directory2:AddFile(File5)
	Directory2:AddFile(File6)
	File1:SetStatus(NexusEnums.FileStatus.Untracked)
	File2:SetStatus(NexusEnums.FileStatus.Modified)
	File3:SetStatus(NexusEnums.FileStatus.Untracked)
	File4:SetStatus(NexusEnums.FileStatus.Modified)
	File5:SetStatus(NexusEnums.FileStatus.Created)
	File6:SetStatus(NexusEnums.FileStatus.Modified)
	
	--Create a mock class.
	local MockGitAddView = GitAddView:Extend()
	local StatusBarUpdatesCalled = {}
	function MockGitAddView:SendAddRequest(Files)
		for _,String in pairs(Files) do
			if String == "TestDirectory1/TestFileName3.lua" then
				error("Mock network error.")
			end
		end
	end
	function MockGitAddView:UpdateAddStatus(Percent)
		table.insert(StatusBarUpdatesCalled,Percent)
	end
	
	--Create the component under testing.
	local CuT = MockGitAddView.new({File1,File2,Directory1,Directory2})
	for _,ListFrame in pairs(CuT.ListFrame:GetChildren()) do
		ListFrame.CheckedState = NexusEnums.CheckBoxState.Checked
	end
	CuT:AddFiles(CuT.ListFrame:GetSelectedFiles())
	
	--Assert the loading bar was updated corretly.
	UnitTest:AssertEquals(StatusBarUpdatesCalled,{0,0.5,false},"Progress bar was not called correctly.")
end)



return true