using NT.BL.Domain.flowpck;
using NT.BL.Domain.projectpck;
using NT.BL.Domain.sessionpck;

namespace NT.UI.MVC.Models.UserViewModels;

public class AttendantFlowDetailViewModel
{
    public Flow Flow { get; set; }
    public IEnumerable<Step> Steps { get; set; }
    public IEnumerable<Content> Contents { get; set; }
    public IEnumerable<RunningFlow> OpenRunningFlows { get; set; }
    public IEnumerable<RunningFlow> ClosedRunningFlows { get; set; }
    public IEnumerable<Note> Notes { get; set; }
    public Project Project { get; set; }
    public int StepsCount { get; set; }
}