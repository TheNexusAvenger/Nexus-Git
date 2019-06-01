--[[
TheNexusAvenger

Performs a local pull from the file system.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local Serialization = Root:WaitForChild("Serialization")
local InstanceSerializier = require(Serialization:WaitForChild("InstanceSerializier"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local PartitionsRequest = require(script.Parent:WaitForChild("PartitionsRequest"))
local HttpService = game:GetService("HttpService")

local LocalPullRequest = NexusInstance:Extend()
LocalPullRequest:SetClassName("LocalPullRequest")
LocalPullRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a get partition request.
--]]
function LocalPullRequest:__new(BaseURL)
	self.BaseURL = BaseURL
	self.Request = MultiHttpRequest.new("GET",BaseURL.."/LocalPull")
end

--[[
Sends the request and returns a response.
--]]
function LocalPullRequest:SendRequest()
	return self.Request:SendRequest()
end

--[[
Returns a map containing the partition names to the DataModel location.
--]]
function LocalPullRequest:PerformLocalPull()
	local MissingParentLocations = {}
	
	--Get the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Get the target partitions.
	local PartitionDataJSON = PartitionsRequest.new(self.BaseURL):SendRequest()
	local PartitionData = HttpService:JSONDecode(PartitionDataJSON:GetResponse())
	
	--Return the response if it is only 1 line.
	if #ParsedResponse == 1 then
		return ParsedResponse[1]
	end
	
	--Get and update the partitions.
	for Name,StoredInstanceData in pairs(ParsedResponse.Instances) do
		--Get the parent location.
		local SplitParentDirectory = string.split(PartitionData[Name],".")
		table.remove(SplitParentDirectory,#SplitParentDirectory)
		local ParentDirectory = table.concat(SplitParentDirectory,".")
		local ParentLocation = PartitionsRequest:GetInstancePath(ParentDirectory)
		
		--Update the partition.
		if ParentLocation then
			local NewInstance = InstanceSerializier:Deserialize(StoredInstanceData)
			local ExistingInstance = ParentLocation:FindFirstChild(NewInstance.Name)
			if ExistingInstance then
				ExistingInstance:Destroy()
			end
			NewInstance.Parent = ParentLocation
		else
			MissingParentLocations[Name] = ParentDirectory
		end
	end
	
	--Return the missing references.
	return MissingParentLocations
end


return LocalPullRequest