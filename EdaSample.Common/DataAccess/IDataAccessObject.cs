using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EdaSample.Common.DataAccess
{
    /// <summary>
    /// Represents that the implemented classes are data access objects that perform
    /// CRUD operations on the given entity type.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataAccessObject : IDisposable
    {
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity">对象类型</typeparam>
        /// <param name="lstModel">对象集合</param>
        /// <param name="sql">插入语句模版</param>
        /// <returns></returns>
        int AddRange<TEntity>(List<TEntity> models, string sql)
            where TEntity : IEntity;


        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> AddAsync<TEntity>(TEntity model, string sql) where TEntity : IEntity;
        // DapperExtensions.DapperExtensions .Insert(conn,model,null,null);



        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> UpdateAsync<TEntity>(TEntity model, string sql) where TEntity : IEntity;


        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="models"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        int UpdateRange<TEntity>(List<TEntity> models, string sql) where TEntity : IEntity;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<TEntity>(TEntity model, string sql) where TEntity : IEntity;
        Task<int> DeleteListAsync<TEntity>(List<TEntity> models, string sql) where TEntity : IEntity;

        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql);

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<TEntity> GetModelAsync<TEntity>(string sql) where TEntity : IEntity;



        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetModelLstAsync<TEntity>(string sql) where TEntity : IEntity;
        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetModelLstAsync<TEntity>(string sql, object param) where TEntity : IEntity;
    }
}
