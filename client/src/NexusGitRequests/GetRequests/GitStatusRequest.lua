--[[
TheNexusAvenger

Gets the git status data from the server.
--]]

local Root = script.Parent.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local SplitHttp = Root:WaitForChild("SplitHttp")
local MultiHttpRequest = require(SplitHttp:WaitForChild("MultiHttpRequest"))
local HttpService = game:GetService("HttpService")

local GitStatusRequest = NexusInstance:Extend()
GitStatusRequest:SetClassName("GitStatusRequest")
GitStatusRequest:Implements(require(SplitHttp:WaitForChild("HttpRequest")))



--[[
Creates a get status request.
--]]
function GitStatusRequest:__new(BaseURL)
	self.Request = MultiHttpRequest.new("GET",BaseURL.."/GitStatus")
end

--[[
Sends the request and returns a response.
--]]
function GitStatusRequest:SendRequest()
	return self.Request:SendRequest()
end

--[[
Gets and parses the Git status.
--]]
function GitStatusRequest:GetStatus()
	local CurrentReadMode
	local CurrentBranch,RemoteBranch,AheadBy
	local TrackedFiles = {
		Modified = {},
		Renamed = {},
		Created = {},
		Deleted = {},
	}
	local UntrackedFiles = {}
	
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
				local TableToAddTo = TrackedFiles.Modified
				local StartIndex = 1
				if string.sub(Line,1,9) == "New file:" then
					TableToAddTo = TrackedFiles.Created
					StartIndex = 11
				elseif string.sub(Line,1,8) == "Deleted:" then
					TableToAddTo = TrackedFiles.Deleted
					StartIndex = 10
				elseif string.sub(Line,1,9) == "Modified:" then
					TableToAddTo = TrackedFiles.Modified
					StartIndex = 11
				elseif string.sub(Line,1,8) == "Renamed:" then
					TableToAddTo = TrackedFiles.Renamed
					StartIndex = 10
				end
				
				--Add the line.
				table.insert(TableToAddTo,string.sub(Line,StartIndex))
			elseif CurrentReadMode == "Untracked" then
				table.insert(UntrackedFiles,Line)
			end
		end
		
	end
	
	--Return the result.
	return {
		CurrentBranch = CurrentBranch,
		RemoteBranch = RemoteBranch,
		AheadBy = AheadBy,
		TrackedFiles = TrackedFiles,
		UntrackedFiles = UntrackedFiles,
	}
end



return GitStatusRequest