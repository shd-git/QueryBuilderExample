using System.Linq.Expressions;
using System.Text;

namespace QueryBuilderFluent
{
    public class QueryBuilder<TTable> : IQueryBuilder, IQueryBuilderSelect<TTable>
    {
        private const string TableKey = nameof(TableKey);
        private const string SelectorsKey = nameof(SelectorsKey);
        private const string OrderKey = nameof(OrderKey);

        private readonly Dictionary<string, object> _buildContext;

        public QueryBuilder()
        {
            _buildContext = new()
            {
                [SelectorsKey] = new List<Expression>(),
                [OrderKey] = new List<Expression>(),
            };
        }

        public IQueryBuilderSelect<TTable> Select<TPropertyType>(Expression<Func<TTable, TPropertyType>> selector)
        {
            var list = _buildContext[SelectorsKey] as List<Expression>;

            list.Add(selector);

            return this;
        }

        public IQueryBuilderBuild OrderBy<TPropertyType>(Expression<Func<TTable, TPropertyType>> selector)
        {
            var list = _buildContext[OrderKey] as List<Expression>;

            list.Add(selector);

            return this;
        }

        public string Build(bool isShouldPrint)
        {
            var stringBuilder = new StringBuilder();

            ProcessSelectors();

            ProcessFrom();

            ProcessOrder();

            var query = stringBuilder.ToString();

            if (isShouldPrint)
                Console.WriteLine(query);

            return query;

            void ProcessSelectors()
            {
                var expressions = _buildContext[SelectorsKey] as IReadOnlyCollection<Expression>;
                
                var columnNames = !expressions.Any() 
                    ? "* " 
                    : string.Join(", ", expressions.Select(GetPropertyName));

                stringBuilder
                    .Append("Select ")
                    .AppendLine(columnNames);
            }

            void ProcessFrom()
            {
                var type = _buildContext[TableKey] as Type;

                stringBuilder
                    .Append("From ")
                    .AppendLine(type.Name);
            }

            void ProcessOrder()
            {
                var expressions = _buildContext[OrderKey] as IReadOnlyCollection<Expression>;

                if (expressions.Any())
                {
                    var columnNames = string.Join(", ", expressions.Select(GetPropertyName));

                    stringBuilder
                        .Append("Order by ")
                        .Append(columnNames);
                }
            }
        }

        public IQueryBuilderSelect<TTable> From<TTable>()
        {
            _buildContext[TableKey] = typeof(TTable);
            return (IQueryBuilderSelect<TTable>) this;
        }

        private string GetPropertyName(Expression expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            var memberExpression = lambdaExpression.Body as MemberExpression;

            return memberExpression.Member.Name;
        }
    }
}
