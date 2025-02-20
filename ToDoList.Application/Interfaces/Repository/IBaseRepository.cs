namespace ToDoList.Application.Interfaces.Repository;

public interface IBaseRepository<TEntity> where TEntity : class
{
	Task<TEntity> Add(TEntity entity);
	Task<bool> Delete(TEntity entity);
	IQueryable<TEntity> GetAll();
	Task<TEntity> Update(TEntity entity);
}
