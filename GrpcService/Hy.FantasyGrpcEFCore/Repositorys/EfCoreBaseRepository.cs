using Hy.FantasyGrpcEFCore.Common;
using Hy.FantasyGrpcEFCore.EFCore;
using Hy.FantasyGrpcEFCore.Entitys;
using Hy.FantasyGrpcEFCore.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hy.FantasyGrpcEFCore.Repositorys
{
    public class EfCoreBaseRepository<TEntity> : EfCoreBaseRepository<TEntity, int>, IRepository<TEntity> where TEntity : class, IEntity<int>
    {
        public EfCoreBaseRepository(FantasyDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class EfCoreBaseRepository<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly FantasyDbContext _dbContext;
        // private readonly IUnitOfWork _unitOfWork;

        public EfCoreBaseRepository(FantasyDbContext dbContext)
        {
            _dbContext = dbContext;
            // _unitOfWork = new UnitOfWork(dbContext);
        }

        #region 事务处理

        ///// <summary>
        ///// 事务对象
        ///// </summary>
        //public IDbContextTransaction DbContextTransaction { get; set; }

        ////public DbConnection dbConnection => _dbContext.Database.GetDbConnection();

        ////public virtual DbTransaction Transaction { get; set; }

        //public void BeginTransaction(IsolationLevel isolationLevel)
        //{
        //    //Transaction = Connection.BeginTransaction();
        //    Connection.BeginTransaction(isolationLevel);
        //}

        //public async Task BeginTransactionAsync(IsolationLevel isolationLevel)
        //{
        //    //dbConnection.Open();
        //    // Transaction = await Connection.BeginTransactionAsync();
        //    await Connection.BeginTransactionAsync();
        //}

        //public void CommitTransaction()
        //{
        //    Transaction.Commit();
        //}

        //public async Task CommitTransactionAsync()
        //{
        //    await Transaction.CommitAsync();
        //}

        //public void RollbackTransaction()
        //{
        //    Transaction.Rollback();
        //}

        //public async Task RollbackTransactionAsync()
        //{
        //    await Transaction.RollbackAsync();
        //}

        //public void DisposeTransaction()
        //{
        //    Transaction.Dispose();
        //}

        //public (bool Success, Exception ex) RunTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        //{
        //    bool success = true;
        //    Exception resEx = null;
        //    try
        //    {
        //        BeginTransaction(isolationLevel);

        //        action();

        //        CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        success = false;
        //        resEx = ex;
        //        RollbackTransaction();
        //    }
        //    finally
        //    {
        //        DisposeTransaction();
        //    }

        //    return (success, resEx);
        //}

        //public async Task<(bool Success, Exception ex)> RunTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        //{
        //    bool success = true;
        //    Exception resEx = null;
        //    try
        //    {
        //        await BeginTransactionAsync(isolationLevel);

        //        await action();

        //        CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        success = false;
        //        resEx = ex;
        //        RollbackTransaction();
        //    }
        //    finally
        //    {
        //        DisposeTransaction();
        //    }

        //    return (success, resEx);
        //}

        #endregion 事务处理

        #region 现在的

        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();

        public virtual DbSet<TEntity> DbQueryTable => _dbContext.Set<TEntity>();

        private static readonly ConcurrentDictionary<Type, bool> EntityIsDbQuery =
          new ConcurrentDictionary<Type, bool>();

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            if (EntityIsDbQuery.GetOrAdd(typeof(TEntity), key => _dbContext.GetType().GetProperties().Any(property =>
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbQuery<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                        typeof(IEntity<>)) &&
                    property.PropertyType.GetGenericArguments().Any(x => x == typeof(TEntity)))))
            {
                return DbQueryTable.AsQueryable();
            }

            return Table.AsQueryable();
        }

        public virtual DbTransaction Transaction
        {
            get
            {
                return _dbContext.Database.CurrentTransaction.GetDbTransaction();
            }
        }

        public virtual DbConnection Connection
        {
            get
            {
                var connection = _dbContext.Database.GetDbConnection();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
        }

        //public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        public override IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (propertySelectors == null && propertySelectors.Count() > 0)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(r => r.Id.Equals(id));
            // return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                _dbContext.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await _dbContext.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                _dbContext.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await _dbContext.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            entity = Update(entity);
            return Task.FromResult(entity);
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void BatchDelete(IEnumerable<TEntity> entities)
        {
            Table.RemoveRange(entities);
        }

        public override void BatchInsert(IEnumerable<TEntity> entities)
        {
            Table.AddRange(entities);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            //Could not found the entity, do nothing.
        }

        public override async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public override async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _dbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        public Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return _dbContext.Entry(entity).Collection(collectionExpression).LoadAsync(cancellationToken);
        }

        public void EnsureCollectionLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            _dbContext.Entry(entity).Collection(collectionExpression).Load();
        }

        public Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            return _dbContext.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        public void EnsurePropertyLoaded<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken)
            where TProperty : class
        {
            _dbContext.Entry(entity).Reference(propertyExpression).Load();
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = _dbContext.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        private static bool MayHaveTemporaryKey(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(byte))
            {
                return true;
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(entity.Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(entity.Id) <= 0;
            }

            return false;
        }

        #endregion 现在的

        #region 使用SQL

        public int ExecuteNonQuery(string strSql, params DbParameter[] dbParameters)
        {
            try
            {
                using (var conn = _dbContext.Database.GetDbConnection())
                {
                    conn.ConnectionString = "";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Clear();
                        if (dbParameters != null && dbParameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(dbParameters);
                        }

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return -1;
        }

        public async Task<int> ExecuteNonQueryAsync(string strSql, params DbParameter[] dbParameters)
        {
            // _dbContext.Database.ExecuteSqlRaw()
            try
            {
                using (var conn = _dbContext.Database.GetDbConnection())
                {
                    conn.ConnectionString = "";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = strSql;
                        cmd.Parameters.Clear();
                        if (dbParameters != null && dbParameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(dbParameters);
                        }

                        return await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return -1;
        }

        public DataTable GetDataTable(string strSql, params DbParameter[] dbParameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = _dbContext.Database.GetDbConnection())
                {
                    conn.ConnectionString = "";
                    using (var da = DbProviderFactories.GetFactory(conn).CreateDataAdapter())
                    {
                        da.SelectCommand.CommandText = strSql;
                        da.SelectCommand.Parameters.AddRange(dbParameters);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        #endregion 使用SQL

        #region 原来的

        //public override void Delete(TEntity entity)
        //{
        //    _dbContext.Set<TEntity>().Remove(entity);
        //    _dbContext.SaveChanges();
        //}

        //public override void Delete(TPrimaryKey id)
        //{
        //    var entity = _dbContext.Set<TEntity>().SingleOrDefault(t => t.Id.Equals(id));
        //    Delete(entity);
        //}

        //public override IQueryable<TEntity> GetAll()
        //{
        //    return _dbContext.Set<TEntity>().AsQueryable();
        //}

        //public override TEntity Insert(TEntity entity)
        //{
        //    _dbContext.Set<TEntity>().Add(entity);
        //    _dbContext.SaveChanges();
        //    return entity;
        //}

        //public override TEntity Update(TEntity entity)
        //{
        //    _dbContext.Set<TEntity>().Update(entity);
        //    _dbContext.SaveChanges();
        //    return entity;
        //}

        //public override void BatchDelete(IEnumerable<TEntity> entities)
        //{
        //    _dbContext.Set<TEntity>().RemoveRange(entities);
        //}

        //public override void BatchInsert(IEnumerable<TEntity> entities)
        //{
        //    _dbContext.Set<TEntity>().AddRange(entities);
        //}

        //public void BatchUpdate(IEnumerable<TEntity> entities)
        //{
        //    _dbContext.Set<TEntity>().UpdateRange(entities);
        //}

        #endregion 原来的
    }
}