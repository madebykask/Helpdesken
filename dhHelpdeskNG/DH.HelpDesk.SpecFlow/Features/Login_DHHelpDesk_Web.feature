Feature: Login.DH.HelpDesk.Web

Login to DH HelpDesk.Web
Scenario: As a user I should be able to login to DH HelpDesk.Web using all supported browsers and user name and password
	Given I launch the DH.HelpDesk.Web application
		| Browser   | BrowserVersion   | OS   | Build |
		| <Browser> | <BrowserVersion> | <OS> | <Build> |
	When I enter the following details
		|Username | Password |
		| ds    | ds    |
	And I click login button
	Then I should be able to see the Case Summary pages

	Examples: 
	| Browser | BrowserVersion | OS    | Build        |
	| Firefox | 108.0.1462.54  | WIN11 | Chrome build |
	| Microsoft Edge | 108.0.1462.54  | WIN11 | Chrome build |
	| Chrome | 108.0.1462.54  | WIN11 | Chrome build |