Feature: FirstAccessByPrescriptionActivation_item_3779
	In order to activate myHEXplan profile
	As an Anonymous User 
	I want to scan my own QR code
     
# Objective a

Scenario: User tap back option in Start your treatment page
	Given User is 'anonymous'
	And User navigate to 'Start your treatment' page
	When User tap on '<' option
	Then 'Home Anonymous' page is visualized

Scenario: User tap Scan your access code option in Start your treatment page
	Given User is 'anonymous'
	And User navigate to 'Start your treatment' page
	When User tap on 'Scan your access code' option
	Then 'Your digital prescription' page is visualized
	And The 'QR Scanner' should be 'activated'

Scenario: User tap back option in Your digital prescription page
	Given User is 'anonymous'
	And User navigate to 'Your digital prescription' page
	When User tap on '<' option
	Then 'Start your treatment' page is visualized

Scenario: Successful QR code scan 
	Given User is 'anonymous'
	And User navigate to 'Start your treatment' page
	When User scan a 'valid' QR code
	Then A 'success' message is shown 
	And The 'Prescription downloaded' should be '3'

Scenario: User tap OK option after Success QR scan
	Given User is 'anonymous'
	And User navigate to 'Start your treatment' page
	When User scan a 'valid' QR code
	And User tap on 'OK' option
	Then 'Welcome User Normal' page is visualized

# Objective b

Scenario Outline: User scan a not working QR code 
	Given User is 'anonymous'
	And User navigate to 'Start your treatment' page
	When User scan a <QR type> QR code
	Then A <Error type> message is shown
Examples: 
| QR type      | Error type    |
| 'random'     | 'error QRE01' |
| 'invalid'    | 'error QRE02' |
# random = not generated from tlhex

Scenario Outline: User tap close option after a not working QR code
	Given User is 'anonymous'
	And User navigate to 'Start your treatment' page
	When User scan a <QR type> QR code
	And User tap on 'Close' option
	Then 'Start your treatment' page is visualized
Examples: 
| QR type      |
| 'random'     |
| 'invalid'    |
# random = not generated from tlhex