using Microsoft.Extensions.Options;
using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse.Separator;
using Telegramper.Executors.QueryHandlers.ParameterParser.Enums;
using Telegramper.Executors.QueryHandlers.ParameterParser.Models;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler;
using Telegramper.Executors.QueryHandlers.ParameterParser.Strategies;

namespace Telegramper.Executors.QueryHandlers.ParameterParser
{
    public class ParametersParser : IParametersParser
    {
        private readonly IParametersParseStrategy _strategy;
        private readonly ParametersParserOptions _parametersParserOptions;
        private readonly IParseErrorHandler _parseErrorHandler;
        private readonly UpdateContext _updateContext;

        public ParametersParser(IParametersParseStrategy strategy, IOptions<ParametersParserOptions> parametersParserOptions, IParseErrorHandler parseErrorHandler, UpdateContextAccessor updateContextAccessor)
        {
            _strategy = strategy;
            _parseErrorHandler = parseErrorHandler;
            _updateContext = updateContextAccessor.UpdateContext;
            _parametersParserOptions = parametersParserOptions.Value;
        }

        public ParametersParseResult TryParse(ExecutorMethod method)
        {
            var parametersInfos = method.MethodInfo.GetParameters();
            var argsAsString = getArgs();
            var defaultSeparator = getDefaultSeparator(method);

            if (parametersInfos.Length == 0)
            {
                return success(Array.Empty<object?>());
            }

            if (argsAsString.Length == 0)
            {
                return error(ParseStatus.ArgsLengthIsLess, method, Array.Empty<object?>());
            }

            var argsAsArray = argsAsString.Split(defaultSeparator);
            var status = _strategy.TryParse(argsAsArray, parametersInfos, out var convertedArgs);

            return status == ParseStatus.Success
                ? success(convertedArgs)
                : error(status, method, convertedArgs);
        }

        private ParametersParseResult success(object?[] convertedArgs)
        {
            return new ParametersParseResult
            {
                ConvertedParameters = convertedArgs,
                ErrorMessage = null
            };
        }
        
        private ParametersParseResult error(ParseStatus status, ExecutorMethod method, object?[] convertedArgs)
        {
            return new ParametersParseResult
            {
                ConvertedParameters = convertedArgs,
                ErrorMessage = _parseErrorHandler.Handle(status, convertedArgs, method)
            };
        }

        private string getArgs()
        {
            return _updateContext.Message?.Text
                   ?? _updateContext.Update.CallbackQuery?.Data
                   ?? "";
        }

        private string getDefaultSeparator(ExecutorMethod method)
        {
            return method.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                   _parametersParserOptions.DefaultSeparator;
        }
    }
}
