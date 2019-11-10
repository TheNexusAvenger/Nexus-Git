--[[
TheNexusAvenger

Pulls files from remote repositories on the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")

local HttpService = game:GetService("HttpService")

local RemotePushRequest = MultiHttpRequest:Extend()
RemotePushRequest:SetClassName("RemotePushRequest")



--[[
Creates a remote pull request.
--]]
function RemotePushRequest:__new(BaseURL,Remote,Branch)
	--Create the URL.
	local URL = BaseURL.."/RemotePull?remote"..Remote.."..&branch="..Branch
	
	--Create the request.
	self:InitializeSuper("GET",URL)
end



return RemotePushRequest