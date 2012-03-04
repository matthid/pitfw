﻿#r @"fake\FakeLib.dll"

open Fake

let release = @".\bin\release"
let debug   = @".\bin\debug"
let dbgdebug    = debug + @"\silverlight"
let dbgrelease  = release + @"\silverlight"
    
let pitcore     = !! @"src\Pit.sln"
let pitcompiler = !! @"src\Pit.Compiler.sln"
let pitdbg      = !! @"src\Pit.dbg.sln"
let pitbuild    = !! @"build\Pit.Build\Pit.Build.sln"

Target? Clean <-
    fun () ->
        CleanDirs [release;debug]
        [@"src\scripts\pit.js";@"src\scripts\pit.core.min.js"]
        |> CopyTo debug

        [@"src\scripts\pit.js";@"src\scripts\pit.core.min.js"]
        |> CopyTo release

Target? BuildDebug <-
    fun () ->
        trace "Building Debug assemblies"

        MSBuildDebug debug "Clean" pitcore
        |> Log "Clean Debug Build:"
        MSBuildDebug debug "Clean" pitcompiler
        |> Log "Clean Debug Build:"
        MSBuildDebug debug "Clean" pitbuild
        |> Log "Clean Debug Build:"
        MSBuildDebug dbgdebug "Clean" pitdbg
        |> Log "Clean Debug Build:"

        MSBuildDebug debug "Build" pitcore
        |> Log "Debug Build:"
        MSBuildDebug debug "Build" pitcompiler
        |> Log "Debug Build:"
        MSBuildDebug debug "Build" pitbuild
        |> Log "Debug Build:"
        MSBuildDebug dbgdebug "Build" pitdbg
        |> Log "Debug Build:"

Target? Release <-
    fun () ->
        trace "Building Release assemblies"
        MSBuildRelease release "Clean" pitcore
        |> Log "Clean Release Build:"
        MSBuildRelease release "Clean" pitcompiler
        |> Log "Clean Release Build:"
        MSBuildRelease release "Clean" pitbuild
        |> Log "Clean Release Build:"
        MSBuildRelease dbgrelease "Clean" pitdbg
        |> Log "Clean Release Build:"

        MSBuildRelease release "Build" pitcore
        |> Log "Release Build:"
        MSBuildRelease release "Build" pitcompiler
        |> Log "Release Build:"
        MSBuildRelease release "Build" pitbuild
        |> Log "Release Build:"
        MSBuildRelease dbgrelease "Build" pitdbg
        |> Log "Release Build:"

Target? Default <-
    fun () -> trace "Finished..."

"Clean"
    ==> "Release" <=> "BuildDebug"
    ==> "Default"

Run? Default