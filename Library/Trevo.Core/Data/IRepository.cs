using System.Collections.Generic;
using System.Linq;

namespace Trevo.Core.Data
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : BaseEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
             where TEntity : BaseEntity, new();
        void ExecuteStoredProcedure(string commandText, params object[] parameters);
        int ExecuteSqlCommand(string commandText, params object[] parameters);
        IEnumerable<TEnity> SqlQuery<TEnity>(string commandText, params object[] parameters);
        void ExecuteStoredProcedureForOutParams(string commandText,out List<object> pa, params object[] parameters);
    }
}
