Feature: Login.DH.HelpDesk.Web

Login to DH HelpDesk.Web
Scenario: As an admin user I should be able to login to DH HelpDesk.Web using all supported browsers and user name and password
	Given I launch the DH.HelpDesk.Web application
		| Browser   | BrowserVersion   | OS   | 
		| <Browser> | <BrowserVersion> | <OS> | 
	When I enter the following details
		|Username | Password |
		| ds    | ds    |
	And I click login button
	Then I should be able to see the Administrator button

	Examples: 
	| Browser | BrowserVersion | OS    |
	| Microsoft Edge | .  | WIN11 |
	| Chrome | .  | WIN11 | 



Scenario: As a user I should be able to login to a case summary starting page
	Given I login as the an admin user
		| Browser | Username | Password | BrowserVersion | OS    |
		| Chrome  | ds       | ds       | .              | Win11 |
	And I give the following user the following starting page
		| Username | Starting Page |
		| ds       | Case Summary  |
	And I logout from the admin user 
		|Username |
		| ds |
	 When  I login as the user
		| Username | Password |
		| ds      | ds     |
	Then I should be able to see the Case Summary page
