--[[
TheNexusAvenger

Sends HTTP requests to a server to get partial responses.
--]]

local Root = script.Parent.Parent
local NexusInstance = require(Root:WaitForChild("NexusInstance"):WaitForChild("NexusInstance"))
local HttpRequest = require(script.Parent:WaitForChild("HttpRequest"))
local HttpResponse = require(script.Parent:WaitForChild("HttpResponse"))

local PartialHttpResponseRequest = NexusInstance:Extend()
PartialHttpResponseRequest:Implements(HttpRequest)
PartialHttpResponseRequest:SetClassName("PartialHttpResponseRequest")

local HttpService = game:GetService("HttpService")



--[[
Creates a partial Http request
--]]
function PartialHttpResponseRequest:__new(URL,ResponseId,PacketId)
	self.URL = URL
	self.ResponseId = ResponseId
	self.PacketId = PacketId
end

--[[
Sends an HTTP request and returns a response object.
--]]
function PartialHttpResponseRequest:__SendRequest(Request)
	--Send the request.
	local Response = HttpService:GetAsync(Request)
	
	--Return a response object.
	return HttpResponse.FromJSON(Response)
end

--[[
Formats the URL for the request.
--]]
function PartialHttpResponseRequest:FormatURL()
	--Format the base URL.
	local BaseURL = self.URL
	if string.match(BaseURL,"\?") then
		BaseURL = BaseURL.."&"
	else
		BaseURL = BaseURL.."?"
	end
	
	--Add the parameters.
	BaseURL = BaseURL.."getResponse=true&responseId="..tostring(self.ResponseId).."&packet="..tostring(self.PacketId)
	
	--Return the URL.
	return BaseURL
end

--[[
Sends the request and returns a response.
--]]
function PartialHttpResponseRequest:SendRequest()
	--Get the URL.
	local URL = self:FormatURL()
	
	--Return the response.
	return self:__SendRequest(URL)
end



return PartialHttpResponseRequest