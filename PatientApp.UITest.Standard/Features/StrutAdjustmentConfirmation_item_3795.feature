Feature: StrutAdjustmentConfirmation_item_3795
	In order to make my assignment
	As a Normal User
	I want to execute my own struts adjustment

# Objective a

Scenario: Alert message appears
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Home Normal' page
	Then The 'Alert icon' should be 'visualized'

Scenario: User tap on Strut adjust hexagon
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Home Normal' page
	When User tap on 'Strut Adjust' option
	Then 'Strut Adjust' page is visualized
	And Text with id 'LbFrameId' is correct
	And Text with id 'LbStrutSite' is correct
	And Text with id 'LbStrutToBeDone' is correct
	And Text with id 'LblHelpVideoTutorial' is correct
	And The 'Video' should be 'visualized'
	Then The 'Frame IDs' should be 'Qr001'
	#Check alphabetic order and number of Frame ID visualized
	And Counter of Frame ID 'all' should be 'Qr001'
	#Check counter of strut adjustment of Frame ID
	And '<' option is 'shown'

Scenario Outline: User check information of Frame ID
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID <FrameID>
	Then The 'FrameIdInfo' should be 'correct'
	And 'FrameIDInfo' page is visualized
Examples: 
| FrameID |
| 'A'     |
#| 'B'     |
#| 'C'     |
#| 'D'     |
#| 'E'     |
#| 'F'     |
#| 'G'     |
#| 'H'     |
#| 'I'     |

Scenario Outline: User tap back option in Frame ID information page
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID <FrameID>
	And User tap on '<' option
	Then 'Strut Adjust' page is visualized
Examples: 
| FrameID |
| 'A'     |
| 'B'     |
| 'C'     |
| 'D'     |
| 'E'     |
| 'F'     |
| 'G'     |
| 'H'     |
| 'I'     |

Scenario: User tap back option in Strut Adjustment page
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on '<' option
	Then 'Alert icon' appears
	And 'Home Normal' page is visualized

Scenario Outline: User tap Do it later option in Frame ID information page
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID <FrameID>
	And User tap on 'Do it later' option
	Then 'Confirm Postpone' option is 'shown'
	And 'Not Confirm Postpone' option is 'shown'
	When User tap on 'Not Confirm Postpone' option
	Then 'FrameIDInfo' page is visualized
Examples: 
| FrameID |
| 'A'     |
#| 'B'     |
#| 'C'     |
#| 'D'     |
#| 'E'     |
#| 'F'     |
#| 'G'     |
#| 'H'     |
#| 'I'     |

Scenario Outline: Information and page option of a Single Strut of Frame ID is correct
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID 'A'
	And User tap on 'Start Adjustment' option
	And User navigate to <Strut n>
	Then <Strut n> page visualized correctly
	And Option page of <Strut n> work correctly
Examples: 
| Strut n   |
| 'Strut 1' |
#| 'Strut 2' |
#| 'Strut 3' |
#| 'Strut 4' |
#| 'Strut 5' |
#| 'Strut 6' |

Scenario:  User ends Strut Adjustment operation by tapping Done
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID 'A'
	And User tap on 'Start Adjustment' option
	And User navigate to 'Strut 6'
	When User tap on 'Done Adjustment' option
	Then Text with id 'LblWellDone' is correct
	And Information about 'End Adjustment' are correct
	And Option page of 'End Adjustment' work correctly

Scenario: User tap back after finish one of two Strut Adjustment of Frame ID A
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	Then 'Strut Adjust' page is visualized
	And Text with id 'LbFrameId' is correct
	And Text with id 'LbStrutSite' is correct
	And Text with id 'LbStrutToBeDone' is correct
	And Text with id 'LblHelpVideoTutorial' is correct
	And The 'Video' should be 'visualized'
	And The 'Frame IDs' should be 'Qr001'
	#Check alphabetic order and number of Frame ID visualized
	And Counter of Frame ID 'A' should be '1'
	#Check counter of strut adjustment of Frame ID
	And '<' option is 'shown'

@Failed
Scenario: User return to Home Normal after finish one of two Strut Adjustment of Frame ID A
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then The 'Alert icon' should be 'visualized'

Scenario: User finish the only Strut Adjustment of Frame ID B
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User 'complete' strut adjustment of 'Frame ID B'
	Then 'Strut Adjust' page is visualized
	And Text with id 'LbFrameId' is correct
	And Text with id 'LbStrutSite' is correct
	And Text with id 'LbStrutToBeDone' is correct
	And Text with id 'LblHelpVideoTutorial' is correct
	And The 'Video' should be 'visualized'
	And The 'Frame IDs' should be 'Qr001'
	#Check alphabetic order and number of Frame ID visualized
	And Counter of Frame ID 'B' should be 'done'
	#Check counter of strut adjustment of Frame ID
	And '<' option is 'shown'

