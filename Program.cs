using System.Text.Json.Serialization;
using MessageEstimator.Models;
using MessageEstimator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMessageEstimationService, MessageEstimationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var messages = app.MapGroup("/api/messages");

messages.MapPost("/analyze", (AnalyzeMessageRequest request, IMessageEstimationService estimationService) =>
{
    if (string.IsNullOrWhiteSpace(request.Message))
        return Results.BadRequest(new { error = "Message cannot be empty." });

    return Results.Ok(estimationService.Analyze(request.Message));
});

messages.MapPost("/analyze-batch", (AnalyzeBatchRequest request, IMessageEstimationService estimationService) =>
{
    if (request.Messages is null || request.Messages.Count == 0)
        return Results.BadRequest(new { error = "Messages cannot be empty." });

    return Results.Ok(estimationService.AnalyzeBatch(request.Messages));
});

app.Run();
