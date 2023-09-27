Feature: AccessWithAtLeastAPrescriptionLoaded_item_3775
	In order to use myHEXplan app
	As an Normal User
	I want to visualize the Normal Home Page
	Rules:
		at least one valid prescription scanned

# Date
Scenario: Today's date is correct
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	Then 'Today Date' is correct

# Hexagons
Scenario: Pin site care option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Pin site care' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: Strut Adjust option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Strut Adjust' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

@ignore
#This scenario is ignored because the application compiled with ENABLE_TEST_CLOUD directive 
#when perform a tap operation on prescription hexagon it crashes
Scenario: Prescription option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Prescription' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: Support option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Support' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: My Diary option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'My Diary' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

# Other element
Scenario: All my daily task option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'All my daily tasks' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	
Scenario: How do you feel today? option enabled
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'Home Normal' page
	When User tap on 'How do you feel today?' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: Settings Option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Settings' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: Profile Option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Profile' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized