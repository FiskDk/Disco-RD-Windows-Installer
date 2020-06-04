Module keyGen

    Sub keyGen()
        log("You now have the following options")
        log("To generate a key for this machine - type local")
        log("To generate a key for a specific user - type user")
        clog("What do you want to do? (local, user)")
        Dim YN As String = Console.ReadLine()
        If YN = "local" Then
            ilog("Generating key for this machine...")
            Dim input As String = EncodeBase64(Environment.UserName)
            Dim correctKey As String = StringToBinary(Environment.UserName, "")
            llog("Key has been generated for : " + input)
            llog("Key : " & correctKey)
            clog("Do you want to use that key now? (y/n)")
            YN = Console.ReadLine()
            If YN = "y" Then
                llog("Using key : " & correctKey)
            Else

                If YN = "n" Then
                    clog("Press enter to restart after you've copied your key.")
                Else

                End If
        End If

                Console.ReadLine()
            Console.Clear()
            Module1.Loader(True)

        Else
            If YN = "user" Then
                ilog("Generating key for specific user")
                clog("Please enter the encoded user string")
                Dim input As String = DecodeBase64(Console.ReadLine())

                Dim correctKey As String = StringToBinary(Environment.UserName, "")
                Console.WriteLine(correctKey)
                Console.WriteLine("Key has been generated for : " + input)
                Console.ReadLine()
            Else
                errorHandler.ErrorHandler("1x0001", True)
            End If
        End If

    End Sub
End Module
