Feature: ShoppingList_item_3700
	As a User 
	I want to create the item list of runned out stuffs

# Anonymous User

Scenario: Anonymous User can see correctly shopping list page
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	Then 'Shopping List' page is visualized
	And 'share' option is 'shown'
	And '<' option is 'shown'
	And Shopping List 'image' is 'visualized'
	And Shopping List 'list' is 'visualized'

Scenario: Anonymous User tap < option in Shopping List page
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	When User tap on '<' option
	Then 'Pin site care' page is visualized

Scenario: Anonymous User select all items
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	And All items are 'unselected'
	When User 'select' all items
	Then All item are 'selected'

Scenario: Anonymous User deselect all item
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	And All items are 'selected'
	When User 'deselect' all items
	Then All item are 'deselected'

Scenario: Anonymous User tap i and a frame appear
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	When User tap on 'i' option
	Then Shopping List 'Frame' is 'visualized'
	And 'X' option is 'shown'
	And 'Close' option is 'visualized'

Scenario Outline: Anonymous User enter a list and close Frame (Shopping List) then list remain the same and Frame disappear
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	When User enter 'list' with '3 items'
	And User tap on 'i' option
	And User tap on <this> option
	Then Shopping List 'Frame' is 'not visualized'
	And The 'list' should be 'the same'
Examples:
| this    |
| 'X'     |
| 'Close' |

Scenario: Anonymous User tap cancel before sharing his shopping list
	Given  User is 'anonymous'
	And User navigate to 'Shopping List' page
	When User enter 'list' with '1 item'
	And User tap on 'Share' option
	And User tap on 'Cancel' option
	Then 'Shopping List' page is visualized

# Normal User

Scenario: Normal User can see correctly shopping list page
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	Then 'Shopping List' page is visualized
	And 'share' option is 'shown'
	And '<' option is 'shown'
	And Shopping List 'image' is 'visualized'
	And Shopping List 'list' is 'visualized'

Scenario: Normal User tap < option in Shopping List page
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	When User tap on '<' option
	Then 'Pin site care' page is visualized

Scenario: Normal User select all items
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	And All items are 'unselected'
	When User 'select' all items
	Then All item are 'selected'

Scenario: Normal User deselect all item
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	And All items are 'selected'
	When User 'deselect' all items
	Then All item are 'deselected'

Scenario: Normal User tap i and a frame appear
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	When User tap on 'i' option
	Then Shopping List 'Frame' is 'visualized'
	And 'X' option is 'shown'
	And 'Close' option is 'visualized'

Scenario Outline: Normal User enter a list and close Frame (Shopping List) then list remain the same and Frame disappear
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	When User enter 'list' with '3 items'
	And User tap on 'i' option
	And User tap on <this> option
	Then Shopping List 'Frame' is 'not visualized'
	And The 'list' should be 'the same'
Examples:
| this    |
| 'X'     |
| 'Close' |

Scenario: Normal User tap cancel before sharing his shopping list
	Given  User is 'normal'
	And User navigate to 'Shopping List' page
	When User enter 'list' with '1 item'
	And User tap on 'Share' option
	And User tap on 'Cancel' option
	Then 'Shopping List' page is visualized