--[[
TheNexusAvenger

Tests the MessageView class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local MessageView = NexusGit:GetResource("UI.View.Frame.MessageView")



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	local CuT = MessageView.new("Test Message")
	UnitTest:AssertEquals(CuT.ClassName,"MessageView","Class name is incorrect.")
end)

--[[
Tests the YieldForClose method.
--]]
NexusUnitTesting:RegisterUnitTest("YieldForClose",function(UnitTest)
	--Create the component under testing.
	local CuT = MessageView.new("Test Message")
	
	--Close the frame.
	delay(0.1,function()
		CuT:Close()
	end)
	
	--Yield for it to close (test won't pass until it ends).
	CuT:YieldForClose()
end)



return true