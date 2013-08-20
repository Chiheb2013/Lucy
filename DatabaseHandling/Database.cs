using System.Collections.Generic;

using Lucy.Communication;

namespace Lucy.DatabaseHandling
{
    class Database
    {
        bool changed;

        ExpressionBase expressions;
        List<Transposition> knowledge;
        List<Transposition> transpositions;

        public bool Changed { get { return changed; } }

        public ExpressionBase Expressions { get { return expressions; } }
        public Transposition[] Knowledge { get { return knowledge.ToArray(); } }
        public Transposition[] Transpositions { get { return transpositions.ToArray(); } }

        public Database()
        {
            this.changed = false;

            this.expressions = new ExpressionBase();
            this.knowledge = new List<Transposition>();
            this.transpositions = new List<Transposition>();
        }

        public Database(ExpressionBase expressions, List<Transposition> knowledge,
            List<Transposition> transpositions)
        {
            this.changed = false;

            this.knowledge = knowledge;
            this.expressions = expressions;
            this.transpositions = transpositions;
        }

        public void AddKnowledge(Transposition newKnowledge, bool changer = false)
        {
            if (changer) changed = true;

            knowledge.Add(newKnowledge);
        }

        public void AddKnowledge(string title, string content, bool changer = false)
        {
            Transposition newKnowledge = new Transposition("knowledge", title, content);

            AddKnowledge(newKnowledge, changer);
        }

        public void AddTransposition(Transposition newTransposition)
        {
            transpositions.Add(newTransposition);
        }

        public void AddTransposition(string from, string to)
        {
            Transposition newTransposition = new Transposition("transposition", from, to);

            AddTransposition(newTransposition);
        }

        public void AddExpression(Expression expression, bool changer = false)
        {
            if (changer) changed = true;

            expressions.AddExpression(expression);
        }

        public void AddExpression(string[] keywords, string[] responses, string[] contexts,
            ExpressionType type, ExpressionLocation location, bool changer = false)
        {
            Expression newExpression = new Expression(keywords, location, type, responses, contexts);

            AddExpression(newExpression, changer);
        }

        public Transposition RetrieveKnowledge(string title)
        {
            return knowledge.Find(x => x.First == title);
        }

        public Expression GetQuitExpression()
        {
            return expressions[0];
        }
    }
}
