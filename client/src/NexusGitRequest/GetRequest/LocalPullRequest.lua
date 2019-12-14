--[[
TheNexusAvenger

Performs a local pull from the file system.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local InstanceSerializier = NexusGit:GetResource("Serialization.InstanceSerializier")
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local PartitionsRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetPartitionsRequest")

local HttpService = game:GetService("HttpService")

local LocalPullRequest = MultiHttpRequest:Extend()
LocalPullRequest:SetClassName("LocalPullRequest")



--[[
Creates a get partition request.
--]]
function LocalPullRequest:__new(BaseURL)
	self:InitializeSuper("GET",BaseURL.."/LocalPull")
	self.BaseURL = BaseURL
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
			InstanceSerializier:Deserialize(StoredInstanceData,ParentLocation)
		else
			MissingParentLocations[Name] = ParentDirectory
		end
	end
	
	--Return the missing references.
	return MissingParentLocations
end



return LocalPullRequest