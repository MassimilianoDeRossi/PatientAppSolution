Feature: FirstAccessAsAnonymousUser_item_3778
	In order to use myHEXplan app
	As an Anonymous User
	I want to visualize the anonymous Home page

# Date
Scenario: Today's date is correct and shown
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	Then 'Today Date' is correct

# Hexagons
Scenario: Pin site care option enabled
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'Pin site care' option
	And User tap on '<' option
	Then 'Home Anonymous' page is visualized

Scenario: Support option enabled
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'Support' option
	And User tap on '<' option
	Then 'Home Anonymous' page is visualized

# Added with CR#1 of US 5990
Scenario: Strut Adjust option enabled
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'Strut Adjust' option
	And User tap on '<' option
	Then 'Home Anonymous' page is visualized

# Added with CR#1 of US 5990
Scenario: Time Lapse option enabled
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'Time Lapse' option
	And User tap on '<' option
	Then 'Home Anonymous' page is visualized


# Button in the page
Scenario: User profile customization option enabled
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'Tap here to get started' option
	Then 'User profile customization' page is visualized
	When User tap on '<' option
	Then 'Home Anonymous' page is visualized

Scenario: Settings Option enabled
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'Settings' option
	And User tap on '<' option
	Then 'Home Anonymous' page is visualized

# Other option doesn't work
Scenario: Only Anonymous's option work
	Given User is 'anonymous'
	And User navigate to 'Home Anonymous' page
	When User tap on 'an area without' option
	Then 'Home Anonymous' page is visualized