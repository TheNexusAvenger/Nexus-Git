--[[
TheNexusAvenger

Pushes files to remote repositories on the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")

local HttpService = game:GetService("HttpService")

local RemotePushRequest = MultiHttpRequest:Extend()
RemotePushRequest:SetClassName("RemotePushRequest")



--[[
Creates a push request.
--]]
function RemotePushRequest:__new(BaseURL,Remote,Branch,Force)
	--Create the URL.
	local URL = BaseURL.."/RemotePush?remote"..Remote.."..&branch="..Branch
	if Force then
		URL = URL.."&force="..tostring(Force)
	end
	
	--Create the request.
	self:InitializeSuper("POST",URL)
end



return RemotePushRequest