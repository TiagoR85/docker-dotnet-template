# ADR-0001: Multi-stage build with curl in runtime image

**Date:** 2026-10-02
**Status:** Accepted

## Context

The SDK image needed to compile the app is ~1 GB; the final image should be as
small and as attack-surface-light as possible. The Dockerfile also needs a
`HEALTHCHECK`, which requires making an HTTP call from inside the container.

## Decision

- Stage 1 (`sdk:10.0`): restore + publish.
- Stage 2 (`aspnet:10.0`): copy only `/app/publish`.
- Install `curl` in the runtime stage, then remove apt lists, so the
  `HEALTHCHECK` can probe `/health/live`.

## Alternatives considered

- **distroless runtime:** smaller and no shell at all, but no way to run an
  in-container HEALTHCHECK. Chosen for the future when an orchestrator
  provides probes instead.
- **`wget`-only healthcheck:** not present in the base image either.

## Consequences

- Runtime image stays ~250 MB + ~5 MB of curl.
- `curl` increases the binary surface slightly; acceptable because the
  container runs as a non-root user (ADR-0002).
