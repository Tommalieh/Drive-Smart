namespace DriveSmart.Shared.Quizzes;

public class SubmitQuizRequest
{
    public Guid QuizSessionId { get; set; }
    public List<QuizAnswerDto> Answers { get; set; }
}