﻿'{[{
Imports System.Linq
Imports System.Threading.Tasks
Imports Param_RootNamespace.Core.Services
'}]}

Public Class Tests
    '^^
    '{[{

    ' TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
    ' This test serves only as a demonstration of testing functionality in the Core library.
    <TestMethod>
    Public Async Function EnsureSampleDataServiceReturnsModelDataAsync() As Task
        Dim actual = Await SampleDataService.GetSampleModelDataAsync()

        Assert.AreNotEqual(0, actual.Count())
    End Function
    '}]}
End Class
