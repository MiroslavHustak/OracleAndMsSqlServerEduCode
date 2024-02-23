open DML
open DML_T_SQL

open System
open System.Data.SqlClient
//<PackageReference Include="System.Data.SqlClient" Version="4.8.6" /> musel jsem rucne dodat do fsproj, nevim, cemu nebyla knihovna automaticky aplikovana. Hmmmm....
open Oracle.ManagedDataAccess.Client

open CTEs
open Triggers
open DerivedTables
open ScalarFunctions
open WindowFunctions
open StoredProcedures

open Queries
open SubQueries

open CTEsTSQL
open ITVFsTSQL
open ViewsTSQL
open TriggersTSQL
open DerivedTablesTSQL
open WindowFunctionsTSQL
open ScalarFunctionsTSQL
open StoredProceduresTSQL

open QueriesTSQL
open SubQueriesTSQL

module Program = 

    //vse musi byt v try-with bloku

    [<Literal>]
    let private connectionString = 
        //"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.0.0.2)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));User Id=Test_User;Password=Test_User;"
        "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.0.0.2)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));User Id=Dictionary;Password=Dictionary;"
    
    let private getConnection () =
        let connection = new OracleConnection(connectionString)  
        connection.Open()
        connection
        
    let private closeConnection (connection: OracleConnection) =
        connection.Close()
        connection.Dispose()

    //localhost
    let [<Literal>] private connStringTSQL = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Test_User_MSSQLS;Integrated Security=True"

    //shall be in a tryWith block
    let private getConnectionTSQL () =        
        let connection = new SqlConnection(connStringTSQL)
        connection.Open()
        connection
    
    let private closeConnectionTSQL (connection: SqlConnection) =
        connection.Close()
        connection.Dispose()
    
    //uncomment what is needed

    //insertOrUpdateProductionOrder getConnection closeConnection |> ignore

    //insertOrUpdateProducts getConnection closeConnection |> ignore

    //insertOperators getConnection closeConnection |> ignore

    //insertMachines getConnection closeConnection |> ignore

    //createStoredProcedure getConnection closeConnection |> ignore
    
    //createTrigger getConnection closeConnection |> ignore

    //createFunction getConnection closeConnection |> ignore

    //selectValuesCTE getConnection closeConnection |> ignore

    //selectValuesWF getConnection closeConnection |> ignore

    //selectValuesDT getConnection closeConnection |> ignore

    //printfn "%A" <| querySteelStructures getConnection closeConnection 
    //printfn "%A" <| queryWelds getConnection closeConnection  
    //printfn "%A" <| queryBlastFurnaces getConnection closeConnection  
    //printfn "%A" <| selectValues4Lines getConnection closeConnection  
    
    //insertProducts getConnectionTSQL closeConnectionTSQL |> ignore
    //insertOperators getConnectionTSQL closeConnectionTSQL |> ignore
    //insertMachines getConnectionTSQL closeConnectionTSQL |> ignore
    //insertProductionOrder getConnectionTSQL closeConnectionTSQL |> ignore
    //updateProductionOrder getConnectionTSQL closeConnectionTSQL 

    //selectValuesDT getConnectionTSQL closeConnectionTSQL |> ignore
    //selectValuesWFTSQL getConnectionTSQL closeConnectionTSQL |> ignore
    //selectValuesCTETSQL getConnectionTSQL closeConnectionTSQL |> ignore
    //createScalarFunctionTSQL getConnectionTSQL closeConnectionTSQL |> ignore
    //callScalarFunctionTSQL getConnectionTSQL closeConnectionTSQL |> ignore
    //createITVF getConnectionTSQL closeConnectionTSQL |> ignore
    //callITVF getConnectionTSQL closeConnectionTSQL |> ignore
    //callView getConnectionTSQL closeConnectionTSQL |> ignore
    //executeStoredProcedure getConnectionTSQL closeConnectionTSQL |> ignore 
    //createStoredProcedure getConnectionTSQL closeConnectionTSQL |> ignore 
    //createTriggerTSQL getConnectionTSQL closeConnectionTSQL |> ignore 

    printfn "%A" <| querySteelStructuresTSQL getConnectionTSQL closeConnectionTSQL 
    printfn "%A" <| queryWeldsTSQL getConnectionTSQL closeConnectionTSQL  
    printfn "%A" <| queryBlastFurnacesTSQL getConnectionTSQL closeConnectionTSQL  
    printfn "%A" <| selectValues4LinesTSQL getConnectionTSQL closeConnectionTSQL  
    