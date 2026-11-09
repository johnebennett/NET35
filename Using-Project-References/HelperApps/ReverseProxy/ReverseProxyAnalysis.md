# Reverse proxy production-readiness analysis

## Estimated effort

The largest deficiency is not simply performance tuning: the current proxy handles one complete request before accepting the next. Supporting concurrent users safely requires changing the request-processing architecture and then making logging, configuration, error handling, and shared state safe under concurrency.

### Hardening the existing application

Estimated total: approximately 45–75 person-days, or 9–15 person-weeks.

| Work area | Estimated effort |
|---|---:|
| Bounded asynchronous/concurrent processing | 4–7 days |
| Per-request exception isolation and graceful shutdown | 3–5 days |
| Correct request/response streaming, including chunked bodies | 5–8 days |
| Complete proxy and hop-by-hop header handling | 4–7 days |
| TLS and trusted client-IP architecture | 4–8 days |
| Timeouts, body limits, rate limiting and overload protection | 4–7 days |
| Windows Service hosting, health checks and recovery | 3–6 days |
| Structured logging, rotation, metrics and alerting | 3–6 days |
| Configuration validation and secure deployment | 2–4 days |
| Automated integration, concurrency and security tests | 8–14 days |
| Load testing, staging, documentation and production rollout | 5–10 days |

For one experienced developer/administrator working mostly full-time, allow approximately 10–16 calendar weeks. A tightly scoped internal-production version might be achievable in 6–9 weeks, but that would not be equivalent to a mature public-facing proxy.

### Codex versus manual effort

Codex could feasibly perform about 65–80% of the engineering work, including:

- Refactoring the listener for bounded concurrency.
- Implementing streaming, cancellation and timeouts.
- Correcting header and error-response handling.
- Adding service-hosting and health endpoints.
- Adding structured logging and configuration validation.
- Building automated integration and load-test projects.
- Preparing deployment scripts and documentation.
- Migrating the application to YARP, if selected.

Approximately 20–35% requires human or operational work:

- Choosing the public DNS and TLS termination architecture.
- Acquiring and managing certificates and secrets.
- Deciding which upstream proxies and networks are trusted.
- Establishing realistic request-size, timeout and rate limits.
- Configuring Windows services, firewalls, service accounts and permissions.
- Running tests from genuinely external and approved IP addresses.
- Load testing against representative infrastructure.
- Reviewing security, privacy, logging and retention requirements.
- Production approval, monitoring integration and rollback planning.

Codex can prepare commands and configuration for those activities, but cannot independently establish the trust boundaries or certify that the resulting deployment is safe.

## Migration alternative

Migrating to an established proxy is likely to take 1–6 weeks depending on the product, compared with 9–15 person-weeks to harden the existing proxy.

