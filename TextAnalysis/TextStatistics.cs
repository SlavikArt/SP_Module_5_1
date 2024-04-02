namespace TextAnalysis
{
    public class TextStatistics
    {
        public int Sentences { get; set; } = 0;
        public int Chars { get; set; } = 0;
        public int Words { get; set; } = 0;
        public int QuestionSentences { get; set; } = 0;
        public int ExclamationSentences { get; set; } = 0;
        public bool NewSentence { get; set; } = true;
        public bool NewWord { get; set; } = true;
    }
}
