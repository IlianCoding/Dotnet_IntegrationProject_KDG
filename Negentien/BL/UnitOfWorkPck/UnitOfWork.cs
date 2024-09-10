using NT.DAL.EF;

namespace NT.BL.UnitOfWorkPck;

public class UnitOfWork
{
    private readonly PhygitalDbContext _dbContext;

    public UnitOfWork(PhygitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

/*
 * BeginTransaction() moet in de controller worden aangeroepen
 * wannneer je een transactie wil starten.
 */
    public void BeginTransaction()
    {
        _dbContext.Database.BeginTransaction();
    }

/*
 * Commit() moet in de controller worden aangeroepen wannneer
 * je een transactie wil eindigen, zodat de wijzigingen in
 * de database worden doorgevoerd.
 */
    public void Commit()
    {
        _dbContext.SaveChanges();
        _dbContext.Database.CommitTransaction();
    }
}