# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "ClassName=Tests.SetTests"

# Run a single test method
dotnet test --filter "FullyQualifiedName~Tests.SetTests.SomeMethod"

# Pack NuGet package
dotnet pack BidirectionalDictionary/BidirectionalDictionary.csproj
```

## Architecture

This is a single-class NuGet library (`DotNetXtensions.BidirectionalDictionary`) with one main source file:

- **`BidirectionalDictionary/BidirectionalDictionary.cs`** — The entire library: `BidirectionalDictionary<TKey, TValue>` class plus `BidirectionalDictionaryX` extension class.
- **`BidirectionalDictionary.Tests/`** — xUnit v3 tests targeting net10.0.

### Core Design

`BidirectionalDictionary<TKey, TValue>` wraps two `Dictionary` instances (`_fmap` for key→value, `_rmap` for value→key) to provide O(1) bidirectional lookups. Both `TKey` and `TValue` are `notnull`.

**Key behavioral properties:**
- `Force` (default `true`): When setting a key's value to a value already owned by a different key — `true` silently evicts the conflicting mapping (like Guava's `forcePut`), `false` throws `ArgumentException` (like Guava's `put`).
- `AllowDefaults` (default `true`): When `false`, rejects keys/values equal to their type's default (`0`, `Guid.Empty`, etc.).

**Setter hierarchy:**
- `Add` — strict, throws if key OR value already exists
- `Set(key, value, force)` / indexer `[key] = value` — upsert with Force semantics
- `TrySet(key, value, out conflictKey)` — returns `false` (never throws, never modifies) when value is owned by a different key; caller gets the conflicting key

**Critical invariant in `Set`**: conflict checks happen before any mutation so a throw leaves the dictionary untouched.

### Test Conventions

`globals.cs` imports `DotNetXtensions`, `Xunit.Assert` (static), and `Tests.TestsX` (static). Test helpers in `TestsX` provide `IsSingle(map)` and `IsCount(n, map)` which also assert `CountsAreEqual` (both internal maps have matching counts).

Test files are organized by operation: `AddTests`, `SetTests`, `TrySetTests`, `RemoveTests`, `LookupTests`, `AllowDefaultsTests`, `CollectionTests`, `ScenarioTests`, `ReadmeExampleTests`.
