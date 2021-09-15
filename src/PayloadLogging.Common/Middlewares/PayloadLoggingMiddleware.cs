using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using PayloadLogging.Common.Extensions;
using PayloadLogging.Common.Models.PayloadLogging;
using PayloadLogging.Common.Models.PayloadLogging.Enum;
using PayloadLogging.Common.RestClients.Interface;
using PayloadLogging.Common.Settings;

namespace PayloadLogging.Common.Middlewares
{
    public class PayloadLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IRestService<PayloadModel> _restService;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        private readonly List<string> _ignoreUrls = new()
        {
            "/",
            "/health/live",
            "/health/ready"
        };

        private const string MethodName = "payload";

        public PayloadLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IRestService<PayloadModel> restService)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _restService = restService ?? throw new ArgumentNullException(nameof(restService));
            _logger = loggerFactory.CreateLogger<PayloadLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();

            if (!string.IsNullOrWhiteSpace(ApiSettings.IgnorePayloadUrls))
                _ignoreUrls.AddRange(ApiSettings.IgnorePayloadUrls.Split(",").ToList());
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = await LogRequest(context);
            await LogResponse(context, correlationId);
        }

        private async Task<string> LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var requestBody = ReadStreamInChunks(requestStream);
            context.Request.Body.Position = 0;

            if (_ignoreUrls.Contains(context.Request.Path))
            {
                return null;
            }
            var payload = BuildPayloadModel(context, requestBody, PayloadType.Request);

            var result = await _restService.Post(ApiSettings.PayloadLoggingHost, MethodName, payload);
            if (!result.IsSuccessful)
            {
                _logger.LogWarning($"Failed to write request to Payload Api with CorrelationId: '{ payload.CorrelationId }', Error: {result.Content}");
            }

            _logger.LogInformation($"{ PayloadType.Request } payload has been logged with CorrelationId: '{ payload.CorrelationId }'");
            return payload.CorrelationId;
        }

        private async Task LogResponse(HttpContext context, string correlationId)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var requestBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            if (_ignoreUrls.Contains(context.Request.Path))
            {
                return;
            }

            var payload = BuildPayloadModel(context, requestBody, PayloadType.Response, correlationId);

            var result = await _restService.Post(ApiSettings.PayloadLoggingHost, MethodName, payload);
            if (!result.IsSuccessful)
            {
                _logger.LogWarning($"Failed to write request to Payload Api with CorrelationId: '{ payload.CorrelationId }', Error: {result.Content}");
            }

            _logger.LogInformation($"{ PayloadType.Response } payload has been logged with CorrelationId: '{ payload.CorrelationId }'");
        }

        #region Private methods

        private PayloadModel BuildPayloadModel(HttpContext context, string requestBody, PayloadType? payloadType, string correlationId = null)
        {
            var payloadContent = new PayloadContent
            {
                Query = context.Request.QueryString.Value,
                Headers = context.Request.Headers?.ToStringDictionary(),
                Body = requestBody.MinifyJsonText()
            };

            var payload = new PayloadModel
            {
                Source = $"{context.Request.Method}: {context.Request.Host}{context.Request.Path}{payloadContent.Query}",
                Type = payloadType.ToString(),
                Payload = JsonConvert.SerializeObject(payloadContent)
            };

            switch (payloadType)
            {
                case PayloadType.Request:
                    payload.CorrelationId = payloadContent.Headers != null &&
                                            payloadContent.Headers.TryGetValue("X-PB-CorrelationId",
                                              out var payloadCorrelationId) && !string.IsNullOrWhiteSpace(payloadCorrelationId)
                      ? payloadCorrelationId
                      : $"{DateTime.UtcNow:yyyyMMddHHmmssffff}-{payloadContent.Body.Length}";
                    break;
                case PayloadType.Response:
                    payload.CorrelationId = correlationId;
                    payload.ResponseCode = context.Response.StatusCode;
                    break;
                default:
                    throw new InvalidOperationException("Invalid Payload type");
            }

            var stringBuilder = new StringBuilder()
              .AppendLine($"Http Response Information:")
              .AppendLine($"Payload Type: {payload.Type}")
              .AppendLine($"Source: {payload.Source}")
              .AppendLine($"Headers: {payloadContent.Headers.ToStringValue()}")
              .AppendLine($"StatusCode: {payload.ResponseCode}")
              .AppendLine($"Response Body: {requestBody.MinifyJsonText()}");

            _logger.LogDebug(stringBuilder.ToString());

            return payload;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        #endregion
    }
}
