// See https://aka.ms/new-console-template for more information

using QueryBuilderSimple;

var builder = new QueryBuilder<Persons>();

builder.From<Persons>();
builder.Select(p => p.Id);
builder.Select(p => p.LastName);

builder.OrderBy(p => p.BirthDate);

builder.Build(true);


Console.ReadKey();