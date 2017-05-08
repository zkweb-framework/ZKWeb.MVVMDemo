# 数据的增删查改

这份Demo使用了工作单元和仓储模式，工作单元负责管理数据库上下文的生命周期，并且随着Api请求建立和销毁

### **工作单元的建立**

``` csharp
var uow = ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
using (uow.Scope())
{
    // 在这里的操作都会在同一个数据库连接中
}
```

因为工作单元随着Api请求自动建立，上面的代码在一般开发中是不会使用到的，但是有一些情况需要手动管理工作单元，例如在后台线程或定时任务中操作数据库时会需要调用上面的代码

### **添加或更新数据**

已前一篇的`ExampleData`为例，在领域服务中可以使用`Repository`成员保存数据

``` csharp
[ExportMany, SingletonReuse]
public class ExampleDataManager : DomainServiceBase<ExampleData, Guid>
{

    public void AddExample()
    {
        var entity = new ExampleData() { Name = "a", Description = "b" };
        Repository.Save(ref entity);
    }
}
```

这个实体中的部分成员会自动分配
- 主键，如果主键是int或者Guid会自动分配，Guid会分配一个有序的Guid值(具体实现可以看GuidEntityFilter)
- 创建时间(具体实现可以看CreateTimeFilter)
- 更新时间(具体实现可以看UpdateTimeFilter)
- 租户(具体实现可以看OwnerTenantFilter)

添加和更新都使用Save函数，底层会自动检测是用insert还是update命令

如果需要同时添加其他实体的数据可以注入其他实体的仓储，如下

``` csharp
[ExportMany, SingletonReuse]
public class ExampleDataManager : DomainServiceBase<ExampleData, Guid>
{
    private IRepository<User, Guid> _userRepository;

    public ExampleDataManager(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public void AddExample()
    {
        var entity = new ExampleData() { Name = "a", Description = "b" };
        Repository.Save(ref entity);
        var user = _userRepository.Get(u => u.Username == "admin");
        if (user != null)
            _userRepository.Save(ref user, u => u.Remark += "added example");
    }
}
```

### **查询数据**

查询数据可以使用`Repository.Get`或者`Repository.Query`，代码如下

``` csharp
public void QueryExample()
{
    var a = Repository.Get(x => x.Name == "a");
    var b = Repository.Query().Where(x => x.Name.StartsWith("b"));
    // 数据库中保存的时间应该统一为utc时间
    var weeksAgo = DateTime.UtcNow.AddDays(-3);
    var count = Repository.Count(x => x.CreateTime >= weeksAgo);
}
```

注意在这个Demo中数据库中保存的时间都是utc时间，在传给客户端时会自动转换为客户端指定时区的时间

### **删除数据**

删除数据可以使用`Repository.Delete`或者`Repository.Delete

``` csharp
public void DeleteExample()
{
    var a = Repository.Get(x => x.Name == "a");
    Repository.Delete(a);
    var deletedCount = Repository.BatchDelete(x => x.Name.StartsWith("b"));
}
```

### **事务**

Demo默认不开启事务，事务可以通过在Api函数上标记`[UnitOfWork(IsTransactional = true)]`或者使用以下的代码手动开启

``` csharp
public void TransactinExample()
{
    using (UnitOfWork.Scope())
    {
        UnitOfWork.Context.BeginTransaction();
        var entity = new ExampleData() { Name = "a", Description = "b" };
        Repository.Save(ref entity);
        UnitOfWork.Context.FinishTransaction();
    }
}
```

事务可以嵌套开启，如果发生了嵌套会统一使用最外层的事务，如果在调用FinishTransaction之前发生了例外则事务不会被提交

### **数据过滤器**

数据过滤器用于全局过滤数据的查询和修改，Demo自带了以下的过滤器

- CreateTimeFilter: 自动设置创建时间
- UpdateTimeFilter: 自动设置更新时间
- DeletedFilter: 自动过滤假删除的数据，Demo未实现界面上的假删除
- GuidEntityFilter: 自动设置有序的Guid主键
- OwnerTenantFilter: 自动设置租户和按租户过滤数据
- OwnerFilter: 自动设置所属人和按所属人过滤，Demo中未用到

数据过滤器可以通过继承以下的接口实现

``` csharp
/// <summary>
/// 查询过滤器
/// </summary>
public interface IEntityQueryFilter
{
    /// <summary>
    /// 过滤查询
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    IQueryable<TEntity> FilterQuery<TEntity, TPrimaryKey>(
        IQueryable<TEntity> query)
        where TEntity : class, IEntity<TPrimaryKey>;

    /// <summary>
    /// 过滤查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <param name="predicate">查询条件</param>
    /// <returns></returns>
    Expression<Func<TEntity, bool>> FilterPredicate<TEntity, TPrimaryKey>(
        Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity<TPrimaryKey>;
}
```

``` csharp
/// <summary>
/// 操作过滤器
/// </summary>
public interface IEntityOperationFilter
{
    /// <summary>
    /// 过滤保存
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <param name="entity">实体对象</param>
    void FilterSave<TEntity, TPrimaryKey>(TEntity entity)
        where TEntity : class, IEntity<TPrimaryKey>;

    /// <summary>
    /// 过滤删除
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <param name="entity">实体对象</param>
    void FilterDelete<TEntity, TPrimaryKey>(TEntity entity)
        where TEntity : class, IEntity<TPrimaryKey>;
}
```

实现过滤器后，如果标记了`[ExportMany]`则会自动全局启用该过滤器，如果不标记则需要手动启用

手动启用过滤器可以使用以下的代码

``` csharp
public void FilterExample()
{
    using (UnitOfWork.Scope())
    using (UnitOfWork.EnableQueryFilter(new OwnerFilter()))
    {
        var entity = new ExampleData() { Name = "a", Description = "b" };
        Repository.Save(ref entity);
    }
}
```

同样的可以手动禁止过滤器

``` csharp
using (UnitOfWork.Scope())
using (UnitOfWork.DisableQueryFilter(typeof(OwnerFilter)))
{
    var entity = new ExampleData() { Name = "a", Description = "b" };
    Repository.Save(ref entity);
}
```

注意过滤器只影响通过仓储的操作，如果直接使用数据库上下文(Context)操作数据，则不会受到这些过滤器的影响，
并且过滤器只适用于保存的数据本身，不适用于外键的关联数据

需要更多信息可以参考ZKWeb的文档

- [数据库](http://zkweb-framework.github.io/cn/site/core/database/index.html)
- [仓储和工作单元](http://zkweb-framework.github.io/cn/site/plugin/common.base.repository_uow/index.html)
  - 这篇文档是给ZKWeb.Plugin(MPA版本)写的，但是大部分内容适用于这个Demo
