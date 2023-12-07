
# Syncy
A tool to periodically interact remotely with browsers using cloud-sync.

## Setup:

Compile it yourself. I reccomend VS19 as it will compile for default Windows.Net

## How to use:
Run Syncy and start whatever browsers you want to enroll in sync:
```
.\Syncy.exe Start All
```

Then sign into your sync accounts. Once this is done, wrap up these sync files via Syncy Zip:
```
.\Syncy.exe Zip All
```

Then grab the corresponding Zip files, and bring them, the Extension folder, and a copy of Syncy to the target device.
Then use Syncy to unzip these profiles:
```
.\Syncy.exe Unzip All
```

Then to check that the profiles deployed correctly, run Syncy Start again. 
```
.\Syncy.exe Start All
```

From this point, Firefox will work out of the box, but Chrome and Edge will need to reauthenticate, I'm yet to fix this issue unfortunately.
If you want to embed the test extension, this should be done now as well.

Then, all you need to do is set Syncy to loop and it will open once a minute, these numbers can currently only be changed at compile time, but I will be making this dynamic soon.
```
.\Syncy.exe Loop All
```

From this you can test all the techniques from the whitepaper, the link to which will be added here once the whitepaper is published.

To kill Syncy, submit a start URL in any browser with "SUPERSECRETKILLCODE" anywhere in it. This can also be changed at compile time.

## Disclaimer
This tool is built for automated testing of browser sync.
