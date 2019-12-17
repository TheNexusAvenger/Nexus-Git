--[[
TheNexusAvenger

Serializes and deserializes userdata.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")

local UserdataSerializier = NexusInstance:Extend()
UserdataSerializier:SetClassName("UserdataSerializier")



local ROBLOX_TYPE_TO_SERIALIZATION_TYPE = {
	["boolean"] = "Bool",
	["number"] = "Float",
	["EnumItem"] = "Enum",
	["string"] = "String",
}

local USERDATA_SERIALIZERS = {
	["Axes"] = function(Userdata)
		local AxesTable = {}
		if Userdata.X then table.insert(AxesTable,"X") end
		if Userdata.Y then table.insert(AxesTable,"Y") end
		if Userdata.Z then table.insert(AxesTable,"Z") end
		
		return AxesTable
	end,
	["Bool"] = true,
	["BrickColor"] = function(Userdata)
		return Userdata.Name
	end,
	["CFrame"] = function(Userdata)
		return {Userdata:components()}
	end,
	["Color3"] = function(Userdata)
		return {Userdata.R,Userdata.G,Userdata.B}
	end,
	["ColorSequence"] = function(Userdata)
		local Numbers = {}
		for _,Keypoint in pairs(Userdata.Keypoints) do
			table.insert(Numbers,Keypoint.Time)
			table.insert(Numbers,Keypoint.Value.R)
			table.insert(Numbers,Keypoint.Value.G)
			table.insert(Numbers,Keypoint.Value.B)
			table.insert(Numbers,0)
		end
		
		return Numbers
	end,
	["Enum"] = function(Userdata)
		return {tostring(Userdata.EnumType),Userdata.Name}
	end,
	["Faces"] = function(Userdata)
		local AxesTable = {}
		if Userdata.Top then table.insert(AxesTable,"Top") end
		if Userdata.Bottom then table.insert(AxesTable,"Bottom") end
		if Userdata.Left then table.insert(AxesTable,"Left") end
		if Userdata.Right then table.insert(AxesTable,"Right") end
		if Userdata.Back then table.insert(AxesTable,"Back") end
		if Userdata.Front then table.insert(AxesTable,"Front") end
		
		return AxesTable
	end,
	["Float"] = true,
	["NumberRange"] = function(Userdata)
		return {Userdata.Min,Userdata.Max}
	end,
	["NumberSequence"] = function(Userdata)
		local Numbers = {}
		for _,Keypoint in pairs(Userdata.Keypoints) do
			table.insert(Numbers,Keypoint.Time)
			table.insert(Numbers,Keypoint.Value)
			table.insert(Numbers,Keypoint.Envelope)
		end
		
		return Numbers
	end,
	["PhysicalProperties"] = function(Userdata)
		return {Userdata.Density,Userdata.Friction,Userdata.Elasticity,Userdata.FrictionWeight,Userdata.ElasticityWeight}
	end,
	["Ray"] = function(Userdata)
		return {Userdata.Origin.X,Userdata.Origin.Y,Userdata.Origin.Z,Userdata.Direction.X,Userdata.Direction.Y,Userdata.Direction.Z}
	end,
	["Rect"] = function(Userdata)
		return {Userdata.Min.X,Userdata.Min.Y,Userdata.Max.X,Userdata.Max.Y}
	end,
	["Region3int16"] = function(Userdata)
		local Numbers = {}
		for Number in string.gmatch(tostring(Userdata),"[%d%-.eE]+") do
			table.insert(Numbers,tonumber(Number))
		end
	
		return Numbers
	end,
	["Region3"] = function(Userdata)
		local RegionData = {Userdata.CFrame:components()}
		table.insert(RegionData,Userdata.Size.X)
		table.insert(RegionData,Userdata.Size.Y)
		table.insert(RegionData,Userdata.Size.Z)
		return RegionData
	end,
	["String"] = true,
	["UDim"] = function(Userdata)
		return {Userdata.Scale,Userdata.Offset}
	end,
	["UDim2"] = function(Userdata)
		return {Userdata.X.Scale,Userdata.X.Offset,Userdata.Y.Scale,Userdata.Y.Offset}
	end,
	["Vector2"] = function(Userdata)
		return {Userdata.X,Userdata.Y}
	end,
	["Vector3"] = function(Userdata)
		return {Userdata.X,Userdata.Y,Userdata.Z}
	end,
	["Vector3int16"] = function(Userdata)
		return {Userdata.X,Userdata.Y,Userdata.Z}
	end,
}

local USERDATA_DEESERIALIZERS = {
	["Axes"] = function(Table)
		local AxesConstructor = {}
		for _,AxisString in pairs(Table) do
			table.insert(AxesConstructor,Enum.Axis[AxisString])
		end
		
		return Axes.new(unpack(AxesConstructor))
	end,
	["Bool"] = true,
	["boolean"] = true,
	["BrickColor"] = function(String)
		return BrickColor.new(String)
	end,
	["CFrame"] = function(Table)
		return CFrame.new(unpack(Table))
	end,
	["Color3"] = function(Table)
		return Color3.new(unpack(Table))
	end,
	["ColorSequence"] = function(Table)
		local Keypoints = {}
		for i = 1,#Table,5 do
			table.insert(Keypoints,ColorSequenceKeypoint.new(Table[i],Color3.new(Table[i + 1],Table[i + 2],Table[i + 3])))
		end
		
		return ColorSequence.new(Keypoints)
	end,
	["Enum"] = function(Table)
		return Enum[Table[1]][Table[2]]
	end,
	["EnumItem"] = function(Table)
		return Enum[Table[1]][Table[2]]
	end,
	["Faces"] = function(Table)
		local FacesConstructor = {}
		for _,FaceString in pairs(Table) do
			table.insert(FacesConstructor,Enum.NormalId[FaceString])
		end
		
		return Faces.new(unpack(FacesConstructor))
	end,
	["Float"] = true,
	["number"] = true,
	["String"] = true,
	["string"] = true,
	["NumberRange"] = function(Table)
		return NumberRange.new(unpack(Table))
	end,
	["NumberSequence"] = function(Table)
		local Keypoints = {}
		for i = 1,#Table,3 do
			table.insert(Keypoints,NumberSequenceKeypoint.new(Table[i],Table[i + 1],Table[i + 2]))
		end
		
		return NumberSequence.new(Keypoints)
	end,
	["PhysicalProperties"] = function(Table)
		return PhysicalProperties.new(unpack(Table))
	end,
	["Ray"] = function(Table)
		return Ray.new(Vector3.new(Table[1],Table[2],Table[3]),Vector3.new(Table[4],Table[5],Table[6]))
	end,
	["Rect"] = function(Table)
		return Rect.new(unpack(Table))
	end,
	["Region3"] = function(Table)
		local Center = CFrame.new(Table[1],Table[2],Table[3],Table[4],Table[5],Table[6],Table[7],Table[8],Table[9],Table[10],Table[11],Table[12])
		local Size = Vector3.new(Table[13],Table[14],Table[15])
		
		return Region3.new(Center * -Size/2,Center * Size/2)
	end,
	["Region3int16"] = function(Table)
		return Region3int16.new(Vector3int16.new(Table[1],Table[2],Table[3]),Vector3int16.new(Table[4],Table[5],Table[6]))
	end,
	["UDim"] = function(Table)
		return UDim.new(unpack(Table))
	end,
	["UDim2"] = function(Table)
		return UDim2.new(unpack(Table))
	end,
	["Vector2"] = function(Table)
		return Vector2.new(unpack(Table))
	end,
	["Vector3"] = function(Table)
		return Vector3.new(unpack(Table))
	end,
	["Vector3int16"] = function(Table)
		return Vector3int16.new(unpack(Table))
	end,
}



--[[
Serializes a userdata to a table.
--]]
function UserdataSerializier:Serialize(Userdata)
	--Get the type.
	local Type = typeof(Userdata)
	Type = ROBLOX_TYPE_TO_SERIALIZATION_TYPE[Type] or Type
	
	--Throw a warning if the userdata is not supported.
	if not USERDATA_SERIALIZERS[Type] then
		warn("Unsupported serialization: "..tostring(Type))
		return {}
	end
	
	--Serialize the userdata.
	local Serializer = USERDATA_SERIALIZERS[Type]
	if Serializer == true then
		return {Type = Type,Value = Userdata}
	else
		return {Type = Type,Value = Serializer(Userdata)}
	end
end

--[[
Deserializes a table to a userdata.
--]]
function UserdataSerializier:Deserialize(Table)
	--Get the type.
	local Type = Table.Type
	
	--Throw a warning if the userdata is not supported.
	if not USERDATA_DEESERIALIZERS[Type] then
		warn("Unsupported deserialization: "..tostring(Type))
		return {}
	end
	
	--Serialize the userdata.
	local Deserializer = USERDATA_DEESERIALIZERS[Type]
	if Deserializer == true then
		return Table.Value
	else
		return Deserializer(Table.Value)
	end
end



return UserdataSerializier