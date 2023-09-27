Feature: AllTextMyHEXPlanApp
	As a user
	I want to test if all the text are correct 
	According to the phone language

Scenario: Correct text Start your treatment page
	Given User is 'anonymous'
	Given User navigate to 'Start your treatment' page
	Then Text with id 'LblWithPrescription' is correct
	And Text with id 'LblNoPrescription' is correct

# Set user profile anonymous

Scenario: Correct text User profile anonymous without photo
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	Then Text with id 'LblPreferences' is correct
	And Text with id 'LblSelectPhoto_0' is correct

Scenario: Correct text Pin site care anonymous
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	Then Text with id 'LblDailyTimeInfo' is correct
	And Text with id 'LblPreferences' is correct

# Set user profile normal

Scenario: Correct text Welcome User normal
	Given User is 'normal'
	And User navigate to 'Welcome User Normal' page
	Then Text with id 'LblInfoMessage' is correct
	And Text with id 'LblInfoPref' is correct

Scenario: Correct text User profile normal
	Given User is 'normal'
	And User navigate to 'User Profile' page
	Then Text with id 'LblPreferences' is correct

Scenario: Correct text Pin site care normal
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	Then Text with id 'LblDailyTimeInfo' is correct
	And Text with id 'LblPreferences' is correct

Scenario: Correct text Personal Goal normal
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	Then Text with id 'LblPreferences' is correct
	And Text with id 'LblJourneyRemind' is correct
	And Text with id 'LblDisableGoal' is correct
	And Text with id 'LblSetPersonalGoal' is correct

Scenario: Correct text Insights Messages normal
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	Then Text with id 'LblInsights' is correct
	And Text with id 'LblSetDailyTime' is correct
	And Text with id 'LblPreferenceInfo' is correct
	And Text with id 'LblGeneralInfo' is correct

# All patient daily task

Scenario: Correct text All my daily task normal
	Given User is 'normal'
	And User navigate to 'All my daily tasks' page
	Then Text with id 'LblToDoTitle' is correct
	And Text with id 'LblDoneTitle' is correct

# Da self test survey

Scenario: Correct text How do you feel today normal without Goal and mood not set
	Given User is 'normal'
	And Device time is '18:00'
	And User navigate to 'How do you feel today?' page
	Then Text with id 'LblClickOnEmoticons' is correct
	And Text with id 'LblGeneralMessage' is correct
	And Text with id 'LblDontGiveUp' is correct

Scenario: Correct text How do you feel today normal with Goal and mood set
	Given User is 'normal' 
	And Device time is '18:00'
	When User 'set and save' profile with
	| nickname | timePinSite | mytext          | timeInsigth | GoalStat | InsightsStat |
	| myNick   | empty       | this is my text | empty       | enabled  | enabled      |
	And User tap on 'How do you feel today?' option
	And User enter 'Mood' with 'Nervous'
	And User tap on 'Confirm this mood' option
	Then Text with id 'LblClickOnEmoticons' is correct
	And Text with id 'LblGeneralMessage' is correct
	And Text with id 'LblDontGiveUp' is correct
	And The 'Goal Text' should be 'this is my text'