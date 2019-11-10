--[[
TheNexusAvenger

Gets the version and project of the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local HttpService = game:GetService("HttpService")

local GetVersionRequest = MultiHttpRequest:Extend()
GetVersionRequest:SetClassName("GetVersionRequest")



--[[
Creates a get status request.
--]]
function GetVersionRequest:__new(BaseURL)
	self:InitializeSuper("GET",BaseURL.."/GetVersion")
end


--[[
Gets and parses the version.
--]]
function GetVersionRequest:GetVersion()
	--Read and parse the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the version.
	return ParsedResponse.version.." ("..ParsedResponse.project..")"
end



return GetVersionRequest