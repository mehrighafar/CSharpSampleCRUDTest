Feature: Customer Manager
As a an operator I wish to be able to Create, Update, Delete customers and list all customers
	
	Background:

		Given I am a client

	Scenario: 1.Customer get successfully
	Getting the list of all stored customers
		Given The repository has customer data
		When I make a GET request to 'customer'
		Then The response for get status code is 200
		And The response for get json should be 'customers.json'

	Scenario: 2.Customer get created successfully
	Creating a customer successfully in database
		When I make a POST request with 'customer.json' to 'customer'
		Then The 'customer' is created successfully 
		And The response for creation status code is 201
		And The response for creation json should be 'customer.json'
	
	Scenario: 3.Customer get updated successfully
	Update a stored customer successfully
		Given The repository has customer data
		When I make a PUT request with 'customer_update.json' to 'customer'
		Then The response for update status code is 200
		And The response for update json should be 'customer_update.json'

	Scenario: 4.Customer get removed successfully
	Delete a customer successfully from database 
		Given The repository has customer data
		When I make a DELETE request with id '1' to 'customer'
		Then The response for delete status code is 204	
