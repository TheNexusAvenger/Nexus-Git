--[[
TheNexusAvenger

Class representing a window for changing settings.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local SettingsView = NexusGit:GetResource("UI.View.Frame.SettingsView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")

local SettingsWindow = SettingsView:Extend()
SettingsWindow:SetClassName("SettingsWindow")



--[[
Opens a new window.
--]]
function SettingsWindow.OpenWindow()
	return SettingsWindow.new()
end

--[[
Creates an version window object.
--]]
function SettingsWindow:__new()
	self:InitializeSuper()
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Settings",300,120,300,120)
	self.Window.Enabled = true
end

--[[
Closes the view.
--]]
function SettingsWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return SettingsWindow