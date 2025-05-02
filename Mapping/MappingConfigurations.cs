using Survey_Basket.Contracts.Answer;
using Survey_Basket.Contracts.Question;

namespace Survey_Basket.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, src => src.Answers.Select(a => new Answer { Content = a }));

        config.NewConfig<Question, QuestionResponse>()
            .Map(dest => dest.Answers, src => src.Answers.Select(a => new AnswerResponse(a.Id, a.Content)));
    }
}
