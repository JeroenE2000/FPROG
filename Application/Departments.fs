module Application.Departments

open Model.Department
open Common

type IDeparment =
    abstract member getSubDepartments : DepartmentID -> List<DepartmentID>
    abstract member updateDepartmentName : DepartmentName -> DepartmentID -> Result<unit, string>
    
let getSubDepartments (deparment: IDeparment) (name: DepartmentID) : List<DepartmentID> =
    deparment.getSubDepartments name
    |> List.distinct 
    
let updateDepartmentName (deparment : IDeparment) (name: DepartmentName) (id: DepartmentID) : Result<unit, string> =
    deparment.updateDepartmentName name id