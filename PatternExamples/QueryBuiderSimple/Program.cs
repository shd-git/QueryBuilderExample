// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using System.Reflection;
using QueryBuilderSimple;

var builder = new QueryBuilder<Persons>();

string query = builder
    .From<Persons>()
    .Select(p => p.Id)
    .Select(p => p.LastName)
    .OrderBy(p => p.BirthDate)
    .Build(true);

builder = new QueryBuilder<Persons>();
builder
    .From<Persons>()
    .OrderBy(p => p.BirthDate)
    .Build(true);

builder = new QueryBuilder<Persons>();
builder
    .From<Persons>()
    .Build(true);

Console.ReadKey();