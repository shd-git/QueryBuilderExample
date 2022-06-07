using System.Linq.Expressions;

namespace QueryBuilderFluent;

public interface IQueryBuilder
{
    IQueryBuilderSelect<TTable> From<TTable>();
}
public interface IQueryBuilderSelect<TTable> : IQueryBuilderOrder<TTable>, IQueryBuilderBuild
{
    IQueryBuilderSelect<TTable> Select<TPropertyType>(Expression<Func<TTable, TPropertyType>> selector);
}

public interface IQueryBuilderOrder<TTable> : IQueryBuilderBuild
{
    IQueryBuilderBuild OrderBy<TPropertyType>(Expression<Func<TTable, TPropertyType>> selector);
}

public interface IQueryBuilderBuild
{
    string Build(bool isShouldPrint);
}