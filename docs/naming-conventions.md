# Naming Conventions

This document references the naming conventions to be used across the solution. This is to ensure consistency, when building across several microservices.

## Services

By default, services should use the 'singular' version of their name. For example, the service accountable for handling Player Decisions would be known as PlayerDecision.

- PlayerDecision
- PlayerState
- WorldEventsEngine

## Cloud Resources

Some naming conventions may require a resource within a 'context' of a service. For example, some services may require state to be persisted within a database. Some services may require queues to decouple components.

The naming convention should use the convention ServiceName.ComponentName.ResourceType. ComponentName and ResourceType are both optional aspects. Dashes can be used as an alternative to full stops, and is dependent upon context (e.g. Azure Resources, vs. being used in code).

## Casing

- Variable names across languages will use camelCase.
- Class names across languages will use PascalCase.
- Dapr component names will use lowercase

# Example Library

## PlayerDecision Service

PlayerDecision
playerdecision-store
playerdecision-store-table

## PlayerState Service

PlayerState
playerstate-store
playerstate-store-

## WorldEventsEngine Service

WorldEventsEngine
worldeventsengine-store-table
worldeventsengine-store-queue
