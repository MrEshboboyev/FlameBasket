# 🛒 Shopping Basket API  

Welcome to the **Shopping Basket API**, a robust backend service built with **C#** to manage shopping baskets. This project is crafted with modern software design principles like **Domain-Driven Design (DDD)**, **Clean Architecture**, and **CQRS patterns**.  

It supports advanced features such as:  
- Basket management  
- Discount application  
- Event-driven communication using Kafka  
- Comprehensive testing with **xUnit**  

## 🚀 Features  
### Core Functionality  
- **Basket Management**: Create, update, and manage customer baskets with ease.  
- **Discount Application**: Apply and manage discounts dynamically.  
- **Event-Driven Design**: Seamless integration with **Kafka** for event-driven microservices communication.  

### Software Design Principles  
- **Domain-Driven Design (DDD)**: Ensures the codebase aligns with business needs.  
- **Clean Architecture**: Facilitates maintainability and scalability.  
- **CQRS Pattern**: Separates command and query responsibilities for optimized performance.  

### Additional Features  
- **Extensive Testing**: Includes detailed unit and integration tests written with **xUnit**.  
- **Extensibility**: Built to be easily extendable for new features or services.  

## 📂 Repository Structure  
Here’s an overview of the project structure:  

```
📦 src
 📦 common
  ┣ 📂 Flame.Common.Core    # Common Core logic (events, exceptions)
  ┣ 📂 Flame.Common.Domain  # Common Domain logic (entities (aggregate root, entity), domain events, domain errors, exceptions, extensions, primitives, value objects)  
 ┣ 📂 Flame.BasketContext.Application    # Core application logic (CQRS commands, queries)  
 ┣ 📂 Flame.BasketContext.Domain         # Business logic and domain entities  
 ┣ 📂 Flame.BasketContext.Infrastructure # Kafka integration, database access, etc.  
 ┣ 📂 Flame.BasketContext.Api            # ASP.NET Core API layer  
📦 tests  
 ┣ 📂 Flame.BasketContext.Tests.Data       # Data for tests  
 ┣ 📂 Flame.BasketContext.Tests.Unit       # Unit tests with xUnit
```  

## 🛠 Getting Started  

### Prerequisites  
Before you start, ensure you have:  
- .NET Core SDK installed  
- Kafka set up and running (for event-driven features)  
- A modern C# IDE (e.g., Visual Studio or JetBrains Rider)  

### Step 1: Clone the Repository  
```bash  
git clone https://github.com/MrEshboboyev/FlameBasket.git  
cd FlameBasket
```  

### Step 2: Configure Kafka  
1. Install and start Kafka on your local machine or connect to a cloud Kafka service.  
2. Update the `appsettings.json` file to include your Kafka configuration.  

### Step 3: Run the API  
```bash  
dotnet run --project src/Flame.BasketContext.API  
```  

### Step 4: Run the Tests  
```bash  
dotnet test  
```  

## 🌐 API Endpoints  

| Method | Endpoint                | Description                           |  
|--------|-------------------------|---------------------------------------|  
| GET    | `api/basket/{id}`       | Retrieve a specific basket            |  
| POST   | `api/basket`            | Create a new basket                   |  
| POST   | `api/basket/{id}/items` | Add iten to basket                    | 

## 🧪 Testing  
The project uses **xUnit** for unit and integration testing. Each layer of the application is covered with detailed test cases to ensure robustness and reliability.  

### Example Test Case  
Here’s an example of a basket creation test:  
```csharp  
[Theory]
[InlineData(.18)]
[InlineData(.24)]
[InlineData(.45)]
public void Create_WhenValidArgumentProvided_ShouldCreateBasket(decimal taxPercentage)
{
    //Arrange
    var customer = TestFactories.CustomerFactory.Create();

    //Act
    var basket = Basket.Create(taxPercentage, customer);

    //Assert
    basket.Should().NotBeNull();
}
```  

## 🌟 Why This Project?  
1. **Modern Architecture**: Combines DDD, Clean Architecture, and CQRS for a production-ready backend.  
2. **Scalable and Extendable**: Easily integrates with microservices via Kafka.  
3. **Well-Tested**: Comprehensive tests ensure code quality and reliability.  
4. **Real-World Application**: Ideal for e-commerce or similar use cases.  

## 🏗 About the Author  
This project was developed by [MrEshboboyev](https://github.com/MrEshboboyev), who is passionate about building scalable, maintainable, and high-quality software solutions.  

## 📄 License  
This project is licensed under the MIT License. Feel free to use and adapt the code for your own projects.  

## 🔖 Tags  
C#, Shopping Basket, Domain-Driven Design, Clean Architecture, CQRS, Event-Driven Design, Kafka, xUnit, API Development, Backend Development, Microservices, Software Architecture  

---  

Feel free to reach out for any questions, suggestions, or contributions! 🚀 
