--[[
TheNexusAvenger

Class representing a view for displaying the settings.
--]]

local NexusGit = require(script.Parent.Parent.Parent.Parent):GetContext(script)
local NexusWrappedInstance = NexusGit:GetResource("NexusPluginFramework.Base.NexusWrappedInstance")
local Settings = NexusGit:GetResource("Persistence.Settings")
local GetVersionRequest = NexusGit:GetResource("NexusGitRequest.GetRequest.GetVersionRequest")

local SettingsView = NexusWrappedInstance:Extend()
SettingsView:SetClassName("SettingsView")
SettingsView:Implements(NexusGit:GetResource("UI.View.Frame.IViewFrame"))



--[[
Creates an settings view object.
--]]
function SettingsView:__new()
	self:InitializeSuper("Frame")
	
	--Create the container.
	local CommitsScrollingFrame = NexusWrappedInstance.GetInstance("Frame")
	CommitsScrollingFrame.BorderSizePixel = 1
	CommitsScrollingFrame.Size = UDim2.new(1,0,1,-30)
	CommitsScrollingFrame.Parent = self
	
	--Create the host setting.
	local HostHeaderText = NexusWrappedInstance.GetInstance("TextLabel")
	HostHeaderText.Size = UDim2.new(0,84,0,22)
	HostHeaderText.Position = UDim2.new(0,4,0,4)
	HostHeaderText.Text = "Host:"
	HostHeaderText.Parent = CommitsScrollingFrame
	
	local ProtocolText = NexusWrappedInstance.GetInstance("TextLabel")
	ProtocolText.Size = UDim2.new(0,36,0,22)
	ProtocolText.Position = UDim2.new(0,8,0,26)
	ProtocolText.Text = "http://"
	ProtocolText.Parent = CommitsScrollingFrame
	
	local HostNameInput = NexusWrappedInstance.GetInstance("TextBox")
	HostNameInput.Size = UDim2.new(0,120,0,22)
	HostNameInput.Position = UDim2.new(0,44,0,26)
	HostNameInput.TextXAlignment = "Left"
	HostNameInput.Text = Settings.GetHost()
	HostNameInput.Parent = CommitsScrollingFrame
	
	local PortSeperatorText = NexusWrappedInstance.GetInstance("TextLabel")
	PortSeperatorText.Size = UDim2.new(0,10,0,22)
	PortSeperatorText.Position = UDim2.new(0,164,0,26)
	PortSeperatorText.Text = ":"
	PortSeperatorText.TextXAlignment = "Center"
	PortSeperatorText.Parent = CommitsScrollingFrame
	
	local PortNumberInput = NexusWrappedInstance.GetInstance("TextBox")
	PortNumberInput.Size = UDim2.new(0,60,0,22)
	PortNumberInput.Position = UDim2.new(0,174,0,26)
	PortNumberInput.TextXAlignment = "Left"
	PortNumberInput.Text = Settings.GetPort()
	PortNumberInput.Parent = CommitsScrollingFrame
	
	--Create the host test.
	local TestConnectionButton = NexusWrappedInstance.GetInstance("TextButton")
	TestConnectionButton.Size = UDim2.new(0,64,0,18)
	TestConnectionButton.Position = UDim2.new(0,28,0,56)
	TestConnectionButton.BackgroundColor3 = "DialogMainButton"
	TestConnectionButton.TextColor3 = "DialogMainButtonText"
	TestConnectionButton.Text = "Test"
	TestConnectionButton.Parent = CommitsScrollingFrame
	
	local TestConnectionText = NexusWrappedInstance.GetInstance("TextLabel")
	TestConnectionText.Size = UDim2.new(0,120,0,18)
	TestConnectionText.Position = UDim2.new(0,96,0,56)
	TestConnectionText.Text = ""
	TestConnectionText.Parent = CommitsScrollingFrame
	
	--Create the save buttons.
	local CancelButton = NexusWrappedInstance.GetInstance("TextButton")
	CancelButton.Size = UDim2.new(0,84,0,22)
	CancelButton.Position = UDim2.new(1,-88,1,-26)
	CancelButton.BackgroundColor3 = "DialogButton"
	CancelButton.TextColor3 = "DialogButtonText"
	CancelButton.Text = "Cancel"
	CancelButton.Parent = self
	
	local SaveButton = NexusWrappedInstance.GetInstance("TextButton")
	SaveButton.Size = UDim2.new(0,84,0,22)
	SaveButton.Position = UDim2.new(1,-176,1,-26)
	SaveButton.BackgroundColor3 = "DialogMainButton"
	SaveButton.TextColor3 = "DialogMainButtonText"
	SaveButton.Text = "Save"
	SaveButton.Parent = self
	
	--[[
	Corrects the port number.
	--]]
	local function CorrectPortNumber()
		if PortNumberInput.Text == "" then
			PortNumberInput.Text = "8000"
		end
	end
	
	--Connect the correction for the port.
	local LastPortInput = PortNumberInput.Text
	PortNumberInput:GetPropertyChangedSignal("Text"):Connect(function()
		local NewText = PortNumberInput.Text
		
		--Revert the text if not blank and not a number.
		if NewText ~= "" and tonumber(NewText) == nil then
			PortNumberInput.Text = LastPortInput
		else
			LastPortInput = NewText
		end
	end)
	
	--Connect the buttons.
	local DB = true
	SaveButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			
			--Save the settings.
			CorrectPortNumber()
			Settings.SetHost(HostNameInput.Text)
			Settings.SetPort(PortNumberInput.Text)
			
			--Close the window.
			self.object:Close()
		end
	end)
	
	CancelButton.MouseButton1Down:Connect(function()
		if DB then
			DB = false
			self.object:Close()
		end
	end)
	
	local LastTest
	TestConnectionButton.MouseButton1Down:Connect(function()
		--Create the version request.
		CorrectPortNumber()
		local URL = "http://"..HostNameInput.Text..":"..PortNumberInput.Text
		local Request = GetVersionRequest.new(URL)
		
		--Set the current time.
		local CurrentTime = tick()
		LastTest = CurrentTime
		
		--Send the request.
		TestConnectionText.Text = "Testing connection..."
		local Worked,Error = pcall(function()
			Request:GetVersion()
		end)
		
		--Set the test result.
		if CurrentTime ~= LastTest then return end
		if Worked then
			TestConnectionText.Text = "Testing successful"
		else
			TestConnectionText.Text = "Testing failed"
			warn("Getting the version from "..URL.."/version failed because "..tostring(Error))
		end
	end)
end

--[[
Closes the view.
--]]
function SettingsView:Close()
	self:Destroy()
	self.Closed = true
end



return SettingsView