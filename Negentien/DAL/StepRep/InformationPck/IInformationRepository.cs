using NT.BL.Domain.flowpck;

namespace NT.DAL.StepRep.InformationPck;

public interface IInformationRepository
{
    Content CreateInformation(InformationContent newInformation);
}