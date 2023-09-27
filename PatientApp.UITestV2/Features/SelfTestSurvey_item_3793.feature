Feature: SelfTestSurvey_item_3793
	As a Normal User
	I want to select my daily mood

# 00:00<=hour<10:00

Scenario: How do you feel not visualized between 00:00 AM and 10:00 AM in Home Normal page
	Given User is 'normal'
	And Device time is '08:00'
	And User navigate to 'Home Normal' page
	Then 'How do you feel today?' option is 'not shown'

Scenario: How do you feel not visualized between 00:00 AM and 10:00 AM in All my daily task page
	Given User is 'normal'
	And Device time is '08:00'
	And User navigate to 'All my daily tasks' page
	Then 'How do you feel today?' option is 'not shown'

# 10:00<=hour<00:00

Scenario: How do you feel visualized between 10:00 AM and 11:59 PM in Home Normal page
	Given User is 'normal'
	And Device time is '18:00'
	And User navigate to 'Home Normal' page
	Then 'How do you feel today?' option is 'shown'

Scenario: How do you feel visualized between 10:00 AM and 11:59 PM in All my daily task page
	Given User is 'normal'
	And Device time is '18:00'
	And User navigate to 'All my daily tasks' page
	Then 'How do you feel today?' option is 'shown'

# Navigation from Home page

Scenario: User tap How do you feel today in Home Normal page
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'Home Normal' page
	When User tap on 'How do you feel today?' option
	Then 'How do you feel today?' page is visualized

Scenario: User tap '<' after How do you feel today in Home Normal page
	Given User is 'normal'
	And Device time is '20:00'
	And User navigate to 'Home Normal' page
	When User tap on 'How do you feel today?' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

Scenario: User tap on Today I feel option in Home Normal page
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with 'Nervous'
	And User tap on 'Today I feel' option
	Then 'How do you feel today?' page is visualized

Scenario: User tap '<' option after Today I feel in Home Normal page
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with 'Annoyed'
	And User tap on 'Today I feel' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized

# Navigation from All my daily task

Scenario: User tap How do you feel today in All my daily task page
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'All my daily tasks' page
	When User tap on 'How do you feel today?' option
	Then 'How do you feel today?' page is visualized

Scenario: User tap '<' after How do you feel today in All my daily task page
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'All my daily tasks' page
	When User tap on 'How do you feel today?' option
	And User tap on '<' option
	Then 'All my daily tasks' page is visualized

Scenario: User tap on Today I feel option in All my daily tasks page
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'All my daily tasks' page
	When User 'set and save' mood with 'Happy'
	And User tap on 'Today I feel' option
	Then 'How do you feel today?' page is visualized

Scenario: User tap '<' option after Today I feel in All my daily tasks page
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'All my daily tasks' page
	When User 'set and save' mood with 'Excited'
	And User tap on 'Today I feel' option
	And User tap on '<' option
	Then 'All my daily tasks page' page is visualized

# Cancellation of mood update

Scenario: User tap Today I feel option after mood update Confirmation
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with 'Bored'
	And User 'update and save' mood with 'Happy'
	And User tap on 'Today I feel' option
	Then 'How do you feel today?' page is visualized

Scenario: User tap Today I feel option after mood update Cancellation
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with 'Bored'
	And User 'update and not save' mood with 'Happy'
	And User tap on 'Today I feel' option
	Then 'How do you feel today?' page is visualized

# Mood selection discarded

Scenario Outline: User change Mood with Select Mood option and tap Cancel
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'How do you feel today?' page
	When User enter 'Mood' with <MoodName>
	And User tap on 'Cancel Mood' option
	Then The 'Frame' should be 'disappear'
	And The 'Mood' should be 'empty'
Examples: 
| MoodName     |
| 'Frustrated' |
| 'Nervous'    |
#| 'Sad'        |
#| 'Bored'      |
#| 'IamOK'    |
#| 'Calm'       |
#| 'Optimistic' |
#| 'Happy'      |
#| 'Encouraged' |

# Mood set

