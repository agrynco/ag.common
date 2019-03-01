using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Web.API.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }
    }
}