using EdaSample.Common;
using EdaSample.Common.DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Linq;

namespace EdaSample.DataAccess.Dapper
{
    public class DapperDataAccessObject : IDataAccessObject
    {
        private readonly string connStr;

        private bool disposedValue = false;
        public DapperDataAccessObject(string connStr)
        {
            this.connStr = connStr;
        }
        public int AddRange<TEntity>(List<TEntity> models, string sql) where TEntity:IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                var tran = conn.BeginTransaction(IsolationLevel.Serializable);
                int i = conn.Execute(sql, models, tran);
                tran.Commit();
                return i;
            }

        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> AddAsync<TEntity>(TEntity model, string sql) where TEntity : IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                return await conn.ExecuteAsync(sql, model);
            }
        }

        // DapperExtensions.DapperExtensions .Insert(conn,model,null,null);



        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(TEntity model, string sql) where TEntity : IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                return await conn.ExecuteAsync(sql, model);
            }

        }



        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateRange<TEntity>(List<TEntity> models, string sql) where TEntity : IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                var tran = conn.BeginTransaction(IsolationLevel.Serializable);
                int i = conn.Execute(sql, models, tran);
                tran.Commit();
                return i;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(TEntity model, string sql) where TEntity : IEntity
        {
            using (var connection = new SqlConnection(connStr))
            {
                int i = await connection.ExecuteAsync(sql, model);
                return i;
            }


        }

        public async Task<int> DeleteListAsync<TEntity>(List<TEntity> models, string sql) where TEntity : IEntity
        {
            using (var connection = new SqlConnection(connStr))
            {
                var tran = connection.BeginTransaction(IsolationLevel.Serializable);
                int i = connection.Execute(sql, models, tran);
                await Task.Run(() => tran.Commit());
                return i;

            }

        }

        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql)
        {
            using (var conn = new SqlConnection(connStr))
            {
                return await conn.ExecuteAsync(sql);

            }
        }






        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<TEntity> GetModelAsync<TEntity>(string sql) where TEntity : IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                var t = await conn.QueryFirstAsync<TEntity>(sql);
                //conn.Close();
                //conn.Dispose();
                return t;
            }

        }



        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetModelLstAsync<TEntity>(string sql) where TEntity : IEntity
        {
            // return await Task.Run(() => conn.Query<TEntity>(sql).ToList());
            using (var conn = new SqlConnection(connStr))
            {
                List<TEntity> ts = await Task.Run(() => conn.Query<TEntity>(sql).ToList());
                return ts;
            }
        }
        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetModelLstAsync<TEntity>(string sql, object param) where TEntity : IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                List<TEntity> ts = await Task.Run(() => conn.Query<TEntity>(sql, param).ToList());
                return ts;
            }


        }
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async  Task<TEntity> GetByIdAsync<TEntity>(string sql, object param) where TEntity : IEntity
        {
            using (var conn = new SqlConnection(connStr))
            {
                var ts = await Task.Run(() => conn.Query<TEntity>(sql, param).FirstOrDefault());
                return ts;
            }
        }
        
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 暂不能使用
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : IEntity
        {
            throw new NotImplementedException();
        }
    }
}
