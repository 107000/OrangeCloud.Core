using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OrangeCloud.Core.AiExpression
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(System.Linq.Expressions.ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }

        public Expression Replace(Expression expr) =>
            this.Visit(expr);

        protected override Expression VisitParameter(System.Linq.Expressions.ParameterExpression p) =>
            this.ParameterExpression;

        public System.Linq.Expressions.ParameterExpression ParameterExpression { get; private set; }
    }
}
