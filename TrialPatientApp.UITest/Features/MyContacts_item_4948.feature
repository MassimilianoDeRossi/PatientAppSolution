Feature: MyContacts_item_4948
	After skipping the first profile set-up
	I want to add/change my profile set-up individually

# Objective a

Scenario: Anonymous User access to User details page without first setup
	Given User is 'anonymous'
	When User tap on 'Profile' option
	Then 'User Details' page is visualized
	And The 'nickname' should be 'empty'

Scenario: Anonymous User add his nickname
	Given User is 'anonymous'
	And User navigate to 'User Details' page
	When User enter 'nickname' with '-myNickname-'
	Then The 'nickname' should be '-myNickname-'

Scenario: Max Anonymous nickname lenght without first setup
	Given User is 'anonymous'
	And User navigate to 'User Details' page
	When User enter 'nickname' with '12345678901234567890'
	And User tap on 'a char' option
	Then The 'nickname' should be '12345678901234567890'

Scenario: Not enabled chars without first setup anonymous
	Given User is 'anonymous'
	And User navigate to 'User Details' page
	When User enter 'nickname' with 'ciao<>%#&?'
	Then The 'nickname' should be 'ciao'

# Objective b

Scenario: Anonymous User access to User details page with first setup
	Given User is 'anonymous'
	When User 'set and save' profile with
         | nickname  | time  |
         | _Nickname | empty |
	And User tap on 'Profile' option
	Then 'User Details' page is visualized
	And The 'nickname' should be '_Nickname'

Scenario: Anonymous User update his nickname
	Given User is 'anonymous'
	When User 'set and save' profile with
         | nickname       | time  |
         | first_Nickname | empty |
	And User tap on 'Profile' option
	And User enter 'nickname' with 'second_Nickname'
	Then The 'nickname' should be 'second_Nickname'

Scenario: Max Anonymous nickname lenght with first setup
	Given User is 'anonymous'
	When User 'set and save' profile with
         | nickname             | time   |
         | 12345678901234567890 | empty |
	And User tap on 'Profile' option
	And User tap on 'a char' option
	Then The 'nickname' should be '12345678901234567890'

Scenario: Not enabled chars with first setup anonymous
	Given User is 'anonymous'
	When User 'set and save' profile with
         | nickname  | time  |
         | justAname | empty |
	And User tap on 'Profile' option
	And User enter 'nickname' with 'ciao<>%#&?'
	Then The 'nickname' should be 'ciao'

# Objective c

Scenario: Normal User access to User details page without first setup
	Given User is 'normal'
	And User navigate to 'Home Normal' page
	When User tap on 'Profile' option
	Then 'User Details' page is visualized
	And The 'nickname' should be 'empty'

Scenario: Normal User add his nickname
	Given User is 'normal'
	And User navigate to 'User Details' page
	When User enter 'nickname' with '-myNickname-'
	Then The 'nickname' should be '-myNickname-'

Scenario: Max Normal nickname lenght without first setup
	Given User is 'normal'
	And User navigate to 'User Details' page
	When User enter 'nickname' with '12345678901234567890'
	And User tap on 'a char' option
	Then The 'nickname' should be '12345678901234567890'

Scenario: Not enabled chars without first setup normal
	Given User is 'normal'
	And User navigate to 'User Details' page
	When User enter 'nickname' with 'ciao<>%#&?'
	Then The 'nickname' should be 'ciao'

# Objective d

Scenario: Normal User access to User details page with first setup
	Given User is 'normal'
	When User 'set and save' profile with
    | nickname | timePinSite | mytext          | timeInsigth | GoalStat | InsightsStat |
	| myNick   | empty       | this is my text | empty       | enabled  | enabled      |
	And User tap on 'Profile' option
	Then 'User Details' page is visualized
	And The 'nickname' should be 'myNick'

Scenario: Normal User update his nickname
	Given User is 'normal'
	When User 'set and save' profile with
    | nickname | timePinSite | mytext          | timeInsigth | GoalStat | InsightsStat |
	| myNick   | empty       | this is my text | empty       | enabled  | enabled      |
	And User tap on 'Profile' option
	And User enter 'nickname' with 'second_Nickname'
	Then The 'nickname' should be 'second_Nickname'

Scenario: Max Normal nickname lenght with first setup
	Given User is 'normal'
	When User 'set and save' profile with
    | nickname             | timePinSite | mytext          | timeInsigth | GoalStat | InsightsStat |
    | 12345678901234567890 | empty       | this is my text | empty       | enabled  | enabled      |
	And User tap on 'Profile' option
	And User tap on 'a char' option
	Then The 'nickname' should be '12345678901234567890'

Scenario: Not enabled chars with first setup normal
	Given User is 'normal'
	When User 'set and save' profile with
    | nickname | timePinSite | mytext          | timeInsigth | GoalStat | InsightsStat |
	| myNick   | empty       | this is my text | empty       | enabled  | enabled      |
	And User tap on 'Profile' option
	And User enter 'nickname' with 'ciao<>%#&?'
	Then The 'nickname' should be 'ciao'

#Objective e

Scenario: When user set-up with anonymous and became normal then nickname is not lost
	Given User is 'anonymous'
	When User 'set and save' profile with
         | nickname  | time  |
         | _Nickname | empty |
	And User became normal
	When User tap on 'Profile' option
	Then 'User Details' page is visualized
	And The 'nickname' should be '_Nickname'