Feature: SetUserProfileNormal_item_4196
	In order to set user profile
	As a logged Patient of myHEXplan app
	I want to configure User Profile settings related to the treatment
	Rules:
		user logged
		at least one prescription scanned
		nickname max 20 chars
		personal goal max 200 chars

# Welcome User normal page

Scenario: User tap and confirm exit option in Welcome User Normal page
	Given User is 'normal'
	And User navigate to 'Welcome User Normal' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Normal' page is visualized

Scenario: User tap and not confirm exit option in Welcome User Normal page
	Given User is 'normal'
	And User navigate to 'Welcome User Normal' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'Welcome User Normal' page is visualized

Scenario: User tap next option in Welcome User Normal page
	Given User is 'normal'
	And User navigate to 'Welcome User Normal' page
	When User tap on 'Next' option
	Then 'User Profile' page is visualized

# User Profile page

Scenario: User tap back option in User Profile page
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User tap on 'Back' option
	Then 'Welcome User Normal' page is visualized

Scenario: User tap next option in User Profile page
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User tap on 'Next' option
	Then 'Pin Site Care' page is visualized

Scenario: User tap and confirm exit option in User Profile page without data entered
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Normal' page is visualized

Scenario: User tap and not confirm exit option in User Profile page without data entered
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'User Profile' page is visualized

Scenario: Verify default values in User Profile page
	Given User is 'normal'
	And User navigate to 'User Profile' page
	Then The 'nickname' should be 'empty'

Scenario: User wants to set nickname
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with 'myN - . à ick'
	Then The 'nickname' should be 'myN - . à ick'

Scenario: User wants to set nickname with special <>%#&? chars
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with '<>%#&?'
	Then The 'nickname' should be 'empty' 

Scenario: User wants to set nickname with more than 20 chars
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with '12345678901234567890'
	And User tap on 'a char' option
	Then The 'nickname' should be '12345678901234567890'

Scenario Outline: Next/Back option not lose User data in User Profile page
	Given User is 'normal'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with 'myNick'
	And User tap on <first> option
	And User tap on <second> option
	Then The 'nickname' should be 'myNick'
Examples:
| first  | second |
| 'Back' | 'Next' |
| 'Next' | 'Back' |

# Pin Site Care page

Scenario: User tap back option in Pin Site Care page
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Back' option
	Then 'User Profile' page is visualized

Scenario: User tap next option in Pin Site Care page
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Next' option
	Then 'Personal Goal' page is visualized

Scenario: User tap and confirm exit option in Pin Site Care page
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Normal' page is visualized

Scenario: User tap and not confirm exit option in Pin Site Care page
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'Pin Site Care' page is visualized

Scenario: User wants to set daily time in Pin Site Care page 
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	When User enter 'set daily time' with '10:21'
	Then The 'set daily time' should be '10:21 AM'

Scenario: Verify default values in Pin Site Care page
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	Then The 'set daily time' should be '9:00 AM'

Scenario Outline: Verify correct format hour in AM/PM normal user
	Given User is 'normal'
	And User navigate to 'Pin Site Care' page
	When User enter 'set daily time' with <hour>
	Then The 'set daily time' should be <format>
Examples:
| hour    | format |
| '10:00' | 'AM'   |
| '03:50' | 'AM'   |
| '17:34' | 'PM'   |
| '23:43' | 'PM'   |

# Personal Goal page

Scenario: Verify default values in Personal Goal page
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	Then The 'Goal' should be 'enabled'
	And The 'set your personal goal' should be 'empty'

Scenario: User tap back option in Personal Goal page
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	When User tap on 'Back' option
	Then 'Pin Site Care' page is visualized

Scenario: User tap next option in Personal Goal page with goal disabled
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'disabled'
	When User tap on 'Next' option
	Then 'Insights Messages' page is visualized 

Scenario: User tap next option in Personal Goal page with goal enabled and empty
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'enabled'
	When User enter 'set your personal goal' with 'empty'
	And User tap on 'Next' option
	Then 'Alert message' appears
	And 'Personal Goal' page is visualized 

Scenario: User tap next option in Personal Goal page with goal enabled and not empty
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'enabled'
	When User enter 'set your personal goal' with 'my own personal text'
	And User tap on 'Next' option
	Then 'Insights Messages' page is visualized

