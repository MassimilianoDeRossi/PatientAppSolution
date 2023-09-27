Feature: AllPatientsDailyTasks_item_3783
	As a normal User
	I want to see all my activities for the current Day

Scenario: All my daily tasks option enabled
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'All my daily tasks' option
	Then 'All my daily tasks' page is visualized
	
Scenario: Back option working in All my daily tasks page
	Given User is 'normal'
	And User navigate to 'All my daily tasks' page
	When User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: Today Date is correct in All my daily tasks page
	Given User is 'normal'
	And User navigate to 'All my daily tasks' page
	Then 'Today Date' is correct

Scenario: User cannot set or modify his mood between 00:00 and 09:59
	Given User is 'normal'
	And Device time is '08:00'
	And User navigate to 'All my daily tasks' page
	Then 'How do you feel today?' option is 'not shown'
	And 'Mood Self Assestment' appears '1 times' in 'To do' section
	And 'Mood Self Assestment' appears '0 times' in 'Done' section

Scenario: Only How do you feel today option visualized between 10AM and 12PM and mood not setted
	Given User is 'normal'
	And Device time is '13:00'
	And User navigate to 'All my daily tasks' page
	Then 'How do you feel today?' option is 'shown'
	And 'Mood Self Assestment' appears '1 times' in 'To do' section
	And 'Mood Self Assestment' appears '0 times' in 'Done' section

Scenario: Only Today I feel option visualized between 10AM and 12AM and mood setted
	Given User is 'normal'
	And Device time is '18:00'
	And User navigate to 'All my daily tasks' page
	When User 'set and save' mood with 'Happy'
	Then  'How do you feel today?' option is 'not shown'
	And 'Today I feel' option is 'shown'
	And 'Mood Self Assestment' appears '0 times' in 'To do' section
	And 'Mood Self Assestment' appears '1 times' in 'Done' section

Scenario: Correct activities shown in To do and Done section
	Given The user access with QrCode 'Prescription1' at time '11:00'
	And User navigate to 'All my daily tasks' page
	Then 'To do' section counter is correct
	And 'Struts Adjustment A' appears '4 times' in 'To do' section
	And 'Struts Adjustment B' appears '2 times' in 'To do' section
	And 'Struts Adjustment C' appears '2 times' in 'To do' section
	And 'Mood Self Assestment' appears '1 times' in 'To do' section
	And 'Done' section counter is correct
	And 'Mood Self Assestment' appears '0 times' in 'Done' section 
	And 'Struts Adjustment A' appears '0 times' in 'Done' section
	And 'Struts Adjustment B' appears '0 times' in 'Done' section
	And 'Struts Adjustment C' appears '0 times' in 'Done' section

@Failed
Scenario: Correct activities shown in To do and Done section after doing activities that can be done 
	Given The user access with QrCode 'Prescription1' at time '11:00'
	And User navigate to 'All my daily tasks' page
	When User do activities that can be done
	Then 'To do' section counter is correct
	And 'Struts Adjustment A' appears '1 times' in 'To do' section
	And 'Struts Adjustment B' appears '2 times' in 'To do' section
	And 'Struts Adjustment C' appears '1 times' in 'To do' section
	And 'Mood Self Assestment' appears '0 times' in 'To do' section
	And 'Done' section counter is correct
	And 'Mood Self Assestment' appears '1 times' in 'Done' section
	And 'Struts Adjustment A' appears '3 times' in 'Done' section
	And 'Struts Adjustment B' appears '0 times' in 'Done' section
	And 'Struts Adjustment C' appears '1 times' in 'Done' section