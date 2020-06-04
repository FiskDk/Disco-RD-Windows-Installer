Module errorHandler
    Sub ErrorHandler(errorCode As String, Optional canRecover As Boolean = False)

        If errorCode = "0x0001" Then
            canRecover = False
            elog("0x0001 - Invalid version!" & Environment.NewLine & "Level : FATAL" & Environment.NewLine & "canRecover : " & canRecover)
        End If
        If errorCode = "0x0002" Then
            elog(errorCode + " - Unable to update - Not installed before - User chose no when asked if they wanted to install instead." & Environment.NewLine & "Level : MEDIUM" & Environment.NewLine & "canRecover : " & canRecover)
        End If
        If errorCode = "0x0003" Then
            elog(errorCode + " - Unable to update - Not installed before - User chose INVALID ARGUMENTS when asked if they wanted to install instead." & Environment.NewLine & "Level : MEDIUM" & Environment.NewLine & "canRecover : " & canRecover)
        End If
        If errorCode = "1x0001" Then
            elog(errorCode + " - Unable to generate key - invalid arguments (local, user)")
        End If
        If errorCode = "1x0002" Then
            elog(errorCode + " FATAL Unable to decrypt")
        End If
        ilog("Press any key to continue.")
        Console.ReadKey()
        If canRecover = False Then
            End
        Else
            Main()
        End If
    End Sub
End Module
