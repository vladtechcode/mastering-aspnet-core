# Status code Results

> Status code result sends an empty response with specified status code.

## StatusCodeResult 
> Represents response with the specified status code.
> Used when you would like to send a specific HTTP status code as response.
> `return new StatusCodeResult(int statusCode);`
> `return StatusCode(int statusCode);`


## UnauthorizedResult
> Represents response with HTTP status code 401 (Unauthorized).
> Used when authentication is required and has failed or has not yet been provided.
> `return new UnauthorizedResult();`
> `return Unauthorized();`

## BadRequestResult
> Represents response with HTTP status code 400 (Bad Request).
> Used when the server cannot or will not process the request due to something that is perceived to be a client error.
> `return new BadRequestResult();`
> `return BadRequest();`

## NotFoundResult
> `return new NotFoundResult();`
> `return NotFound();`

## Overview

In ASP.NET Core MVC, status code results are handled by a set of classes and convenient helper methods provided by the `ControllerBase` class.

There are two main categories of status code results:
1.  **Results with no body:** These just return an HTTP status code. They are all based on the `StatusCodeResult` class.
2.  **Results with a body:** These return both an HTTP status code and a data payload (usually formatted as JSON). They are based on the `ObjectResult` class.

---

## 1. Classes for Status Code Results

Your controller actions return an object that implements `IActionResult`. The framework then executes this result object to write the response.

### `StatusCodeResult` (No Body)

This is the base class for all action results that simply set an HTTP status code without writing a response body.

* **`StatusCodeResult`**: The generic class. You use it when there isn't a more specific class for the code you want.
    * **Example:** `return new StatusCodeResult(500);` (for 500 Internal Server Error)

More specific, commonly used classes inherit from `StatusCodeResult`:

* **`OkResult`**: Represents an **HTTP 200 OK** response.
* **`NoContentResult`**: Represents an **HTTP 204 No Content** response.
* **`BadRequestResult`**: Represents an **HTTP 400 Bad Request** response.
* **`UnauthorizedResult`**: Represents an **HTTP 401 Unauthorized** response.
* **`ForbidResult`**: Represents an **HTTP 403 Forbidden** response.
* **`NotFoundResult`**: Represents an **HTTP 404 Not Found** response.

### `ObjectResult` (With a Body)

This is the base class for all action results that return both a status code *and* an object, which is then serialized into the response body (e.g., as JSON).

* **`ObjectResult`**: The generic base class. You can use it to return any status code along with data.
    * **Example:** `return new ObjectResult(myErrorObject) { StatusCode = 500 };`

More specific, commonly used classes inherit from `ObjectResult`:

* **`OkObjectResult`**: Represents an **HTTP 200 OK** with a data payload.
* **`CreatedAtActionResult`**: Represents an **HTTP 201 Created** response. It includes the newly created object in the body and a `Location` header pointing to where the new resource can be found.
* **`BadRequestObjectResult`**: Represents an **HTTP 400 Bad Request** with details about the error (like a `ModelStateDictionary`).
* **`NotFoundObjectResult`**: Represents an **HTTP 404 Not Found** and can optionally include an object with details about what wasn't found.

---

## 2. Controller Helper Methods

You will almost never use the classes above directly (e.g., `new OkResult()`). Instead, you will use the helper methods provided by the `ControllerBase` class, which your controllers inherit from. These methods are shortcuts that create and return the correct result object for you.

Here are the most common methods and the objects they create.

### Methods for Results (No Body)

* `Ok()`: Returns an `OkResult` (200).
    * `return Ok();`
* `NoContent()`: Returns a `NoContentResult` (204).
    * `return NoContent();`
* `BadRequest()`: Returns a `BadRequestResult` (400).
    * `return BadRequest();`
* `Unauthorized()`: Returns an `UnauthorizedResult` (401).
    * `return Unauthorized();`
* `Forbid()`: Returns a `ForbidResult` (403).
    * `return Forbid();`
* `NotFound()`: Returns a `NotFoundResult` (404).
    * `return NotFound();`

### Methods for Results (With a Body)

Many methods are overloaded. If you pass an object to them, they automatically return the `ObjectResult` version.

* `Ok(object value)`: Returns an `OkObjectResult` (200) with the `value` in the body.
    * `var user = _db.Users.Find(1);`
    * `return Ok(user);`
* `CreatedAtAction(string actionName, object routeValues, object value)`: Returns a `CreatedAtActionResult` (201).
    * `var user = new User { ... };`
    * `_db.Users.Add(user);`
    * `return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);`
* `BadRequest(object error)`: Returns a `BadRequestObjectResult` (400) with the error details.
    * `if (!ModelState.IsValid) { return BadRequest(ModelState); }`
* `NotFound(object value)`: Returns a `NotFoundObjectResult` (404) with details.
    * `return NotFound(new { error = "User with id 1 not found." });`

### The General-Purpose Method

* `StatusCode(int statusCode)`: Returns a `StatusCodeResult` for any code.
    * `return StatusCode(500);`
* `StatusCode(int statusCode, object value)`: Returns an `ObjectResult` for any code, with a body.
    * `return StatusCode(500, new { error = "A server error occurred." });`
    * `return StatusCode(201, myNewObject);`