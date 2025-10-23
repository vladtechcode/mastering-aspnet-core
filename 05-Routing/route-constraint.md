# Route Constraints

## int
- Matches with any integer.
- Eg: `{id:int}` matches with 123456789, -123456789

## decimal
- Matches with any decimal value.
- Eg: `{price:decimal}` matches with 123.45, -123.45

## long
- Matches with any long integer.
- Eg: `{id:long}` matches with 1234567890123456789, -1234567890123456789

## bool
- Matches with true or false. Case-sensitive.
- Eg: `{isActive:bool}` matches with true, false, True, False

## datetime
- Matches a valid DateTime value with format `yyyy-MM-dd hh:mm:ss tt` and `MM/dd/yyyy hh:mm:ss tt`
- Eg: `{id:datetime}` matches with `2030-01-01%2011:59:59%20PM`, `01/01/2030%2011:59:59%20PM`
- Note: URL encode the space character as `%20`

## guid	
- Matches with a valid GUID value. GUID (Global Unique Identifier - hexadecimal number that is universsaly unique). 
- It is a 128-bit integer (16 bytes) that can be used across all computers and networks wherever a unique identifier is required.
- Eg: `{id:guid}` matches with `d9b2c1e4-5f6b-4c8e-9f1b-2c1e4f6b4c8e`
- Note: URL encode the hyphen character as `%2D`

## minlength(value)
- Matches with a string that has a minimum length of `value`.
- Eg: `{name:minlength(4)}` matches with `John`, `Alice`, but not `Bob`

## maxlength(value)
- Matches with a string that has a maximum length of `value`.
- Eg: `{name:maxlength(8)}` matches with `John`, `Alice`, but not `Bob-Smith`

## length(min, max)
- Matches with a string that has number of charcacters between given `min` and `max` length (both number included).
- Eg: `{name:length(4, 8)}` matches with `John`, `Alice`, but not `Bob` or `Christopher`

## length(value)
- Matches with a string that has exact `value` length.
- Eg: `{name:length(5)}` matches with `Alice`, but not `John` or `Christopher`

## min(value)
- Matches with a numeric value that is greater than or equal to `value`.
- Eg: `{age:min(18)}` matches with `18`, `25`, but not `17`

## max(value)
- Matches with a numeric value that is less than or equal to `value`.
- Eg: `{age:max(65)}` matches with `65`, `50`, but not `66`

## range(min, max)
- Matches with a numeric value that is between given `min` and `max` (both numbers included).
- Eg: `{age:range(18, 65)}` matches with `18`, `25`, `65`, but not `17` or `66`

## alpha
- Matches with alphabetic characters only (a-z, A-Z).
- Eg: `{name:alpha}` matches with `John`, `Alice`, but not `John123` or `Alice!`

## regex(expression)
Matches with a string that matches with the specified regular expression pattern.
- Eg: `{code:regex(^[A-Z]{3}\d{3}$)}` matches with `ABC123`, but not `abc123` or `AB1234`
- Eg: `{phone:regex(^\d{3}-\d{3}-\d{4}$)}` matches with `123-456-7890`, but not `1234567890` or `123-45-67890`
- [Regular Expressions](Regular-Expressions.md)