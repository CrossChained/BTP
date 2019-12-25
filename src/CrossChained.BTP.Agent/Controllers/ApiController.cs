using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NBitcoin;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("MarginWalletScript")]
        [SwaggerOperation("Margin Wallet", "Get margin wallet")]
        public ActionResult<string> MarginWalletScript(
            [FromServices] IOptions<Config.AgentConfig> options)
        {
            var result = PayToMultiSigTemplate
                .Instance
                .GenerateScriptPubKey(2, options.Value.PublicKeys.Select(x => new NBitcoin.PubKey(x)).ToArray());

            return Ok(result.GetScriptAddress(Network.Main).ToString());
        }

    }
}
