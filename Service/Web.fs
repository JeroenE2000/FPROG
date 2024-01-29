module Paidride.Web

open Giraffe
open Thoth.Json.Giraffe
open Microsoft.AspNetCore.Http
open Application.Employee
open Application.Hours
open Application.Departments
open Application
open Thoth.Json.Net

//haalt alle employees op
let getEmployees (next: HttpFunc) (ctx: HttpContext) = 
    task {
          let dataAcces = ctx.GetService<IEmployee> ()
          let employees = Employee.getAllEmployees dataAcces
          return! ThothSerializer.RespondJsonSeq employees Serialization.encodeEmployees next ctx
    }

// haalt een singel employee op
let getEmployee (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAcces = ctx.GetService<IEmployee>()
            let employee = Employee.lookup dataAcces name 
            match employee with
            | None -> return! RequestErrors.NOT_FOUND "Medewerker niet gevonden!" next ctx
            | Some employee ->
                return! ThothSerializer.RespondJson employee Serialization.encodeEmployees next ctx
        }

// regristeren van uren van een employee
let registerHours (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IHours>()
            let! hoursResult = ThothSerializer.ReadBody ctx Serialization.decodeHours
            
            match hoursResult with
            | Ok hours ->
                match Hours.registerHours dataAccess (name, hours.Date, hours.Amount) with
                | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
                | Ok () -> return! Successful.ok (text "Uren geregistreerd") next ctx
            | Error errorMessage -> return! RequestErrors.notAcceptable (text $"Fout: De uren kon niet aangemaakt worden {errorMessage}") earlyReturn ctx
        }

//haalt alle uren op van een user        
let totalHoursFor(name: string) : HttpHandler = 
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IHours>()
            let hours = Hours.totalHoursFor dataAccess name
            return! ThothSerializer.RespondJson hours Encode.int next ctx
        }
        
//haalt alle overtimeUren op van een user        
let overtimeFor (name: string) : HttpHandler = 
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IHours>()
            let overTime = Hours.overTimeFor dataAccess name
            return! ThothSerializer.RespondJson overTime Encode.int next ctx
        }


//haalt alle uren op van een bepaald deparment
let totalHoursWorkedInDeparment (id: string) : HttpHandler = 
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IDeparment>()
            let deparments = getSubDepartments dataAccess (Serialization.decodeDeparmentIdFromString id)
            let employees = getAllEmployeesFromDepartment  deparments (ctx.GetService<IEmployee>())
            let hours = totalHoursForEmployees (ctx.GetService<IHours>()) employees 
            
            return! ThothSerializer.RespondJson hours Encode.int next ctx
        }
        
//haalt alle overTime uren op van een bepaald deparment
let totalOverTimeHoursForDepartment (id: string) : HttpHandler =
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IDeparment>()
            let deparments = getSubDepartments dataAccess (Serialization.decodeDeparmentIdFromString id)
            let employees = getAllEmployeesFromDepartment  deparments (ctx.GetService<IEmployee>())
            let hours = totalOverTimeHoursForEmployees (ctx.GetService<IHours>()) employees 
            return! ThothSerializer.RespondJson hours Encode.int next ctx
        }
        
//kan een deparmentName updaten
let updateDepartmentName (id: string) : HttpHandler = 
    fun next ctx ->
        task {
            let dataAccess = ctx.GetService<IDeparment>()
            let! checkDepartment = ThothSerializer.ReadBody ctx Serialization.decodeDepartment
            match checkDepartment with
            | Ok department ->
                match updateDepartmentName dataAccess department.Name (Serialization.decodeDeparmentIdFromString id) with
                | Error errorMessage -> return! RequestErrors.BAD_REQUEST errorMessage next ctx
                | Ok _ -> return! Successful.ok (text $"Afdeling naam aangepast naar {department.Name}") next ctx
            | Error errorMessage -> return! RequestErrors.notAcceptable (text $"Fout: Deparmentname kon niet geupdate worden {errorMessage}") earlyReturn ctx
        }


let routes: HttpHandler =
    choose [ GET >=> route "/employee" >=> getEmployees
             GET >=> routef "/employee/%s" getEmployee
             POST >=> routef "/employee/%s/hours" registerHours 
             GET >=> routef "/employee/%s/hours" totalHoursFor
             GET >=> routef "/employee/%s/overtime" overtimeFor
             GET >=> routef "/department/%s/hours" totalHoursWorkedInDeparment
             GET >=> routef "/department/%s/overtime" totalOverTimeHoursForDepartment
             PUT >=> routef "/department/%s" updateDepartmentName
            ]
