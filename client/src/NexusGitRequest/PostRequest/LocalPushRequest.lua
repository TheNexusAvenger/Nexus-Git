--[[
TheNexusAvenger

Pushes the local instances to the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")
local HttpRequest = NexusGit:GetResource("SplitHttp.HttpRequest")
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local Partitions = NexusGit:GetResource("Serialization.Partitions")
local PartitionsRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetPartitionsRequest")

local HttpService = game:GetService("HttpService")

local LocalPushRequest = NexusInstance:Extend()
LocalPushRequest:SetClassName("LocalPushRequest")
LocalPushRequest:Implements(HttpRequest)



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