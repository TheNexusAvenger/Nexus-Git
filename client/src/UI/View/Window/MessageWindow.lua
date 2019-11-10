--[[
TheNexusAvenger

Class representing a window for displaying a message.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local MessageView = NexusGit:GetResource("UI.View.Frame.MessageView")
local WindowCreator = NexusGit:GetResource("UI.View.Window.WindowCreator")

local MessageWindow = MessageView:Extend()
MessageWindow:SetClassName("MessageWindow")



--[[
Creates an message window object.
--]]
function MessageWindow:__new(Message,WindowName,MinWidth,AdditionalHeight)
	self:InitializeSuper(Message)
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
function MessageWindow:Close()
	self.super:Close()
	self.Window:Destroy()
end



return MessageWindow