Scenario Outline: User change Mood with Select Mood option and tap Confirm this mood 
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'How do you feel today?' page
	When User enter 'Mood' with <MoodName>
	And User tap on 'Confirm this mood' option
	Then The 'Frame' should be 'disappear'
	And The 'Mood' should be <MoodName>
	And Text with id <TextMoodId> is correct
Examples: 
| MoodName     | TextMoodId          |
| 'Frustrated' | 'TxtFrustratedMood' |
| 'Nervous'    | 'TxtNervousMood'    |
#| 'Sad'        | 'TxtSadMood'        |
#| 'Bored'      | 'TxtBoredMood'      |
#| 'IamOK'    | 'TxtIamOKMood'      |
#| 'Calm'       | 'TxtCalmMood'       |
#| 'Optimistic' | 'TxtOptimisticMood' |
#| 'Happy'      | 'TxtHappyMood'      |
#| 'Encouraged' | 'TxtEncouragedMood' |

Scenario Outline: User tap < option after setting the mood passing by Home Normal
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'Home Normal' page
	When User tap on 'How do you feel today?' option
	When User enter 'Mood' with <MoodName>
	And User tap on 'Confirm this mood' option
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And 'How do you feel today?' option is 'not shown'
	And The 'Today I feel' should be <MoodName>
Examples: 
| MoodName     |
| 'Frustrated' |
| 'Nervous'    |
#| 'Sad'        |
#| 'Bored'      |
#| 'IamOK'    |
#| 'Calm'       |
#| 'Optimistic' |
#| 'Happy'      |
#| 'Encouraged' |

Scenario Outline: User tap < option after setting the mood passing by All my daily tasks
	Given User is 'normal'
	And Device time is '21:00'
	And User navigate to 'How do you feel today?' page
	When User enter 'Mood' with <MoodName>
	And User tap on 'Confirm this mood' option
	And User tap on '<' option
	Then 'All my daily tasks' page is visualized
	And 'How do you feel today?' option is 'not shown'
	And The 'Today I feel' should be <MoodName>
Examples: 
| MoodName     |
| 'Frustrated' |
| 'Nervous'    |
#| 'Sad'        |
#| 'Bored'      |
#| 'IamOK'    |
#| 'Calm'       |
#| 'Optimistic' |
#| 'Happy'      |
#| 'Encouraged' |

# Mood update cancelled

Scenario Outline: User change Mood with Select Mood option and Cancel with Mood inserted
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with <MoodName_before>
	And User tap on 'Today I feel' option
	And User enter 'Mood' with <MoodName_after>
	And User tap on 'Cancel Mood' option
	Then The 'Frame' should be 'disappear'
	And The 'Mood' should be <MoodName_before>
Examples: 
| MoodName_before | MoodName_after |
| 'Frustrated'    | 'Nervous'      |
| 'Nervous'       | 'Sad'          |
#| 'Sad'           | 'Bored'        |
#| 'Bored'         | 'IamOK'      |
#| 'IamOK'       | 'Calm'         |
#| 'Calm'          | 'Optimistic'   |
#| 'Optimistic'    | 'Happy'        |
#| 'Happy'         | 'Encouraged'   |
#| 'Encouraged'    | 'Frustrated'   |

Scenario Outline: User tap < option after mood update cancellation passing by Home Normal
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with <MoodName_before>
	And User tap on 'Today I feel' option
	And User 'not update' mood with <MoodName_after>
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Today I feel' should be <MoodName_before>
Examples: 
| MoodName_before | MoodName_after |
| 'Frustrated'    | 'Nervous'      |
| 'Nervous'       | 'Sad'          |
#| 'Sad'           | 'Bored'        |
#| 'Bored'         | 'IamOK'      |
#| 'IamOK'       | 'Calm'         |
#| 'Calm'          | 'Optimistic'   |
#| 'Optimistic'    | 'Happy'        |
#| 'Happy'         | 'Encouraged'   |
#| 'Encouraged'    | 'Frustrated'   |

