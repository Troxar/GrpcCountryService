# GrpcCountryService

This project is based on the book of Anthony Giretti "Beginning gRPC with ASP.NET Core 6: Build Applications using ASP.NET Core Razor Pages, Angular, and Best Practices in .NET 6"

## Manual testing

Start the solution:

```bash
docker compose up --build
```

Open the web application:

```text
http://localhost:5082
```

Use the sample file below to test JSON upload:

```text
samples/countries-upload.sample.json
```

The file contains a small set of countries in the format expected by the upload form.
After upload, `CountryWiki.Web` sends the data to `CountryService.Grpc` via gRPC, and the service persists it to PostgreSQL.

Useful checks:

```bash
grpcurl -plaintext localhost:5076 list
```

```text
http://localhost:5080/health/ready   # CountryService.Grpc
http://localhost:5082/health/ready   # CountryWiki.Web
```
