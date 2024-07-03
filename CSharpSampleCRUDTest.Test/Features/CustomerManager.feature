Feature: Customer Manager

As a an operator I wish to be able to Create, Update, Delete customers and list all customers
	
Background:
	Given I am a client

Scenario: 1.Customer get successfully
	Given The repository has customer data
	When I make a GET request to 'customer'
	Then The response for get status code is 200
	And The response for get json should be 'customers.json'
