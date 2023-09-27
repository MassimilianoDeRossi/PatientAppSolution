Feature: StrutAdjustmentReminder_item_3788
	As a normal User
	I want to check the correct behavior of alert icon
	on the Strut Adjust hexagon

# Objective a
Scenario: Alert icon not visualized if no adjustment to be done
	Given The user access with QrCode 'Qr002' at time '10:59'
	And User navigate to 'Home Normal' page
	Then The 'Alert icon' should be 'not visualized'

@ignore
#The pop-up it’s shown only after the schedule local notifications operation
#which is an unpredictable event
Scenario: Pop-up appears at the adjustment time
	Given The user access with QrCode 'Qr002' at time '10:59'
	And User navigate to 'Home Normal' page
	When User wait '2' minutes
	Then 'Adjustment push notification' appears

@ignore
#To perform this test is necessary the correct execution of the previous one
Scenario: Alert icon appears at the adjustment time
	Given The user access with QrCode 'Qr002' at time '10:59'
	And User navigate to 'Home Normal' page
	When User wait '2' minutes
	And User tap on 'OK' option
	Then The 'Alert icon' should be 'visualized'

# Objective b

Scenario: User see alert icon if device time is over adjustment time
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Home Normal' page
	Then The 'Alert icon' should be 'visualized'

Scenario: Alert icon disappear when user complete all the adjustment
	Given The user access with QrCode 'Qr002' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then The 'Alert icon' should be 'not visualized'

# Objective c

Scenario: Alert icon not disappear if user not complete all the adjustment
	Given The user access with QrCode 'Qr003' at time '12:00'
	And User navigate to 'Strut Adjustment' page
	When User 'complete' strut adjustment of 'Frame ID A'
	And User tap on '<' option
	Then The 'Alert icon' should be 'visualized'