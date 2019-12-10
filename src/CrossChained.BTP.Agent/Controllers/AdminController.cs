using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace CrossChained.BTP.Agent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost("Unlock")]
        [SwaggerOperation("Unlock agent", "Unlock agent")]
        public ActionResult Unlock(
            [FromBody] Dto.UnlockModelDto model,
            [FromServices] IOptions<Config.AgentConfig> options)
        {
            if(options.Value.Address != NBitcoin.Key.Parse(model.Key).PubKey.GetAddress(
                NBitcoin.ScriptPubKeyType.Legacy,
                NBitcoin.Network.Main).ToString())
            {
                return Unauthorized("Invalid key");
            }

            options.Value.Key = model.Key;
            return Ok();
        }
    }
}