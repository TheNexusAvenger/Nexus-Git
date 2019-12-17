--[[
TheNexusAvenger

Tests the PartialHttpRequest class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")
local DependencyInjector = NexusUnitTesting.Util.DependencyInjector

local Root = game:GetService("ServerStorage"):WaitForChild("NexusGit")
local SplitHttp = Root:WaitForChild("SplitHttp")
local NexusGit = require(Root)
local PartialHttpRequest = NexusGit:GetResource("SplitHttp.PartialHttpRequest")

local HttpService = game:GetService("HttpService")



--[[
Tests that the constructor works without failing.
--]]
NexusUnitTesting:RegisterUnitTest("Constructor",function(UnitTest)
	PartialHttpRequest.new("GET","http://localhost:8000")
	PartialHttpRequest.new("POST","http://localhost:8000","Body")
	PartialHttpRequest.new("POST","http://localhost:8000","Body",12,1,2)
end)

--[[
Tests the FormatURL method.
--]]
NexusUnitTesting:RegisterUnitTest("FormatURL",function(UnitTest)
	--Create the components under testing.
	local CuT1 = PartialHttpRequest.new("GET","http://localhost:8000")
	local CuT2 = PartialHttpRequest.new("POST","http://localhost:8000","Body",nil,2,5)
	local CuT3 = PartialHttpRequest.new("POST","http://localhost:8000","Body",12,1,2)
	local CuT4 = PartialHttpRequest.new("POST","http://localhost:8000?parameter=true","Body",12,1,2)
	
	--Assert the URLs are formatted correctly.
	UnitTest:AssertEquals(CuT1:FormatURL(),"http://localhost:8000?packet=0&maxPackets=1","URL is incorrect.")
	UnitTest:AssertEquals(CuT2:FormatURL(),"http://localhost:8000?packet=2&maxPackets=5","URL is incorrect.")
	UnitTest:AssertEquals(CuT3:FormatURL(),"http://localhost:8000?requestId=12&packet=1&maxPackets=2","URL is incorrect.")
	UnitTest:AssertEquals(CuT4:FormatURL(),"http://localhost:8000?parameter=true&requestId=12&packet=1&maxPackets=2","URL is incorrect.")
end)

--[[
Tests the SendRequest method.
--]]
NexusUnitTesting:RegisterUnitTest("SendRequest",function(UnitTest)
	--Create the mock HttpService.
	local HttpServiceInjector = DependencyInjector.CreateOverrider()
	HttpServiceInjector:WhenCalled("GetAsync"):ThenCall(function(_,URL)
		if URL == "http://localhost:8000?requestId=3&packet=0&maxPackets=3" or URL == "http://localhost:8000?requestId=3&packet=1&maxPackets=3" then
			return "{\"status\":\"incomplete\",\"id\":3}"
		elseif URL == "http://localhost:8000?requestId=3&packet=2&maxPackets=3" then
			return "{\"status\":\"success\",\"id\":12,\"currentPacket\":0,\"maxPackets\":2,\"packet\":\"Hello world!\"}"
		end
	end)
	HttpServiceInjector:WhenCalled("JSONDecode"):ThenCall(function(_,JSON)
		return HttpService:JSONDecode(JSON)
	end)
	local FakeHttpService = DependencyInjector.Inject({},HttpServiceInjector)
	
	--Create the mock game.
	local GameInjector = DependencyInjector.CreateOverrider()
	GameInjector:WhenIndexed("GetService"):ThenReturn(function(_,Index)
		if Index == "HttpService" then
			return FakeHttpService
		end
	end)
	local FakeGame = DependencyInjector.Inject({},GameInjector)
	
	--Create the main injector.
	local Injector = DependencyInjector.CreateOverrider()
	Injector:WhenIndexed("game"):ThenReturn(FakeGame)
	
	--Require the injected module.
	local PartialHttpRequest = DependencyInjector.Require(SplitHttp:WaitForChild("PartialHttpRequest"),Injector)
	
	--Create the components under testing.
	local CuT1 = PartialHttpRequest.new("GET","http://localhost:8000","Body",3,0,3)
	local CuT2 = PartialHttpRequest.new("GET","http://localhost:8000","Body",3,1,3)
	local CuT3 = PartialHttpRequest.new("GET","http://localhost:8000","Body",3,2,3)
	
	--Get the responses.
	local Response1 = CuT1:SendRequest()
	local Response2 = CuT2:SendRequest()
	local Response3 = CuT3:SendRequest()
	
	--Assert the responses are correct.
	UnitTest:AssertEquals(Response1.Status,"Incomplete","Status is incomplete.")
	UnitTest:AssertEquals(Response2.Status,"Incomplete","Status is incomplete.")
	UnitTest:AssertEquals(Response3.Status,"Success","Status is incomplete.")
end)



return true