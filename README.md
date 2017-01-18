# GenerateTable
Easily generate a model file in C# based on a database table
# What it does
It connects to the database (in this case a DSN based on IBM's DB2) and gets the SCHEMA for the input table. In this case, it treats CHAR and VARCHAR as string and DECIMAL as decimal, because that's the way the project manager wanted it. 
You will need to adapt this to your own database, I'm only presenting it here because I was asked for it to be used as an inspiration. 
This will create, as well, a constructor with a full select clause, which in turn will fill and initialize all of the fields from the database, with an empty WHERE clause (maybe I'll go back in and give it the primary keys)
It's a very basic way of mapping, just in case you don't want to use any of the very good tools already out there.
In this case, the project being large, and the only thing needed was a basic model class, I was asked to write a class that could pull the model classes automatically instead of writing them by hand, and to do so in the shortest possible time. 
# NOTE
There are many improvements to be done here, I guess we can all agree that creating a .cs file by means of a StringBuilder with manual indents and with ifs checking for the types of variables is not the best way to go about doing this. 
