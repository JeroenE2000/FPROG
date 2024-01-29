module DataAcces.Store

open System
open DataAcces.Database
open Model.Hours
open Common
open Model.Department
open Model.Employee

/// Here a store is created that contains the following tables with the following attributes
///
/// employees (primary key is name)
/// - name
/// - department id
///
/// hours (primary key is compound: employee name and date)
/// - employee name (foreign key to employees)
/// - date
/// - amount (int)
///
/// departments
/// - id
/// - name
/// - super (nullable department id of which this is a subdepartment)
///
/// departmentDepartments


type Store() =
    member val employees: InMemoryDatabase<string, string * DepartmentID> =
        [ "Jeroen", CheckDepartmentIDModule.fromStringToDeparmentID "AAAA13"
          "Bob", CheckDepartmentIDModule.fromStringToDeparmentID "BBBB37"
          "Ernst", CheckDepartmentIDModule.fromStringToDeparmentID "CCCC42" ]
        |> Seq.map (fun t -> fst t, t)
        |> InMemoryDatabase.ofSeq

    member val hours: InMemoryDatabase<string * DateTime, string * DateTime * HoursAmount> =
        [ "Jeroen", DateTime(2023, 5, 1), HoursAmountModule.fromInt 8 
          "Jeroen", DateTime(2023, 5, 2), HoursAmountModule.fromInt 10
          "Jeroen", DateTime(2023, 5, 3), HoursAmountModule.fromInt 9
          "Ernst", DateTime(2023, 5, 1), HoursAmountModule.fromInt 10
          "Ernst", DateTime(2023, 5, 2), HoursAmountModule.fromInt 9
          "Bob", DateTime(2023, 5, 8), HoursAmountModule.fromInt 10
          "Bob", DateTime(2023, 5, 2), HoursAmountModule.fromInt 8 ]
        |> Seq.map (fun (n, v, p) -> (n, v), (n, v, p))
        |> InMemoryDatabase.ofSeq
        
    member val departments : InMemoryDatabase<DepartmentID, DepartmentID * DepartmentName * DepartmentID Option> =
        [  CheckDepartmentIDModule.fromStringToDeparmentID "AAAA13", DepartmentNameModule.fromString "Lost and Found", Some (CheckDepartmentIDModule.fromStringToDeparmentID "CCCC42")
           CheckDepartmentIDModule.fromStringToDeparmentID "BBBB37", DepartmentNameModule.fromString "Customer Service", Some (CheckDepartmentIDModule.fromStringToDeparmentID "CCCC42")
           CheckDepartmentIDModule.fromStringToDeparmentID "CCCC42", DepartmentNameModule.fromString "Human Resources", None ]
        |> Seq.map (fun (x, y, z) -> x, (x, y, z))
        |> InMemoryDatabase.ofSeq
             