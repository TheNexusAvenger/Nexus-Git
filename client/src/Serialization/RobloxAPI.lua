--[[
TheNexusAvenger

Determines the attributes of instances.
--]]

local Root = script.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local ApiDump = require(script.Parent:WaitForChild("ApiDump"))

local RobloxAPI = NexusInstance:Extend()
RobloxAPI:SetClassName("RobloxAPI")



--[[
Returns if a property can be saved or loaded.
--]]
local function CanSerializeProperty(PropertyData)
	--Return false if it can't be saved or loaded.
	if not PropertyData.Serialization or (not PropertyData.Serialization.CanLoad and not PropertyData.Serialization.CanSave) then
		return false
	end
	
	--Convert the tags to a map.
	local TagsMap = {}
	for _,Tag in pairs(PropertyData.Tags or {}) do
		TagsMap[Tag] = true
	end
	
	--Return false if Deprecated or Hidden exists.
	if TagsMap["Deprecated"] or TagsMap["Hidden"] then
		return false
	end
	
	--Return true (can save).
	return true
end



--[[
Returns the raw data for a class name.
--]]
function RobloxAPI:GetRawData(ClassName)
	for _,Data in pairs(ApiDump.Classes) do
		if Data.Name == ClassName then
			return Data
		end
	end
end

--[[
Returns the super class name of a given class name.
--]]
function RobloxAPI:GetSuperClassName(ClassName)
	local ClassData = RobloxAPI:GetRawData(ClassName)
	if ClassData then
		return ClassData.Superclass
	end
end

--[[
Returns if a class can be created.
--]]
function RobloxAPI:IsCreatable(ClassName)
	local ClassData = RobloxAPI:GetRawData(ClassName)
	if ClassData then
		--Return false if a NotCreatable tag exist.
		for _,Tag in pairs(ClassData.Tags or {}) do
			if Tag == "NotCreatable" then
				return false
			end
		end
		
		--Return true (success).
		return true
	end
	
	--Return false (unknown class.
	return false
end

--[[
Returns the non-read-only, non-deprecated properties for a given class.
--]]
function RobloxAPI:GetProperties(ClassName)
	local Properties = {}
	local ClassData = RobloxAPI:GetRawData(ClassName)
	
	--Recurse through the class data.
	while ClassData do
		--Iterate through the properties.
		for _,Property in pairs(ClassData.Members) do
			if Property.MemberType == "Property" and CanSerializeProperty(Property) then
				local PropertyData = {Name = Property.Name,Type = Property.ValueType}
				table.insert(Properties,PropertyData)
			end
		end
		
		--Set the next class data to use.
		ClassData = RobloxAPI:GetRawData(ClassData.Superclass)
	end
	
	--Return the properties.
	return Properties
end



return RobloxAPI