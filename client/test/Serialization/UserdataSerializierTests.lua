--[[
TheNexusAvenger

Tests the UserdataSerializier class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local UserdataSerializier = NexusGit:GetResource("Serialization.UserdataSerializier")



--[[
Tests the Serialize method.
--]]
NexusUnitTesting:RegisterUnitTest("Serialize",function(UnitTest)
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Axes.new(Enum.Axis.X,Enum.Axis.Z)),{Type="Axes",Value={"X","Z"}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(true),{Type="Bool",Value=true},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(false),{Type="Bool",Value=false},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(BrickColor.new("Bright green")),{Type="BrickColor",Value="Bright green"},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(CFrame.new(1,2,3,4,5,6,7,8,9,10,11,12)),{Type="CFrame",Value={1,2,3,4,5,6,7,8,9,10,11,12}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Color3.new(0.25,0.5,0.75)),{Type="Color3",Value={0.25,0.5,0.75}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(ColorSequence.new({ColorSequenceKeypoint.new(0,Color3.new(0.25,0.5,0.75)),ColorSequenceKeypoint.new(0.5,Color3.new(0.75,0.25,0.5)),ColorSequenceKeypoint.new(1,Color3.new(0.5,0.75,0.25))})),{Type="ColorSequence",Value={0,0.25,0.5,0.75,0,0.5,0.75,0.25,0.5,0,1,0.5,0.75,0.25,0}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Enum.ActionType.Draw),{Type="Enum",Value={"ActionType","Draw"}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Faces.new(Enum.NormalId.Back,Enum.NormalId.Right)),{Type="Faces",Value={"Right","Back"}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(3.14),{Type="Float",Value=3.14},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(NumberRange.new(1,5)),{Type="NumberRange",Value={1,5}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(NumberSequence.new({NumberSequenceKeypoint.new(0,0,0),NumberSequenceKeypoint.new(0.5,0.5,0.5),NumberSequenceKeypoint.new(1,1,0)})),{Type="NumberSequence",Value={0,0,0,0.5,0.5,0.5,1,1,0}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(PhysicalProperties.new(0.5,0.5,0.5,0.5,0.5)),{Type="PhysicalProperties",Value={0.5,0.5,0.5,0.5,0.5}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Ray.new(Vector3.new(1,2,3),Vector3.new(4,5,6))),{Type="Ray",Value={1,2,3,4,5,6}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Rect.new(1,2,3,4)),{Type="Rect",Value={1,2,3,4}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Region3.new(Vector3.new(-2,-2,-2),Vector3.new(2,2,2))),{Type="Region3",Value={0,0,0,1,0,0,0,1,0,0,0,1,4,4,4}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Region3int16.new(Vector3int16.new(1,2,3),Vector3int16.new(4,5,6))),{Type="Region3int16",Value={1,2,3,4,5,6}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize("Test string"),{Type="String",Value="Test string"},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(UDim.new(1,2)),{Type="UDim",Value={1,2}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(UDim2.new(1,2,3,4)),{Type="UDim2",Value={1,2,3,4}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Vector2.new(1,2)),{Type="Vector2",Value={1,2}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Vector3.new(1,2,3)),{Type="Vector3",Value={1,2,3}},"Serialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Serialize(Vector3int16.new(1,2,3)),{Type="Vector3int16",Value={1,2,3}},"Serialization is correct.")
end)

--[[
Tests the Deserialize method.
--]]
NexusUnitTesting:RegisterUnitTest("Deserialize",function(UnitTest)
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Axes",Value={"Y","Z"}}),Axes.new(Enum.Axis.Y,Enum.Axis.Z),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Bool",Value=true}),true,"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Bool",Value=false}),false,"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="BrickColor",Value="Bright green"}),BrickColor.new("Bright green"),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="CFrame",Value={1,2,3,4,5,6,7,8,9,10,11,12}}),CFrame.new(1,2,3,4,5,6,7,8,9,10,11,12),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Color3",Value={0.25,0.5,0.75}}),Color3.new(0.25,0.5,0.75),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="ColorSequence",Value={0,0.25,0.5,0.75,0,0.5,0.75,0.25,0.5,0,1,0.5,0.75,0.25,0}}),ColorSequence.new({ColorSequenceKeypoint.new(0,Color3.new(0.25,0.5,0.75)),ColorSequenceKeypoint.new(0.5,Color3.new(0.75,0.25,0.5)),ColorSequenceKeypoint.new(1,Color3.new(0.5,0.75,0.25))}),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Enum",Value={"Material","Ice"}}),Enum.Material.Ice,"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Faces",Value={"Top","Right"}}),Faces.new(Enum.NormalId.Top,Enum.NormalId.Right),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Float",Value=3.14}),3.14,"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="NumberRange",Value={1,5}}),NumberRange.new(1,5),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="NumberSequence",Value={0,0,0,0.5,0.5,0.5,1,1,0}}),NumberSequence.new({NumberSequenceKeypoint.new(0,0,0),NumberSequenceKeypoint.new(0.5,0.5,0.5),NumberSequenceKeypoint.new(1,1,0)}),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="PhysicalProperties",Value={0.1,0.2,0.3,0.4,0.5}}),PhysicalProperties.new(0.1,0.2,0.3,0.4,0.5),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Ray",Value={1,2,3,4,5,6}}),Ray.new(Vector3.new(1,2,3),Vector3.new(4,5,6)),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Rect",Value={1,2,3,4}}),Rect.new(1,2,3,4),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Region3",Value={0,0,0,1,0,0,0,1,0,0,0,1,4,4,4}}),Region3.new(Vector3.new(-2,-2,-2),Vector3.new(2,2,2)),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Region3int16",Value={1,2,3,4,5,6}}),Region3int16.new(Vector3int16.new(1,2,3),Vector3int16.new(4,5,6)),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="String",Value="Test string"}),"Test string","Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="UDim",Value={1,2}}),UDim.new(1,2),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="UDim2",Value={1,2,3,4}}),UDim2.new(1,2,3,4),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Vector2",Value={1,2}}),Vector2.new(1,2),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Vector3",Value={1,2,3}}),Vector3.new(1,2,3),"Deserialization is correct.")
	UnitTest:AssertEquals(UserdataSerializier:Deserialize({Type="Vector3int16",Value={1,2,3}}),Vector3int16.new(1,2,3),"Deserialization is correct.")
end)



return true