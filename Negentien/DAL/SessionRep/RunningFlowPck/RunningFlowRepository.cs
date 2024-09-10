using Microsoft.EntityFrameworkCore;
using NT.BL.Domain.flowpck;
using NT.DAL.EF;

namespace NT.DAL.SessionRep.RunningFlowPck;

public class RunningFlowRepository : IRunningFlowRepository
{
    private readonly PhygitalDbContext _phygitalDbContext;

    public RunningFlowRepository(PhygitalDbContext phygitalDbContext)
    {
        _phygitalDbContext = phygitalDbContext ?? throw new ArgumentNullException(nameof(phygitalDbContext));
    }
    public RunningFlow ReadRunningFlow(long flowId)
    {
        return _phygitalDbContext.RunningFlows
            .Find(flowId);
    }

    public RunningFlow ReadFlowByIdAndTesting(long flowId)
    {
        return _phygitalDbContext.RunningFlows
            .SingleOrDefault(wh => wh.CurrentFlow.Id == flowId && wh.IsForTesting);
    }

    public void CreateRunningFlow(RunningFlow runningFlow)
    {
        _phygitalDbContext.RunningFlows.Add(runningFlow);
        _phygitalDbContext.SaveChanges();
    }

    public IEnumerable<RunningFlow> ReadAllRunningFlowByCurrentFlowId(long currentFlowId)
    {
        return _phygitalDbContext.RunningFlows
            .Include(runFlow => runFlow.RunningFlowTime)
            .Include(runFlow => runFlow.CurrentFlow)
            .Where(runFlow => runFlow.CurrentFlow.Id == currentFlowId);
    }

    public void UpdateRunningFlow(RunningFlow runningFlow)
    {
        _phygitalDbContext.RunningFlows.Update(runningFlow);
        _phygitalDbContext.SaveChanges();
    }

    public IEnumerable<RunningFlow> ReadAllRunningFlow()
    {
        return _phygitalDbContext.RunningFlows
            .Include(runFlow => runFlow.CurrentFlow);
    }

    public RunningFlow ReadRunningFlowByIdWithCurrentFlow(long runningFlowId)
    {
        return _phygitalDbContext.RunningFlows
            .Include(rf => rf.CurrentFlow)
            .SingleOrDefault(rf => rf.Id == runningFlowId);
    }
    public IEnumerable<RunningFlow> ReadRunningFlowsByFlowId(long flowId)
    {
      return  _phygitalDbContext.RunningFlows
            .Include(rf => rf.CurrentFlow)
            .Where(rf => rf.CurrentFlow.Id == flowId);
    }

    public void DeleteRunningFlowByFlowIdAndTest(long flowId)
    {
        var runningFlow = _phygitalDbContext.RunningFlows
            .SingleOrDefault(wh => wh.CurrentFlow.Id == flowId && wh.IsForTesting);

        if (runningFlow != null)
        {
            _phygitalDbContext.RunningFlows.Remove(runningFlow);
        }
    }

    public void DeleteRunningFlows(IEnumerable<RunningFlow> runningFlowOfFlow)
    {
        _phygitalDbContext.RunningFlows.RemoveRange(runningFlowOfFlow);
    }
}