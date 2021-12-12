dapr run `
    --app-id subscriber `
    --app-port 6008 `
    --dapr-http-port 3608 `
    --dapr-grpc-port 60008 `
    --config ../dapr/config/config.yaml `
    --components-path ../dapr/components `
    dotnet run