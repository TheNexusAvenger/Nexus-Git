--[[
TheNexusAvenger

Tests the InstanceSerializier class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")

local Root = game:GetService("ServerStorage"):WaitForChild("NexusGit")
local Serialization = Root:WaitForChild("Serialization")

local InstanceSerializier = require(Serialization:WaitForChild("InstanceSerializier"))



--[[
Tests the Serialize method.
--]]
NexusUnitTesting:RegisterUnitTest("Serialize",function(UnitTest)
	--Create the test classes.
	local Part = Instance.new("Part")
	Part.BrickColor = BrickColor.new("Bright red")
	Part.Anchored = true
	Part.CanCollide = false
	Part.TopSurface = Enum.SurfaceType.Smooth
	
	local SurfaceGui = Instance.new("SurfaceGui")
	SurfaceGui.Adornee = Part
	SurfaceGui.Parent = Part
	
	local Script = Instance.new("Script")
	Script.Source = "--Test script"
	Script.Parent = SurfaceGui
	
	local RemoteEvent = Instance.new("RemoteEvent")
	RemoteEvent.Name = "Event"
	RemoteEvent.Parent = Part
	
	--Test serializing the instance.
	local SerializedInstance = InstanceSerializier:Serialize(Part)
	
	--Assert the temporary ids are correct,
	UnitTest:AssertEquals(SerializedInstance.Type,"Instance","Type is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.TemporaryId,1,"Temporary id is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Children[1].TemporaryId,2,"Temporary id is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Children[1].Children[1].TemporaryId,3,"Temporary id is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Children[2].TemporaryId,4,"Temporary id is incorrect.")
	
	--Assert the properties for the first instance are correct.
	UnitTest:AssertEquals(SerializedInstance.Properties["ClassName"],{Type="String",Value="Part"},"Property is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Properties["BrickColor"],{Type="BrickColor",Value="Bright red"},"Property is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Properties["Anchored"],{Type="Bool",Value=true},"Property is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Properties["CanCollide"],{Type="Bool",Value=false},"Property is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Properties["TopSurface"],{Type="Enum",Value={"SurfaceType","Smooth"}},"Property is incorrect.")
	
	--Assert the properties for the second and third instances are correct.
	UnitTest:AssertEquals(SerializedInstance.Children[1].Properties["Adornee"],{Type="TemporaryInstanceReference",Value=1},"Property is incorrect.")
	UnitTest:AssertNil(SerializedInstance.Children[1].Properties["Parent"],"Property is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Children[1].Children[1].Properties["Source"],{Type="String",Value="--Test script"},"Property is incorrect.")
	UnitTest:AssertEquals(SerializedInstance.Children[2].Properties["Name"],{Type="String",Value="Event"},"Property is incorrect.")
end)

--[[
Tests the Deserialize method.
--]]
NexusUnitTesting:RegisterUnitTest("Deserialize",function(UnitTest)
	--Create the deserialization data.
	local SerializationData = {
		TemporaryId = 1,
		Type = "Instance",
		Properties = {
			["ClassName"] = {Type="String",Value="Part"},
			["BrickColor"] = {Type="BrickColor",Value="Bright red"},
			["Anchored"] = {Type="Bool",Value=true},
			["CanCollide"] = {Type="Bool",Value=false},
			["TopSurface"] = {Type="Enum",Value={"SurfaceType","Smooth"}},
		},
		Children = {
			{
				TemporaryId = 2,
				Type = "Instance",
				Properties = {
					["ClassName"] = {Type="String",Value="SurfaceGui"},
					["Adornee"] = {Type="TemporaryInstanceReference",Value=1},
				},
				Children ={
					{
						TemporaryId = 3,
						Type = "Instance",
						Properties = {
							["ClassName"] = {Type="String",Value="LocalScript"},
							["Source"] = {Type="String",Value="--Test script"},
						},
						Children ={},
					}	
				},
			},
			{
				TemporaryId = 4,
				Type = "Instance",
				Properties = {
					["ClassName"] = {Type="String",Value="RemoteEvent"},
					["Name"] = {Type="String",Value="Event"},
				},
				Children ={},
			},
		},
	}
	
	--Test deserialization of the data.
	local Part = InstanceSerializier:Deserialize(SerializationData)
	
	--Assert it was deserialized correctly.
	local SurfaceGui = Part:FindFirstChildOfClass("SurfaceGui")
	local LocalScript = SurfaceGui:FindFirstChildOfClass("LocalScript")
	local RemoteEvent = Part:FindFirstChildOfClass("RemoteEvent")
	UnitTest:AssertEquals(Part.BrickColor,BrickColor.new("Bright red"),"Property is incorrect.")
	UnitTest:AssertEquals(Part.Anchored,true,"Property is incorrect.")
	UnitTest:AssertEquals(Part.CanCollide,false,"Property is incorrect.")
	UnitTest:AssertEquals(Part.TopSurface,Enum.SurfaceType.Smooth,"Property is incorrect.")
	UnitTest:AssertEquals(SurfaceGui.Adornee,Part,"Property is incorrect.")
	UnitTest:AssertEquals(LocalScript.Source,"--Test script","Property is incorrect.")
	UnitTest:AssertEquals(RemoteEvent.Name,"Event","Property is incorrect.")
end)



return true