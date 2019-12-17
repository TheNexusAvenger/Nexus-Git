--[[
TheNexusAvenger

Tests the PartitionsRequest class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local PartitionsRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetPartitionsRequest")



--[[
Tests the GetInstancePath method.
--]]
NexusUnitTesting:RegisterUnitTest("GetInstancePath",function(UnitTest)
	local Script = game:GetService("ServerStorage"):WaitForChild("NexusGitTests"):WaitForChild("NexusGitRequest"):WaitForChild("GetRequest"):WaitForChild("GetPartitionsRequestTests")
	
	UnitTest:AssertEquals(PartitionsRequest:GetInstancePath(Script:GetFullName()),Script,"Reference is incorrect.")
	UnitTest:AssertEquals(PartitionsRequest:GetInstancePath("game."..Script:GetFullName()),Script,"Reference is incorrect.")
	UnitTest:AssertNil(PartitionsRequest:GetInstancePath("ReplicatedStorage.NexusGit2.NexusGitRequest"),"Reference exists.")
end)



return true