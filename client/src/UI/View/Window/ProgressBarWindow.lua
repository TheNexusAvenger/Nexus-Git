--[[
TheNexusAvenger

Class representing a window for displaying a message.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ProgressBarView = NexusGit:GetResource("UI.View.Frame.ProgressBarView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")

local ProgressBarWindow = ProgressBarView:Extend()
ProgressBarWindow:SetClassName("ProgressBarWindow")



--[[
Creates an message window object.
--]]
function ProgressBarWindow:__new(WindowName,MinWidth,AdditionalHeight)
	self:InitializeSuper()
	MinWidth = MinWidth or 300
	AdditionalHeight = AdditionalHeight or 16
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,WindowName or "Loading",MinWidth,AdditionalHeight + 30,MinWidth,AdditionalHeight + 30)
	self.Window.Enabled = true
	
	--Set up enabling and disabling closing.
	self:__SetChangedOverride("AllowClosing",function() end)
	self.AllowClosing = true
end

--[[
Closes the view.
--]]
function ProgressBarWindow:Close()
	if self.AllowClosing then
		self.super:Close()
		self.Window:Destroy()
	else
		self.Window.Enabled = true
	end
end



return ProgressBarWindow