cd %~dp0  
copy /y ZAZAPIt.dll C:\Windows\SysWOW64
copy /y eAlgDLL.dll C:\Windows\SysWOW64
copy /y ARTH_DLL.dll C:\Windows\SysWOW64 
copy ZAZFingerActivexT.ocx C:\Windows\SysWOW64   
regsvr32 %windir%\SysWOW64\ZAZFingerActivexT.ocx 
exit