using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtAuthentication.Filters
{
    public class DebugResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine($"{context.ActionDescriptor.DisplayName}... is executing");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
             Console.WriteLine($"{context.ActionDescriptor.DisplayName}... is executed");
        }
    }
}