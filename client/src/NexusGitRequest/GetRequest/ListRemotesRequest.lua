--[[
TheNexusAvenger

Gets the remotes of the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local HttpService = game:GetService("HttpService")

local ListRemotesRequest = MultiHttpRequest:Extend()
ListRemotesRequest:SetClassName("ListRemotesRequest")



--[[
Creates a get status request.
--]]
function ListRemotesRequest:__new(BaseURL)
	self:InitializeSuper("GET",BaseURL.."/ListRemotes")
end


--[[
Gets and parses the version.
--]]
function ListRemotesRequest:GetRemotes()
	--Read and parse the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the remotes.
	return ParsedResponse
end



return ListRemotesRequest