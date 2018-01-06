namespace Android1
{
    public class Question
    {
        public int QuestionNr { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public string URL { get; set; }

        public Question(int q, string t, string a, string url)
        {
            QuestionNr = q;
            QuestionText = t;
            Answer = a;
            URL = url;
        }
    }
}