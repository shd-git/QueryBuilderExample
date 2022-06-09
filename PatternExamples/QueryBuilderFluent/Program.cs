using QueryBuilderFluent;


#region Fluent API usage

string query1 = new QueryBuilderFluent<Persons>()
    .Select(p => p.Id)
    .Select(p => p.LastName)
    .OrderBy(p => p.BirthDate)
    .Build(true);

#endregion

#region Safe Fluent API usage

string query2 = QueryBuilder
    .From<Persons>()
    .Select(p => p.Id)
    .Select(p => p.LastName)
    .OrderBy(p => p.BirthDate)
    .Build(true);

#endregion

Console.ReadKey();