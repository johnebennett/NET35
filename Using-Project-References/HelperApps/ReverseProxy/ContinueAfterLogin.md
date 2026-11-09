# Continuing after login through the reverse proxy

## Most likely cause

The current proxy follows redirects itself. `Proxy.cs` creates an `HttpWebRequest` but does not set `AllowAutoRedirect`, whose default is `true`.

A typical ASP.NET MVC 5 Forms Authentication login works like this:

1. The browser posts credentials to the application.
2. The application replies with a `302` redirect to `/Portfolio/Index` and a `Set-Cookie` header containing the authentication cookie (often `.ASPXAUTH`).
3. The browser stores that cookie and follows the redirect, sending the cookie on the request for `/Portfolio/Index`.

Through this proxy, step 3 is instead performed by `HttpWebRequest`. The request has no `CookieContainer`, so the authentication cookie is not carried into the internally followed redirect. The application sees an anonymous request for `/Portfolio/Index` and redirects to Login again. The browser ultimately receives another fresh Login page, which matches the observed empty textboxes/new-session symptom.

## Recommended workaround

Make redirects browser-visible by setting this immediately after creating `proxyRequest` in `ProcessRequest(...)`:

```csharp
HttpWebRequest proxyRequest = (HttpWebRequest)WebRequest.Create(targetUrl);
proxyRequest.AllowAutoRedirect = false;
proxyRequest.Method = request.HttpMethod;
```

This is preferable to keeping a server-side `CookieContainer`: authentication cookies belong in the user's browser, and the browser needs to receive the original `Set-Cookie` and `Location` headers.

The proxy must then return all of the upstream response status and headers to the browser, especially:

- the original `302` status;
- every `Set-Cookie` header without combining them; and
- the `Location` header.

The existing response-header loop may not safely preserve multiple `Set-Cookie` values. Cookie headers must not be comma-combined because cookie `Expires` attributes themselves contain commas. Handle `Set-Cookie` separately with `GetValues` and `AppendHeader`, for example:

```csharp
string[] setCookieHeaders = proxyResponse.Headers.GetValues("Set-Cookie");
if (setCookieHeaders != null)
{
    foreach (string setCookieHeader in setCookieHeaders)
    {
        response.AppendHeader("Set-Cookie", setCookieHeader);
    }
}
```

The general response-header loop should skip `Set-Cookie` after adding this special handling so cookies are not emitted twice. It should continue forwarding `Location`.

## Host name and IIS considerations

The outgoing request currently connects to `TargetIp`, and the original `Host` request header is skipped because `Host` is a restricted `HttpWebRequest` header. Therefore IIS receives the target IP/port as the host rather than the public host name.

Set the `Host` property explicitly if IIS site bindings, generated URLs, or authentication-cookie configuration depend on the public host:

```csharp
proxyRequest.Host = request.Headers["Host"];
```

Only do this for a host already matched against `DomainConfig.Domains`, as this project does. Ensure the corresponding host binding exists on the target IIS site.

If the MVC application produces an absolute `Location` containing the internal IP or machine name, configure the application to use the external origin or rewrite that `Location` to the public origin before returning it. Relative locations such as `/Portfolio/Index` need no rewrite.

It is also conventional to supply trusted forwarding information to the application:

```csharp
proxyRequest.Headers["X-Forwarded-Host"] = request.Headers["Host"];
proxyRequest.Headers["X-Forwarded-Proto"] = request.IsSecureConnection ? "https" : "http";
```

Only trust these headers at the target application when requests are known to come from this proxy. If HTTPS is terminated before this process, `request.IsSecureConnection` may be false and the proxy needs a trusted way to preserve the original scheme.

## Cookie settings to check

After fixing redirect handling, inspect the login response in browser developer tools and verify that the authentication cookie is stored. Check these attributes if it is rejected or not returned:

- `Domain`: it must be absent (host-only) or match the public host; it must not name the internal machine/IP.
- `Path`: normally `/`; a narrower path would prevent sending the cookie to `/Portfolio/Index`.
- `Secure`: the browser sends such a cookie only over HTTPS.
- `SameSite`: this is normally fine for a same-site login, but an external identity provider or cross-site POST may require `SameSite=None; Secure` and suitable MVC/.NET updates.

If ASP.NET Session State is also lost, verify that the session cookie (usually `ASP.NET_SessionId`) is passed in both directions using the same checks. With multiple backend servers, use shared/out-of-process session state and a consistent `<machineKey>` (or sticky sessions); this proxy currently selects only one target per domain, so that is less likely here.

## How to verify the diagnosis

In the browser's Network tab, preserve the log and submit the login form:

1. The login POST should return `302`, not a final `200` Login page.
2. Its response should contain the authentication `Set-Cookie` header and `Location: /Portfolio/Index` (or an appropriate external URL).
3. The next browser request to `/Portfolio/Index` should contain that cookie.
4. It should return the authenticated page rather than redirecting back to Login.

The proxy logs can also record the upstream status code and the presence/name (but never the value) of `Set-Cookie` during diagnosis. Current logging records all incoming header values, including `Cookie` and potentially authorization data; those values should be redacted in production logs.

## Changes implemented

`Proxy.cs` now leaves redirects for the browser to follow, preserves the validated public host, supplies trusted `X-Forwarded-Host` and `X-Forwarded-Proto` values, forwards each `Set-Cookie` header separately, and redacts cookie and authorization values from request-header logs.

The pre-change files are retained as `Proxy.cs.bak` and `ContinueAfterLogin.md.bak`. The modified project was successfully rebuilt in the Debug configuration with MSBuild.
