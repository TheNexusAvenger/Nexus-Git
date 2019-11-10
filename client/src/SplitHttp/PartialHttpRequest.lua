--[[
TheNexusAvenger

Sends partial HTTP requests to a server.
--]]

local NexusGit = require(script.Parent.Parent):GetContext(script)
local NexusInstance = NexusGit:GetResource("NexusInstance.NexusInstance")
local HttpRequest = NexusGit:GetResource("SplitHttp.HttpRequest")
local HttpResponse = NexusGit:GetResource("SplitHttp.HttpResponse")

local HttpService = game:GetService("HttpService")

local PartialHttpRequest = NexusInstance:Extend()
PartialHttpRequest:Implements(HttpRequest)
PartialHttpRequest:SetClassName("PartialHttpRequest")



--[[
Creates a partial Http request
--]]
function PartialHttpRequest:__new(Method,URL,Body,RequestId,PacketId,MaxPacketId)
	self.Method = string.upper(Method)
	self.URL = URL
	self.Body = Body
	self.RequestId = RequestId
	self.PacketId = PacketId or 0
	self.MaxPacketId = MaxPacketId or 1
end

--[[
Sends an HTTP request and returns a response object.
--]]
function PartialHttpRequest:__SendRequest(Method,Request,Body)
	--Send the request.
	local Response
	if Method == "POST" then
		Response = HttpService:PostAsync(Request,Body)
	else
		Response = HttpService:GetAsync(Request)
	end
	
	--Return a response object.
	return HttpResponse.FromJSON(Response)
end

--[[
Formats the URL for the request.
--]]
function PartialHttpRequest:FormatURL()
	--Format the base URL.
	local BaseURL = self.URL
	if string.match(BaseURL,"\?") then
		BaseURL = BaseURL.."&"
	else
		BaseURL = BaseURL.."?"
	end
	
	--Add the parameters.
	if self.RequestId then
		BaseURL = BaseURL.."requestId="..tostring(self.RequestId).."&"
	end
	BaseURL = BaseURL.."packet="..tostring(self.PacketId).."&maxPackets="..tostring(self.MaxPacketId)
	
	--Return the URL.
	return BaseURL
end

--[[
Sends the request and returns a response.
--]]
function PartialHttpRequest:SendRequest()
	--Get the URL.
	local URL = self:FormatURL()
	
	--Return the response.
	return self:__SendRequest(self.Method,URL,self.Body)
end



return PartialHttpRequest