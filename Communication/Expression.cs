using System;

namespace Lucy.Communication
{
    class Expression
    {
        public string[] Keywords { get; set; }
        public string[] Responses { get; set; }
        public string[] Contexts { get; set; }

        public ExpressionType Type { get; set; }
        public ExpressionLocation Location { get; set; }

        public Expression()
        {
            this.Keywords = new string[0];
            this.Responses = new string[0];
            this.Contexts = new string[0];
        }

        public Expression(string[] keywords, ExpressionLocation location, ExpressionType type,
            string[] responses, string[] contexts)
        {
            this.Type = type;
            this.Keywords = keywords;
            this.Location = location;
            this.Responses = responses;
            this.Contexts = contexts;
        }

        /// <summary>
        /// This doesn't return (string + Env.NewLine) !
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string keywords = Keywords.Length > 0 ? "\t" + GetKeywordsString() : string.Empty;
            string responses = Responses.Length > 0 ? "\t" + GetResponsesString() : string.Empty;
            string contexts = Contexts.Length > 0 ? "\t" + GetContextsString() : string.Empty;
            string type = Type == ExpressionType.Knowledge ? "\ttype:knowledge" : string.Empty;
            string location = GetLocationString();

            string[] lines = new string[] {
                "entry:",
                keywords,
                responses,
                contexts,
                type,
                location
            };
            lines = StringHelper.CleanLines(lines);

            return GetLine(lines);
        }

        private string GetLine(string[] lines)
        {
            string line = string.Empty;

            foreach (string linei in lines)
                line += linei + Environment.NewLine;
            line = line.Remove(line.Length - 2);

            return line;
        }

        private string GetKeywordsString()
        {
            string str = "keywords:";

            foreach (string keyword in Keywords)
                str += keyword + ",";
            str = str.Remove(str.Length - 1);

            return str;
        }

        private string GetResponsesString()
        {
            string str = "responses:";

            foreach (string response in Responses)
                str += response + ",";
            str = str.Remove(str.Length - 1);

            return str;
        }

        private string GetContextsString()
        {
            if (Contexts.Length > 0)
            {
                string str = "\tcontexts:";
                foreach (string context in Contexts)
                    str += context + ",";
                str = str.Remove(str.Length - 1);

                return str;
            }
            return string.Empty;
        }

        private string GetLocationString()
        {
            string loc = Location == ExpressionLocation.Alone
                            ? "alone"
                            : Location == ExpressionLocation.BeginOrMiddle
                                ? "begmid"
                                : Location == ExpressionLocation.MiddleOrEnd
                                    ? "midend"
                                    : "irr";
            loc = loc.Insert(0, "location:");
            return loc;
        }
    }
}
