using Drivia.Data;
using Drivia.Entities;
using Drivia.Quizzes;
using Microsoft.EntityFrameworkCore;

namespace Drivia.Services;

public class QuizService(AppDbContext context)
{
    public StartQuizResponse StartQuiz(StartQuizRequest request)
    {
        // 1. Select questions (by type)
        IQueryable<Question> query = context.Questions;

        if (request.Type == "chapter" && request.ChapterIds != null && request.ChapterIds.Any())
        {
            query = query.Where(q => request.ChapterIds.Contains(q.ChapterId));
        }
        // Add adaptivity here if needed for "adaptive" type

        var questionsList = query
            .OrderBy(x => Guid.NewGuid()) // simple randomization
            .Take(request.NumberOfQuestions)
            .ToList();

        // 2. Create QuizSession
        var quizSession = new QuizSession
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            StartedAt = DateTime.UtcNow,
            Type = request.Type,
            QuizQuestions = questionsList.Select(q => new QuizQuestion
            {
                Id = Guid.NewGuid(),
                QuestionId = q.Id
            }).ToList()
        };
        context.QuizSessions.Add(quizSession);
        context.SaveChanges();

        // 3. Prepare response
        var questionDtos = questionsList.Select(q => new QuizQuestionDto
        {
            QuestionId = q.Id,
            Text = q.Text,
            GroupTitle = q.GroupTitle,
            ImageUrl = q.ImageUrl
        }).ToList();

        return new StartQuizResponse
        {
            QuizSessionId = quizSession.Id,
            Questions = questionDtos
        };
    }

    public void SubmitQuiz(SubmitQuizRequest request)
    {
        var session = context.QuizSessions
            .Include(s => s.QuizQuestions)
            .ThenInclude(qq => qq.Question)
            .FirstOrDefault(s => s.Id == request.QuizSessionId);

        if (session == null) throw new Exception("Quiz session not found.");

        foreach (var ans in request.Answers)
        {
            var qq = session.QuizQuestions.FirstOrDefault(q => q.QuestionId == ans.QuestionId);
            if (qq != null)
            {
                qq.UserAnswer = ans.UserAnswer;
                qq.IsCorrect = qq.Question.CorrectAnswer == ans.UserAnswer;
            }
        }
        session.EndedAt = DateTime.UtcNow;

        // Optionally calculate score and save in session (add Score property if you want)
        // session.Score = session.QuizQuestions.Count(q => q.IsCorrect == true);

        context.SaveChanges();
    }

    public QuizSessionDto? GetQuizSession(Guid quizSessionId)
    {
        var session = context.QuizSessions
            .Include(qs => qs.QuizQuestions)
            .ThenInclude(qq => qq.Question)
            .FirstOrDefault(s => s.Id == quizSessionId);

        if (session == null) return null;

        var questions = session.QuizQuestions.Select(qq => new QuizQuestionDto
        {
            QuestionId = qq.QuestionId,
            Text = qq.Question.Text,
            GroupTitle = qq.Question.GroupTitle,
            ImageUrl = qq.Question.ImageUrl,
            UserAnswer = qq.UserAnswer
        }).ToList();

        var score = session.QuizQuestions.Count(q => q.IsCorrect == true);

        return new QuizSessionDto
        {
            QuizSessionId = session.Id,
            StartedAt = session.StartedAt,
            EndedAt = session.EndedAt,
            Type = session.Type ?? "chapter",
            Questions = questions,
            Score = score
        };
    }
}
