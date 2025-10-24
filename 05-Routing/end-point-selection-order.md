# Endpoint Selection Order

> URL template with more segments
> - e.g: "a/b/c/d" is higher than "a/b/c"

This is the most straightforward rule. The routing engine prefers the longest possible path that matches the URL. It assumes a longer template is inherently more specific.

Example:
Imagine you have these two endpoints:

```c#
// Endpoint A: More specific
app.MapGet("/products/featured/all", () => "All Featured Products");

// Endpoint B: Less specific
app.MapGet("/products/featured", () => "Featured Products landing page");
```

If a request comes in for https://example.com/products/featured/all, Endpoint A will be chosen because its template /products/featured/all has more segments and is a more exact match than /products/featured.
    
> URL template with literal text has more precedence than a parameter segment
> - e.g: "a/b" is higher than "a/{parameter}"

A hardcoded, literal segment (like /users/all) is considered more specific than a flexible placeholder (like /users/{id}). The system will always prioritize an exact text match over a route that requires interpreting a parameter.

Example:
Consider these two endpoints defined in this order:

```c#

// Endpoint A: Has a placeholder
app.MapGet("/users/{id}", (int id) => $"User Profile for ID: {id}");

// Endpoint B: Uses literal text
app.MapGet("/users/all", () => "All Users List");
```

A request for /users/123 can only match Endpoint A.

A request for /users/all could technically match both (all could be an id). However, Endpoint B wins because its /users/all template is a literal and therefore more specific match.

> URL template that has a parameter segment with constraints has more precedence than a parameter segment without constraints. 
> - e.g: "a/{b:int}" is higher than "a/{b}"

Adding a constraint (like :int, :guid, or a custom constraint) makes a route more specific. You're telling the router, "I don't just want any value here, I want one that is specifically an integer." This added restriction gives it a higher priority.

Example:
Imagine you want to look up articles by numeric ID or by a string-based "slug".

```c#
// Endpoint A: Constrained to an integer
app.MapGet("/articles/{id:int}", (int id) => $"Article by ID: {id}");

// Endpoint B: Unconstrained (accepts any string)
app.MapGet("/articles/{slug}", (string slug) => $"Article by Slug: {slug}");
```

A request for /articles/123 matches both templates. However, Endpoint A wins because 123 satisfies the :int constraint, making it the more specific choice.

A request for /articles/my-first-post fails the :int constraint of Endpoint A, so Endpoint B is the only possible match.

> Catch-all parameters (**)
> - e.g: "a/{b}" is higher than "a/**".

A standard parameter ({b}) is designed to capture a single URL segment. A catch-all parameter ({*b} or **b) is greedy; it captures everything from that point to the end of the URL. Because the standard parameter is more limited in what it captures, it's considered more specific.

Example:
Let's say you're routing file paths.

```c#
// Endpoint A: Standard parameter (captures one segment)
app.MapGet("/files/{filename}", (string filename) => $"Serving single file: {filename}");

// Endpoint B: Catch-all parameter (captures the rest of the URL)
app.MapGet("/files/{*filepath}", (string filepath) => $"Serving file at path: {filepath}");
```


A request for /files/report.pdf matches both. Endpoint A wins because it's looking for a single segment, which is a more specific pattern.

A request for /files/documents/2025/report.pdf can only be handled by Endpoint B, as the catch-all is needed to capture the multiple segments (documents/2025/report.pdf).