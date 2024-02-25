using Chatter.WebApp.Models;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chatter.WebApp.Extensions;

public class CustomErrorAttribute : ActionFilterAttribute, IExceptionFilter
{
    private static string _error = null;

    public void OnException(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled) return;

        Exception e = filterContext.Exception;
        filterContext.ExceptionHandled = true;
        _error = e.Message;
        Console.WriteLine(e.Message);
        RequestHeaders headers = filterContext.HttpContext.Request.GetTypedHeaders();
        Uri uriReferer = headers.Referer;

        filterContext.Result = new RedirectResult(uriReferer.AbsolutePath);
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await base.OnActionExecutionAsync(context, next);

        var controller = context.Controller as Controller;
        if (controller == null) return;
        
        if(_error != null)
            controller.TempData.Put("message", new ResultMessage()
        {
            Title = "Hata",
            Message = _error,
            Css = "danger"
        });;
        _error = null;
    }
}