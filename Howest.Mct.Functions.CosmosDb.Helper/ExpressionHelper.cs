using System;
using System.Linq.Expressions;

namespace Howest.Mct.Functions.CosmosDb.Helper;

public static class ExpressionHelper
{
    public static Expression<Func<T, bool>> CreateEqualExpression<T>(string propertyName, object value)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var member = Expression.Property(param, propertyName);
        var constant = Expression.Constant(value);
        var body = Expression.Equal(member, constant);
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}