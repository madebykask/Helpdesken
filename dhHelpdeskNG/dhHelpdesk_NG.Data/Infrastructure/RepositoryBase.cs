// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the RepositoryBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Dal.DbContext;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Infrastructure
{
    /// <summary>
    ///     The repository base.
    /// </summary>
    /// <typeparam name="T">
    ///     entity type
    /// </typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        /// <summary>
        ///     The table.
        /// </summary>
        private readonly IDbSet<T> _dbset;

        /// <summary>
        ///     The initialize after commit actions.
        /// </summary>
        private readonly List<Action> _initializeAfterCommitActions;

        /// <summary>
        ///     The _data context.
        /// </summary>
        private HelpdeskDbContext _dataContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RepositoryBase{T}" /> class.
        /// </summary>
        /// <param name="databaseFactory">
        ///     The database factory.
        /// </param>
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
            _initializeAfterCommitActions = new List<Action>();
        }

        /// <summary>
        ///     Gets the database factory.
        /// </summary>
        protected IDatabaseFactory DatabaseFactory { get; }

        /// <summary>
        ///     Gets the data context.
        /// </summary>
        protected HelpdeskDbContext DataContext => _dataContext ?? (_dataContext = DatabaseFactory.Get());

        /// <summary>
        ///     Gets the table.
        /// </summary>
        protected IDbSet<T> Table => _dbset;

        /// <summary>
        ///     The commit.
        /// </summary>
        public void Commit()
        {
            try
            {
                DataContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    sb.AppendFormat("Property: {0}; Value: {1}; Error message: {2}",
                            validationError.PropertyName,
                            validationErrors.Entry.Property(validationError.PropertyName).CurrentValue,
                            validationError.ErrorMessage)
                        .AppendLine();

                    Trace.TraceInformation("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                }

                throw new Exception(sb.ToString(), ex);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            foreach (var initializeAfterCommit in _initializeAfterCommitActions)
            {
                initializeAfterCommit();
            }

            _initializeAfterCommitActions.Clear();
        }

        /// <summary>
        ///     The add.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
            _dataContext.Entry(entity).State = EntityState.Added;
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        /// <summary>
        ///     The add text.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        public virtual void AddText(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Detached;
            _dbset.Add(entity);
            _dataContext.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        public virtual void Delete(T entity)
        {
            _dbset.Attach(entity);
            _dbset.Remove(entity);
        }

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="where">
        ///     The where.
        /// </param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = _dbset.Where(where).AsEnumerable();
            foreach (var obj in objects)
            {
                _dbset.Attach(obj);
                _dbset.Remove(obj);
            }
        }

        /// <summary>
        ///     The get by id.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public virtual T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public virtual Task<T> GetByIdAsync(int id)
        {
            return ((DbSet<T>) _dbset).FindAsync(id);
        }

        /// <summary>
        ///     The get by id.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <returns>
        ///     The result.
        /// </returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset;
        }

        /// <summary>
        ///     The get many.
        /// </summary>
        /// <param name="where">
        ///     The where.
        /// </param>
        /// <param name="isAttached">
        /// false - AsNoTracking applied
        /// </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, bool isAttached = true)
        {
            return isAttached ? _dbset.Where(where) : _dbset.AsNoTracking().Where(where);
        }

        /// <summary>
        ///     The get.
        /// </summary>
        /// <param name="where">
        ///     The where.
        /// </param>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).AsNoTracking().FirstOrDefault();
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        ///     The get.
        /// </summary>
        /// <param name="where">
        ///     The where.
        /// </param>
        /// ///
        /// <param name="selector">
        ///     The selector.
        /// </param>
        /// <returns>
        ///     The <see cref="TResult" />.
        /// </returns>
        public virtual TResult Get<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector)
        {
            return _dbset.Where(where).AsNoTracking().Select(selector).FirstOrDefault();
        }

        /// <summary>
        ///     The initialize after commit.
        /// </summary>
        /// <param name="businessModel">
        ///     The business model.
        /// </param>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <typeparam name="T1">
        ///     first type
        /// </typeparam>
        /// <typeparam name="T2">
        ///     second type
        /// </typeparam>
        protected void InitializeAfterCommit<T1, T2>(T1 businessModel, T2 entity)
            where T1 : INewBusinessModel
            where T2 : Entity
        {
            var initializeAfterCommit = new Action(() => businessModel.Id = entity.Id);
            _initializeAfterCommitActions.Add(initializeAfterCommit);
        }

        /// <summary>
        ///     Applies new musltiselect list selection to an existing selection from repository - removes what needs to be
        ///     removed, add what needs to be added
        /// </summary>
        /// <param name="currentPredicate">Predicate for existing repository to select a set of entries to be updated</param>
        /// <param name="newList">New selection, list of entries to what a repository needs to be updated to</param>
        /// <param name="comparePredicate">Comparer expression used to compare old and new entries</param>
        /// <param name="updater">Function to update if existing matching records from new list</param>
        /// <param name="skipDelete">If true, do not remove missing items</param>
        /// <returns></returns>
        public void MergeList(Expression<Func<T, bool>> currentPredicate, IList<T> newList,
            Func<T, T, bool> comparePredicate, Action<T, T> updater = null, bool skipDelete = false)
        {
            var current = Table.Where(currentPredicate).ToList();

            if (!skipDelete)
            {
                foreach (var toDel in current.Where(x => !newList.Any(y => comparePredicate(x, y))).ToList())
                {
                    Delete(toDel);
                }
            }
            
            foreach (var toIns in newList.Where(x => !current.Any(y => comparePredicate(x, y))).ToList())
            {
                Add(toIns);
            }

            if (updater != null)
            {
                for (var i = 0; i < current.ToList().Count; i++)
                {
                    var newRow = newList.FirstOrDefault(x => comparePredicate(current[i], x));
                    if (newRow != null)
                        updater(current[i], newRow);
                }
            }
        }
    }
} 