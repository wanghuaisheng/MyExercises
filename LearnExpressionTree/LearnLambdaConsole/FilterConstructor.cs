using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LearnLambdaConsole
{

    public class FilterConstructor : ExpressionVisitor
    {
        private static readonly Dictionary<ExpressionType, string> _logicalOperators;
        private static readonly Dictionary<Type, Func<object, string>> _typeConverters;
        static FilterConstructor()
        {
            //mappings for table, shown above
            _logicalOperators = new Dictionary<ExpressionType, string>
            {
                [ExpressionType.Not] = "not",
                [ExpressionType.GreaterThan] = "gt",
                [ExpressionType.GreaterThanOrEqual] = "ge",
                [ExpressionType.LessThan] = "lt",
                [ExpressionType.LessThanOrEqual] = "le",
                [ExpressionType.Equal] = "eq",
                [ExpressionType.Not] = "not",
                [ExpressionType.AndAlso] = "and",
                [ExpressionType.OrElse] = "or"
            };

            //if type is string we will wrap it into single quotes
            //if it is a DateTime we will format it like datetime'2008-07-10T00:00:00Z'
            //bool.ToString() returns "True" or "False" with first capital letter, so .ToLower() is applied
            //if it is one of the rest "simple" types we will just call .ToString() method on it
            _typeConverters = new Dictionary<Type, Func<object, string>>
            {
                [typeof(string)] = x => $"'{x}'",
                [typeof(DateTime)] =
                x => $"datetime'{((DateTime)x).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}'",
                [typeof(bool)] = x => x.ToString().ToLower(),
                [typeof(List<string>)] = x => "'"+string.Join("','", (List<string>)x)+"'",
            };
        }

        private StringBuilder _queryStringBuilder;
        private Stack<string> _fieldNames;
        private Dictionary<string, object> _paras;

        public Dictionary<string, object> GetParas()
        {
            return _paras;
        }
        public FilterConstructor()
        {
            //here we will collect our query
            _queryStringBuilder = new StringBuilder();
            //will be discussed below
            _fieldNames = new Stack<string>();

            _paras = new Dictionary<string, object>();
        }

        //entry point
        public string GetQuery(LambdaExpression predicate)
        {
            //Visit transfer abstract Expression to concrete method, like VisitUnary
            //it's invocation chain (at case of unary operator) approximetely looks this way:
            //inside visitor: predicate.Body.Accept(ExpressionVisitor this)
            //inside expression(visitor is this from above): visitor.VisitUnary(this) 
            //here this is Expression
            //we not pass whole predicate, just Body, because we not need predicate.Parameters: "x =>" part
            Visit(predicate.Body);
            var query = _queryStringBuilder.ToString();
            _queryStringBuilder.Clear();

            return query;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            //assume we only allow not (!) unary operator:
            if (node.NodeType != ExpressionType.Not)
                throw new NotSupportedException("Only not(\"!\") unary operator is supported!");


            //_queryStringBuilder.Append($"{_logicalOperators[node.NodeType]} ");//!

            _queryStringBuilder.Append("(");                                   //(!
                                                                               //go down from a tree
            Visit(node.Operand);                                               //(!expression

            _paras.Add("p1", 0);
            _queryStringBuilder.Append(" = @p1");
            _queryStringBuilder.Append(")");                                   //(!expression)

            //we should return expression, it will allow to create new expression based on existing one,
            //but, at our case, it is not needed, so just return initial node argument
            return node;
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            return base.VisitInvocation(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {


            if (node.Method.Name == "Contains")
            {
                Visit(node.Arguments);

                _queryStringBuilder.Append(" IN ");

                _queryStringBuilder.Append("(");

            var exp = Visit(node.Object);
               
                _queryStringBuilder.Append(")");
            }
            return node;
        }

        //corresponds to: and, or, greater than, less than, etc.
        protected override Expression VisitBinary(BinaryExpression node)
        {
            _queryStringBuilder.Append("(");                                    //(
            //left side of binary operator
            Visit(node.Left);                                                   //(leftExpr

            _queryStringBuilder.Append($" {_logicalOperators[node.NodeType]} ");//(leftExpr and

            //right side of binary operator
            Visit(node.Right);                                                  //(leftExpr and RighExpr
            _queryStringBuilder.Append(")");                                    //(leftExpr and RighExpr)

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            //corresponds to: order.Customer, .order, today variables
            //when we pass parameters to expression via closure, CLR internally creates class:

            //class NameSpace+<>c__DisplayClass12_0
            //{
            //    public Order order;
            //    public DateTime today;
            //}

            //which contains values of parameters. When we face order.Customer, it's node.Expression
            //will not have reference to value "Tom", but instead reference to parent (.order), so we
            //will go to it via Visit(node.Expression) and also save node.Member.Name into 
            //Stack(_fieldNames) fo further usage. order.Customer has type ExpressionType.MemberAccess. 
            //.order - ExpressionType.Constant, because it's node.Expression is ExpressionType.Constant
            //(VisitConstant will be called) that is why we can get it's actual value(instance of Order). 
            //Our Stack at this point: "Customer" <- "order". Firstly we will get "order" field value, 
            //when it will be reached, on NameSpace+<>c__DisplayClass12_0 class instance
            //(type.GetField(fieldName)) then value of "Customer" property
            //(type.GetProperty(fieldName).GetValue(input)) on it. We started from 
            //order.Customer Expression then go up via reference to it's parent - "order", get it's value 
            //and then go back - get value of "Customer" property on order. Forward and backward
            //directions, at this case, reason to use Stack structure

            if (node.Expression.NodeType == ExpressionType.Constant
                ||
                node.Expression.NodeType == ExpressionType.MemberAccess)
            {
                
                _fieldNames.Push(node.Member.Name);
                Visit(node.Expression);
            }
            else
            {
                //corresponds to: x.Customer - just write "Customer"
                var paraName = node.Member.Name;
                _queryStringBuilder.Append(paraName);
            }
            return node;
        }

        //corresponds to: 1, "Tom", instance of NameSpace+<>c__DisplayClass12_0, instance of Order, i.e.
        //any expression with value
        protected override Expression VisitConstant(ConstantExpression node)
        {
            //just write value
            var paraName = $"@p{Guid.NewGuid()}";
            var val = GetValue(node.Value);
            _paras.Add(paraName,val);
            _queryStringBuilder.Append(paraName);
            return node;
        }

        private string GetValue(object input)
        {
            var type = input.GetType();
            //if it is not simple value
            if (type.IsClass && type != typeof(string)&& type != typeof(List<string>))
            {
                //proper order of selected names provided by means of Stack structure
                var fieldName = _fieldNames.Pop();
                var fieldInfo = type.GetField(fieldName);
                object value;
                if (fieldInfo != null)
                    //get instance of order    
                    value = fieldInfo.GetValue(input);
                else
                    //get value of "Customer" property on order
                    value = type.GetProperty(fieldName).GetValue(input);
                return GetValue(value);
            }
            else
            {
                //our predefined _typeConverters
                if (_typeConverters.ContainsKey(type))
                    return _typeConverters[type](input);
                else
                    //rest types
                    return input.ToString();
            }
        }

        #region 

        //protected override Expression VisitBinary(BinaryExpression node)
        //{
        //    return node;
        //}
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.BlockExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitBlock(BlockExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.ConditionalExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return node;
        }
        ///// <summary>
        /////   访问 <see cref="T:System.Linq.Expressions.ConstantExpression" />。
        ///// </summary>
        ///// <param name="node">要访问的表达式。</param>
        ///// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        //protected override Expression VisitConstant(ConstantExpression node)
        //{
        //    return node;
        //}
        /// <summary>
        ///   访问 <see cref="T:System.Linq.Expressions.DebugInfoExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.DynamicExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问 <see cref="T:System.Linq.Expressions.DefaultExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitDefault(DefaultExpression node)
        {
            return node;
        }
        /// <summary>访问扩展的表达式的子级。</summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitExtension(Expression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.GotoExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitGoto(GotoExpression node)
        {
            return node;
        }
        ///// <summary>
        /////   访问的子级 <see cref="T:System.Linq.Expressions.InvocationExpression" />。
        ///// </summary>
        ///// <param name="node">要访问的表达式。</param>
        ///// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        //protected override Expression VisitInvocation(InvocationExpression node)
        //{
        //    return node;
        //}
        /// <summary>
        ///   访问 <see cref="T:System.Linq.Expressions.LabelTarget" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual LabelTarget VisitLabelTarget(LabelTarget node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.LabelExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitLabel(LabelExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.Expression`1" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <typeparam name="T">该委托的类型。</typeparam>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.LoopExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitLoop(LoopExpression node)
        {
            return node;
        }
        ///// <summary>
        /////   访问的子级 <see cref="T:System.Linq.Expressions.MemberExpression" />。
        ///// </summary>
        ///// <param name="node">要访问的表达式。</param>
        ///// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        //protected override Expression VisitMember(MemberExpression node)
        //{
        //    return node;
        //}
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.IndexExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitIndex(IndexExpression node)
        {
            return node;
        }
        ///// <summary>
        /////   访问的子级 <see cref="T:System.Linq.Expressions.MethodCallExpression" />。
        ///// </summary>
        ///// <param name="node">要访问的表达式。</param>
        ///// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        //protected override Expression VisitMethodCall(MethodCallExpression node)
        //{
        //    return node;
        //}
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.NewArrayExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.NewExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitNew(NewExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问 <see cref="T:System.Linq.Expressions.ParameterExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.RuntimeVariablesExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.SwitchCase" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual SwitchCase VisitSwitchCase(SwitchCase node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.SwitchExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.CatchBlock" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual CatchBlock VisitCatchBlock(CatchBlock node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.TryExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitTry(TryExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.TypeBinaryExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return node;
        }
        ///// <summary>
        /////   访问的子级 <see cref="T:System.Linq.Expressions.UnaryExpression" />。
        ///// </summary>
        ///// <param name="node">要访问的表达式。</param>
        ///// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        //protected override Expression VisitUnary(UnaryExpression node)
        //{
        //    return node;
        //}
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.MemberInitExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.ListInitExpression" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.ElementInit" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual ElementInit VisitElementInit(ElementInit node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.MemberBinding" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual MemberBinding VisitMemberBinding(MemberBinding node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.MemberAssignment" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.MemberMemberBinding" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            return node;
        }
        /// <summary>
        ///   访问的子级 <see cref="T:System.Linq.Expressions.MemberListBinding" />。
        /// </summary>
        /// <param name="node">要访问的表达式。</param>
        /// <returns>修改后的表达式，如果它或任何子表达式已修改;否则，返回原始的表达式。</returns>
        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            return node;
        }

        #endregion

    }
}
