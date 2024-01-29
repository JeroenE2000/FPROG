module Application.Employee

open Model.Employee
open Common

type IEmployee =
    abstract member all: unit -> List<Employee>
    abstract member lookup: string -> Employee Option

// Haalt alle medewerkers op.
// - `employees`: Een instantie van het `IEmployee`-interface voor toegang tot medewerkersgegevens.
// Geeft terug: Een lijst van alle medewerkers.
let getAllEmployees (employees: IEmployee) : List<Employee> =
    employees.all() 

// Zoekt een medewerker op basis van de naam.
// - `name`: De naam van de medewerker die moet worden opgezocht.
// - `employees`: Een instantie van het `IEmployee`-interface voor toegang tot medewerkersgegevens.
// Geeft terug: Een `Employee Option` die `Some` bevat met de gevonden medewerker als deze bestaat, of `None` als de medewerker niet gevonden wordt.
let lookup (employees: IEmployee) (name: string)  : Employee option = 
    employees.lookup name

// Haalt alle medewerkers op die behoren tot de opgegeven afdelingsnaam.
// - `name`: De naam van de afdeling waaruit de medewerkers worden opgehaald.
// - `employees`: Een instantie van het `IEmployee`-interface voor toegang tot medewerkersgegevens.
// Geeft terug: Een lijst van medewerkers die behoren tot de opgegeven afdelingsnaam.
let getAllEmployeeIdFromDepartmentId  (name: string) (employees: IEmployee)  : List<string> = 
    let departmentId = CheckDepartmentIDModule.fromStringToDeparmentID name
    employees.all() 
    |> List.filter (fun x -> x.DepartmentId = departmentId)
    |> List.map (fun x -> x.Name)

// Geeft een lijst van alle medewerkers terug die behoren tot de opgegeven afdelingen.
// - `deparments`: Een lijst van afdelingen waaruit de medewerkers worden opgehaald.
// - `employees`: Een instantie van het `IEmployee`-interface voor toegang tot medewerkersgegevens.
// Geeft terug: Een lijst van medewerkers die behoren tot de opgegeven afdelingen.    
let getAllEmployeesFromDepartment  (deparments: List<DepartmentID>) (employees: IEmployee)  : List<string> = 
    let departmentNames = deparments |> List.map (fun x -> x)
    let employeesFromDepartment = departmentNames |> List.map (fun x -> getAllEmployeeIdFromDepartmentId (CheckDepartmentIDModule.fromDepartmentId x) employees)
    employeesFromDepartment |> List.concat