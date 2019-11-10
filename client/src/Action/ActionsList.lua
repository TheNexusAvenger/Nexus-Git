--[[
TheNexusAvenger

List of the actions to use.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)



return {
	NexusGit:GetResource("Action.ShowVersion").new(),
	NexusGit:GetResource("Action.ShowSettings").new(),
	NexusGit:GetResource("Action.GitAdd").new(),
	NexusGit:GetResource("Action.GitCommit").new(),
	NexusGit:GetResource("Action.GitPull").new(),
	NexusGit:GetResource("Action.GitPush").new(),
	NexusGit:GetResource("Action.LocalPull").new(),
	NexusGit:GetResource("Action.LocalPush").new(),
}