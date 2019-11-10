--[[
TheNexusAvenger

Class representing a view for the actions.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local GetVersionRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetVersionRequest")
local ActionsList = NexusGit:GetResource("Action.ActionsList")
local Settings = NexusGit:GetResource("Persistence.Settings")
local ActionListFrame = NexusGit:GetResource("UI.Frame.Action.ActionListFrame")

local ActionsView = NexusWrappedInstance:Extend()
ActionsView:SetClassName("ActionsView")
ActionsView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))

local HttpService = game:GetService("HttpService")



--[[
Creates an git add view object.
--]]
function ActionsView:__new()
	self:InitializeSuper("Frame")
	self:__SetChangedOverride("Closed",function() end)
	self.Closed = false
	
	--Create the top text.
	local VersionText = NexusWrappedInstance.GetInstance("TextLabel")
	VersionText.Size = UDim2.new(1,-8,0,22)
	VersionText.Position = UDim2.new(0,4,0,4)
	VersionText.Text = "Server not detected"
	VersionText.Parent = self
	
	--Create the action list.
	local ListFrame = ActionListFrame.new(ActionsList)
	ListFrame.BorderSizePixel = 1
	ListFrame.Position = UDim2.new(0,-16,0,30)
	ListFrame.Size = UDim2.new(1,16,1,-30)
	ListFrame.Parent = self
	
	--Set up the loop.
	spawn(function()
		while not self.Closed do
			--Get the version.
			local Request = GetVersionRequest.new(Settings.GetURL())
			local Worked,Result = pcall(function()
				return Request:GetVersion()
			end)
			
			--Set the message.
			if Worked then
				VersionText.Text = "Server Version: "..tostring(Result)
				VersionText.TextColor3 = "MainText"
			else
				if string.find(Result,"Http requests are not enabled") then
					VersionText.Text = "HttpService is disabled."
				else
					VersionText.Text = "Nexus Git server not detected."
				end
				VersionText.TextColor3 = "ErrorText"
			end
			
			--Refresh after 1 second.
			wait(1)
		end
	end)
end

--[[
Closes the view.
--]]
function ActionsView:Close()
	self.Closed = true
	self:Destroy()
end



return ActionsView