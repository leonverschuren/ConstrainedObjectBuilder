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

        public ObjectBuilder<T> ShouldBeGreater<TValue>(System.Linq.Expressions.Expression<Func<T, TValue>> expression1, System.Linq.Expressions.Expression<Func<T, TValue>> expression2)
        {
            VariableInteger variable1 = CreateVariable(expression1.Body);
            VariableInteger variable2 = CreateVariable(expression2.Body);

            Constraints.Add(new ConstraintInteger(variable1 > variable2));

            return this;
        }

        public ObjectBuilder<T> Constraint(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            if (expression.Body is BinaryExpression binaryExpression)
            {
                var variable1 = CreateVariable(binaryExpression.Left);
                var variable2 = CreateVariable(binaryExpression.Right);

                var variableIntegerExpression = Expression.MakeBinary(binaryExpression.NodeType, Create(variable1), Create(variable2));

                var result = Expression.Lambda(variableIntegerExpression).Compile().DynamicInvoke();
                Constraints.Add(new ConstraintInteger((ExpressionInteger)result));

                return this;
            }

            throw new ArgumentException();
        }

        private static Expression Create(VariableInteger variableInteger) => Expression.Constant(variableInteger, typeof(ExpressionInteger));

        private VariableInteger CreateVariable(Expression expression)
        {
            if (expression is MemberExpression memberSelectorExpression && memberSelectorExpression.Member is PropertyInfo property)
            {
                if (!Variables.ContainsKey(property))
                {
                    Variables.Add(property, new VariableInteger(property.Name, 0, 9));
                }

                return Variables[property];
            }

            throw new ArgumentException(nameof(expression));
        }

        public abstract T Build();
    }
}
