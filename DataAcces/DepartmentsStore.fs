module DataAcces.DepartmentsStore

open Application.Departments
open DataAcces.Store
open Model.Department
open DataAcces.Database
open Common


type DepartmentsStore(store: Store) =
    interface IDeparment with
        // Function to update the department name
        // Parameters:
        // - name: The new name for the department
        // - id: The ID of the department to update
        // Returns:
        // - Result<unit, string>: Result indicating success or failure
       member this.updateDepartmentName (name: DepartmentName) (id: DepartmentID) : Result<unit, string> =
            match InMemoryDatabase.lookup id store.departments with
            | Some (_, _, parentDepartmentOption) ->
                let updatedDepartment = (id, name, parentDepartmentOption)
                InMemoryDatabase.update id updatedDepartment store.departments
                Result.Ok ()
            | None -> Result.Error "Afdeling niet gevonden"
            
        // Functie om alle sub-deparments ids's op te halen
        // Parameters:
        // - id: ID van de deparment
        // Returns:
        // - List<DepartmentID>: Lijst van department IDs die sub-deparments presenteren    
        member this.getSubDepartments (id: DepartmentID) : List<DepartmentID> =
            let mainDepartment = store.departments |> InMemoryDatabase.lookup id
            let subDepartments = store.departments |> InMemoryDatabase.filter (fun (_, _, parentDepartment) -> parentDepartment = Some id)
            let departmentToDepartmentInfo (department, _, _) = department
            let mainDepartmentInfo = Option.map departmentToDepartmentInfo mainDepartment
            let subDepartmentInfos = subDepartments |> Seq.map departmentToDepartmentInfo |> Seq.toList
            let departmentList = match mainDepartmentInfo with
                                 | Some mainDep -> mainDep :: subDepartmentInfos
                                 | None -> subDepartmentInfos
            departmentList |> List.map (fun departmentInfo -> departmentInfo)

    