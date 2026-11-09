# Limiting a site by client IP address

## Short answer

Yes. The reverse proxy now supports exact-address and CIDR allow lists independently for each configured site.

The existing `blocked_ips.json` and `blocked_ranges.json` checks are global deny lists: a listed address is denied from every domain handled by the proxy, while every address not listed is permitted. They cannot express "allow only these addresses for this one test site."

The global `blocked_ips.json` and `blocked_ranges.json` behavior remains in place after the per-site allow-list check.

## Recommended design

Add an optional allow list to each `DomainConfig`, rather than extending the global block list. A configuration could look like this:

```json
{
  "Domains": ["test.example.com"],
  "TargetMachineName": "test-server",
  "TargetIp": "192.0.2.10",
  "TargetPort": 80,
  "AllowedClientIps": [
    "203.0.113.10",
    "203.0.113.24"
  ],
  "AllowedClientRanges": [
    "198.51.100.64/28"
  ]
}
```

For normal unrestricted sites, omit both properties or use empty lists:

```json
"AllowedClientIps": [],
"AllowedClientRanges": []
```

The implemented semantics are:

- both properties absent or empty: no per-site restriction, so the site remains public.
- at least one entry in either property: allow only an exact IP or CIDR match and reject every other client.
- invalid IP addresses or CIDR values: reject the configuration at startup rather than silently ignoring them.

## Where the check should occur

`Proxy.Execute()` obtains the direct peer address from `context.Request.RemoteEndPoint.Address` and resolves the requested host to `matchedConfig`. The implemented site-specific allow-list check runs immediately after confirming that `matchedConfig` is not null and before the existing global `IsIPBlocked` check and `ProcessRequest` call.

The intended decision order is:

1. Reject an unknown host.
2. If that host's configuration enables IP restriction, reject an address not in its allow list.
3. Apply the existing global IP/range deny lists.
4. Proxy the request.

A rejection should return `403 Forbidden`, close the response, and log the domain and rejected client address. Do not reveal the contents of the allow list in the response.

Configured addresses are parsed and validated once at startup. Exact addresses are compared as `System.Net.IPAddress` values rather than raw strings so equivalent address representations are handled correctly. The per-site CIDR matcher supports both IPv4 and IPv6.

## Important client-IP caveat

`RemoteEndPoint.Address` is trustworthy as the address of the machine directly connected to this proxy. If users connect directly to the reverse-proxy process, it is the appropriate value to check.

If another reverse proxy, load balancer, CDN, firewall proxy, or IIS instance sits in front of this process, `RemoteEndPoint` may always be that intermediary's address. In that topology, the real client address may be supplied in `Forwarded` or `X-Forwarded-For`, but those headers are spoofable when accepted directly from the internet.

Only use a forwarded client-IP header when all of the following are true:

- direct public access to this reverse proxy is blocked;
- requests can arrive only from explicitly trusted intermediary addresses;
- the trusted intermediary removes untrusted incoming forwarding headers and creates its own; and
- the proxy extracts the correct address using the known proxy-chain length, rather than blindly trusting the first arbitrary value.

Otherwise, continue using `RemoteEndPoint.Address`.

## Operational limitations of IP allow lists

An IP address identifies a connection location, not a particular user. Consider these effects:

- several users behind the same office or home NAT share one public address and will all be allowed;
- residential and mobile addresses can change without notice;
- VPN use changes the observed address;
- IPv6 privacy addresses can rotate, and the current `IPRange` implementation is IPv4-specific;
- permitting an entire CIDR range grants access to every device using that range.

Confirm each user's public egress address as observed in the proxy log, not the private address shown on their workstation. Keep an administrative recovery path so an address change does not lock out every maintainer.

The existing global `IPRange` implementation remains IPv4-specific. The new per-site matcher is separate and supports both IPv4 and IPv6 CIDR ranges.

## Better protection for selected users

Because the goal is to grant access to a select list of **users**, application-level authorization is the stronger primary control. Give approved accounts a test-site role or group and require that role throughout the test application. This provides individual identity, revocation, and auditing even when users share or change IP addresses.

For a non-public test site, a VPN, identity-aware access gateway, or IIS authentication plus authorization rules can also be preferable. An IP allow list is useful as an additional outer layer, but it should not replace HTTPS, secure authentication, and user authorization.

## Alternatives without changing this proxy

- If the test host has a dedicated public IP or listening port, an edge firewall can allow selected source IPs to that endpoint. This is efficient and keeps rejected traffic out of the application process.
- IIS IP Address and Domain Restrictions can protect the backend only if IIS can see the true client IP. In the present topology IIS normally sees the reverse proxy as its client, so applying the rule there would likely allow or deny the proxy itself rather than individual end users.
- A VPN can expose the test host only to authenticated VPN members and avoids dependence on users' changing public addresses.

Whichever layer enforces the rule must distinguish the test hostname or endpoint from the unrestricted production sites.

## Suggested validation before deployment

Test at least these cases:

1. An allowed exact IPv4 address can open the test site and complete login.
2. An address inside an allowed CIDR can open it.
3. An unlisted address receives `403` for the test host.
4. That same unlisted address can still open an unrestricted configured host.
5. A globally blocked address remains blocked even if it appears in the site's allow list.
6. Missing or empty allow lists leave that site unrestricted.
7. Invalid IP/CIDR configuration prevents startup with a clear error.
8. Host matching includes the expected port form, if clients send a non-default port in the `Host` header.
