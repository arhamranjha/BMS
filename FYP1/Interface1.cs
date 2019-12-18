
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
[ComImport()]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("BD39D1D2-BA2F-486A-89B0-B4B0CB466891")]
interface ICLRRuntimeInfo
{
    void xGetVersionString();
    void xGetRuntimeDirectory();
    void xIsLoaded();
    void xIsLoadable();
    void xLoadErrorString();
    void xLoadLibrary();
    void xGetProcAddress();
    void xGetInterface();
    void xSetDefaultStartupFlags();
    void xGetDefaultStartupFlags();
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BindAsLegacyV2Runtime();
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
