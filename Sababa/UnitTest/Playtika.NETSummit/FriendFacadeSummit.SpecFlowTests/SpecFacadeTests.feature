Feature: SpecFacadeTests

Background:
	Given Setup before test

Scenario: CheckCallMethodGetFriends_GetInstanceFriendFacade_VerifyCalls
	Given I have entered 10 into the facade
	And I have also entered 2 into the facade
	Then I setup Api GetFriends inter "keyFriend:9" and "keyFriend:10" values
	When I call Get
	Then the result should be Api Get called once

Scenario: CheckCallMethodGetLog_GetMockIFriendsApi_VerifyCalls
	Given I have entered 2 into the facade
	And I have also entered 2 into the facade
	Then I setup Api GetFriends inter "keyFriend:1" and "keyFriend:2" values
	When I call Get
	Then the result should be Logger Log called once

Scenario: SaveCollectionFriendsIntoTheCache_CallsTwiceMethodGet_CheckLogAngEqualsCollection
	Given I have entered 5 into the facade
	And I have also entered 3 into the facade
	Then I setup Api GetFriends with three keys inter "keyFriend:4" and "keyFriend:5" and "keyFriend:6" values
	When I call Get
	Then I call Get again
	Then the result should be Logger Log called once and collection should be equals

Scenario: CheckThrowException_SetInvalidValues_VerifyThrowIndexOutOfRangeException
	Then I entered an invalid number in parameters Get and check throw IndexOutOfRangeException

Scenario: CheckThrowException_SetInvalidValues_VerifyThrowArgumentException
	Then I entered an invalid number in parameters Get and check throw ArgumentException