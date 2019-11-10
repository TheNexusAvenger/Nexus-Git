--[[
TheNexusAvenger

Creates windows for views.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusPluginGui = NexusGit:GetResource("NexusPluginFramework.Plugin.NexusPluginGui")
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")

local WindowCreator = NexusInstance:Extend()
WindowCreator:SetClassName("WindowCreator")



local CreatedWindows = {}
setmetatable(CreatedWindows,{__mode={"kv"}})



--[[
Creates a window from a view.
--]]
function WindowCreator.CreateWindow(ViewFrame,WidgetName,InitialSizeX,InitialSizeY,MinSizeX,MinSizeY)
	--Create the dock information.
	local DockInformation = DockWidgetPluginGuiInfo.new(
		Enum.InitialDockState.Float,
		false,
		true,
		InitialSizeX or 200,
		InitialSizeY or 200,
		MinSizeX or 0,
		MinSizeY or 0
	)
	
	--Close the existing view.
	local ExistingWindow = CreatedWindows[WidgetName]
	if ExistingWindow and ExistingWindow.Parent then
		ExistingWindow.ViewFrame:Close()
		if ExistingWindow.Parent then
			ExistingWindow:Destroy()
		end
	end
	
	--Create the window in a seperate thread.
	--Work around for: https://devforum.roblox.com/t/destroying-then-recreating-in-same-thread-plugin-guis-yields/349918
	local NewWindow 
	spawn(function()
		NewWindow = NexusPluginGui.new(WidgetName,DockInformation)
		NewWindow:__SetChangedOverride("ViewFrame",function() end)
		NewWindow.ViewFrame = ViewFrame
		ViewFrame.Size = UDim2.new(1,0,1,0)
		ViewFrame.Parent = NewWindow
		CreatedWindows[WidgetName] = NewWindow
	end)
	
	--Wait for the new window.
	while not NewWindow do wait() end
	
	--Set up invoking closing on close.
	NewWindow:BindToClose(function()
		ViewFrame:Close()
	end)
	
	--Replace the destroy method.
	NewWindow:__SetChangedOverride("Destroy",function() end)
	function NewWindow:Destroy()
		NewWindow:GetWrappedInstance():Destroy()
		CreatedWindows[WidgetName] = nil
	end
	
	--Return the window.
	return NewWindow
end



return WindowCreator