# pip3 install neo4j
# python3 example.py

from neo4j import GraphDatabase, basic_auth

cypher_query = '''
MATCH (l:Location {address:$address})<-[r:OCCURRED_AT]-(c:Crime)
RETURN c.date as crimeDate
'''

with GraphDatabase.driver(
    "neo4j://<HOST>:<BOLTPORT>",
    auth=("<USERNAME>", "<PASSWORD>")
) as driver:
    result = driver.execute_query(
        cypher_query,
        address="Piccadilly",
        database_="neo4j")
    for record in result.records:
        print(record['crimeDate'])
