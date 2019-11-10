--[[
TheNexusAvenger

Class representing a view for displaying a confirmation request.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")

local ConfirmationView = NexusWrappedInstance:Extend()
ConfirmationView:SetClassName("ConfirmationView")
ConfirmationView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))





--[[
Creates an confirmation view object.
--]]
function ConfirmationView:__new(Message,ConfirmText,CancelText)
	self:InitializeSuper("Frame")
	self:__SetChangedOverride("Closed",function() end)
	
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
	
	--Create the buttons.
	local ConfirmButton = NexusWrappedInstance.GetInstance("TextButton")
	ConfirmButton.Size = UDim2.new(0,84,0,22)
	ConfirmButton.AnchorPoint = Vector2.new(0.5,0)
	ConfirmButton.Position = UDim2.new(0.5,-43,1,-26)
	ConfirmButton.BackgroundColor3 = "DialogMainButton"
	ConfirmButton.TextColor3 = "DialogMainButtonText"
	ConfirmButton.Text = ConfirmText or "Yes"
	ConfirmButton.Parent = self
	self:__SetChangedOverride("ConfirmButton",function() end)
	self.ConfirmButton = ConfirmButton
	
	local CloseButton = NexusWrappedInstance.GetInstance("TextButton")
	CloseButton.Size = UDim2.new(0,70,0,22)
	CloseButton.AnchorPoint = Vector2.new(0.5,0)
	CloseButton.Position = UDim2.new(0.5,43,1,-26)
	CloseButton.BackgroundColor3 = "DialogButton"
	CloseButton.TextColor3 = "DialogButtonText"
	CloseButton.Text = CancelText or "No"
	CloseButton.Parent = self
	self:__SetChangedOverride("CloseButton",function() end)
	self.CloseButton = CloseButton
	
	--Set up the clicked events.
	local DB = true
	ConfirmButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:Close(true)
		end
	end)
	
	CloseButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:Close(false)
		end
	end)
end

--[[
Closes the view.
--]]
function ConfirmationView:Close(WasConfirmed)
	self:Destroy()
	self.Closed = WasConfirmed or false
end

--[[
Yields for the frame to close and returns if
it was confirmed.
--]]
function ConfirmationView:YieldForClose()
	--Wait for closed to be invoked.
	while self.Closed == nil do
		self:GetPropertyChangedSignal("Closed"):Wait()
	end
	
	--Return if it was confirmed.
	return self.Closed
end



return ConfirmationView