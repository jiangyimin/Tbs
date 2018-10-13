cd %~dp0   
copy ZAZAPIt.dll %windir%\system32 
copy eAlgDLL.dll %windir%\system32 
copy ARTH_DLL.dll %windir%\system32 
copy ZAZFingerActivexT.ocx %windir%\system32 
regsvr32 %windir%\system32\ZAZFingerActivexT.ocx  

//copy /y ZAZAPIt.dll C:\Windows\SysWOW64
//copy /y eAlgDLL.dll C:\Windows\SysWOW64
//copy /y ARTH_DLL.dll C:\Windows\SysWOW64 
//copy ZAZFingerActivexT.ocx C:\Windows\SysWOW64   
//regsvr32 %windir%\SysWOW64\ZAZFingerActivexT.ocx 
exit