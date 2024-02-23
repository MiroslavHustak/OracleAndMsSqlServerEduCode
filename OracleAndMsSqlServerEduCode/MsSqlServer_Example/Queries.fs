module QueriesTSQL

open FSharp.Interop.Excel

open System
open System.Data
open FsToolkit.ErrorHandling

open System.Data.SqlClient

open Helpers
open ExcelTypeProviderTSQL

let private queryUpdate3 =
    "
    BEGIN
        EXEC DELETE_NULL_ROWS :table_name, :primary_key_column;
    END;  
    "

let internal querySteelStructuresTSQL getConnection closeConnection =
        
    let queryDropSequence = 
        "
        DECLARE @sequence_exists INT;
        SELECT @sequence_exists = COUNT(*) FROM sys.sequences WHERE name = 'EACH_TABLE_SEQUENCE';          
        IF @sequence_exists > 0
        BEGIN
            EXEC sp_executesql N'DROP SEQUENCE EACH_TABLE_SEQUENCE';
        END;
        "

    let queryDeleteAll = "DELETE FROM Steel_Structures"
       
    let queryCreateSequence = 
        "
        CREATE SEQUENCE EACH_TABLE_SEQUENCE
        START WITH 1
        INCREMENT BY 1
        NOCACHE
        NOCYCLE;
        "       
         
    let queryInsert = 
         "
         INSERT INTO Steel_Structures (English, Czech, Note) 
         VALUES (@English, @Czech, @Note);
         "

    let queryUpdate1 =
        "
        UPDATE Steel_Structures
        SET Note = NULL
        WHERE Note IS NOT NULL AND CHARINDEX('void', Note) > 0;
        " 

    let queryUpdate2 = // replaced with a stored procedure named DELETE_NULL_ROWS //_Steel_Structures
        "
        DECLARE @v_primary_key_value INT;
        DECLARE @v_count INT;

        SELECT TOP 1 @v_primary_key_value = ID_Steel
        FROM Steel_Structures
        WHERE English IS NULL AND Czech IS NULL AND Note IS NULL;

        IF @v_primary_key_value IS NOT NULL
        BEGIN
            DELETE FROM Steel_Structures
            WHERE ID_Steel = @v_primary_key_value;
        END;
        -- TODO: Handle the case when no row is found
        "
    
    let query = 
        [
            queryDropSequence
            queryDeleteAll
            queryCreateSequence
            queryInsert
            queryUpdate1
            queryUpdate3
        ]

    let list = 
        [
            "Steel_Structures"
            "ID_STEEL"
        ]
    
    let path = "e:\\source\\repos\\OracleDB_Excel_Files\\Slovnicek AJ new steel structures.xlsx"

    insertOrUpdateDictionaryTSQL getConnection closeConnection query path list

let internal queryWeldsTSQL getConnection closeConnection =
    
    let queryDropSequence = 
        "
        DECLARE @sequence_exists INT;
        SELECT @sequence_exists = COUNT(*) FROM sys.sequences WHERE name = 'EACH_TABLE_SEQUENCE';          
        IF @sequence_exists > 0
        BEGIN
            EXEC sp_executesql N'DROP SEQUENCE EACH_TABLE_SEQUENCE';
        END;
        "

    let queryDeleteAll = "DELETE FROM WELDS"
   
    let queryCreateSequence = 
        "
        CREATE SEQUENCE EACH_TABLE_SEQUENCE
        START WITH 1
        INCREMENT BY 1
        NOCACHE
        NOCYCLE;
        "       
     
    let queryInsert = 
         "
         INSERT INTO WELDS (English, Czech, Note) 
         VALUES (@English, @Czech, @Note);
         "

    let queryUpdate1 =
        "
        UPDATE WELDS
        SET Note = NULL
        WHERE Note IS NOT NULL AND CHARINDEX('void', Note) > 0;
        " 

    let queryUpdate2 = // replaced with a stored procedure named DELETE_NULL_ROWS //_WELDS
        "
        DECLARE @v_primary_key_value INT;
        DECLARE @v_count INT;

        SELECT TOP 1 @v_primary_key_value = ID_WELD
        FROM WELDS
        WHERE English IS NULL AND Czech IS NULL AND Note IS NULL;

        IF @v_primary_key_value IS NOT NULL
        BEGIN
            DELETE FROM WELDS
            WHERE ID_WELD = @v_primary_key_value;
        END;
        -- TODO: Handle the case when no row is found
        "
                      
    let query = 
        [
            queryDropSequence
            queryDeleteAll
            queryCreateSequence
            queryInsert
            queryUpdate1
            queryUpdate3
        ]

    let list = 
        [
            "WELDS"
            "ID_WELD"
        ]

    let path = "e:\\source\\repos\\OracleDB_Excel_Files\\Slovnicek AJ new welding.xlsx"

    insertOrUpdateDictionaryTSQL getConnection closeConnection query path list

