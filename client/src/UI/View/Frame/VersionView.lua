--[[
TheNexusAvenger

Class representing a view for displaying the version.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local Configuration = NexusGit:GetResource("Configuration")
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")

local VersionView = NexusWrappedInstance:Extend()
VersionView:SetClassName("VersionView")
VersionView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))



--[[
Creates an version view object.
--]]
function VersionView:__new(ServerVersionString)
	self:InitializeSuper("Frame")
	
	--Create the logo.
	local Logo = NexusWrappedInstance.GetInstance("ImageLabel")
	Logo.Size = UDim2.new(0,200,0,200)
	Logo.AnchorPoint = Vector2.new(0.5,0)
	Logo.Position = UDim2.new(0.5,0,0,5)
	Logo.Image = Configuration.Logo
	Logo.BackgroundTransparency = 1
	Logo.Parent = self
	
	--Create the message labels.
	local NameText = NexusWrappedInstance.GetInstance("TextLabel")
	NameText.Text = Configuration.Name
	NameText.Position = UDim2.new(0,0,0,208)
	NameText.TextSize = 48
	NameText.Size = UDim2.new(1,0,0,30)
	NameText.TextXAlignment = "Center"
	NameText.Parent = self
	
	local AuthorText = NexusWrappedInstance.GetInstance("TextLabel")
	AuthorText.Text = Configuration.Author
	AuthorText.Position = UDim2.new(0,0,0,242)
	AuthorText.Size = UDim2.new(1,0,0,16)
	AuthorText.TextSize = 16
	AuthorText.TextXAlignment = "Center"
	AuthorText.Parent = self
	
	local ClientVersion = NexusWrappedInstance.GetInstance("TextLabel")
	ClientVersion.Text = "Plugin Version: "..tostring(Configuration.Version)
	ClientVersion.Position = UDim2.new(0,0,0,258)
	ClientVersion.Size = UDim2.new(1,0,0,16)
	ClientVersion.TextSize = 16
	ClientVersion.TextXAlignment = "Center"
	ClientVersion.Parent = self
	
	local ServerVersion = NexusWrappedInstance.GetInstance("TextLabel")
	ServerVersion.Text = "Server Version: "..ServerVersionString
	ServerVersion.Position = UDim2.new(0,0,0,274)
	ServerVersion.Size = UDim2.new(1,0,0,16)
	ServerVersion.TextSize = 16
	ServerVersion.TextXAlignment = "Center"
	ServerVersion.Parent = self
end

--[[
Closes the view.
--]]
function VersionView:Close()
	self:Destroy()
	self.Closed = true
end



return VersionView