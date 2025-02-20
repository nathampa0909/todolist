using ToDoList.Application.Interfaces.Repository;
using ToDoList.Infraestructure.Context;

namespace ToDoList.Infraestructure.Repository
{
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{
		public AppDbContext dbContext;

		public BaseRepository(AppDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<TEntity> Add(TEntity objeto)
		{
			var savedEntity = await dbContext.Set<TEntity>().AddAsync(objeto);
			var contextoSalvo = await dbContext.SaveChangesAsync();

			if (contextoSalvo > 0)
			{
				return savedEntity.Entity;
			}

			throw new Exception($"Erro ao salvar objeto {objeto.GetType()} no banco.");
		}

		public async Task<bool> Delete(TEntity entidade)
		{
			dbContext.Set<TEntity>().Remove(entidade);
			var savedChanges = await dbContext.SaveChangesAsync();

			return savedChanges > 0 ? true : throw new Exception($"Erro ao deletar objeto {entidade.GetType()} no banco.");
		}

		public IQueryable<TEntity> GetAll()
		{
			var entities = dbContext.Set<TEntity>();
			return entities;
		}

		public async Task<TEntity> Update(TEntity entidade)
		{
			var addedEntity = dbContext.Set<TEntity>().Update(entidade);
			var savedChanges = dbContext.SaveChanges();

			if (savedChanges > 0)
			{
				return await Task.FromResult(addedEntity.Entity);
			}

			throw new Exception($"Erro ao atualizar objeto {entidade.GetType()} no banco");
		}
	}
}
