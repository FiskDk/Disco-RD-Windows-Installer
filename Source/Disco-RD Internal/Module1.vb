
Imports System.Deployment
Imports System.Reflection
Imports System.Threading
Imports System.Net
Imports System.IO
Imports System.Drawing
Imports System.Security.Cryptography

Module Module1
    Dim checksum As String = "D326E28F9D968E9CA254A68D28D1D18A"
    Dim option_Step As Boolean
    Dim option_Debug As Boolean = False
    Dim option_SilentStep As Boolean = False
    Dim version As String = "0.0.4"
    Dim latestVersion As String = ""
    Dim clientVersion As String = ""
    Dim options_registered As Boolean = False
    Dim options_fromKeyGen As Boolean = False
    Dim option_installed As Boolean = False
    Sub checkVersion(installedVersion)
        Dim address As String = "https://raw.githubusercontent.com/FiskDk/discord-security-exploiting/master/installerVersion.txt"
        Dim client As WebClient = New WebClient()
        Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
        Dim tmpVer As String = reader.ReadToEnd
        latestVersion = tmpVer.Replace(".", "")
        log("Checking for updates...")
        ilog("Latest version : " + tmpVer, "INTERNAL VALUE : " & Convert.ToInt16(latestVersion))
        ilog("Current version : " + installedVersion, "INTERNAL VALUE : " & Convert.ToInt16(version.Replace(".", "")))
        If IO.File.Exists("C:\Program Data\Disco-RD\ver.txt") Then
            clientVersion = IO.File.ReadAllText("C:\Program Data\Disco-RD\ver.txt")
            option_installed = True
            ilog("Installed Disco-RD Client Version : " + clientVersion)
        Else
            ilog("No instalation of Disco-RD could be found on your system.")
        End If

        If Convert.ToInt16(latestVersion) > Convert.ToInt16(version.Replace(".", "")) Then
            ilog("There is a new update for the installer.")
            ilog("Please go to the Github Releases and download the latest installer!")
        Else
            If Convert.ToInt16(latestVersion) < Convert.ToInt16(version.Replace(".", "")) Then
                If option_Debug = False Then
                    ilog("You are running a unsupported version! - Please re-install.")
                    errorHandler.ErrorHandler("0x0001")
                Else
                    ilog("You are running a debugging or unreleased version!")
                End If
            Else
                ilog("No new updates!")
            End If
        End If
        checkLic()
    End Sub
    Public Function EncodeBase64(input As String) As String
        Return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input))
    End Function

    Function md5(ByVal file_name As String)
        Dim hash = md5.Create()
        Dim hashValue() As Byte
        Dim fileStream As FileStream = File.OpenRead(file_name)
        fileStream.Position = 0
        hashValue = hash.ComputeHash(fileStream)
        Dim hash_hex = PrintByteArray(hashValue)
        fileStream.Close()
        Return hash_hex
    End Function

    Public Function PrintByteArray(ByVal array() As Byte)
        Dim hex_value As String = ""
        Dim i As Integer
        For i = 0 To array.Length - 1
            hex_value += array(i).ToString("X2")
        Next i
        Return hex_value.ToLower
    End Function
    Sub checkLic()
        Dim correctKey As String = StringToBinary(Environment.UserName, "")
        'Because this is the public version then the licemse system thing isnt used and just skipped
        options_registered = True
        System.IO.File.WriteAllText("lic.key", correctKey)
        If options_registered = True Then
            llog("You are a registered INDEV tester!")
            llog("Your key : " + correctKey)
            invalidArgs()
        Else
            If IO.File.Exists("lic.key") Then
                If IO.File.ReadAllText("lic.key") = correctKey Then
                    options_registered = True
                    llog("You are a registered INDEV tester!")
                    llog("Your key : " + correctKey)
                    invalidArgs()
                Else
                    elog("Invalid lic.key file.")
                End If

            End If
            llog("You are not a registered INDEV tester.")
            llog("To get your key - type the following key to the dev to get your personal INDEV key.")
            Console.WriteLine(EncodeBase64(Environment.UserName))
            'Console.WriteLine(correctKey)
            clog("Please type in your INDEV testing key.")
            Dim inputKey = Console.ReadLine()
            Try
                Dim licKey As String = inputKey
                If licKey = correctKey Then
                    options_registered = True
                    System.IO.File.WriteAllText("lic.key", licKey)
                    Console.Clear()
                    Main()
                Else
                    elog("Wrong key.")
                    Console.Clear()
                    Main()
                End If

            Catch err As Exception

            Finally
                elog("Error - Wrong key or format")
                Main()
            End Try
        End If

    End Sub
    Sub llog(text)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.DarkGreen
        Console.Write("Disco-RD DRM")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("] ")
        Console.ResetColor()
        Console.Write(text + Environment.NewLine)
    End Sub

    Sub elog(text As String)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.DarkRed
        Console.Write("Disco-RD ERROR")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("] ")
        Console.ResetColor()
        Console.Write(text + Environment.NewLine)
    End Sub
    Sub log(text As String, Optional debug As String = "")
        If Not text = "" Then
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("[")
            Console.ForegroundColor = ConsoleColor.DarkBlue
            Console.Write("Disco-RD Installer")
            Console.ForegroundColor = ConsoleColor.White
            Console.Write("] ")
            Console.ResetColor()
            Console.Write(text + Environment.NewLine)
        End If
        If option_Debug = True Then
            If Not debug = "" Then
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("[")
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Disco-RD DEBUG")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("] ")
                Console.ResetColor()
                Console.Write(debug + Environment.NewLine)
            End If
        End If
        If option_Step = True Then
            If Not option_SilentStep = True Then
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("[")
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Disco-RD STEPPING")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("] ")
                Console.ResetColor()
                Console.Write("Taking a step" + Environment.NewLine)
            End If

            Console.ReadKey()
        End If
    End Sub
    Sub dlog(text As String)
        If option_Debug = True Then
            If Not text = "" Then
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("[")
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Disco-RD DEBUG")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("] ")
                Console.ResetColor()
                Console.Write(text + Environment.NewLine)
            End If
        End If
    End Sub
    Sub ilog(text As String, Optional Debug As String = "")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.Write("Disco-RD INFO")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("] ")
        Console.ResetColor()
        Console.Write(text + Environment.NewLine)
        If option_Debug = True Then
            If Not Debug = "" Then
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("[")
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Disco-RD DEBUG")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("] ")
                Console.ResetColor()
                Console.Write(Debug + Environment.NewLine)
            End If
        End If
        If option_Step = True Then
            If Not option_SilentStep = True Then
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("[")
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Disco-RD STEPPING")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("] ")
                Console.ResetColor()
                Console.Write("Taking a step" + Environment.NewLine)
            End If

            Console.ReadKey()
        End If
    End Sub
    Sub clog(text As String)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("[")
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write("Disco-RD USER INPUT")
        Console.ForegroundColor = ConsoleColor.White
        Console.Write("] ")
        Console.ResetColor()
        Console.Write(text + Environment.NewLine)
    End Sub
    Sub install()
        Dim appData As String = "C:\Program Data\Disco-RD"
        log("Starting to install Disco-RD")
        log("", "Checking if the directory C:\Program Data\Disco-RD exists")

        If Not IO.Directory.Exists("C:\Program Data\Disco-RD") Then
            dlog("C:\Program Data\Disco-RD Didn't exist - Create it")
            IO.Directory.CreateDirectory("C:\Program Data\Disco-RD")
            dlog("Created directory : C:\Program Data\Disco-RD")
        Else
            dlog("C:\Program Data\Disco-RD Does exist, it shouldnt becuase this is soppoused to be a clean install. - Delete it")

            If System.IO.Directory.Exists(appData) Then
                System.IO.Directory.Delete(appData, True)
            End If
            dlog("Deleted the directory : " + appData)

            dlog("C:\Program Data\Disco-RD Didn't exist - Create it")
            IO.Directory.CreateDirectory("C:\Program Data\Disco-RD")
            dlog("Created directory : C:\Program Data\Disco-RD")
        End If
        Dim prjPath As String = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
        log("Writing kill script...")
        System.IO.File.WriteAllText(appData & "\kill.cmd", "taskkill /F /IM discord.exe")
        dlog("Wrote the killscript to : " + appData + "\kill.cmd")
        log("Creating new thread for downloading the latest files...")
        Dim thread As New Thread(
  Sub()
      My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/FiskDk/discord-security-exploiting/master/core.asar",
appData & "\core.asar")
      dlog("Downloading file from url : https://raw.githubusercontent.com/FiskDk/discord-security-exploiting/master/core.asar - to location : " + appData & "core.asar")
      dlog("Waiting 5000 miliseconds so the download can compleate,")
      Threading.Thread.Sleep(8000)
      Console.WriteLine(md5(appData & "\core.asar"))
      Console.ReadLine()
      If Not md5(appData & "\core.asar") = checksum Then
          elog("Error : Unable to download file!")
          errorHandler.ErrorHandler("0x1000", False)
      End If
  End Sub
)
        log("Starting thread...")
        thread.Start()

        log("Starting kill script...")
        dlog("Launching process cmd.exe with args /c " + appData & "\kill.cmd - which launches the killscript")
        Process.Start(appData & "\kill.cmd")
        log("Waiting for Discord to terminate...")
        dlog("Waiting 5000 miliseconds for Discord to close")
        Threading.Thread.Sleep(5000)
        log("Setting internal vars...")
        Dim discordDir As String = "C:\Users\" & Environment.UserName & "\AppData\Roaming\Discord\0.0.306\modules\discord_desktop_core"
        log("Getting ready to install...")
        dlog("Checks if " + discordDir + " exists")
        If System.IO.Directory.Exists(discordDir) Then
            dlog("It does.")
            log("Discord version check : OK")
            dlog("Checks if " + discordDir + "\core.bak" + " exists")
            If System.IO.File.Exists(discordDir & "\core.bak") Then
                dlog("It does. - Delete it")
                log("Removing old version...")
                System.IO.File.Delete(discordDir & "\core.bak")
                dlog("Deleted.")
                log("Old version removed!")
            End If
            dlog("Checks if " + discordDir + "\core.asar" + " exists")
            If System.IO.File.Exists(discordDir & "\core.asar") Then
                dlog("It does. - Rename it to core.bak")
                log("Copying original files...")
                My.Computer.FileSystem.RenameFile(discordDir & "\core.asar", "core.bak")
                dlog("Renamed.")
                log("Copied successfully!")
            End If
            log("Installing...")
            dlog("Copying file " + appData & "\core.asar" + " to " + discordDir & "\core.asar")
            System.IO.File.Copy(appData & "\core.asar", discordDir & "\core.asar")
            dlog("Done.")
            log("Done! Launching Discord!")
            'Launch Discord
            dlog("Launching process " + "C:\Users\" & Environment.UserName & "\AppData\Local\Discord\Update.exe" + " with the arguments : " + "--processStart Discord.exe")
            Process.Start("C:\Users\" & Environment.UserName & "\AppData\Local\Discord\Update.exe", "--processStart Discord.exe")
            log("Finished!")
            dlog("Launched process.")
            readCommand()
        Else

        End If
    End Sub
    Sub updateRD()
        log("Checking if you can update...")
        log("", "Checking if the directory C:\Program Data\Disco-RD exists")
        If Not IO.Directory.Exists("C:\Program Data\Disco-RD") Then
            dlog("The directory dosn't exists!")
            log("It looks like you havn't installed Disco-RD!")
            log("Do you want to install Disco-RD? (y/n)")
            Dim YN As String = Console.ReadLine().ToLower()
            If YN = "y" Then
                install()
            ElseIf YN = "n" Then
                errorHandler.ErrorHandler("0x0002", True)
            Else
                log("Invalid arguments!")
                errorHandler.ErrorHandler("0x0003", True)
            End If
        End If
        log("Updating...")
        Dim appData As String = "C:\Program Data\Disco-RD"
        Dim prjPath As String = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
        log("Checking for old files")
        dlog("Checking if the file : " + appData & "\core.asar" + " exists.")
        If IO.File.Exists(appData & "\core.asar") Then
            dlog("It does. - Delete it.")
            IO.File.Delete(appData & "\core.asar")
            dlog("Deleted.")
            log("Deleted core.asar")
        End If
        dlog("Checking if the file : " + appData & "\ver.txt" + " exists.")
        If IO.File.Exists(appData & "\ver.txt") Then
            dlog("It does. - Delete it")
            IO.File.Delete(appData & "\ver.txt")
            dlog("Deleted,")
            log("Deleted ver.txt")
        End If
        dlog("Downloading file from url : https://raw.githubusercontent.com/FiskDk/discord-security-exploiting/master/core.asar - tp location : " + appData & "core.asar")

        My.Computer.Network.DownloadFile("https://raw.githubusercontent.com/FiskDk/discord-security-exploiting/master/core.asar",
appData & "\core.asar")
        dlog("Waiting 5000 miliseconds so the download can compleate,")
        Threading.Thread.Sleep(8000)
        dlog("Writing killscript")
        log("Writing killscript...")
        System.IO.File.WriteAllText(appData & "\kill.cmd", "taskkill /F /IM discord.exe")
        dlog("Wrote the killscript to : " + appData + "\kill.cmd")
        dlog("Launching process cmd.exe with args /c " + appData & "\kill.cmd - which launches the killscript")
        Process.Start(appData & "\kill.cmd")
        dlog("Waiting 5000 miliseconds for Discord to close.")
        Threading.Thread.Sleep(5000)
        Dim discordDir As String = "C:\Users\" & Environment.UserName & "\AppData\Roaming\Discord\0.0.306\modules\discord_desktop_core"
        log("Getting ready to install...")
        dlog("Checks if " + discordDir + " exists")
        If System.IO.Directory.Exists(discordDir) Then
            dlog("It does.")
            log("Discord version check : OK")
            dlog("Checks if " + discordDir + "\core.bak" + " exists")
            If System.IO.File.Exists(discordDir & "\core.bak") Then
                dlog("It does. - Delete it")
                log("Removing old version...")
                System.IO.File.Delete(discordDir & "\core.bak")
                dlog("Deleted.")
                log("Old version removed!")
            End If
            dlog("Checks if " + discordDir + "\core.asar" + " exists")
            If System.IO.File.Exists(discordDir & "\core.asar") Then
                dlog("It does. - Rename it to core.bak")
                log("Copying original files...")
                My.Computer.FileSystem.RenameFile(discordDir & "\core.asar", "core.bak")
                dlog("Renamed.")
                log("Copied successfully!")
            End If
            log("Installing...")
            dlog("Copying file " + appData & "\core.asar" + " to " + discordDir & "\core.asar")
            System.IO.File.Copy(appData & "\core.asar", discordDir & "\core.asar")
            dlog("Done.")
            log("Done! Launching Discord!")
            'Launch Discord
            dlog("Launching process " + "C:\Users\" & Environment.UserName & "\AppData\Local\Discord\Update.exe" + " with the arguments : " + "--processStart Discord.exe")
            Process.Start("C:\Users\" & Environment.UserName & "\AppData\Local\Discord\Update.exe", "--processStart Discord.exe")
            log("Finished!")
            dlog("Launched process.")
            dlog("Cleaning up.")
            IO.File.Delete(appData & "\core.asar")
            dlog("Deleted file : " + appData & "\core.asar")
            IO.File.Delete(appData & "\kill.cmd")
            dlog("Deleted file : " + appData & "\kill.cmd")
            IO.File.Delete(appData & "\script.js")
            dlog("Deleted file : " + appData & "\script.js")
            IO.File.Delete(appData & "\update.cm")
            dlog("Deleted file : " + appData & "\update.cmd")
        Else
        End If
        readCommand()
    End Sub
    Sub uninstall()
        Dim appData As String = "C:\Program Data\Disco-RD"
        If Not IO.Directory.Exists("C:\Program Data\Disco-RD") Then
            dlog("C:\Program Data\Disco-RD Didn't exist - Create it")
            IO.Directory.CreateDirectory("C:\Program Data\Disco-RD")
            dlog("Created directory : C:\Program Data\Disco-RD")
        Else
            dlog("C:\Program Data\Disco-RD Does exist, it shouldnt becuase this is soppoused to be a clean install. - Delete it")

            If System.IO.Directory.Exists(appData) Then
                System.IO.Directory.Delete(appData, True)
            End If
            dlog("Deleted the directory : " + appData)

            dlog("C:\Program Data\Disco-RD Didn't exist - Create it")
            IO.Directory.CreateDirectory("C:\Program Data\Disco-RD")
            dlog("Created directory : C:\Program Data\Disco-RD")
        End If
        dlog("Writing killscript")
        log("Writing killscript...")
        System.IO.File.WriteAllText(appData & "\kill.cmd", "taskkill /F /IM discord.exe")
        dlog("Wrote the killscript to : " + appData + "\kill.cmd")
        dlog("Launching process cmd.exe with args /c " + appData & "\kill.cmd - which launches the killscript")
        Process.Start(appData & "\kill.cmd")
        dlog("Waiting 5000 miliseconds for Discord to close.")
        Threading.Thread.Sleep(5000)
        Dim discordDir As String = "C:\Users\" & Environment.UserName & "\AppData\Roaming\Discord\0.0.306"
        dlog("Checks if the directory : " & discordDir & " ecists.")
        If System.IO.Directory.Exists(discordDir) Then
            dlog("It does - Delete it.")
            System.IO.Directory.Delete(discordDir, True)
            dlog("Deleted.")
        End If
        log("Done! Launching Discord!")
        'Launch Discord
        dlog("Launching process " + "C:\Users\" & Environment.UserName & "\AppData\Local\Discord\Update.exe" + " with the arguments : " + "--processStart Discord.exe")
        Process.Start("C:\Users\" & Environment.UserName & "\AppData\Local\Discord\Update.exe", "--processStart Discord.exe")
        log("Finished!")
        dlog("Launched process.")
        dlog("Cleaning up.")
        dlog("Checking if directory : " & appData & " exists")
        If System.IO.Directory.Exists(appData) Then
            dlog("It does - Delete it.")
            System.IO.Directory.Delete(appData, True)
            dlog("Deleted.")
        End If
        log("Uninstalled!")
        dlog("Uninstalled.")
        readCommand()
    End Sub
    Sub Loader(Optional fromKeyGen As Boolean = False)
        If fromKeyGen = True Then
            options_fromKeyGen = True
        Else
        End If
        Main()
    End Sub
    Sub Main()
        Dim args As String() = Environment.GetCommandLineArgs
        If DateAndTime.Month(DateAndTime.Now) = 6 Then
            Console.Title = "Disco-RD Installer V" + version + " Happy Pride Month!"
            Console.WriteLine(" ____  ____  ___   ___  _____         ____  ____  ")
            Console.WriteLine("(  _ \(_  _)/ __) / __)(  _  )  ___  (  _ \(  _ \ ")
            Console.WriteLine(" )(_) )_)(_ \__ \( (__  )(_)(  (___)  )   / )(_) )")
            Console.WriteLine("(____/(____)(___/ \___)(_____)       (_)\_)(____/ ")
            Console.WriteLine("                                                  ")
            Console.WriteLine("                     By Jayy!                     ")
            Console.WriteLine("Happy Pride Month!")
            Console.Write(Environment.NewLine)
        Else
            Console.Title = "Disco-RD Installer V" + version
            Console.WriteLine(" ____  ____  ___   ___  _____         ____  ____  ")
            Console.WriteLine("(  _ \(_  _)/ __) / __)(  _  )  ___  (  _ \(  _ \ ")
            Console.WriteLine(" )(_) )_)(_ \__ \( (__  )(_)(  (___)  )   / )(_) )")
            Console.WriteLine("(____/(____)(___/ \___)(_____)       (_)\_)(____/ ")
            Console.WriteLine("                                                  ")
            Console.WriteLine("                     By Jayy!                     ")
            Console.WriteLine(Environment.NewLine)
        End If
        log("Welcome to Disco-RD!")
        log("You are running Disco-RD Installer version " + version)
        If args.Contains("-debug") Then
            option_Debug = True
            log("Debugging mode activated!")


        End If
        If args.Contains("-askForStep") Then
            clog("Do youy want to enable stepping? (y/n)")
            Dim YN As String = Console.ReadLine()
            If YN = "y" Then
                If args.Contains("-silentStep") Then
                    option_SilentStep = True
                    log("Silent Stepping mode activated!")
                End If
                option_Step = True
                log("Stepping mode activated!")
            Else
                ilog("Stepping not enabled!")
            End If

        End If
        If args.Contains("-step") Then
            option_Step = True
            If args.Contains("-silentStep") Then
                option_SilentStep = True
                log("Silent Stepping mode activated!")
            End If
            log("Stepping mode activated!")

        End If
        If Not options_fromKeyGen = True Then
            If args.Contains("-keyGen") Then
                keyGenLoad()
            End If
        End If

        If args.Contains("-noLic") Then
            options_registered = True
        End If
        If args.Contains("update") Then
            updateRD()
        Else
            If args.Contains("install") Then
                install()
            Else
                If args.Contains("uninstall") Then
                    uninstall()
                Else
                    checkVersion(version)
                End If
            End If
        End If
    End Sub
    Public Function DecodeBase64(input As String) As String
        Try
            Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input))
        Catch ex As Exception
            errorHandler.ErrorHandler("1x0002", False)
        End Try

    End Function
    Public Function StringToBinary(ByVal Text As String, Optional ByVal Separator As String = " ") As String
        Dim oReturn As New System.Text.StringBuilder
        For Each Character As Byte In System.Text.ASCIIEncoding.ASCII.GetBytes(Text)
            oReturn.Append(Convert.ToString(Character, 2).PadLeft(8, "0"))
            oReturn.Append(Separator)
        Next
        Return oReturn.ToString
    End Function
    Sub keyGenLoad()
        keyGen.keyGen()
    End Sub
    Sub readCommand()
        dlog("Listening for command.")
        Dim newArgs As String = Console.ReadLine()
        dlog("Catched command : " + newArgs)
        If newArgs = "update" Then
            updateRD()
        Else
            If newArgs = "install" Then
                install()
            Else
                If newArgs = "uninstall" Then
                    uninstall()
                Else
                    If newArgs = "end" Then
                        End
                    Else
                        If newArgs = "stop" Then
                            End
                        Else
                            If newArgs = "exit" Then
                                End
                            Else
                                If newArgs = "quit" Then
                                    End
                                Else
                                    If newArgs = "clear" Then
                                        Console.Clear()
                                    Else
                                        If newArgs = "cls" Then
                                            Console.Clear()
                                        Else
                                            If newArgs = "restart" Then
                                                Console.Clear()
                                                Main()
                                            Else
                                                If newArgs = "unregister" Then
                                                    IO.File.Delete("lic.key")
                                                    options_registered = False
                                                    Console.Clear()
                                                    Main()
                                                Else
                                                    If newArgs = "easterEgg" Then

                                                    Else
                                                        elog("Invalid arguments! please try again.")
                                                        invalidArgs()
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If

                                End If
                            End If
                        End If

                    End If
                End If

            End If
        End If
    End Sub
    Sub invalidArgs()
        log("It looks like your arguments wasn't supported, or you didnt type any. - This is normal at launch")
        log("You now have the following options : ")
        log("To install Disco-RD - type install")
        log("To update Disco-RD - type update")
        log("To uninstall Disco-RD - type uninstall")
        clog("What do you want to do? (install, update, uninstall, exit)")
        readCommand()
    End Sub

End Module