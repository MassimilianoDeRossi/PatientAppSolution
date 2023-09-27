Feature: SetUserProfileAnonymous_item_3776
	In order to set user profile
	As an anonymous user of myHEXplan app
	I want to configure User Profile settings
	Rules:
		nickname max 20 chars

# Welcome User anonymous page

Scenario: User tap and confirm exit option in Welcome User Anonymous page
	Given User is 'anonymous'
	And User navigate to 'Welcome User Anonymous' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Anonymous' page is visualized

Scenario: User tap but not confirm exit option in Welcome User Anonymous page
	Given User is 'anonymous'
	And User navigate to 'Welcome User Anonymous' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'Welcome User Anonymous' page is visualized

Scenario: User tap next option in Welcome User Anonymous page
	Given User is 'anonymous'
	And User navigate to 'Welcome User Anonymous' page
	When User tap on 'Next' option
	Then 'User Profile' page is visualized

# User Profile page

Scenario: User tap back option in User Profile page
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User tap on 'Back' option
	Then 'Welcome User Anonymous' page is visualized

Scenario: User tap next option in User Profile page
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User tap on 'Next' option
	Then 'Pin Site Care' page is visualized

Scenario: User tap and confirm exit option in User Profile page
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Anonymous' page is visualized

Scenario: User tap but not confirm exit option in User Profile page
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'User Profile' page is visualized

Scenario Outline: User tap exit option in User Profile page with data entered
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with <myname>
	And User tap on 'Exit' option
	And User 'confirm' to Exit
	And User navigate to 'User Profile' page from 'Home Anonymous' page
	Then The 'nickname' should be 'empty'
Examples: 
| myname   |
| 'Davide' |
| 'Diego'  |
| 'empty'  |

Scenario: Verify default values in User Profile page
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	Then The 'nickname' should be 'empty'
	And The 'photo' should be 'empty'

Scenario Outline: User wants to set nickname
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with <myname>
	Then The 'nickname' should be <myname>
Examples: 
| myname            |
| 'Davide'          |
| 'Diego'           |
| 'empty'           |
| '§§Tizio -.-" °!' |

Scenario: User wants to set nickname with special <>%#&? chars
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with '<>%#&?'
	Then The 'nickname' should be 'empty'

Scenario: User wants to set nickname with more than 20 chars
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with '12345678901234567890'
	And User tap on 'a char' option
	Then The 'nickname' should be '12345678901234567890'

# Pin Site Care page

Scenario Outline: User tap back option in Pin Site Care page with data entered
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with <nickname>
	And User navigate to 'Pin Site Care' page from 'User Profile' page
	And User tap on 'Back' option
	Then 'User Profile' page is visualized
	And The 'nickname' should be <nickname>
Examples:
| nickname    |
| 'Davide123' |
| 'empty'     |

Scenario: User tap and confirm exit option in Pin Site Care page
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Exit' option
	And User 'confirm' to Exit
	Then 'Home Anonymous' page is visualized

Scenario: User tap but not confirm exit option in Pin Site Care page
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Exit' option
	And User 'not confirm' to Exit
	Then 'Pin Site Care' page is visualized

Scenario Outline: User tap exit option in Pin Site Care page with data entered 
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with <nickname>
	And User navigate to 'Pin Site Care' page from 'User Profile' page
	And User tap on 'Exit' option
	And User 'confirm' to Exit
	And User navigate to 'User Profile' page from 'Home Anonymous' page
	Then The 'nickname' should be 'empty'
Examples: 
| nickname |
| 'Enrico' |
| 'empty'  |

Scenario: Verify default values in Pin Site Care page
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	Then The 'set daily time' should be '9:00 AM'

Scenario: Successful profile settings using default values
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User tap on 'Done' option
	Then 'Home Anonymous' page is visualized
	And Anonymous Profile settings are successfully saved
	| nickname | time    |
	| empty    | 9:00 AM |

Scenario: Successful profile settings using not default values
	Given User is 'anonymous'
	When User 'set and save' profile with
	| nickname   | time  |
	| MyNickname | 18:40 |
	Then 'Home Anonymous' page is visualized
	And Anonymous Profile settings are successfully saved
	| nickname   | time    |
	| MyNickname | 6:40 PM |

Scenario: Successful profile settings update
	Given User is 'anonymous'
	When User 'set and save' profile with
	| nickname   | time  |
	| Mynickname | 15:45 |
	When User 'update' profile with
	| nickname | time  |
	| newNick  | 17:32 |
	Then Anonymous Profile settings are successfully saved
	| nickname | time    |
	| newNick  | 5:32 PM |

Scenario Outline: Verify correct format hour in AM/PM anonymous user
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User enter 'set daily time' with <hour>
	Then The 'set daily time' should be <format>
Examples:
| hour    | format |
| '10:00' | 'AM'   |
| '03:50' | 'AM'   |
| '17:34' | 'PM'   |
| '23:43' | 'PM'   |

Scenario: User want to change set daily time N times
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User enter 'set daily time' with '06:20'
	Then The 'set daily time' should be '6:20 AM'
	When User enter 'set daily time' with '10:40'
	Then The 'set daily time' should be '10:40 AM'
	When User enter 'set daily time' with '11:02'
	Then The 'set daily time' should be '11:02 AM'
	When User enter 'set daily time' with '14:02'
	Then The 'set daily time' should be '2:02 PM'
	When User enter 'set daily time' with '16:44'
	Then The 'set daily time' should be '4:44 PM'
	When User enter 'set daily time' with '21:07'
	Then The 'set daily time' should be '9:07 PM'
	When User enter 'set daily time' with '22:33'
	Then The 'set daily time' should be '10:33 PM'

# Swipe

Scenario: Swiping right with data entered in User Profile not lost data
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with 'myNick'
	And User swipe from 'left' to 'right'
	And User swipe from 'right' to 'left'
	Then The 'nickname' should be 'myNick'

Scenario: Swiping right with data entered in Pin Site Care not lost data
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User enter 'set daily time' with '06:20'
	And User swipe from 'left' to 'right'
	And User navigate to 'Pin Site Care' page from 'User Profile' page
	Then The 'set daily time' should be '6:20 AM'

Scenario: Swiping left with data entered in User Profile not lost
	Given User is 'anonymous'
	And User navigate to 'User Profile' page
	When User enter 'nickname' with 'myNick'
	And User swipe from 'right' to 'left'
	And User swipe from 'left' to 'right'
	Then The 'nickname' should be 'myNick'

Scenario: Successful navigation swapping from right to left
	Given User is 'anonymous'
	And User navigate to 'Welcome User Anonymous' page
	When User swipe from 'right' to 'left'
	And User swipe from 'right' to 'left'
	Then 'Pin Site Care' page is visualized

Scenario: Successful navigation swapping from left to right
	Given User is 'anonymous'
	And User navigate to 'Pin Site Care' page
	When User swipe from 'left' to 'right'
	And User swipe from 'left' to 'right'
	Then 'Welcome User Anonymous' page is visualized