--[[
TheNexusAvenger

Pushes files to remote repositories on the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local HttpService = game:GetService("HttpService")

local RemotePushRequest = NexusInstance:Extend()
RemotePushRequest:SetClassName("RemotePushRequest")
RemotePushRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a commit request.
--]]
function RemotePushRequest:__new(BaseURL,Remote,Branch,Force)
	--Create the URL.
	local URL = BaseURL.."/RemotePush?remote"..Remote.."..&branch="..Branch
	if Force then
		URL = URL.."&force="..tostring(Force)
	end
	
	--Create the request.
	self.Request = MultiHttpRequest.new("POST",URL)
end

--[[
Sends the request and returns a response.
--]]
function RemotePushRequest:SendRequest()
	return self.Request:SendRequest()
end



return RemotePushRequest