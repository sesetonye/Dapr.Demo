dapr run `
    --app-id publisher `
    --dapr-http-port 3607 `
    --dapr-grpc-port 60007 `
    --config ../dapr/config/config.yaml `
    --components-path ../dapr/components `
    dotnet run