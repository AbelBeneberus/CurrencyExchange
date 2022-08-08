using CurrencyExchange.Application.Commands.TradeCurrencyExchangeCommand;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Api.Controllers
{
	[Route("api/[controller]")]
	public class CurrencyExchangeController : ApiControllerBase
	{   
		[HttpPost]
		public async Task<ActionResult> MakeAnExchange(TradeCurrencyExchangeCommand command)
		{
			return Ok(await Mediator.Send(command));
		}
	}
}
