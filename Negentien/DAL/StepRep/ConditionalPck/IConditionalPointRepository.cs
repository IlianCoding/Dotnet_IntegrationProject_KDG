using NT.BL.Domain.flowpck;

namespace NT.DAL.StepRep.ConditionalPck;

public interface IConditionalPointRepository
{
    void CreateConditionalPoint(ConditionalPoint newConditionalPoint);
}