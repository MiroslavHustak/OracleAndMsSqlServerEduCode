﻿module ITVFsTSQL //buzzword FUNCTION, RETURN, strucna charakteristika: vraci virtual table (result set)

open System
open System.Data.SqlClient

open Helpers

(*
Inline Table-Valued Functions should always return a result set in T-SQL.

MS SQL SERVER MANAGEMENT STUDIO

USE [XlxsToDb.ContextClass];

DROP FUNCTION IF EXISTS dbo.MyFunction;
GO

CREATE FUNCTION dbo.MyFunction
(@cid AS INT) RETURNS TABLE
AS
RETURN
	SELECT
		id,
		[name],
		salary,
		departmentId   
	FROM
		Employee1
	WHERE salary > 70000;

-- en example of an ITVF based on my CTE
CREATE FUNCTION dbo.GetOperatorsByJobTitle(@jobTitle NVARCHAR(50))
RETURNS TABLE
AS
RETURN (
    SELECT OperatorID, FirstName, LastName, JobTitle
    FROM Operators
    WHERE JobTitle = @jobTitle
);

-- Using the ITVF
SELECT *
FROM dbo.GetOperatorsByJobTitle('Valcíř');

*)

let private queryCreateITVF =  
    //jen zkusebni function, a jo, vytvorilo se to do dane db (connection string stacil)
    //a samo to poznalo scalar function a skoncilo to v danem adresari
    "   
    CREATE FUNCTION dbo.GetOperatorsByJobTitle(@jobTitle NVARCHAR(50))
    RETURNS TABLE
    AS
    RETURN (
        SELECT OperatorID, FirstName, LastName, JobTitle
        FROM Operators
        WHERE JobTitle = @jobTitle
    );
    " 

let internal createITVF getConnectionTSQL closeConnectionTSQL =
    
    try
        let connection: SqlConnection = getConnectionTSQL()
                 
        try   
            use cmdCreateFunction = new SqlCommand(queryCreateITVF, connection)                            
                         
            cmdCreateFunction.ExecuteNonQuery()  |> ignore 
          
        finally
            closeConnectionTSQL connection
    with
    | ex -> printfn "%s" ex.Message

let internal callITVF getConnectionTSQL closeConnectionTSQL =
    
    try
        let connection: SqlConnection = getConnectionTSQL()
                 
        try  
            use cmdCallFunction = new SqlCommand("SELECT * FROM dbo.GetOperatorsByJobTitle(@jobTitle)", connection)
           
            cmdCallFunction.Parameters.AddWithValue("@jobTitle", "Valcíř") |> ignore
            
            let reader = cmdCallFunction.ExecuteReader() 

            let getValues =                                                 
                Seq.initInfinite (fun _ -> reader.Read() && reader.HasRows = true)
                |> Seq.takeWhile ((=) true) 
                |> Seq.collect
                    (fun _ ->
                            //V pripade pouziti Oracle zkontroluj skutecny typ sloupce v .NET   

                            //Jen pro overeni 
                            let columnType = reader.GetFieldType(reader.GetOrdinal("OperatorID"))
                            printfn "Column Type: %s" columnType.Name
                                                         
                            seq 
                                {                                       
                                    Casting.castAs<int> reader.["OperatorID"] 
                                    |> Option.bind (fun item -> Option.filter (fun item -> not (item.Equals(String.Empty))) (Some (string item))) 
                                    Casting.castAs<string> reader.["FirstName"]                                                                               
                                    Casting.castAs<string> reader.["LastName"]
                                    Casting.castAs<string> reader.["JobTitle"]   
                                } 
                    ) |> List.ofSeq             

            printfn "Result from ITVF: %A" getValues
          
        finally
            closeConnectionTSQL connection
    with
    | ex -> printfn "%s" ex.Message

