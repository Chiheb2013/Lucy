using System;

using Lucy.DatabaseHandling;
using Lucy.Communication;

namespace Lucy.Knowledge
{
    class KnowledgeSearch
    {
        Database database;

        public KnowledgeSearch(Database database)
        {
            this.database = database;
        }

        public string GetKnowledgeResponse(string userInput)
        {
            Expression match = FindMatch(userInput);

            if (match != null)
            {
                string requestedKnowledge = GetRequestedKnowledge(userInput, match);

                return GetActualKnowledge(requestedKnowledge);
            }

            return string.Empty;
        }

        private Expression FindMatch(string userInput)
        {
            foreach (Expression expr in database.Expressions.Expressions)
                if (expr.Type == ExpressionType.Knowledge)
                    foreach (string expression in expr.Keywords)
                        if (IsSimilar(userInput, expression))
                            return expr;
            return null;
        }

        private bool IsSimilar(string userInput, string expression)
        {
            return userInput.Contains(expression.Replace('$', ' ').Trim());
        }

        private string GetRequestedKnowledge(string userInput, Expression match)
        {
            string expr = match.Keywords[new Random().Next(match.Keywords.Length)];

            if (KnowledgeAtEnd(expr))
                return userInput.Substring(expr.Length - 2).Trim();

            int dollarPos = expr.IndexOf('$');
            string nextWord = GetNextWordAfterKnowledge(expr);

            return userInput.Substring(dollarPos, nextWord.Length).Trim();
        }

        private bool KnowledgeAtEnd(string expr)
        {
            return expr[expr.Length - 1] == '$';
        }

        private string GetNextWordAfterKnowledge(string expr)
        {
            int dollarPos = expr.IndexOf('$');
            return expr.Substring(dollarPos + 1);
        }

        private string GetActualKnowledge(string requestedKnowledge)
        {
            Transposition knowledge = database.RetrieveKnowledge(requestedKnowledge);
            if (knowledge != null)
                return knowledge.Second;
            return "I DON'T KNOW WHAT YOU ARE TALKING ABOUT.";
        }
    }
}
