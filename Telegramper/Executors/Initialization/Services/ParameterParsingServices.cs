using Microsoft.Extensions.DependencyInjection;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.ParameterParser;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler.Strategies;
using Telegramper.Executors.QueryHandlers.ParameterParser.Strategies;

namespace Telegramper.Executors.Initialization.Services;

public static class ParameterParsingServices
{
    internal static IServiceCollection AddParameterParsing(this IServiceCollection services, ParametersParserOptions options)
    {
        services.AddTransient<IParseErrorHandler, ParseErrorHandler>();
        services.AddScoped(typeof(IParseErrorStrategy), options.ErrorHandlerStrategyType);
        services.AddScoped(typeof(IParametersParseStrategy), options.ParameterParseStrategyType);
        services.AddScoped(typeof(IParametersParser), options.ParserType);
        return services;
    }
}