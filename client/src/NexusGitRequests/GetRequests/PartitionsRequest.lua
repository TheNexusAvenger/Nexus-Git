--[[
TheNexusAvenger

Gets the partitions data from the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local HttpService = game:GetService("HttpService")

local PartitionsRequest = NexusInstance:Extend()
PartitionsRequest:SetClassName("PartitionsRequest")
PartitionsRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a get partition request.
--]]
function PartitionsRequest:__new(BaseURL)
	self.Request = MultiHttpRequest.new("GET",BaseURL.."/ProjectPartitions")
end

--[[
Sends the request and returns a response.
--]]
function PartitionsRequest:SendRequest()
	return self.Request:SendRequest()
end

--[[
Returns the Roblox instance for a path.
--]]
function PartitionsRequest:GetInstancePath(Path)
	--Split the path.
	local SplitPath = string.split(Path,".")
	
	--Remove the first section if it is the game reference.
	if #SplitPath >= 1 and string.lower(SplitPath[1]) == "game" then
		table.remove(SplitPath,1)
	end
	
	--Iterate through the path until the end.
	local Reference = game
	for _,PathPart in pairs(SplitPath) do
		Reference = Reference:FindFirstChild(PathPart)
		
		--Return nil if the reference doesn't exist.
		if not Reference then
			return nil
		end
	end
	
	--Return the reference.
	return Reference
end

--[[
Returns a map containing the partition names to the DataModel location.
--]]
function PartitionsRequest:GetPartitions()
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the response if it is only 1 line.
	if #ParsedResponse == 1 then
		return ParsedResponse[1]
	end
	
	--Get and parse the partitions.
	local Partitions = {}
	for Name,Path in pairs(ParsedResponse) do
		local ReferenceInstance = PartitionsRequest:GetInstancePath(Path)
		Partitions[Name] = ReferenceInstance
		
		--Warn if the reference doesn't exist.
		if not ReferenceInstance then
			warn("Missing reference for "..tostring(Name)..": "..tostring(Path))
		end
	end
	
	--Return the partitions.
	return Partitions
end


return PartitionsRequest