--[[
TheNexusAvenger

Extends NexusObject to allow for changed singalling
and locking of properties.
--]]

local CLASS_NAME = "NexusInstance"



local NexusObject = require(script.Parent:WaitForChild("NexusObject"))
local NexusEventCreator = require(script.Parent:WaitForChild("Event"):WaitForChild("NexusEventCreator"))

local NexusInstance = NexusObject:Extend()
NexusInstance:SetClassName(CLASS_NAME)



--[[
Creates an instance of a Nexus Instance.
--]]
function NexusInstance:__new()
	--Set up the base object.
	self:InitializeSuper()
	
	--Set up the internal properties.
	self:__InitInternalProperties()
	
	--Set up the metamethods.
	self:__InitMetaMethods()
end

--[[
Called after the constructor when another object
extends the object. The primary purpose is to be
able to manipulate the metatables of a class for
something like NexusInstance.
For NexusObject, nothing is done.
--]]
function NexusInstance:__extended(OtherObject)
	self.super:__extended(OtherObject)
	self.__InitMetaMethods(OtherObject)
end

--[[
Sets up the internal properties.
--]]
function NexusInstance:__InitInternalProperties()
	--Set up the properties.
	self.__PropertyValidators = {}
	self.__HiddenProperties = {}
	self.__LockedProperties = {}
	self.__BlockNextChangedSignals = {}
	self.__PropertyChanged = {}
	self.__ChangedEvent = NexusEventCreator:CreateEvent()
	self.Changed = self.__ChangedEvent
	
	--Lock the internal states.
	self:LockProperty("__PropertyValidators")
	self:LockProperty("__HiddenProperties")
	self:LockProperty("__LockedProperties")
	self:LockProperty("__BlockNextChangedSignals")
	self:LockProperty("__PropertyChanged")
	self:LockProperty("__ChangedEvent")
	self:LockProperty("Changed")
	self:LockProperty("ClassName")
end

--[[
Sets up the meta methods.
--]]
function NexusInstance:__InitMetaMethods()
	--Set up the internal state.
	local InternalProperties = {}
	local PropertyValidators = self.__PropertyValidators
	local HiddenProperties = self.__HiddenProperties
	local LockedProperties = self.__LockedProperties
	local BlockNextChangedSignals = self.__BlockNextChangedSignals
	local PropertyChanged = self.__PropertyChanged
	local ChangedBindableEvent = self.__ChangedEvent
	
	--Set up changes.
	local Metatable = getmetatable(self)
	Metatable.__newindex = function(_,Index,Value)
		--Throw an error if the property is locked.
		if LockedProperties[Index] then
			error(tostring(Index).." is read-only.")
		end
		
		--Return if the new and old values are the same.
		if self[Index] == Value then
			return
		end
		
		--Validate the value.
		local Validators = self.__PropertyValidators[Index]
		if Validators then
			for _,Validator in pairs(Validators) do
				Value = Validator:ValidateChange(self,Index,Value)
			end
		end
		
		--Change the property.
		rawset(InternalProperties,Index,Value)
		
		--Return if the event is hidden.
		if BlockNextChangedSignals[Index] then
			BlockNextChangedSignals[Index] = nil
			return
		end
		
		--Invoke the property changed event.
		local PropertyChangedEvent = PropertyChanged[Index]
		if PropertyChangedEvent then
			PropertyChangedEvent:Fire()
		end
		
		--Invoke the Changed event.
		if HiddenProperties[Index] then
			return
		end
		ChangedBindableEvent:Fire(Index)
	end
	
	--Set up custom indexing.
	local ExistingIndex = Metatable.__index
	Metatable.__index = function(_,Index)
		return rawget(InternalProperties,Index) or ExistingIndex(self,Index)
	end
end

--[[
Adds a validator for a given property.
--]]
function NexusInstance:AddPropertyValidator(PropertyName,Validator)
	if not self.__PropertyValidators[PropertyName] then
		self.__PropertyValidators[PropertyName] = {}
	end
	table.insert(self.__PropertyValidators[PropertyName],Validator)
end

--[[
Prevents a property from being overriden.
--]]
function NexusInstance:LockProperty(PropertyName)
	self.__LockedProperties[PropertyName] = true
end

--[[
Prevents a property being changed from registering the Changed property.
--]]
function NexusInstance:HidePropertyChanges(PropertyName)
	self.__HiddenProperties[PropertyName] = true
end

--[[
Prevents all changed signals being fired for a property change 1 time.
Does not stack with multiple calls.
--]]
function NexusInstance:HideNextPropertyChange(PropertyName)
	self.__BlockNextChangedSignals[PropertyName] = true
end

--[[
Returns a changed signal specific to the property.
--]]
function NexusInstance:GetPropertyChangedSignal(PropertyName)
	--If there is no event created, create a bindable event.
	if not self.__PropertyChanged[PropertyName] then
		self.__PropertyChanged[PropertyName] = NexusEventCreator:CreateEvent()
	end
	
	--Return the event.
	return self.__PropertyChanged[PropertyName]
end



return NexusInstance