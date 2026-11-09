# AllowedPaths findings

## Summary

`DomainConfig.AllowedPaths` is not used to authorize or reject requests anywhere in this project. It is only declared as a configuration property and populated (when present in JSON) by Newtonsoft.Json deserialization.

Consequently, the proxy already permits **any URL path** for a configured domain, subject to its separate host/domain and source-IP checks. There is no special wildcard or bypass value to put in `AllowedPaths`; all values, including `[]`, `["*"]`, `["/"]`, or omitting the property, have the same effect because the property is never read.

## Where it appears

- `DomainConfig.cs` declares `public List<string> AllowedPaths { get; set; }`.
- `DomainConfig.LoadConfig(...)` reads `DomainConfig.json` and deserializes it into `List<DomainConfig>`. This is the only mechanism that can populate `AllowedPaths`.
- A repository-wide search found no access to `config.AllowedPaths`, no iteration over it, and no path authorization logic.
- The included `ReverseProxy.zip` contains an older `DomainConfig` that does not define `AllowedPaths` at all, and its proxy code also has no allowed-path check.

## Actual request flow

`Proxy.Execute()` performs these gates before proxying a request:

1. The request `Host` header must exactly match an entry in a configuration's `Domains` list, using a case-insensitive comparison.
2. The requester's IP address must not match `blocked_ips.json` or `blocked_ranges.json`.
3. `ProcessRequest(...)` selects the domain configuration again, constructs the target URL using `request.Url.PathAndQuery`, and forwards the request.

Because `request.Url.PathAndQuery` is appended unchanged to the target server URL, MVC routes, static-resource paths, query strings, and other paths are all passed through without consulting `AllowedPaths`.

## Configuration recommendation for the MVC 5 site

Configure the site's host name(s), target IP, and target port normally. `AllowedPaths` may be omitted from that domain entry or set to an empty list for clarity; neither form restricts paths in the current executable:

```json
{
  "Domains": ["example.test", "www.example.test"],
  "TargetMachineName": "mvc-server",
  "TargetIp": "192.0.2.10",
  "TargetPort": 80,
  "AllowedPaths": []
}
```

The exact `Domains` strings must match the incoming `Host` header. The current comparison includes any port present in that header, so a non-default externally visible port may require a corresponding `host:port` entry.

## Conclusion and changes

No application change is required to bypass an allowed-path restriction: the restriction is not implemented in this project. Only this findings document was added. No existing files were modified, so no backup copies were necessary.

If a deployed instance is rejecting particular paths, the cause is outside this source's `AllowedPaths` property—for example IIS/MVC authorization or routing, request filtering, a different deployed build, the target application, host matching, or the IP block lists.
