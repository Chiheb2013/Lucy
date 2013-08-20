using Lucy.DatabaseHandling;

namespace Lucy.Knowledge
{
    class NonIOKnowledgeAdder
    {
        Database database;

        public NonIOKnowledgeAdder(Database database)
        {
            this.database = database;
        }

        public void AddFromString(string knowledgeString)
        {
            string[] lines = knowledgeString.Replace("$new$", "").Trim().Split('\r', '\n');

            DatabaseLoader loader = new DatabaseLoader(lines);
            Database buffer = loader.Load();

            if (buffer.Expressions.Expressions.Count > 0)
                database.AddExpression(buffer.Expressions.Expressions[0], true);
            else
                database.AddKnowledge(buffer.Knowledge[0], true);
        }
    }
}