Scenario: User complete all the Strut Adjustment and tap on back option
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'all Frame ID'
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Alert icon' should be 'not visualized'


Scenario: User complete all the Strut Adjustment
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'all Frame ID'
	Then 'Strut Adjust' page is visualized
	And The 'Frame IDs' should be 'Qr001'
	#Check alphabetic order and number of Frame ID visualized
	And Counter of Frame ID 'all' should be 'done'
	#Check counter of strut adjustment of Frame ID

# Objective b 

Scenario: User postpone the Frame ID A
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID 'A'
	And User tap on 'Do it later' option
	And User tap on 'Confirm Postpone' option
	Then 'Strut Adjust' page is visualized
	And The 'Frame IDs' should be 'Qr001'
	#Check alphabetic order and number of Frame ID visualized
	And Counter of Frame ID 'A' should be '2'
	#Check counter of strut adjustment of Frame ID

Scenario: User check strut adjustment operation order of frame A after postpone
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'postpone' strut adjustment of 'Frame ID A'
	And User tap on Frame ID 'A'
	Then The 'Adjust Operation' should be 'the same'

Scenario: User return to Home Normal after postponing frame A
	Given The user access with QrCode 'Qr001' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'postpone' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Alert icon' should be 'visualized'

Scenario: User not postpone the only one Frame ID
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'not postpone' strut adjustment of 'Frame ID A'
	Then 'FrameIdInfo' page is visualized
	And The 'FrameIdInfo' should be 'correct'

Scenario: User execute after trying to postpone adjustment operation and tap back option
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'not postpone' strut adjustment of 'Frame ID A'
	And User 'execute' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Alert icon' should be 'not visualized'

Scenario: User check strut adjustment after trying to postpone and then execute it
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'not postpone' strut adjustment of 'Frame ID A'
	And User 'execute' strut adjustment of 'Frame ID A'
	Then 'Strut Adjust' page is visualized
	And Text with id 'LbFrameId' is correct
	And Text with id 'LbStrutSite' is correct
	And Text with id 'LbStrutToBeDone' is correct
	And The 'Frame IDs' should be 'Qr002'
	#Check alphabetic order and number of Frame ID visualized
	And Counter of Frame ID 'A' should be 'done'
	#Check counter of strut adjustment of Frame ID

# Objective c 

Scenario: User complete a postponed Strut adjustment
	Given The user access with QrCode 'Qr003' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	Then 'Strut Adjust' page is visualized

Scenario: User check Frame ID status after executing a postponed Strut adjustment
	Given The user access with QrCode 'Qr003' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	Then 'Strut Adjust' page is visualized
	And Counter of Frame ID 'A' should be '1'
	#Check counter of strut adjustment of Frame ID

Scenario: User return to Home page after completing a postponed Strut adjustment
	Given The user access with QrCode 'Qr003' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Alert icon' should be 'visualized'

Scenario: User return to home page after complete all the postponed or not strut adjustment of Frame ID A
	Given The user access with QrCode 'Qr003' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User 'complete' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then 'Home Normal' page is visualized
	And The 'Alert icon' should be 'not visualized'

Scenario: User complete all the postponed or not strut adjustment of Frame ID A
	Given The user access with QrCode 'Qr003' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User 'complete' strut adjustment of 'Frame ID A'
	Then 'Strut Adjust' page is visualized
	And Counter of Frame ID 'A' should be 'done'
	#Check counter of strut adjustment of Frame ID

# Objective d 

Scenario: Alert not shown if there are not adjustment to be done
	Given The user access with QrCode 'Qr000' at time '12:00'
	And User navigate to 'Home Normal' page
	Then The 'Alert icon' should be 'not visualized'

Scenario: User navigate in strut adjustment page without strut adjustment to be done
	Given The user access with QrCode 'Qr000' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	Then 'Strut Adjust' page is visualized
	And Text with id 'LbFrameId' is correct
	And Text with id 'LbStrutSite' is correct
	And Text with id 'LbStrutToBeDone' is correct
	And Text with id 'LblHelpVideoTutorial' is correct
	And The 'Video' should be 'visualized'
	And The 'Frame IDs' should be 'Qr000'
	#Check alphabetic order and number of Frame ID visualized
	Then Counter of Frame ID 'all' should be 'done'
	#Check counter of strut adjustment of Frame ID
	And '<' option is 'shown'

Scenario: User try to see information about all the Frame ID without strut adjustment to be done
	Given The user access with QrCode 'Qr000' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User tap on Frame ID 'A'
	And User tap on Frame ID 'B'
	And User tap on Frame ID 'C'
	And User tap on Frame ID 'D'
	And User tap on Frame ID 'E'
	And User tap on Frame ID 'F'
	And User tap on Frame ID 'G'
	And User tap on Frame ID 'H'
	And User tap on Frame ID 'I'
	Then 'Strut Adjust' page is visualized