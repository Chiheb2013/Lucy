using System;
using System.IO;

using Lucy.Communication;

namespace Lucy.DatabaseHandling
{
    class DatabaseSaver
    {
        string LEGAL_COMMENT =
            "# Lucy database -- Daaloul Chiheb (C) 2013" +
            "# This database contains all the expressions and knowledge " + Environment.NewLine +
            "# that your Lucy instance chatterbot can handle and respond to." + Environment.NewLine +
            "# Please, for your and my sake, don't touch this unless you know how" + Environment.NewLine +
            "# it works !";

        string path;
        Database database;

        public DatabaseSaver(Database database, string path)
        {
            this.path = path;
            this.database = database;
        }

        public void Save()
        {
            string output = GetOutput();

            File.WriteAllText(path, output);
        }

        private string GetOutput()
        {
            string text = LEGAL_COMMENT + Environment.NewLine +
                GetTranspositionString() + Environment.NewLine +
                GetKnowledgeString() + Environment.NewLine +
                GetExpressionString() + Environment.NewLine;

            return text;
        }

        private string GetTranspositionString()
        {
            string transpoText = string.Empty;

            foreach (Transposition transpo in database.Transpositions)
                transpoText += transpo.ToString() + Environment.NewLine;

            return transpoText;
        }

        private string GetKnowledgeString()
        {
            string knowledgeText = string.Empty;

            foreach (Transposition knowledge in database.Knowledge)
                knowledgeText += knowledge.ToString() + Environment.NewLine;

            return knowledgeText;
        }

        private string GetExpressionString()
        {
            string exprText = string.Empty;

            foreach (Expression expr in database.Expressions.Expressions)
                exprText += expr.ToString() + Environment.NewLine;

            return exprText;
        }
    }
}
