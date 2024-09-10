namespace NT.BL.Domain.Util;

public class RunningFlowsPerFlow
{
    public string FlowName { get; set; }
    public IEnumerable<RunningFlowsPerFlowPerMonth> RunningFlowsPerFlowPerMonth { get; set; }
}