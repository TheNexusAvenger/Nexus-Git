--[[
TheNexusAvenger

Stores and fetches settings.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusPluginFramework.NexusInstance.NexusInstance")
local Plugin = NexusGit:GetResource("NexusPluginFramework.Plugin.NexusPlugin").GetPlugin()


local Settings = NexusInstance:Extend()
Settings:SetClassName("Settings")



--[[
Returns a setting.
--]]
function Settings.GetSetting(Name)
	return Plugin:GetSetting(Name)
end

--[[
Sets a setting.
--]]
function Settings.SetSetting(Name,Value)
	return Plugin:SetSetting(Name,Value)
end

--[[
Returns if the actions is open.
--]]
function Settings.IsActionsWindowOpen()
	return Settings.GetSetting("NexusGitActionsWindowOpen") or false
end

--[[
Returns the port to use.
--]]
function Settings.GetPort()
	return Settings.GetSetting("NexusGitPort") or "8000"
end

--[[
Returns the host to use.
--]]
function Settings.GetHost()
	return Settings.GetSetting("NexusGitHost") or "localhost"
end

--[[
Returns the complete URL to use.
--]]
function Settings.GetURL()
	return "http://"..Settings.GetHost()..":"..Settings.GetPort()
end

--[[
Sets if the actions is open.
--]]
function Settings.SetActionsWindowOpen(IsOpen)
	Settings.SetSetting("NexusGitActionsWindowOpen",IsOpen)
end

--[[
Sets the host to use.
--]]
function Settings.SetHost(Host)
	Settings.SetSetting("NexusGitHost",Host)
end

--[[
Sets the host to use.
--]]
function Settings.SetPort(Port)
	Settings.SetSetting("NexusGitPort",Port)
end





return Settings