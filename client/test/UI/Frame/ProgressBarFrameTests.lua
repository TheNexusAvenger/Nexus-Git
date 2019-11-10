--[[
TheNexusAvenger

Tests the ProgressBarFrame class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local ProgressBarFrame = NexusGit:GetResource("UI.Frame.ProgressBarFrame")



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = ProgressBarFrame.new()
	UnitTest:AssertEquals(CuT.ClassName,"ProgressBarFrame","Class name is incorrect.")
	UnitTest:AssertEquals(CuT.FillAmount,0,"Progress bar is initially filled.")
	UnitTest:AssertEquals(#CuT:GetChildren(),0,"Fill frame isn't hidden.")
end)

--[[
Tests the FillAmount property.
--]]
NexusUnitTesting:RegisterUnitTest("FillAmount",function(UnitTest)
	--Create the component under testing.
	local CuT = ProgressBarFrame.new()
	local FillBar = CuT:GetWrappedInstance():GetChildren()[1]
	
	--Set the fill amoutn and assert the size is correct.
	CuT.FillAmount = 0.2
	UnitTest:AssertEquals(FillBar.Size,UDim2.new(0.2,0,1,0))
	CuT.FillAmount = 1
	UnitTest:AssertEquals(FillBar.Size,UDim2.new(1,0,1,0))
	CuT.FillAmount = 0
	UnitTest:AssertEquals(FillBar.Size,UDim2.new(0,0,1,0))
end)

--[[
Tests the SetAsNormal and SetAsFailed methods.
--]]
NexusUnitTesting:RegisterUnitTest("SetAsNormalAndSetAsFailed",function(UnitTest)
	--Create the component under testing.
	local CuT = ProgressBarFrame.new()
	local FillBar = CuT:GetWrappedInstance():GetChildren()[1]
	local BaseFillColor,BaseActualFillColor = CuT.FillColor3,FillBar.BackgroundColor3
	
	--Set the progress bar as failed and assert the color changed.
	CuT:SetAsFailed()
	UnitTest:AssertNotEquals(CuT.FillColor3,BaseFillColor,"Color was not changed.")
	UnitTest:AssertNotEquals(FillBar.BackgroundColor3,BaseActualFillColor,"Color was not changed.")
	
	--Set the progress bar as normal and assert the color changed.
	CuT:SetAsNormal()
	UnitTest:AssertEquals(CuT.FillColor3,BaseFillColor,"Color was not reverted.")
	UnitTest:AssertEquals(FillBar.BackgroundColor3,BaseActualFillColor,"Color was not reverted.")
end)



return true