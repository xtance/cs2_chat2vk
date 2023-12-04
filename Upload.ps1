# Load WinSCP .NET assembly
Add-Type -Path "WinSCPnet.dll"

# Set up session options

# Set up session options
$sessionOptions = New-Object WinSCP.SessionOptions -Property @{
    Protocol = [WinSCP.Protocol]::Sftp
    HostName = "192.168.0.103"
    PortNumber = 22
    UserName = "cs"
    Password = "cs"
    SshHostKeyFingerprint = "ssh-ed25519 255 OVKzGTdNbv/xuXsvjtXXTebSY1OlTIT8EcmkIXlSDj0"
}

$session = New-Object WinSCP.Session

try
{
    # Connect
    $session.Open($sessionOptions)

	$localFolder = "S:\CS2\cs2_chat2vk\bin\Debug\net7.0\publish\*"
	$remoteFolder = "/home/cs/server/game/csgo/addons/counterstrikesharp/plugins/cs2_chat2vk/*"

    # Server
    $session.PutFiles($localFolder, $remoteFolder).Check()
	#$session.ExecuteCommand("(cd $serverRemote && npm ci)").Check()

}
finally
{
    $session.Dispose()
}