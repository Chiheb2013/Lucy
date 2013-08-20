using Lucy.Communication;

namespace Lucy.DatabaseHandling
{
    class DatabaseHandler
    {
        string path;
        
        Database dataBase;

        public bool BaseChanged { get { return dataBase.Changed; } }

        public ExpressionBase Expressions { get { return dataBase.Expressions; } }

        public Transposition[] Knowledge { get { return dataBase.Knowledge; } }
        public Transposition[] Transpositions { get { return dataBase.Transpositions; } }

        public DatabaseHandler(string dataBasePath)
        {
            this.path = dataBasePath;

            this.dataBase = new Database();
        }

        public void Load()
        {
            DatabaseLoader loader = new DatabaseLoader(path);
            dataBase = loader.Load();
        }

        public void Save()
        {
            DatabaseSaver saver = new DatabaseSaver(dataBase, path);
            saver.Save();
        }

        public void AddKnowledge(string title, string content)
        {
            dataBase.AddKnowledge(title, content);
        }

        public void AddExpression(string[] keywords, string[] responses, string[] contexts,
            ExpressionType type, ExpressionLocation location)
        {
            dataBase.AddExpression(keywords, responses, contexts, type, location);
        }

        public void AddTransposition(string from, string to)
        {
            dataBase.AddTransposition(from, to);
        }
    }
}
