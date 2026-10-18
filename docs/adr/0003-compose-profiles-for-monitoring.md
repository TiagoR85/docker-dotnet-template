# ADR-0003: Optional monitoring via Compose profiles

**Date:** 2026-10-02
**Status:** Accepted

## Context

Every developer should be able to run the template with one command, but the
demo environment also needs Prometheus scraping the app's `/metrics`.

## Decision

Keep the Prometheus service in `docker-compose.yml` behind
`profiles: ["monitoring"]`.

- `docker compose up -d` → only the API.
- `docker compose --profile monitoring up -d` → API + Prometheus (v3.13.3 LTS).

## Consequences

- Default startup stays lightweight (one container).
- CI and quickstart stay deterministic; the profile is opt-in and documented
  in the README.
