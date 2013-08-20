using Lucy.Communication;
using Lucy.DatabaseHandling;

namespace Lucy
{
    class IOKnowledgeAdder
    {
        IO io;
        Database database;

        public IOKnowledgeAdder(IO io, Database database)
        {
            this.io = io;
            this.database = database;
        }

        public void AddNewData()
        {
            ExpressionType type = GetNewDataType();

            if (type == ExpressionType.Communication)
                AddNewExpression();
            else
                AddNewKnowledge();
        }

        private ExpressionType GetNewDataType()
        {
            io.WriteLine("IS THE NEW DATA KNOWLEDGE OR COMMUNICATION ?");
            io.WriteLine("PRESS '1' FOR KNOWLEDGE, OTHERWISE COMMUNICATION.");
            io.Write("> ");

            if (int.Parse(io.ReadLine()) == 1)
                return ExpressionType.Knowledge;
            return ExpressionType.Communication;
        }

        private void AddNewExpression()
        {
            io.WriteLine("PLEASE WRITE THE NEW EXPRESION FOLLOWING THE");
            io.WriteLine("THE DATABASE RULES :");

            io.WriteLine("Ok. So what is the keywords that match this expression ?\nSeparate them with comas and write upper-case.");
            io.Write("> ");
            string[] keywords = io.ReadLine().Split(',');

            io.WriteLine("And what should I respond to that ?\nSeparate them with comas and write upper-case.");
            io.Write("> ");
            string[] resps = io.ReadLine().Split(',');

            io.WriteLine("In what contexts should I respond this ?\nSeparate them with comas and write upper-case.");
            io.Write("> ");
            string[] contexts = io.ReadLine().Split(',');

            io.WriteLine("What is this expression's type ? (1-knowledge, 2-comm.)");
            ExpressionType type = io.ReadLine() == "1" ? ExpressionType.Knowledge : ExpressionType.Communication;

            io.WriteLine("What is this expression's location ?\nbegmid, alone, midend or irr ?");
            io.Write("> ");
            string input = io.ReadLine();

            ExpressionLocation location =
                input == "alone" ? ExpressionLocation.Alone
                : input == "begmid" ? ExpressionLocation.BeginOrMiddle
                : input == "medend" ? ExpressionLocation.MiddleOrEnd
                : ExpressionLocation.IsIrrelevant;

            Expression expr = new Expression(keywords, location, type, resps, contexts);
            database.AddExpression(expr, true);
        }

        private void AddNewKnowledge()
        {
            io.WriteLine("Ok. So what is the knowledge title ?");
            io.Write("> ");
            string title = io.ReadLine();

            io.WriteLine("And with what data does it match ?");
            io.Write("> ");
            string data = io.ReadLine();

            Transposition knowledge = new Transposition("knowledge", title, data);
            database.AddKnowledge(knowledge, true);
        }
    }
}
