Feature: FirstAccessAsAnonymousUser_item_3778
	In order to use myHEXplan app
	As an Anonymous User
	I want to visualize the anonymous Home page

Scenario: Alert message appears
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Home Normal' page
	Then The 'Alert icon' should be 'visualized'

Scenario: Anonymous user accesses Pin site care page with alert
	Given User is 'anonymous'
	And 'Pin site care' activity 'to do' 
	When User tap on 'Pin site care' option
	Then 'Pin site care' page is visualized
	And '<' option is 'shown'
	And The 'pin site care video' should be 'visualized'
	And 'How will I know if I get an infection?' option is 'shown'
	And 'What do I need?' option is 'shown'
	And Text with id 'LblPinSiteCareHelpVideo' is correct
	And Text with id 'LblHelpInfectionText' is correct
	And Text with id 'LblHelpListText' is correct
	And Text with id 'LblHelpVideoText' is correct
	And 'Confirm pin site care' option is 'shown'
	And 'Cancel pin site care' option is 'shown'

Scenario: Alert icon disappear when user complete all the adjustment
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then The 'Alert icon' should be 'not visualized'