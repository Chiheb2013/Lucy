using System;
using System.IO;

using Lucy.Knowledge;
using Lucy.DatabaseHandling;

namespace Lucy
{
    public class Lucy
    {
        const string GREETINGS = "HELLO !";
        const string USER_REPEATED = "STOP REPEATING YOURSELF.";
        const string LUCY_DIDNT_UNDERSTAND = "I DIDN'T UNDERSTAND WHAT YOU SAID.";

        public event Action LucyStopped;
        public event Action<string> LucyAnswered;

        IO io;

        UserInput input;
        LucyResponder responder;

        string databasePath;
        Database database;

        /// <summary>
        /// Creates a new instance of Lucy that is IO-free (fit for anything, but can't learn from the user).
        /// </summary>
        /// <param name="databasePath">The path to the database file.</param>
        public Lucy(string databasePath)
        {
            this.databasePath = databasePath;

            LoadDatabase();
            CreateUserInput();
            CreateResponder();
        }

        /// <summary>
        /// Creates a new instance of Lucy depending heavily on IO capabilities (fit for Console Applications).
        /// </summary>
        /// <param name="databasePath">The path to the database file.</param>
        /// <param name="inStream">The 'in' stream, from where Lucy will read user input.</param>
        /// <param name="outStream">The 'out' stream where Lucy will write her responses.</param>
        public Lucy(string databasePath, Stream inStream, Stream outStream)
        {
            this.databasePath = databasePath;

            LoadDatabase();
            CreateIO(inStream, outStream);
            CreateUserInput();
            CreateResponder();
        }

        /// <summary>
        /// Starts a driven-by-Lucy chat session depending heavily on IO.
        /// </summary>
        public void StartDrivenChatSession()
        {
            RaiseOutputEvent(GREETINGS);
            DoDrivenChat();
            SaveDatabaseIfChanges();
        }

        /// <summary>
        /// Responds to an user input asking or affirming something.
        /// RespondTo() raises LucyAnswered event if a response was found.
        /// </summary>
        /// <remarks>Doesn't support $new$, so if it meets it RespondTo() will just return;</remarks>
        /// <param name="userInput">The input from the user, asking or affirming something.</param>
        public void RespondTo(string userInput)
        {
            if (userInput.StartsWith("$new$"))
                AddNewKnowledgeFromString(userInput);
            else if (UserWantsToQuit(StringHelper.CleanString(userInput)))
                RaiseStopEvent();
            else
                Respond(userInput);
        }

        private void AddNewKnowledgeFromString(string userInput)
        {
            NonIOKnowledgeAdder nonIoAdder = new NonIOKnowledgeAdder(database);
            nonIoAdder.AddFromString(userInput);
        }

        private bool UserWantsToQuit(string userInput)
        {
            return UserInput.UserWantsToQuit(database.Expressions[0], userInput);
        }

        private void Respond(string userInput)
        {
            input.GetResponse(userInput);

            GetResponse(input.Response);
            RaiseCorrectResponseEvent();
        }

        private void CreateIO(Stream inStream, Stream outStream)
        {
            io = new IO(inStream, outStream);
        }

        private void LoadDatabase()
        {
            DatabaseLoader loader = new DatabaseLoader(databasePath);
            database = loader.Load();
        }

        private void CreateUserInput()
        {
            if (io != null)
                CreateIOUserInput();
            else
                CreateNonIOUserInput();

            input.OnNewKnowledgeNeeded += input_OnNewKnowledgeNeeded;
            input.OnUserRepetition += input_OnUserRepetition;
        }

        private void CreateIOUserInput()
        {
            input = new UserInput(io, database.GetQuitExpression());
        }

        private void CreateNonIOUserInput()
        {
            input = new UserInput(database.GetQuitExpression());
        }

        private void CreateResponder()
        {
            responder = new LucyResponder(database);
        }

        private void DoDrivenChat()
        {
            while (input.ChatContinues())
            {
                GetResponse();
                RaiseCorrectResponseEvent();
            }
            RaiseStopEvent();
        }

        private void GetResponse()
        {
            GetResponse(input.Response);
        }

        private void GetResponse(string userInput)
        {
            responder.GetResponse(userInput);
        }

        private void SaveDatabaseIfChanges()
        {
            if (database.Changed)
                SaveDatabase();
        }

        private void input_OnUserRepetition()
        {
            RaiseOutputEvent(USER_REPEATED);
        }

        private void input_OnNewKnowledgeNeeded()
        {
            IOKnowledgeAdder adder = new IOKnowledgeAdder(io, database);
            adder.AddNewData();
        }

        private void SaveDatabase()
        {
            DatabaseSaver saver = new DatabaseSaver(database, databasePath);
            saver.Save();
        }

        private void RaiseCorrectResponseEvent()
        {
            if (responder.UnderstandsUserInput())
                RaiseOutputEvent(responder.Response);
            else
                RaiseOutputEvent(LUCY_DIDNT_UNDERSTAND);
        }

        private void RaiseOutputEvent(string response)
        {
            if (LucyAnswered != null)
                LucyAnswered(response);
        }

        private void RaiseStopEvent()
        {
            SaveDatabase();

            if (LucyStopped != null)
                LucyStopped();
        }
    }
}
