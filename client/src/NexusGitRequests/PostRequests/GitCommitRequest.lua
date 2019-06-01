--[[
TheNexusAvenger

Commits files on the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local HttpService = game:GetService("HttpService")

local GitCommitRequest = NexusInstance:Extend()
GitCommitRequest:SetClassName("GitCommitRequest")
GitCommitRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



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
	self.Request = MultiHttpRequest.new("POST",BaseURL.."/GitCommit",HttpService:JSONEncode(Payload))
end

--[[
Sends the request and returns a response.
--]]
function GitCommitRequest:SendRequest()
	return self.Request:SendRequest()
end



return GitCommitRequest