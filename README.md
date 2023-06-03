# Carpool Management System

The Carpool Management System is a web-based application built using C# and .NET 6. It allows users to manage car and driver information, assign drivers to cars, record transactions, and request car deliveries. The application is deployed on Azure Web Service and uses various Azure services such as Service Bus, SQL Database, Cosmos DB, and Blob Storage.

## Features

- Add and manage cars: Users can add new cars to the system, providing details such as make, model, and license plate.
- Add and manage drivers: Users can add new drivers to the system, including information like name, contact details, and license number.
- Assign drivers to cars: Users can assign a driver to a specific car, ensuring that each car has an assigned driver.
- Record transactions: When a driver is assigned to a car, a message is sent via Service Bus to the Transaction Service, which records the transaction in Cosmos DB. This helps track the history of car and driver assignments.
- Request car deliveries: Upon driver assignment, a message is sent to the Garage Service via Service Bus, requesting the delivery of the car to the assigned driver. This helps facilitate the efficient management of car logistics.
- Manage car photos: The application includes a Photo Service that allows users to upload and download car photos to and from Azure Blob Storage. This feature helps maintain visual records of the cars.

## Technologies Used

- C# .NET 6: The application is built using the latest version of .NET framework, providing a powerful and efficient development environment.
- Azure Web Service: The application is deployed on Azure Web Service, ensuring scalability, availability, and easy management.
- Azure Service Bus: It is used to facilitate communication between the different components of the system, enabling reliable messaging.
- SQL Database: The Management Service utilizes SQL Database to store and retrieve car and driver information.
- Cosmos DB: The Transaction Service and Garage Service utilize Cosmos DB to record transactions and manage car logistics.
- Azure Blob Storage: The Photo Service utilizes Azure Blob Storage to store and retrieve car photos.

## Deployment

To deploy the Carpool Management System, follow these steps:

1. Clone the repository: `git clone https://github.com/amitaysh/AzureCarPoolWebService`
2. Configure the Azure services:
   - Create an Azure Service Bus namespace and obtain the connection string.
   - Create a SQL Database and configure the connection string in the Management Service.
   - Create Cosmos DB accounts for the Transaction Service and Garage Service, and configure the connection strings accordingly.
   - Set up Azure Blob Storage and configure the connection string in the Photo Service.
3. Build and publish the application using the .NET CLI or Visual Studio.
4. Deploy the published application to Azure Web Service.
5. Ensure that the necessary environment variables, such as connection strings and other configurations, are set correctly in the deployed environment.

## Contributing

Contributions to the Carpool Management System are welcome! If you have any suggestions, bug reports, or would like to contribute new features, please open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
