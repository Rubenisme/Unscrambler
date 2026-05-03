# Unscrambler

Solves Dutch railway station (NS) anagrams.
Give it a set of scrambled letters and it returns all valid station names that can be formed from those letters.

## Usage

### CLI

```sh
# Interactive mode
dotnet run --project Console

# Single solve
dotnet run --project Console -- "Laser meld ge"
```

### Web

```sh
dotnet run --project Unscrambler.Web
```

POST to `/solve` with a form body containing the `anagram` field, or use the web UI served at the root.

### Docker

```sh
docker compose -f deploy/docker-compose.yml up
```

The web app will be available on ports 8080 (HTTP) and 8081 (HTTPS).

## How it works

Each station name and the input are reduced to letter-frequency arrays.
A station matches if every letter in the input appears at least as many times as required by the station name.
Non-letter characters (spaces, dashes, apostrophes) are ignored during matching.

The database contains ~600 official NS station names.

## Building

Requires .NET 10 SDK.

```sh
dotnet build
dotnet test  # if tests are added
```

## License

MIT — see [LICENSE](LICENSE).
