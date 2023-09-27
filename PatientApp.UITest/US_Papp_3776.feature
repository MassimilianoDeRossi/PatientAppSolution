#commento1
Feature: US_Papp_3776
	As an anonymous user without a prescription scanned
	I want to test:
	1) complete user profile settings in all situations (with/without photo or nickname)
	2) abandon user profile settings in all situations

Background:
	Given 'Start your treatment' page
	When press 'without prescription' button
	Then 'Welcome to myHEXplan!' page is visualized

Scenario: Complete Set User Profile with nickname and photo 
	Given 'Welcome to myHEXplan!' page
	When press 'Next_Welcome_to_myHEXplan_page'

	And Upload <my_photo> on 'User Profile' page
	And Insert <my_nickname> on 'User Profile' page
	
	And press 'Next_User_Profile_page'
	And set pin site care daily time
	And press 'Start_pin_site_care_page'
	Then User profile is saved
	And 'Home' page is visualized 
	
	#Examples:
 #   | my_photo | my_nickname |
 #   |          |             |
 #   |          |    mario    |
 #   |mario.jpg |             |
	#|mario.jpg |    mario    |
	#|mario.jpg |  (>50 char) |

@mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen