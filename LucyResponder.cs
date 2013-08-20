using System;
using System.Linq;
using System.Collections.Generic;

using Lucy.Knowledge;
using Lucy.Communication;
using Lucy.DatabaseHandling;

namespace Lucy
{
    class LucyResponder
    {
        const int KEYWORD_NOT_FOUND_IN_INPUT = -1;

        string userInput;

        Database database;
        KnowledgeSearch knowledgeSearch;

        string response;
        string lastResponse;
        List<string> availableResponses;

        string lastContext;
        string currentContext;
        
        public string Response { get { return response; } }

        public LucyResponder(Database database)
        {
            this.lastContext = string.Empty;
            this.currentContext = string.Empty;

            this.response = string.Empty;
            this.lastResponse = string.Empty;
            this.availableResponses = new List<string>();

            this.database = database;
            this.knowledgeSearch = new KnowledgeSearch(database);
        }

        public void GetResponse(string userInput)
        {
            this.userInput = userInput;
            GetAvailableResponses();

            if (UnderstandsUserInput())
            {
                SelectResponse();
                PostProcessTransposition();

                lastResponse = response;
            }
            else
                response = "SORRY, I DIDN'T UNDERSTAND WHAT YOU JUST SAID.";
        }

        private void GetAvailableResponses()
        {
            availableResponses.Clear();

            foreach (Expression expr in database.Expressions.Expressions)
                if (expr.Type == ExpressionType.Communication)
                    GetCommunicationResponses(expr);
                else
                    GetKnowledgeResponse();
        }

        private void GetCommunicationResponses(Expression expr)
        {
            foreach (string keyword in expr.Keywords)
                if (EligibleAsResponse(expr, keyword))
                    AddExpressionResponsesToResponseList(expr);
        }

        private bool EligibleAsResponse(Expression expr, string keyword)
        {
            string ui = StringHelper.InsertSpacesBAF(userInput);
            int keywordPosition = ui.IndexOf(StringHelper.InsertSpacesBAF(keyword));

            if (keywordPosition != KEYWORD_NOT_FOUND_IN_INPUT)
            {
                if (WrongLocation(keyword, expr.Location, keywordPosition))
                    return false;
                if (WrongContext(expr.Contexts))
                    return false;
                return true;
            }
            return false;
        }

        private void AddExpressionResponsesToResponseList(Expression expr)
        {
            foreach (string response in expr.Responses)
                if (!availableResponses.Contains(response))
                    availableResponses.Add(response);
        }

        private bool WrongLocation(string keyword, ExpressionLocation exprLocation,
            int pos)
        {
            if (exprLocation == ExpressionLocation.IsIrrelevant) return false;

            bool isNotAlone = exprLocation == ExpressionLocation.Alone && userInput != keyword;
            bool isNotMiddle = exprLocation == ExpressionLocation.MiddleOrEnd && pos != userInput.Length;
            bool isNotBegin = exprLocation == ExpressionLocation.BeginOrMiddle && pos == userInput.Length;

            return isNotAlone || isNotMiddle || isNotBegin;
        }

        private bool WrongContext(string[] contexts)
        {
            if (contexts.Length == 0) return false;
            string lastResponseContext = StringHelper.CleanString(lastResponse);

            foreach (string context in contexts)
                if (context == lastResponseContext)
                {
                    lastContext = currentContext;
                    currentContext = context;
                    return false;
                }
            return true;
        }

        private void GetKnowledgeResponse()
        {
            string knowledge = knowledgeSearch.GetKnowledgeResponse(userInput);

            availableResponses.Add(knowledge);
            availableResponses.Remove(string.Empty);
        }

        public bool UnderstandsUserInput()
        {
            return availableResponses.Count > 0;
        }

        private void SelectResponse()
        {
            availableResponses.Remove(lastResponse);

            string previousResponse = FindBestResponse();

            response = previousResponse;
        }

        private string FindBestResponse()
        {
            string previousResponse = string.Empty;
            
            foreach (string response in availableResponses)
                if (response.Length > previousResponse.Length)
                    previousResponse = response;

            return previousResponse;
        }

        private void PostProcessTransposition()
        {
            string subject = string.Empty;

            if (response.Contains('*'))
            {
                subject = Transpose(userInput);
                subject = ReplaceSymbol(response, '*', subject);
            }
        }

        private string Transpose(string subject)
        {
            string transposed = subject;

            foreach (Transposition transpo in database.Transpositions)
                transposed = DoTransposition(transpo, transposed);

            return transposed;
        }

        private string DoTransposition(Transposition transpo, string transposed)
        {
            string first = transpo.First;
            string second = transpo.Second;

            InsertSpace(first);
            InsertSpace(second);

            return transposed.Replace(first, second);
        }

        private string ReplaceSymbol(string toChange, char symbol, string change)
        {
            int pos = toChange.IndexOf(symbol);
            return toChange.Remove(pos) + change;
        }

        private void InsertSpace(string str)
        {
            str = str.Insert(0, " ");
            str = str.Insert(str.Length - 1, " ");
        }
    }
}
