using Subscriber.State.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3609";
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60009";
builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

builder.Services.AddSingleton<SubscriberService>(_ =>
    new SubscriberService(DaprClient.CreateInvokeHttpClient(
        "subscriber", $"http://localhost:{daprHttpPort}")));

builder.Services.AddControllers().AddDapr();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCloudEvents();

// configure routing
app.MapControllers();
app.UseRouting();
app.MapSubscribeHandler();
if(Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") != null)
{
    app.Run("http://localhost:6009");
}
else
{
    app.Run();
}


