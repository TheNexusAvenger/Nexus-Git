--[[
TheNexusAvenger

Adds files to be tracked on the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local HttpService = game:GetService("HttpService")

local GitAddRequest = NexusInstance:Extend()
GitAddRequest:SetClassName("GitAddRequest")
GitAddRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a add request.
--]]
function GitAddRequest:__new(BaseURL,Files)
	self.Request = MultiHttpRequest.new("POST",BaseURL.."/GitAdd",HttpService:JSONEncode(Files))
end

--[[
Sends the request and returns a response.
--]]
function GitAddRequest:SendRequest()
	return self.Request:SendRequest()
end



return GitAddRequest