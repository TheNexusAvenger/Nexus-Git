--[[
TheNexusAvenger

Commits files on the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")

local HttpService = game:GetService("HttpService")

local GitCommitRequest = MultiHttpRequest:Extend()
GitCommitRequest:SetClassName("GitCommitRequest")



--[[
Creates a commit request.
--]]
function GitCommitRequest:__new(BaseURL,Files,Message)
	--Create the payload.
	local Payload = {
		message = Message,
		files = Files,
	}
	
	--Create the request.
	self:InitializeSuper("POST",BaseURL.."/GitCommit",HttpService:JSONEncode(Payload))
end



return GitCommitRequest