Scenario: User tap and confirm exit option in Personal Goal page
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Normal' page is visualized

Scenario: User tap and not confirm exit option in Personal Goal page
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'Personal goal' page is visualized

Scenario Outline: User tap on Goal Switch in order to change it
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is <Status1>
	When User tap on 'Goal' option
	Then The 'Goal' should be <Status2>
Examples:
| Status1   | Status2    |
| 'enabled' | 'disabled' |
| 'disabled | 'enabled'  |

Scenario: User wants to set personal goal with goal enabled
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'enabled'
	When User enter 'set your personal goal' with 'my own personal text'
	Then The 'set your personal goal' should be 'my own personal text'

Scenario: User wants to set personal goal with goal disabled
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'disabled'
	When User enter 'set your personal goal' with 'my own personal text'
	Then The 'set your personal goal' should be 'empty'

Scenario: Text in personal goal is read-only if Goal is disabled
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'enabled'
	When User enter 'set your personal goal' with 'my first text'
	And User tap on 'Goal' option
	And User enter 'set your personal goal' with 'my second text'
	Then The 'set your personal goal' should be 'my first text'

Scenario Outline: Back/Next option with data entered not lost data in Personal Goal page
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'enabled'
	When User enter 'set your personal goal' with 'my own personal text'
	And User tap on <first> option
	And User tap on <second> option
	Then The 'Goal' should be 'enabled'
	And The 'set your personal goal' should be 'my own personal text'
Examples:
| first  | second |
| 'Next' | 'Back' |
| 'Back' | 'Next' |

@ignore
Scenario: User want to enter in Personal Goal more than 200 chars
	Given User is 'normal'
	And User navigate to 'Personal Goal' page
	And 'Goal' option is 'enabled'
	When User enter 'set your personal goal' with 'a 200 chars string'
	And User tap on 'a char' option
	Then The 'set your personal goal' should be 'a 200 chars string'

# Insights Messages Page

Scenario: User tap Back option in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	When User tap on 'Back' option
	Then 'Personal Goal' page is visualized

Scenario: Back option temporary save data in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	When User enter 'set daily time' with '10:58'
	And User tap on 'Back' option
	And User tap on 'Next' option
	Then The 'Insights' should be 'enabled'
	And The 'set daily time' should be '10:58 AM'

Scenario: User tap Done option in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	When User tap on 'Done' option
	Then 'Home Normal' page is visualized

Scenario: User tap and confirm Exit option in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Normal' page is visualized

Scenario: User tap and not confirm Exit option in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'Insights Messages' page is visualized

Scenario Outline: User tap on Insights Switch in order to change it
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	And 'Insights' option is <Status1>
	When User tap on 'Insights' option
	Then The 'Insights' should be <Status2>
Examples:
| Status1    | Status2    |
| 'enabled'  | 'disabled' |
| 'disabled' | 'enabled'  |

Scenario: Verify default value in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	Then The 'set daily time' should be '9:00 AM'
	And The 'Insights' should be 'enabled'

Scenario Outline: User want to change set daily time in Insights Messages page
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	And 'Insights' option is <Status>
	When User enter 'set daily time' with <TimeEnter>
	Then The 'set daily time' should be <TimeCheck>
Examples: 
| Status     | TimeEnter | TimeCheck |
| 'enabled'  | '01:33'   | '1:33 AM' |
| 'disabled' | '09:00'   | '9:00 AM' |

# Swipe

Scenario: Swiping right navigation work correctly
	Given User is 'normal'
	And User navigate to 'Welcome User Normal' page
	When User swipe from 'right' to 'left'
	And User swipe from 'right' to 'left'
	And User swipe from 'right' to 'left'
	And User tap on 'Goal' option
	And User swipe from 'right' to 'left'
	Then 'Insights Messages' page is visualized

Scenario: Swiping left navigation work correctly
	Given User is 'normal'
	And User navigate to 'Insights Messages' page
	When User swipe from 'left' to 'right'
	And User swipe from 'left' to 'right'
	And User swipe from 'left' to 'right'
	And User swipe from 'left' to 'right'
	Then 'Welcome User Normal' page is visualized