using System.Linq.Expressions;
using System.Text;

namespace QueryBuilderSimple
{
    public class QueryBuilder<TTable> 
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

        public void From<T>()
        {
            _buildContext[TableKey] = typeof(T);
        }

        public void Select<TPropertyType>(Expression<Func<TTable, TPropertyType>> selector)
        {
            var list = _buildContext[SelectorsKey] as List<Expression>;

            list.Add(selector);
        }

        public void OrderBy<TPropertyType>(Expression<Func<TTable, TPropertyType>> selector)
        {
            var list = _buildContext[OrderKey] as List<Expression>;

            list.Add(selector);
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

            #region Implementation

            void ProcessSelectors()
            {
                var expressions = _buildContext[SelectorsKey] as IReadOnlyCollection<Expression>;
                
                var columnNames = !expressions.Any() 
                    ? "* " 
                    : string.Join(", ", expressions.Select(GetPropertyName));

                // Another example of Builder pattern 
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

            #endregion
        }

        #region Private methods

        private string GetPropertyName(Expression expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            var memberExpression = lambdaExpression.Body as MemberExpression;

            return memberExpression.Member.Name;
        }

        #endregion
    }
}
