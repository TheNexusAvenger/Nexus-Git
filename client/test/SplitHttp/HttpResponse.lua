--[[
TheNexusAvenger

Tests the HttpResponse class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")
local DependencyInjector = NexusUnitTesting.Util.DependencyInjector

local Root = game:GetService("ServerStorage"):WaitForChild("NexusGit")
local SplitHttp = Root:WaitForChild("SplitHttp")

local HttpResponse = require(SplitHttp:WaitForChild("HttpResponse"))



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	HttpResponse.new("Test response")
end)

--[[
Tests the FormatURL method.
--]]
NexusUnitTesting:RegisterUnitTest("FormatURL",function(UnitTest)
	--Create the components under testing.
	local CuT1 = HttpResponse.FromJSON("{\"status\":\"incomplete\",\"id\":2}")
	local CuT2 = HttpResponse.FromJSON("{\"status\":\"success\",\"id\":0,\"currentPacket\":2,\"maxPackets\":3,\"packet\":\"Test\"}")
	
	--Assert the responses are correct.
	UnitTest:AssertEquals(CuT1:GetResponse(),"{\"status\":\"incomplete\",\"id\":2}","Parameter is incorrect.")
	UnitTest:AssertEquals(CuT1.Status,"Incomplete","Parameter is incorrect.")
	UnitTest:AssertEquals(CuT2:GetResponse(),"{\"status\":\"success\",\"id\":0,\"currentPacket\":2,\"maxPackets\":3,\"packet\":\"Test\"}","Parameter is incorrect.")
	UnitTest:AssertEquals(CuT2.Status,"Success","Parameter is incorrect.")
	UnitTest:AssertEquals(CuT2.Id,0,"Parameter is incorrect.")
	UnitTest:AssertEquals(CuT2.CurrentPacket,2,"Parameter is incorrect.")
	UnitTest:AssertEquals(CuT2.MaxPackets,3,"Parameter is incorrect.")
	UnitTest:AssertEquals(CuT2.Packet,"Test","Parameter is incorrect.")
end)



return true