using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monsta.dto;

namespace Monsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {

        [HttpGet("checkHealth")]
        public ActionResult<dynamic> getData()
        {
            return Ok(new {message = "authenticated data"});
        }



        //[HttpGet("getDatas"),AllowAnonymous]
        //public ActionResult<DummyData> getDatas()
        //{
        //    DummyData dummyData = new DummyData();

        //    dummyData.msg = "Sending data successfully";

        //    return Ok(dummyData);

        //    return BadRequest("bad result");
        //}
    }
}
