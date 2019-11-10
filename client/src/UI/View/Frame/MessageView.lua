--[[
TheNexusAvenger

Class representing a view for displaying a message.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")

local MessageView = NexusWrappedInstance:Extend()
MessageView:SetClassName("MessageView")
MessageView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))





--[[
Creates an message view object.
--]]
function MessageView:__new(Message)
	self:InitializeSuper("Frame")
	self:__SetChangedOverride("Closed",function() end)
	self.Closed = false
	
	--Create the message label.
	local MessageLabel = NexusWrappedInstance.GetInstance("TextLabel")
	MessageLabel.Text = Message
	MessageLabel.TextWrapped = true
	MessageLabel.Position = UDim2.new(0,0,0,2)
	MessageLabel.Size = UDim2.new(1,0,1,-32)
	MessageLabel.TextXAlignment = "Center"
	MessageLabel.Parent = self
	self:__SetChangedOverride("MessageLabel",function() end)
	self.MessageLabel = MessageLabel
	
	--Create the button.
	local CloseButton = NexusWrappedInstance.GetInstance("TextButton")
	CloseButton.Size = UDim2.new(0,70,0,22)
	CloseButton.AnchorPoint = Vector2.new(0.5,0)
	CloseButton.Position = UDim2.new(0.5,0,1,-26)
	CloseButton.BackgroundColor3 = "DialogMainButton"
	CloseButton.TextColor3 = "DialogMainButtonText"
	CloseButton.Text = "Ok"
	CloseButton.Parent = self
	self:__SetChangedOverride("CloseButton",function() end)
	self.CloseButton = CloseButton
	
	--Set up the clicked event.
	local DB = true
	CloseButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:Close()
		end
	end)
end

--[[
Closes the view.
--]]
function MessageView:Close()
	self:Destroy()
	self.Closed = true
end

--[[
Yields for the frame to close.
--]]
function MessageView:YieldForClose()
	while not self.Closed do
		self:GetPropertyChangedSignal("Closed"):Wait()
	end
end



return MessageView