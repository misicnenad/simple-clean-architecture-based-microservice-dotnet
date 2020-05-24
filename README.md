# A Simple Clean Architecture Based Microservice (.NET)
A simple "Clean Architecture" based microservices, written in .NET, created as a simple reference on how things can, to put it in simple terms, be done simply.

<i>Created for personal and communities knowledge reference.</i>

Consists of two microservices with slightly different development philosophies:
- AccountManager - "dirtier" but simple Clean Architecture; even though it has third-party libraries (MediatR) in the Domain project it does it's job quite well 
- UserManager - "cleaner" Clean Architecture; all unnecessary dependencies are abstracted away, the result of which is more complex code, but a more "independent" Domain project.

Identity Service + Authentication + Authorization are on the way.

-----------------------------------------------------------------------------------------------------------------------------------------------
<i>"Simplicity" cannot be overstated.

Elegancy is what we're trying to achieve.

Elegancy through Simplicity.</i>
