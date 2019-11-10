--[[
TheNexusAvenger

Class representing a window for displaying the version.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local GetVersionRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetVersionRequest")
local VersionView = NexusGit:GetResource("UI.View.Frame.VersionView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")

local VersionWindow = VersionView:Extend()
VersionWindow:SetClassName("VersionWindow")



--[[
Opens a new window.
--]]
function VersionWindow.OpenWindow(Host)
	--Create a progress bar.
	local ProgressBarWindow = ProgressBarWindow.new("Commiting Files")
	ProgressBarWindow:SetText("Getting server version...")
	
	--Get the server version.
	local StatusRequest = GetVersionRequest.new(Host)
	local Worked,ServerVersion = pcall(function()
		return StatusRequest:GetVersion()
	end)
	ProgressBarWindow:Close()
	
	--Display an error message if the request failed and return a window.
	if not Worked then
		warn("Get status failed because \""..tostring(ServerVersion).."\"")
		return VersionWindow.new("Unreachable")
	else
		return VersionWindow.new(ServerVersion)
	end
end

--[[
Creates an version window object.
--]]
function VersionWindow:__new(ServerVersion)
	self:InitializeSuper(ServerVersion)
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Nexus Git",300,300,300,300)
	self.Window.Enabled = true
end

--[[
Closes the view.
--]]
function VersionWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return VersionWindow