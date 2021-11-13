using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace mesoBoard.Framework
{
    public static class Extensions
    {
        public static void AddModelErrorFor<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression, string errorMessage) where TModel : class
        {
            var inputName = ExpressionHelper.GetExpressionText(expression);
            modelState.AddModelError(inputName, errorMessage);
        }
    }
}