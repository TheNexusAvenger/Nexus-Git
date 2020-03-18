--[[
TheNexusAvenger

Stores partitions of instances.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local RobloxAPI = NexusGit:GetResource("Serialization.RobloxAPI")
local InstanceSerializier = NexusGit:GetResource("Serialization.InstanceSerializier")

local HttpService = game:GetService("HttpService")

local Partitions = NexusInstance:Extend()
Partitions:SetClassName("Partitions")



--[[
Creates a partitions object.
--]]
function Partitions:__new()
	self.Partitions = {}
end

--[[
Returns the data for a partition.
--]]
function Partitions:GetData(Name)
	return self.Partitions[Name]
end

--[[
Returns the instance for a partition.
--]]
function Partitions:GetInstance(Name)
	return InstanceSerializier:Deserialize(self.Partitions[Name])
end

--[[
Stores data in a partition.
--]]
function Partitions:StoreData(Name,Data)
	self.Partitions[Name] = Data
end

--[[
Stores an Instance in a partition.
--]]
function Partitions:StoreInstance(Name,Ins)
	self:StoreData(Name,InstanceSerializier:Serialize(Ins))
end

--[[
Serializes the partitions to a JSON string.
--]]
function Partitions:ToJSON()
	return HttpService:JSONEncode({
		Type = "Partitions",
		Instances = self.Partitions,
	})
end

--[[
Deserailizes the partitions from a JSON string.
--]]
function Partitions:FromJSON(JSONString)
	--Deserialize the string.
	local PartitionsData = HttpService:JSONDecode(JSONString)
	
	--Add the data.
	local NewPartitions = Partitions.new()
	for Name,PartitionData in pairs(PartitionsData.Instances) do
		NewPartitions:StoreData(Name,PartitionData)
	end
	
	--Return the new partitions.
	return NewPartitions
end



return Partitions