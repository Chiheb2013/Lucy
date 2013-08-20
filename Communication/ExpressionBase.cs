using System;
using System.Collections.Generic;

namespace Lucy.Communication
{
    class ExpressionBase
    {
        public List<Expression> Expressions { get; private set; }

        public Expression this[int index]
        {
            get
            {
                if (Expressions.Count > 0
                    && InMargins(index))
                    return Expressions[index];
                throw new IndexOutOfRangeException("ExpressionBase[int index] : index is not in range !");
            }
        }

        private bool InMargins(int index)
        {
            return index >= 0 && Expressions.Count - 1 >= index;
        }

        public ExpressionBase()
        {
            this.Expressions = new List<Expression>();
        }

        public void AddExpression(Expression newKey)
        {
            Expressions.Add(newKey);
        }

        public void AddExpression(string[] keywords, string[] responses, string[] contexts,
            ExpressionLocation location, ExpressionType type)
        {
            Expression newKey = new Expression(keywords, location, type, responses, contexts);

            AddExpression(newKey);
        }
    }
}
