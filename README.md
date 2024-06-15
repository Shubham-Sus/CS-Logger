# CS-Logger

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A simple, thread-safe logger utility class for C# applications.

## Features 
- Logs messages of various levels: Trace, Info, Debug, Warning, Error, Fatal
- Thread-safe logging to ensure consistency in multi-threaded applications
- Customizable log file directory, name, and date-time format, with an option to clear existing log
- Includes calling file name and method name in log entries

## Installation
Simply copy the [Log.cs](CSLogger/Log.cs) file into your project.

## Usage/Examples

### Initialization
To start logging, initialize the logger with the desired log file directory, **optional** log file name, date-time format, and the option to clear existing log.
```cs
using CSLogger;

// Initialize the logger
Log.Start(Environment.CurrentDirectory);
// OR
Log.Start(Environment.CurrentDirectory, "AppLog", "yyyy-MM-dd HH:mm:ss.fff", true);
```

### Logging Messages
You can log messages of different levels using the provided methods.
```cs
Log.Trace("This is a trace message.");
Log.Info("This is an info message.");
Log.Debug("This is a debug message.");
Log.Warn("This is a warning message.");
Log.Error("This is an error message.");
Log.Fatal("This is a fatal error message.");
```

### Example
Here's a complete example of how to use the logger in your application:
```cs
using System;
using CSLogger;

class Program
{
    static void Main(string[] args)
    {
        Log.Start(Environment.CurrentDirectory);
        // OR
        Log.Start(Environment.CurrentDirectory, "AppLog", "yyyy-MM-dd HH:mm:ss.fff", true);

        Log.Trace("This is a trace message.");
        Log.Info("This is an info message.");
        Log.Debug("This is a debug message.");
        Log.Warn("This is a warning message.");
        Log.Error("This is an error message.");
        Log.Fatal("This is a fatal error message.");

        Console.WriteLine("Logging complete. Check the log file in the 'logs' directory.");
    }
}
```
### Log Output
Here is an example of what the log entries will look like:
```
2024-06-07 16:03:19.658 [INFO] (Log.Start) Log Created.
2024-06-07 16:03:19.668 [TRACE] (Program.Main) This is a trace message.
2024-06-07 16:03:19.810 [INFO] (Program.Main) This is an info message.
2024-06-07 16:03:19.832 [DEBUG] (Program.Main) This is a debug message.
2024-06-07 16:03:19.852 [WARNING] (Program.Main) This is a warning message.
2024-06-07 16:03:20.019 [ERROR] (Program.Main) This is an error message.
2024-06-07 16:03:20.020 [FATAL] (Program.Main) This is a fatal error message.
```
