--[[
TheNexusAvenger

Tests the PartialHttpResponseRequest class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local PartialHttpResponseRequest = NexusGit:GetResource("SplitHttp.PartialHttpResponseRequest")



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	PartialHttpResponseRequest.new("http://localhost:8000",0,2)
end)

--[[
Tests the FormatURL method.
--]]
NexusUnitTesting:RegisterUnitTest("FormatURL",function(UnitTest)
	--Create the components under testing.
	local CuT1 = PartialHttpResponseRequest.new("http://localhost:8000",1,2)
	local CuT2 = PartialHttpResponseRequest.new("http://localhost:8000?parameter=true",1,2)
	
	--Assert the URLs are formatted correctly.
	UnitTest:AssertEquals(CuT1:FormatURL(),"http://localhost:8000?getResponse=true&responseId=1&packet=2","URL is incorrect.")
	UnitTest:AssertEquals(CuT2:FormatURL(),"http://localhost:8000?parameter=true&getResponse=true&responseId=1&packet=2","URL is incorrect.")
end)



return true