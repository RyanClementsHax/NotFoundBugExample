using Microsoft.AspNetCore.Mvc;

namespace NotFoundBugExample.Controllers
{
    [ApiController]
    public class NotFoundController : ControllerBase
    {
        /// <summary>
        /// This endpoint works fine and returns a 400 as expected in all cases
        /// </summary>
        /// <returns></returns>
        [HttpGet("throw-domain-ex")]
        public IActionResult ThrowDomainEx() => throw new DomainException();

        /// <summary>
        /// This endpoint returns a 404 in IIS Express and causes http protocol errors otherwise when using the builder pattern for UseExceptionHandler and not configuring AllowStatusCode404Response and ExceptionHandler on ExceptionHandlerOptions.
        /// Chrome logs this as "Failed to load resource: net::ERR_HTTP2_PROTOCOL_ERROR".
        /// Trying to call the endpoint when it isn't running in IIS Express under this situation with something like HttpClient will cause: System.Net.Http.HttpRequestException: Error while copying content to a stream. ---> System.IO.IOException:  ---> NotFoundBugExample.NotFoundBugExample
        /// </summary>
        /// <returns></returns>
        [HttpGet("throw-not-found")]
        public IActionResult ThrowNotFound() => throw new NotFoundException();

        /// <summary>
        /// This endpoint works fine and returns a 404 as expected in all cases
        /// </summary>
        /// <returns></returns>
        [HttpGet("return-not-found")]
        public IActionResult ReturnNotFound() => NotFound();
    }
}
