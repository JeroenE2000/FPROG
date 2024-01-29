/// ====================================
/// ==== DO NOT CHANGE THIS FILE    ====
/// ====                            ====
/// ==== You do not have to alter   ====
/// ==== this file for the          ====
/// ==== assessment                 ====
/// ====================================
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Thoth.Json.Giraffe
open Thoth.Json.Net
open DataAcces.Store
open DataAcces.EmployeeStore
open Application
open Paidride
open DataAcces.HourStore
open DataAcces.DepartmentsStore

let configureApp (app: IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe Web.routes

let configureServices (services: IServiceCollection) =
    // Add Giraffe dependencies
    services
        .AddGiraffe()
        .AddSingleton<Store>(Store())
        .AddSingleton<Employee.IEmployee, EmployeeStore>()
        .AddSingleton<Hours.IHours, HoursStore>()
        .AddSingleton<Departments.IDeparment, DepartmentsStore>()
        .AddSingleton<Json.ISerializer>(ThothSerializer(skipNullField = false, caseStrategy = CaseStrategy.CamelCase))
    |> ignore
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(configureApp)
                .ConfigureServices(configureServices)
            |> ignore)
        .Build()
        .Run()

    0
