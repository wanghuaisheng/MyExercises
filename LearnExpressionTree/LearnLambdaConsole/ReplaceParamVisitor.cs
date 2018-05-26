using System;
using System.Linq.Expressions;

namespace LearnLambdaConsole
{

    public class ReplaceParamVisitor : ExpressionVisitor
    {

        public ParameterExpression Param1 { get; set; }

        public ParameterExpression Param2 { get; set; }

        public ReplaceParamVisitor(ParameterExpression p1, ParameterExpression p2)
        {
            Param1 = p1;
            Param2 = p2;
        }

        public Expression Replace(Expression exp)
        {
            return this.Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Name == "x") return Param1;
            if (node.Name == "y") return Param2;
            throw new ArgumentNullException("无参数" + node.Name);
            ////return base.VisitParameter(node);
            //return this.Param1;
        }
    }
}
