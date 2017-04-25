using System;
using System.Collections.Generic;
using System.FastReflection;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ZKWeb.Database;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Application.Dtos;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Filters.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Services.Interfaces;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Extensions;
using ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Domain.Uow.Interfaces;
using ZKWebStandard.Extensions;

namespace ZKWeb.MVVMPlugins.MVVM.Common.Base.src.Components.GridSearchResponseBuilder {
	/// <summary>
	/// 表格搜索回应的构建器
	/// </summary>
	/// <typeparam name="TEntity">实体类型</typeparam>
	/// <typeparam name="TPrimaryKey">主键类型</typeparam>
	public class GridSearchResponseBuilder<TEntity, TPrimaryKey>
		where TEntity : class, IEntity, IEntity<TPrimaryKey> {
		protected GridSearchRequestDto _request;
		protected IList<IEntityQueryFilter> _enableFilters;
		protected IList<Type> _disableFilters;
		protected IList<Expression<Func<TEntity, bool>>> _keywordConditions;
		protected IList<Func<IQueryable<TEntity>, IQueryable<TEntity>>> _queryFilters;
		protected Func<IQueryable<TEntity>, IQueryable<TEntity>> _querySorter;

		public GridSearchResponseBuilder(GridSearchRequestDto request) {
			_request = request;
			_enableFilters = new List<IEntityQueryFilter>();
			_disableFilters = new List<Type>();
			_keywordConditions = new List<Expression<Func<TEntity, bool>>>();
			_queryFilters = new List<Func<IQueryable<TEntity>, IQueryable<TEntity>>>();
			_querySorter = q => q;
		}

		/// <summary>
		/// 启用查询过滤器
		/// </summary>
		public GridSearchResponseBuilder<TEntity, TPrimaryKey>
			EnableQueryFilter(IEntityQueryFilter filter) {
			_enableFilters.Add(filter);
			return this;
		}

		/// <summary>
		/// 禁用查询过滤器
		/// </summary>
		public GridSearchResponseBuilder<TEntity, TPrimaryKey>
			DisableQueryFilter(Type type) {
			_disableFilters.Add(type);
			return this;
		}

		/// <summary>
		/// 指定过滤关键词时使用的成员
		/// </summary>
		public GridSearchResponseBuilder<TEntity, TPrimaryKey>
			FilterKeywordWith<TMember>(Expression<Func<TEntity, TMember>> memberExpr) {
			var paramExpr = memberExpr.Parameters[0];
			var bodyExpr = memberExpr.Body;
			Expression newBodyExpr;
			if (typeof(TMember) == typeof(string)) {
				newBodyExpr = Expression.Call(
					bodyExpr,
					typeof(string).FastGetMethod(nameof(string.Contains)),
					Expression.Constant(_request.Keyword));
			} else {
				newBodyExpr = Expression.Equal(
					bodyExpr,
					Expression.Constant(_request.Keyword));
			}
			var conditionExpr = Expression.Lambda<Func<TEntity, bool>>(newBodyExpr, paramExpr);
			return FilterKeywordWithCondition(conditionExpr);
		}

		/// <summary>
		/// 指定过滤关键词时使用的条件
		/// </summary>
		public GridSearchResponseBuilder<TEntity, TPrimaryKey>
			FilterKeywordWithCondition(Expression<Func<TEntity, bool>> conditionExpr) {
			_keywordConditions.Add(conditionExpr);
			return this;
		}

		/// <summary>
		/// 指定自定义的查询过滤函数
		/// </summary>
		public GridSearchResponseBuilder<TEntity, TPrimaryKey>
			FilterQuery(Func<IQueryable<TEntity>, IQueryable<TEntity>> filterFunc) {
			_queryFilters.Add(filterFunc);
			return this;
		}

		/// <summary>
		/// 获取可在执行过程中应用过滤器设置的函数执行器
		/// </summary>
		/// <returns></returns>
		protected Func<Func<GridSearchResponseDto>, GridSearchResponseDto> GetScopeInvoker() {
			var uow = ZKWeb.Application.Ioc.Resolve<IUnitOfWork>();
			var scopeInvoker = new Func<Func<GridSearchResponseDto>, GridSearchResponseDto>(func => func());
			// 应用启用的过滤器
			foreach (var enableFilter in _enableFilters) {
				var oldScopeInvoker = scopeInvoker;
				scopeInvoker = func => {
					using (uow.EnableQueryFilter(enableFilter)) {
						return oldScopeInvoker(func);
					}
				};
			}
			// 应用禁用的过滤器
			foreach (var disableFilter in _disableFilters) {
				var oldScopeInvoker = scopeInvoker;
				scopeInvoker = func => {
					using (uow.DisableQueryFilter(disableFilter)) {
						return oldScopeInvoker(func);
					}
				};
			}
			// 开启uow，以防万一不是在应用服务中调用
			return func => {
				using (uow.Scope()) {
					return scopeInvoker(func);
				}
			};
		}

		/// <summary>
		/// http://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool
		/// </summary>
		protected class ReplaceExpressionVisitor : ExpressionVisitor {
			private readonly Expression _oldValue;
			private readonly Expression _newValue;

			public ReplaceExpressionVisitor(Expression oldValue, Expression newValue) {
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override Expression Visit(Expression node) {
				if (node == _oldValue)
					return _newValue;
				return base.Visit(node);
			}
		}

		/// <summary>
		/// 按关键词进行过滤
		/// </summary>
		protected virtual IQueryable<TEntity> ApplyKeywordFilter(IQueryable<TEntity> query) {
			// 无关键字或无过滤条件时跳过
			if (string.IsNullOrEmpty(_request.Keyword) || _keywordConditions.Count == 0) {
				return query;
			}
			// 使用or合并所有表达式
			var firstCondition = _keywordConditions.First();
			var leftParamExpr = firstCondition.Parameters[0];
			var leftBodyExpr = firstCondition.Body;
			foreach (var condition in _keywordConditions.Skip(1)) {
				var rightParamExpr = condition.Parameters[0];
				var visitor = new ReplaceExpressionVisitor(rightParamExpr, leftParamExpr);
				var rightBodyExpr = visitor.Visit(condition.Body);
				leftBodyExpr = Expression.OrElse(leftBodyExpr, rightBodyExpr);
			}
			var predicate = Expression.Lambda<Func<TEntity, bool>>(leftBodyExpr, leftParamExpr);
			return query.Where(predicate);
		}

		/// <summary>
		/// 过滤时使用的函数
		/// </summary>
		protected static MethodInfo _stringContainsMethod =
			typeof(string).FastGetMethod(nameof(string.Contains));
		protected static MethodInfo _stringStartsWithMethod =
			typeof(string).FastGetMethod(nameof(string.StartsWith));
		protected static MethodInfo _stringEndsWithMethod =
			typeof(string).FastGetMethod(nameof(string.EndsWith));

		/// <summary>
		/// 按列查询条件进行过滤
		/// </summary>
		protected virtual IQueryable<TEntity> ApplyColumnFilter(IQueryable<TEntity> query) {
			// 无列查询条件时跳过
			if (_request.ColumnFilters == null || _request.ColumnFilters.Count == 0) {
				return query;
			}
			// 按列查询条件进行过滤
			var entityType = typeof(TEntity);
			foreach (var columnFilter in _request.ColumnFilters) {
				// 获取属性，时间或枚举列可能以Text结尾需要去掉再试
				var filterProperty = entityType.FastGetProperty(columnFilter.Column);
				if (filterProperty == null && columnFilter.Column.EndsWith("Text")) {
					filterProperty = entityType.FastGetProperty(
						columnFilter.Column.Substring(0, columnFilter.Column.Length - 4));
				}
				if (filterProperty == null) {
					continue;
				}
				var propertyType = Nullable.GetUnderlyingType(filterProperty.PropertyType);
				var isNullable = propertyType != filterProperty.PropertyType;
				// 构建过滤表达式
				var paramExpr = Expression.Parameter(entityType, "e");
				var memberExpr = Expression.Property(paramExpr, filterProperty);
				var memberValueExpr = isNullable ?
					Expression.Property(memberExpr, nameof(Nullable<int>.Value)) : memberExpr;
				Expression bodyExpr;
				if (propertyType == typeof(string)) {
					// 成员类型是字符串
					var valueExpr = Expression.Constant(columnFilter.Value?.ToString());
					if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith) {
						// 以搜索值开始
						bodyExpr = Expression.Call(memberExpr, _stringStartsWithMethod, valueExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith) {
						// 以搜索值结尾
						bodyExpr = Expression.Call(memberExpr, _stringEndsWithMethod, valueExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.Equals) {
						// 等于搜索值
						bodyExpr = Expression.Equal(memberExpr, valueExpr);
					} else {
						// 包含搜索值
						bodyExpr = Expression.Call(memberExpr, _stringContainsMethod, valueExpr);
					}
				} else if (propertyType == typeof(DateTime)) {
					// 成员类型是时间
					// 首先从传入值解析时间，解析失败时跳过
					// 传入值可以是时间，也可以是{ start: 时间, end: 时间 }
					DateTime startTime, endTime;
					if (columnFilter.Value == null) {
						continue;
					} else if (columnFilter.Value is string) {
						if (!DateTime.TryParse((string)columnFilter.Value, out startTime)) {
							continue;
						}
						endTime = startTime;
					} else if (columnFilter.Value is DateTime) {
						endTime = startTime = (DateTime)columnFilter.Value;
					} else {
						var dict = columnFilter.Value.ConvertOrDefault<IDictionary<string, object>>();
						if (dict == null) {
							continue;
						}
						startTime = dict.GetOrDefault<DateTime>("start");
						endTime = dict.GetOrDefault<DateTime>("end");
					}
					// 转换时区
					startTime = startTime.FromClientTime();
					endTime = endTime.FromClientTime().AddDays(1);
					// 生成表达式
					var startTimeExpr = Expression.Constant(startTime);
					var endTimeExpr = Expression.Constant(endTime);
					if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith) {
						// 以搜索时间开始
						bodyExpr = Expression.GreaterThanOrEqual(memberValueExpr, startTimeExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith) {
						// 以搜索时间结尾
						bodyExpr = Expression.LessThan(memberValueExpr, endTimeExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.Equals) {
						// 等于搜索时间(1天内)
						bodyExpr = Expression.AndAlso(
							Expression.GreaterThanOrEqual(memberValueExpr, startTimeExpr),
							Expression.LessThan(memberValueExpr, Expression.Constant(startTime.AddDays(1))));
					} else {
						// 包含在搜索时间范围内
						bodyExpr = Expression.AndAlso(
							Expression.GreaterThanOrEqual(memberValueExpr, startTimeExpr),
							Expression.LessThan(memberValueExpr, endTimeExpr));
					}
				} else if (propertyType == typeof(sbyte) || propertyType == typeof(byte) ||
					propertyType == typeof(short) || propertyType == typeof(ushort) ||
					propertyType == typeof(int) || propertyType == typeof(uint) ||
					propertyType == typeof(long) || propertyType == typeof(ulong) ||
					propertyType == typeof(decimal)) {
					// 成员类型是数值
					// 首先从传入值解析数值，解析失败时跳过
					// 传入值可以是数值，也可以是"数值~数值"，也可以是{ start: 数值, end: 数值 }
					object lower, upper;
					if (columnFilter.Value == null) {
						continue;
					}
					lower = columnFilter.Value.ConvertOrDefault(propertyType, null);
					if (lower != null) {
						upper = lower;
					} else if (columnFilter.Value?.ToString().Contains("~") ?? false) {
						var range = columnFilter.Value.ToString().Split('~');
						lower = range[0].ConvertOrDefault(propertyType, null);
						upper = range[1].ConvertOrDefault(propertyType, null);
					} else {
						var dict = columnFilter.Value.ConvertOrDefault<IDictionary<string, object>>();
						if (dict == null) {
							continue;
						}
						lower = dict.GetOrDefault("start").ConvertOrDefault(propertyType, null);
						upper = dict.GetOrDefault("end").ConvertOrDefault(propertyType, null);
					}
					if (lower == null || upper == null) {
						continue;
					}
					// 生成表达式
					var lowerExpr = Expression.Constant(lower);
					var upperExpr = Expression.Constant(upper);
					if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith) {
						// 以搜索数值开始
						bodyExpr = Expression.GreaterThanOrEqual(memberValueExpr, lowerExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith) {
						// 以搜索数值结尾
						bodyExpr = Expression.LessThanOrEqual(memberValueExpr, upperExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.Equals) {
						// 等于搜索数值
						bodyExpr = Expression.Equal(memberValueExpr, lowerExpr);
					} else {
						// 包含在搜索时间范围内
						bodyExpr = Expression.AndAlso(
							Expression.GreaterThanOrEqual(memberValueExpr, lowerExpr),
							Expression.LessThanOrEqual(memberValueExpr, upperExpr));
					}
				} else {
					// 成员类型是其他
					var value = columnFilter.Value.ConvertOrDefault(propertyType, null);
					if (value == null) {
						continue;
					}
					var valueExpr = Expression.Constant(value);
					if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.StartsWith) {
						// 以搜索值开始
						bodyExpr = Expression.GreaterThanOrEqual(memberValueExpr, valueExpr);
					} else if (columnFilter.MatchMode == GridSearchColumnFilterMatchMode.EndsWith) {
						// 以搜索值结尾
						bodyExpr = Expression.LessThanOrEqual(memberValueExpr, valueExpr);
					} else {
						// 等于搜索值
						bodyExpr = Expression.Equal(memberValueExpr, valueExpr);
					}
				}
				// 如果类型是nullable还要再加一层过滤避免出错
				if (isNullable) {
					bodyExpr = Expression.AndAlso(
						Expression.NotEqual(memberExpr, Expression.Constant(null)), bodyExpr);
				}
				// 根据构建出来的条件筛选
				var predicate = Expression.Lambda<Func<TEntity, bool>>(bodyExpr, paramExpr);
				query = query.Where(predicate);
			}
			return query;
		}


		/// <summary>
		/// 调用自定义的过滤函数
		/// </summary>
		protected virtual IQueryable<TEntity> ApplyCustomFilter(IQueryable<TEntity> query) {
			// 无自定义过滤函数时跳过
			if (_queryFilters == null || _queryFilters.Count == 0) {
				return query;
			}
			// 调用自定义的过滤函数
			foreach (var filter in _queryFilters) {
				query = filter(query);
			}
			return query;
		}

		/// <summary>
		/// 排序的泛型函数
		/// https://github.com/aspnet/EntityFramework/issues/6735
		/// </summary>
		protected IQueryable<TEntity> SortQueryGeneric<TProperty>(
			IQueryable<TEntity> query, PropertyInfo orderProperty, bool ascending) {
			var orderExprParam = Expression.Parameter(typeof(TEntity), "e");
			var orderExprMember = Expression.Property(orderExprParam, orderProperty);
			var orderExpr = Expression.Lambda<Func<TEntity, TProperty>>(orderExprMember, orderExprParam);
			if (ascending) {
				query = query.OrderBy(orderExpr);
			} else {
				query = query.OrderByDescending(orderExpr);
			}
			return query;
		}
		protected static readonly MethodInfo _sortQueryGenericMethod =
			typeof(GridSearchResponseBuilder<TEntity, TPrimaryKey>).FastGetMethod(
				nameof(SortQueryGeneric), BindingFlags.NonPublic | BindingFlags.Instance);

		/// <summary>
		/// 按自定义的排序函数或者请求的排序字段进行排序
		/// </summary>
		protected virtual IQueryable<TEntity> ApplySorter(IQueryable<TEntity> query) {
			// 有自定义的排序函数时使用自定义的排序函数
			if (_querySorter != null) {
				return _querySorter(query);
			}
			// 按查询给出的列进行排序，如果列不存在则按Id进行排序
			var entityType = typeof(TEntity);
			PropertyInfo orderProperty = null;
			if (!string.IsNullOrEmpty(_request.OrderBy)) {
				orderProperty = entityType.FastGetProperty(_request.OrderBy);
				if (orderProperty == null) {
					entityType.FastGetProperty(_request.OrderBy + "Text");
				}
			}
			if (orderProperty == null) {
				orderProperty = entityType.FastGetProperty(nameof(IEntity<object>.Id));
			}
			query = (IQueryable<TEntity>)_sortQueryGenericMethod
				.MakeGenericMethod(typeof(TEntity), orderProperty.PropertyType)
				.FastInvoke(this, query, orderProperty, _request.Ascending);
			return query;
		}

		/// <summary>
		/// 创建搜索回应
		/// </summary>
		/// <returns></returns>
		public virtual GridSearchResponseDto ToResponse() {
			return GetScopeInvoker()(() => {
				// 获取领域服务
				var domainService = ZKWeb.Application.Ioc.Resolve<IDomainService<TEntity, TPrimaryKey>>();
				// 获取查询对象
				return domainService.GetMany<GridSearchResponseDto>(query => {
					// 按关键词进行过滤
					query = ApplyKeywordFilter(query);
					// 按列查询条件进行过滤
					query = ApplyColumnFilter(query);
					// 调用自定义的过滤函数
					query = ApplyCustomFilter(query);
					// 按自定义的排序函数或者请求的排序字段进行排序
					query = ApplySorter(query);
					// 构建搜索回应
					var count = query.LongCount();
					var records = query.ToList<object>();
					return new GridSearchResponseDto(count, records);
				});
			});
		}
	}
}
