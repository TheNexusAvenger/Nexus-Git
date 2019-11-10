--[[
TheNexusAvenger

Tests the ProgressBarView class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")
local DependencyInjector = NexusUnitTesting.Util.DependencyInjector

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local ProgressBarView = NexusGit:GetResource("UI.View.Frame.ProgressBarView")



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = ProgressBarView.new()
	UnitTest:AssertEquals(CuT.ClassName,"ProgressBarView","Class name is incorrect.")
end)

--[[
Tests the SetProgressBarFill method.
--]]
NexusUnitTesting:RegisterUnitTest("SetProgressBarFill",function(UnitTest)
	--Create the component under testing.
	local CuT = ProgressBarView.new()
	
	--Set the fill and assert the value is correct.
	CuT:SetProgressBarFill(0.4)
	UnitTest:AssertEquals(CuT.ProgressBar.FillAmount,0.4,"Fill is incorrect.")
end)

--[[
Tests the SetText method.
--]]
NexusUnitTesting:RegisterUnitTest("SetText",function(UnitTest)
	--Create the component under testing.
	local CuT = ProgressBarView.new()
	
	--Set the text and assert the value is correct.
	CuT:SetText("Message")
	UnitTest:AssertEquals(CuT.MessageLabel.Text,"Message","Text is incorrect.")
end)





return true