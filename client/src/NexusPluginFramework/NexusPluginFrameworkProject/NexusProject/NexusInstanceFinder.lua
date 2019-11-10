--[[
TheNexusAvenger

Finds a stored version of Nexus Instance. Can be either
under script, script.Parent, or script.Parent.Parent.
--]]

local WARNING_TIME_SECONDS = 5



local StartTime = tick()
local WarningDisplayed = false
local NexusInstance



--[[
Returns the possible locations to scan for.
--]]
local function GetPossibleParents()
	local Locations = {}
	
	--Add the script and parents.
	local CurrentReference = script
	while CurrentReference do
		table.insert(Locations,CurrentReference)
		CurrentReference = CurrentReference.Parent
	end
	
	--Return the locations.
	return Locations
end

--[[
Displays a warning that the time has passed. 
--]]
local function DisplayWarning()
	--Display the base message.
	warn("Unable to find NexusInstance package in any of the following:")
	
	--Display the possible parents.
	for _,Parent in pairs(GetPossibleParents()) do
		warn("\t"..Parent:GetFullName())
	end
end

--[[
Returns if the warning should be displayed.
--]]
local function ShouldDisplayWarning()
	--Return false if the warning was diplayed.
	if WarningDisplayed then
		return false
	end
	
	--Return if the elapsed time is high enough.
	return tick() - StartTime < WARNING_TIME_SECONDS
end

--[[
Returns the current NexusInstance package if one
exists. Returns nil if none was found.
--]]
local function GetNexusInstance()
	--Iterate through the parents and return if NexusInstance was found.
	for _,Parent in pairs(GetPossibleParents()) do
		local NexusInstance = Parent:FindFirstChild("NexusInstance")
		if NexusInstance then
			return NexusInstance
		end
	end
end



--Run the loop for finding NexusInstance.
while not NexusInstance do
	--Find NexusInstance and break the loop if it was found.
	NexusInstance = GetNexusInstance()
	if NexusInstance then
		break
	end
	
	--Display a warning if needed.
	if ShouldDisplayWarning() then
		WarningDisplayed = true
		DisplayWarning()
	end
	wait()
end



--Return NexusInstance.
return NexusInstance