let internal queryBlastFurnacesTSQL getConnection closeConnection =
    
    let queryDropSequence = 
        "
        DECLARE @sequence_exists INT;
        SELECT @sequence_exists = COUNT(*) FROM sys.sequences WHERE name = 'EACH_TABLE_SEQUENCE';          
        IF @sequence_exists > 0
        BEGIN
            EXEC sp_executesql N'DROP SEQUENCE EACH_TABLE_SEQUENCE';
        END;
        "

    let queryDeleteAll = "DELETE FROM BLAST_FURNACES"
   
    let queryCreateSequence = 
        "
        CREATE SEQUENCE EACH_TABLE_SEQUENCE
        START WITH 1
        INCREMENT BY 1
        NOCACHE
        NOCYCLE;
        "       
     
    let queryInsert = 
         "
         INSERT INTO BLAST_FURNACES (English, Czech, Note) 
         VALUES (@English, @Czech, @Note);
         "

    let queryUpdate1 =
        "
        UPDATE BLAST_FURNACES
        SET Note = NULL
        WHERE Note IS NOT NULL AND CHARINDEX('void', Note) > 0;
        " 

    let queryUpdate2 = // replaced with a stored procedure named DELETE_NULL_ROWS //_BLAST_FURNACES
        "
        DECLARE @v_primary_key_value INT;
        DECLARE @v_count INT;

        SELECT TOP 1 @v_primary_key_value = ID_BF
        FROM BLAST_FURNACES
        WHERE English IS NULL AND Czech IS NULL AND Note IS NULL;

        IF @v_primary_key_value IS NOT NULL
        BEGIN
            DELETE FROM BLAST_FURNACES
            WHERE ID_BF = @v_primary_key_value;
        END;
        -- TODO: Handle the case when no row is found
        "
                
    let query = 
        [
            queryDropSequence
            queryDeleteAll
            queryCreateSequence
            queryInsert
            queryUpdate1
            queryUpdate3
        ]

    let list = 
        [
            "BLAST_FURNACES"
            "ID_BF"
        ]

    let path = "e:\\source\\repos\\OracleDB_Excel_Files\\Slovnicek AJ new.xlsx"

    insertOrUpdateDictionaryTSQL getConnection closeConnection query path list


    (*        
    -- Drop trigger
    IF OBJECT_ID('Steel_Structures_Trigger', 'TR') IS NOT NULL
        DROP TRIGGER Steel_Structures_Trigger;
        
    -- Drop sequence
    IF OBJECT_ID('Each_Table_Sequence', 'SO') IS NOT NULL
        DROP SEQUENCE Each_Table_Sequence;
    
    -- Drop table
    IF OBJECT_ID('Steel_Structures', 'U') IS NOT NULL
        DROP TABLE Steel_Structures;
    
    -- Drop sequence
    IF OBJECT_ID('Each_Table_Sequence', 'SO') IS NOT NULL
    BEGIN
        DECLARE @sequence_exists INT;
        SELECT @sequence_exists = COUNT(*) FROM sys.sequences WHERE name = 'Each_Table_Sequence';
        
        IF @sequence_exists > 0
        BEGIN
            EXEC('DROP SEQUENCE Each_Table_Sequence');
        END;
    END;
    
    -- Create sequence
    CREATE SEQUENCE Each_Table_Sequence
        START WITH 1
        INCREMENT BY 1
        NO CACHE
        NO CYCLE;
    
    USE [Dictionary_MSSQLS]
    
    -- Create table
    CREATE TABLE Blast_Furnaces
    (
        ID_BF INT PRIMARY KEY NOT NULL,
        English NVARCHAR(100),
        Czech NVARCHAR(100),
        Note NVARCHAR(1000)
    );
    
    -- Create table
    CREATE TABLE Steel_Structures
    (
        ID_Steel INT PRIMARY KEY NOT NULL,
        English NVARCHAR(100),
        Czech NVARCHAR(100),
        Note NVARCHAR(1000)
    );
    
    -- Create table
    CREATE TABLE Welds
    (
        ID_Weld INT PRIMARY KEY NOT NULL,
        English NVARCHAR(100),
        Czech NVARCHAR(100),
        Note NVARCHAR(1000)
    );
    
    -- To use the sequence for automatic numbering
    CREATE TRIGGER Steel_Structures_Trigger
    ON Steel_Structures
    AFTER INSERT
    AS
    BEGIN
        SET NOCOUNT ON;
    
        UPDATE Steel_Structures
        SET ID_Steel = NEXT VALUE FOR Each_Table_Sequence
        WHERE ID_Steel IS NULL;
    END;
    
    -- Update queries
    UPDATE Steel_Structures
    SET Note = NULL
    WHERE Note IS NOT NULL AND CHARINDEX('void', Note) > 0;
    
    UPDATE Steel_Structures
    SET Note = REPLACE(Note, 'void', NULL)
    WHERE Note IS NOT NULL AND CHARINDEX('void', Note) > 0;
    
    -- Stored Procedure DELETE_NULL_ROWS
    IF OBJECT_ID('DELETE_NULL_ROWS', 'P') IS NOT NULL
        DROP PROCEDURE DELETE_NULL_ROWS;
    GO
    
    CREATE PROCEDURE DELETE_NULL_ROWS
        @p_table_name NVARCHAR(100),
        @p_primary_key_column NVARCHAR(100)
    AS
    BEGIN
        DECLARE @v_primary_key_value INT;
        DECLARE @v_sql_query NVARCHAR(1000); -- Adjust the size based on your needs
    
        -- Build the dynamic SQL query with a parameter for the table name
        SET @v_sql_query =
            'SELECT ' + @p_primary_key_column +
            ' FROM ' + @p_table_name +
            ' WHERE English IS NULL AND Czech IS NULL AND Note IS NULL';
    
        -- Find a row meeting the conditions where all non-primary key columns are NULL
        EXEC sp_executesql @v_sql_query, N'@v_primary_key_value INT OUTPUT', @v_primary_key_value OUTPUT;
    
        -- Check if a row meeting the conditions is found
        IF @v_primary_key_value IS NOT NULL
        BEGIN
            -- Build the dynamic SQL delete statement
            SET @v_sql_query =
                'DELETE FROM ' + @p_table_name +
                ' WHERE ' + @p_primary_key_column + ' = @v_primary_key_value';
    
            -- Delete the row using the dynamically determined primary key value
            EXEC sp_executesql @v_sql_query, N'@v_primary_key_value INT', @v_primary_key_value;
        END;
        ELSE
            -- TODO: Handle the case when no row is found
            NULL;
        END IF;
    END;    
    
    *)


