namespace Drivia.Quizzes;

public class QuizAnswerDto
{
    public Guid QuestionId { get; set; }
    public bool UserAnswer { get; set; }
}