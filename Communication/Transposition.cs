namespace Lucy.Communication
{
    class Transposition
    {
        string type;
        public string First { get; private set; }
        public string Second { get; private set; }

        public Transposition(string type, string first, string second)
        {
            this.type = type;

            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// This doesn't return (string + Env.NewLine) !
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return type + ":" + First + "," + Second;
        }
    }
}
