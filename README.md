# selenium-utils
**The purpose of this framework is to make UI testing much easier and cheaper.**
Framework is based on selenium wrappers and principles which canopy uses. 



# APIs

We created types of 3 APIs. 
 - AssertAPI
 - FluentAPI
 - LambdaAPI - *just concept - not implemented*

 ## Assert API
 Assert API uses concept of one assertion on one line. This approach has its own advantages when you need to quickly navigate between stack trace and source code. 
  
 ```
 AssertUI.InnerTextEquals(element, "expected value", "Custom error message.");
 ``` 

## Fluent API
Fluent API is perfect for really simple tests. You can stack more checking methods in a row. 

```
element.CheckIfInnerTextEquals("value", ...).CheckHasAttribute("data-custom");
```

## Lambda API 
Lambda API is a theoretical concept for now. It is a compromise between AssertApi and FluentApi. 

```
    browser.First("#button").Check().InnerText(s => s.Contains("text"));
    browser.First("#input").Check().Tag(s => s.Contains("input"));
```

## Configuration
All critical settings are configurable in seleniumconfig.json. This provides you the possibility of creation specific configuration for your CI servers.  

> *seleniumconfig.json file has to be copied to bin folder.*

```
{
  "factories": { // defines browser factories
    "chrome:fast": {
      "capabilities": [ "--window-size=1920,1080" ]
    }
    "chrome:coordinator": { // integration with docker
      "options": {
        "coordinatorUrl": "http://192.168.88.216:5001/"
      }
    }
  },
  "baseUrls": [ // all tested base urls for tests
    "http://localhost:6277/"
  ],
  "testRunOptions": {
    "runInParallel": false, // sets whether browsers are used in parallel mode or not
    "testAttemptsCount": 1 // number of attempts to pass the test (1 = do not re-try) 
  },
  "logging": {
    "loggers": {
      "testContext": {}, // uses TestContext in MSTest to log 
    }
  }
}
```
