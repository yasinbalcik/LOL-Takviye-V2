using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

internal static class ProcessCommandLine
{
  private static bool ReadStructFromProcessMemory<TStruct>(
    IntPtr hProcess,
    IntPtr lpBaseAddress,
    out TStruct val)
  {
    val = default (TStruct);
    int cb = Marshal.SizeOf<TStruct>();
    IntPtr num = Marshal.AllocHGlobal(cb);
    try
    {
      uint lpNumberOfBytesRead;
      if (ProcessCommandLine.Win32Native.ReadProcessMemory(hProcess, lpBaseAddress, num, (uint) cb, out lpNumberOfBytesRead) && (long) lpNumberOfBytesRead == (long) cb)
      {
        val = Marshal.PtrToStructure<TStruct>(num);
        return true;
      }
    }
    finally
    {
      Marshal.FreeHGlobal(num);
    }
    return false;
  }

  public static string ErrorToString(int error) => new string[7]
  {
    "Başarılı",
    "Okuma için işlem açılamadı",
    "İşlem bilgileri sorgulanamadı",
    "PEB adres bulunamadı",
    "PEB bilgileri okunamadı",
    "İşlem parametreleri okunamadı",
    "İşlemden parametre okunamadı"
  }[Math.Abs(error)];

  public static int Retrieve(
    Process process,
    out string parameterValue,
    ProcessCommandLine.Parameter parameter = ProcessCommandLine.Parameter.CommandLine)
  {
    int rc = 0;
    parameterValue = (string) null;
    IntPtr hProcess = ProcessCommandLine.Win32Native.OpenProcess(ProcessCommandLine.Win32Native.OpenProcessDesiredAccessFlags.PROCESS_VM_READ | ProcessCommandLine.Win32Native.OpenProcessDesiredAccessFlags.PROCESS_QUERY_INFORMATION, false, (uint) process.Id);
    if (hProcess != IntPtr.Zero)
    {
      try
      {
        int cb = Marshal.SizeOf<ProcessCommandLine.Win32Native.ProcessBasicInformation>();
        IntPtr num = Marshal.AllocHGlobal(cb);
        try
        {
          uint len;
          if (ProcessCommandLine.Win32Native.NtQueryInformationProcess(hProcess, 0U, num, (uint) cb, out len) == 0U)
          {
            ProcessCommandLine.Win32Native.ProcessBasicInformation structure = Marshal.PtrToStructure<ProcessCommandLine.Win32Native.ProcessBasicInformation>(num);
            if (structure.PebBaseAddress != IntPtr.Zero)
            {
              ProcessCommandLine.Win32Native.PEB val1;
              if (ProcessCommandLine.ReadStructFromProcessMemory<ProcessCommandLine.Win32Native.PEB>(hProcess, structure.PebBaseAddress, out val1))
              {
                ProcessCommandLine.Win32Native.RtlUserProcessParameters val2;
                if (ProcessCommandLine.ReadStructFromProcessMemory<ProcessCommandLine.Win32Native.RtlUserProcessParameters>(hProcess, val1.ProcessParameters, out val2))
                {
                  switch (parameter)
                  {
                    case ProcessCommandLine.Parameter.CommandLine:
                      parameterValue = ReadUnicodeString(val2.CommandLine);
                      break;
                    case ProcessCommandLine.Parameter.WorkingDirectory:
                      parameterValue = ReadUnicodeString(val2.CurrentDirectory);
                      break;
                  }
                }
                else
                  rc = -5;
              }
              else
                rc = -4;
            }
            else
              rc = -3;
          }
          else
            rc = -2;

          string ReadUnicodeString(
            ProcessCommandLine.Win32Native.UnicodeString unicodeString)
          {
            ushort maximumLength = unicodeString.MaximumLength;
            IntPtr numara = Marshal.AllocHGlobal((int) maximumLength);
            try
            {
              if (ProcessCommandLine.Win32Native.ReadProcessMemory(hProcess, unicodeString.Buffer, numara, (uint) maximumLength, out len))
              {
                rc = 0;
                return Marshal.PtrToStringUni(numara);
              }
              rc = -6;
            }
            finally
            {
              Marshal.FreeHGlobal(numara);
            }
            return (string) null;
          }
        }
        finally
        {
          Marshal.FreeHGlobal(num);
        }
      }
      finally
      {
        ProcessCommandLine.Win32Native.CloseHandle(hProcess);
      }
    }
    else
      rc = -1;
    return rc;
  }

  public static IReadOnlyList<string> CommandLineToArgs(string commandLine)
  {
    if (string.IsNullOrEmpty(commandLine))
      return (IReadOnlyList<string>) Array.Empty<string>();
    int pNumArgs;
    IntPtr argv = ProcessCommandLine.Win32Native.CommandLineToArgv(commandLine, out pNumArgs);
    if (argv == IntPtr.Zero)
      throw new Win32Exception(Marshal.GetLastWin32Error());
    try
    {
      string[] strArray = new string[pNumArgs];
      for (int index = 0; index < strArray.Length; ++index)
      {
        IntPtr ptr = Marshal.ReadIntPtr(argv, index * IntPtr.Size);
        strArray[index] = Marshal.PtrToStringUni(ptr);
      }
      return (IReadOnlyList<string>) ((IEnumerable<string>) strArray).ToList<string>().AsReadOnly();
    }
    finally
    {
      Marshal.FreeHGlobal(argv);
    }
  }

  private static class Win32Native
  {
    public const uint PROCESS_BASIC_INFORMATION = 0;

    [DllImport("ntdll.dll")]
    public static extern uint NtQueryInformationProcess(
      IntPtr ProcessHandle,
      uint ProcessInformationClass,
      IntPtr ProcessInformation,
      uint ProcessInformationLength,
      out uint ReturnLength);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(
      ProcessCommandLine.Win32Native.OpenProcessDesiredAccessFlags dwDesiredAccess,
      [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
      uint dwProcessId);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReadProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      IntPtr lpBuffer,
      uint nSize,
      out uint lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("shell32.dll", EntryPoint = "CommandLineToArgvW", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr CommandLineToArgv(string lpCmdLine, out int pNumArgs);

    [Flags]
    public enum OpenProcessDesiredAccessFlags : uint
    {
      PROCESS_VM_READ = 16, // 0x00000010
      PROCESS_QUERY_INFORMATION = 1024, // 0x00000400
    }

    public struct ProcessBasicInformation
    {
      public IntPtr Reserved1;
      public IntPtr PebBaseAddress;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
      public IntPtr[] Reserved2;
      public IntPtr UniqueProcessId;
      public IntPtr Reserved3;
    }

    public struct UnicodeString
    {
      public ushort Length;
      public ushort MaximumLength;
      public IntPtr Buffer;
    }

    public struct PEB
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public IntPtr[] Reserved;
      public IntPtr ProcessParameters;
    }

    public struct RtlUserProcessParameters
    {
      public uint MaximumLength;
      public uint Length;
      public uint Flags;
      public uint DebugFlags;
      public IntPtr ConsoleHandle;
      public uint ConsoleFlags;
      public IntPtr StandardInput;
      public IntPtr StandardOutput;
      public IntPtr StandardError;
      public ProcessCommandLine.Win32Native.UnicodeString CurrentDirectory;
      public IntPtr CurrentDirectoryHandle;
      public ProcessCommandLine.Win32Native.UnicodeString DllPath;
      public ProcessCommandLine.Win32Native.UnicodeString ImagePathName;
      public ProcessCommandLine.Win32Native.UnicodeString CommandLine;
    }
  }

  public enum Parameter
  {
    CommandLine,
    WorkingDirectory,
  }
}