The established products already solve the core concurrency, HTTP parsing, streaming, connection management, TLS and graceful-shutdown problems. Microsoft specifically describes YARP as a toolkit for building high-performance proxies, with route configuration, health checks and rate limiting available through ASP.NET Core. [YARP repository](https://github.com/dotnet/yarp), [configuration](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/config-files), [rate limiting](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/rate-limiting), [health checks](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/yarp/dests-health-checks).

## Product comparison and ranking

| Rank | Product | Software cost | Estimated adoption effort | Assessment |
|---:|---|---:|---:|---|
| 1 | Caddy | Free, open source; paid sponsorship/support optional | 1–3 weeks | Best overall fit for a small multi-site deployment |
| 2 | Microsoft YARP | Free, MIT licensed | 3–6 weeks | Best if custom .NET behavior and configuration must be retained |
| 3 | IIS ARR | Free IIS extension; Windows Server licensing still applies | 1–3 weeks | Natural Windows/IIS fit, but an older product |
| 4 | Traefik Proxy | Free open-source edition; commercial Hub/support pricing by quote | 2–4 weeks | Production-capable but more oriented toward dynamic/cloud environments |
| 5 | HAProxy Community | Free, GPLv2; Enterprise pricing by quote | 2–5 weeks | Excellent production proxy, but a better operational fit on Linux |
| 6 | nginx Open Source | Free; NGINX Plus is commercial | 2–5 weeks on Linux | Excellent on Linux, but the official Windows build is considered beta |

### 1. Caddy — recommended off-the-shelf option

Caddy provides a production-ready concurrent HTTP/HTTPS reverse proxy, automatic HTTPS, health checks, load balancing, graceful configuration changes and logging. Its `client_ip` matcher accepts exact IP addresses and CIDR ranges and supports trusted proxy configuration. [Caddy reverse proxy](https://caddyserver.com/docs/command-line), [IP matchers](https://caddyserver.com/docs/caddyfile/matchers), [features and licensing](https://caddyserver.com/).

It can express public and restricted sites independently. It is likely the quickest way to replace the current program without maintaining a custom HTTP proxy implementation.

Software cost: $0. Paid commercial support is optional; pricing depends on the support arrangement.

### 2. Microsoft YARP — recommended custom-development option

YARP is the best choice if retaining JSON-driven domain mappings, custom global deny lists and custom per-site allow lists is important. It runs on ASP.NET Core/Kestrel and solves the simultaneous-request limitation without manually implementing HTTP proxy semantics.

Software cost: $0 under the MIT licence.

Expected effort: approximately 15–30 person-days, including migration, IP policies, Windows Service hosting, tests, TLS configuration and deployment. Codex could probably perform 75–85% of the implementation, with infrastructure and acceptance testing remaining manual.

### 3. IIS Application Request Routing

ARR integrates directly with the existing Windows/IIS environment. It provides rule-based routing, concurrent processing, TLS through IIS, load balancing, health monitoring and tracing. [Microsoft ARR overview](https://learn.iis.net/downloads/microsoft/application-request-routing).

Software cost: free download, although Windows Server and operational costs remain.

Its disadvantages are age and long-term maintenance uncertainty: ARR 3 dates from the previous IIS extension generation. It remains useful, but a fresh long-term investment may be better directed toward YARP or Caddy.

### 4. Traefik Proxy

Traefik provides concurrent reverse proxying, automatic service discovery, TLS, metrics and an `IPAllowList` middleware supporting exact addresses and CIDR ranges. [Traefik Proxy](https://traefik.io/traefik), [IP allow list](https://doc.traefik.io/traefik/v3.1/middlewares/http/ipallowlist/).

Software cost: the open-source proxy is free. Commercial Hub, enterprise features and support require vendor pricing.

It is strong for Docker, Kubernetes and dynamic infrastructure, but likely more complex than necessary for the present Windows/IIS environment.

### 5. HAProxy

HAProxy Community is a mature, high-performance concurrent proxy with extensive ACL, TLS, logging, rate-limiting and health-check capabilities. The community edition is GPLv2. HAProxy Enterprise adds commercial support and security features, with pricing provided by sales. [HAProxy Community licensing](https://docs.haproxy.org/2.4/intro.html), [HAProxy Enterprise](https://www.haproxy.com/products/haproxy-enterprise).

It is an excellent production option when Linux operations expertise is available. It is less natural for the current Windows-only environment.

### 6. nginx

nginx Open Source supports per-server IP allow and deny rules, IPv4/IPv6 CIDRs, TLS, rate limiting and high concurrency. [nginx access controls](https://nginx.org/en/docs/http/ngx_http_access_module.html).

Software cost: nginx Open Source is free. NGINX Plus requires a commercial subscription.

The official Windows build should not be selected for this production requirement: nginx documents it as beta, says high scalability should not be expected, and notes that only one worker performs work. [nginx for Windows limitations](https://nginx.org/en/docs/windows.html). It would rank much higher if a supported Linux host were available.

## Recommendation

Do not spend 9–15 person-weeks turning the current `HttpListener` implementation into a general production proxy unless its custom behavior is strategically important.

For this environment:

1. Select Caddy if the priority is the quickest, lowest-maintenance production proxy.
2. Select YARP if retaining a custom .NET application and the current configuration model is important.
3. Consider IIS ARR if deployment must remain completely within familiar IIS administration and its product age is acceptable.

A short proof of concept comparing Caddy and YARP should take approximately 3–5 days and would provide better evidence before committing to either migration.
