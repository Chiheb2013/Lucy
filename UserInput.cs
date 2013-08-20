using System;
using System.Linq;

using Lucy.Communication;

namespace Lucy
{
    class UserInput
    {
        public event Action OnNewKnowledgeNeeded;
        public event Action OnUserRepetition;

        private string INPUT_SIGN = "> ";
        private string NEW_KNOWLEDGE_REQUESTED = "$new$";

        IO io;

        string response;
        string lastResponse;
        Expression quitExpr;

        public string Response { get { return response; } }

        public UserInput(Expression quitExpr)
        {
            this.quitExpr = quitExpr;
            this.response = string.Empty;
            this.lastResponse = string.Empty;
        }

        public UserInput(IO io, Expression quitExpr)
        {
            this.io = io;
            this.quitExpr = quitExpr;
            this.response = string.Empty;
            this.lastResponse = string.Empty;
        }

        public bool ChatContinues()
        {
            GetResponse();

            return !UserWantsToQuit();
        }

        public void GetResponse()
        {
            GetUserInput();
            TreatResponse();
        }

        public void GetResponse(string userInput)
        {
            response = userInput;
            TreatResponse();
        }

        private void GetUserInput()
        {
            WriteInputSign();
            ReadInput();
        }

        private void TreatResponse()
        {
            if (response != string.Empty)
                HandleInputProperly();
        }

        private void HandleInputProperly()
        {
            if (response != NEW_KNOWLEDGE_REQUESTED)
                HandleRegularUserInput();
            else
                RaiseNewKnowledgeNeeded();
        }

        private void HandleRegularUserInput()
        {
            CleanResponse();
            RaiseRepetitionIfRepeats();

            lastResponse = response;
        }

        private void WriteInputSign()
        {
            io.Write(INPUT_SIGN);
        }

        private void ReadInput()
        {
            response = io.ReadLine();
        }

        private bool UserWantsToQuit()
        {
            return UserWantsToQuit(quitExpr, response);
        }

        public static bool UserWantsToQuit(Expression quit, string userInput)
        {
            return quit.Keywords.Contains(userInput);
        }

        private void CleanResponse()
        {
            response = StringHelper.CleanString(response);
        }

        private void RaiseNewKnowledgeNeeded()
        {
            if (OnNewKnowledgeNeeded != null)
                OnNewKnowledgeNeeded();
        }

        private void RaiseRepetitionEvent()
        {
            if (OnUserRepetition != null)
                OnUserRepetition();
        }

        private bool UserRepeats()
        {
            return response == lastResponse;
        }

        public void RaiseRepetitionIfRepeats()
        {
            if (UserRepeats())
                RaiseRepetitionEvent();
        }
    }
}
