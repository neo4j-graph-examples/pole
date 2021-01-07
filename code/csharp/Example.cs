// install dotnet core on your system
// dotnet new console -o .
// dotnet add package Neo4j.Driver
// paste in this code into Program.cs
// dotnet run

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;
  
namespace dotnet {
  class Example {
  static async Task Main() {
    var driver = GraphDatabase.Driver("bolt://<HOST>:<BOLTPORT>", 
                    AuthTokens.Basic("<USERNAME>", "<PASSWORD>"));

    var cypherQuery =
      @"
      MATCH (l:Location {address:$address})<-[r:OCCURRED_AT]-(c:Crime) 
      RETURN c.date as crimeDate
      ";

    var session = driver.AsyncSession(o => o.WithDatabase("neo4j"));
    var result = await session.ReadTransactionAsync(async tx => {
      var r = await tx.RunAsync(cypherQuery, 
              new { address="Piccadilly"});
      return await r.ToListAsync();
    });

    await session?.CloseAsync();
    foreach (var row in result)
      Console.WriteLine(row["crimeDate"].As<string>());
	  
    }
  }
}