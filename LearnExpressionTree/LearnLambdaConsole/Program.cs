using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LearnLambda;

namespace LearnLambdaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var date = DateTime.Today;
            var order = new Order { Customer = "Tom", Amount = 1000 };
            var customers = new List<string> { "customer1", "customer2", "customer3" };

            Expression<Func<Order, bool>> filter = x => 
                 //(x.Customer == order.Customer || x.Amount > order.Amount)
                 //&&
                 customers.Contains(x.Customer)
                 //&&
                 //(x.TheDate == date ||  !x.Discount)
                 ;//x.Discount==false);
            var visitor = new FilterConstructor();
            var query = visitor.GetQuery(filter);
            var paras = visitor.GetParas();
            //desired formatted query:
            //(
            //    (
            //        (Customer eq 'Tom') 
            //        and 
            //        (Amount gt 1000)
            //    ) 
            //    or 
            //    (
            //        (TheDate eq datetime'2018-04-27T21:00:00.000Z') 
            //        and 
            //        not (Discount)
            //    )
            //)



            //x=>x>5 和 x=>x<10 -->  x=> x>5 && x<10
            Expression<Func<int, int, bool>> exp1 = (x, y) => x > 5 && y > 4;
            Expression<Func<int, int, bool>> exp2 = (x, y) => x < 10 && y < 8;
            ParameterExpression yPara1 = Expression.Parameter(typeof(int), "y1");
            ParameterExpression yPara2 = Expression.Parameter(typeof(int), "y2");
            var newExp = new ReplaceParamVisitor(yPara1, yPara2);
            var newexp1 = newExp.Replace(exp1.Body);
            var newexp2 = newExp.Replace(exp2.Body);
            var newbody = Expression.And(newexp1, newexp2);
            Expression<Func<int,int, bool>> res = Expression.Lambda<Func<int,int, bool>>(newbody, yPara1,yPara2);
            //将表达式树描述的lambda表达式编译为可执行代码，并生成表示该lambda表达式的委托
            Func<int,int, bool> del = res.Compile();
            Console.WriteLine(del(7,6));
            Console.ReadLine();
            Expression<Func<double, double>> exp = a => Math.Sin(a);


            //创建表达式树：Expression<Func<int, int>> exp = x => x + 1;
            ParameterExpression param = Expression.Parameter(typeof(int), "x");
            ConstantExpression value = Expression.Constant(1, typeof(int));
            BinaryExpression body = Expression.Add(param, value);
            Expression<Func<int, int>> lambdatree = Expression.Lambda<Func<int, int>>(body, param);
            Console.WriteLine("参数param：{0}", param);
            Console.WriteLine("描述body：{0}", body);
            Console.WriteLine("表达式树：{0}", lambdatree);
            //解析表达式树：
            //取得表达式树的参数
            ParameterExpression dparam = lambdatree.Parameters[0] as ParameterExpression;
            //取得表达式树描述
            BinaryExpression dbody = lambdatree.Body as BinaryExpression;
            //取得节点
            ParameterExpression left = dbody.Left as ParameterExpression;
            ConstantExpression right = body.Right as ConstantExpression;
            Console.WriteLine("解析出的表达式：{0}=>{1} {2} {3}", param.Name, left.Name, body.NodeType, right.Value);
            Console.ReadLine();

            //委托：方法作为参数传递
            var r1 = Calculate(8, 4, Sum);
            //使用匿名方法传递委托
            var r4 = Calculate(8, 4, delegate (int x, int y) { return x - y; });
            //语句lambda传递委托
            var r2 = Calculate(8, 4, (a, b) => { return a * b; });
            //lambda传递委托
            var r3 = Calculate(8, 4, (a, b) => a / b);
            Console.ReadLine();
        }

        public delegate int CalculateDelegate(int n1, int n2);

        public static int Calculate(int a, int b, CalculateDelegate @delegate)
        {
            return @delegate(a, b);
        }
        public static int Sum(int a, int b)
        {
            return a + b;
        }
    }
}
