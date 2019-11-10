--[[
TheNexusAvenger

Gets the git status data from the server.
--]]

local NexusGit = require(script.Parent.Parent.Parent):GetContext(script)
local MultiHttpRequest = NexusGit:GetResource("SplitHttp.MultiHttpRequest")
local Directory = NexusGit:GetResource("NexusGitRequest.File.Directory")
local NexusEnums = NexusGit:GetResource("NexusPluginFramework.Data.Enum.NexusEnumCollection").GetBuiltInEnums()

local HttpService = game:GetService("HttpService")

local GetGitStatusRequest = MultiHttpRequest:Extend()
GetGitStatusRequest:SetClassName("GetGitStatusRequest")



--[[
Creates a get status request.
--]]
function GetGitStatusRequest:__new(BaseURL)
	self:InitializeSuper("GET",BaseURL.."/GetGitStatus")
end

--[[
Converts a table of files to file objects.
--]]
function GetGitStatusRequest:StringsToFiles(FileStrings)
	--Combined the files to a single table.
	local AllFileStrings = {}
	for _,Table in pairs(FileStrings) do
		for _,String in pairs(Table) do
			table.insert(AllFileStrings,String)
		end
	end
	
	--Convert the strings to files and add them to a root directory.
	local Files = Directory.FromStrings(AllFileStrings)
	local TempRoot = Directory.new("")
	for _,File in pairs(Files) do
		TempRoot:AddFile(File)
	end
	
	--[[
	Sets the status of the files.
	--]]
	local function SetStates(Strings,Status)
		for _,String in pairs(Strings) do
			local SplitStrings = string.split(String,"/")
			local CurrentDirectory = TempRoot
			
			--Move to the directory.
			for i = 1,#SplitStrings - 1 do
				CurrentDirectory = CurrentDirectory:GetFile(SplitStrings[i])
			end
			
			--Set the status.
			CurrentDirectory:GetFile(SplitStrings[#SplitStrings]):SetStatus(Status)
		end
	end
	
	--Set the statuses.
	SetStates(FileStrings.Modified,NexusEnums.FileStatus.Modified)
	SetStates(FileStrings.Renamed,NexusEnums.FileStatus.Renamed)
	SetStates(FileStrings.Created,NexusEnums.FileStatus.Created)
	SetStates(FileStrings.Deleted,NexusEnums.FileStatus.Deleted)
	SetStates(FileStrings.Untracked,NexusEnums.FileStatus.Untracked)
	
	--Return the files.
	return Files
end

--[[
Gets and parses the Git status.
--]]
function GetGitStatusRequest:GetStatus()
	local CurrentReadMode
	local CurrentBranch,RemoteBranch,AheadBy
	local FileStrings = {
		Modified = {},
		Renamed = {},
		Created = {},
		Deleted = {},
		Untracked = {},
	}
	
	--Read and parse the response.
	local Response = self:SendRequest():GetResponse()
	local ParsedResponse = HttpService:JSONDecode(Response)
	
	--Return the response if it is only 1 line.
	if #ParsedResponse == 1 then
		return ParsedResponse[1]
	end
	
	--Parse the lines.
	for _,Line in pairs(ParsedResponse) do
		--Set the new read mode or read the line.
		if Line == "Current branch:" then
			CurrentReadMode = "CurrentBranch"
		elseif Line == "Remote branch:" then
			CurrentReadMode = "RemoteBranch"
		elseif Line == "Ahead by:" then
			CurrentReadMode = "AheadBy"
		elseif Line == "Changes to be committed:" then
			CurrentReadMode = "ChangesToCommit"
		elseif Line == "Untracked files:" then
			CurrentReadMode = "Untracked"
		else
			--Read the line.
			if CurrentReadMode == "CurrentBranch" then
				CurrentBranch = Line
			elseif CurrentReadMode == "RemoteBranch" then
				RemoteBranch = Line
			elseif CurrentReadMode == "AheadBy" then
				AheadBy = tonumber(Line)
			elseif CurrentReadMode == "ChangesToCommit" then
				--Get the table to add to.
				local TableToAddTo = FileStrings.Modified
				local StartIndex = 1
				if string.sub(Line,1,9) == "New file:" then
					TableToAddTo = FileStrings.Created
					StartIndex = 11
				elseif string.sub(Line,1,8) == "Deleted:" then
					TableToAddTo = FileStrings.Deleted
					StartIndex = 10
				elseif string.sub(Line,1,9) == "Modified:" then
					TableToAddTo = FileStrings.Modified
					StartIndex = 11
				elseif string.sub(Line,1,8) == "Renamed:" then
					TableToAddTo = FileStrings.Renamed
					StartIndex = 10
				end
				
				--Add the line.
				table.insert(TableToAddTo,string.sub(Line,StartIndex))
			elseif CurrentReadMode == "Untracked" then
				table.insert(FileStrings.Untracked,Line)
			end
		end
		
	end
	
	--Return the result.
	return {
		CurrentBranch = CurrentBranch,
		RemoteBranch = RemoteBranch,
		AheadBy = AheadBy,
		Files = self:StringsToFiles(FileStrings),
	}
end



return GetGitStatusRequest