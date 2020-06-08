# Simple Clean Architecture Based Microservices (.NET)
Simple "Clean Architecture" based microservices, written in .NET, created as a simple reference on how things can, to put it in simple terms, be done simply.

<i>Created for personal and community's knowledge reference.</i>

Consists of two microservices with slightly different development philosophies:
- AccountManager - "dirtier" Clean Architecture but much simpler and easier to understand. Take this design approach when the microservice should be simple and you don't desire to separate your Domain project from <i>all</i> third-party dependencies. 
- UserManager - "cleaner" Clean Architecture, but a bit harder to reason about. Take this design approact when all (most) third-party dependencies should be abstracted away, the result of which would be an increase in code complexity, albeit with a more "independent" Domain project.

Identity Service + Authentication + Authorization are on the way.

-----------------------------------------------------------------------------------------------------------------------------------------------
<i>"Simplicity" cannot be overstated.

Elegancy is what we're trying to achieve.

Elegancy through Simplicity.</i>
