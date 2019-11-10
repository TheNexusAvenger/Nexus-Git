--[[
TheNexusAvenger

Gets the remote branches of the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local HttpService = game:GetService("HttpService")

local ListRemoteBranchesRequest = MultiHttpRequest:Extend()
ListRemoteBranchesRequest:SetClassName("ListRemoteBranchesRequest")



--[[
Creates a get status request.
--]]
function ListRemoteBranchesRequest:__new(BaseURL)
	self:InitializeSuper("GET",BaseURL.."/ListRemoteBranches")
end


--[[
Gets and parses the remote branches.
--]]
function ListRemoteBranchesRequest:GetRemotes()
	--Read and parse the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the remotes.
	return ParsedResponse
end



return ListRemoteBranchesRequest