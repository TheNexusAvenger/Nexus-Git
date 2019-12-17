--[[
TheNexusAvenger

Tests the RobloxAPI class.
--]]

local NexusUnitTesting = require("NexusUnitTesting")

local NexusGit = require(game:GetService("ServerStorage"):WaitForChild("NexusGit"))
local RobloxAPI = NexusGit:GetResource("Serialization.RobloxAPI")



--[[
Tests the GetRawData method.
--]]
NexusUnitTesting:RegisterUnitTest("GetRawData",function(UnitTest)
	UnitTest:AssertEquals(RobloxAPI:GetRawData("Part").Name,"Part","Class name is incorrect.")
	UnitTest:AssertEquals(RobloxAPI:GetRawData("Part").Superclass,"FormFactorPart","Superclass name is incorrect.")
	UnitTest:AssertEquals(RobloxAPI:GetRawData("FormFactorPart").Superclass,"BasePart","Superclass name is incorrect.")
end)

--[[
Tests the GetSuperClassName method.
--]]
NexusUnitTesting:RegisterUnitTest("GetSuperClassName",function(UnitTest)
	UnitTest:AssertEquals(RobloxAPI:GetSuperClassName("Part"),"FormFactorPart","Superclass name is incorrect.")
	UnitTest:AssertEquals(RobloxAPI:GetSuperClassName("FormFactorPart"),"BasePart","Superclass name is incorrect.")
end)

--[[
Tests the IsCreatable method.
--]]
NexusUnitTesting:RegisterUnitTest("IsCreatable",function(UnitTest)
	UnitTest:AssertTrue(RobloxAPI:IsCreatable("Part"),"Part is uncreatable.")
	UnitTest:AssertFalse(RobloxAPI:IsCreatable("Instance"),"Instance is creatable.")
end)

--[[
Tests the GetProperties method.
This unit test relies on specific properties existing, so it may need to be modified later.
--]]
NexusUnitTesting:RegisterUnitTest("GetProperties",function(UnitTest)
	--Get the properties.
	local PropertiesMap = {}
	for _,PropertyData in pairs(RobloxAPI:GetProperties("Part")) do
		PropertiesMap[PropertyData.Name] = PropertyData.Type
	end
	
	--Assert the correct properties exist.
	UnitTest:AssertEquals(PropertiesMap["Name"].Name,"string","Property type name is incorrect.")
	UnitTest:AssertEquals(PropertiesMap["Size"].Name,"Vector3","Property type name is incorrect.")
	UnitTest:AssertEquals(PropertiesMap["CFrame"].Name,"CFrame","Property type name is incorrect.")
	UnitTest:AssertNil(PropertiesMap["archivable"],"Missing property exists.")
end)



return true