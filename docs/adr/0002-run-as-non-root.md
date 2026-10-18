# ADR-0002: Run the container as the built-in non-root user

**Date:** 2026-10-02
**Status:** Accepted

## Context

Containers that run as root let a potential container escape inherit full
rights inside the container. Most image scanners and benchmarks (Docker Bench,
CIS) flag `USER root` as a finding.

## Decision

Switch to the built-in `app` user of the official .NET images with
`USER $APP_UID` (uid 1654) after all root-level setup (package install,
`COPY`) is finished.

## Consequences

- Kestrel listens on 8080 (> 1024), so no capability is needed to bind.
- The app only needs read access to `/app` — no writes are performed
  (logs go to stdout).
- Root inside the container is unavailable for debugging; use
  `docker exec -u root` deliberately when needed.
