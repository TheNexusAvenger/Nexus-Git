--[[
TheNexusAvenger

Gets the unpushed commits of the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local HttpService = game:GetService("HttpService")

local ListCommitsRequest = MultiHttpRequest:Extend()
ListCommitsRequest:SetClassName("ListCommitsRequest")



--[[
Creates a list commits request.
--]]
function ListCommitsRequest:__new(BaseURL,Remote,Branch)
	self:InitializeSuper("GET",BaseURL.."/ListCommits?remote="..Remote.."&branch="..Branch)
end

--[[
Gets and parses the commits.
--]]
function ListCommitsRequest:GetCommits()
	--Read and parse the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the remotes.
	return ParsedResponse
end



return ListCommitsRequest