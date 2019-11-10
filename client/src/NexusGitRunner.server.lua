--[[
TheNexusAvenger

Runs the Nexus Git Plugin.
--]]

local NexusGit = require(script.Parent)
local Configuration = NexusGit:GetResource("Configuration")
local ActionsWindow = NexusGit:GetResource("UI.View.Window.ActionsWindow")
local Settings = NexusGit:GetResource("Persistence.Settings")
local NexusPluginFramework = NexusGit:GetResource("NexusPluginFramework")

local ExistingWindow
local WindowOpen = Settings.IsActionsWindowOpen()
local DB = true

local NexusWidgetsToolbar = NexusPluginFramework.new("PluginToolbar","Nexus Widgets")
local NexusGitButton = NexusPluginFramework.new("PluginButton",NexusWidgetsToolbar,"Nexus Git","Opens the Nexus Git window",Configuration.Logo)
NexusGitButton.ClickableWhenViewportHidden = true



--[[
Opens the actions window.
--]]
local function OpenWindow()
	if ExistingWindow then
		ExistingWindow.Enabled = true
	else
		--Create the window.
		ExistingWindow = ActionsWindow.OpenWindow()
		
		--Set up checking the enabled status.
		ExistingWindow:GetPropertyChangedSignal("Enabled"):Connect(function()
			if not ExistingWindow.Enabled then
				WindowOpen = false
				NexusGitButton:SetActive(WindowOpen)
				Settings.SetActionsWindowOpen(WindowOpen)
				ExistingWindow = nil
			end
		end)
	end
end

--[[
Closes the actions window.
--]]
local function CloseWindow()
	ExistingWindow:Close()
	ExistingWindow = nil
end



--Set up button clicking.
NexusGitButton.Click:Connect(function()
	if DB then
		--Toggle it being enabled.
		DB = false
		WindowOpen = not WindowOpen
		NexusGitButton:SetActive(WindowOpen)
		Settings.SetActionsWindowOpen(WindowOpen)
		
		if WindowOpen then
			OpenWindow()
		else
			CloseWindow()
		end
		
		wait()
		DB = true
	end
end)

--Set the initial state.
if WindowOpen then
	NexusGitButton:SetActive(true)
	OpenWindow()
end
