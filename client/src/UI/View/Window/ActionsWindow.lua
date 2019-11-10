--[[
TheNexusAvenger

Class representing a window for the actions.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ActionsView = NexusGit:GetResource("UI.View.Frame.ActionsView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")
local ProgressBarWindow = NexusGit:GetResource("UI.View.Window.ProgressBarWindow")

local ActionsWindow = ActionsView:Extend()
ActionsWindow:SetClassName("ActionsWindow")



--[[
Opens a new window.
--]]
function ActionsWindow.OpenWindow()
	return ActionsWindow.new()
end

--[[
Creates an version window object.
--]]
function ActionsWindow:__new()
	self:InitializeSuper()
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,"Nexus Git Actions",200,200,200,200)
	self.Window.Enabled = true
end

--[[
Closes the view.
--]]
function ActionsWindow:Close()
	self.super:Close()
	self.Enabled = false
	self.Window:Destroy()
end



return ActionsWindow