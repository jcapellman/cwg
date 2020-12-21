[System.Console]::ForegroundColor = [System.ConsoleColor]::White;
[System.Console]::BackgroundColor = [System.ConsoleColor]::Red;

[System.Console]::Clear();

Start-Sleep -s 1;

Write-Output "Owned by CWG at $(Get-Date)";

$strComputer = "."

$colItems = get-wmiobject -class "Win32_NetworkAdapterConfiguration"  -computername $strComputer | Where{$_.IpEnabled -Match "True"}

[console]::WriteLine("$env:UserName I'm getting them MACs");

foreach ($objItem in $colItems) {
	[console]::WriteLine($objItem.MACAddress);
	[console]::WriteLine($objItem.IPAddress);	
}

[console]::WriteLine("Parsing all of your Windows Services for some exp0it3...");

[console]::WriteLine("While you wait, here's a tune...");

[console]::beep(440,500) 
[console]::beep(440,500) 
[console]::beep(440,500) 
[console]::beep(349,350) 
[console]::beep(523,150) 
[console]::beep(440,500) 
[console]::beep(349,350) 
[console]::beep(523,150) 
[console]::beep(440,1000) 
[console]::beep(659,500) 
[console]::beep(659,500) 
[console]::beep(659,500) 
[console]::beep(698,350) 
[console]::beep(523,150) 
[console]::beep(415,500) 
[console]::beep(349,350) 
[console]::beep(523,150) 
[console]::beep(440,1000)

$countServices = (Get-Service  | Where Status -eq "Running").Length;

[console]::WriteLine("$countServices exp0oitable servies found...");

$path = "c:\temp";
$source = "cwgpsdump";

If (![System.IO.Directory]::Exists($path))
{
	[System.IO.Directory]::CreateDirectory($path);      
}

$client = New-Object System.Net.WebClient;

Start-Sleep -s 1;

For ($x = 0; $x -lt 25; $x++) {
	$rand = New-Object System.Random;

	$value = $rand.Next(0, 4);

	If ($value -eq 0) {
		[System.Console]::ForegroundColor = [System.ConsoleColor]::White;
		[System.Console]::BackgroundColor = [System.ConsoleColor]::Red;
	}
	
	If ($value -eq 1) {
		[System.Console]::ForegroundColor = [System.ConsoleColor]::White;
		[System.Console]::BackgroundColor = [System.ConsoleColor]::Blue;
	}
	
	If ($value -eq 2) {
		[System.Console]::ForegroundColor = [System.ConsoleColor]::White;	
		[System.Console]::BackgroundColor = [System.ConsoleColor]::Green;
	}

	If ($value -eq 3) {
		[System.Console]::ForegroundColor = [System.ConsoleColor]::White;	
		[System.Console]::BackgroundColor = [System.ConsoleColor]::Yellow;
	}

	[System.Console]::Clear();

	$downloadPath = "$path\$source$x";

	$client.DownloadFile("https://cwg.io/", $downloadPath);	
	Add-Content $downloadPath $encrypted;

	[console]::WriteLine("0wning your $path folder again and again $env:UserName with bytes to $downloadPath...");

	[System.IO.File]::Delete($downloadPath);	

	Start-Sleep -m 50;
}

[console]::WriteLine("Attempting to own your registry $env:UserName...");

$registryPath = "HKCU:\Software\CWG\0wned"

$name = "0wn3dDate"

$value = $(Get-Date)

IF (!(Test-Path $registryPath)) {
	New-Item -Path $registryPath -Force | Out-Null

    New-ItemProperty -Path $registryPath -Name $name -Value $value -PropertyType DWORD -Force | Out-Null
} ELSE {
    New-ItemProperty -Path $registryPath -Name $name -Value $value -PropertyType DWORD -Force | Out-Null
}

[console]::WriteLine("Registry 0wn3d $env:UserName...");

$placeholder;

$timestamp;

[System.Console]::ForegroundColor = [System.ConsoleColor]::White;	
[System.Console]::BackgroundColor = [System.ConsoleColor]::Blue;

[System.Console]::Clear();

[console]::WriteLine("Totally 0wn3d");