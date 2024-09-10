using NT.BL.Domain.flowpck;
using NT.DAL.EF;

namespace NT.DAL.StepRep.ConditionalPck;

public class ConditionalPointRepository : IConditionalPointRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public ConditionalPointRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    
    public void CreateConditionalPoint(ConditionalPoint conditionalPoint)
    {
        _phygitalDbContext.ConditionalPoints.Add(conditionalPoint);
        _phygitalDbContext.SaveChanges();
    }
}