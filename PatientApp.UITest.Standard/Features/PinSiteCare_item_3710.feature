Feature: PinSiteCare_item_3710
	In order to clean my struts
	As a generic User
	I access to Pin Site Care page

# Objective a

Scenario: Anonymous user accesses Pin site care page without alert
	Given User is 'anonymous'
	And 'Pin site care' activity 'not to do' 
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

Scenario: Anonymous user tap back option in Pin site care page without alert
	Given User is 'anonymous'
	And 'Pin site care' activity 'not to do'
	And User navigate to 'Pin site care' page
	When User tap on '<' option
	Then 'Home Anonymous' page is visualized
	And The 'pin site care alert' should be 'not visualized'

Scenario: Anonymous user tap on the option in order to show infection info
	Given User is 'anonymous'
	And 'Pin site care' activity ' not to do' 
	And User navigate to 'Pin site care' page
	When User tap on 'How will I know if I get an infection?' option
	Then 'Am I getting an infection?' page is visualized
	And Text with id 'LblInfectionQuestion1' is correct
	And Text with id 'LblInfectionQuestion2' is correct
	And Text with id 'LblInfectionQuestion3' is correct
	And Text with id 'LblInfectionQuestion4' is correct
	And Text with id 'LblInfectionQuestion5' is correct
	And '<' option is 'shown'
	And 'Send a photo' option is 'shown'
	And Text with id 'LblGeneralMessageInfection' is correct

Scenario: Anonymous user tap on the option in order to show items for pin site care
	Given User is 'anonymous'
	And 'Pin site care' activity ' not to do'  
	And User navigate to 'Pin site care' page
	When User tap on 'What do I need?' option
	Then 'Shopping List' page is visualized

# Objective c

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

Scenario: Anonymous user tap back option in Pin site care page with alert
	Given User is 'anonymous'
	And 'Pin site care' activity 'to do' 
	And User navigate to 'Pin site care' page
	When User tap on '<' option
	Then 'Home Anonymous' page is visualized
	And The 'pin site care alert' should be 'visualized'

Scenario: Anonymous user confirm pin site care activity
	Given User is 'anonymous'
	And 'Pin site care' activity 'to do' 
	And User navigate to 'Pin site care' page
	When User tap on 'Confirm pin site care' option
	Then 'Home Anonymous' page is visualized
	And The 'pin site care alert' should be 'not visualized'

Scenario: Anonymous user cancel pin site care activity
	Given User is 'anonymous'
	And 'Pin site care' activity 'to do' 
	And User navigate to 'Pin site care' page
	When User tap on 'Cancel pin site care' option
	Then 'Home Anonymous' page is visualized
	And The 'pin site care alert' should be 'visualized'