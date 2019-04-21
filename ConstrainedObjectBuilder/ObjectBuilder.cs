using Decider.Csp.BaseTypes;
using Decider.Csp.Integer;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ConstrainedObjectBuilder
{
    public abstract class ObjectBuilder<T>
    {
        protected IDictionary<PropertyInfo, VariableInteger> Variables { get; } = new Dictionary<PropertyInfo, VariableInteger>();
        protected IList<IConstraint> Constraints { get; } = new List<IConstraint>();

        public ObjectBuilder<T> Constraint(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            if (expression.Body is BinaryExpression binaryExpression)
            {
                var left = CreateExpression(binaryExpression.Left);
                var right = CreateExpression(binaryExpression.Right);

                var variableIntegerExpression = Expression.MakeBinary(binaryExpression.NodeType, left, right);

                var result = Expression.Lambda(variableIntegerExpression).Compile().DynamicInvoke();
                Constraints.Add(new ConstraintInteger((ExpressionInteger)result));

                return this;
            }

            throw new ArgumentException();
        }

        private Expression CreateExpression(Expression input)
        {
            if (input is MemberExpression memberExpression && memberExpression.Member is PropertyInfo property)
            {
                VariableInteger variable1 = CreateVariable(property);

                return Expression.Constant(variable1, typeof(ExpressionInteger));
            }

            object value = Expression.Lambda(input).Compile().DynamicInvoke();

            switch (value)
            {
                case DateTime date:
                    return Expression.Constant((ExpressionInteger)DateHelper.ToInt(date), typeof(ExpressionInteger));
                case int number:
                    return Expression.Constant((ExpressionInteger)number, typeof(ExpressionInteger));
            }

            throw new ArgumentException(nameof(input));
        }

        private VariableInteger CreateVariable(PropertyInfo property)
        {
            if (!Variables.ContainsKey(property))
            {
                Variables.Add(property, new VariableInteger(property.Name, 0, 9));
            }

            return Variables[property];
        }

        public abstract T Build();
    }
}
