using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.Admin.Models
{
    public class AnswerViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Vui lòng chọn câu hỏi.")]
        public int QuestionID { get; set; }

        public List<AnswerInput> Answers { get; set; }

        public AnswerViewModel()
        {
            Answers = new List<AnswerInput>
            {
                new AnswerInput(),
                new AnswerInput(),
                new AnswerInput()
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int nonEmptyAnswers = Answers.Count(a => !string.IsNullOrWhiteSpace(a.AnswerText));
            if (nonEmptyAnswers < 1)
            {
                yield return new ValidationResult("Phải nhập ít nhất một câu trả lời.", new[] { nameof(Answers) });
            }

            // Ensure at least one answer is marked as correct if there are non-empty answers
            if (nonEmptyAnswers > 0 && !Answers.Any(a => a.IsCorrect && !string.IsNullOrWhiteSpace(a.AnswerText)))
            {
                yield return new ValidationResult("Phải có ít nhất một câu trả lời đúng.", new[] { nameof(Answers) });
            }
        }
    }

    public class AnswerInput
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}