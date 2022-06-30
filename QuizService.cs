namespace Atika.API
{
    public class QuizService
    {
        private readonly DataContext _db;
        private static readonly Dictionary<Guid, int> QuizCache = new();

        public QuizService(DataContext db)
        {
            _db = db;
        }

        public QuestionResponse GetQuestion()
        {
            var random = new Random();
            var first = random.Next(0, 99);
            var second = random.Next(0, 9);
            var opr = random.Next(0, 1) == 0 ? "+" : "-";

            var questionId = Guid.NewGuid();
            var answer = opr == "+" ? first + second : first - second;

            QuizCache.Add(questionId, answer);

            return new QuestionResponse(questionId, $"{first} {opr} {second}");
        }

        public AnswerResponse SubmitAnswer(AnswerRequest request)
        {
            var scoreboard = _db.Scoreboard.FirstOrDefault(x => x.PlayerName == request.PlayerName);
            if (scoreboard == null)
            {
                scoreboard = new Scoreboard
                {
                    PlayerName = request.PlayerName,
                    Score = 0
                };
                _db.Scoreboard.Add(scoreboard);
                _db.SaveChanges();
            }
            
            var exists = QuizCache.TryGetValue(request.QuestionId, out int answer);
            if (!exists)
                return new AnswerResponse(false, "Question not found.", scoreboard.Score);
            
            if (request.Answer == answer)
            {
                scoreboard.Score += 1;
                _db.SaveChanges();

                QuizCache.Remove(request.QuestionId);

                return new AnswerResponse(true, "Correct answer.", scoreboard.Score);
            }

            return new AnswerResponse(false, "Wrong answer.", scoreboard.Score);
        }

        public List<Scoreboard> GetScoreboards()
        {
            return _db.Scoreboard.OrderByDescending(x => x.Score).ThenBy(x => x.PlayerName).ToList();
        }
    }

    public record QuestionResponse(Guid QuestionId, string Question);

    public record AnswerRequest(Guid QuestionId, string PlayerName, int Answer);

    public record AnswerResponse(bool Success, string Message, int Score);
}
