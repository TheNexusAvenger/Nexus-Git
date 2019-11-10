--[[
TheNexusAvenger

Tests the MultiHttpResponse class.
--]]

local NexusUnitTesting = require("NexusUnitTesting__")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local HttpResponse = NexusGit:GetResource("SplitHttp.HttpResponse")
local MultiHttpResponse = NexusGit:GetResource("SplitHttp.MultiHttpResponse")



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	MultiHttpResponse.new(3)
end)

--[[
Tests the AddPartialResponse method.
--]]
NexusUnitTesting:RegisterUnitTest("AddPartialResponse",function(UnitTest)
	--Create the component under testing.
	local CuT = MultiHttpResponse.new(3)
	
	--Create the responses.
	local Response1 = HttpResponse.FromJSON("{\"status\":\"success\",\"id\":0,\"currentPacket\":0,\"maxPackets\":3,\"packet\":\"Hello\"}")
	local Response2 = HttpResponse.FromJSON("{\"status\":\"success\",\"id\":0,\"currentPacket\":1,\"maxPackets\":3,\"packet\":\" wor\"}")
	local Response3 = HttpResponse.FromJSON("{\"status\":\"success\",\"id\":0,\"currentPacket\":2,\"maxPackets\":3,\"packet\":\"ld!\"}")
	
	--Assert adding the first response.
	CuT:AddPartialResponse(Response1)
	UnitTest:AssertFalse(CuT:IsComplete(),"Response is complete.")
	UnitTest:AssertErrors(function()
		CuT:GetResponse()
	end,"Response reading didn't throw an error.")	
	
	--Assert adding the second response.
	CuT:AddPartialResponse(Response2)
	UnitTest:AssertFalse(CuT:IsComplete(),"Response is complete.")
	UnitTest:AssertErrors(function()
		CuT:GetResponse()
	end,"Response reading didn't throw an error.")	
	
	--Assert re-adding the second response.
	CuT:AddPartialResponse(Response2)
	UnitTest:AssertFalse(CuT:IsComplete(),"Response is complete.")
	UnitTest:AssertErrors(function()
		CuT:GetResponse()
	end,"Response reading didn't throw an error.")	
	
	--Assert adding the final response.
	CuT:AddPartialResponse(Response3)
	UnitTest:AssertTrue(CuT:IsComplete(),"Response isn't complete.")
	UnitTest:AssertSame(CuT:GetResponse(),"Hello world!","Response is incorrect.")
end)



return true