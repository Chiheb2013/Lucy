using System.IO;

namespace Lucy
{
    class IO
    {
        StreamReader reader;
        StreamWriter writer;

        public IO(Stream inStream, Stream outStream)
        {
            this.reader = new StreamReader(inStream);
            this.writer = new StreamWriter(outStream);
        }

        ~IO()
        {
            reader.Close();
            writer.Close();
        }

        public string ReadLine()
        {
            return reader.ReadLine();
        }

        public void Write(string str)
        {
            writer.Write(str);
            writer.Flush();
        }

        public void WriteLine(string str)
        {
            writer.WriteLine(str);
            writer.Flush();
        }
    }
}
