namespace DataAcces.EmployeeStore

open DataAcces.Store
open Application.Employee
open Model.Employee
open DataAcces.Database

type EmployeeStore(store: Store) =
    interface IEmployee with
        member this.lookup name =
            match InMemoryDatabase.lookup name store.employees with
            | Some (name, departmentId) -> Some { Name = name; DepartmentId = departmentId }
            | None -> None
        member this.all () =
            InMemoryDatabase.all store.employees
            |> Seq.map (fun (name, departmentId) -> { Name = name; DepartmentId = departmentId })
            |> Seq.toList
