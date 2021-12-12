dapr run `
    --app-id subscriber-state `
    --app-port 6009 `
    --dapr-http-port 3609 `
    --dapr-grpc-port 60009 `
    --config ../dapr/config/config.yaml `
    --components-path ../dapr/components `
    dotnet run