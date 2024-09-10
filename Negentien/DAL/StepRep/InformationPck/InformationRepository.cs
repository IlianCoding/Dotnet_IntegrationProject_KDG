using NT.BL.Domain.flowpck;
using NT.DAL.EF;

namespace NT.DAL.StepRep.InformationPck;

public class InformationRepository : IInformationRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public InformationRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }


    public Content CreateInformation(InformationContent newInformation)
    {
        _phygitalDbContext.Content.Add(newInformation);
        _phygitalDbContext.SaveChanges();
        return ReadInformation(newInformation.Id);
    }

    public Content ReadInformation(long newInformationId)
    {
        return _phygitalDbContext.Content.Find(newInformationId);
    }
}