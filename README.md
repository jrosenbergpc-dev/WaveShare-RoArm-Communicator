WaveShare RoArm Communicator Windows Service Project
===============================================================

A Windows OS Service designed to operate as a webhook server that accepts commands to send instructions to a RoArm Robot using a custom file format.
Typically, RoArm devices have mission files that can be used to automate several actions by calling a simple command.

However, there seem to be some known issues with the standard built-in mission files on the ESP32 firmware, and I found this to be an easier method to automate the arms.

Webhook Service Request
=================================
The webhook service accepts commands at the following URL path:
http://0.0.0.0:5001/roarm/mission/start

The webhook service requires headers to be sent along, passing the API Key, 
which can be configured in `Common/WebhookBase.cs`.

Headers must be in JSON Format and include the following key-value pair:

```json
{"x-api-key": "YOUR_API_KEY"}
```

The webhook service requires data to be passed along in the BODY like so:

```json
{
  "`ip`": "xxx.xxx.xxx.xxx",
  "`mission`": "my_mission"
}
```

> [!IMPORTANT]
> DO NOT INCLUDE THE FILE EXTENSION for the `mission` variable!!


Mission Files
==============================

All mission files are stored in the mission folder within the Service Installation Path!

Default Install Path = `Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\WaveShare\\Communicator";`

The service will look for the mission files being requested automatically within the following path:

`Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\WaveShare\\Communicator\\missions";`

Example Mission File Format
=====================================
A mission file (e.g., `my_mission.mi`) should typically look like this:

```json
{"T":101,"joint":1,"rad":1.5751292519943295,"spd":0.0,"acc":0.0}@250
{"T":101,"joint":3,"rad":0.0,"spd":0.0,"acc":0.0}@250
{"T":101,"joint":2,"rad":1.5730382858376184,"spd":0.0,"acc":0.0}@250
```

> [!NOTE]
> Note the portion at the end of each JSON command line which starts with `@`, the symbol designates a time interval in milliseconds.
> 
> For example, `@250` is asking the service to wait 250 milliseconds before running the next line.

If you run into issues or need assistance understanding the use of this project, please feel free to contact me!

Happy Coding ^_^!
