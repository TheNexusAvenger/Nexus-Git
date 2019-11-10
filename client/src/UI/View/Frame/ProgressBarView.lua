--[[
TheNexusAvenger

Class representing a view for displaying a progress bar.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local ProgressBarFrame = NexusGit:GetResource("UI.Frame.ProgressBarFrame")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")

local ProgressBarView = NexusWrappedInstance:Extend()
ProgressBarView:SetClassName("ProgressBarView")
ProgressBarView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))



--[[
Creates an progress view object.
--]]
function ProgressBarView:__new()
	self:InitializeSuper("Frame")
	
	--Create the progress bar.
	local ProgressBar = ProgressBarFrame.new()
	ProgressBar.Size = UDim2.new(1,-12,0,18)
	ProgressBar.Position = UDim2.new(0,6,0,6)
	ProgressBar.Parent = self
	self:__SetChangedOverride("ProgressBar",function() end)
	self.ProgressBar = ProgressBar
	
	--Create the message label.
	local MessageLabel = NexusWrappedInstance.GetInstance("TextLabel")
	MessageLabel.TextWrapped = true
	MessageLabel.Position = UDim2.new(0,0,0,28)
	MessageLabel.Size = UDim2.new(1,0,1,-30)
	MessageLabel.TextXAlignment = "Center"
	MessageLabel.Parent = self
	self:__SetChangedOverride("MessageLabel",function() end)
	self.MessageLabel = MessageLabel
end

--[[
Closes the view.
--]]
function ProgressBarView:Close()
	self:Destroy()
end

--[[
Sets the progress bar as normal.
--]]
function ProgressBarView:SetAsNormal()
	self.ProgressBar:SetAsNormal()
end

--[[
Sets the progress bar as failed.
--]]
function ProgressBarView:SetAsFailed()
	self.ProgressBar:SetAsFailed()
end

--[[
Sets the progress bar fill.
--]]
function ProgressBarView:SetProgressBarFill(Percent)
	self.ProgressBar.FillAmount = Percent
end

--[[
Sets the text label text.
--]]
function ProgressBarView:SetText(Text)
	self.MessageLabel.Text = Text
end



return ProgressBarView