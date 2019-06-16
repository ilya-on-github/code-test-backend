The problems I see here:
1. The signature `int SubmitApplicationFor(ISellerApplication application)` is not transparent. It says nothing about the return value. It should at least be described in XML comments, but it would be better if a number had an object representation such as `struct ApplicationId`
2. `return (result.Success) ? result.ApplicationId ?? -1 : -1;` It's a bad practice for .NET. Here you should use exceptions and not return codes.
3. LSP violations such as `if (application.Product is SelectiveInvoiceDiscount sid)`.