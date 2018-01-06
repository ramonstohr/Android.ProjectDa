using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Android1
{
    public static class Processor
    {
        private static string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        private static string filename = Path.Combine(path.ToString(), "miauz.txt");

        public static int GetCurrentQuestion()
        {
            int number = 0;
            if (!File.Exists(filename))
            {
                try
                {
                    using (var streamWriter = new StreamWriter(filename, false))
                    {
                        streamWriter.WriteLine(0);
                    }
                }
                catch (Exception)
                {
                }
            }

            using (var streamReader = new StreamReader(filename))
            {
                string numba = streamReader.ReadLine();
                number = Convert.ToInt32(numba);
            }

            return number;
        }

        public static void SaveQuestionNumber(int i)
        {
            try
            {
                using (var streamWriter = new StreamWriter(filename, false))
                {
                    streamWriter.WriteLine(i);
                }
            }
            catch (Exception)
            {
            }
        }

        public static List<Question> GetQuestions(Stream stream)
        {
            List<Question> questions = new List<Question>();

            using (XmlReader reader = XmlReader.Create(stream))
            {
                while (reader.ReadToFollowing("Frage"))
                {
                    Question q;
                    int nr = 0;
                    string text = "";
                    string answer = "";
                    string url = "";
                    if (reader.ReadToDescendant("Nummer"))
                        nr = Convert.ToInt32(reader.ReadInnerXml());
                    if (reader.ReadToNextSibling("Fragetext"))
                        text = reader.ReadInnerXml();
                    if (reader.ReadToNextSibling("Antwort"))
                        answer = reader.ReadInnerXml();
                    if (reader.ReadToNextSibling("URL"))
                        url = reader.ReadInnerXml();

                    q = new Question(nr, text, answer, url);
                    questions.Add(q);
                }
            }
            return questions;
        }
    }
}