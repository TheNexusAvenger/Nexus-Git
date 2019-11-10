--[[
TheNexusAvenger

Class representing a window for displaying a confirmation.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ConfirmationView = NexusGit:GetResource("UI.View.Frame.ConfirmationView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")

local ConfirmationWindow = ConfirmationView:Extend()
ConfirmationWindow:SetClassName("ConfirmationWindow")



--[[
Creates an message window object.
--]]
function ConfirmationWindow:__new(Message,WindowName,ConfirmationText,CancelText,MinWidth,AdditionalHeight)
	self:InitializeSuper(Message,ConfirmationText,CancelText)
	MinWidth = MinWidth or 400
	AdditionalHeight = AdditionalHeight or 20
	
	--Create the window.
	self:__SetChangedOverride("Window",function() end)
	self.Window = WindowCreator.CreateWindow(self,WindowName or "Message",MinWidth,AdditionalHeight + 30,MinWidth,AdditionalHeight + 30)
	self.Window.Enabled = true
end

--[[
Closes the view.
--]]
function ConfirmationWindow:Close(WasConfirmed)
	self.super:Close(WasConfirmed)
	self.Window:Destroy()
end



return ConfirmationWindow