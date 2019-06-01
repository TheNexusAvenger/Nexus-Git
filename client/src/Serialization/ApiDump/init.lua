--[[
TheNexusAvenger

Returns a cache of the Roblox API Dump without being too long.
--]]

local DUMP_VALUE_COUNT = 11



local HttpService = game:GetService("HttpService")

--Concatinate the strings.
local JSONString = ""
for i = 1,DUMP_VALUE_COUNT do
	JSONString = JSONString..script:WaitForChild("ApiDumpSection"..i).Value
end

--Parse and return the string.
return HttpService:JSONDecode(JSONString)