﻿module ExcelTypeProviderTSQL

open FSharp.Interop.Excel

open System
open System.Data
open FsToolkit.ErrorHandling

open System.Data.SqlClient

open Helpers

[<Struct>]
type private Builder2 = Builder2 with    
    member _.Bind((optionExpr, errDuCase), nextFunc) =
        match optionExpr with
        | Some value -> nextFunc value 
        | _          -> errDuCase  
    member _.Return x : 'a = x

let private pyramidOfDoom = Builder2

//a type provider ensures type safety at compile time
type private DataTypesTest = ExcelFile<"e:\\source\\repos\\OracleDB_Excel_Files\\Slovnicek AJ new.xlsx", "AJ-CJ-AJ", HasHeaders = false>
//kdyz se daji headers, vezme to pouze sloupce s headers

let internal insertOrUpdateDictionaryTSQL getConnectionTSQL closeConnectionTSQL query path list =

    try
        let connection: SqlConnection = getConnectionTSQL() 
        
        try  
            //new = an instance of a type, not a class in the traditional object-oriented programming sense
            let file = new DataTypesTest(path, "AJ-CJ-AJ") // F# type, hopefully no nulls
            let rows = file.Data |> Seq.toArray 
            let epRowsCount = rows.Length     
            let listRange = [ 0 .. epRowsCount - 1 ] 

            use cmdDropSequence = new SqlCommand(List.item 0 query, connection)
            use cmdDeleteAll = new SqlCommand(List.item 1 query, connection)
            use cmdCreateSequence = new SqlCommand(List.item 2 query, connection)
            use cmdInsert = new SqlCommand(List.item 3 query, connection)
            use cmdUpdate1 = new SqlCommand(List.item 4 query, connection)
            use cmdUpdate2 = new SqlCommand(List.item 5 query, connection)
             
            printfn "drop seq %i" <| cmdDropSequence.ExecuteNonQuery() // -1
            
            printfn "del all %i" <| cmdDeleteAll.ExecuteNonQuery() //number of affected rows
            
            printfn "create seq %i" <| cmdCreateSequence.ExecuteNonQuery() // -1                         
            
            listRange
            |> List.iter
                (fun i ->
                        cmdInsert.Parameters.Clear() // Clear parameters for each iteration                                                
                        cmdInsert.Parameters.AddWithValue("@English", (rows |> Array.item i).Column3) |> ignore
                        cmdInsert.Parameters.AddWithValue("@Czech", (rows |> Array.item i).Column5) |> ignore
                        cmdInsert.Parameters.AddWithValue("@Note", (rows |> Array.item i).Column13) |> ignore

                        cmdInsert.ExecuteNonQuery() |> ignore //number of affected rows
                )
         
            //number of affected rows
            printfn "update1 %i" <| cmdUpdate1.ExecuteNonQuery() //Dynamic SQL needed for stored procedure with the table name and primary key column as parameters

            //-1
            printfn "update2 %i" 
            <|
            (
                let x = 
                    cmdUpdate2.Parameters.Clear() // Clear parameters for each iteration                                                
                    cmdUpdate2.Parameters.AddWithValue("@table_name", List.item 0 list) |> ignore
                    cmdUpdate2.Parameters.AddWithValue("@primary_key_column", List.item 1 list) |> ignore

                    cmdUpdate2.ExecuteNonQuery() 
                x
            )               
            
        finally
            closeConnectionTSQL connection
    with
    | ex ->
          printfn "Error1 %s" ex.Message

    //Dynamic SQL is achieved using the EXECUTE IMMEDIATE statement. 
    //The use of bind variables with the USING clause helps prevent SQL injection and provides a way to pass values into the dynamic SQL statement.
    //Using dynamic SQL in the DELETE_NULL_ROWS procedure allows you to pass the table name and primary key column as parameters, 
    //While dynamic SQL (building SQL statements dynamically) is sometimes necessary, avoid it when a static query suffices. This helps reduce the risk of introducing vulnerabilities.