--[[
TheNexusAvenger

Pushes the local instances to the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local Serialization = Root:WaitForChild("Serialization")
local Partitions = require(Serialization:WaitForChild("Partitions"))
local NexusGitRequests = Root:WaitForChild("NexusGitRequests")
local GetRequests = NexusGitRequests:WaitForChild("GetRequests")
local PartitionsRequest = require(GetRequests:WaitForChild("PartitionsRequest"))
local HttpService = game:GetService("HttpService")

local LocalPushRequest = NexusInstance:Extend()
LocalPushRequest:SetClassName("LocalPushRequest")
LocalPushRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a get partition request.
--]]
function LocalPushRequest:__new(BaseURL)
	self.BaseURL = BaseURL
end

--[[
Sends the request and returns a response.
--]]
function LocalPushRequest:SendRequest()
	error("SendRequest is not callable. Use LocalPush::PerformLocalPush")
end

--[[
Pushes the local instances to the server.
--]]
function LocalPushRequest:PerformLocalPush()
	--Get the partition data.
	local PartitionsRequest = PartitionsRequest.new(self.BaseURL)
	local PartitionData = PartitionsRequest:GetPartitions()
	
	--Create the serializable partitions.
	local NewPartitions = Partitions.new()
	for Name,Ins in pairs(PartitionData) do
		if Ins ~= nil then
			NewPartitions:StoreInstance(Name,Ins)
		end
	end
	
	--Create and send the request.
	local Request = MultiHttpRequest.new("POST",self.BaseURL.."/LocalPush",NewPartitions:ToJSON())
	return Request:SendRequest()
end



return LocalPushRequest