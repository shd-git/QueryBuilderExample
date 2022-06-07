using QueryBuilderFluent;


#region Fluent API usage

string query1 = new QueryBuilderFluent<Persons>()
    .Select(p => p.Id)
    .Select(p => p.LastName)
    .OrderBy(p => p.BirthDate)
    .Build(true);

#endregion

#region Safe Fluent API usage

string query2 = new QueryBuilder<Persons>()
    .From<Persons>()
    .Select(p => p.Id)
    .Select(p => p.LastName)
    .OrderBy(p => p.BirthDate)
    .Build(true);

string query3 = new QueryBuilder<Persons>()
    .From<Persons>()
    .OrderBy(p => p.BirthDate)
    .Build(true);

string query4 = new QueryBuilder<Persons>()
    .From<Persons>()
    .Build(true);

#endregion

Console.ReadKey();