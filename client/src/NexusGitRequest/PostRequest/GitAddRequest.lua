--[[
TheNexusAvenger

Adds files to be tracked on the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")

local HttpService = game:GetService("HttpService")

local GitAddRequest = MultiHttpRequest:Extend()
GitAddRequest:SetClassName("GitAddRequest")



--[[
Creates a add request.
--]]
function GitAddRequest:__new(BaseURL,Files)
	self:InitializeSuper("POST",BaseURL.."/GitAdd",HttpService:JSONEncode(Files))
end



return GitAddRequest