# Currency Exchange Trading Api
The purpose this service is to execute currency exchange trades by fetching rates from third party rate providers. mainly from https://fixer.io/ and  https://exchangeratesapi.io/.
the desired provider can be confgigured from the configuration file.

## Architecture Used 
the application is a simple rest api microservice application developed with Clean architecture basis.
the application contains the fllowing projects
1. CurrencyExchange.Domain // Abstract the core part of the bussiness Domain. and also hold some domain implmentation for making a domain dependent Trading.
2. CurrencyExchange.Applicaiton
3. CurrencyExchange.Infrastructure 
4. CurrencyExchange.Api

#### Scenarios
  * When an exchange is used it should never be older than 30 minutes:*
  used Redis caching TTL to invalidate the cache after 30 minute and this logic is also guarded on the domain layer as well.
 *Limiting each client to 10 currency exchange trades per hour
 Implmented a custom middleware which is responsible for RateLimiting the customer by caching the counts of the user visit using Lua script increament per key.
the middleware use header value to get client information.
 // To do: configure the rate limiter for specific end point if we add an additional end point.

##### Asumption 
 *The user Will use a single session at a time.
 for the user to use multiple device at the same time we have to use distributed lock for our rate limiter.
###### In Progress
 * Integration Test
 * some service Tests and Application level Unit tests.
