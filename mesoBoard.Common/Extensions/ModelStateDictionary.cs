using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

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