--[[
TheNexusAvenger

Serializes and deserializes Roblox instances.
--]]

local IGNORED_PROPERTIES = {
	Parent = true,
}



local Root = script.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local RobloxAPI = require(script.Parent:WaitForChild("RobloxAPI"))
local UserdataSerializier = require(script.Parent:WaitForChild("UserdataSerializier"))

local InstanceSerializier = NexusInstance:Extend()
InstanceSerializier:SetClassName("InstanceSerializier")



--[[
Serializes a Roblox Instance to a table.
--]]
function InstanceSerializier:Serialize(RobloxInstance)
	--Create a map of the temporary ids.
	local TemporaryIdMap = {}
	TemporaryIdMap[RobloxInstance] = 1
	local CurrentId = 2
	for _,Ins in pairs(RobloxInstance:GetDescendants()) do
		if Ins.Archivable and RobloxAPI:IsCreatable(Ins.ClassName) then
			TemporaryIdMap[Ins] = CurrentId
			CurrentId = CurrentId + 1
		end
	end
	
	--[[
	Serializes an instance.
	--]]
	local function LocalSerialize(RobloxInstance)
		--Serialize the children.
		local Children = {}
		for _,Child in pairs(RobloxInstance:GetChildren()) do
			if TemporaryIdMap[Child] then
				table.insert(Children,LocalSerialize(Child,TemporaryIdMap))
			end
		end
		
		--Serialize the properties.
		local Properties = {}
		for _,PropertyData in pairs(RobloxAPI:GetProperties(RobloxInstance.ClassName)) do
			local PropertyName = PropertyData.Name
			if not IGNORED_PROPERTIES[PropertyName] then
				local PropertyValue = RobloxInstance[PropertyName]
				if PropertyValue ~= nil then
					if PropertyData.Type and PropertyData.Type.Name == "Instance" then
						local InstanceReference = TemporaryIdMap[PropertyValue]
						if InstanceReference then
							Properties[PropertyName] = {Type = "TemporaryInstanceReference",Value = InstanceReference}
						end
					else
						Properties[PropertyName] = UserdataSerializier:Serialize(PropertyValue)
					end
				end
			end
		end
		
		--Serialize the class name.
		Properties["ClassName"] = UserdataSerializier:Serialize(RobloxInstance.ClassName)
	
		--Return the serialized instance.
		return {
			Type = "Instance",
			TemporaryId = TemporaryIdMap[RobloxInstance],
			Properties = Properties,
			Children = Children,
		}
	end
	
	--Serialize the given instance.
	return LocalSerialize(RobloxInstance)
end

--[[
Deserializes a table to a Roblox Instance.
--]]
function InstanceSerializier:Deserialize(Table)
	--Create the base instances.
	local BaseInstances = {}
	
	--[[
	Creates and registers a base instance.
	--]]
	local function CreateBaseInstance(InstanceData,Parent)
		--Create and store the instance.
		local NewInstance = Instance.new(InstanceData.Properties.ClassName.Value)
		NewInstance.Parent = Parent
		BaseInstances[InstanceData.TemporaryId] = NewInstance
		
		--Create the children.
		for _,ChildData in pairs(InstanceData.Children) do
			CreateBaseInstance(ChildData,NewInstance)
		end
		
		--Return the instance.
		return NewInstance
	end
	
	--[[
	Deserializes the properties.
	--]]
	local function Deserializes(InstanceData)
		--Get the instance.
		local NewInstance = BaseInstances[InstanceData.TemporaryId]
		
		--Apply the properties.
		for PropertyName,PropertyData in pairs(InstanceData.Properties) do
			if PropertyName ~= "ClassName" then
				if PropertyData.Type == "TemporaryInstanceReference" then
					NewInstance[PropertyName] = BaseInstances[PropertyData.Value]
				else
					NewInstance[PropertyName] = UserdataSerializier:Deserialize(PropertyData)
				end
			end
		end
		
		--Deserialize the children.
		for _,ChildData in pairs(InstanceData.Children) do
			Deserializes(ChildData)
		end
	end
	
	--Create the base instances.
	local NewInstance = CreateBaseInstance(Table)
	Deserializes(Table)
	return NewInstance
end



return InstanceSerializier