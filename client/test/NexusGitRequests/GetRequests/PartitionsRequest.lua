--[[
TheNexusAvenger

Tests the PartitionsRequest class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")

local Root = game:GetService("ServerStorage"):WaitForChild("NexusGit")
local NexusGitRequests = Root:WaitForChild("NexusGitRequests")
local GetRequests = NexusGitRequests:WaitForChild("GetRequests")

local PartitionsRequest = require(GetRequests:WaitForChild("PartitionsRequest"))



--[[
Tests the GetInstancePath method.
--]]
NexusUnitTesting:RegisterUnitTest("GetInstancePath",function(UnitTest)
	local Script = game:GetService("ServerStorage"):WaitForChild("NexusGitTests"):WaitForChild("NexusGitRequests"):WaitForChild("GetRequests"):WaitForChild("PartitionsRequest")
	
	UnitTest:AssertEquals(PartitionsRequest:GetInstancePath(Script:GetFullName()),Script,"Reference is incorrect.")
	UnitTest:AssertEquals(PartitionsRequest:GetInstancePath("game."..Script:GetFullName()),Script,"Reference is incorrect.")
	UnitTest:AssertNil(PartitionsRequest:GetInstancePath("ReplicatedStorage.NexusGit2.NexusGitRequests"),"Reference exists.")
end)



return true