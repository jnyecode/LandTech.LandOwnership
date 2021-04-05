# LandTech Land Ownership exercise

Thank you for taking the time to review this submission. This submission is based on the 
instructions outlined in the [original repo](https://github.com/landtechnologies/technical-challenge/tree/master/land-ownership)
(saved to [INSTRUCTIONS.md](./INSTRUCTIONS.md) for reference)

## Running the application in Docker

1. Ensure you are at the project root directory: `cd LandTech.LandOwnership/`
1. Build docker image: `docker build -t jnye.landtech .`
1. Run the application: `docker run -it jnye.landtech`

> Once finished clean up the docker image with `docker rmi jnye.landtech`

![application demo](./app_demo.gif)

## Notes

I used [tests](./LandTech.LandOwnership.Tests/OwnershipProviderTests.cs) to drive this implementation
which I then refactored to a more optimised solution using the tests as my safety net.

Given more time, I would look to introduce `CompanyRelationshipProvider` and `LandOwnershipProvider`
abstractions that would allow different data sources to be injected into the `OwnershipProvider`

I would also look to make the application a bit more user friendly by having it allow you to enter
another `companyId` after returning the results of the first request.

---

🙏 Thank you again for your time reviewing this submission, I welcome any feedback you may have.
