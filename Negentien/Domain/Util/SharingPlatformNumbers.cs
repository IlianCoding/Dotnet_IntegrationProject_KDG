using NT.BL.Domain.projectpck;

namespace NT.BL.Domain.Util;

public class SharingPlatformNumbers
{
    public int TotalProjects { get; set; }
    public int TotalFlows { get; set; }
    public double AvgParticipantsPerFlow { get; set; }
    public double AvgAmountOfStepsPerFlow { get; set; }
    public IEnumerable<ProjectNameAndId> ProjectData { get; set; }
    public IEnumerable<FlowIdAndName> FlowData { get; set; }
}