Scenario Outline: User tap < option after mood update cancellation passing by All my daily tasks
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'All my daily tasks' page
	When User 'set and save' mood with <MoodName_before>
	And User tap on 'Today I feel' option
	And User 'not update' mood with <MoodName_after>
	And User tap on '<' option
	Then 'All my daily tasks' page is visualized
	And The 'Today I feel' should be <MoodName_before>
Examples: 
| MoodName_before | MoodName_after |
| 'Frustrated'    | 'Nervous'      |
| 'Nervous'       | 'Sad'          |
#| 'Sad'           | 'Bored'        |
#| 'Bored'         | 'IamOK'      |
#| 'IamOK'       | 'Calm'         |
#| 'Calm'          | 'Optimistic'   |
#| 'Optimistic'    | 'Happy'        |
#| 'Happy'         | 'Encouraged'   |
#| 'Encouraged'    | 'Frustrated'   |

# Mood update

Scenario Outline: User change Mood with Select Mood option and Confirm with Mood inserted
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with <MoodName_before>
	And User tap on 'Today I feel' option
	And User enter 'Mood' with <MoodName_after>
	And User tap on 'Confirm this mood' option
	Then The 'Frame' should be 'disappear'
	And '<' option is 'shown'
	And The 'Mood' should be <MoodName_after>
Examples: 
| MoodName_before | MoodName_after |
| 'Frustrated'    | 'Nervous'      |
| 'Nervous'       | 'Sad'          |
#| 'Sad'           | 'Bored'        |
#| 'Bored'         | 'IamOK'      |
#| 'IamOK'       | 'Calm'         |
#| 'Calm'          | 'Optimistic'   |
#| 'Optimistic'    | 'Happy'        |
#| 'Happy'         | 'Encouraged'   |
#| 'Encouraged'    | 'Frustrated'   |

Scenario Outline: User tap < option after mood update Confirmation passing by Home Normal
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'Home Normal' page
	When User 'set and save' mood with <MoodName_before>
	And User tap on 'Today I feel' option
	And User 'update' mood with <MoodName_after>
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Today I feel' should be <MoodName_after>
Examples: 
| MoodName_before | MoodName_after |
| 'Frustrated'    | 'Nervous'      |
| 'Nervous'       | 'Sad'          |
#| 'Sad'           | 'Bored'        |
#| 'Bored'         | 'IamOK'      |
#| 'IamOK'       | 'Calm'         |
#| 'Calm'          | 'Optimistic'   |
#| 'Optimistic'    | 'Happy'        |
#| 'Happy'         | 'Encouraged'   |
#| 'Encouraged'    | 'Frustrated'   |

Scenario Outline: User tap < option after mood update Confirmation passing by All my daily tasks
	Given User is 'normal'
	And Device time is '19:00'
	And User navigate to 'All my daily tasks' page
	When User 'set and save' mood with <MoodName_before>
	And User tap on 'Today I feel' option
	And User 'update' mood with <MoodName_after>
	And User tap on '<' option
	Then 'All my daily tasks' page is visualized
	And The 'Today I feel' should be <MoodName_after>
Examples: 
| MoodName_before | MoodName_after |
| 'Frustrated'    | 'Nervous'      |
| 'Nervous'       | 'Sad'          |
#| 'Sad'           | 'Bored'        |
#| 'Bored'         | 'IamOK'      |
#| 'IamOK'       | 'Calm'         |
#| 'Calm'          | 'Optimistic'   |
#| 'Optimistic'    | 'Happy'        |
#| 'Happy'         | 'Encouraged'   |
#| 'Encouraged'    | 'Frustrated'   |

# Anonymous User

Scenario: Anoymous User cannot set his mood  between 00:00 AM and 10:00 AM
	Given User is 'anonymous'
	And Device time is '8:00'
	And User navigate to 'Home Anonymous' page
	Then 'How do you feel today?' option is 'not shown'

Scenario: Anoymous User cannot set his mood between 10:00 AM and 12:00 PM
	Given User is 'anonymous'
	And Device time is '21:00'
	And User navigate to 'Home Anonymous' page
	Then 'How do you feel today?' option is 'not shown'