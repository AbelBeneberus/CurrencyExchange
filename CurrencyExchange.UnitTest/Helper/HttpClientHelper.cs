using System.Net;
using System.Net.Http.Headers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace CurrencyExchange.UnitTest.Helper
{
	public static class HttpClientHelper
	{
		public static Mock<HttpMessageHandler> GetResults<T>(T response)
		{
			var mockResponse = new HttpResponseMessage()
			{
				Content = new StringContent(JsonConvert.SerializeObject(response)),
				StatusCode = HttpStatusCode.OK
			};

			mockResponse.Content.Headers.ContentType =
				new MediaTypeHeaderValue("application/json");

			var mockHandler = new Mock<HttpMessageHandler>();

			mockHandler
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(mockResponse);

			return mockHandler;
		}
	}
 
}
