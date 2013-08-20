using System;
using System.IO;
using System.Collections.Generic;

using Lucy.Communication;

namespace Lucy.DatabaseHandling
{
    class DatabaseLoader
    {
        const string ENTRY = "entry:";
        const string KNOWLEDGE_ENTRY = "knowledge:";
        const string TRANSPOSITION = "transposition:";
        const string KEYWORDS = "keywords:";
        const string RESPONSES = "responses:";
        const string CONTEXTS = "contexts:";
        const string TYPE = "type:";
        const string LOCATION = "location:";

        string path;

        int readIndex;
        string[] databaseLines;

        Database database;

        public DatabaseLoader(string path)
        {
            this.path = path;

            this.databaseLines = new string[0];
            this.database = new Database();
        }

        public DatabaseLoader(string[] lines)
        {
            this.databaseLines = lines;

            this.database = new Database();
        }

        public Database Load()
        {
            if (databaseLines.Length == 0)
                GetDataBaseLines();

            CreateDataBase();

            return database;
        }

        private void GetDataBaseLines()
        {
            if (File.Exists(path))
                databaseLines = StringHelper.CleanLines(File.ReadAllLines(path));
            else throw new FileNotFoundException("Path was not found : " + path);
        }

        private void CreateDataBase()
        {
            for (; readIndex < databaseLines.Length; readIndex++)
            {
                if (databaseLines[readIndex].StartsWith(ENTRY))
                    AddNewExpression();
                if (databaseLines[readIndex].StartsWith(KNOWLEDGE_ENTRY))
                    AddNewKnowledgeEntry();
                if (databaseLines[readIndex].StartsWith(TRANSPOSITION))
                    AddNewTransposition();
            }
        }

        private void AddNewExpression()
        {
            readIndex++;

            Expression expr = GetNewExpression();
            database.AddExpression(expr);
        }

        private Expression GetNewExpression()
        {
            string type = string.Empty;
            string location = string.Empty;
            Expression expr = new Expression();

            DoExtractionProcess(ref type, location, expr);

            expr.Type = GetExpressionType(type);
            expr.Location = GetExpressionLocation(location);
            return expr;
        }

        private void DoExtractionProcess(ref string type, string location, Expression expr)
        {
            int z = 0;
            for (int i = 0; i < 3; z++, i++)
            {
                int p = i + readIndex;

                if (databaseLines[p].StartsWith(KEYWORDS))
                    ExtractKeywords(expr, p);
                if (databaseLines[p].StartsWith(RESPONSES))
                    ExtractResponses(expr, p);
                if (databaseLines[p].StartsWith(CONTEXTS))
                    ExtractContexts(expr, p);
                if (databaseLines[p].StartsWith(TYPE))
                    ExtractType(ref type, p);
                if (databaseLines[p].StartsWith(LOCATION))
                    ExtractLocation(location, p);
            }
            readIndex += z - 1;
        }

        const char SEPARATION = ',';

        private void ExtractKeywords(Expression expr, int actualPos)
        {
            expr.Keywords = databaseLines[actualPos].Substring(KEYWORDS.Length).Split(SEPARATION);
        }

        private void ExtractResponses(Expression expr, int actualPos)
        {
            expr.Responses = databaseLines[actualPos].Substring(RESPONSES.Length).Split(SEPARATION);
        }

        private void ExtractContexts(Expression expr, int actualPos)
        {
            expr.Contexts = databaseLines[actualPos].Substring(CONTEXTS.Length).Split(SEPARATION);
        }

        private void ExtractType(ref string type, int actualPos)
        {
            type = databaseLines[actualPos].Substring(TYPE.Length);
        }

        private void ExtractLocation(string location, int actualPos)
        {
            location = databaseLines[actualPos].Substring(LOCATION.Length);
        }

        private ExpressionType GetExpressionType(string type)
        {
            return type == "knowledge"  ? ExpressionType.Knowledge 
                                        : ExpressionType.Communication;
        }

        private ExpressionLocation GetExpressionLocation(string location)
        {
            return location == "alone" ? ExpressionLocation.Alone
                                        : location == "begmid" ? ExpressionLocation.BeginOrMiddle
                                        : location == "midend" ? ExpressionLocation.MiddleOrEnd
                                       : ExpressionLocation.IsIrrelevant;
        }

        private void AddNewKnowledgeEntry()
        {
            string[] knowledgeString = databaseLines[readIndex].Substring(KNOWLEDGE_ENTRY.Length).Split(SEPARATION);
            database.AddKnowledge(knowledgeString[0], knowledgeString[1]);
        }

        private void AddNewTransposition()
        {
            string[] transpoString = databaseLines[readIndex].Substring(TRANSPOSITION.Length).Split(SEPARATION);
            database.AddTransposition(transpoString[0], transpoString[1]);
        }
    }
}
