﻿//	Copyright (c) 2012, Michael Kunz. All rights reserved.
//	http://kunzmi.github.io/managedCuda
//
//	This file is part of ManagedCuda.
//
//	ManagedCuda is free software: you can redistribute it and/or modify
//	it under the terms of the GNU Lesser General Public License as 
//	published by the Free Software Foundation, either version 2.1 of the 
//	License, or (at your option) any later version.
//
//	ManagedCuda is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//	GNU Lesser General Public License for more details.
//
//	You should have received a copy of the GNU Lesser General Public
//	License along with this library; if not, write to the Free Software
//	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
//	MA 02110-1301  USA, http://www.gnu.org/licenses/.


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ManagedCuda.BasicTypes;
using ManagedCuda.VectorTypes;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

namespace ManagedCuda
{
    /// <summary>
    /// C# wrapper for the NVIDIA CUDA Driver API (--> cuda.h)
    /// </summary>
    public static class DriverAPINativeMethods
    {
        internal const string CUDA_DRIVER_API_DLL_NAME = "nvcuda";
        internal const string CUDA_OBSOLET_9_2 = "Don't use this CUDA API call with CUDA version >= 9.2.";
        internal const string CUDA_OBSOLET_11 = "Don't use this CUDA API call with CUDA version >= 11";

#if (NETCOREAPP)
        internal const string CUDA_DRIVER_API_DLL_NAME_LINUX = "libcuda";

        static DriverAPINativeMethods()
        {
            NativeLibrary.SetDllImportResolver(typeof(DriverAPINativeMethods).Assembly, ImportResolver);
        }

        private static IntPtr ImportResolver(string libraryName, System.Reflection.Assembly assembly, DllImportSearchPath? searchPath)
        {
            IntPtr libHandle = IntPtr.Zero;

            if (libraryName == CUDA_DRIVER_API_DLL_NAME)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    NativeLibrary.TryLoad(CUDA_DRIVER_API_DLL_NAME_LINUX, assembly, DllImportSearchPath.SafeDirectories, out libHandle);
                }
            }
            //On Windows, use the default library name
            return libHandle;
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        internal static void Init()
        {
            //Need that to have the constructor called before any library call.
        }
#endif

        //Per thread default stream appendices
#if _PerThreadDefaultStream
		internal const string CUDA_PTDS = "_ptds";
		internal const string CUDA_PTSZ = "_ptsz";
#else
        internal const string CUDA_PTDS = "";
        internal const string CUDA_PTSZ = "";
#endif

        /// <summary>
        /// Gives the version of the wrapped api
        /// </summary>
        public static Version Version
        {
            get { return new Version(11, 4); }
        }

        #region Initialization
        /// <summary>
        /// Initializes the driver API and must be called before any other function from the driver API. Currently, 
        /// the Flags parameter must be <see cref="CUInitializationFlags.None"/>. If <see cref="cuInit"/> has not been called, any function from the driver API will return 
        /// <see cref="CUResult.ErrorNotInitialized"/>.
        /// </summary>
        /// <remarks>Before any call to the CUDA Driver API can be done, the API must be initialized with cuInit(0).</remarks>
        /// <param name="Flags">Currently, Flags must always be <see cref="CUInitializationFlags.None"/>.</param>
        /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.<remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
        [DllImport(CUDA_DRIVER_API_DLL_NAME)]
        public static extern CUResult cuInit(CUInitializationFlags Flags);
        #endregion

        #region Driver Version Query
        /// <summary>
        /// Returns in <c>driverVersion</c> the version number of the installed CUDA driver. This function automatically returns
        /// <see cref="CUResult.ErrorInvalidValue"/> if the driverVersion argument is NULL.
        /// </summary>
        /// <param name="driverVersion">Returns the CUDA driver version</param>
        /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>.<remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
        [DllImport(CUDA_DRIVER_API_DLL_NAME)]
        public static extern CUResult cuDriverGetVersion(ref int driverVersion);
        #endregion

        #region Device management
        /// <summary>
        /// Combines all API calls for device management
        /// </summary>
        public static class DeviceManagement
        {
#if (NETCOREAPP)
            static DeviceManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif

            /// <summary>
            /// Returns in <c>device</c> a device handle given an ordinal in the range [0, <see cref="cuDeviceGetCount"/>-1].
            /// </summary>
            /// <param name="device">Returned device handle</param>
            /// <param name="ordinal">Device number to get handle for</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGet(ref CUdevice device, int ordinal);

            /// <summary>
            /// Returns in <c>count</c> the number of devices with compute capability greater than or equal to 2.0 that are available for
            /// execution. If there is no such device, <see cref="cuDeviceGetCount"/> returns 0.
            /// </summary>
            /// <param name="count">Returned number of compute-capable devices</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetCount(ref int count);

            /// <summary>
            /// Returns an ASCII string identifying the device <c>dev</c> in the NULL-terminated string pointed to by name. <c>len</c> specifies
            /// the maximum length of the string that may be returned.
            /// </summary>
            /// <param name="name">Returned identifier string for the device</param>
            /// <param name="len">Maximum length of string to store in <c>name</c></param>
            /// <param name="dev">Device to get identifier string for</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetName([Out] byte[] name, int len, CUdevice dev);

            /// <summary>
            /// Return an UUID for the device<para/>
            /// Returns 16-octets identifing the device \p dev in the structure pointed by the \p uuid.
            /// </summary>
            /// <param name="uuid">Returned UUID</param>
            /// <param name="dev">Device to get identifier string for</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetUuid(ref CUuuid uuid, CUdevice dev);

            /// <summary>
            /// Return an UUID for the device (11.4+)<para/>
            /// Returns 16-octets identifing the device \p dev in the structure
            /// pointed by the \p uuid.If the device is in MIG mode, returns its
            /// MIG UUID which uniquely identifies the subscribed MIG compute instance.
            /// Returns 16-octets identifing the device \p dev in the structure pointed by the \p uuid.
            /// </summary>
            /// <param name="uuid">Returned UUID</param>
            /// <param name="dev">Device to get identifier string for</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetUuid_v2(ref CUuuid uuid, CUdevice dev);

            /// <summary>
            /// Return an LUID and device node mask for the device. <para/>
            /// Return identifying information (\p luid and \p deviceNodeMask) to allow
            /// matching device with graphics APIs.
            /// </summary>
            /// <param name="luid">Returned LUID</param>
            /// <param name="deviceNodeMask">Returned device node mask</param>
            /// <param name="dev">Device to get identifier string for</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetLuid(ref Luid luid, ref uint deviceNodeMask, CUdevice dev);


            /// <summary>
            /// Returns in <c>bytes</c> the total amount of memory available on the device <c>dev</c> in bytes.
            /// </summary>
            /// <param name="bytes">Returned memory available on device in bytes</param>
            /// <param name="dev">Device handle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceTotalMem_v2(ref SizeT bytes, CUdevice dev);

            /// <summary>
            /// Returns the maximum number of elements allocatable in a 1D linear texture for a given texture element size.
            /// Returns in \p maxWidthInElements the maximum number of texture elements allocatable in a 1D linear texture
            /// for given \p format and \p numChannels.
            /// </summary>
            /// <param name="maxWidthInElements">Returned maximum number of texture elements allocatable for given \p format and \p numChannels.</param>
            /// <param name="format">Texture format.</param>
            /// <param name="numChannels">Number of channels per texture element.</param>
            /// <param name="dev">Device handle.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetTexture1DLinearMaxWidth(ref SizeT maxWidthInElements, CUArrayFormat format, uint numChannels, CUdevice dev);

            /// <summary>
            /// Returns in <c>pi</c> the integer value of the attribute <c>attrib</c> on device <c>dev</c>. See <see cref="CUDeviceAttribute"/>.
            /// </summary>
            /// <param name="pi">Returned device attribute value</param>
            /// <param name="attrib">Device attribute to query</param>
            /// <param name="dev">Device handle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetAttribute(ref int pi, CUDeviceAttribute attrib, CUdevice dev);

            /// <summary>
            /// Return NvSciSync attributes that this device can support.<para/>
            /// Returns in \p nvSciSyncAttrList, the properties of NvSciSync that
            /// this CUDA device, \p dev can support.The returned \p nvSciSyncAttrList
            /// can be used to create an NvSciSync object that matches this deviceâ€™s capabilities.
            /// <para/>
            /// If NvSciSyncAttrKey_RequiredPerm field in \p nvSciSyncAttrList is
            /// already set this API will return ::CUDA_ERROR_INVALID_VALUE.
            /// <para/>
            /// The applications should set \p nvSciSyncAttrList to a valid
            /// NvSciSyncAttrList failing which this API will return
            /// ::CUDA_ERROR_INVALID_HANDLE.
            /// <para/>
            /// The \p flags controls how applications intends to use
            /// the NvSciSync created from the \p nvSciSyncAttrList. The valid flags are:
            /// - ::CUDA_NVSCISYNC_ATTR_SIGNAL, specifies that the applications intends to 
            /// signal an NvSciSync on this CUDA device.
            /// - ::CUDA_NVSCISYNC_ATTR_WAIT, specifies that the applications intends to 
            /// wait on an NvSciSync on this CUDA device.
            /// <para/>
            /// At least one of these flags must be set, failing which the API
            /// returns::CUDA_ERROR_INVALID_VALUE.Both the flags are orthogonal
            /// to one another: a developer may set both these flags that allows to
            /// set both wait and signal specific attributes in the same \p nvSciSyncAttrList.
            /// </summary>
            /// <param name="nvSciSyncAttrList">Return NvSciSync attributes supported</param>
            /// <param name="dev">Valid Cuda Device to get NvSciSync attributes for.</param>
            /// <param name="flags">flags describing NvSciSync usage.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetNvSciSyncAttributes(IntPtr nvSciSyncAttrList, CUdevice dev, NvSciSyncAttr flags);

            /// <summary>
            /// Sets the current memory pool of a device<para/>
            /// The memory pool must be local to the specified device.
            /// ::cuMemAllocAsync allocates from the current mempool of the provided stream's device.
            /// By default, a device's current memory pool is its default memory pool.
            /// <para/>
            /// note Use ::cuMemAllocFromPoolAsync to specify asynchronous allocations from a device different than the one the stream runs on.
            /// </summary>
            /// <param name="dev"></param>
            /// <param name="pool"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceSetMemPool(CUdevice dev, CUmemoryPool pool);

            /// <summary>
            /// Gets the current mempool for a device<para/>
            /// Returns the last pool provided to ::cuDeviceSetMemPool for this device
            /// or the device's default memory pool if ::cuDeviceSetMemPool has never been called.
            /// By default the current mempool is the default mempool for a device.
            /// Otherwise the returned pool must have been set with::cuDeviceSetMemPool.
            /// </summary>
            /// <param name="pool"></param>
            /// <param name="dev"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetMemPool(ref CUmemoryPool pool, CUdevice dev);

            /// <summary>
            /// Returns the default mempool of a device<para/>
            /// The default mempool of a device contains device memory from that device.
            /// </summary>
            /// <param name="pool_out"></param>
            /// <param name="dev"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetDefaultMemPool(ref CUmemoryPool pool_out, CUdevice dev);

            #region Missing from 4.1
            /// <summary>
            /// Returns in <c>device</c> a device handle given a PCI bus ID string.
            /// </summary>
            /// <param name="dev">Returned device handle</param>
            /// <param name="pciBusId">String in one of the following forms: <para/>
            /// [domain]:[bus]:[device].[function]<para/>
            /// [domain]:[bus]:[device]<para/>
            /// [bus]:[device].[function]<para/>
            /// where domain, bus, device, and function are all hexadecimal values</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetByPCIBusId(ref CUdevice dev, [In, Out] byte[] pciBusId);

            /// <summary>
            /// Returns an ASCII string identifying the device <c>dev</c> in the NULL-terminated
            /// string pointed to by <c>pciBusId</c>. <c>len</c> specifies the maximum length of the
            /// string that may be returned.
            /// </summary>
            /// <param name="pciBusId">Returned identifier string for the device in the following format
            /// [domain]:[bus]:[device].[function]<para/>
            /// where domain, bus, device, and function are all hexadecimal values.<para/>
            /// pciBusId should be large enough to store 13 characters including the NULL-terminator.</param>
            /// <param name="len">Maximum length of string to store in <c>name</c></param>
            /// <param name="dev">Device to get identifier string for</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetPCIBusId([In, Out] byte[] pciBusId, int len, CUdevice dev);

            /// <summary>
            /// Takes as input a previously allocated event. This event must have been 
            /// created with the ::CU_EVENT_INTERPROCESS and ::CU_EVENT_DISABLE_TIMING 
            /// flags set. This opaque handle may be copied into other processes and
            /// opened with ::cuIpcOpenEventHandle to allow efficient hardware
            /// synchronization between GPU work in different processes.
            /// <para/>
            /// After the event has been been opened in the importing process, 
            /// ::cuEventRecord, ::cuEventSynchronize, ::cuStreamWaitEvent and 
            /// ::cuEventQuery may be used in either process. Performing operations 
            /// on the imported event after the exported event has been freed 
            /// with ::cuEventDestroy will result in undefined behavior.
            /// <para/>
            /// IPC functionality is restricted to devices with support for unified 
            /// addressing on Linux operating systems.
            /// </summary>
            /// <param name="pHandle">Pointer to a user allocated CUipcEventHandle in which to return the opaque event handle</param>
            /// <param name="cuevent">Event allocated with ::CU_EVENT_INTERPROCESS and  ::CU_EVENT_DISABLE_TIMING flags.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorMapFailed"/></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuIpcGetEventHandle(ref CUipcEventHandle pHandle, CUevent cuevent);

            /// <summary>
            /// Opens an interprocess event handle exported from another process with 
            /// ::cuIpcGetEventHandle. This function returns a ::CUevent that behaves like 
            /// a locally created event with the ::CU_EVENT_DISABLE_TIMING flag specified. 
            /// This event must be freed with ::cuEventDestroy.
            /// <para/>
            /// Performing operations on the imported event after the exported event has 
            /// been freed with ::cuEventDestroy will result in undefined behavior.
            /// <para/>
            /// IPC functionality is restricted to devices with support for unified 
            /// addressing on Linux operating systems.
            /// </summary>
            /// <param name="phEvent">Returns the imported event</param>
            /// <param name="handle">Interprocess handle to open</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorMapFailed"/></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuIpcOpenEventHandle(ref CUevent phEvent, CUipcEventHandle handle);

            /// <summary>
            /// Takes a pointer to the base of an existing device memory allocation created 
            /// with ::cuMemAlloc and exports it for use in another process. This is a 
            /// lightweight operation and may be called multiple times on an allocation
            /// without adverse effects. 
            /// <para/>
            /// If a region of memory is freed with ::cuMemFree and a subsequent call
            /// to ::cuMemAlloc returns memory with the same device address,
            /// ::cuIpcGetMemHandle will return a unique handle for the
            ///  new memory. 
            /// <para/>
            /// IPC functionality is restricted to devices with support for unified 
            /// addressing on Linux operating systems.
            /// </summary>
            /// <param name="pHandle">Pointer to user allocated ::CUipcMemHandle to return the handle in.</param>
            /// <param name="dptr">Base pointer to previously allocated device memory </param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorMapFailed"/></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuIpcGetMemHandle(ref CUipcMemHandle pHandle, CUdeviceptr dptr);

            /// <summary>
            /// Maps memory exported from another process with ::cuIpcGetMemHandle into
            /// the current device address space. For contexts on different devices 
            /// ::cuIpcOpenMemHandle can attempt to enable peer access between the
            /// devices as if the user called ::cuCtxEnablePeerAccess. This behavior is 
            /// controlled by the ::CU_IPC_MEM_LAZY_ENABLE_PEER_ACCESS flag. 
            /// ::cuDeviceCanAccessPeer can determine if a mapping is possible.
            /// <para/>
            /// Contexts that may open ::CUipcMemHandles are restricted in the following way.
            /// ::CUipcMemHandles from each ::CUdevice in a given process may only be opened 
            /// by one ::CUcontext per ::CUdevice per other process.
            /// <para/>
            /// Memory returned from ::cuIpcOpenMemHandle must be freed with
            /// ::cuIpcCloseMemHandle.
            /// <para/>
            /// Calling ::cuMemFree on an exported memory region before calling
            /// ::cuIpcCloseMemHandle in the importing context will result in undefined
            /// behavior.
            /// <para/>
            /// IPC functionality is restricted to devices with support for unified 
            /// addressing on Linux operating systems.
            /// </summary>
            /// <param name="pdptr">Returned device pointer</param>
            /// <param name="handle">::CUipcMemHandle to open</param>
            /// <param name="Flags">Flags for this operation. Must be specified as ::CU_IPC_MEM_LAZY_ENABLE_PEER_ACCESS</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidHandle"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorMapFailed"/>, <see cref="CUResult.ErrorTooManyPeers"/></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuIpcOpenMemHandle_v2")]

            public static extern CUResult cuIpcOpenMemHandle(ref CUdeviceptr pdptr, CUipcMemHandle handle, uint Flags);

            /// <summary>
            /// Unmaps memory returnd by ::cuIpcOpenMemHandle. The original allocation
            /// in the exporting process as well as imported mappings in other processes
            /// will be unaffected.
            /// <para/>
            /// Any resources used to enable peer access will be freed if this is the
            /// last mapping using them.
            /// <para/>
            /// IPC functionality is restricted to devices with support for unified 
            ///  addressing on Linux operating systems.
            /// </summary>
            /// <param name="dptr">Device pointer returned by ::cuIpcOpenMemHandle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidHandle"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorMapFailed"/></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuIpcCloseMemHandle(CUdeviceptr dptr);

            #endregion

            /// <summary>
            /// Returns information about the execution affinity support of the device.<para/>
            /// Returns in \p *pi whether execution affinity type \p type is supported by device \p dev.<para/>
            /// The supported types are:<para/>
            /// - ::CU_EXEC_AFFINITY_TYPE_SM_COUNT: 1 if context with limited SMs is supported by the device,
            /// or 0 if not;
            /// </summary>
            /// <param name="pi">1 if the execution affinity type \p type is supported by the device, or 0 if not</param>
            /// <param name="type">Execution affinity type to query</param>
            /// <param name="dev">Device handle</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetExecAffinitySupport(ref int pi, CUexecAffinityType type, CUdevice dev);
        }
        #endregion

        #region Context management
        /// <summary>
        /// Combines all API calls for context management
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class ContextManagement
        {
#if (NETCOREAPP)
            static ContextManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Creates a new CUDA context and associates it with the calling thread. The <c>flags</c> parameter is described in <see cref="CUCtxFlags"/>. The
			/// context is created with a usage count of 1 and the caller of <see cref="cuCtxCreate_v2"/> must call <see cref="cuCtxDestroy_v2"/> or <see cref="cuCtxDetach"/>
            /// when done using the context. If a context is already current to the thread, it is supplanted by the newly created context
            /// and may be restored by a subsequent call to <see cref="cuCtxPopCurrent_v2"/>.
            /// </summary>
            /// <param name="pctx">Returned context handle of the new context</param>
            /// <param name="flags">Context creation flags. See <see cref="CUCtxFlags"/></param>
            /// <param name="dev">Device to create context on</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>, <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxCreate_v2(ref CUcontext pctx, CUCtxFlags flags, CUdevice dev);

            /// <summary>
            /// Create a CUDA context with execution affinity<para/>
            /// Creates a new CUDA context with execution affinity and associates it with
            /// the calling thread.The \p paramsArray and \p flags parameter are described below.
            /// The context is created with a usage count of 1 and the caller of ::cuCtxCreate() must
            /// call::cuCtxDestroy() or when done using the context.If a context is already
            /// current to the thread, it is supplanted by the newly created context and may
            /// be restored by a subsequent call to ::cuCtxPopCurrent().<para/>
            /// The type and the amount of execution resource the context can use is limited by \p paramsArray
            /// and \p numParams.The \p paramsArray is an array of \p CUexecAffinityParam and the \p numParams
            /// describes the size of the array. If two \p CUexecAffinityParam in the array have the same type,
            /// the latter execution affinity parameter overrides the former execution affinity parameter.
            /// <para/>
            /// The supported execution affinity types are:
            /// ::CU_EXEC_AFFINITY_TYPE_SM_COUNT limits the portion of SMs that the context can use.The portion
            /// of SMs is specified as the number of SMs via \p CUexecAffinitySmCount. This limit will be internally
            /// rounded up to the next hardware-supported amount. Hence, it is imperative to query the actual execution
            /// affinity of the context via \p cuCtxGetExecAffinity after context creation.Currently, this attribute
            /// is only supported under Volta+ MPS.
            /// </summary>
            /// <param name="pctx">Returned context handle of the new context</param>
            /// <param name="paramsArray"></param>
            /// <param name="numParams"></param>
            /// <param name="flags">Context creation flags. See <see cref="CUCtxFlags"/></param>
            /// <param name="dev">Device to create context on</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxCreate_v3(ref CUcontext pctx, CUexecAffinityParam[] paramsArray, int numParams, CUCtxFlags flags, CUdevice dev);


            /// <summary>
            /// Destroys the CUDA context specified by <c>ctx</c>. The context <c>ctx</c> will be destroyed regardless of how many threads it is current to.
            /// It is the responsibility of the calling function to ensure that no API call is issued to <c>ctx</c> while cuCtxDestroy_v2() is executing.
            /// If <c>ctx</c> is current to the calling thread then <c>ctx</c> will also be 
            /// popped from the current thread's context stack (as though cuCtxPopCurrent()
            /// were called).  If <c>ctx</c> is current to other threads, then <c>ctx</c> will
            /// remain current to those threads, and attempting to access <c>ctx</c> from
            /// those threads will result in the error <see cref="CUResult.ErrorContextIsDestroyed"/>.
            /// </summary>
            /// <param name="ctx">Context to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>,  <see cref="CUResult.ErrorContextIsDestroyed"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxDestroy_v2(CUcontext ctx);

            /// <summary>
            /// Increments the usage count of the context and passes back a context handle in <c>pctx</c> that must be passed to <see cref="cuCtxDetach"/>
            /// when the application is done with the context. <see cref="cuCtxAttach"/> fails if there is no context current to the
            /// thread. Currently, the <c>flags</c> parameter must be <see cref="CUCtxAttachFlags.None"/>.
            /// </summary>
            /// <param name="pctx">Returned context handle of the current context</param>
            /// <param name="flags">Context attach flags (must be <see cref="CUCtxAttachFlags.None"/>)</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuCtxAttach(ref CUcontext pctx, CUCtxAttachFlags flags);

            /// <summary>
            /// Decrements the usage count of the context <c>ctx</c>, and destroys the context if the usage count goes to 0. The context
			/// must be a handle that was passed back by <see cref="cuCtxCreate_v2"/> or <see cref="cuCtxAttach"/>, and must be current to the calling thread.
            /// </summary>
            /// <param name="ctx">Context to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuCtxDetach([In] CUcontext ctx);

            /// <summary>
            /// Pushes the given context <c>ctx</c> onto the CPU thread’s stack of current contexts. The specified context becomes the
            /// CPU thread’s current context, so all CUDA functions that operate on the current context are affected.<para/>
            /// The previous current context may be made current again by calling <see cref="cuCtxDestroy_v2"/> or <see cref="cuCtxPopCurrent_v2"/>.<para/>
            /// The context must be "floating," i.e. not attached to any thread. Contexts are made to float by calling <see cref="cuCtxPopCurrent_v2"/>.
            /// </summary>
            /// <param name="ctx">Floating context to attach</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxPushCurrent_v2([In] CUcontext ctx);

            /// <summary>
            /// Pops the current CUDA context from the CPU thread. The CUDA context must have a usage count of 1. CUDA contexts
            /// have a usage count of 1 upon creation; the usage count may be incremented with <see cref="cuCtxAttach"/> and decremented
            /// with <see cref="cuCtxDetach"/>.<para/>
            /// If successful, <see cref="cuCtxPopCurrent_v2"/> passes back the old context handle in <c>pctx</c>. That context may then be made current
            /// to a different CPU thread by calling <see cref="cuCtxPushCurrent_v2"/>.<para/>
            /// Floating contexts may be destroyed by calling <see cref="cuCtxDestroy_v2"/>.<para/>
			/// If a context was current to the CPU thread before <see cref="cuCtxCreate_v2"/> or <see cref="cuCtxPushCurrent_v2"/> was called, this function makes
            /// that context current to the CPU thread again.
            /// </summary>
            /// <param name="pctx">Returned new context handle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxPopCurrent_v2(ref CUcontext pctx);

            /// <summary>
            /// Binds the specified CUDA context to the calling CPU thread.
            /// If <c>ctx</c> is NULL then the CUDA context previously bound to the
            /// calling CPU thread is unbound and <see cref="CUResult.Success"/> is returned.
            /// <para/>
            /// If there exists a CUDA context stack on the calling CPU thread, this
            /// will replace the top of that stack with <c>ctx</c>.  
            /// If <c>ctx</c> is NULL then this will be equivalent to popping the top
            /// of the calling CPU thread's CUDA context stack (or a no-op if the
            /// calling CPU thread's CUDA context stack is empty).
            /// </summary>
            /// <param name="ctx">Context to bind to the calling CPU thread</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxSetCurrent([In] CUcontext ctx);

            /// <summary>
            /// Returns in <c>ctx</c> the CUDA context bound to the calling CPU thread.
            /// If no context is bound to the calling CPU thread then <c>ctx</c> is
            /// set to NULL and <see cref="CUResult.Success"/> is returned.
            /// </summary>
            /// <param name="pctx">Returned context handle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetCurrent(ref CUcontext pctx);

            /// <summary>
            /// Returns in <c>device</c> the ordinal of the current context’s device.
            /// </summary>
            /// <param name="device">Returned device ID for the current context</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetDevice(ref CUdevice device);

            /// <summary>
            /// Blocks until the device has completed all preceding requested tasks. <see cref="cuCtxSynchronize"/> returns an error if one of the
            /// preceding tasks failed. If the context was created with the <see cref="CUCtxFlags.BlockingSync"/> flag, the CPU thread will
            /// block until the GPU context has finished its work.
            /// </summary>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxSynchronize();

            /// <summary>
            /// Returns the API version used to create <c>ctx</c> in <c>version</c>. If <c>ctx</c>
            /// is NULL, returns the API version used to create the currently bound
            /// context.<para/>
            /// This wil return the API version used to create a context (for example,
            /// 3010 or 3020), which library developers can use to direct callers to a
            /// specific API version. Note that this API version may not be the same as
            /// returned by <see cref="cuDriverGetVersion(ref int)"/>.
            /// </summary>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetApiVersion(CUcontext ctx, ref uint version);

            /// <summary>
            /// On devices where the L1 cache and shared memory use the same hardware
            /// resources, this function returns through <c>pconfig</c> the preferred cache configuration
            /// for the current context. This is only a preference. The driver will use
            /// the requested configuration if possible, but it is free to choose a different
            /// configuration if required to execute functions.<para/>
            /// This will return a <c>pconfig</c> of <see cref="CUFuncCache.PreferNone"/> on devices
            /// where the size of the L1 cache and shared memory are fixed.
            /// </summary>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetCacheConfig(ref CUFuncCache pconfig);

            /// <summary>
            /// On devices where the L1 cache and shared memory use the same hardware
            /// resources, this sets through <c>config</c> the preferred cache configuration for
            /// the current context. This is only a preference. The driver will use
            /// the requested configuration if possible, but it is free to choose a different
            /// configuration if required to execute the function. Any function preference
            /// set via <see cref="FunctionManagement.cuFuncSetCacheConfig"/> will be preferred over this context-wide
            /// setting. Setting the context-wide cache configuration to
            /// <see cref="CUFuncCache.PreferNone"/> will cause subsequent kernel launches to prefer
            /// to not change the cache configuration unless required to launch the kernel.<para/>
            /// This setting does nothing on devices where the size of the L1 cache and
            /// shared memory are fixed.<para/>
            /// Launching a kernel with a different preference than the most recent
            /// preference setting may insert a device-side synchronization point.
            /// </summary>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxSetCacheConfig(CUFuncCache config);

            /// <summary>
            /// Returns the current shared memory configuration for the current context.
            /// <para/>
            /// This function will return in \p pConfig the current size of shared memory banks
            /// in the current context. On devices with configurable shared memory banks, 
            /// <see cref="cuCtxSetSharedMemConfig"/> can be used to change this setting, so that all 
            /// subsequent kernel launches will by default use the new bank size. When 
            /// <see cref="cuCtxGetSharedMemConfig"/> is called on devices without configurable shared 
            /// memory, it will return the fixed bank size of the hardware.
            ///<para/>
            /// The returned bank configurations can be either:
            /// - <see cref="CUsharedconfig.FourByteBankSize"/>: set shared memory bank width to
            ///   be natively four bytes.
            /// - <see cref="CUsharedconfig.EightByteBankSize"/>: set shared memory bank width to
            ///   be natively eight bytes.
            /// </summary>
            /// <param name="pConfig">returned shared memory configuration</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetSharedMemConfig(ref CUsharedconfig pConfig);

            /// <summary>
            /// Sets the shared memory configuration for the current context.<para/>
            /// On devices with configurable shared memory banks, this function will set
            /// the context's shared memory bank size which is used for subsequent kernel 
            /// launches. <para/> 
            /// Changed the shared memory configuration between launches may insert a device
            /// side synchronization point between those launches.<para/>
            /// Changing the shared memory bank size will not increase shared memory usage
            /// or affect occupancy of kernels, but may have major effects on performance. 
            /// Larger bank sizes will allow for greater potential bandwidth to shared memory,
            /// but will change what kinds of accesses to shared memory will result in bank 
            /// conflicts.<para/>
            /// This function will do nothing on devices with fixed shared memory bank size.
            /// <para/>
            /// The supported bank configurations are:
            /// - <see cref="CUsharedconfig.DefaultBankSize"/>: set bank width to the default initial
            ///   setting (currently, four bytes).
            /// - <see cref="CUsharedconfig.FourByteBankSize"/>: set shared memory bank width to
            ///   be natively four bytes.
            /// - <see cref="CUsharedconfig.EightByteBankSize"/>: set shared memory bank width to
            ///   be natively eight bytes.
            /// </summary>
            /// <param name="config">requested shared memory configuration</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxSetSharedMemConfig(CUsharedconfig config);

            /// <summary>
            /// Returns numerical values that correspond to the least and greatest stream priorities.<para/>
            /// Returns in <c>leastPriority</c> and <c>greatestPriority</c> the numerical values that correspond
            /// to the least and greatest stream priorities respectively. Stream priorities
            /// follow a convention where lower numbers imply greater priorities. The range of
            /// meaningful stream priorities is given by [<c>greatestPriority</c>, <c>leastPriority</c>].
            /// If the user attempts to create a stream with a priority value that is
            /// outside the meaningful range as specified by this API, the priority is
            /// automatically clamped down or up to either <c>leastPriority</c> or <c>greatestPriority</c>
            /// respectively. See ::cuStreamCreateWithPriority for details on creating a
            /// priority stream.
            /// A NULL may be passed in for <c>leastPriority</c> or <c>greatestPriority</c> if the value
            /// is not desired.
            /// This function will return '0' in both <c>leastPriority</c> and <c>greatestPriority</c> if
            /// the current context's device does not support stream priorities
            /// (see ::cuDeviceGetAttribute).
            /// </summary>
            /// <param name="leastPriority">Pointer to an int in which the numerical value for least
            /// stream priority is returned</param>
            /// <param name="greatestPriority">Pointer to an int in which the numerical value for greatest stream priority is returned</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetStreamPriorityRange(ref int leastPriority, ref int greatestPriority);

            /// <summary>
            /// Resets all persisting lines in cache to normal status.<para/>
            /// ::cuCtxResetPersistingL2Cache Resets all persisting lines in cache to normal
            /// status.Takes effect on function return.
            /// </summary>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxResetPersistingL2Cache();

            /// <summary>
            /// Returns the execution affinity setting for the current context.<para/>
            /// Returns in \p *pExecAffinity the current value of \p type. The supported ::CUexecAffinityType values are:
            /// - ::CU_EXEC_AFFINITY_TYPE_SM_COUNT: number of SMs the context is limited to use.
            /// </summary>
            /// <param name="pExecAffinity"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetExecAffinity(ref CUexecAffinityParam pExecAffinity, CUexecAffinityType type);

            /// <summary>
            /// Returns the flags for the current context<para/>
            /// Returns in \p *flags the flags of the current context. See ::cuCtxCreate for flag values.
            /// </summary>
            /// <param name="flags">Pointer to store flags of current context</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetFlags(ref CUCtxFlags flags);



            #region Primary Context

            /// <summary>
            /// Retain the primary context on the GPU.<para/>
            /// Retains the primary context on the device, creating it if necessary,
            /// increasing its usage count. The caller must call
            /// ::cuDevicePrimaryCtxRelease() when done using the context.
            /// Unlike ::cuCtxCreate() the newly created context is not pushed onto the stack.
            /// <para/>
            /// Context creation will fail with ::CUDA_ERROR_UNKNOWN if the compute mode of
            /// the device is ::CU_COMPUTEMODE_PROHIBITED. Similarly, context creation will
            /// also fail with ::CUDA_ERROR_UNKNOWN if the compute mode for the device is
            /// set to ::CU_COMPUTEMODE_EXCLUSIVE and there is already an active, non-primary,
            /// context on the device. The function ::cuDeviceGetAttribute() can be used with
            /// ::CU_DEVICE_ATTRIBUTE_COMPUTE_MODE to determine the compute mode of the
            /// device. The <i>nvidia-smi</i> tool can be used to set the compute mode for
            /// devices. Documentation for <i>nvidia-smi</i> can be obtained by passing a
            /// -h option to it.
            /// <para/> 
            /// Please note that the primary context always supports pinned allocations. Other
            /// flags can be specified by ::cuDevicePrimaryCtxSetFlags().
            /// </summary>
            /// <param name="pctx">Returned context handle of the new context</param>
            /// <param name="dev">Device for which primary context is requested</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDevicePrimaryCtxRetain(ref CUcontext pctx, CUdevice dev);

            /// <summary>
            /// Release the primary context on the GPU<para/>
            /// Releases the primary context interop on the device by decreasing the usage
            /// count by 1. If the usage drops to 0 the primary context of device \p dev
            /// will be destroyed regardless of how many threads it is current to.
            /// <para/>
            /// Please note that unlike ::cuCtxDestroy() this method does not pop the context
            /// from stack in any circumstances.
            /// </summary>
            /// <param name="dev">Device which primary context is released</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuDevicePrimaryCtxRelease_v2")]
            public static extern CUResult cuDevicePrimaryCtxRelease(CUdevice dev);

            /// <summary>
            /// Set flags for the primary context<para/>
            /// Sets the flags for the primary context on the device overwriting perviously
            /// set ones. If the primary context is already created
            /// ::CUDA_ERROR_PRIMARY_CONTEXT_ACTIVE is returned.
            /// <para/>
            ///	The three LSBs of the \p flags parameter can be used to control how the OS
            ///	thread, which owns the CUDA context at the time of an API call, interacts
            ///	with the OS scheduler when waiting for results from the GPU. Only one of
            ///	the scheduling flags can be set when creating a context.
            /// </summary>
            /// <param name="dev">Device for which the primary context flags are set</param>
            /// <param name="flags">New flags for the device</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuDevicePrimaryCtxSetFlags_v2")]
            public static extern CUResult cuDevicePrimaryCtxSetFlags(CUdevice dev, CUCtxFlags flags);

            /// <summary>
            /// Get the state of the primary context<para/>
            /// Returns in \p *flags the flags for the primary context of \p dev, and in
            /// \p *active whether it is active.  See ::cuDevicePrimaryCtxSetFlags for flag
            /// values.
            /// </summary>
            /// <param name="dev">Device to get primary context flags for</param>
            /// <param name="flags">Pointer to store flags</param>
            /// <param name="active">Pointer to store context state; 0 = inactive, 1 = active</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDevicePrimaryCtxGetState(CUdevice dev, ref CUCtxFlags flags, ref int active);

            /// <summary>
            /// Destroy all allocations and reset all state on the primary context
            /// 
            /// Explicitly destroys and cleans up all resources associated with the current
            /// device in the current process.
            /// 
            /// Note that it is responsibility of the calling function to ensure that no
            /// other module in the process is using the device any more. For that reason
            /// it is recommended to use ::cuDevicePrimaryCtxRelease() in most cases.
            /// However it is safe for other modules to call ::cuDevicePrimaryCtxRelease()
            /// even after resetting the device.
            /// </summary>
            /// <param name="dev">Device for which primary context is destroyed</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuDevicePrimaryCtxReset_v2")]
            public static extern CUResult cuDevicePrimaryCtxReset(CUdevice dev);
            #endregion
        }
        #endregion

        #region Module management
        /// <summary>
        /// Combines all API calls for module management
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class ModuleManagement
        {
#if (NETCOREAPP)
            static ModuleManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Takes a filename <c>fname</c> and loads the corresponding module <c>module</c> into the current context. The CUDA driver API
            /// does not attempt to lazily allocate the resources needed by a module; if the memory for functions and data (constant
            /// and global) needed by the module cannot be allocated, <see cref="cuModuleLoad"/> fails. The file should be a <c>cubin</c> file as output
            /// by <c>nvcc</c> or a <c>PTX</c> file, either as output by <c>nvcc</c> or handwrtten.
            /// </summary>
            /// <param name="module">Returned module</param>
            /// <param name="fname">Filename of module to load</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotFound"/>,
            /// <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorFileNotFound"/>, <see cref="CUResult.ErrorSharedObjectSymbolNotFound"/>,
            /// <see cref="CUResult.ErrorSharedObjectInitFailed"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleLoad(ref CUmodule module, string fname);

            /// <summary>
            /// Takes a byte[] as <c>image</c> and loads the corresponding module <c>module</c> into the current context. The byte array may be obtained
            /// by mapping a <c>cubin</c> or <c>PTX</c> file, passing a <c>cubin</c> or <c>PTX</c> file as a <c>null</c>-terminated text string.<para/>
            /// The byte[] is a replacement for the original pointer.
            /// </summary>
            /// <param name="module">Returned module</param>
            /// <param name="image">Module data to load</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>,
            /// <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorSharedObjectSymbolNotFound"/>,
            /// <see cref="CUResult.ErrorSharedObjectInitFailed"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleLoadData(ref CUmodule module, [In] byte[] image);

            /// <summary>
            /// Takes a byte[] as <c>image</c> and loads the corresponding module <c>module</c> into the current context. The byte array may be obtained
            /// by mapping a <c>cubin</c> or <c>PTX</c> file, passing a <c>cubin</c> or <c>PTX</c> file as a <c>null</c>-terminated text string. <para/>
            /// Options are passed as an array via <c>options</c> and any corresponding parameters are passed
            /// in <c>optionValues</c>. The number of total options is supplied via <c>numOptions</c>. Any outputs will be returned via
            /// <c>optionValues</c>. Supported options are definen in <see cref="CUJITOption"/>.<para/>
            /// The options values are currently passed in <c>IntPtr</c>-type and should then be cast into their real type. This might change in future.
            /// </summary>
            /// <param name="module">Returned module</param>
            /// <param name="image">Module data to load</param>
            /// <param name="numOptions">Number of options</param>
            /// <param name="options">Options for JIT</param>
            /// <param name="optionValues">Option values for JIT</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>
            /// <see cref="CUResult.ErrorNoBinaryForGPU"/>, <see cref="CUResult.ErrorSharedObjectSymbolNotFound"/>,
            /// <see cref="CUResult.ErrorSharedObjectInitFailed"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleLoadDataEx(ref CUmodule module, [In] byte[] image, uint numOptions, [In] CUJITOption[] options, [In, Out] IntPtr[] optionValues);

            /// <summary>
            /// Takes a byte[] as <c>fatCubin</c> and loads the corresponding module <c>module</c> into the current context. The byte[]
            /// represents a <c>fat binary</c> object, which is a collection of different <c>cubin</c> files, all representing the same device code, but
            /// compiled and optimized for different architectures. Prior to CUDA 4.0, there was no documented API for constructing and using
            /// fat binary objects by programmers. Starting with CUDA 4.0, fat binary objects can be constructed by providing the -fatbin option to nvcc.
            /// More information can be found in the <c>nvcc</c> document.
            /// </summary>
            /// <param name="module">Returned module</param>
            /// <param name="fatCubin">Fat binary to load</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotFound"/>, <see cref="CUResult.ErrorOutOfMemory"/>
            /// <see cref="CUResult.ErrorNoBinaryForGPU"/>, <see cref="CUResult.ErrorSharedObjectSymbolNotFound"/>,
            /// <see cref="CUResult.ErrorSharedObjectInitFailed"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleLoadFatBinary(ref CUmodule module, [In] byte[] fatCubin);

            /// <summary>
            /// Unloads a module <c>hmod</c> from the current context.
            /// </summary>
            /// <param name="hmod">Module to unload</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleUnload(CUmodule hmod);

            /// <summary>
            /// Returns in <c>hfunc</c> the handle of the function of name <c>name</c> located in module <c>hmod</c>. If no function of that name
            /// exists, <see cref="cuModuleGetFunction"/> returns <see cref="CUResult.ErrorNotFound"/>.
            /// </summary>
            /// <param name="hfunc">Returned function handle</param>
            /// <param name="hmod">Module to retrieve function from</param>
            /// <param name="name">Name of function to retrieve</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotFound"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleGetFunction(ref CUfunction hfunc, CUmodule hmod, string name);

            /// <summary>
            /// Returns in <c>dptr</c> and <c>bytes</c> the base pointer and size of the global of name <c>name</c> located in module <c>hmod</c>. If no
			/// variable of that name exists, <see cref="cuModuleGetGlobal_v2"/> returns <see cref="CUResult.ErrorNotFound"/>. Both parameters <c>dptr</c>
            /// and <c>bytes</c> are optional. If one of them is <c>null</c>, it is ignored.
            /// </summary>
            /// <param name="dptr">Returned global device pointer</param>
            /// <param name="bytes">Returned global size in bytes</param>
            /// <param name="hmod">Module to retrieve global from</param>
            /// <param name="name">Name of global to retrieve</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotFound"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleGetGlobal_v2(ref CUdeviceptr dptr, ref SizeT bytes, CUmodule hmod, string name);

            /// <summary>
            /// Returns in <c>pTexRef</c> the handle of the texture reference of name <c>name</c> in the module <c>hmod</c>. If no texture reference
            /// of that name exists, <see cref="cuModuleGetSurfRef"/> returns <see cref="CUResult.ErrorNotFound"/>. This texture reference handle
            /// should not be destroyed, since it will be destroyed when the module is unloaded.
            /// </summary>
            /// <param name="pTexRef">Returned texture reference</param>
            /// <param name="hmod">Module to retrieve texture reference from</param>
            /// <param name="name">Name of texture reference to retrieve</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotFound"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleGetTexRef(ref CUtexref pTexRef, CUmodule hmod, string name);

            /// <summary>
            /// Returns in <c>pSurfRef</c> the handle of the surface reference of name <c>name</c> in the module <c>hmod</c>. If no surface reference
            /// of that name exists, <see cref="cuModuleGetSurfRef"/> returns <see cref="CUResult.ErrorNotFound"/>.
            /// </summary>
            /// <param name="pSurfRef">Returned surface reference</param>
            /// <param name="hmod">Module to retrieve surface reference from</param>
            /// <param name="name">Name of surface reference to retrieve</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotFound"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuModuleGetSurfRef(ref CUsurfref pSurfRef, CUmodule hmod, string name);

            /// <summary>
            /// Creates a pending JIT linker invocation.<para/>
            /// If the call is successful, the caller owns the returned CUlinkState, which should eventually be destroyed with ::cuLinkDestroy.
            /// The device code machine size (32 or 64 bit) will match the calling application.<para/>
            /// Both linker and compiler options may be specified. Compiler options will be applied to inputs to this linker action which must 
            /// be compiled from PTX. The options ::CU_JIT_WALL_TIME, 
            /// ::CU_JIT_INFO_LOG_BUFFER_SIZE_BYTES, and ::CU_JIT_ERROR_LOG_BUFFER_SIZE_BYTES will accumulate data until the CUlinkState is destroyed.<para/>
            /// <c>optionValues</c> must remain valid for the life of the CUlinkState if output options are used. No other references to inputs are maintained after this call returns.
            /// </summary>
            /// <param name="numOptions">Size of options arrays</param>
            /// <param name="options">Array of linker and compiler options</param>
            /// <param name="optionValues">Array of option values, each cast to void *</param>
            /// <param name="stateOut">On success, this will contain a CUlinkState to specify and complete this action</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuLinkCreate_v2")]
            public static extern CUResult cuLinkCreate(uint numOptions, CUJITOption[] options, [In, Out] IntPtr[] optionValues, ref CUlinkState stateOut);



            /// <summary>
            /// Add an input to a pending linker invocation.<para/>
            /// Ownership of <c>data</c> data is retained by the caller.  No reference is retained to any inputs after this call returns.<para/>
            /// This method accepts only compiler options, which are used if the data must be compiled from PTX, and does not accept any of
            /// ::CU_JIT_WALL_TIME, ::CU_JIT_INFO_LOG_BUFFER, ::CU_JIT_ERROR_LOG_BUFFER, ::CU_JIT_TARGET_FROM_CUCONTEXT, or ::CU_JIT_TARGET.
            /// </summary>
            /// <param name="state">A pending linker action.</param>
            /// <param name="type">The type of the input data.</param>
            /// <param name="data">The input data.  PTX must be NULL-terminated.</param>
            /// <param name="size">The length of the input data.</param>
            /// <param name="name">An optional name for this input in log messages.</param>
            /// <param name="numOptions">Size of options.</param>
            /// <param name="options">Options to be applied only for this input (overrides options from ::cuLinkCreate).</param>
            /// <param name="optionValues">Array of option values, each cast to void *.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuLinkAddData_v2")]
            public static extern CUResult cuLinkAddData(CUlinkState state, CUJITInputType type, byte[] data, SizeT size, [MarshalAs(UnmanagedType.LPStr)] string name,
                uint numOptions, CUJITOption[] options, IntPtr[] optionValues);

            /// <summary>
            /// Add a file input to a pending linker invocation.<para/>
            /// No reference is retained to any inputs after this call returns.<para/>
            /// This method accepts only compiler options, which are used if the data must be compiled from PTX, and does not accept any of
            /// ::CU_JIT_WALL_TIME, ::CU_JIT_INFO_LOG_BUFFER, ::CU_JIT_ERROR_LOG_BUFFER, ::CU_JIT_TARGET_FROM_CUCONTEXT, or ::CU_JIT_TARGET.
            /// <para/>This method is equivalent to invoking ::cuLinkAddData on the contents of the file.
            /// </summary>
            /// <param name="state">A pending linker action.</param>
            /// <param name="type">The type of the input data.</param>
            /// <param name="path">Path to the input file.</param>
            /// <param name="numOptions">Size of options.</param>
            /// <param name="options">Options to be applied only for this input (overrides options from ::cuLinkCreate).</param>
            /// <param name="optionValues">Array of option values, each cast to void *.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuLinkAddFile_v2")]
            public static extern CUResult cuLinkAddFile(CUlinkState state, CUJITInputType type, string path, uint numOptions, CUJITOption[] options, IntPtr[] optionValues);


            /// <summary>
            /// Complete a pending linker invocation.<para/>
            /// Completes the pending linker action and returns the cubin image for the linked
            /// device code, which can be used with ::cuModuleLoadData. <para/>The cubin is owned by
            /// <c>state</c>, so it should be loaded before <c>state</c> is destroyed via ::cuLinkDestroy.
            /// This call does not destroy <c>state</c>.
            /// </summary>
            /// <param name="state">A pending linker invocation</param>
            /// <param name="cubinOut">On success, this will point to the output image</param>
            /// <param name="sizeOut">Optional parameter to receive the size of the generated image</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuLinkComplete(CUlinkState state, ref IntPtr cubinOut, ref SizeT sizeOut);

            /// <summary>
            /// Destroys state for a JIT linker invocation.
            /// </summary>
            /// <param name="state">State object for the linker invocation</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuLinkDestroy(CUlinkState state);

        }
        #endregion

        #region Memory management
        /// <summary>
        /// Combines all API calls for memory management
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class MemoryManagement
        {
#if (NETCOREAPP)
            static MemoryManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Returns in <c>free</c> and <c>total</c> respectively, the free and total amount of memory available for allocation by the 
            /// CUDA context, in bytes.
            /// </summary>
            /// <param name="free">Returned free memory in bytes</param>
            /// <param name="total">Returned total memory in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemGetInfo_v2(ref SizeT free, ref SizeT total);

            /// <summary>
            /// Allocates <c>bytesize</c> bytes of linear memory on the device and returns in <c>dptr</c> a pointer to the allocated memory.
            /// The allocated memory is suitably aligned for any kind of variable. The memory is not cleared. If <c>bytesize</c> is 0,
			/// <see cref="cuMemAlloc_v2"/> returns <see cref="CUResult.ErrorInvalidValue"/>.
            /// </summary>
            /// <param name="dptr">Returned device pointer</param>
            /// <param name="bytesize">Requested allocation size in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAlloc_v2(ref CUdeviceptr dptr, SizeT bytesize);

            /// <summary>
            /// Allocates at least <c>WidthInBytes * Height</c> bytes of linear memory on the device and returns in <c>dptr</c> a pointer
            /// to the allocated memory. The function may pad the allocation to ensure that corresponding pointers in any given
            /// row will continue to meet the alignment requirements for coalescing as the address is updated from row to row. <para/>
            /// <c>ElementSizeBytes</c> specifies the size of the largest reads and writes that will be performed on the memory range.<para/>
            /// <c>ElementSizeBytes</c> may be 4, 8 or 16 (since coalesced memory transactions are not possible on other data sizes). If
            /// <c>ElementSizeBytes</c> is smaller than the actual read/write size of a kernel, the kernel will run correctly, but possibly
			/// at reduced speed. The pitch returned in <c>pPitch</c> by <see cref="cuMemAllocPitch_v2"/> is the width in bytes of the allocation. The
            /// intended usage of pitch is as a separate parameter of the allocation, used to compute addresses within the 2D array.<para/>
            /// Given the row and column of an array element of type T, the address is computed as:<para/>
            /// <code>T * pElement = (T*)((char*)BaseAddress + Row * Pitch) + Column;</code><para/>
			/// The pitch returned by <see cref="cuMemAllocPitch_v2"/> is guaranteed to work with <see cref="SynchronousMemcpy_v2.cuMemcpy2D_v2"/> under all circumstances. For
			/// allocations of 2D arrays, it is recommended that programmers consider performing pitch allocations using <see cref="cuMemAllocPitch_v2"/>.
            /// Due to alignment restrictions in the hardware, this is especially true if the application will be performing
            /// 2D memory copies between different regions of device memory (whether linear memory or CUDA arrays). <para/>
			/// The byte alignment of the pitch returned by <see cref="cuMemAllocPitch_v2"/> is guaranteed to match or exceed the alignment
			/// requirement for texture binding with <see cref="TextureReferenceManagement.cuTexRefSetAddress2D_v2"/>.
            /// </summary>
            /// <param name="dptr">Returned device pointer</param>
            /// <param name="pPitch">Returned pitch of allocation in bytes</param>
            /// <param name="WidthInBytes">Requested allocation width in bytes</param>
            /// <param name="Height">Requested allocation height in rows</param>
            /// <param name="ElementSizeBytes">Size of largest reads/writes for range</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAllocPitch_v2(ref CUdeviceptr dptr, ref SizeT pPitch, SizeT WidthInBytes, SizeT Height, uint ElementSizeBytes);

            /// <summary>
			/// Frees the memory space pointed to by <c>dptr</c>, which must have been returned by a previous call to <see cref="cuMemAlloc_v2"/> or
			/// <see cref="cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dptr">Pointer to memory to free</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemFree_v2(CUdeviceptr dptr);

            /// <summary>
			/// Returns the base address in <c>pbase</c> and size in <c>psize</c> of the allocation by <see cref="cuMemAlloc_v2"/> or <see cref="cuMemAllocPitch_v2"/>
            /// that contains the input pointer <c>dptr</c>. Both parameters <c>pbase</c> and <c>psize</c> are optional. If one of them is <c>null</c>, it is
            /// ignored.
            /// </summary>
            /// <param name="pbase">Returned base address</param>
            /// <param name="psize">Returned size of device memory allocation</param>
            /// <param name="dptr">Device pointer to query</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemGetAddressRange_v2(ref CUdeviceptr pbase, ref SizeT psize, CUdeviceptr dptr);

            /// <summary>
            /// Allocates <c>bytesize</c> bytes of host memory that is page-locked and accessible to the device. The driver tracks the virtual
			/// memory ranges allocated with this function and automatically accelerates calls to functions such as <see cref="SynchronousMemcpy_v2.cuMemcpyHtoD_v2(CUdeviceptr, IntPtr, SizeT)"/>.
            /// Since the memory can be accessed directly by the device, it can be read or written with much higher bandwidth than
			/// pageable memory obtained with functions such as <c>malloc()</c>. Allocating excessive amounts of memory with <see cref="cuMemAllocHost_v2"/>
            /// may degrade system performance, since it reduces the amount of memory available to the system for paging.
            /// As a result, this function is best used sparingly to allocate staging areas for data exchange between host and device.
            /// </summary>
            /// <param name="pp">Returned host pointer to page-locked memory</param>
            /// <param name="bytesize">Requested allocation size in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAllocHost_v2(ref IntPtr pp, SizeT bytesize);

            /// <summary>
			/// Frees the memory space pointed to by <c>p</c>, which must have been returned by a previous call to <see cref="cuMemAllocHost_v2"/>.
            /// </summary>
            /// <param name="p">Pointer to memory to free</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemFreeHost(IntPtr p);

            /// <summary>
            /// Allocates <c>bytesize</c> bytes of host memory that is page-locked and accessible to the device. The driver tracks the virtual
			/// memory ranges allocated with this function and automatically accelerates calls to functions such as <see cref="SynchronousMemcpy_v2.cuMemcpyHtoD_v2(CUdeviceptr, IntPtr, SizeT)"/>.
            /// Since the memory can be accessed directly by the device, it can be read or written with much higher bandwidth than
            /// pageable memory obtained with functions such as <c>malloc()</c>. Allocating excessive amounts of pinned
            /// memory may degrade system performance, since it reduces the amount of memory available to the system for paging.
            /// As a result, this function is best used sparingly to allocate staging areas for data exchange between host and device.<para/>
            /// For the <c>Flags</c> parameter see <see cref="CUMemHostAllocFlags"/>.<para/>
            /// The CUDA context must have been created with the <see cref="CUCtxFlags.MapHost"/> flag in order for the <see cref="CUMemHostAllocFlags.DeviceMap"/>
            /// flag to have any effect.<para/>
            /// The <see cref="CUCtxFlags.MapHost"/> flag may be specified on CUDA contexts for devices that do not support
			/// mapped pinned memory. The failure is deferred to <see cref="cuMemHostGetDevicePointer_v2"/> because the memory may be
            /// mapped into other CUDA contexts via the <see cref="CUMemHostAllocFlags.Portable"/> flag. <para/>
            /// The memory allocated by this function must be freed with <see cref="cuMemFreeHost"/>.<para/>
            /// Note all host memory allocated using <see cref="cuMemHostAlloc"/> will automatically
            /// be immediately accessible to all contexts on all devices which support unified
            /// addressing (as may be queried using ::CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING).
            /// Unless the flag ::CU_MEMHOSTALLOC_WRITECOMBINED is specified, the device pointer 
            /// that may be used to access this host memory from those contexts is always equal 
            /// to the returned host pointer <c>pp</c>.  If the flag ::CU_MEMHOSTALLOC_WRITECOMBINED
			/// is specified, then the function <see cref="cuMemHostGetDevicePointer_v2"/> must be used
            /// to query the device pointer, even if the context supports unified addressing.
            /// See \ref CUDA_UNIFIED for additional details.
            /// </summary>
            /// <param name="pp">Returned host pointer to page-locked memory</param>
            /// <param name="bytesize">Requested allocation size in bytes</param>
            /// <param name="Flags">Flags for allocation request</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemHostAlloc(ref IntPtr pp, SizeT bytesize, CUMemHostAllocFlags Flags);

            /// <summary>
            /// Passes back the device pointer <c>pdptr</c> corresponding to the mapped, pinned host buffer <c>p</c> allocated by <see cref="cuMemHostAlloc"/>.
			/// <see cref="cuMemHostGetDevicePointer_v2"/> will fail if the <see cref="CUMemHostAllocFlags.DeviceMap"/> flag was not specified at the
            /// time the memory was allocated, or if the function is called on a GPU that does not support mapped pinned memory.
            /// Flags provides for future releases. For now, it must be set to 0.
            /// </summary>
            /// <param name="pdptr">Returned device pointer</param>
            /// <param name="p">Host pointer</param>
            /// <param name="Flags">Options (must be 0)</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemHostGetDevicePointer_v2(ref CUdeviceptr pdptr, IntPtr p, int Flags);

            /// <summary>
            /// Passes back the flags <c>pFlags</c> that were specified when allocating the pinned host buffer <c>p</c> allocated by
            /// <see cref="cuMemHostAlloc"/>.<para/>
			/// <see cref="cuMemHostGetFlags"/> will fail if the pointer does not reside in an allocation performed by <see cref="cuMemAllocHost_v2"/> or
            /// <see cref="cuMemHostAlloc"/>.
            /// </summary>
            /// <param name="pFlags">Returned flags</param>
            /// <param name="p">Host pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemHostGetFlags(ref CUMemHostAllocFlags pFlags, IntPtr p);

            /// <summary>
            /// Page-locks the memory range specified by <c>p</c> and <c>bytesize</c> and maps it
            /// for the device(s) as specified by <c>Flags</c>. This memory range also is added
            /// to the same tracking mechanism as ::cuMemHostAlloc to automatically accelerate
            /// calls to functions such as <see cref="SynchronousMemcpy_v2.cuMemcpyHtoD_v2(BasicTypes.CUdeviceptr, VectorTypes.dim3[], BasicTypes.SizeT)"/>. Since the memory can be accessed 
            /// directly by the device, it can be read or written with much higher bandwidth 
            /// than pageable memory that has not been registered.  Page-locking excessive
            /// amounts of memory may degrade system performance, since it reduces the amount
            /// of memory available to the system for paging. As a result, this function is
            /// best used sparingly to register staging areas for data exchange between
            /// host and device.<para/>
            /// The pointer <c>p</c> and size <c>bytesize</c> must be aligned to the host page size (4 KB).<para/>
            /// The memory page-locked by this function must be unregistered with <see cref="cuMemHostUnregister"/>
            /// </summary>
            /// <param name="p">Host pointer to memory to page-lock</param>
            /// <param name="byteSize">Size in bytes of the address range to page-lock</param>
            /// <param name="Flags">Flags for allocation request</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemHostRegister_v2")]
            public static extern CUResult cuMemHostRegister(IntPtr p, SizeT byteSize, CUMemHostRegisterFlags Flags);

            /// <summary>
            /// Unmaps the memory range whose base address is specified by <c>p</c>, and makes it pageable again.<para/>
            /// The base address must be the same one specified to <see cref="cuMemHostRegister"/>.
            /// </summary>
            /// <param name="p">Host pointer to memory to page-lock</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemHostUnregister(IntPtr p);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref CUcontext data, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref CUMemoryType data, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref CUdeviceptr data, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref IntPtr data, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref CudaPointerAttributeP2PTokens data, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref int data, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer
            /// </summary>
            /// <param name="data">Returned pointer attribute value</param>
            /// <param name="attribute">Pointer attribute to query</param>
            /// <param name="ptr">Pointer</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttribute(ref ulong data, CUPointerAttribute attribute, CUdeviceptr ptr);


            /// <summary>
            /// Prefetches memory to the specified destination device<para/>
            /// Prefetches memory to the specified destination device. devPtr is the 
            /// base device pointer of the memory to be prefetched and dstDevice is the 
            /// destination device. count specifies the number of bytes to copy. hStream
            /// is the stream in which the operation is enqueued.<para/>
            /// 
            /// Passing in CU_DEVICE_CPU for dstDevice will prefetch the data to CPU memory.<para/>
            /// 
            /// If no physical memory has been allocated for this region, then this memory region
            /// will be populated and mapped on the destination device. If there's insufficient
            /// memory to prefetch the desired region, the Unified Memory driver may evict pages
            /// belonging to other memory regions to make room. If there's no memory that can be
            /// evicted, then the Unified Memory driver will prefetch less than what was requested.<para/>
            /// 
            /// In the normal case, any mappings to the previous location of the migrated pages are
            /// removed and mappings for the new location are only setup on the dstDevice.
            /// The application can exercise finer control on these mappings using ::cudaMemAdvise.
            /// </summary>
            /// <param name="devPtr">Pointer to be prefetched</param>
            /// <param name="count">Size in bytes</param>
            /// <param name="dstDevice">Destination device to prefetch to</param>
            /// <param name="hStream">Stream to enqueue prefetch operation</param>
            /// <remarks>Note that this function is asynchronous with respect to the host and all work on other devices.</remarks>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemPrefetchAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemPrefetchAsync(CUdeviceptr devPtr, SizeT count, CUdevice dstDevice, CUstream hStream);

            /// <summary>
            /// Advise about the usage of a given memory range<para/>
            /// Advise the Unified Memory subsystem about the usage pattern for the memory range starting at devPtr with a size of count bytes.<para/>
            /// <para/>
            /// The \p advice parameter can take the following values:<para/>
            /// - ::CU_MEM_ADVISE_SET_READ_MOSTLY: This implies that the data is mostly going to be read
            /// from and only occasionally written to. This allows the driver to create read-only
            /// copies of the data in a processor's memory when that processor accesses it. Similarly,
            /// if cuMemPrefetchAsync is called on this region, it will create a read-only copy of
            /// the data on the destination processor. When a processor writes to this data, all copies
            /// of the corresponding page are invalidated except for the one where the write occurred.
            /// The \p device argument is ignored for this advice.<para/>
            /// - ::CU_MEM_ADVISE_UNSET_READ_MOSTLY: Undoes the effect of ::CU_MEM_ADVISE_SET_READ_MOSTLY. Any read
            /// duplicated copies of the data will be freed no later than the next write access to that data.<para/>
            /// - ::CU_MEM_ADVISE_SET_PREFERRED_LOCATION: This advice sets the preferred location for the
            /// data to be the memory belonging to \p device. Passing in CU_DEVICE_CPU for \p device sets the
            /// preferred location as CPU memory. Setting the preferred location does not cause data to
            /// migrate to that location immediately. Instead, it guides the migration policy when a fault
            /// occurs on that memory region. If the data is already in its preferred location and the
            /// faulting processor can establish a mapping without requiring the data to be migrated, then
            /// the migration will be avoided. On the other hand, if the data is not in its preferred location
            /// or if a direct mapping cannot be established, then it will be migrated to the processor accessing
            /// it. It is important to note that setting the preferred location does not prevent data prefetching
            /// done using ::cuMemPrefetchAsync.<para/>
            /// Having a preferred location can override the thrash detection and resolution logic in the Unified
            /// Memory driver. Normally, if a page is detected to be constantly thrashing between CPU and GPU
            /// memory say, the page will eventually be pinned to CPU memory by the Unified Memory driver. But
            /// if the preferred location is set as GPU memory, then the page will continue to thrash indefinitely.
            /// When the Unified Memory driver has to evict pages from a certain location on account of that
            /// memory being oversubscribed, the preferred location will be used to decide the destination to which
            /// a page should be evicted to.<para/>
            /// If ::CU_MEM_ADVISE_SET_READ_MOSTLY is also set on this memory region or any subset of it, the preferred
            /// location will be ignored for that subset.<para/>
            /// - ::CU_MEM_ADVISE_UNSET_PREFERRED_LOCATION: Undoes the effect of ::CU_MEM_ADVISE_SET_PREFERRED_LOCATION
            /// and changes the preferred location to none.<para/>
            /// - ::CU_MEM_ADVISE_SET_ACCESSED_BY: This advice implies that the data will be accessed by \p device.
            /// This does not cause data migration and has no impact on the location of the data per se. Instead,
            /// it causes the data to always be mapped in the specified processor's page tables, as long as the
            /// location of the data permits a mapping to be established. If the data gets migrated for any reason,
            /// the mappings are updated accordingly.<para/>
            /// This advice is useful in scenarios where data locality is not important, but avoiding faults is.
            /// Consider for example a system containing multiple GPUs with peer-to-peer access enabled, where the
            /// data located on one GPU is occasionally accessed by other GPUs. In such scenarios, migrating data
            /// over to the other GPUs is not as important because the accesses are infrequent and the overhead of
            /// migration may be too high. But preventing faults can still help improve performance, and so having
            /// a mapping set up in advance is useful. Note that on CPU access of this data, the data may be migrated
            /// to CPU memory because the CPU typically cannot access GPU memory directly. Any GPU that had the
            /// ::CU_MEM_ADVISE_SET_ACCESSED_BY flag set for this data will now have its mapping updated to point to the
            /// page in CPU memory.<para/>
            /// - ::CU_MEM_ADVISE_UNSET_ACCESSED_BY: Undoes the effect of CU_MEM_ADVISE_SET_ACCESSED_BY. The current set of
            /// mappings may be removed at any time causing accesses to result in page faults.
            /// <para/>
            /// Passing in ::CU_DEVICE_CPU for \p device will set the advice for the CPU.
            /// <para/>
            /// Note that this function is asynchronous with respect to the host and all work
            /// on other devices.
            /// </summary>
            /// <param name="devPtr">Pointer to memory to set the advice for</param>
            /// <param name="count">Size in bytes of the memory range</param>
            /// <param name="advice">Advice to be applied for the specified memory range</param>
            /// <param name="device">Device to apply the advice for</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAdvise(CUdeviceptr devPtr, SizeT count, CUmemAdvise advice, CUdevice device);


            /// <summary>
            /// Query an attribute of a given memory range
            /// </summary>
            /// <param name="data">A pointers to a memory location where the result of each attribute query will be written to.</param>
            /// <param name="dataSize">Array containing the size of data</param>
            /// <param name="attribute">The attribute to query</param>
            /// <param name="devPtr">Start of the range to query</param>
            /// <param name="count">Size of the range to query</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemRangeGetAttribute(IntPtr data, SizeT dataSize, CUmem_range_attribute attribute, CUdeviceptr devPtr, SizeT count);

            /// <summary>
            /// Query attributes of a given memory range.
            /// </summary>
            /// <param name="data">A two-dimensional array containing pointers to memory locations where the result of each attribute query will be written to.</param>
            /// <param name="dataSizes">Array containing the sizes of each result</param>
            /// <param name="attributes">An array of attributes to query (numAttributes and the number of attributes in this array should match)</param>
            /// <param name="numAttributes">Number of attributes to query</param>
            /// <param name="devPtr">Start of the range to query</param>
            /// <param name="count">Size of the range to query</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemRangeGetAttributes([In, Out] IntPtr[] data, [In, Out] SizeT[] dataSizes, [In, Out] CUmem_range_attribute[] attributes, SizeT numAttributes, CUdeviceptr devPtr, SizeT count);

            /// <summary>
            /// Allocates memory that will be automatically managed by the Unified Memory system
            /// <para/>
            /// Allocates <c>bytesize</c> bytes of managed memory on the device and returns in
            /// <c>dptr</c> a pointer to the allocated memory. If the device doesn't support
            /// allocating managed memory, <see cref="CUResult.ErrorNotSupported"/> is returned. Support
            /// for managed memory can be queried using the device attribute
            /// <see cref="CUDeviceAttribute.ManagedMemory"/>. The allocated memory is suitably
            /// aligned for any kind of variable. The memory is not cleared. If <c>bytesize</c>
            /// is 0, ::cuMemAllocManaged returns ::CUDA_ERROR_INVALID_VALUE. The pointer
            /// is valid on the CPU and on all GPUs in the system that support managed memory.
            /// All accesses to this pointer must obey the Unified Memory programming model.
            /// <para/>
            /// <c>flags</c> specifies the default stream association for this allocation.
            /// <c>flags</c> must be one of ::CU_MEM_ATTACH_GLOBAL or ::CU_MEM_ATTACH_HOST. If
            /// ::CU_MEM_ATTACH_GLOBAL is specified, then this memory is accessible from
            /// any stream on any device. If ::CU_MEM_ATTACH_HOST is specified, then the
            /// allocation is created with initial visibility restricted to host access only;
            /// an explicit call to ::cuStreamAttachMemAsync will be required to enable access
            /// on the device.
            /// <para/>
            /// If the association is later changed via ::cuStreamAttachMemAsync to
            /// a single stream, the default association as specifed during ::cuMemAllocManaged
            /// is restored when that stream is destroyed. For __managed__ variables, the
            /// default association is always ::CU_MEM_ATTACH_GLOBAL. Note that destroying a
            /// stream is an asynchronous operation, and as a result, the change to default
            /// association won't happen until all work in the stream has completed.
            /// <para/>
            /// Memory allocated with ::cuMemAllocManaged should be released with ::cuMemFree.
            /// <para/>
            /// On a multi-GPU system with peer-to-peer support, where multiple GPUs support
            /// managed memory, the physical storage is created on the GPU which is active
            /// at the time ::cuMemAllocManaged is called. All other GPUs will reference the
            /// data at reduced bandwidth via peer mappings over the PCIe bus. The Unified
            /// Memory management system does not migrate memory between GPUs.
            /// <para/>
            /// On a multi-GPU system where multiple GPUs support managed memory, but not
            /// all pairs of such GPUs have peer-to-peer support between them, the physical
            /// storage is created in 'zero-copy' or system memory. All GPUs will reference
            /// the data at reduced bandwidth over the PCIe bus. In these circumstances,
            /// use of the environment variable, CUDA_VISIBLE_DEVICES, is recommended to
            /// restrict CUDA to only use those GPUs that have peer-to-peer support. This
            /// environment variable is described in the CUDA programming guide under the
            /// "CUDA environment variables" section.
            /// </summary>
            /// <param name="dptr">Returned device pointer</param>
            /// <param name="bytesize">Requested allocation size in bytes</param>
            /// <param name="flags">Must be one of <see cref="CUmemAttach_flags.Global"/> or <see cref="CUmemAttach_flags.Host"/></param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorNotSupported"/>, , <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAllocManaged(ref CUdeviceptr dptr, SizeT bytesize, CUmemAttach_flags flags);


            /// <summary>
            /// Set attributes on a previously allocated memory region<para/>
            /// The supported attributes are:<para/>
            /// <see cref="CUPointerAttribute.SyncMemops"/>: A boolean attribute that can either be set (1) or unset (0). When set,
            /// memory operations that are synchronous. If there are some previously initiated
            /// synchronous memory operations that are pending when this attribute is set, the
            /// function does not return until those memory operations are complete.
            /// See further documentation in the section titled "API synchronization behavior"
            /// to learn more about cases when synchronous memory operations can
            /// exhibit asynchronous behavior.
            /// <c>value</c> will be considered as a pointer to an unsigned integer to which this attribute is to be set.
            /// </summary>
            /// <param name="value">Pointer to memory containing the value to be set</param>
            /// <param name="attribute">Pointer attribute to set</param>
            /// <param name="ptr">Pointer to a memory region allocated using CUDA memory allocation APIs</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidDevice"/></returns>.
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerSetAttribute(ref int value, CUPointerAttribute attribute, CUdeviceptr ptr);

            /// <summary>
            /// Returns information about a pointer.<para/>
            /// The supported attributes are (refer to ::cuPointerGetAttribute for attribute descriptions and restrictions):
            /// <para/>
            /// - ::CU_POINTER_ATTRIBUTE_CONTEXT<para/>
            /// - ::CU_POINTER_ATTRIBUTE_MEMORY_TYPE<para/>
            /// - ::CU_POINTER_ATTRIBUTE_DEVICE_POINTER<para/>
            /// - ::CU_POINTER_ATTRIBUTE_HOST_POINTER<para/>
            /// - ::CU_POINTER_ATTRIBUTE_SYNC_MEMOPS<para/>
            /// - ::CU_POINTER_ATTRIBUTE_BUFFER_ID<para/>
            /// - ::CU_POINTER_ATTRIBUTE_IS_MANAGED<para/>
            /// </summary>
            /// <param name="numAttributes">Number of attributes to query</param>
            /// <param name="attributes">An array of attributes to query (numAttributes and the number of attributes in this array should match)</param>
            /// <param name="data">A two-dimensional array containing pointers to memory
            /// locations where the result of each attribute query will be written to.</param>
            /// <param name="ptr">Pointer to query</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuPointerGetAttributes(uint numAttributes, [In, Out] CUPointerAttribute[] attributes, IntPtr data, CUdeviceptr ptr);

            /// <summary>
            /// Allocate an address range reservation. <para/>
            /// Reserves a virtual address range based on the given parameters, giving
            /// the starting address of the range in \p ptr.This API requires a system that
            /// supports UVA.The size and address parameters must be a multiple of the
            /// host page size and the alignment must be a power of two or zero for default
            /// alignment.
            /// </summary>
            /// <param name="ptr">Resulting pointer to start of virtual address range allocated</param>
            /// <param name="size">Size of the reserved virtual address range requested</param>
            /// <param name="alignment">Alignment of the reserved virtual address range requested</param>
            /// <param name="addr">Fixed starting address range requested</param>
            /// <param name="flags">Currently unused, must be zero</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAddressReserve(ref CUdeviceptr ptr, SizeT size, SizeT alignment, CUdeviceptr addr, ulong flags);

            /// <summary>
            /// Free an address range reservation.<para/>
            /// Frees a virtual address range reserved by cuMemAddressReserve.  The size
            /// must match what was given to memAddressReserve and the ptr given must
            /// match what was returned from memAddressReserve.
            /// </summary>
            /// <param name="ptr">Starting address of the virtual address range to free</param>
            /// <param name="size">Size of the virtual address region to free</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemAddressFree(CUdeviceptr ptr, SizeT size);


            /// <summary>
            /// Create a shareable memory handle representing a memory allocation of a given size described by the given properties <para/>
            /// This creates a memory allocation on the target device specified through the
            /// \p prop strcuture.The created allocation will not have any device or host
            /// mappings.The generic memory \p handle for the allocation can be
            /// mapped to the address space of calling process via::cuMemMap.This handle
            /// cannot be transmitted directly to other processes(see
            /// ::cuMemExportToShareableHandle).  On Windows, the caller must also pass
            /// an LPSECURITYATTRIBUTE in \p prop to be associated with this handle which
            /// limits or allows access to this handle for a recepient process (see
            /// ::CUmemAllocationProp::win32HandleMetaData for more).  The \p size of this
            /// allocation must be a multiple of the the value given via
            /// ::cuMemGetAllocationGranularity with the ::CU_MEM_ALLOC_GRANULARITY_MINIMUM
            /// flag.
            /// </summary>
            /// <param name="handle">Value of handle returned. All operations on this allocation are to be performed using this handle.</param>
            /// <param name="size">Size of the allocation requested</param>
            /// <param name="prop">Properties of the allocation to create.</param>
            /// <param name="flags">flags for future use, must be zero now.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemCreate(ref CUmemGenericAllocationHandle handle, SizeT size, ref CUmemAllocationProp prop, ulong flags);


            /// <summary>
            /// Release a memory handle representing a memory allocation which was previously allocated through cuMemCreate.<para/>
            /// Frees the memory that was allocated on a device through cuMemCreate.
            /// <para/>
            /// The memory allocation will be freed when all outstanding mappings to the memory
            /// are unmapped and when all outstanding references to the handle(including it's
            /// shareable counterparts) are also released.The generic memory handle can be
            /// freed when there are still outstanding mappings made with this handle.Each
            /// time a recepient process imports a shareable handle, it needs to pair it with
            /// ::cuMemRelease for the handle to be freed.If \p handle is not a valid handle
            /// the behavior is undefined.
            /// </summary>
            /// <param name="handle">handle Value of handle which was returned previously by cuMemCreate.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemRelease(CUmemGenericAllocationHandle handle);


            /// <summary>
            /// Maps an allocation handle to a reserved virtual address range.<para/>
            /// Maps bytes of memory represented by \p handle starting from byte \p offset to
            /// \p size to address range[\p addr, \p addr + \p size]. This range must be an<para/>
            /// address reservation previously reserved with ::cuMemAddressReserve, and
            /// \p offset + \p size must be less than the size of the memory allocation.
            /// Both \p ptr, \p size, and \p offset must be a multiple of the value given via
            /// ::cuMemGetAllocationGranularity with the::CU_MEM_ALLOC_GRANULARITY_MINIMUM flag.<para/>
            /// Please note calling::cuMemMap does not make the address accessible,
            /// the caller needs to update accessibility of a contiguous mapped VA
            /// range by calling::cuMemSetAccess.<para/>
            /// Once a recipient process obtains a shareable memory handle
            /// from::cuMemImportFromShareableHandle, the process must
            /// use ::cuMemMap to map the memory into its address ranges before
            /// setting accessibility with::cuMemSetAccess.<para/>
            /// ::cuMemMap can only create mappings on VA range reservations
            /// that are not currently mapped.
            /// </summary>
            /// <param name="ptr">Address where memory will be mapped. </param>
            /// <param name="size">Size of the memory mapping. </param>
            /// <param name="offset">Offset into the memory represented by \p handle from which to start mapping - Note: currently must be zero.</param>
            /// <param name="handle">Handle to a shareable memory </param>
            /// <param name="flags">flags for future use, must be zero now. </param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemMap(CUdeviceptr ptr, SizeT size, SizeT offset, CUmemGenericAllocationHandle handle, ulong flags);

            /// <summary>
            /// Maps or unmaps subregions of sparse CUDA arrays and sparse CUDA mipmapped arrays
            /// </summary>
            /// <param name="mapInfoList">List of ::CUarrayMapInfo</param>
            /// <param name="count">Count of ::CUarrayMapInfo  in \p mapInfoList</param>
            /// <param name="hStream">Stream identifier for the stream to use for map or unmap operations</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "pcuMemMapArrayAsync" + CUDA_PTSZ)]
            public static extern CUResult pcuMemMapArrayAsync(CUarrayMapInfo[] mapInfoList, uint count, CUstream hStream);



            /// <summary>
            /// Unmap the backing memory of a given address range.<para/>
            /// The range must be the entire contiguous address range that was mapped to.  In
            /// other words, ::cuMemUnmap cannot unmap a sub-range of an address range mapped
            /// by::cuMemCreate / ::cuMemMap.Any backing memory allocations will be freed
            /// if there are no existing mappings and there are no unreleased memory handles.<para/>
            /// When::cuMemUnmap returns successfully the address range is converted to an
            /// address reservation and can be used for a future calls to ::cuMemMap.Any new
            /// mapping to this virtual address will need to have access granted through
            /// ::cuMemSetAccess, as all mappings start with no accessibility setup.
            /// </summary>
            /// <param name="ptr">Starting address for the virtual address range to unmap</param>
            /// <param name="size">Size of the virtual address range to unmap</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemUnmap(CUdeviceptr ptr, SizeT size);


            /// <summary>
            /// Set the access flags for each location specified in \p desc for the given virtual address range<para/>
            /// Given the virtual address range via \p ptr and \p size, and the locations
            /// in the array given by \p desc and \p count, set the access flags for the
            /// target locations.The range must be a fully mapped address range
            /// containing all allocations created by ::cuMemMap / ::cuMemCreate.
            /// </summary>
            /// <param name="ptr">Starting address for the virtual address range</param>
            /// <param name="size">Length of the virtual address range</param>
            /// <param name="desc">Array of ::CUmemAccessDesc that describe how to change the mapping for each location specified</param>
            /// <param name="count">Number of ::CUmemAccessDesc in \p desc</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemSetAccess(CUdeviceptr ptr, SizeT size, CUmemAccessDesc[] desc, SizeT count);


            /// <summary>
            /// Get the access \p flags set for the given \p location and \p ptr
            /// </summary>
            /// <param name="flags">Flags set for this location</param>
            /// <param name="location">Location in which to check the flags for</param>
            /// <param name="ptr">Address in which to check the access flags for</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemGetAccess(ref ulong flags, ref CUmemLocation location, CUdeviceptr ptr);


            /// <summary>
            /// Exports an allocation to a requested shareable handle type<para/>
            /// Given a CUDA memory handle, create a shareable memory
            /// allocation handle that can be used to share the memory with other
            /// processes.The recipient process can convert the shareable handle back into a
            /// CUDA memory handle using ::cuMemImportFromShareableHandle and map
            /// it with::cuMemMap.The implementation of what this handle is and how it
            /// can be transferred is defined by the requested handle type in \p handleType<para/>
            /// Once all shareable handles are closed and the allocation is released, the allocated
            /// memory referenced will be released back to the OS and uses of the CUDA handle afterward
            /// will lead to undefined behavior.<para/>
            /// This API can also be used in conjunction with other APIs (e.g.Vulkan, OpenGL)
            /// that support importing memory from the shareable type
            /// </summary>
            /// <param name="shareableHandle">Pointer to the location in which to store the requested handle type</param>
            /// <param name="handle">CUDA handle for the memory allocation</param>
            /// <param name="handleType">Type of shareable handle requested (defines type and size of the \p shareableHandle output parameter)</param>
            /// <param name="flags">Reserved, must be zero</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemExportToShareableHandle(IntPtr shareableHandle, CUmemGenericAllocationHandle handle, CUmemAllocationHandleType handleType, ulong flags);


            /// <summary>
            /// Imports an allocation from a requested shareable handle type.<para/>
            /// If the current process cannot support the memory described by this shareable
            /// handle, this API will error as CUDA_ERROR_NOT_SUPPORTED.<para/>
            /// \note Importing shareable handles exported from some graphics APIs(Vulkan, OpenGL, etc)
            /// created on devices under an SLI group may not be supported, and thus this API will
            /// return CUDA_ERROR_NOT_SUPPORTED.
            /// There is no guarantee that the contents of \p handle will be the same CUDA memory handle
            /// for the same given OS shareable handle, or the same underlying allocation.
            /// </summary>
            /// <param name="handle">CUDA Memory handle for the memory allocation.</param>
            /// <param name="osHandle">Shareable Handle representing the memory allocation that is to be imported. </param>
            /// <param name="shHandleType">handle type of the exported handle ::CUmemAllocationHandleType.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemImportFromShareableHandle(ref CUmemGenericAllocationHandle handle, IntPtr osHandle, CUmemAllocationHandleType shHandleType);


            /// <summary>
            /// Calculates either the minimal or recommended granularity <para/>
            /// Calculates either the minimal or recommended granularity
            /// for a given allocation specification and returns it in granularity.This
            /// granularity can be used as a multiple for alignment, size, or address mapping.
            /// </summary>
            /// <param name="granularity">granularity Returned granularity.</param>
            /// <param name="prop">prop Property for which to determine the granularity for</param>
            /// <param name="option">option Determines which granularity to return</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemGetAllocationGranularity(ref SizeT granularity, ref CUmemAllocationProp prop, CUmemAllocationGranularity_flags option);


            /// <summary>
            /// Retrieve the contents of the property structure defining properties for this handle
            /// </summary>
            /// <param name="prop">Pointer to a properties structure which will hold the information about this handle</param>
            /// <param name="handle">Handle which to perform the query on</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemGetAllocationPropertiesFromHandle(ref CUmemAllocationProp prop, CUmemGenericAllocationHandle handle);


            /// <summary>
            /// Given an address \p addr, returns the allocation handle of the backing memory allocation.<para/>
            /// The handle is guaranteed to be the same handle value used to map the memory. If the address
            /// requested is not mapped, the function will fail.The returned handle must be released with
            /// corresponding number of calls to::cuMemRelease.<para/>
            /// <para/>
            /// The address \p addr, can be any address in a range previously mapped
            /// by::cuMemMap, and not necessarily the start address.
            /// </summary>
            /// <param name="handle">CUDA Memory handle for the backing memory allocation.</param>
            /// <param name="addr">Memory address to query, that has been mapped previously.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemRetainAllocationHandle(ref CUmemGenericAllocationHandle handle, IntPtr addr);

            /// <summary>
            /// Frees memory with stream ordered semantics<para/>
            /// Inserts a free operation into \p hStream.<para/>
            /// The allocation must not be accessed after stream execution reaches the free.
            /// After this API returns, accessing the memory from any subsequent work launched on the GPU
            /// or querying its pointer attributes results in undefined behavior.
            /// </summary>
            /// <param name="dptr">memory to free</param>
            /// <param name="hStream">The stream establishing the stream ordering contract.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemFreeAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemFreeAsync(CUdeviceptr dptr, CUstream hStream);

            /// <summary>
            /// Allocates memory with stream ordered semantics<para/>
            /// Inserts an allocation operation into \p hStream.<para/>
            /// A pointer to the allocated memory is returned immediately in *dptr.<para/>
            /// The allocation must not be accessed until the the allocation operation completes.<para/>
            /// The allocation comes from the memory pool current to the stream's device.<para/>
            /// <para/>
            /// note The default memory pool of a device contains device memory from that device.<para/>
            /// note Basic stream ordering allows future work submitted into the same stream to use the allocation.
            /// Stream query, stream synchronize, and CUDA events can be used to guarantee that the allocation
            /// operation completes before work submitted in a separate stream runs. 
            /// </summary>
            /// <param name="dptr">Returned device pointer</param>
            /// <param name="bytesize">Number of bytes to allocate</param>
            /// <param name="hStream">The stream establishing the stream ordering contract and the memory pool to allocate from</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemAllocAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemAllocAsync(ref CUdeviceptr dptr, SizeT bytesize, CUstream hStream);

            /// <summary>
            /// Tries to release memory back to the OS<para/>
            /// Releases memory back to the OS until the pool contains fewer than minBytesToKeep
            /// reserved bytes, or there is no more memory that the allocator can safely release.<para/>
            /// The allocator cannot release OS allocations that back outstanding asynchronous allocations.<para/>
            /// The OS allocations may happen at different granularity from the user allocations.<para/>
            /// <para/>
            /// note: Allocations that have not been freed count as outstanding.<para/>
            /// note: Allocations that have been asynchronously freed but whose completion has
            /// not been observed on the host (eg.by a synchronize) can count as outstanding.
            /// </summary>
            /// <param name="pool">The memory pool to trim</param>
            /// <param name="minBytesToKeep">If the pool has less than minBytesToKeep reserved,
            /// the TrimTo operation is a no-op.Otherwise the pool will be guaranteed to have at least minBytesToKeep bytes reserved after the operation.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolTrimTo(CUmemoryPool pool, SizeT minBytesToKeep);

            /// <summary>
            /// Sets attributes of a memory pool<para/>
            /// Supported attributes are:<para/>
            /// - ::CU_MEMPOOL_ATTR_RELEASE_THRESHOLD: (value type = cuuint64_t)<para/>
            /// Amount of reserved memory in bytes to hold onto before trying to release memory back to the OS.When more than the release
            /// threshold bytes of memory are held by the memory pool, the allocator will try to release memory back to the OS on the next 
            /// call to stream, event or context synchronize. (default 0)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_FOLLOW_EVENT_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to use memory asynchronously freed
            /// in another stream as long as a stream ordering dependency
            /// of the allocating stream on the free action exists.
            /// Cuda events and null stream interactions can create the required
            /// stream ordered dependencies. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_OPPORTUNISTIC: (value type = int)<para/>
            /// Allow reuse of already completed frees when there is no dependency
            /// between the free and allocation. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_INTERNAL_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to insert new stream dependencies
            /// in order to establish the stream ordering required to reuse
            /// a piece of memory released by::cuMemFreeAsync(default enabled).
            /// </summary>
            /// <param name="pool">The memory pool to modify</param>
            /// <param name="attr">The attribute to modify</param>
            /// <param name="value">Pointer to the value to assign</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolSetAttribute(CUmemoryPool pool, CUmemPool_attribute attr, ref int value);

            /// <summary>
            /// Sets attributes of a memory pool<para/>
            /// Supported attributes are:<para/>
            /// - ::CU_MEMPOOL_ATTR_RELEASE_THRESHOLD: (value type = cuuint64_t)<para/>
            /// Amount of reserved memory in bytes to hold onto before trying to release memory back to the OS.When more than the release
            /// threshold bytes of memory are held by the memory pool, the allocator will try to release memory back to the OS on the next 
            /// call to stream, event or context synchronize. (default 0)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_FOLLOW_EVENT_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to use memory asynchronously freed
            /// in another stream as long as a stream ordering dependency
            /// of the allocating stream on the free action exists.
            /// Cuda events and null stream interactions can create the required
            /// stream ordered dependencies. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_OPPORTUNISTIC: (value type = int)<para/>
            /// Allow reuse of already completed frees when there is no dependency
            /// between the free and allocation. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_INTERNAL_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to insert new stream dependencies
            /// in order to establish the stream ordering required to reuse
            /// a piece of memory released by::cuMemFreeAsync(default enabled).
            /// </summary>
            /// <param name="pool">The memory pool to modify</param>
            /// <param name="attr">The attribute to modify</param>
            /// <param name="value">Pointer to the value to assign</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolSetAttribute(CUmemoryPool pool, CUmemPool_attribute attr, ref ulong value);

            /// <summary>
            /// Gets attributes of a memory pool<para/>
            /// Supported attributes are:<para/>
            /// - ::CU_MEMPOOL_ATTR_RELEASE_THRESHOLD: (value type = cuuint64_t)<para/>
            /// Amount of reserved memory in bytes to hold onto before trying
            /// to release memory back to the OS.When more than the release
            /// threshold bytes of memory are held by the memory pool, the
            /// allocator will try to release memory back to the OS on the
            /// next call to stream, event or context synchronize. (default 0)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_FOLLOW_EVENT_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to use memory asynchronously freed
            /// in another stream as long as a stream ordering dependency
            /// of the allocating stream on the free action exists.
            /// Cuda events and null stream interactions can create the required
            /// stream ordered dependencies. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_OPPORTUNISTIC: (value type = int)<para/>
            /// Allow reuse of already completed frees when there is no dependency between the free and allocation. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_INTERNAL_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to insert new stream dependencies in order to establish the stream ordering 
            /// required to reuse a piece of memory released by::cuMemFreeAsync(default enabled).
            /// </summary>
            /// <param name="pool">The memory pool to get attributes of</param>
            /// <param name="attr">The attribute to get</param>
            /// <param name="value">Retrieved value</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolGetAttribute(CUmemoryPool pool, CUmemPool_attribute attr, ref int value);

            /// <summary>
            /// Gets attributes of a memory pool<para/>
            /// Supported attributes are:<para/>
            /// - ::CU_MEMPOOL_ATTR_RELEASE_THRESHOLD: (value type = cuuint64_t)<para/>
            /// Amount of reserved memory in bytes to hold onto before trying
            /// to release memory back to the OS.When more than the release
            /// threshold bytes of memory are held by the memory pool, the
            /// allocator will try to release memory back to the OS on the
            /// next call to stream, event or context synchronize. (default 0)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_FOLLOW_EVENT_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to use memory asynchronously freed
            /// in another stream as long as a stream ordering dependency
            /// of the allocating stream on the free action exists.
            /// Cuda events and null stream interactions can create the required
            /// stream ordered dependencies. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_OPPORTUNISTIC: (value type = int)<para/>
            /// Allow reuse of already completed frees when there is no dependency between the free and allocation. (default enabled)<para/>
            /// - ::CU_MEMPOOL_ATTR_REUSE_ALLOW_INTERNAL_DEPENDENCIES: (value type = int)<para/>
            /// Allow::cuMemAllocAsync to insert new stream dependencies in order to establish the stream ordering 
            /// required to reuse a piece of memory released by::cuMemFreeAsync(default enabled).
            /// </summary>
            /// <param name="pool">The memory pool to get attributes of</param>
            /// <param name="attr">The attribute to get</param>
            /// <param name="value">Retrieved value</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolGetAttribute(CUmemoryPool pool, CUmemPool_attribute attr, ref ulong value);

            /// <summary>
            /// Controls visibility of pools between devices
            /// </summary>
            /// <param name="pool">The pool being modified</param>
            /// <param name="map">Array of access descriptors. Each descriptor instructs the access to enable for a single gpu.</param>
            /// <param name="count">Number of descriptors in the map array.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolSetAccess(CUmemoryPool pool, CUmemAccessDesc[] map, SizeT count);

            /// <summary>
            /// Returns the accessibility of a pool from a device<para/>
            /// Returns the accessibility of the pool's memory from the specified location.
            /// </summary>
            /// <param name="flags">the accessibility of the pool from the specified location</param>
            /// <param name="memPool">the pool being queried</param>
            /// <param name="location">the location accessing the pool</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolGetAccess(ref CUmemAccess_flags flags, CUmemoryPool memPool, ref CUmemLocation location);

            /// <summary>
            /// Creates a memory pool<para/>
            /// Creates a CUDA memory pool and returns the handle in \p pool. The \p poolProps determines
            /// the properties of the pool such as the backing device and IPC capabilities.<para/>
            /// By default, the pool's memory will be accessible from the device it is allocated on.<para/>
            /// note Specifying CU_MEM_HANDLE_TYPE_NONE creates a memory pool that will not support IPC.
            /// </summary>
            /// <param name="pool"></param>
            /// <param name="poolProps"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolCreate(ref CUmemoryPool pool, ref CUmemPoolProps poolProps);

            /// <summary>
            /// Destroys the specified memory pool<para/>
            /// If any pointers obtained from this pool haven't been freed or
            /// the pool has free operations that haven't completed
            /// when::cuMemPoolDestroy is invoked, the function will return immediately and the
            /// resources associated with the pool will be released automatically
            /// once there are no more outstanding allocations.<para/>
            /// Destroying the current mempool of a device sets the default mempool of
            /// that device as the current mempool for that device.<para/>
            /// note A device's default memory pool cannot be destroyed.
            /// </summary>
            /// <param name="pool"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolDestroy(CUmemoryPool pool);

            /// <summary>
            /// Allocates memory from a specified pool with stream ordered semantics.<para/>
            /// Inserts an allocation operation into \p hStream.<para/>
            /// A pointer to the allocated memory is returned immediately in *dptr.<para/>
            /// The allocation must not be accessed until the the allocation operation completes.<para/>
            /// The allocation comes from the specified memory pool.<para/>
            /// note<para/>
            /// -  The specified memory pool may be from a device different than that of the specified \p hStream.<para/>
            /// -  Basic stream ordering allows future work submitted into the same stream to use the allocation.
            /// Stream query, stream synchronize, and CUDA events can be used to guarantee that the allocation
            /// operation completes before work submitted in a separate stream runs. 
            /// </summary>
            /// <param name="dptr">Returned device pointer</param>
            /// <param name="bytesize">Number of bytes to allocate</param>
            /// <param name="pool">The pool to allocate from</param>
            /// <param name="hStream">The stream establishing the stream ordering semantic</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemAllocFromPoolAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemAllocFromPoolAsync(ref CUdeviceptr dptr, SizeT bytesize, CUmemoryPool pool, CUstream hStream);

            /// <summary>
            /// Exports a memory pool to the requested handle type.<para/>
            /// Given an IPC capable mempool, create an OS handle to share the pool with another process.<para/>
            /// A recipient process can convert the shareable handle into a mempool with::cuMemPoolImportFromShareableHandle.
            /// Individual pointers can then be shared with the ::cuMemPoolExportPointer and ::cuMemPoolImportPointer APIs.
            /// The implementation of what the shareable handle is and how it can be transferred is defined by the requested
            /// handle type.<para/>
            /// note: To create an IPC capable mempool, create a mempool with a CUmemAllocationHandleType other than CU_MEM_HANDLE_TYPE_NONE.
            /// </summary>
            /// <param name="handle_out">Returned OS handle</param>
            /// <param name="pool">pool to export</param>
            /// <param name="handleType">the type of handle to create</param>
            /// <param name="flags">must be 0</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolExportToShareableHandle(ref IntPtr handle_out, CUmemoryPool pool, CUmemAllocationHandleType handleType, ulong flags);

            /// <summary>
            /// imports a memory pool from a shared handle.<para/>
            /// Specific allocations can be imported from the imported pool with cuMemPoolImportPointer.<para/>
            /// note Imported memory pools do not support creating new allocations. As such imported memory pools 
            /// may not be used in cuDeviceSetMemPool or ::cuMemAllocFromPoolAsync calls.
            /// </summary>
            /// <param name="pool_out">Returned memory pool</param>
            /// <param name="handle">OS handle of the pool to open</param>
            /// <param name="handleType">The type of handle being imported</param>
            /// <param name="flags">must be 0</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolImportFromShareableHandle(
                    ref CUmemoryPool pool_out,
                    IntPtr handle,
                    CUmemAllocationHandleType handleType,
                    ulong flags);

            /// <summary>
            /// Export data to share a memory pool allocation between processes.<para/>
            /// Constructs \p shareData_out for sharing a specific allocation from an already shared memory pool.<para/>
            /// The recipient process can import the allocation with the::cuMemPoolImportPointer api.<para/>
            /// The data is not a handle and may be shared through any IPC mechanism.
            /// </summary>
            /// <param name="shareData_out">Returned export data</param>
            /// <param name="ptr">pointer to memory being exported</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolExportPointer(ref CUmemPoolPtrExportData shareData_out, CUdeviceptr ptr);

            /// <summary>
            /// Import a memory pool allocation from another process.<para/>
            /// Returns in \p ptr_out a pointer to the imported memory.<para/>
            /// The imported memory must not be accessed before the allocation operation completes
            /// in the exporting process.The imported memory must be freed from all importing processes before
            /// being freed in the exporting process.The pointer may be freed with cuMemFree
            /// or cuMemFreeAsync.If cuMemFreeAsync is used, the free must be completed
            /// on the importing process before the free operation on the exporting process.<para/>
            /// note The cuMemFreeAsync api may be used in the exporting process before
            /// the cuMemFreeAsync operation completes in its stream as long as the
            /// cuMemFreeAsync in the exporting process specifies a stream with
            /// a stream dependency on the importing process's cuMemFreeAsync.
            /// </summary>
            /// <param name="ptr_out">pointer to imported memory</param>
            /// <param name="pool">pool from which to import</param>
            /// <param name="shareData">data specifying the memory to import</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMemPoolImportPointer(ref CUdeviceptr ptr_out, CUmemoryPool pool, ref CUmemPoolPtrExportData shareData);
        }
        #endregion

        #region Synchronous Memcpy_v2
        /// <summary>
        /// Intra-device memcpy's done with these functions may execute in parallel with the CPU,
        /// but if host memory is involved, they wait until the copy is done before returning.
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class SynchronousMemcpy_v2
        {
#if (NETCOREAPP)
            static SynchronousMemcpy_v2()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            //New memcpy functions in CUDA 4.0 for unified addressing
            /// <summary>
            /// Copies data between two pointers. <para/>
            /// <c>dst</c> and <c>src</c> are base pointers of the destination and source, respectively.  
            /// <c>ByteCount</c> specifies the number of bytes to copy.
            /// Note that this function infers the type of the transfer (host to host, host to 
            /// device, device to device, or device to host) from the pointer values.  This
            /// function is only allowed in contexts which support unified addressing.
            /// Note that this function is synchronous.
            /// </summary>
            /// <param name="dst">Destination unified virtual address space pointer</param>
            /// <param name="src">Source unified virtual address space pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy" + CUDA_PTDS)]
            public static extern CUResult cuMemcpy(CUdeviceptr dst, CUdeviceptr src, SizeT ByteCount);

            /// <summary>
            /// Copies from device memory in one context to device memory in another
            /// context. <c>dstDevice</c> is the base device pointer of the destination memory 
            /// and <c>dstContext</c> is the destination context.  <c>srcDevice</c> is the base 
            /// device pointer of the source memory and <c>srcContext</c> is the source pointer.  
            /// <c>ByteCount</c> specifies the number of bytes to copy.
            /// <para/>
            /// Note that this function is asynchronous with respect to the host, but 
            /// serialized with respect all pending and future asynchronous work in to the 
            /// current context, <c>srcContext</c>, and <c>dstContext</c> (use <see cref="AsynchronousMemcpy_v2.cuMemcpyPeerAsync"/> 
            /// to avoid this synchronization).
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstContext">Destination context</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="srcContext">Source context</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyPeer" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyPeer(CUdeviceptr dstDevice, CUcontext dstContext, CUdeviceptr srcDevice, CUcontext srcContext, SizeT ByteCount);

            /// <summary>
            /// Perform a 3D memory copy according to the parameters specified in
            /// <c>pCopy</c>.  See the definition of the <see cref="CUDAMemCpy3DPeer"/> structure
            /// for documentation of its parameters.<para/>
            /// Note that this function is synchronous with respect to the host only if
            /// the source or destination memory is of type ::CU_MEMORYTYPE_HOST.
            /// Note also that this copy is serialized with respect all pending and future 
            /// asynchronous work in to the current context, the copy's source context,
            /// and the copy's destination context (use <see cref="AsynchronousMemcpy_v2.cuMemcpy3DPeerAsync"/> to avoid 
            /// this synchronization).
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy3DPeer" + CUDA_PTDS)]
            public static extern CUResult cuMemcpy3DPeer(ref CUDAMemCpy3DPeer pCopy);



            // 1D functions
            // system <-> device memory
            #region VectorTypesArray
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] dim3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] char1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] char2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] char3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] char4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uchar1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uchar2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uchar3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uchar4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] short1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] short2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] short3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] short4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ushort1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ushort2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ushort3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ushort4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] int1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] int2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] int3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] int4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uint1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uint2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uint3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uint4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] long1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] long2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] long3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] long4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ulong1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ulong2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ulong3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ulong4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] float1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] float2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] float3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] float4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] double1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] double2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] cuDoubleComplex[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] cuDoubleReal[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] cuFloatComplex[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] cuFloatReal[] srcHost, SizeT ByteCount);
            #endregion
            #region NumberTypesArray
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] byte[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] sbyte[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ushort[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] short[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] uint[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] int[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ulong[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] long[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] float[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] double[] srcHost, SizeT ByteCount);
            #endregion
            #region VectorTypes
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref dim3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref char1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref char2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref char3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref char4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uchar1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uchar2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uchar3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uchar4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref short1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref short2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref short3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref short4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ushort1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ushort2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ushort3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ushort4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref int1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref int2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref int3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref int4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uint1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uint2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uint3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uint4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref long1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref long2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref long3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref long4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ulong1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ulong2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ulong3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ulong4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref float1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref float2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref float3 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref float4 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref double1 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref double2 srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref cuDoubleComplex srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref cuDoubleReal srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref cuFloatComplex srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref cuFloatReal srcHost, SizeT ByteCount);
            #endregion
            #region NumberTypes
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref byte srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref sbyte srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ushort srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref short srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref uint srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref int srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref ulong srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref long srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref float srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] ref double srcHost, SizeT ByteCount);
            #endregion
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoD_v2(CUdeviceptr dstDevice, [In] IntPtr srcHost, SizeT ByteCount);


            //Device to Host
            #region VectorTypesArray
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] dim3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] char1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] char2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] char3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] char4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uchar1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uchar2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uchar3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uchar4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] short1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] short2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] short3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] short4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ushort1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ushort2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ushort3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ushort4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] int1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] int2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] int3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] int4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uint1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uint2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uint3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uint4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] long1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] long2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] long3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] long4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ulong1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ulong2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ulong3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ulong4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] float1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] float2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] float3[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] float4[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] double1[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] double2[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] cuDoubleComplex[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] cuDoubleReal[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns> 
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] cuFloatComplex[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] cuFloatReal[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            #endregion
            #region NumberTypesArray
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] byte[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] sbyte[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ushort[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] short[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] uint[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] int[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] ulong[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] long[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] float[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] double[] dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            #endregion
            #region VectorTypes
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref dim3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref char1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref char2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref char3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref char4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uchar1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uchar2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uchar3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uchar4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref short1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref short2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref short3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref short4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ushort1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ushort2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ushort3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ushort4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref int1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref int2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref int3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref int4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uint1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uint2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uint3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uint4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref long1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref long2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref long3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref long4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ulong1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ulong2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ulong3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ulong4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref float1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref float2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref float3 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref float4 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref double1 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref double2 dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref cuDoubleComplex dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref cuDoubleReal dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref cuFloatComplex dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref cuFloatReal dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            #endregion
            #region NumberTypes
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref byte dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref sbyte dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ushort dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref short dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref uint dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref int dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref ulong dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref long dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref float dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2(ref double dstHost, CUdeviceptr srcDevice, SizeT ByteCount);
            #endregion
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is synchronous.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoH_v2([Out] IntPtr dstHost, CUdeviceptr srcDevice, SizeT ByteCount);

            // device <-> device memory
            /// <summary>
            /// Copies from device memory to device memory. <c>dstDevice</c> and <c>srcDevice</c> are the base pointers of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is asynchronous.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoD_v2(CUdeviceptr dstDevice, CUdeviceptr srcDevice, SizeT ByteCount);

            // device <-> array memory
            /// <summary>
            /// Copies from device memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting index of the destination data. <c>srcDevice</c> specifies the base pointer of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyDtoA_v2(CUarray dstArray, SizeT dstOffset, CUdeviceptr srcDevice, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to device memory. <c>dstDevice</c> specifies the base pointer of the destination and
            /// must be naturally aligned with the CUDA array elements. <c>srcArray</c> and <c>srcOffset</c> specify the CUDA array
            /// handle and the offset in bytes into the array where the copy is to begin. <c>ByteCount</c> specifies the number of bytes to
            /// copy and must be evenly divisible by the array element size.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes. Must be evenly divisible by the array element size.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoD_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoD_v2(CUdeviceptr dstDevice, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);

            // system <-> array memory
            #region VectorTypesArray
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] dim3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] char1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] char2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] char3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] char4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uchar1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uchar2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uchar3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uchar4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] short1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] short2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] short3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] short4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ushort1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ushort2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ushort3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ushort4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] int1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] int2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] int3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] int4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uint1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uint2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uint3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uint4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] long1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] long2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] long3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] long4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ulong1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ulong2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ulong3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ulong4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] float1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] float2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] float3[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] float4[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] double1[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] double2[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] cuDoubleComplex[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] cuDoubleReal[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] cuFloatComplex[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] cuFloatReal[] srcHost, SizeT ByteCount);
            #endregion
            #region NumberTypesArray
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] byte[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] sbyte[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ushort[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] short[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] uint[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] int[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] ulong[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] long[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] float[] srcHost, SizeT ByteCount);
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] double[] srcHost, SizeT ByteCount);
            #endregion
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>pSrc</c> specifies the base address of the source. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyHtoA_v2(CUarray dstArray, SizeT dstOffset, [In] IntPtr srcHost, SizeT ByteCount);

            #region VectorTypesArray
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] dim3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] char1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] char2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] char3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] char4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uchar1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uchar2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uchar3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uchar4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] short1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] short2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] short3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] short4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ushort1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ushort2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ushort3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ushort4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] int1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] int2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] int3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] int4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uint1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uint2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uint3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uint4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] long1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] long2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] long3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] long4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ulong1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ulong2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ulong3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ulong4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] float1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] float2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] float3[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] float4[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] double1[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] double2[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] cuDoubleComplex[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] cuDoubleReal[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] cuFloatComplex[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] cuFloatReal[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            #endregion
            #region NumberTypesArray
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] byte[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] sbyte[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ushort[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] short[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] uint[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] int[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] ulong[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] long[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] float[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] double[] dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);
            #endregion

            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.
            /// </summary>
            /// <param name="dstHost">Destination device pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoH_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoH_v2([Out] IntPtr dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);

            // array <-> array memory
            /// <summary>
            /// Copies from one 1D CUDA array to another. <c>dstArray</c> and <c>srcArray</c> specify the handles of the destination and
            /// source CUDA arrays for the copy, respectively. <c>dstOffset</c> and <c>srcOffset</c> specify the destination and source
            /// offsets in bytes into the CUDA arrays. <c>ByteCount</c> is the number of bytes to be copied. The size of the elements
            /// in the CUDA arrays need not be the same format, but the elements must be the same size; and count must be evenly
            /// divisible by that size.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoA_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpyAtoA_v2(CUarray dstArray, SizeT dstOffset, CUarray srcArray, SizeT srcOffset, SizeT ByteCount);

            // 2D memcpy
            /// <summary>
            /// Perform a 2D memory copy according to the parameters specified in <c>pCopy</c>. See <see cref="CUDAMemCpy2D"/>.
            /// <see cref="cuMemcpy2D_v2"/> returns an error if any pitch is greater than the maximum allowed (<see cref="CUDeviceProperties.memPitch"/>).
			/// <see cref="MemoryManagement.cuMemAllocPitch_v2"/> passes back pitches that always work with <see cref="cuMemcpy2D_v2"/>. On intra-device
            /// memory copies (device <![CDATA[<->]]> device, CUDA array <![CDATA[<->]]> device, CUDA array <![CDATA[<->]]> CUDA array), <see cref="cuMemcpy2D_v2"/> may fail
			/// for pitches not computed by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>. <see cref="cuMemcpy2DUnaligned_v2"/> does not have this restriction, but
            /// may run significantly slower in the cases where <see cref="cuMemcpy2D_v2"/> would have returned an error code.
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy2D_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpy2D_v2(ref CUDAMemCpy2D pCopy);
            /// <summary>
            /// Perform a 2D memory copy according to the parameters specified in <c>pCopy</c>. See <see cref="CUDAMemCpy2D"/>.
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy2DUnaligned_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpy2DUnaligned_v2(ref CUDAMemCpy2D pCopy);

            // 3D memcpy
            /// <summary>
            /// Perform a 3D memory copy according to the parameters specified in <c>pCopy</c>. See <see cref="CUDAMemCpy3D"/>.<para/>
            /// The srcLOD and dstLOD members of the CUDAMemCpy3D structure must be set to 0.
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>            
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy3D_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemcpy3D_v2(ref CUDAMemCpy3D pCopy);
        }
        #endregion

        #region Asynchronous Memcpy_v2
        /// <summary>
        /// Any host memory involved must be DMA'able (e.g., allocated with cuMemAllocHost).
        /// memcpy's done with these functions execute in parallel with the CPU and, if
        /// the hardware is available, may execute in parallel with the GPU.
        /// Asynchronous memcpy must be accompanied by appropriate stream synchronization.
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class AsynchronousMemcpy_v2
        {
#if (NETCOREAPP)
            static AsynchronousMemcpy_v2()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            //New memcpy functions in CUDA 4.0 for unified addressing
            /// <summary>
            /// Copies data between two pointers. 
            /// <c>dst</c> and <c>src</c> are base pointers of the destination and source, respectively.  
            /// <c>ByteCount</c> specifies the number of bytes to copy.
            /// Note that this function infers the type of the transfer (host to host, host to 
            /// device, device to device, or device to host) from the pointer values.  This
            /// function is only allowed in contexts which support unified addressing.
            /// Note that this function is asynchronous and can optionally be associated to 
            /// a stream by passing a non-zero <c>hStream</c> argument
            /// </summary>
            /// <param name="dst">Destination unified virtual address space pointer</param>
            /// <param name="src">Source unified virtual address space pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>   
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyAsync(CUdeviceptr dst, CUdeviceptr src, SizeT ByteCount, CUstream hStream);

            /// <summary>
            /// Copies from device memory in one context to device memory in another
            /// context. <c>dstDevice</c> is the base device pointer of the destination memory 
            /// and <c>dstContext</c> is the destination context. <c>srcDevice</c> is the base 
            /// device pointer of the source memory and <c>srcContext</c> is the source pointer.  
            /// <c>ByteCount</c> specifies the number of bytes to copy.  Note that this function
            /// is asynchronous with respect to the host and all work in other streams in
            /// other devices.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstContext">Destination context</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="srcContext">Source context</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>   
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyPeerAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyPeerAsync(CUdeviceptr dstDevice, CUcontext dstContext, CUdeviceptr srcDevice, CUcontext srcContext, SizeT ByteCount, CUstream hStream);

            /// <summary>
            /// Perform a 3D memory copy according to the parameters specified in
            /// <c>pCopy</c>.  See the definition of the <see cref="BasicTypes.CUDAMemCpy3DPeer"/> structure
            /// for documentation of its parameters.
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>   
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy3DPeerAsync" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpy3DPeerAsync(ref CUDAMemCpy3DPeer pCopy, CUstream hStream);



            // 1D functions
            // system <-> device memory
            /// <summary>
            /// Copies from host memory to device memory. <c>dstDevice</c> and <c>srcHost</c> are the base addresses of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. <para/>
            /// <see cref="cuMemcpyHtoDAsync_v2(CUdeviceptr, IntPtr, SizeT, CUstream)"/> is asynchronous and can optionally be associated to a stream by passing a non-zero <c>hStream</c>
            /// argument. It only works on page-locked memory and returns an error if a pointer to pageable memory is passed as
            /// input.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>   
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoDAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyHtoDAsync_v2(CUdeviceptr dstDevice, [In] IntPtr srcHost, SizeT ByteCount, CUstream hStream);

            //Device -> Host
            /// <summary>
            /// Copies from device to host memory. <c>dstHost</c> and <c>srcDevice</c> specify the base pointers of the destination and
            /// source, respectively. <c>ByteCount</c> specifies the number of bytes to copy.<para/>
            /// <see cref="cuMemcpyDtoHAsync_v2(IntPtr, CUdeviceptr, SizeT, CUstream)"/> is asynchronous and can optionally be associated to a stream by passing a non-zero
            /// <c>hStream</c> argument. It only works on page-locked memory and returns an error if a pointer to pageable memory
            /// is passed as input.
            /// </summary>
            /// <param name="dstHost">Destination host pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>    
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoHAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyDtoHAsync_v2([Out] IntPtr dstHost, CUdeviceptr srcDevice, SizeT ByteCount, CUstream hStream);

            // device <-> device memory
            /// <summary>
            /// Copies from device memory to device memory. <c>dstDevice</c> and <c>srcDevice</c> are the base pointers of the destination
            /// and source, respectively. <c>ByteCount</c> specifies the number of bytes to copy. Note that this function is asynchronous
            /// and can optionally be associated to a stream by passing a non-zero <c>hStream</c> argument.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="srcDevice">Source device pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>    
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyDtoDAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyDtoDAsync_v2(CUdeviceptr dstDevice, CUdeviceptr srcDevice, SizeT ByteCount, CUstream hStream);

            // system <-> array memory
            /// <summary>
            /// Copies from host memory to a 1D CUDA array. <c>dstArray</c> and <c>dstOffset</c> specify the CUDA array handle and
            /// starting offset in bytes of the destination data. <c>srcHost</c> specifies the base address of the source. <c>ByteCount</c>
            /// specifies the number of bytes to copy.<para/>
            /// <see cref="cuMemcpyHtoAAsync_v2(CUarray, SizeT, IntPtr, SizeT, CUstream)"/> is asynchronous and can optionally be associated to a stream by passing a non-zero
            /// <c>hStream</c> argument. It only works on page-locked memory and returns an error if a pointer to pageable memory
            /// is passed as input.
            /// </summary>
            /// <param name="dstArray">Destination array</param>
            /// <param name="dstOffset">Offset in bytes of destination array</param>
            /// <param name="srcHost">Source host pointer</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>    
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyHtoAAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyHtoAAsync_v2(CUarray dstArray, SizeT dstOffset, [In] IntPtr srcHost, SizeT ByteCount, CUstream hStream);

            //Array -> Host
            /// <summary>
            /// Copies from one 1D CUDA array to host memory. <c>dstHost</c> specifies the base pointer of the destination. <c>srcArray</c>
            /// and <c>srcOffset</c> specify the CUDA array handle and starting offset in bytes of the source data. <c>ByteCount</c> specifies
            /// the number of bytes to copy.<para/>
            /// <see cref="cuMemcpyAtoHAsync_v2(IntPtr, CUarray, SizeT, SizeT, CUstream)"/> is asynchronous and can optionally be associated to a stream by passing a non-zero stream <c>hStream</c>
            /// argument. It only works on page-locked host memory and returns an error if a pointer to pageable memory is passed
            /// as input.
            /// </summary>
            /// <param name="dstHost">Destination pointer</param>
            /// <param name="srcArray">Source array</param>
            /// <param name="srcOffset">Offset in bytes of source array</param>
            /// <param name="ByteCount">Size of memory copy in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>    
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpyAtoHAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpyAtoHAsync_v2([Out] IntPtr dstHost, CUarray srcArray, SizeT srcOffset, SizeT ByteCount, CUstream hStream);

            // 2D memcpy
            /// <summary>
            /// Perform a 2D memory copy according to the parameters specified in <c>pCopy</c>. See <see cref="CUDAMemCpy2D"/>.
            /// <see cref="cuMemcpy2DAsync_v2"/> returns an error if any pitch is greater than the maximum allowed (<see cref="CUDeviceProperties.memPitch"/>).
			/// <see cref="MemoryManagement.cuMemAllocPitch_v2"/> passes back pitches that always work with <see cref="cuMemcpy2DAsync_v2"/>. On intra-device
            /// memory copies (device <![CDATA[<->]]> device, CUDA array <![CDATA[<->]]> device, CUDA array <![CDATA[<->]]> CUDA array), <see cref="cuMemcpy2DAsync_v2"/> may fail
			/// for pitches not computed by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>. <see cref="SynchronousMemcpy_v2.cuMemcpy2DUnaligned_v2"/> (not async!) does not have this restriction, but
            /// may run significantly slower in the cases where <see cref="cuMemcpy2DAsync_v2"/> would have returned an error code.
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy2DAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpy2DAsync_v2(ref CUDAMemCpy2D pCopy, CUstream hStream);

            // 3D memcpy
            /// <summary>
            /// Perform a 3D memory copy according to the parameters specified in <c>pCopy</c>. See <see cref="CUDAMemCpy3D"/>.
            /// <see cref="cuMemcpy3DAsync_v2"/> returns an error if any pitch is greater than the maximum allowed (<see cref="CUDeviceProperties.memPitch"/>).<para/>
            /// <see cref="cuMemcpy3DAsync_v2"/> is asynchronous and can optionally be associated to a stream by passing a non-zero <c>hStream</c>
            /// argument. It only works on page-locked host memory and returns an error if a pointer to pageable memory is passed
            /// as input. <para/>
            /// The srcLOD and dstLOD members of the CUDAMemCpy3D structure must be set to 0.
            /// </summary>
            /// <param name="pCopy">Parameters for the memory copy</param>
            /// <param name="hStream">Stream indetifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>   
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemcpy3DAsync_v2" + CUDA_PTSZ)]
            public static extern CUResult cuMemcpy3DAsync_v2(ref CUDAMemCpy3D pCopy, CUstream hStream);
        }
        #endregion

        #region Memset
        /// <summary>
        /// Combines all memset API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class Memset
        {
#if (NETCOREAPP)
            static Memset()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Sets the memory range of <c>N</c> 8-bit values to the specified value <c>b</c>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="b">Value to set</param>
            /// <param name="N">Number of elements</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD8_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemsetD8_v2(CUdeviceptr dstDevice, byte b, SizeT N);

            /// <summary>
            /// Sets the memory range of <c>N</c> 16-bit values to the specified value <c>us</c>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="us">Value to set</param>
            /// <param name="N">Number of elements</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD16_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemsetD16_v2(CUdeviceptr dstDevice, ushort us, SizeT N);

            /// <summary>
            /// Sets the memory range of <c>N</c> 32-bit values to the specified value <c>ui</c>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="ui">Value to set</param>
            /// <param name="N">Number of elements</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD32_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemsetD32_v2(CUdeviceptr dstDevice, uint ui, SizeT N);

            /// <summary>
            /// Sets the 2D memory range of <c>Width</c> 8-bit values to the specified value <c>b</c>. <c>Height</c> specifies the number of rows to
            /// set, and <c>dstPitch</c> specifies the number of bytes between each row. This function performs fastest when the pitch is
			/// one that has been passed back by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstPitch">Pitch of destination device pointer</param>
            /// <param name="b">Value to set</param>
            /// <param name="Width">Width of row</param>
            /// <param name="Height">Number of rows</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD2D8_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemsetD2D8_v2(CUdeviceptr dstDevice, SizeT dstPitch, byte b, SizeT Width, SizeT Height);

            /// <summary>
            /// Sets the 2D memory range of <c>Width</c> 16-bit values to the specified value <c>us</c>. <c>Height</c> specifies the number of rows to
            /// set, and <c>dstPitch</c> specifies the number of bytes between each row. This function performs fastest when the pitch is
			/// one that has been passed back by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstPitch">Pitch of destination device pointer</param>
            /// <param name="us">Value to set</param>
            /// <param name="Width">Width of row</param>
            /// <param name="Height">Number of rows</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD2D16_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemsetD2D16_v2(CUdeviceptr dstDevice, SizeT dstPitch, ushort us, SizeT Width, SizeT Height);

            /// <summary>
            /// Sets the 2D memory range of <c>Width</c> 32-bit values to the specified value <c>us</c>. <c>Height</c> specifies the number of rows to
            /// set, and <c>dstPitch</c> specifies the number of bytes between each row. This function performs fastest when the pitch is
			/// one that has been passed back by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstPitch">Pitch of destination device pointer</param>
            /// <param name="ui">Value to set</param>
            /// <param name="Width">Width of row</param>
            /// <param name="Height">Number of rows</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD2D32_v2" + CUDA_PTDS)]
            public static extern CUResult cuMemsetD2D32_v2(CUdeviceptr dstDevice, SizeT dstPitch, uint ui, SizeT Width, SizeT Height);
        }
        #endregion

        #region MemsetAsync
        /// <summary>
        /// Combines all async memset API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class MemsetAsync
        {
#if (NETCOREAPP)
            static MemsetAsync()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Sets the memory range of <c>N</c> 8-bit values to the specified value <c>b</c>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="b">Value to set</param>
            /// <param name="N">Number of elements</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD8Async" + CUDA_PTSZ)]
            public static extern CUResult cuMemsetD8Async(CUdeviceptr dstDevice, byte b, SizeT N, CUstream hStream);

            /// <summary>
            /// Sets the memory range of <c>N</c> 16-bit values to the specified value <c>us</c>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="us">Value to set</param>
            /// <param name="N">Number of elements</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD16Async" + CUDA_PTSZ)]
            public static extern CUResult cuMemsetD16Async(CUdeviceptr dstDevice, ushort us, SizeT N, CUstream hStream);

            /// <summary>
            /// Sets the memory range of <c>N</c> 32-bit values to the specified value <c>ui</c>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="ui">Value to set</param>
            /// <param name="N">Number of elements</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD32Async" + CUDA_PTSZ)]
            public static extern CUResult cuMemsetD32Async(CUdeviceptr dstDevice, uint ui, SizeT N, CUstream hStream);

            /// <summary>
            /// Sets the 2D memory range of <c>Width</c> 8-bit values to the specified value <c>b</c>. <c>Height</c> specifies the number of rows to
            /// set, and <c>dstPitch</c> specifies the number of bytes between each row. This function performs fastest when the pitch is
			/// one that has been passed back by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstPitch">Pitch of destination device pointer</param>
            /// <param name="b">Value to set</param>
            /// <param name="Width">Width of row</param>
            /// <param name="Height">Number of rows</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD2D8Async" + CUDA_PTSZ)]
            public static extern CUResult cuMemsetD2D8Async(CUdeviceptr dstDevice, SizeT dstPitch, byte b, SizeT Width, SizeT Height, CUstream hStream);

            /// <summary>
            /// Sets the 2D memory range of <c>Width</c> 16-bit values to the specified value <c>us</c>. <c>Height</c> specifies the number of rows to
            /// set, and <c>dstPitch</c> specifies the number of bytes between each row. This function performs fastest when the pitch is
			/// one that has been passed back by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstPitch">Pitch of destination device pointer</param>
            /// <param name="us">Value to set</param>
            /// <param name="Width">Width of row</param>
            /// <param name="Height">Number of rows</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD2D16Async" + CUDA_PTSZ)]
            public static extern CUResult cuMemsetD2D16Async(CUdeviceptr dstDevice, SizeT dstPitch, ushort us, SizeT Width, SizeT Height, CUstream hStream);

            /// <summary>
            /// Sets the 2D memory range of <c>Width</c> 32-bit values to the specified value <c>us</c>. <c>Height</c> specifies the number of rows to
            /// set, and <c>dstPitch</c> specifies the number of bytes between each row. This function performs fastest when the pitch is
			/// one that has been passed back by <see cref="MemoryManagement.cuMemAllocPitch_v2"/>.
            /// </summary>
            /// <param name="dstDevice">Destination device pointer</param>
            /// <param name="dstPitch">Pitch of destination device pointer</param>
            /// <param name="ui">Value to set</param>
            /// <param name="Width">Width of row</param>
            /// <param name="Height">Number of rows</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuMemsetD2D32Async" + CUDA_PTSZ)]
            public static extern CUResult cuMemsetD2D32Async(CUdeviceptr dstDevice, SizeT dstPitch, uint ui, SizeT Width, SizeT Height, CUstream hStream);
        }
        #endregion

        #region Function management
        /// <summary>
        /// Combines all function / kernel API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class FunctionManagement
        {
#if (NETCOREAPP)
            static FunctionManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Specifies the <c>x</c>, <c>y</c>, and <c>z</c> dimensions of the thread blocks that are created when the kernel given by <c>hfunc</c> is launched.
            /// </summary>
            /// <param name="hfunc">Kernel to specify dimensions of</param>
            /// <param name="x">X dimension</param>
            /// <param name="y">Y dimension</param>
            /// <param name="z">Z dimension</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuFuncSetBlockShape(CUfunction hfunc, int x, int y, int z);

            /// <summary>
            /// Sets through <c>bytes</c> the amount of dynamic shared memory that will be available to each thread block when the kernel
            /// given by <c>hfunc</c> is launched.
            /// </summary>
            /// <param name="hfunc">Kernel to specify dynamic shared-memory size for</param>
            /// <param name="bytes">Dynamic shared-memory size per thread in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuFuncSetSharedSize(CUfunction hfunc, uint bytes);

            /// <summary>
            /// Returns in <c>pi</c> the integer value of the attribute <c>attrib</c> on the kernel given by <c>hfunc</c>. See <see cref="CUFunctionAttribute"/>.
            /// </summary>
            /// <param name="pi">Returned attribute value</param>
            /// <param name="attrib">Attribute requested</param>
            /// <param name="hfunc">Function to query attribute of</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuFuncGetAttribute(ref int pi, CUFunctionAttribute attrib, CUfunction hfunc);

            /// <summary>
            /// Sets information about a function
            /// <para/>
            /// This call sets the value of a specified attribute \p attrib on the kernel given
            /// by \p hfunc to an integer value specified by \p val
            /// <para/>
            /// This function returns CUDA_SUCCESS if the new value of the attribute could be
            /// successfully set. If the set fails, this call will return an error.
            /// <para/>
            /// Not all attributes can have values set. Attempting to set a value on a read-only
            /// attribute will result in an error (CUDA_ERROR_INVALID_VALUE)
            /// <para/>
            /// Supported attributes for the cuFuncSetAttribute call are:
            /// <para/>
            /// ::CU_FUNC_ATTRIBUTE_MAX_DYNAMIC_SHARED_SIZE_BYTES: This maximum size in bytes of
            /// dynamically-allocated shared memory.The value should contain the requested
            /// maximum size of dynamically-allocated shared memory.The sum of this value and
            /// the function attribute::CU_FUNC_ATTRIBUTE_SHARED_SIZE_BYTES cannot exceed the
            /// device attribute ::CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK_OPTIN.
            /// The maximal size of requestable dynamic shared memory may differ by GPU
            /// architecture.
            /// <para/>
            /// ::CU_FUNC_ATTRIBUTE_PREFERRED_SHARED_MEMORY_CARVEOUT: On devices where the L1
            /// cache and shared memory use the same hardware resources, this sets the shared memory
            /// carveout preference, in percent of the total resources.This is only a hint, and the
            /// driver can choose a different ratio if required to execute the function.
            /// </summary>
            /// <param name="hfunc">Function to query attribute of</param>
            /// <param name="attrib">Attribute requested</param>
            /// <param name="value">The value to set</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuFuncSetAttribute(CUfunction hfunc, CUFunctionAttribute attrib, int value);



            /// <summary>
            /// On devices where the L1 cache and shared memory use the same hardware resources, this sets through <c>config</c>
            /// the preferred cache configuration for the device function <c>hfunc</c>. This is only a preference. The driver will use the
            /// requested configuration if possible, but it is free to choose a different configuration if required to execute <c>hfunc</c>. <para/>
            /// This setting does nothing on devices where the size of the L1 cache and shared memory are fixed.<para/>
            /// Switching between configuration modes may insert a device-side synchronization point for streamed kernel launches.<para/>
            /// The supported cache modes are defined in <see cref="CUFuncCache"/>
            /// </summary>
            /// <param name="hfunc">Kernel to configure cache for</param>
            /// <param name="config">Requested cache configuration</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuFuncSetCacheConfig(CUfunction hfunc, CUFuncCache config);

            /// <summary>
            /// Sets the shared memory configuration for a device function.<para/>
            /// On devices with configurable shared memory banks, this function will 
            /// force all subsequent launches of the specified device function to have
            /// the given shared memory bank size configuration. On any given launch of the
            /// function, the shared memory configuration of the device will be temporarily
            /// changed if needed to suit the function's preferred configuration. Changes in
            /// shared memory configuration between subsequent launches of functions, 
            /// may introduce a device side synchronization point.<para/>
            /// Any per-function setting of shared memory bank size set via
            /// <see cref="cuFuncSetSharedMemConfig"/>  will override the context wide setting set with
            /// <see cref="DriverAPINativeMethods.ContextManagement.cuCtxSetSharedMemConfig"/>.<para/>
            /// Changing the shared memory bank size will not increase shared memory usage
            /// or affect occupancy of kernels, but may have major effects on performance. 
            /// Larger bank sizes will allow for greater potential bandwidth to shared memory,
            /// but will change what kinds of accesses to shared memory will result in bank 
            /// conflicts.<para/>
            /// This function will do nothing on devices with fixed shared memory bank size.<para/>
            /// The supported bank configurations are<para/> 
            /// - <see cref="CUsharedconfig.DefaultBankSize"/>: set bank width to the default initial
            ///   setting (currently, four bytes).
            /// - <see cref="CUsharedconfig.FourByteBankSize"/>: set shared memory bank width to
            ///   be natively four bytes.
            /// - <see cref="CUsharedconfig.EightByteBankSize"/>: set shared memory bank width to
            ///   be natively eight bytes.
            /// </summary>
            /// <param name="hfunc">kernel to be given a shared memory config</param>
            /// <param name="config">requested shared memory configuration</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuFuncSetSharedMemConfig(CUfunction hfunc, CUsharedconfig config);

            /// <summary>
            /// Returns a module handle<para/>
            /// Returns in \p *hmod the handle of the module that function \p hfunc
            /// is located in. The lifetime of the module corresponds to the lifetime of
            /// the context it was loaded in or until the module is explicitly unloaded.<para/>
            /// The CUDA runtime manages its own modules loaded into the primary context.
            /// If the handle returned by this API refers to a module loaded by the CUDA runtime,
            /// calling ::cuModuleUnload() on that module will result in undefined behavior.
            /// </summary>
            /// <param name="hmod"></param>
            /// <param name="hfunc"></param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuFuncGetModule(ref CUmodule hmod, CUfunction hfunc);
        }
        #endregion

        #region Array management
        /// <summary>
        /// Combines all array management API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class ArrayManagement
        {
#if (NETCOREAPP)
            static ArrayManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Creates a CUDA array according to the <see cref="CUDAArrayDescriptor"/> structure <c>pAllocateArray</c> and returns a
            /// handle to the new CUDA array in <c>pHandle</c>.
            /// </summary>
            /// <param name="pHandle">Returned array</param>
            /// <param name="pAllocateArray">Array descriptor</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArrayCreate_v2(ref CUarray pHandle, ref CUDAArrayDescriptor pAllocateArray);

            /// <summary>
            /// Returns in <c>pArrayDescriptor</c> a descriptor containing information on the format and dimensions of the CUDA
            /// array <c>hArray</c>. It is useful for subroutines that have been passed a CUDA array, but need to know the CUDA array
            /// parameters for validation or other purposes.
            /// </summary>
            /// <param name="pArrayDescriptor">Returned array descriptor</param>
            /// <param name="hArray">Array to get descriptor of</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArrayGetDescriptor_v2(ref CUDAArrayDescriptor pArrayDescriptor, CUarray hArray);

            /// <summary>
            /// Returns the layout properties of a sparse CUDA array
            /// Returns the layout properties of a sparse CUDA array in \p sparseProperties
            /// If the CUDA array is not allocated with flag ::CUDA_ARRAY3D_SPARSE ::CUDA_ERROR_INVALID_VALUE will be returned.
            /// If the returned value in ::CUDA_ARRAY_SPARSE_PROPERTIES::flags contains ::CU_ARRAY_SPARSE_PROPERTIES_SINGLE_MIPTAIL,
            /// then::CUDA_ARRAY_SPARSE_PROPERTIES::miptailSize represents the total size of the array.Otherwise, it will be zero.
            /// Also, the returned value in ::CUDA_ARRAY_SPARSE_PROPERTIES::miptailFirstLevel is always zero.
            /// Note that the \p array must have been allocated using ::cuArrayCreate or::cuArray3DCreate.For CUDA arrays obtained
            /// using ::cuMipmappedArrayGetLevel, ::CUDA_ERROR_INVALID_VALUE will be returned.Instead, ::cuMipmappedArrayGetSparseProperties
            /// must be used to obtain the sparse properties of the entire CUDA mipmapped array to which \p array belongs to.
            /// </summary>
            /// <param name="sparseProperties">Pointer to ::CUDA_ARRAY_SPARSE_PROPERTIES</param>
            /// <param name="array"> CUDA array to get the sparse properties of</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArrayGetSparseProperties(ref CudaArraySparseProperties sparseProperties, CUarray array);

            /// <summary>
            /// Returns the layout properties of a sparse CUDA mipmapped array
            /// Returns the sparse array layout properties in \p sparseProperties
            /// If the CUDA mipmapped array is not allocated with flag ::CUDA_ARRAY3D_SPARSE
            /// ::CUDA_ERROR_INVALID_VALUE will be returned.
            /// For non-layered CUDA mipmapped arrays, ::CUDA_ARRAY_SPARSE_PROPERTIES::miptailSize returns the
            /// size of the mip tail region.The mip tail region includes all mip levels whose width, height or depth
            /// is less than that of the tile.
            /// For layered CUDA mipmapped arrays, if ::CUDA_ARRAY_SPARSE_PROPERTIES::flags contains ::CU_ARRAY_SPARSE_PROPERTIES_SINGLE_MIPTAIL,
            /// then ::CUDA_ARRAY_SPARSE_PROPERTIES::miptailSize specifies the size of the mip tail of all layers combined. 
            /// Otherwise, ::CUDA_ARRAY_SPARSE_PROPERTIES::miptailSize specifies mip tail size per layer.
            /// The returned value of::CUDA_ARRAY_SPARSE_PROPERTIES::miptailFirstLevel is valid only if ::CUDA_ARRAY_SPARSE_PROPERTIES::miptailSize is non-zero.
            /// </summary>
            /// <param name="sparseProperties">Pointer to ::CUDA_ARRAY_SPARSE_PROPERTIES</param>
            /// <param name="mipmap">CUDA mipmapped array to get the sparse properties of</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMipmappedArrayGetSparseProperties(ref CudaArraySparseProperties sparseProperties, CUmipmappedArray mipmap);

            /// <summary>
            /// Gets a CUDA array plane from a CUDA array<para/>
            /// Returns in \p pPlaneArray a CUDA array that represents a single format plane
            /// of the CUDA array \p hArray.<para/>
            /// If \p planeIdx is greater than the maximum number of planes in this array or if the array does
            /// not have a multi-planar format e.g: ::CU_AD_FORMAT_NV12, then::CUDA_ERROR_INVALID_VALUE is returned.<para/>
            /// Note that if the \p hArray has format ::CU_AD_FORMAT_NV12, then passing in 0 for \p planeIdx returns
            /// a CUDA array of the same size as \p hArray but with one channel and::CU_AD_FORMAT_UNSIGNED_INT8 as its format.
            /// If 1 is passed for \p planeIdx, then the returned CUDA array has half the height and width
            /// of \p hArray with two channels and ::CU_AD_FORMAT_UNSIGNED_INT8 as its format.
            /// </summary>
            /// <param name="pPlaneArray">Returned CUDA array referenced by the planeIdx</param>
            /// <param name="hArray">Multiplanar CUDA array</param>
            /// <param name="planeIdx">Plane index</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArrayGetPlane(ref CUarray pPlaneArray, CUarray hArray, uint planeIdx);

            /// <summary>
            /// Destroys the CUDA array hArray.
            /// </summary>
            /// <param name="hArray">Array to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorArrayIsMapped"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArrayDestroy(CUarray hArray);

            /// <summary>
            /// Creates a CUDA array according to the <see cref="CUDAArray3DDescriptor"/> structure <c>pAllocateArray</c> and returns
            /// a handle to the new CUDA array in <c>pHandle</c>.
            /// </summary>
            /// <param name="pHandle">Returned array</param>
            /// <param name="pAllocateArray">3D array descriptor</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArray3DCreate_v2(ref CUarray pHandle, ref CUDAArray3DDescriptor pAllocateArray);

            /// <summary>
            /// Returns in <c>pArrayDescriptor</c> a descriptor containing information on the format and dimensions of the CUDA
            /// array <c>hArray</c>. It is useful for subroutines that have been passed a CUDA array, but need to know the CUDA array
            /// parameters for validation or other purposes.<para/>
            /// This function may be called on 1D and 2D arrays, in which case the Height and/or Depth members of the descriptor
            /// struct will be set to 0.
            /// </summary>
            /// <param name="pArrayDescriptor">Returned 3D array descriptor</param>
            /// <param name="hArray">3D array to get descriptor of</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuArray3DGetDescriptor_v2(ref CUDAArray3DDescriptor pArrayDescriptor, CUarray hArray);

            /// <summary>
            /// Creates a CUDA mipmapped array according to the ::CUDA_ARRAY3D_DESCRIPTOR structure
            /// <c>pMipmappedArrayDesc</c> and returns a handle to the new CUDA mipmapped array in <c>pHandle</c>.
            /// <c>numMipmapLevels</c> specifies the number of mipmap levels to be allocated. This value is
            /// clamped to the range [1, 1 + floor(log2(max(width, height, depth)))]. 
            /// </summary>
            /// <param name="pHandle">Returned mipmapped array</param>
            /// <param name="pMipmappedArrayDesc">mipmapped array descriptor</param>
            /// <param name="numMipmapLevels">Number of mipmap levels</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>, <see cref="CUResult.ErrorUnknown"/>. </returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMipmappedArrayCreate(ref CUmipmappedArray pHandle, ref CUDAArray3DDescriptor pMipmappedArrayDesc, uint numMipmapLevels);

            /// <summary>
            /// Returns in <c>pLevelArray</c> a CUDA array that represents a single mipmap level
            /// of the CUDA mipmapped array <c>hMipmappedArray</c>.
            /// </summary>
            /// <param name="pLevelArray">Returned mipmap level CUDA array</param>
            /// <param name="hMipmappedArray">CUDA mipmapped array</param>
            /// <param name="level">Mipmap level</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMipmappedArrayGetLevel(ref CUarray pLevelArray, CUmipmappedArray hMipmappedArray, uint level);

            /// <summary>
            /// Destroys the CUDA mipmapped array <c>hMipmappedArray</c>.
            /// </summary>
            /// <param name="hMipmappedArray">Mipmapped array to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuMipmappedArrayDestroy(CUmipmappedArray hMipmappedArray);


        }
        #endregion

        #region Texture reference management
        /// <summary>
        /// Groups all texture reference management API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class TextureReferenceManagement
        {
#if (NETCOREAPP)
            static TextureReferenceManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Creates a texture reference and returns its handle in <c>pTexRef</c>. Once created, the application must call <see cref="cuTexRefSetArray"/>
			/// or <see cref="cuTexRefSetAddress_v2"/> to associate the reference with allocated memory. Other texture reference functions
            /// are used to specify the format and interpretation (addressing, filtering, etc.) to be used when the memory is read
            /// through this texture reference.
            /// </summary>
            /// <param name="pTexRef">Returned texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuTexRefCreate(ref CUtexref pTexRef);

            /// <summary>
            /// Destroys the texture reference specified by <c>hTexRef</c>.
            /// </summary>
            /// <param name="hTexRef">Texture reference to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuTexRefDestroy(CUtexref hTexRef);

            /// <summary>
            /// Binds the CUDA array <c>hArray</c> to the texture reference <c>hTexRef</c>. Any previous address or CUDA array state
            /// associated with the texture reference is superseded by this function. Flags must be set to 
            /// <see cref="CUTexRefSetArrayFlags.OverrideFormat"/>. Any CUDA array previously bound to hTexRef is unbound.
            /// </summary>
            /// <param name="hTexRef">Texture reference to bind</param>
            /// <param name="hArray">Array to bind</param>
            /// <param name="Flags">Options (must be <see cref="CUTexRefSetArrayFlags.OverrideFormat"/>)</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetArray(CUtexref hTexRef, CUarray hArray, CUTexRefSetArrayFlags Flags);

            /// <summary>
            /// Binds the CUDA mipmapped array <c>hMipmappedArray</c> to the texture reference <c>hTexRef</c>.
            /// Any previous address or CUDA array state associated with the texture reference
            /// is superseded by this function. <c>Flags</c> must be set to <see cref="CUTexRefSetArrayFlags.OverrideFormat"/>. 
            /// Any CUDA array previously bound to <c>hTexRef</c> is unbound.
            /// </summary>
            /// <param name="hTexRef">Texture reference to bind</param>
            /// <param name="hMipmappedArray">Mipmapped array to bind</param>
            /// <param name="Flags">Options (must be <see cref="CUTexRefSetArrayFlags.OverrideFormat"/>)</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetMipmappedArray(CUtexref hTexRef, CUmipmappedArray hMipmappedArray, CUTexRefSetArrayFlags Flags);

            /// <summary>
            /// Binds a linear address range to the texture reference <c>hTexRef</c>. Any previous address or CUDA array state associated
            /// with the texture reference is superseded by this function. Any memory previously bound to <c>hTexRef</c> is unbound.<para/>
			/// Since the hardware enforces an alignment requirement on texture base addresses, <see cref="cuTexRefSetAddress_v2"/> passes back
            /// a byte offset in <c>ByteOffset</c> that must be applied to texture fetches in order to read from the desired memory. This
            /// offset must be divided by the texel size and passed to kernels that read from the texture so they can be applied to the
            /// <c>tex1Dfetch()</c> function.<para/>
			/// If the device memory pointer was returned from <see cref="MemoryManagement.cuMemAlloc_v2"/>, the offset is guaranteed to be 0 and <c>null</c> may be
            /// passed as the <c>ByteOffset</c> parameter.
            /// </summary>
            /// <param name="ByteOffset">Returned byte offset</param>
            /// <param name="hTexRef">Texture reference to bind</param>
            /// <param name="dptr">Device pointer to bind</param>
            /// <param name="bytes">Size of memory to bind in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetAddress_v2(ref SizeT ByteOffset, CUtexref hTexRef, CUdeviceptr dptr, SizeT bytes);

            /// <summary>
            /// Binds a linear address range to the texture reference <c>hTexRef</c>. Any previous address or CUDA array state associated
            /// with the texture reference is superseded by this function. Any memory previously bound to <c>hTexRef</c> is unbound. <para/>
            /// Using a <c>tex2D()</c> function inside a kernel requires a call to either <see cref="cuTexRefSetArray"/> to bind the corresponding texture
			/// reference to an array, or <see cref="cuTexRefSetAddress2D_v2"/> to bind the texture reference to linear memory.<para/>
			/// Function calls to <see cref="cuTexRefSetFormat"/> cannot follow calls to <see cref="cuTexRefSetAddress2D_v2"/> for the same texture reference.<para/>
            /// It is required that <c>dptr</c> be aligned to the appropriate hardware-specific texture alignment. You can query this value
            /// using the device attribute <see cref="CUDeviceAttribute.TextureAlignment"/>. If an unaligned <c>dptr</c> is supplied,
            /// <see cref="CUResult.ErrorInvalidValue"/> is returned.
            /// </summary>
            /// <param name="hTexRef">Texture reference to bind</param>
            /// <param name="desc">Descriptor of CUDA array</param>
            /// <param name="dptr">Device pointer to bind</param>
            /// <param name="Pitch">Line pitch in bytes></param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetAddress2D_v2(CUtexref hTexRef, ref CUDAArrayDescriptor desc, CUdeviceptr dptr, SizeT Pitch);

            /// <summary>
            /// Specifies the format of the data to be read by the texture reference <c>hTexRef</c>. <c>fmt</c> and <c>NumPackedComponents</c>
            /// are exactly analogous to the Format and NumChannels members of the <see cref="CUDAArrayDescriptor"/> structure:
            /// They specify the format of each component and the number of components per array element.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="fmt">Format to set</param>
            /// <param name="NumPackedComponents">Number of components per array element</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetFormat(CUtexref hTexRef, CUArrayFormat fmt, int NumPackedComponents);

            /// <summary>
            /// Specifies the addressing mode <c>am</c> for the given dimension <c>dim</c> of the texture reference <c>hTexRef</c>. If <c>dim</c> is zero,
            /// the addressing mode is applied to the first parameter of the functions used to fetch from the texture; if <c>dim</c> is 1, the
            /// second, and so on. See <see cref="CUAddressMode"/>.<para/>
            /// Note that this call has no effect if <c>hTexRef</c> is bound to linear memory.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="dim">Dimension</param>
            /// <param name="am">Addressing mode to set</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetAddressMode(CUtexref hTexRef, int dim, CUAddressMode am);

            /// <summary>
            /// Specifies the filtering mode <c>fm</c> to be used when reading memory through the texture reference <c>hTexRef</c>. See <see cref="CUFilterMode"/>.<para/>
            /// Note that this call has no effect if hTexRef is bound to linear memory.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="fm">Filtering mode to set</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetFilterMode(CUtexref hTexRef, CUFilterMode fm);

            /// <summary>
            /// Specifies optional flags via <c>Flags</c> to specify the behavior of data returned through the texture reference <c>hTexRef</c>. See <see cref="CUTexRefSetFlags"/>.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="Flags">Optional flags to set</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetFlags(CUtexref hTexRef, CUTexRefSetFlags Flags);

            /// <summary>
            /// Returns in <c>pdptr</c> the base address bound to the texture reference <c>hTexRef</c>, or returns <see cref="CUResult.ErrorInvalidValue"/>
            /// if the texture reference is not bound to any device memory range.
            /// </summary>
            /// <param name="pdptr">Returned device address</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetAddress(ref CUdeviceptr pdptr, CUtexref hTexRef);

            /// <summary>
            /// Returns in <c>phArray</c> the CUDA array bound to the texture reference <c>hTexRef</c>, or returns <see cref="CUResult.ErrorInvalidValue"/>
            /// if the texture reference is not bound to any CUDA array.
            /// </summary>
            /// <param name="phArray">Returned array</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetArray(ref CUarray phArray, CUtexref hTexRef);

            /// <summary>
            /// Returns in <c>phMipmappedArray</c> the CUDA mipmapped array bound to the texture 
            /// reference <c>hTexRef</c>, or returns <see cref="CUResult.ErrorInvalidValue"/> if the texture reference
            /// is not bound to any CUDA mipmapped array.
            /// </summary>
            /// <param name="phMipmappedArray">Returned mipmapped array</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetMipmappedArray(ref CUmipmappedArray phMipmappedArray, CUtexref hTexRef);

            /// <summary>
            /// Returns in <c>pam</c> the addressing mode corresponding to the dimension <c>dim</c> of the texture reference <c>hTexRef</c>. Currently,
            /// the only valid value for <c>dim</c> are 0 and 1.
            /// </summary>
            /// <param name="pam">Returned addressing mode</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="dim">Dimension</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetAddressMode(ref CUAddressMode pam, CUtexref hTexRef, int dim);

            /// <summary>
            /// Returns in <c>pfm</c> the filtering mode of the texture reference <c>hTexRef</c>.
            /// </summary>
            /// <param name="pfm">Returned filtering mode</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetFilterMode(ref CUFilterMode pfm, CUtexref hTexRef);

            /// <summary>
            /// Returns in <c>pFormat</c> and <c>pNumChannels</c> the format and number of components of the CUDA array bound to
            /// the texture reference <c>hTexRef</c>. If <c>pFormat</c> or <c>pNumChannels</c> is <c>null</c>, it will be ignored.
            /// </summary>
            /// <param name="pFormat">Returned format</param>
            /// <param name="pNumChannels">Returned number of components</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetFormat(ref CUArrayFormat pFormat, ref int pNumChannels, CUtexref hTexRef);

            /// <summary>
            /// Returns in <c>pFlags</c> the flags of the texture reference <c>hTexRef</c>.
            /// </summary>
            /// <param name="pFlags">Returned flags</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetFlags(ref CUTexRefSetFlags pFlags, CUtexref hTexRef);

            /// <summary>
            /// Returns the mipmap filtering mode in <c>pfm</c> that's used when reading memory through
            /// the texture reference <c>hTexRef</c>.
            /// </summary>
            /// <param name="pfm">Returned mipmap filtering mode</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetMipmapFilterMode(ref CUFilterMode pfm, CUtexref hTexRef);

            /// <summary>
            /// Returns the mipmap level bias in <c>pBias</c> that's added to the specified mipmap
            /// level when reading memory through the texture reference <c>hTexRef</c>.
            /// </summary>
            /// <param name="pbias">Returned mipmap level bias</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetMipmapLevelBias(ref float pbias, CUtexref hTexRef);

            /// <summary>
            /// Returns the min/max mipmap level clamps in <c>pminMipmapLevelClamp</c> and <c>pmaxMipmapLevelClamp</c>
            /// that's used when reading memory through the texture reference <c>hTexRef</c>. 
            /// </summary>
            /// <param name="pminMipmapLevelClamp">Returned mipmap min level clamp</param>
            /// <param name="pmaxMipmapLevelClamp">Returned mipmap max level clamp</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetMipmapLevelClamp(ref float pminMipmapLevelClamp, ref float pmaxMipmapLevelClamp, CUtexref hTexRef);

            /// <summary>
            /// Returns the maximum aniostropy in <c>pmaxAniso</c> that's used when reading memory through
            /// the texture reference. 
            /// </summary>
            /// <param name="pmaxAniso">Returned maximum anisotropy</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetMaxAnisotropy(ref int pmaxAniso, CUtexref hTexRef);

            /// <summary>
            /// Specifies the mipmap filtering mode <c>fm</c> to be used when reading memory through
            /// the texture reference <c>hTexRef</c>.<para/>
            /// Note that this call has no effect if <c>hTexRef</c> is not bound to a mipmapped array.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="fm">Filtering mode to set</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetMipmapFilterMode(CUtexref hTexRef, CUFilterMode fm);

            /// <summary>
            /// Specifies the mipmap level bias <c>bias</c> to be added to the specified mipmap level when 
            /// reading memory through the texture reference <c>hTexRef</c>.<para/>
            /// Note that this call has no effect if <c>hTexRef</c> is not bound to a mipmapped array.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="bias">Mipmap level bias</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetMipmapLevelBias(CUtexref hTexRef, float bias);

            /// <summary>
            /// Specifies the min/max mipmap level clamps, <c>minMipmapLevelClamp</c> and <c>maxMipmapLevelClamp</c>
            /// respectively, to be used when reading memory through the texture reference 
            /// <c>hTexRef</c>.<para/>
            /// Note that this call has no effect if <c>hTexRef</c> is not bound to a mipmapped array.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="minMipmapLevelClamp">Mipmap min level clamp</param>
            /// <param name="maxMipmapLevelClamp">Mipmap max level clamp</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetMipmapLevelClamp(CUtexref hTexRef, float minMipmapLevelClamp, float maxMipmapLevelClamp);

            /// <summary>
            /// Specifies the maximum aniostropy <c>maxAniso</c> to be used when reading memory through
            /// the texture reference <c>hTexRef</c>. <para/>
            /// Note that this call has no effect if <c>hTexRef</c> is not bound to a mipmapped array.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="maxAniso">Maximum anisotropy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetMaxAnisotropy(CUtexref hTexRef, uint maxAniso);


            /// <summary>
            /// Sets the border color for a texture reference<para/>
            /// Specifies the value of the RGBA color via the \p pBorderColor to the texture reference
            /// \p hTexRef. The color value supports only float type and holds color components in
            /// the following sequence:<para/>
            /// pBorderColor[0] holds 'R' component<para/>
            /// pBorderColor[1] holds 'G' component<para/>
            /// pBorderColor[2] holds 'B' component<para/>
            /// pBorderColor[3] holds 'A' component<para/>
            /// <para/>
            /// Note that the color values can be set only when the Address mode is set to
            /// CU_TR_ADDRESS_MODE_BORDER using ::cuTexRefSetAddressMode.<para/>
            /// Applications using integer border color values have to "reinterpret_cast" their values to float.
            /// </summary>
            /// <param name="hTexRef">Texture reference</param>
            /// <param name="pBorderColor">RGBA color</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefSetBorderColor(CUtexref hTexRef, [In, Out] float[] pBorderColor);

            /// <summary>
            /// Gets the border color used by a texture reference<para/>
            /// Returns in \p pBorderColor, values of the RGBA color used by
            /// the texture reference \p hTexRef.<para/>
            /// The color value is of type float and holds color components in
            /// the following sequence:<para/>
            /// pBorderColor[0] holds 'R' component
            /// pBorderColor[1] holds 'G' component
            /// pBorderColor[2] holds 'B' component
            /// pBorderColor[3] holds 'A' component
            /// </summary>
            /// <param name="pBorderColor">Returned Type and Value of RGBA color</param>
            /// <param name="hTexRef">Texture reference</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuTexRefGetBorderColor([In, Out] float[] pBorderColor, CUtexref hTexRef);
        }
        #endregion

        #region Surface reference management
        /// <summary>
        /// Combines all surface management API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class SurfaceReferenceManagement
        {
#if (NETCOREAPP)
            static SurfaceReferenceManagement()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Sets the CUDA array <c>hArray</c> to be read and written by the surface reference <c>hSurfRef</c>. Any previous CUDA array
            /// state associated with the surface reference is superseded by this function. Flags must be set to <see cref="CUSurfRefSetFlags.None"/>. The 
            /// <see cref="CUDAArray3DFlags.SurfaceLDST"/> flag must have been set for the CUDA array. Any CUDA array previously bound to
            /// <c>hSurfRef</c> is unbound.
            /// </summary>
            /// <param name="hSurfRef">Surface reference handle</param>
            /// <param name="hArray">CUDA array handle</param>
            /// <param name="Flags">set to <see cref="CUSurfRefSetFlags.None"/></param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuSurfRefSetArray(CUsurfref hSurfRef, CUarray hArray, CUSurfRefSetFlags Flags);

            /// <summary>
            /// Returns in <c>phArray</c> the CUDA array bound to the surface reference <c>hSurfRef</c>, or returns
            /// <see cref="CUResult.ErrorInvalidValue"/> if the surface reference is not bound to any CUDA array.
            /// </summary>
            /// <param name="phArray">Surface reference handle</param>
            /// <param name="hSurfRef">Surface reference handle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_11)]
            public static extern CUResult cuSurfRefGetArray(ref CUarray phArray, CUsurfref hSurfRef);
        }
        #endregion

        #region Launch functions
        /// <summary>
        /// Groups all kernel launch API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class Launch
        {
#if (NETCOREAPP)
            static Launch()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Invokes the kernel <c>f</c> on a 1 x 1 x 1 grid of blocks. The block contains the number of threads specified by a previous
            /// call to <see cref="FunctionManagement.cuFuncSetBlockShape"/>.
            /// </summary>
            /// <param name="f">Kernel to launch</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>
            /// <see cref="CUResult.ErrorLaunchFailed"/>, <see cref="CUResult.ErrorLaunchOutOfResources"/>
            /// <see cref="CUResult.ErrorLaunchTimeout"/>, <see cref="CUResult.ErrorLaunchIncompatibleTexturing"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuLaunch([In] CUfunction f);

            /// <summary>
            /// Invokes the kernel <c>f</c> on a <c>grid_width</c> x <c>grid_height</c> grid of blocks. Each block contains the number of threads
            /// specified by a previous call to <see cref="FunctionManagement.cuFuncSetBlockShape"/>.
            /// </summary>
            /// <param name="f">Kernel to launch</param>
            /// <param name="grid_width">Width of grid in blocks</param>
            /// <param name="grid_height">Height of grid in blocks</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>
            /// <see cref="CUResult.ErrorLaunchFailed"/>, <see cref="CUResult.ErrorLaunchOutOfResources"/>
            /// <see cref="CUResult.ErrorLaunchTimeout"/>, <see cref="CUResult.ErrorLaunchIncompatibleTexturing"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuLaunchGrid([In] CUfunction f, [In] int grid_width, [In] int grid_height);

            /// <summary>
            /// Invokes the kernel <c>f</c> on a <c>grid_width</c> x <c>grid_height</c> grid of blocks. Each block contains the number of threads
            /// specified by a previous call to <see cref="FunctionManagement.cuFuncSetBlockShape"/>.<para/>
            /// <see cref="cuLaunchGridAsync"/> can optionally be associated to a stream by passing a non-zero <c>hStream</c> argument.
            /// </summary>
            /// <param name="f">Kernel to launch</param>
            /// <param name="grid_width">Width of grid in blocks</param>
            /// <param name="grid_height">Height of grid in blocks</param>
            /// <param name="hStream">Stream identifier</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>
            /// <see cref="CUResult.ErrorLaunchFailed"/>, <see cref="CUResult.ErrorLaunchOutOfResources"/>
            /// <see cref="CUResult.ErrorLaunchTimeout"/>, <see cref="CUResult.ErrorLaunchIncompatibleTexturing"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete(CUDA_OBSOLET_9_2)]
            public static extern CUResult cuLaunchGridAsync([In] CUfunction f, [In] int grid_width, [In] int grid_height, [In] CUstream hStream);

            /// <summary>
            /// Invokes the kernel <c>f</c> on a <c>gridDimX</c> x <c>gridDimY</c> x <c>gridDimZ</c>
            /// grid of blocks. Each block contains <c>blockDimX</c> x <c>blockDimY</c> x
            /// blockDimZ threads.
            /// <para/>
            /// <c>sharedMemBytes</c> sets the amount of dynamic shared memory that will be
            /// available to each thread block.
            /// <para/>
            /// <see cref="cuLaunchKernel"/> can optionally be associated to a stream by passing a
            /// non-zero <c>hStream</c> argument.
            /// <para/>
            /// Kernel parameters to <c>f</c> can be specified in one of two ways:
            /// <para/>
            /// 1) Kernel parameters can be specified via <c>kernelParams</c>. If <c>f</c>
            /// has N parameters, then <c>kernelParams</c> needs to be an array of N
            /// pointers. Each of <c>kernelParams[0]</c> through <c>kernelParams[N-1]</c>
            /// must point to a region of memory from which the actual kernel
            /// parameter will be copied.  The number of kernel parameters and their
            /// offsets and sizes do not need to be specified as that information is
            /// retrieved directly from the kernel's image.
            /// <para/>
            /// 2) Kernel parameters can also be packaged by the application into
            /// a single buffer that is passed in via the <c>extra</c> parameter.
            /// This places the burden on the application of knowing each kernel
            /// parameter's size and alignment/padding within the buffer.  
            /// 
            /// <para/>
            /// The <c>extra</c> parameter exists to allow <see cref="cuLaunchKernel"/> to take
            /// additional less commonly used arguments. <c>extra</c> specifies a list of
            /// names of extra settings and their corresponding values.  Each extra
            /// setting name is immediately followed by the corresponding value.  The
            /// list must be terminated with either NULL or ::CU_LAUNCH_PARAM_END.
            /// <para/>
            /// - ::CU_LAUNCH_PARAM_END, which indicates the end of the <c>extra</c>
            ///   array;
            /// - ::CU_LAUNCH_PARAM_BUFFER_POINTER, which specifies that the next
            ///   value in <c>extra</c> will be a pointer to a buffer containing all
            ///   the kernel parameters for launching kernel <c>f</c>;
            /// - ::CU_LAUNCH_PARAM_BUFFER_SIZE, which specifies that the next
            ///   value in <c>extra</c> will be a pointer to a size_t containing the
            ///   size of the buffer specified with ::CU_LAUNCH_PARAM_BUFFER_POINTER;
            /// <para/>
            /// The error ::CUDA_ERROR_INVALID_VALUE will be returned if kernel
            /// parameters are specified with both <c>kernelParams</c> and <c>extra</c>
            /// (i.e. both <c>kernelParams</c> and <c>extra</c> are non-NULL).
            /// <para/>
            /// Calling <see cref="cuLaunchKernel"/> sets persistent function state that is
            /// the same as function state set through the following deprecated APIs:
            ///
            ///  ::cuFuncSetBlockShape()
            ///  ::cuFuncSetSharedSize()
            ///  ::cuParamSetSize()
            ///  ::cuParamSeti()
            ///  ::cuParamSetf()
            ///  ::cuParamSetv()
            /// <para/>
            /// When the kernel <c>f</c> is launched via <see cref="cuLaunchKernel"/>, the previous
            /// block shape, shared size and parameter info associated with <c>f</c>
            /// is overwritten.
            /// <para/>
            /// Note that to use <see cref="cuLaunchKernel"/>, the kernel <c>f</c> must either have
            /// been compiled with toolchain version 3.2 or later so that it will
            /// contain kernel parameter information, or have no kernel parameters.
            /// If either of these conditions is not met, then <see cref="cuLaunchKernel"/> will
            /// return <see cref="CUResult.ErrorInvalidImage"/>.
            /// </summary>
            /// <param name="f">Kernel to launch</param>
            /// <param name="gridDimX">Width of grid in blocks</param>
            /// <param name="gridDimY">Height of grid in blocks</param>
            /// <param name="gridDimZ">Depth of grid in blocks</param>
            /// <param name="blockDimX">X dimension of each thread block</param>
            /// <param name="blockDimY">Y dimension of each thread block</param>
            /// <param name="blockDimZ">Z dimension of each thread block</param>
            /// <param name="sharedMemBytes">Dynamic shared-memory size per thread block in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <param name="kernelParams">Array of pointers to kernel parameters</param>
            /// <param name="extra">Extra options</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>, 
            /// <see cref="CUResult.ErrorInvalidImage"/>, <see cref="CUResult.ErrorInvalidValue"/>
            /// <see cref="CUResult.ErrorLaunchFailed"/>, <see cref="CUResult.ErrorLaunchOutOfResources"/>
            /// <see cref="CUResult.ErrorLaunchTimeout"/>, <see cref="CUResult.ErrorLaunchIncompatibleTexturing"/>, <see cref="CUResult.ErrorSharedObjectInitFailed"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuLaunchKernel" + CUDA_PTSZ)]
            public static extern CUResult cuLaunchKernel(CUfunction f,
                                uint gridDimX,
                                uint gridDimY,
                                uint gridDimZ,
                                uint blockDimX,
                                uint blockDimY,
                                uint blockDimZ,
                                uint sharedMemBytes,
                                CUstream hStream,
                                IntPtr[] kernelParams,
                                IntPtr[] extra);

            /// <summary>
            /// Launches a CUDA function where thread blocks can cooperate and synchronize as they execute
            /// <para/>
            /// Invokes the kernel \p f on a \p gridDimX x \p gridDimY x \p gridDimZ
            /// grid of blocks.Each block contains \p blockDimX x \p blockDimY x
            /// \p blockDimZ threads.
            /// <para/>
            /// \p sharedMemBytes sets the amount of dynamic shared memory that will be
            /// available to each thread block.
            /// <para/>
            /// The device on which this kernel is invoked must have a non-zero value for
            /// the device attribute::CU_DEVICE_ATTRIBUTE_COOPERATIVE_LAUNCH.
            /// <para/>
            /// The total number of blocks launched cannot exceed the maximum number of blocks per
            /// multiprocessor as returned by ::cuOccupancyMaxActiveBlocksPerMultiprocessor (or
            /// ::cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags) times the number of multiprocessors
            /// as specified by the device attribute ::CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT.
            /// <para/>
            /// The kernel cannot make use of CUDA dynamic parallelism.
            /// <para/>
            /// Kernel parameters must be specified via \p kernelParams.  If \p f
            /// has N parameters, then \p kernelParams needs to be an array of N
            /// pointers.  Each of \p kernelParams [0]
            /// through \p kernelParams [N-1]
            /// must point to a region of memory from which the actual kernel
            /// parameter will be copied.  The number of kernel parameters and their
            /// offsets and sizes do not need to be specified as that information is
            /// retrieved directly from the kernel's image.
            /// <para/>
            /// Calling ::cuLaunchCooperativeKernel() sets persistent function state that is
            /// the same as function state set through ::cuLaunchKernel API
            /// <para/>
            /// When the kernel \p f is launched via ::cuLaunchCooperativeKernel(), the previous
            /// block shape, shared size and parameter info associated with \p f
            /// is overwritten.
            /// <para/>
            /// Note that to use ::cuLaunchCooperativeKernel(), the kernel \p f must either have
            /// been compiled with toolchain version 3.2 or later so that it will
            /// contain kernel parameter information, or have no kernel parameters.
            /// If either of these conditions is not met, then ::cuLaunchCooperativeKernel() will
            /// return ::CUDA_ERROR_INVALID_IMAGE.
            /// </summary>
            /// <param name="f">Kernel to launch</param>
            /// <param name="gridDimX">Width of grid in blocks</param>
            /// <param name="gridDimY">Height of grid in blocks</param>
            /// <param name="gridDimZ">Depth of grid in blocks</param>
            /// <param name="blockDimX">X dimension of each thread block</param>
            /// <param name="blockDimY">Y dimension of each thread block</param>
            /// <param name="blockDimZ">Z dimension of each thread block</param>
            /// <param name="sharedMemBytes">Dynamic shared-memory size per thread block in bytes</param>
            /// <param name="hStream">Stream identifier</param>
            /// <param name="kernelParams">Array of pointers to kernel parameters</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuLaunchCooperativeKernel" + CUDA_PTSZ)]
            public static extern CUResult cuLaunchCooperativeKernel(CUfunction f,
                                            uint gridDimX,
                                            uint gridDimY,
                                            uint gridDimZ,
                                            uint blockDimX,
                                            uint blockDimY,
                                            uint blockDimZ,
                                            uint sharedMemBytes,
                                            CUstream hStream,
                                            IntPtr[] kernelParams);

            /// <summary>
            /// Launches CUDA functions on multiple devices where thread blocks can cooperate and synchronize as they execute
            /// <para/>
            /// Invokes kernels as specified in the \p launchParamsList array where each element
            /// of the array specifies all the parameters required to perform a single kernel launch.
            /// These kernels can cooperate and synchronize as they execute. The size of the array is
            /// specified by \p numDevices.
            /// <para/>
            /// No two kernels can be launched on the same device. All the devices targeted by this
            /// multi-device launch must be identical. All devices must have a non-zero value for the
            /// device attribute ::CU_DEVICE_ATTRIBUTE_COOPERATIVE_MULTI_DEVICE_LAUNCH.
            /// <para/>
            /// All kernels launched must be identical with respect to the compiled code. Note that
            /// any __device__, __constant__ or __managed__ variables present in the module that owns
            /// the kernel launched on each device, are independently instantiated on every device.
            /// It is the application's responsiblity to ensure these variables are initialized and
            /// used appropriately.
            /// <para/>
            /// The size of the grids as specified in blocks, the size of the blocks themselves
            /// and the amount of shared memory used by each thread block must also match across
            /// all launched kernels.
            /// <para/>
            /// The streams used to launch these kernels must have been created via either ::cuStreamCreate
            /// or ::cuStreamCreateWithPriority. The NULL stream or ::CU_STREAM_LEGACY or ::CU_STREAM_PER_THREAD
            /// cannot be used.
            /// <para/>
            /// The total number of blocks launched per kernel cannot exceed the maximum number of blocks
            /// per multiprocessor as returned by ::cuOccupancyMaxActiveBlocksPerMultiprocessor (or
            /// ::cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags) times the number of multiprocessors
            /// as specified by the device attribute ::CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT. Since the
            /// total number of blocks launched per device has to match across all devices, the maximum
            /// number of blocks that can be launched per device will be limited by the device with the
            /// least number of multiprocessors.
            /// <para/>
            /// The kernels cannot make use of CUDA dynamic parallelism.
            /// <para/>
            /// By default, the kernel won't begin execution on any GPU until all prior work in all the specified
            /// streams has completed. This behavior can be overridden by specifying the flag
            /// ::CUDA_COOPERATIVE_LAUNCH_MULTI_DEVICE_NO_PRE_LAUNCH_SYNC. When this flag is specified, each kernel
            /// will only wait for prior work in the stream corresponding to that GPU to complete before it begins
            /// execution.
            /// <para/>
            /// Similarly, by default, any subsequent work pushed in any of the specified streams will not begin
            /// execution until the kernels on all GPUs have completed. This behavior can be overridden by specifying
            /// the flag ::CUDA_COOPERATIVE_LAUNCH_MULTI_DEVICE_NO_POST_LAUNCH_SYNC. When this flag is specified,
            /// any subsequent work pushed in any of the specified streams will only wait for the kernel launched
            /// on the GPU corresponding to that stream to complete before it begins execution.
            /// <para/>
            /// Calling ::cuLaunchCooperativeKernelMultiDevice() sets persistent function state that is
            /// the same as function state set through ::cuLaunchKernel API when called individually for each
            /// element in \p launchParamsList.
            /// <para/>
            /// When kernels are launched via ::cuLaunchCooperativeKernelMultiDevice(), the previous
            /// block shape, shared size and parameter info associated with each ::CUDA_LAUNCH_PARAMS::function
            /// in \p launchParamsList is overwritten.
            /// <para/>
            /// Note that to use ::cuLaunchCooperativeKernelMultiDevice(), the kernels must either have
            /// been compiled with toolchain version 3.2 or later so that it will
            /// contain kernel parameter information, or have no kernel parameters.
            /// If either of these conditions is not met, then ::cuLaunchCooperativeKernelMultiDevice() will
            /// return ::CUDA_ERROR_INVALID_IMAGE.
            /// </summary>
            /// <param name="launchParamsList">List of launch parameters, one per device</param>
            /// <param name="numDevices">Size of the \p launchParamsList array</param>
            /// <param name="flags">Flags to control launch behavior</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            [Obsolete("This function is deprecated as of CUDA 11.3")]
            public static extern CUResult cuLaunchCooperativeKernelMultiDevice(CudaLaunchParams[] launchParamsList, uint numDevices, CudaCooperativeLaunchMultiDeviceFlags flags);

            /// <summary>
            /// Enqueues a host function call in a stream<para/>
            /// Enqueues a host function to run in a stream.  The function will be called after currently enqueued work and will block work added after it.
            /// <para/>
            /// The host function must not make any CUDA API calls.  Attempting to use a
            /// CUDA API may result in ::CUDA_ERROR_NOT_PERMITTED, but this is not required.
            /// The host function must not perform any synchronization that may depend on
            /// outstanding CUDA work not mandated to run earlier.  Host functions without a
            /// mandated order (such as in independent streams) execute in undefined order
            /// and may be serialized.<para/>
            /// Note that, in contrast to ::cuStreamAddCallback, the function will not be
            /// called in the event of an error in the CUDA context.
            /// </summary>
            /// <param name="hStream">Stream to enqueue function call in</param>
            /// <param name="fn">The function to call once preceding stream operations are complete</param>
            /// <param name="userData">User-specified data to be passed to the function</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuLaunchHostFunc" + CUDA_PTSZ)]
            public static extern CUResult cuLaunchHostFunc(CUstream hStream, CUhostFn fn, IntPtr userData);

        }


        #endregion

        #region Events
        /// <summary>
        /// Groups all event API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class Events
        {
#if (NETCOREAPP)
            static Events()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Creates an event <c>phEvent</c> with the flags specified via <c>Flags</c>. See <see cref="CUEventFlags"/>
            /// </summary>
            /// <param name="phEvent">Returns newly created event</param>
            /// <param name="Flags">Event creation flags</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuEventCreate(ref CUevent phEvent, CUEventFlags Flags);

            /// <summary>
            /// Records an event. If <c>stream</c> is non-zero, the event is recorded after all preceding operations in the stream have been
            /// completed; otherwise, it is recorded after all preceding operations in the CUDA context have been completed. Since
            /// operation is asynchronous, <see cref="cuEventQuery"/> and/or <see cref="cuEventSynchronize"/> must be used to determine when the event
            /// has actually been recorded. <para/>
            /// If <see cref="cuEventRecord"/> has previously been called and the event has not been recorded yet, this function returns
            /// <see cref="CUResult.ErrorInvalidValue"/>.
            /// </summary>
            /// <param name="hEvent">Event to record</param>
            /// <param name="hStream">Stream to record event for</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuEventRecord" + CUDA_PTSZ)]
            public static extern CUResult cuEventRecord(CUevent hEvent, CUstream hStream);

            /// <summary>
            /// Records an event
            /// Captures in \p hEvent the contents of \p hStream at the time of this call.
            /// \p hEvent and \p hStream must be from the same context.
            /// Calls such as ::cuEventQuery() or ::cuStreamWaitEvent() will then
            /// examine or wait for completion of the work that was captured.Uses of
            /// \p hStream after this call do not modify \p hEvent. See note on default
            /// stream behavior for what is captured in the default case.
            /// ::cuEventRecordWithFlags() can be called multiple times on the same event and
            /// will overwrite the previously captured state.Other APIs such as
            /// ::cuStreamWaitEvent() use the most recently captured state at the time
            /// of the API call, and are not affected by later calls to
            /// ::cuEventRecordWithFlags(). Before the first call to::cuEventRecordWithFlags(), an
            /// event represents an empty set of work, so for example::cuEventQuery()
            /// would return ::CUDA_SUCCESS.
            /// </summary>
            /// <param name="hEvent">Event to record</param>
            /// <param name="hStream">Stream to record event for</param>
            /// <param name="flags">See ::CUevent_capture_flags</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuEventRecordWithFlags" + CUDA_PTSZ)]
            public static extern CUResult cuEventRecordWithFlags(CUevent hEvent, CUstream hStream, CUEventRecordFlags flags);

            /// <summary>
            /// Returns <see cref="CUResult.Success"/> if the event has actually been recorded, or <see cref="CUResult.ErrorNotReady"/> if not. If
            /// <see cref="cuEventRecord"/> has not been called on this event, the function returns <see cref="CUResult.ErrorInvalidValue"/>.
            /// </summary>
            /// <param name="hEvent">Event to query</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorNotReady"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuEventQuery(CUevent hEvent);

            /// <summary>
            /// Waits until the event has actually been recorded. If <see cref="cuEventRecord"/> has been called on this event, the function returns
            /// <see cref="CUResult.ErrorInvalidValue"/>. Waiting for an event that was created with the <see cref="CUEventFlags.BlockingSync"/>
            /// flag will cause the calling CPU thread to block until the event has actually been recorded. <para/>
            /// If <see cref="cuEventRecord"/> has previously been called and the event has not been recorded yet, this function returns <see cref="CUResult.ErrorInvalidValue"/>.
            /// </summary>
            /// <param name="hEvent">Event to wait for</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuEventSynchronize(CUevent hEvent);

            /// <summary>
            /// Destroys the event specified by <c>event</c>.<para/>
            /// In the case that <c>hEvent</c> has been recorded but has not yet been completed
            /// when <see cref="cuEventDestroy_v2"/> is called, the function will return immediately and 
            /// the resources associated with <c>hEvent</c> will be released automatically once
            /// the device has completed <c>hEvent</c>.
            /// </summary>
            /// <param name="hEvent">Event to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuEventDestroy_v2(CUevent hEvent);

            /// <summary>
            /// Computes the elapsed time between two events (in milliseconds with a resolution of around 0.5 microseconds). If
            /// either event has not been recorded yet, this function returns <see cref="CUResult.ErrorNotReady"/>. If either event has been
            /// recorded with a non-zero stream, the result is undefined.
            /// </summary>
            /// <param name="pMilliseconds">Returned elapsed time in milliseconds</param>
            /// <param name="hStart">Starting event</param>
            /// <param name="hEnd">Ending event</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorNotReady"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuEventElapsedTime(ref float pMilliseconds, CUevent hStart, CUevent hEnd);

            /// <summary>
            /// Wait on a memory location<para/>
            /// Enqueues a synchronization of the stream on the given memory location. Work
            /// ordered after the operation will block until the given condition on the
            /// memory is satisfied. By default, the condition is to wait for (int32_t)(*addr - value) >= 0, a cyclic greater-or-equal.
            /// <para/>
            /// Other condition types can be specified via \p flags.
            /// <para/>
            /// If the memory was registered via ::cuMemHostRegister(), the device pointer
            /// should be obtained with::cuMemHostGetDevicePointer(). This function cannot
            /// be used with managed memory(::cuMemAllocManaged).
            /// <para/>
            /// Support for this can be queried with ::cuDeviceGetAttribute() and
            /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS. The only requirement for basic
            /// support is that on Windows, a device must be in TCC mode.
            /// </summary>
            /// <param name="stream">The stream to synchronize on the memory location.</param>
            /// <param name="addr">The memory location to wait on.</param>
            /// <param name="value">The value to compare with the memory location.</param>
            /// <param name="flags"> See::CUstreamWaitValue_flags.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamWaitValue32" + CUDA_PTSZ)]
            public static extern CUResult cuStreamWaitValue32(CUstream stream, CUdeviceptr addr, uint value, CUstreamWaitValue_flags flags);

            /// <summary>
            /// Wait on a memory location
            /// <para/>
            /// Enqueues a synchronization of the stream on the given memory location.Work
            /// ordered after the operation will block until the given condition on the
            /// memory is satisfied.By default, the condition is to wait for
            /// (int64_t)(*addr - value) >= 0, a cyclic greater-or-equal.
            /// <para/>
            /// Other condition types can be specified via \p flags.
            /// <para/>
            /// If the memory was registered via ::cuMemHostRegister(), the device pointer
            /// should be obtained with::cuMemHostGetDevicePointer().
            /// <para/>
            /// Support for this can be queried with ::cuDeviceGetAttribute() and
            /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_64_BIT_STREAM_MEM_OPS.The requirements are
            /// compute capability 7.0 or greater, and on Windows, that the device be in
            /// TCC mode.
            /// </summary>
            /// <param name="stream">The stream to synchronize on the memory location.</param>
            /// <param name="addr">The memory location to wait on.</param>
            /// <param name="value">The value to compare with the memory location.</param>
            /// <param name="flags">See::CUstreamWaitValue_flags.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamWaitValue64" + CUDA_PTSZ)]
            public static extern CUResult cuStreamWaitValue64(CUstream stream, CUdeviceptr addr, ulong value, CUstreamWaitValue_flags flags);


            /// <summary>
            /// Write a value to memory
            /// <para/>
            /// Write a value to memory.Unless the ::CU_STREAM_WRITE_VALUE_NO_MEMORY_BARRIER
            /// flag is passed, the write is preceded by a system-wide memory fence,
            /// equivalent to a __threadfence_system() but scoped to the stream
            /// rather than a CUDA thread.
            /// <para/>
            /// If the memory was registered via ::cuMemHostRegister(), the device pointer
            /// should be obtained with::cuMemHostGetDevicePointer(). This function cannot
            /// be used with managed memory(::cuMemAllocManaged).
            /// <para/>
            /// Support for this can be queried with ::cuDeviceGetAttribute() and
            /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_STREAM_MEM_OPS. The only requirement for basic
            /// support is that on Windows, a device must be in TCC mode.
            /// </summary>
            /// <param name="stream">The stream to do the write in.</param>
            /// <param name="addr">The device address to write to.</param>
            /// <param name="value">The value to write.</param>
            /// <param name="flags">See::CUstreamWriteValue_flags.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamWriteValue32" + CUDA_PTSZ)]
            public static extern CUResult cuStreamWriteValue32(CUstream stream, CUdeviceptr addr, uint value, CUstreamWriteValue_flags flags);


            /// <summary>
            /// Write a value to memory
            /// <para/>
            /// Write a value to memory.Unless the ::CU_STREAM_WRITE_VALUE_NO_MEMORY_BARRIER
            /// flag is passed, the write is preceded by a system-wide memory fence,
            /// equivalent to a __threadfence_system() but scoped to the stream
            /// rather than a CUDA thread.
            /// <para/>
            /// If the memory was registered via ::cuMemHostRegister(), the device pointer
            /// should be obtained with::cuMemHostGetDevicePointer(). This function cannot
            /// be used with managed memory(::cuMemAllocManaged).
            /// <para/>
            /// Support for this can be queried with ::cuDeviceGetAttribute() and
            /// ::CU_DEVICE_ATTRIBUTE_CAN_USE_64_BIT_STREAM_MEM_OPS.The requirements are
            /// compute capability 7.0 or greater, and on Windows, that the device be in
            /// TCC mode.
            /// </summary>
            /// <param name="stream">The stream to do the write in.</param>
            /// <param name="addr">The device address to write to.</param>
            /// <param name="value">The value to write.</param>
            /// <param name="flags">See::CUstreamWriteValue_flags.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamWriteValue64" + CUDA_PTSZ)]
            public static extern CUResult cuStreamWriteValue64(CUstream stream, CUdeviceptr addr, ulong value, CUstreamWriteValue_flags flags);


            /// <summary>
            /// Batch operations to synchronize the stream via memory operations
            /// <para/>
            /// This is a batch version of ::cuStreamWaitValue32() and::cuStreamWriteValue32().
            /// <para/>
            /// Batching operations may avoid some performance overhead in both the API call
            /// and the device execution versus adding them to the stream in separate API
            /// calls.The operations are enqueued in the order they appear in the array.
            /// <para/>
            /// See::CUstreamBatchMemOpType for the full set of supported operations, and
            /// ::cuStreamWaitValue32() and::cuStreamWriteValue32() for details of specific
            /// operations.
            /// <para/>
            /// On Windows, the device must be using TCC, or this call is not supported. See ::cuDeviceGetAttribute().
            /// </summary>
            /// <param name="stream">The stream to enqueue the operations in.</param>
            /// <param name="count">The number of operations in the array. Must be less than 256.</param>
            /// <param name="paramArray">The types and parameters of the individual operations.</param>
            /// <param name="flags"> Reserved for future expansion; must be 0.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamBatchMemOp" + CUDA_PTSZ)]
            public static extern CUResult cuStreamBatchMemOp(CUstream stream, uint count, CUstreamBatchMemOpParams[] paramArray, uint flags);
        }
        #endregion

        #region Streams
        /// <summary>
        /// Groups all stream API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class Streams
        {
#if (NETCOREAPP)
            static Streams()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Creates a stream and returns a handle in <c>phStream</c>. The <c>Flags</c> argument
			/// determines behaviors of the stream. Valid values for <c>Flags</c> are:
			/// - <see cref="CUStreamFlags.Default"/>: Default stream creation flag.
			/// - <see cref="CUStreamFlags.NonBlocking"/>: Specifies that work running in the created 
			/// stream may run concurrently with work in stream 0 (the NULL stream), and that
			/// the created stream should perform no implicit synchronization with stream 0.
            /// </summary>
            /// <param name="phStream">Returned newly created stream</param>
            /// <param name="Flags">Parameters for stream creation</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorOutOfMemory"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuStreamCreate(ref CUstream phStream, CUStreamFlags Flags);

            /// <summary>
            /// Returns <see cref="CUResult.Success"/> if all operations in the stream specified by <c>hStream</c> have completed, or
            /// <see cref="CUResult.ErrorNotReady"/> if not.
            /// </summary>
            /// <param name="hStream">Stream to query status of</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorNotReady"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamQuery" + CUDA_PTSZ)]
            public static extern CUResult cuStreamQuery(CUstream hStream);

            /// <summary>
            /// Waits until the device has completed all operations in the stream specified by <c>hStream</c>. If the context was created
            /// with the <see cref="CUCtxFlags.BlockingSync"/> flag, the CPU thread will block until the stream is finished with all of its
            /// tasks.
            /// </summary>
            /// <param name="hStream">Stream to wait for</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamSynchronize" + CUDA_PTSZ)]
            public static extern CUResult cuStreamSynchronize(CUstream hStream);


            /// <summary>
            /// Destroys the stream specified by hStream.<para/>
            /// In the case that the device is still doing work in the stream <c>hStream</c>
            /// when <see cref="cuStreamDestroy_v2"/> is called, the function will return immediately 
            /// and the resources associated with <c>hStream</c> will be released automatically 
            /// once the device has completed all work in <c>hStream</c>.
            /// </summary>
            /// <param name="hStream">Stream to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuStreamDestroy_v2(CUstream hStream);


            /// <summary>
            /// Copies attributes from source stream to destination stream<para/>
            /// Copies attributes from source stream \p src to destination stream \p dst.<para/>
            /// Both streams must have the same context.
            /// </summary>
            /// <param name="dst">Destination stream</param>
            /// <param name="src">Source stream</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamCopyAttributes" + CUDA_PTSZ)]
            public static extern CUResult cuStreamCopyAttributes(CUstream dst, CUstream src);


            /// <summary>
            /// Queries stream attribute.<para/>
            /// Queries attribute \p attr from \p hStream and stores it in corresponding member of \p value_out.
            /// </summary>
            /// <param name="hStream"></param>
            /// <param name="attr"></param>
            /// <param name="value_out"></param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetAttribute" + CUDA_PTSZ)]
            public static extern CUResult cuStreamGetAttribute(CUstream hStream, CUstreamAttrID attr, ref CUstreamAttrValue value_out);


            /// <summary>
            /// Sets stream attribute.<para/>
            /// Sets attribute \p attr on \p hStream from corresponding attribute of
            /// value.The updated attribute will be applied to subsequent work
            /// submitted to the stream. It will not affect previously submitted work.
            /// </summary>
            /// <param name="hStream"></param>
            /// <param name="attr"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamSetAttribute" + CUDA_PTSZ)]
            public static extern CUResult cuStreamSetAttribute(CUstream hStream, CUstreamAttrID attr, ref CUstreamAttrValue value);


            /// <summary>
            /// Make a compute stream wait on an event<para/>
            /// Makes all future work submitted to <c>hStream</c>  wait until <c>hEvent</c>
            /// reports completion before beginning execution. This synchronization
            /// will be performed efficiently on the device.
            /// <para/>
            /// The stream <c>hStream</c> will wait only for the completion of the most recent
            /// host call to <see cref="Events.cuEventRecord"/> on <c>hEvent</c>. Once this call has returned,
            /// any functions (including <see cref="Events.cuEventRecord"/> and <see cref="Events.cuEventDestroy_v2"/> may be
            /// called on <c>hEvent</c> again, and the subsequent calls will not have any
            /// effect on <c>hStream</c>.
            /// <para/>
            /// If <c>hStream</c> is 0 (the NULL stream) any future work submitted in any stream
            /// will wait for <c>hEvent</c> to complete before beginning execution. This
            /// effectively creates a barrier for all future work submitted to the context.
            /// <para/>
            /// If <see cref="Events.cuEventRecord"/> has not been called on <c>hEvent</c>, this call acts as if
            /// the record has already completed, and so is a functional no-op.
            /// <para/><c>Flags</c> argument must be 0.
            /// </summary>
            /// <param name="hStream">Stream to destroy</param>
            /// <param name="hEvent">Event</param>
            /// <param name="Flags">Flags argument must be set 0.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamWaitEvent" + CUDA_PTSZ)]
            public static extern CUResult cuStreamWaitEvent(CUstream hStream, CUevent hEvent, uint Flags);

            /// <summary>
            /// Adds a callback to be called on the host after all currently enqueued
            /// items in the stream have completed.  For each 
            /// cuStreamAddCallback call, the callback will be executed exactly once.
            /// The callback will block later work in the stream until it is finished.
            /// <para/>
            /// The callback may be passed <see cref="CUResult.Success"/> or an error code.  In the event
            /// of a device error, all subsequently executed callbacks will receive an
            /// appropriate <see cref="CUResult"/>.
            /// <para/>
            /// Callbacks must not make any CUDA API calls.  Attempting to use a CUDA API
            /// will result in <see cref="CUResult.ErrorNotPermitted"/>.  Callbacks must not perform any
            /// synchronization that may depend on outstanding device work or other callbacks
            /// that are not mandated to run earlier.  Callbacks without a mandated order
            /// (in independent streams) execute in undefined order and may be serialized.
            /// <para/>
            /// This API requires compute capability 1.1 or greater.  See
            /// cuDeviceGetAttribute or ::cuDeviceGetProperties to query compute
            /// capability.  Attempting to use this API with earlier compute versions will
            /// return <see cref="CUResult.ErrorNotSupported"/>.
            /// </summary>
            /// <param name="hStream">Stream to add callback to</param>
            /// <param name="callback">The function to call once preceding stream operations are complete</param>
            /// <param name="userData">User specified data to be passed to the callback function</param>
            /// <param name="flags">Reserved for future use; must be 0.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamAddCallback" + CUDA_PTSZ)]
            public static extern CUResult cuStreamAddCallback(CUstream hStream, CUstreamCallback callback, IntPtr userData, CUStreamAddCallbackFlags flags);

            /// <summary>
            /// Create a stream with the given priority<para/>
            /// Creates a stream with the specified priority and returns a handle in <c>phStream</c>. <para/>
            /// This API alters the scheduler priority of work in the stream. Work in a higher priority stream 
            /// may preempt work already executing in a low priority stream.<para/>
            /// <c>priority</c> follows a convention where lower numbers represent higher priorities.<para/>
            /// '0' represents default priority. The range of meaningful numerical priorities can
            /// be queried using <see cref="ContextManagement.cuCtxGetStreamPriorityRange"/>. If the specified priority is
            /// outside the numerical range returned by <see cref="ContextManagement.cuCtxGetStreamPriorityRange"/>,
            /// it will automatically be clamped to the lowest or the highest number in the range.
            /// </summary>
            /// <param name="phStream">Returned newly created stream</param>
            /// <param name="flags">Flags for stream creation. See ::cuStreamCreate for a list of valid flags</param>
            /// <param name="priority">Stream priority. Lower numbers represent higher priorities. <para/>
            /// See <see cref="ContextManagement.cuCtxGetStreamPriorityRange"/> for more information about meaningful stream priorities that can be passed.</param>
            /// <remarks>Stream priorities are supported only on Quadro and Tesla GPUs with compute capability 3.5 or higher.
            /// <para/>In the current implementation, only compute kernels launched in priority streams are affected by the stream's priority. <para/>
            /// Stream priorities have no effect on host-to-device and device-to-host memory operations.</remarks>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuStreamCreateWithPriority(ref CUstream phStream, CUStreamFlags flags, int priority);


            /// <summary>
            /// Query the priority of a given stream<para/>
            /// Query the priority of a stream created using <see cref="cuStreamCreate"/> or <see cref="cuStreamCreateWithPriority"/>
            /// and return the priority in <c>priority</c>. Note that if the stream was created with a
            /// priority outside the numerical range returned by <see cref="ContextManagement.cuCtxGetStreamPriorityRange"/>,
            /// this function returns the clamped priority.
            /// See <see cref="cuStreamCreateWithPriority"/> for details about priority clamping.
            /// </summary>
            /// <param name="hStream">Handle to the stream to be queried</param>
            /// <param name="priority">Pointer to a signed integer in which the stream's priority is returned</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetPriority" + CUDA_PTSZ)]
            public static extern CUResult cuStreamGetPriority(CUstream hStream, ref int priority);

            /// <summary>
            /// Query the flags of a given stream<para/>
            /// Query the flags of a stream created using <see cref="cuStreamCreate"/> or <see cref="cuStreamCreateWithPriority"/>
            /// and return the flags in <c>flags</c>.
            /// </summary>
            /// <param name="hStream">Handle to the stream to be queried</param>
            /// <param name="flags">Pointer to an unsigned integer in which the stream's flags are returned. <para/>
            /// The value returned in <c>flags</c> is a logical 'OR' of all flags that
            /// were used while creating this stream. See <see cref="cuStreamCreate"/> for the list
            /// of valid flags</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetFlags" + CUDA_PTSZ)]
            public static extern CUResult cuStreamGetFlags(CUstream hStream, ref CUStreamFlags flags);

            /// <summary>
            /// Query the context associated with a stream<para/>
            /// Returns the CUDA context that the stream is associated with. .
            /// </summary>
            /// <param name="hStream">Handle to the stream to be queried</param>
            /// <param name="pctx">Returned context associated with the stream</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetCtx" + CUDA_PTSZ)]
            public static extern CUResult cuStreamGetCtx(CUstream hStream, ref CUcontext pctx);

            /// <summary>
            /// Attach memory to a stream asynchronously
            /// <para/>
            /// Enqueues an operation in <c>hStream</c> to specify stream association of
            /// <c>length</c> bytes of memory starting from <c>dptr</c>. This function is a
            /// stream-ordered operation, meaning that it is dependent on, and will
            /// only take effect when, previous work in stream has completed. Any
            /// previous association is automatically replaced.
            /// <para/>
            /// <c>dptr</c> must point to an address within managed memory space declared
            /// using the __managed__ keyword or allocated with cuMemAllocManaged.
            /// <para/>
            /// <c>length</c> must be zero, to indicate that the entire allocation's
            /// stream association is being changed. Currently, it's not possible
            /// to change stream association for a portion of an allocation.
            /// <para/>
            /// The stream association is specified using <c>flags</c> which must be
            /// one of <see cref="CUmemAttach_flags"/>.
            /// If the <see cref="CUmemAttach_flags.Global"/> flag is specified, the memory can be accessed
            /// by any stream on any device.
            /// If the <see cref="CUmemAttach_flags.Host"/> flag is specified, the program makes a guarantee
            /// that it won't access the memory on the device from any stream.
            /// If the <see cref="CUmemAttach_flags.Single"/> flag is specified, the program makes a guarantee
            /// that it will only access the memory on the device from <c>hStream</c>. It is illegal
            /// to attach singly to the NULL stream, because the NULL stream is a virtual global
            /// stream and not a specific stream. An error will be returned in this case.
            /// <para/>
            /// When memory is associated with a single stream, the Unified Memory system will
            /// allow CPU access to this memory region so long as all operations in <c>hStream</c>
            /// have completed, regardless of whether other streams are active. In effect,
            /// this constrains exclusive ownership of the managed memory region by
            /// an active GPU to per-stream activity instead of whole-GPU activity.
            /// <para/>
            /// Accessing memory on the device from streams that are not associated with
            /// it will produce undefined results. No error checking is performed by the
            /// Unified Memory system to ensure that kernels launched into other streams
            /// do not access this region. 
            /// <para/>
            /// It is a program's responsibility to order calls to <see cref="cuStreamAttachMemAsync"/>
            /// via events, synchronization or other means to ensure legal access to memory
            /// at all times. Data visibility and coherency will be changed appropriately
            /// for all kernels which follow a stream-association change.
            /// <para/>
            /// If <c>hStream</c> is destroyed while data is associated with it, the association is
            /// removed and the association reverts to the default visibility of the allocation
            /// as specified at cuMemAllocManaged. For __managed__ variables, the default
            /// association is always <see cref="CUmemAttach_flags.Global"/>. Note that destroying a stream is an
            /// asynchronous operation, and as a result, the change to default association won't
            /// happen until all work in the stream has completed.
            /// <para/>
            /// </summary>
            /// <param name="hStream">Stream in which to enqueue the attach operation</param>
            /// <param name="dptr">Pointer to memory (must be a pointer to managed memory)</param>
            /// <param name="length">Length of memory (must be zero)</param>
            /// <param name="flags">Must be one of <see cref="CUmemAttach_flags"/></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamAttachMemAsync" + CUDA_PTSZ)]
            public static extern CUResult cuStreamAttachMemAsync(CUstream hStream, CUdeviceptr dptr, SizeT length, CUmemAttach_flags flags);


            /// <summary>
            /// Begins graph capture on a stream<para/>
            /// Begin graph capture on \p hStream. When a stream is in capture mode, all operations
            /// pushed into the stream will not be executed, but will instead be captured into
            /// a graph, which will be returned via::cuStreamEndCapture.Capture may not be initiated
            /// if \p stream is CU_STREAM_LEGACY.Capture must be ended on the same stream in which
            /// it was initiated, and it may only be initiated if the stream is not already in capture
            /// mode.The capture mode may be queried via ::cuStreamIsCapturing.
            /// </summary>
            /// <param name="hStream">Stream in which to initiate capture</param>
            /// <param name="mode">Controls the interaction of this capture sequence with other API calls that are potentially unsafe. For more details see ::cuThreadExchangeStreamCaptureMode.</param>
            /// <remarks>Kernels captured using this API must not use texture and surface references. 
            /// Reading or writing through any texture or surface reference is undefined
            /// behavior.This restriction does not apply to texture and surface objects.</remarks>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamBeginCapture_v2" + CUDA_PTSZ)]
            public static extern CUResult cuStreamBeginCapture(CUstream hStream, CUstreamCaptureMode mode);


            /// <summary>
            /// Ends capture on a stream, returning the captured graph<para/>
            /// End capture on \p hStream, returning the captured graph via \p phGraph.<para/>
            /// Capture must have been initiated on \p hStream via a call to::cuStreamBeginCapture.<para/>
            /// If capture was invalidated, due to a violation of the rules of stream capture, then
            /// a NULL graph will be returned.
            /// </summary>
            /// <param name="hStream">Stream to query</param>
            /// <param name="phGraph">The captured graph</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamEndCapture" + CUDA_PTSZ)]
            public static extern CUResult cuStreamEndCapture(CUstream hStream, ref CUgraph phGraph);

            /// <summary>
            /// Returns a stream's capture status<para/>
            /// Return the capture status of \p hStream via \p captureStatus. After a successful
            /// call, \p* captureStatus will contain one of the following:<para/>
            ///  - ::CU_STREAM_CAPTURE_STATUS_NONE: The stream is not capturing.<para/>
            ///  - ::CU_STREAM_CAPTURE_STATUS_ACTIVE: The stream is capturing.<para/>
            ///  - ::CU_STREAM_CAPTURE_STATUS_INVALIDATED: The stream was capturing but an error
            ///  has invalidated the capture sequence. The capture sequence must be terminated
            ///  with::cuStreamEndCapture on the stream where it was initiated in order to
            ///  continue using \p hStream.
            ///  <para/>
            ///  Note that, if this is called on ::CU_STREAM_LEGACY (the "null stream") while
            ///  a blocking stream in the same context is capturing, it will return
            ///  ::CUDA_ERROR_STREAM_CAPTURE_IMPLICIT and \p* captureStatus is unspecified
            ///  after the call.The blocking stream capture is not invalidated.<para/>
            /// When a blocking stream is capturing, the legacy stream is in an
            /// unusable state until the blocking stream capture is terminated.The legacy
            /// stream is not supported for stream capture, but attempted use would have an
            /// implicit dependency on the capturing stream(s).
            /// </summary>
            /// <param name="hStream">Stream to query</param>
            /// <param name="captureStatus">Returns the stream's capture status</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamIsCapturing" + CUDA_PTSZ)]
            public static extern CUResult cuStreamIsCapturing(CUstream hStream, ref CUstreamCaptureStatus captureStatus);


            /// <summary>
            /// Swaps the stream capture interaction mode for a thread<para/>
            /// Sets the calling thread's stream capture interaction mode to the value contained
            /// in \p* mode, and overwrites \p* mode with the previous mode for the thread.To
            /// facilitate deterministic behavior across function or module boundaries, callers
            /// are encouraged to use this API in a push-pop fashion: \code<para/>
            ///   CUstreamCaptureMode mode = desiredMode;<para/>
            ///   cuThreadExchangeStreamCaptureMode(&amp;mode);<para/>
            ///   ...<para/>
            ///   cuThreadExchangeStreamCaptureMode(&amp;mode); // restore previous mode<para/>
            /// \endcode<para/>
            /// During stream capture(see::cuStreamBeginCapture), some actions, such as a call
            /// to::cudaMalloc, may be unsafe. In the case of::cudaMalloc, the operation is
            /// not enqueued asynchronously to a stream, and is not observed by stream capture.
            /// Therefore, if the sequence of operations captured via ::cuStreamBeginCapture
            /// depended on the allocation being replayed whenever the graph is launched, the
            /// captured graph would be invalid.<para/>
            /// Therefore, stream capture places restrictions on API calls that can be made within
            /// or concurrently to a ::cuStreamBeginCapture-::cuStreamEndCapture sequence. This
            /// behavior can be controlled via this API and flags to ::cuStreamBeginCapture.<para/>
            /// A thread's mode is one of the following:<para/>
            /// - \p CU_STREAM_CAPTURE_MODE_GLOBAL: This is the default mode.If the local thread has
            /// an ongoing capture sequence that was not initiated with
            ///   \p CU_STREAM_CAPTURE_MODE_RELAXED at \p cuStreamBeginCapture, or if any other thread
            /// has a concurrent capture sequence initiated with \p CU_STREAM_CAPTURE_MODE_GLOBAL,
            ///   this thread is prohibited from potentially unsafe API calls.<para/>
            /// - \p CU_STREAM_CAPTURE_MODE_THREAD_LOCAL: If the local thread has an ongoing capture
            ///   sequence not initiated with \p CU_STREAM_CAPTURE_MODE_RELAXED, it is prohibited
            /// from potentially unsafe API calls.Concurrent capture sequences in other threads
            ///   are ignored.<para/>
            /// - \p CU_STREAM_CAPTURE_MODE_RELAXED: The local thread is not prohibited from potentially
            ///   unsafe API calls.Note that the thread is still prohibited from API calls which
            ///   necessarily conflict with stream capture, for example, attempting::cuEventQuery
            /// on an event that was last recorded inside a capture sequence.<para/>
            /// </summary>
            /// <param name="mode"></param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuThreadExchangeStreamCaptureMode(ref CUstreamCaptureMode mode);


            /// <summary>
            /// Query capture status of a stream<para/>
            /// Query the capture status of a stream and and get an id for
            /// the capture sequence, which is unique over the lifetime of the process.<para/>
            /// If called on::CU_STREAM_LEGACY(the "null stream") while a stream not created
            /// with::CU_STREAM_NON_BLOCKING is capturing, returns::CUDA_ERROR_STREAM_CAPTURE_IMPLICIT.<para/>
            /// A valid id is returned only if both of the following are true:<para/>
            /// - the call returns CUDA_SUCCESS<para/>
            /// - captureStatus is set to ::CU_STREAM_CAPTURE_STATUS_ACTIVE<para/>
            /// </summary>
            /// <param name="hStream"></param>
            /// <param name="captureStatus"></param>
            /// <param name="id"></param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetCaptureInfo" + CUDA_PTSZ)]
            public static extern CUResult cuStreamGetCaptureInfo(CUstream hStream, ref CUstreamCaptureStatus captureStatus, ref ulong id);


            /// <summary>
            /// Query a stream's capture state (11.3+)<para/>
            /// Query stream state related to stream capture.
            /// <para/>
            /// If called on ::CU_STREAM_LEGACY(the "null stream") while a stream not created 
            /// with::CU_STREAM_NON_BLOCKING is capturing, returns::CUDA_ERROR_STREAM_CAPTURE_IMPLICIT.
            /// <para/>
            /// Valid data(other than capture status) is returned only if both of the following are true:
            /// - the call returns CUDA_SUCCESS
            /// - the returned capture status is ::CU_STREAM_CAPTURE_STATUS_ACTIVE
            /// <para/>
            /// This version of cuStreamGetCaptureInfo is introduced in CUDA 11.3 and will supplant the
            /// previous version in 12.0. Developers requiring compatibility across minor versions to
            /// CUDA 11.0 (driver version 445) should use ::cuStreamGetCaptureInfo or include a fallback
            /// path.
            /// </summary>
            /// <param name="hStream">The stream to query</param>
            /// <param name="captureStatus_out">captureStatus_out - Location to return the capture status of the stream; required</param>
            /// <param name="id_out">Optional location to return an id for the capture sequence, which is unique over the lifetime of the process</param>
            /// <param name="graph_out">Optional location to return the graph being captured into. All operations other than destroy and node removal are permitted on the graph
            /// while the capture sequence is in progress.This API does not transfer
            /// ownership of the graph, which is transferred or destroyed at
            /// ::cuStreamEndCapture.Note that the graph handle may be invalidated before
            /// end of capture for certain errors.Nodes that are or become
            /// unreachable from the original stream at ::cuStreamEndCapture due to direct
            /// actions on the graph do not trigger ::CUDA_ERROR_STREAM_CAPTURE_UNJOINED.</param>
            /// <param name="dependencies_out">Optional location to store a pointer to an array of nodes. The next node to be captured in the stream will depend on this set of nodes,
            /// absent operations such as event wait which modify this set.The array pointer
            /// is valid until the next API call which operates on the stream or until end of
            /// capture. The node handles may be copied out and are valid until they or the
            /// graph is destroyed.The driver-owned array may also be passed directly to
            /// APIs that operate on the graph (not the stream) without copying.</param>
            /// <param name="numDependencies_out">Optional location to store the size of the array returned in dependencies_out.</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetCaptureInfo" + CUDA_PTSZ)]
            public static extern CUResult cuStreamGetCaptureInfo_v2(CUstream hStream, ref CUstreamCaptureStatus captureStatus_out,
                    ref ulong id_out, ref CUgraph graph_out, ref IntPtr dependencies_out, ref SizeT numDependencies_out);
            /**
             * \brief Update the set of dependencies in a capturing stream (11.3+)
             *
             * Modifies the dependency set of a capturing stream. The dependency set is the set
             * of nodes that the next captured node in the stream will depend on.
             *
             * Valid flags are ::CU_STREAM_ADD_CAPTURE_DEPENDENCIES and
             * ::CU_STREAM_SET_CAPTURE_DEPENDENCIES. These control whether the set passed to
             * the API is added to the existing set or replaces it. A flags value of 0 defaults
             * to ::CU_STREAM_ADD_CAPTURE_DEPENDENCIES.
             *
             * Nodes that are removed from the dependency set via this API do not result in
             * ::CUDA_ERROR_STREAM_CAPTURE_UNJOINED if they are unreachable from the stream at
             * ::cuStreamEndCapture.
             *
             * Returns ::CUDA_ERROR_ILLEGAL_STATE if the stream is not capturing.
             *
             * This API is new in CUDA 11.3. Developers requiring compatibility across minor
             * versions to CUDA 11.0 should not use this API or provide a fallback.
             *
             * \return
             * ::CUDA_SUCCESS,
             * ::CUDA_ERROR_INVALID_VALUE,
             * ::CUDA_ERROR_ILLEGAL_STATE
             *
             * \sa
             * ::cuStreamBeginCapture,
             * ::cuStreamGetCaptureInfo,
             * ::cuStreamGetCaptureInfo_v2
             */
            /// <summary>
            /// Update the set of dependencies in a capturing stream (11.3+)<para/>
            /// Modifies the dependency set of a capturing stream. The dependency set is the set of nodes that the next captured node in the stream will depend on.<para/>
            /// Valid flags are ::CU_STREAM_ADD_CAPTURE_DEPENDENCIES and
            /// ::CU_STREAM_SET_CAPTURE_DEPENDENCIES.These control whether the set passed to
            /// the API is added to the existing set or replaces it.A flags value of 0 defaults
            /// to ::CU_STREAM_ADD_CAPTURE_DEPENDENCIES.
            /// Nodes that are removed from the dependency set via this API do not result in
            /// ::CUDA_ERROR_STREAM_CAPTURE_UNJOINED if they are unreachable from the stream at
            /// ::cuStreamEndCapture.
            /// Returns ::CUDA_ERROR_ILLEGAL_STATE if the stream is not capturing.
            /// This API is new in CUDA 11.3. Developers requiring compatibility across minor
            /// versions to CUDA 11.0 should not use this API or provide a fallback.
            /// </summary>
            /// <param name="hStream"></param>
            /// <param name="dependencies"></param>
            /// <param name="numDependencies"></param>
            /// <param name="flags"></param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuStreamGetCaptureInfo" + CUDA_PTSZ)]
            public static extern CUResult cuStreamUpdateCaptureDependencies(CUstream hStream, CUgraphNode[] dependencies, SizeT numDependencies, uint flags);

        }
        #endregion

        #region Graphics interop
        /// <summary>
        /// Combines all graphics interop API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class GraphicsInterop
        {
#if (NETCOREAPP)
            static GraphicsInterop()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Unregisters the graphics resource <c>resource</c> so it is not accessible by CUDA unless registered again.
            /// If resource is invalid then <see cref="CUResult.ErrorInvalidHandle"/> is returned.
            /// </summary>
            /// <param name="resource">Resource to unregister</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphicsUnregisterResource(CUgraphicsResource resource);

            /// <summary>
            /// Returns in <c>pArray</c> an array through which the subresource of the mapped graphics resource resource which
            /// corresponds to array index <c>arrayIndex</c> and mipmap level <c>mipLevel</c> may be accessed. The value set in <c>pArray</c>
            /// may change every time that <c>resource</c> is mapped.<para/>
            /// If <c>resource</c> is not a texture then it cannot be accessed via an array and <see cref="CUResult.ErrorNotMappedAsArray"/>
            /// is returned. If <c>arrayIndex</c> is not a valid array index for <c>resource</c> then <see cref="CUResult.ErrorInvalidValue"/>
            /// is returned. If <c>mipLevel</c> is not a valid mipmap level for <c>resource</c> then <see cref="CUResult.ErrorInvalidValue"/>
            /// is returned. If <c>resource</c> is not mapped then <see cref="CUResult.ErrorNotMapped"/> is returned.
            /// </summary>
            /// <param name="pArray">Returned array through which a subresource of <c>resource</c> may be accessed</param>
            /// <param name="resource">Mapped resource to access</param>
            /// <param name="arrayIndex">Array index for array textures or cubemap face index as defined by <see cref="CUArrayCubemapFace"/> for
            /// cubemap textures for the subresource to access</param>
            /// <param name="mipLevel">Mipmap level for the subresource to access</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorNotMapped"/>, <see cref="CUResult.ErrorNotMappedAsArray"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphicsSubResourceGetMappedArray(ref CUarray pArray, CUgraphicsResource resource, uint arrayIndex, uint mipLevel);

            /// <summary>
            /// Returns in <c>pMipmappedArray</c> a mipmapped array through which the mapped graphics 
            /// resource <c>resource</c>. The value set in <c>pMipmappedArray</c> may change every time 
            /// that <c>resource</c> is mapped.
            /// <para/>
            /// If <c>resource</c> is not a texture then it cannot be accessed via a mipmapped array and
            /// <see cref="CUResult.ErrorNotMappedAsArray"/> is returned.
            /// If <c>resource</c> is not mapped then <see cref="CUResult.ErrorNotMapped"/> is returned.
            /// </summary>
            /// <param name="pMipmappedArray">Returned mipmapped array through which <c>resource</c> may be accessed</param>
            /// <param name="resource">Mapped resource to access</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorNotMapped"/>, <see cref="CUResult.ErrorNotMappedAsArray"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphicsResourceGetMappedMipmappedArray(ref CUmipmappedArray pMipmappedArray, CUgraphicsResource resource);


            /// <summary>
            /// Returns in <c>pDevPtr</c> a pointer through which the mapped graphics resource <c>resource</c> may be accessed. Returns
            /// in <c>pSize</c> the size of the memory in bytes which may be accessed from that pointer. The value set in <c>pPointer</c> may
            /// change every time that <c>resource</c> is mapped.<para/>
            /// If <c>resource</c> is not a buffer then it cannot be accessed via a pointer and <see cref="CUResult.ErrorNotMappedAsPointer"/>
            /// is returned. If resource is not mapped then <see cref="CUResult.ErrorNotMapped"/> is returned.
            /// </summary>
            /// <param name="pDevPtr">Returned pointer through which <c>resource</c> may be accessed</param>
            /// <param name="pSize">Returned size of the buffer accessible starting at <c>pPointer</c></param>
            /// <param name="resource">Mapped resource to access</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorNotMapped"/>, <see cref="CUResult.ErrorNotMappedAsPointer"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphicsResourceGetMappedPointer_v2(ref CUdeviceptr pDevPtr, ref SizeT pSize, CUgraphicsResource resource);

            /// <summary>
            /// Set <c>flags</c> for mapping the graphics resource <c>resource</c>.
            /// Changes to <c>flags</c> will take effect the next time <c>resource</c> is mapped. See <see cref="CUGraphicsMapResourceFlags"/>. <para/>
            /// If <c>resource</c> is presently mapped for access by CUDA then <see cref="CUResult.ErrorAlreadyMapped"/> is returned. If
            /// <c>flags</c> is not one of the <see cref="CUGraphicsMapResourceFlags"/> values then <see cref="CUResult.ErrorInvalidValue"/> is returned.
            /// </summary>
            /// <param name="resource">Registered resource to set flags for</param>
            /// <param name="flags">Parameters for resource mapping</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorAlreadyMapped"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphicsResourceSetMapFlags_v2")]
            public static extern CUResult cuGraphicsResourceSetMapFlags(CUgraphicsResource resource, CUGraphicsMapResourceFlags flags);

            /// <summary>
            /// Maps the <c>count</c> graphics resources in <c>resources</c> for access by CUDA.<para/>
            /// The resources in <c>resources</c> may be accessed by CUDA until they are unmapped. The graphics API from which
            /// <c>resources</c> were registered should not access any resources while they are mapped by CUDA. If an application does
            /// so, the results are undefined.<para/>
            /// This function provides the synchronization guarantee that any graphics calls issued before <see cref="cuGraphicsMapResources(uint, ref CUgraphicsResource, CUstream)"/>
            /// will complete before any subsequent CUDA work issued in <c>stream</c> begins.<para/>
            /// If <c>resources</c> includes any duplicate entries then <see cref="CUResult.ErrorInvalidHandle"/> is returned. If any of
            /// <c>resources</c> are presently mapped for access by CUDA then <see cref="CUResult.ErrorAlreadyMapped"/> is returned.
            /// </summary>
            /// <param name="count">Number of resources to map. Here: must be 1</param>
            /// <param name="resources">Resources to map for CUDA usage</param>
            /// <param name="hStream">Stream with which to synchronize</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorAlreadyMapped"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphicsMapResources" + CUDA_PTSZ)]
            public static extern CUResult cuGraphicsMapResources(uint count, ref CUgraphicsResource resources, CUstream hStream);

            /// <summary>
            /// Maps the <c>count</c> graphics resources in <c>resources</c> for access by CUDA.<para/>
            /// The resources in <c>resources</c> may be accessed by CUDA until they are unmapped. The graphics API from which
            /// <c>resources</c> were registered should not access any resources while they are mapped by CUDA. If an application does
            /// so, the results are undefined.<para/>
            /// This function provides the synchronization guarantee that any graphics calls issued before <see cref="cuGraphicsMapResources(uint, CUgraphicsResource[], CUstream)"/>
            /// will complete before any subsequent CUDA work issued in <c>stream</c> begins.<para/>
            /// If <c>resources</c> includes any duplicate entries then <see cref="CUResult.ErrorInvalidHandle"/> is returned. If any of
            /// <c>resources</c> are presently mapped for access by CUDA then <see cref="CUResult.ErrorAlreadyMapped"/> is returned.
            /// </summary>
            /// <param name="count">Number of resources to map</param>
            /// <param name="resources">Resources to map for CUDA usage</param>
            /// <param name="hStream">Stream with which to synchronize</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorAlreadyMapped"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphicsMapResources" + CUDA_PTSZ)]
            public static extern CUResult cuGraphicsMapResources(uint count, CUgraphicsResource[] resources, CUstream hStream);

            /// <summary>
            /// Unmaps the <c>count</c> graphics resources in resources.<para/>
            /// Once unmapped, the resources in <c>resources</c> may not be accessed by CUDA until they are mapped again.<para/>
            /// This function provides the synchronization guarantee that any CUDA work issued in <c>stream</c> before <see cref="cuGraphicsUnmapResources(uint, ref CUgraphicsResource, CUstream)"/>
            /// will complete before any subsequently issued graphics work begins.<para/>
            /// If <c>resources</c> includes any duplicate entries then <see cref="CUResult.ErrorInvalidHandle"/> is returned. If any of
            /// resources are not presently mapped for access by CUDA then <see cref="CUResult.ErrorNotMapped"/> is returned.
            /// </summary>
            /// <param name="count">Number of resources to unmap. Here: must be 1</param>
            /// <param name="resources">Resources to unmap</param>
            /// <param name="hStream">Stream with which to synchronize</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorNotMapped"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphicsUnmapResources" + CUDA_PTSZ)]
            public static extern CUResult cuGraphicsUnmapResources(uint count, ref CUgraphicsResource resources, CUstream hStream);

            /// <summary>
            /// Unmaps the <c>count</c> graphics resources in resources.<para/>
            /// Once unmapped, the resources in <c>resources</c> may not be accessed by CUDA until they are mapped again.<para/>
            /// This function provides the synchronization guarantee that any CUDA work issued in <c>stream</c> before <see cref="cuGraphicsUnmapResources(uint, CUgraphicsResource[], CUstream)"/>
            /// will complete before any subsequently issued graphics work begins.<para/>
            /// If <c>resources</c> includes any duplicate entries then <see cref="CUResult.ErrorInvalidHandle"/> is returned. If any of
            /// resources are not presently mapped for access by CUDA then <see cref="CUResult.ErrorNotMapped"/> is returned.
            /// </summary>
            /// <param name="count">Number of resources to unmap</param>
            /// <param name="resources">Resources to unmap</param>
            /// <param name="hStream">Stream with which to synchronize</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidHandle"/>,
            /// <see cref="CUResult.ErrorNotMapped"/>, <see cref="CUResult.ErrorUnknown"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
			[DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphicsUnmapResources" + CUDA_PTSZ)]
            public static extern CUResult cuGraphicsUnmapResources(uint count, CUgraphicsResource[] resources, CUstream hStream);
        }
        #endregion   

        #region Export tables
        /// <summary>
        /// cuGetExportTable
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class ExportTables
        {
#if (NETCOREAPP)
            static ExportTables()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// No description found in the CUDA reference manual...
            /// </summary>
            /// <param name="ppExportTable"></param>
            /// <param name="pExportTableId"></param>
            /// <returns>CUDA Error Code<remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGetExportTable(ref IntPtr ppExportTable, ref CUuuid pExportTableId);
        }
        #endregion

        #region Limits
        /// <summary>
        /// Groups all context limit API calls
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class Limits
        {
#if (NETCOREAPP)
            static Limits()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Setting <c>limit</c> to <c>value</c> is a request by the application to update the current limit maintained by the context. The
            /// driver is free to modify the requested value to meet h/w requirements (this could be clamping to minimum or maximum
            /// values, rounding up to nearest element size, etc). The application can use <see cref="cuCtxGetLimit"/> to find out exactly what
            /// the limit has been set to.<para/>
            /// Setting each <see cref="CULimit"/> has its own specific restrictions, so each is discussed here:
            /// <list type="table">  
            /// <listheader><term>Value</term><description>Restriction</description></listheader>  
            /// <item><term><see cref="CULimit.StackSize"/></term><description>
            /// <see cref="CULimit.StackSize"/> controls the stack size of each GPU thread. This limit is only applicable to devices
            /// of compute capability 2.0 and higher. Attempting to set this limit on devices of compute capability less than 2.0
            /// will result in the error <see cref="CUResult.ErrorUnsupportedLimit"/> being returned.
            /// </description></item>  
            /// <item><term><see cref="CULimit.PrintfFIFOSize"/></term><description>
            /// <see cref="CULimit.PrintfFIFOSize"/> controls the size of the FIFO used by the <c>printf()</c> device system call. Setting
            /// <see cref="CULimit.PrintfFIFOSize"/> must be performed before loading any module that uses the printf() device
            /// system call, otherwise <see cref="CUResult.ErrorInvalidValue"/> will be returned. This limit is only applicable to
            /// devices of compute capability 2.0 and higher. Attempting to set this limit on devices of compute capability less
            /// than 2.0 will result in the error <see cref="CUResult.ErrorUnsupportedLimit"/> being returned.
            /// </description></item> 
			/// <item><term><see cref="CULimit.MallocHeapSize"/></term><description>
            /// <see cref="CULimit.MallocHeapSize"/> controls the size in bytes of the heap used by the ::malloc() and ::free() device system calls. Setting
            /// <see cref="CULimit.MallocHeapSize"/> must be performed before launching any kernel that uses the ::malloc() or ::free() device system calls, otherwise
            /// <see cref="CUResult.ErrorInvalidValue"/> will be returned. This limit is only applicable to
            /// devices of compute capability 2.0 and higher. Attempting to set this limit on devices of compute capability less
            /// than 2.0 will result in the error <see cref="CUResult.ErrorUnsupportedLimit"/> being returned.
            /// </description></item> 
			/// <item><term><see cref="CULimit.DevRuntimeSyncDepth"/></term><description>
            /// <see cref="CULimit.DevRuntimeSyncDepth"/> controls the maximum nesting depth of a grid at which a thread can safely call ::cudaDeviceSynchronize(). Setting
            /// this limit must be performed before any launch of a kernel that uses the
			/// device runtime and calls ::cudaDeviceSynchronize() above the default sync
			/// depth, two levels of grids. Calls to ::cudaDeviceSynchronize() will fail 
			/// with error code ::cudaErrorSyncDepthExceeded if the limitation is 
			/// violated. This limit can be set smaller than the default or up the maximum
			/// launch depth of 24. When setting this limit, keep in mind that additional
			/// levels of sync depth require the driver to reserve large amounts of device
			/// memory which can no longer be used for user allocations. If these 
			/// reservations of device memory fail, ::cuCtxSetLimit will return 
			/// <see cref="CUResult.ErrorOutOfMemory"/>, and the limit can be reset to a lower value.
			/// This limit is only applicable to devices of compute capability 3.5 and
			/// higher. Attempting to set this limit on devices of compute capability less
			/// than 3.5 will result in the error <see cref="CUResult.ErrorUnsupportedLimit"/> being 
			/// returned.
            /// </description></item> 
			/// <item><term><see cref="CULimit.DevRuntimePendingLaunchCount"/></term><description>
            /// <see cref="CULimit.DevRuntimePendingLaunchCount"/> controls the maximum number of 
			/// outstanding device runtime launches that can be made from the current
			/// context. A grid is outstanding from the point of launch up until the grid
			/// is known to have been completed. Device runtime launches which violate 
			/// this limitation fail and return ::cudaErrorLaunchPendingCountExceeded when
			/// ::cudaGetLastError() is called after launch. If more pending launches than
			/// the default (2048 launches) are needed for a module using the device
			/// runtime, this limit can be increased. Keep in mind that being able to
			/// sustain additional pending launches will require the driver to reserve
			/// larger amounts of device memory upfront which can no longer be used for
			/// allocations. If these reservations fail, ::cuCtxSetLimit will return
			/// <see cref="CUResult.ErrorOutOfMemory"/>, and the limit can be reset to a lower value.
			/// This limit is only applicable to devices of compute capability 3.5 and
			/// higher. Attempting to set this limit on devices of compute capability less
			/// than 3.5 will result in the error <see cref="CUResult.ErrorUnsupportedLimit"/> being
			/// returned. 
            /// </description></item> 
            /// </list>   
            /// </summary>
            /// <param name="limit">Limit to set</param>
            /// <param name="value">Size in bytes of limit</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorUnsupportedLimit"/>, .
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxSetLimit(CULimit limit, SizeT value);

            /// <summary>
            /// Returns in <c>pvalue</c> the current size of limit. See <see cref="CULimit"/>
            /// </summary>
            /// <param name="pvalue">Returned size in bytes of limit</param>
            /// <param name="limit">Limit to query</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorUnsupportedLimit"/>, .
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxGetLimit(ref SizeT pvalue, CULimit limit);
        }
        #endregion

        #region CudaPeerAccess
        /// <summary>
        /// Peer Context Memory Access
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurityAttribute]
        public static class CudaPeerAccess
        {
#if (NETCOREAPP)
            static CudaPeerAccess()
            {
                DriverAPINativeMethods.Init();
            }
#endif      
            /// <summary>
            /// Returns in <c>canAccessPeer</c> a value of 1 if contexts on <c>dev</c> are capable of
            /// directly accessing memory from contexts on <c>peerDev</c> and 0 otherwise.
            /// If direct access of <c>peerDev</c> from <c>dev</c> is possible, then access may be
            /// enabled on two specific contexts by calling <see cref="cuCtxEnablePeerAccess"/>.
            /// </summary>
            /// <param name="canAccessPeer">Returned access capability</param>
            /// <param name="dev">Device from which allocations on peerDev are to be directly accessed.</param>
            /// <param name="peerDev">Device on which the allocations to be directly accessed by dev reside.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidDevice"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceCanAccessPeer(ref int canAccessPeer, CUdevice dev, CUdevice peerDev);

            /// <summary>
            /// If both the current context and <c>peerContext</c> are on devices which support unified 
            /// addressing (as may be queried using ::CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING), then
            /// on success all allocations from <c>peerContext</c> will immediately be accessible
            /// by the current context.  See \ref CUDA_UNIFIED for additional
            /// details. <para/>
            /// Note that access granted by this call is unidirectional and that in order to access
            /// memory from the current context in <c>peerContext</c>, a separate symmetric call 
            /// to ::cuCtxEnablePeerAccess() is required. <para/>
            /// Returns <see cref="CUResult.ErrorInvalidDevice"/> if <see cref="cuDeviceCanAccessPeer"/> indicates
            /// that the CUdevice of the current context cannot directly access memory
            /// from the CUdevice of <c>peerContext</c>. <para/>
            /// Returns <see cref="CUResult.ErrorPeerAccessAlreadyEnabled"/> if direct access of
            /// <c>peerContext</c> from the current context has already been enabled. <para/>
            /// Returns <see cref="CUResult.ErrorInvalidContext"/> if there is no current context, <c>peerContext</c>
            /// is not a valid context, or if the current context is <c>peerContext</c>. <para/>
            /// Returns <see cref="CUResult.ErrorInvalidValue"/> if <c>Flags</c> is not 0.
            /// </summary>
            /// <param name="peerContext">Peer context to enable direct access to from the current context</param>
            /// <param name="Flags">Reserved for future use and must be set to 0</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidDevice"/>, <see cref="CUResult.ErrorPeerAccessAlreadyEnabled"/>, <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxEnablePeerAccess(CUcontext peerContext, CtxEnablePeerAccessFlags Flags);

            /// <summary>
            /// Disables direct access to memory allocations in a peer context and unregisters any registered allocations.
            /// </summary>
            /// <param name="peerContext">Peer context to disable direct access to</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorPeerAccessNotEnabled"/>, <see cref="CUResult.ErrorInvalidContext"/>.
            /// <remarks>Note that this function may also return error codes from previous, asynchronous launches.</remarks></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuCtxDisablePeerAccess(CUcontext peerContext);



            /// <summary>
            /// Queries attributes of the link between two devices.<para/>
            /// Returns in \p *value the value of the requested attribute \p attrib of the
            /// link between \p srcDevice and \p dstDevice. The supported attributes are:<para/>
            /// - ::CU_DEVICE_P2P_ATTRIBUTE_PERFORMANCE_RANK: A relative value indicating the
            /// performance of the link between two devices.<para/>
            /// - ::CU_DEVICE_P2P_ATTRIBUTE_ACCESS_SUPPORTED P2P: 1 if P2P Access is enable.<para/>
            /// - ::CU_DEVICE_P2P_ATTRIBUTE_NATIVE_ATOMIC_SUPPORTED: 1 if Atomic operations over
            /// the link are supported.
            /// </summary>
            /// <param name="value">Returned value of the requested attribute</param>
            /// <param name="attrib">The requested attribute of the link between \p srcDevice and \p dstDevice.</param>
            /// <param name="srcDevice">The source device of the target link.</param>
            /// <param name="dstDevice">The destination device of the target link.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetP2PAttribute(ref int value, CUdevice_P2PAttribute attrib, CUdevice srcDevice, CUdevice dstDevice);

        }
        #endregion

        #region Texture objects
        /// <summary>
        /// Texture object management functions.
        /// </summary>
        public static class TextureObjects
        {
#if (NETCOREAPP)
            static TextureObjects()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Creates a texture object and returns it in <c>pTexObject</c>. <c>pResDesc</c> describes
            /// the data to texture from. <c>pTexDesc</c> describes how the data should be sampled.
            /// <c>pResViewDesc</c> is an optional argument that specifies an alternate format for
            /// the data described by <c>pResDesc</c>, and also describes the subresource region
            /// to restrict access to when texturing. <c>pResViewDesc</c> can only be specified if
            /// the type of resource is a CUDA array or a CUDA mipmapped array.
            /// </summary>
            /// <param name="pTexObject">Texture object to create</param>
            /// <param name="pResDesc">Resource descriptor</param>
            /// <param name="pTexDesc">Texture descriptor</param>
            /// <param name="pResViewDesc">Resource view descriptor</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuTexObjectCreate(ref CUtexObject pTexObject, ref CudaResourceDesc pResDesc, ref CudaTextureDescriptor pTexDesc, ref CudaResourceViewDesc pResViewDesc);

            /// <summary>
            /// Creates a texture object and returns it in <c>pTexObject</c>. <c>pResDesc</c> describes
            /// the data to texture from. <c>pTexDesc</c> describes how the data should be sampled.
            /// <c>pResViewDesc</c> is an optional argument that specifies an alternate format for
            /// the data described by <c>pResDesc</c>, and also describes the subresource region
            /// to restrict access to when texturing. <c>pResViewDesc</c> can only be specified if
            /// the type of resource is a CUDA array or a CUDA mipmapped array.
            /// </summary>
            /// <param name="pTexObject">Texture object to create</param>
            /// <param name="pResDesc">Resource descriptor</param>
            /// <param name="pTexDesc">Texture descriptor</param>
            /// <param name="pResViewDesc">Resource view descriptor (Null-Pointer)</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuTexObjectCreate(ref CUtexObject pTexObject, ref CudaResourceDesc pResDesc, ref CudaTextureDescriptor pTexDesc, IntPtr pResViewDesc);

            /// <summary>
            /// Destroys the texture object specified by <c>texObject</c>.
            /// </summary>
            /// <param name="texObject">Texture object to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuTexObjectDestroy(CUtexObject texObject);

            /// <summary>
            /// Returns the resource descriptor for the texture object specified by <c>texObject</c>.
            /// </summary>
            /// <param name="pResDesc">Resource descriptor</param>
            /// <param name="texObject">Texture object</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuTexObjectGetResourceDesc(ref CudaResourceDesc pResDesc, CUtexObject texObject);

            /// <summary>
            /// Returns the texture descriptor for the texture object specified by <c>texObject</c>.
            /// </summary>
            /// <param name="pTexDesc">Texture descriptor</param>
            /// <param name="texObject">Texture object</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuTexObjectGetTextureDesc(ref CudaTextureDescriptor pTexDesc, CUtexObject texObject);

            /// <summary>
            /// Returns the resource view descriptor for the texture object specified by <c>texObject</c>.
            /// If no resource view was set for <c>texObject</c>, the ::CUDA_ERROR_INVALID_VALUE is returned.
            /// </summary>
            /// <param name="pResViewDesc">Resource view descriptor</param>
            /// <param name="texObject">Texture object</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuTexObjectGetResourceViewDesc(ref CudaResourceViewDesc pResViewDesc, CUtexObject texObject);

        }
        #endregion

        #region Surface objects
        /// <summary>
        /// Surface object management functions.
        /// </summary>
        public static class SurfaceObjects
        {
#if (NETCOREAPP)
            static SurfaceObjects()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Creates a surface object and returns it in <c>pSurfObject</c>. <c>pResDesc</c> describes
            /// the data to perform surface load/stores on. ::CUDA_RESOURCE_DESC::resType must be 
            /// ::CU_RESOURCE_TYPE_ARRAY and  ::CUDA_RESOURCE_DESC::res::array::hArray
            /// must be set to a valid CUDA array handle. ::CUDA_RESOURCE_DESC::flags must be set to zero.
            /// </summary>
            /// <param name="pSurfObject">Surface object to create</param>
            /// <param name="pResDesc">Resource descriptor</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuSurfObjectCreate(ref CUsurfObject pSurfObject, ref CudaResourceDesc pResDesc);

            /// <summary>
            /// Destroys the surface object specified by <c>surfObject</c>.
            /// </summary>
            /// <param name="surfObject">Surface object to destroy</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuSurfObjectDestroy(CUsurfObject surfObject);

            /// <summary>
            /// Returns the resource descriptor for the surface object specified by <c>surfObject</c>.
            /// </summary>
            /// <param name="pResDesc">Resource descriptor</param>
            /// <param name="surfObject">Surface object</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuSurfObjectGetResourceDesc(ref CudaResourceDesc pResDesc, CUsurfObject surfObject);

        }
        #endregion

        #region Profiling

        /// <summary>
        /// This section describes the profiler control functions of the low-level CUDA
        /// driver application programming interface.
        /// </summary>
        public static class Profiling
        {
#if (NETCOREAPP)
            static Profiling()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Initialize the profiling.<para/>
            /// Using this API user can initialize the CUDA profiler by specifying
            /// the configuration file, output file and output file format. This
            /// API is generally used to profile different set of counters by
            /// looping the kernel launch. The <c>configFile</c> parameter can be used
            /// to select profiling options including profiler counters. Refer to
            /// the "Compute Command Line Profiler User Guide" for supported
            /// profiler options and counters.<para/>
            /// Limitation: The CUDA profiler cannot be initialized with this API
            /// if another profiling tool is already active, as indicated by the
            /// <see cref="CUResult.ErrorProfilerDisabled"/> return code.
            /// </summary>
            /// <param name="configFile">Name of the config file that lists the counters/options for profiling.</param>
            /// <param name="outputFile">Name of the outputFile where the profiling results will be stored.</param>
            /// <param name="outputMode">outputMode</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorProfilerDisabled"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuProfilerInitialize(string configFile, string outputFile, CUoutputMode outputMode);

            /// <summary>
            /// Enable profiling.<para/>
            /// Enables profile collection by the active profiling tool for the
            /// current context. If profiling is already enabled, then
            /// cuProfilerStart() has no effect.<para/>
            /// cuProfilerStart and cuProfilerStop APIs are used to
            /// programmatically control the profiling granularity by allowing
            /// profiling to be done only on selective pieces of code.
            /// </summary>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidContext"/>. 
            /// </returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuProfilerStart();

            /// <summary>
            /// Disables profile collection by the active profiling tool for the
            /// current context. If profiling is already disabled, then
            /// cuProfilerStop() has no effect.<para/>
            /// cuProfilerStart and cuProfilerStop APIs are used to
            /// programmatically control the profiling granularity by allowing
            /// profiling to be done only on selective pieces of code.
            /// </summary>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidContext"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuProfilerStop();
        }
        #endregion

        #region Error Handling

        /// <summary>
        /// This section describes the error handling functions of the low-level CUDA
        /// driver application programming interface.
        /// </summary>
        public static class ErrorHandling
        {
#if (NETCOREAPP)
            static ErrorHandling()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Gets the string description of an error code.<para/>
            /// Sets <c>pStr</c> to the address of a NULL-terminated string description
            /// of the error code <c>error</c>.
            /// If the error code is not recognized, <see cref="CUResult.ErrorInvalidValue"/>
            /// will be returned and <c>pStr</c> will be set to the NULL address
            /// </summary>
            /// <param name="error">Error code to convert to string.</param>
            /// <param name="pStr">Address of the string pointer.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGetErrorString(CUResult error, ref IntPtr pStr);


            /// <summary>
            /// Gets the string representation of an error code enum name.<para/>
            /// Sets <c>pStr</c> to the address of a NULL-terminated string description
            /// of the name of the enum error code <c>error</c>.
            /// If the error code is not recognized, <see cref="CUResult.ErrorInvalidValue"/>
            /// will be returned and <c>pStr</c> will be set to the NULL address
            /// </summary>
            /// <param name="error">Error code to convert to string.</param>
            /// <param name="pStr">Address of the string pointer.</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorInvalidValue"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGetErrorName(CUResult error, ref IntPtr pStr);

        }
        #endregion

        #region Occupancy

        /// <summary>
        /// This section describes the occupancy calculation functions of the low-level CUDA
        /// driver application programming interface.
        /// </summary>
        public static class Occupancy
        {
#if (NETCOREAPP)
            static Occupancy()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Returns in numBlocks the number of the maximum active blocks per
            /// streaming multiprocessor.
            /// </summary>
            /// <param name="numBlocks">Returned occupancy</param>
            /// <param name="func">Kernel for which occupancy is calulated</param>
            /// <param name="blockSize">Block size the kernel is intended to be launched with</param>
            /// <param name="dynamicSMemSize">Per-block dynamic shared memory usage intended, in bytes</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorUnknown"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuOccupancyMaxActiveBlocksPerMultiprocessor(ref int numBlocks, CUfunction func, int blockSize, SizeT dynamicSMemSize);

            /// <summary>
            /// Returns in blockSize a reasonable block size that can achieve
            /// the maximum occupancy (or, the maximum number of active warps with
            /// the fewest blocks per multiprocessor), and in minGridSize the
            /// minimum grid size to achieve the maximum occupancy.
            /// 
            /// If blockSizeLimit is 0, the configurator will use the maximum
            /// block size permitted by the device / function instead.
            /// 
            /// If per-block dynamic shared memory allocation is not needed, the
            /// user should leave both blockSizeToDynamicSMemSize and 
            /// dynamicSMemSize as 0.
            /// 
            /// If per-block dynamic shared memory allocation is needed, then if
            /// the dynamic shared memory size is constant regardless of block
            /// size, the size should be passed through dynamicSMemSize, and 
            /// blockSizeToDynamicSMemSize should be NULL.
            /// 
            /// Otherwise, if the per-block dynamic shared memory size varies with
            /// different block sizes, the user needs to provide a unary function
            /// through blockSizeToDynamicSMemSize that computes the dynamic
            /// shared memory needed by func for any given block size.
            /// dynamicSMemSize is ignored.
            /// </summary>
            /// <param name="minGridSize">Returned minimum grid size needed to achieve the maximum occupancy</param>
            /// <param name="blockSize">Returned maximum block size that can achieve the maximum occupancy</param>
            /// <param name="func">Kernel for which launch configuration is calulated</param>
            /// <param name="blockSizeToDynamicSMemSize">A function that calculates how much per-block dynamic shared memory \p func uses based on the block size</param>
            /// <param name="dynamicSMemSize">Dynamic shared memory usage intended, in bytes</param>
            /// <param name="blockSizeLimit">The maximum block size \p func is designed to handle</param>
            /// <returns>CUDA Error Codes: <see cref="CUResult.Success"/>, <see cref="CUResult.ErrorDeinitialized"/>, <see cref="CUResult.ErrorNotInitialized"/>, 
            /// <see cref="CUResult.ErrorInvalidContext"/>, <see cref="CUResult.ErrorInvalidValue"/>, <see cref="CUResult.ErrorUnknown"/>.</returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuOccupancyMaxPotentialBlockSize(ref int minGridSize, ref int blockSize, CUfunction func, del_CUoccupancyB2DSize blockSizeToDynamicSMemSize, SizeT dynamicSMemSize, int blockSizeLimit);


            /// <summary>
            /// Returns occupancy of a function<para/>
            /// Returns in \p *numBlocks the number of the maximum active blocks per
            /// streaming multiprocessor.
            /// 
            /// The \p Flags parameter controls how special cases are handled. The
            /// valid flags are:
            /// 
            /// - ::CU_OCCUPANCY_DEFAULT, which maintains the default behavior as
            /// ::cuOccupancyMaxActiveBlocksPerMultiprocessor;
            /// - ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE, which suppresses the
            /// default behavior on platform where global caching affects
            /// occupancy. On such platforms, if caching is enabled, but
            /// per-block SM resource usage would result in zero occupancy, the
            /// occupancy calculator will calculate the occupancy as if caching
            /// is disabled. Setting ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE makes
            /// the occupancy calculator to return 0 in such cases. More information
            /// can be found about this feature in the "Unified L1/Texture Cache"
            /// section of the Maxwell tuning guide.
            /// </summary>
            /// <param name="numBlocks">Returned occupancy</param>
            /// <param name="func">Kernel for which occupancy is calculated</param>
            /// <param name="blockSize">Block size the kernel is intended to be launched with</param>
            /// <param name="dynamicSMemSize">Per-block dynamic shared memory usage intended, in bytes</param>
            /// <param name="flags">Requested behavior for the occupancy calculator</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuOccupancyMaxActiveBlocksPerMultiprocessorWithFlags(ref int numBlocks, CUfunction func, int blockSize, SizeT dynamicSMemSize, CUoccupancy_flags flags);

            /// <summary>
            /// Suggest a launch configuration with reasonable occupancy<para/>
            /// An extended version of ::cuOccupancyMaxPotentialBlockSize. In
            /// addition to arguments passed to ::cuOccupancyMaxPotentialBlockSize,
            /// ::cuOccupancyMaxPotentialBlockSizeWithFlags also takes a \p Flags
            /// parameter.
            /// 
            /// The \p Flags parameter controls how special cases are handled. The
            /// valid flags are:
            /// - ::CU_OCCUPANCY_DEFAULT, which maintains the default behavior as
            ///   ::cuOccupancyMaxPotentialBlockSize;
            /// - ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE, which suppresses the
            ///   default behavior on platform where global caching affects
            ///   occupancy. On such platforms, the launch configurations that
            ///   produces maximal occupancy might not support global
            ///   caching. Setting ::CU_OCCUPANCY_DISABLE_CACHING_OVERRIDE
            ///   guarantees that the the produced launch configuration is global
            ///   caching compatible at a potential cost of occupancy. More information
            ///   can be found about this feature in the "Unified L1/Texture Cache"
            ///   section of the Maxwell tuning guide.
            /// </summary>
            /// <param name="minGridSize">Returned minimum grid size needed to achieve the maximum occupancy</param>
            /// <param name="blockSize">Returned maximum block size that can achieve the maximum occupancy</param>
            /// <param name="func">Kernel for which launch configuration is calculated</param>
            /// <param name="blockSizeToDynamicSMemSize">A function that calculates how much per-block dynamic shared memory \p func uses based on the block size</param>
            /// <param name="dynamicSMemSize">Dynamic shared memory usage intended, in bytes</param>
            /// <param name="blockSizeLimit">The maximum block size \p func is designed to handle</param>
            /// <param name="flags">Options</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuOccupancyMaxPotentialBlockSizeWithFlags(ref int minGridSize, ref int blockSize, CUfunction func, del_CUoccupancyB2DSize blockSizeToDynamicSMemSize, SizeT dynamicSMemSize, int blockSizeLimit, CUoccupancy_flags flags);

            /// <summary>
            /// Returns dynamic shared memory available per block when launching \p numBlocks blocks on SM <para/>
            /// Returns in \p *dynamicSmemSize the maximum size of dynamic shared memory to allow \p numBlocks blocks per SM. 
            /// </summary>
            /// <param name="dynamicSmemSize">Returned maximum dynamic shared memory</param>
            /// <param name="func">Kernel function for which occupancy is calculated</param>
            /// <param name="numBlocks">Number of blocks to fit on SM </param>
            /// <param name="blockSize">Size of the blocks</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuOccupancyAvailableDynamicSMemPerBlock(ref SizeT dynamicSmemSize, CUfunction func, int numBlocks, int blockSize);

        }
        #endregion

        #region Extern Resources

        /// <summary>
        /// 
        /// </summary>
        public static class ExternResources
        {
#if (NETCOREAPP)
            static ExternResources()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Imports an external memory object<para/>
            /// Imports an externally allocated memory object and returns a handle to that in \p extMem_out.
            /// </summary>
            /// <param name="extMem_out">Returned handle to an external memory object</param>
            /// <param name="memHandleDesc">Memory import handle descriptor</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuImportExternalMemory(ref CUexternalMemory extMem_out, ref CudaExternalMemoryHandleDesc memHandleDesc);

            /// <summary>
            /// Maps a buffer onto an imported memory object<para/>
            /// Maps a buffer onto an imported memory object and returns a device pointer in \p devPtr.
            /// </summary>
            /// <param name="devPtr">Returned device pointer to buffer</param>
            /// <param name="extMem">Handle to external memory object</param>
            /// <param name="bufferDesc">Buffer descriptor</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuExternalMemoryGetMappedBuffer(ref CUdeviceptr devPtr, CUexternalMemory extMem, ref CudaExternalMemoryBufferDesc bufferDesc);

            /// <summary>
            /// Maps a CUDA mipmapped array onto an external memory object<para/>
            /// Maps a CUDA mipmapped array onto an external object and returns a handle to it in \p mipmap.
            /// The properties of the CUDA mipmapped array being mapped must be described in \p mipmapDesc.
            /// </summary>
            /// <param name="mipmap">Returned CUDA mipmapped array</param>
            /// <param name="extMem">Handle to external memory object</param>
            /// <param name="mipmapDesc">CUDA array descriptor</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuExternalMemoryGetMappedMipmappedArray(ref CUmipmappedArray mipmap, CUexternalMemory extMem, ref CudaExternalMemoryMipmappedArrayDesc mipmapDesc);

            /// <summary>
            /// Releases all resources associated with an external memory object.<para/>
            /// Frees all buffers and CUDA mipmapped arrays that were mapped onto this external memory object and releases any reference
            /// on the underlying memory itself.
            /// </summary>
            /// <param name="extMem">External memory object to be destroyed</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDestroyExternalMemory(CUexternalMemory extMem);

            /// <summary>
            /// Imports an external semaphore<para/>
            /// Imports an externally allocated synchronization object and returns a handle to that in \p extSem_out.<para/>
            /// The properties of the handle being imported must be described in semHandleDesc.
            /// </summary>
            /// <param name="extSem_out">Returned handle to an external semaphore</param>
            /// <param name="semHandleDesc">Semaphore import handle descriptor</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuImportExternalSemaphore(ref CUexternalSemaphore extSem_out, ref CudaExternalSemaphoreHandleDesc semHandleDesc);

            /// <summary>
            /// Signals a set of external semaphore objects<para/>
            /// Enqueues a signal operation on a set of externally allocated
            /// semaphore object in the specified stream.The operations will be
            /// executed when all prior operations in the stream complete.
            /// <para/>
            /// The exact semantics of signaling a semaphore depends on the type of
            /// the object.
            /// </summary>
            /// <param name="extSemArray">Set of external semaphores to be signaled</param>
            /// <param name="paramsArray">Array of semaphore parameters</param>
            /// <param name="numExtSems">Number of semaphores to signal</param>
            /// <param name="stream">Stream to enqueue the signal operations in</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuSignalExternalSemaphoresAsync" + CUDA_PTSZ)]
            public static extern CUResult cuSignalExternalSemaphoresAsync([In, Out] CUexternalSemaphore[] extSemArray, [In, Out] CudaExternalSemaphoreSignalParams[] paramsArray, uint numExtSems, CUstream stream);

            /// <summary>
            /// Waits on a set of external semaphore objects<para/>
            /// Enqueues a wait operation on a set of externally allocated
            /// semaphore object in the specified stream.The operations will be
            /// executed when all prior operations in the stream complete.<para/>
            /// The exact semantics of waiting on a semaphore depends on the type of the object.
            /// </summary>
            /// <param name="extSemArray">External semaphores to be waited on</param>
            /// <param name="paramsArray">Array of semaphore parameters</param>
            /// <param name="numExtSems">Number of semaphores to wait on</param>
            /// <param name="stream">Stream to enqueue the wait operations in</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuWaitExternalSemaphoresAsync" + CUDA_PTSZ)]
            public static extern CUResult cuWaitExternalSemaphoresAsync([In, Out] CUexternalSemaphore[] extSemArray, [In, Out] CudaExternalSemaphoreWaitParams[] paramsArray, uint numExtSems, CUstream stream);

            /// <summary>
            /// Destroys an external semaphore<para/>
            /// Destroys an external semaphore object and releases any references
            /// to the underlying resource.Any outstanding signals or waits must
            /// have completed before the semaphore is destroyed.
            /// </summary>
            /// <param name="extSem">External semaphore to be destroyed</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDestroyExternalSemaphore(CUexternalSemaphore extSem);
        }

        #endregion

        #region Graph Management

        /// <summary>
        /// 
        /// </summary>
        public static class GraphManagment
        {
#if (NETCOREAPP)
            static GraphManagment()
            {
                DriverAPINativeMethods.Init();
            }
#endif
            /// <summary>
            /// Creates a graph<para/>
            /// Creates an empty graph, which is returned via \p phGraph.
            /// </summary>
            /// <param name="phGraph">Returns newly created graph</param>
            /// <param name="flags">Graph creation flags, must be 0</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphCreate(ref CUgraph phGraph, uint flags);

            /// <summary>
            /// Creates a kernel execution node and adds it to a graph<para/>
            /// Creates a new kernel execution node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p nodeParams.<para/>
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.<para/>
            /// A handle to the new node will be returned in \p phGraphNode.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="nodeParams">Parameters for the GPU execution node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddKernelNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, ref CudaKernelNodeParams nodeParams);

            /// <summary>
            /// Returns a kernel node's parameters<para/>
            /// Returns the parameters of kernel node \p hNode in \p nodeParams.
            /// The \p kernelParams or \p extra array returned in \p nodeParams,
            /// as well as the argument values it points to, are owned by the node.
            /// This memory remains valid until the node is destroyed or its
            /// parameters are modified, and should not be modified
            /// directly. Use ::cuGraphKernelNodeSetParams to update the
            /// parameters of this node.<para/>
            /// The params will contain either \p kernelParams or \p extra,
            /// according to which of these was most recently set on the node.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="nodeParams">Pointer to return the parameters</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphKernelNodeGetParams(CUgraphNode hNode, ref CudaKernelNodeParams nodeParams);

            /// <summary>
            /// Sets a kernel node's parameters<para/>
            /// Sets the parameters of kernel node \p hNode to \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to set the parameters for</param>
            /// <param name="nodeParams">Parameters to copy</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphKernelNodeSetParams(CUgraphNode hNode, ref CudaKernelNodeParams nodeParams);

            /// <summary>
            /// Creates a memcpy node and adds it to a graph<para/>
            /// Creates a new memcpy node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies.<para/>
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.<para/>
            /// When the graph is launched, the node will perform the memcpy described by \p copyParams.
            /// See ::cuMemcpy3D() for a description of the structure and its restrictions.<para/>
            /// Memcpy nodes have some additional restrictions with regards to managed memory, if the
            /// system contains at least one device which has a zero value for the device attribute
            /// ::CU_DEVICE_ATTRIBUTE_CONCURRENT_MANAGED_ACCESS. If one or more of the operands refer
            /// to managed memory, then using the memory type ::CU_MEMORYTYPE_UNIFIED is disallowed
            /// for those operand(s). The managed memory will be treated as residing on either the
            /// host or the device, depending on which memory type is specified.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="copyParams">Parameters for the memory copy</param>
            /// <param name="ctx">Context on which to run the node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddMemcpyNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, ref CUDAMemCpy3D copyParams, CUcontext ctx);

            /// <summary>
            /// Returns a memcpy node's parameters<para/>
            /// Returns the parameters of memcpy node \p hNode in \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="nodeParams">Pointer to return the parameters</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphMemcpyNodeGetParams(CUgraphNode hNode, ref CUDAMemCpy3D nodeParams);

            /// <summary>
            /// Sets a memcpy node's parameters<para/>
            /// Sets the parameters of memcpy node \p hNode to \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to set the parameters for</param>
            /// <param name="nodeParams">Parameters to copy</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphMemcpyNodeSetParams(CUgraphNode hNode, ref CUDAMemCpy3D nodeParams);

            /// <summary>
            /// Creates a memset node and adds it to a graph<para/>
            /// Creates a new memset node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies.<para/>
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.<para/>
            /// The element size must be 1, 2, or 4 bytes.<para/>
            /// When the graph is launched, the node will perform the memset described by \p memsetParams.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="memsetParams">Parameters for the memory set</param>
            /// <param name="ctx">Context on which to run the node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddMemsetNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, ref CudaMemsetNodeParams memsetParams, CUcontext ctx);

            /// <summary>
            /// Returns a memset node's parameters<para/>
            /// Returns the parameters of memset node \p hNode in \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="nodeParams">Pointer to return the parameters</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphMemsetNodeGetParams(CUgraphNode hNode, ref CudaMemsetNodeParams nodeParams);

            /// <summary>
            /// Sets a memset node's parameters<para/>
            /// Sets the parameters of memset node \p hNode to \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to set the parameters for</param>
            /// <param name="nodeParams">Parameters to copy</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphMemsetNodeSetParams(CUgraphNode hNode, ref CudaMemsetNodeParams nodeParams);

            /// <summary>
            /// Creates a host execution node and adds it to a graph<para/>
            /// Creates a new CPU execution node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p nodeParams.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.<para/>
            /// When the graph is launched, the node will invoke the specified CPU function.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="nodeParams">Parameters for the host node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddHostNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, ref CudaHostNodeParams nodeParams);

            /// <summary>
            /// Returns a host node's parameters<para/>
            /// Returns the parameters of host node \p hNode in \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="nodeParams">Pointer to return the parameters</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphHostNodeGetParams(CUgraphNode hNode, ref CudaHostNodeParams nodeParams);

            /// <summary>
            /// Sets a host node's parameters<para/>
            /// Sets the parameters of host node \p hNode to \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to set the parameters for</param>
            /// <param name="nodeParams">Parameters to copy</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphHostNodeSetParams(CUgraphNode hNode, ref CudaHostNodeParams nodeParams);

            /// <summary>
            /// Creates a child graph node and adds it to a graph<para/>
            /// Creates a new node which executes an embedded graph, and adds it to \p hGraph with
            /// \p numDependencies dependencies specified via \p dependencies.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.<para/>
            /// The node executes an embedded child graph. The child graph is cloned in this call.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="childGraph">The graph to clone into this node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddChildGraphNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, CUgraph childGraph);

            /// <summary>
            /// Gets a handle to the embedded graph of a child graph node<para/>
            /// Gets a handle to the embedded graph in a child graph node. This call
            /// does not clone the graph. Changes to the graph will be reflected in
            /// the node, and the node retains ownership of the graph.
            /// Allocation and free nodes cannot be added to the returned graph. Attempting to do so will return an error.<para/>
            /// </summary>
            /// <param name="hNode">Node to get the embedded graph for</param>
            /// <param name="phGraph">Location to store a handle to the graph</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphChildGraphNodeGetGraph(CUgraphNode hNode, ref CUgraph phGraph);

            /// <summary>
            /// Creates an empty node and adds it to a graph<para/>
            /// Creates a new node which performs no operation, and adds it to \p hGraph with
            /// \p numDependencies dependencies specified via \p dependencies.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.<para/>
            /// An empty node performs no operation during execution, but can be used for
            /// transitive ordering. For example, a phased execution graph with 2 groups of n
            /// nodes with a barrier between them can be represented using an empty node and
            /// 2*n dependency edges, rather than no empty node and n^2 dependency edges.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddEmptyNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies);


            /// <summary>
            /// Creates an event record node and adds it to a graph
            /// Creates a new event record node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p params.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.
            /// Each launch of the graph will record \p event to capture execution of the
            /// node's dependencies.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="event_">Event for the node</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddEventRecordNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, CUevent event_);

            /// <summary>
            /// Returns the event associated with an event record node
            /// </summary>
            /// <param name="hNode">Node to get the event for</param>
            /// <param name="event_out">Pointer to return the event</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphEventRecordNodeGetEvent(CUgraphNode hNode, ref CUevent event_out);

            /// <summary>
            /// Sets an event record node's event
            /// </summary>
            /// <param name="hNode">Node to set the event for</param>
            /// <param name="event_">Event to use</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphEventRecordNodeSetEvent(CUgraphNode hNode, CUevent event_);


            /// <summary>
            /// Creates an event wait node and adds it to a graph
            /// Creates a new event wait node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p params.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries.
            /// A handle to the new node will be returned in \p phGraphNode.
            /// The graph node will wait for all work captured in \p event.  See ::cuEventRecord()
            /// for details on what is captured by an event. \p event may be from a different context
            /// or device than the launch stream.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="event_">Event for the node</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddEventWaitNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, CUevent event_);

            /// <summary>
            /// Returns the event associated with an event wait node
            /// </summary>
            /// <param name="hNode">Node to get the event for</param>
            /// <param name="event_out">Pointer to return the event</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphEventWaitNodeGetEvent(CUgraphNode hNode, ref CUevent event_out);

            /// <summary>
            /// Sets an event wait node's event
            /// </summary>
            /// <param name="hNode">Node to set the event for</param>
            /// <param name="event_">Event to use</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphEventWaitNodeSetEvent(CUgraphNode hNode, CUevent event_);


            /// <summary>
            /// Creates an external semaphore signal node and adds it to a graph<para/>
            /// Creates a new external semaphore signal node and adds it to \p hGraph with \p
            /// numDependencies dependencies specified via \p dependencies and arguments specified
            /// in \p nodeParams.It is possible for \p numDependencies to be 0, in which case the
            /// node will be placed at the root of the graph. \p dependencies may not have any
            /// duplicate entries. A handle to the new node will be returned in \p phGraphNode.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="nodeParams">Parameters for the node</param>
            /// <returns></returns>
            public static CUResult cuGraphAddExternalSemaphoresSignalNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, CudaExtSemSignalNodeParams nodeParams)
            {
                IntPtr extSemPtr = IntPtr.Zero;
                IntPtr paramsPtr = IntPtr.Zero;
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    if (nodeParams.extSemArray != null && nodeParams.paramsArray != null)
                    {
                        if (nodeParams.extSemArray.Length != nodeParams.paramsArray.Length)
                        {
                            return CUResult.ErrorInvalidValue;
                        }
                        arraySize = nodeParams.extSemArray.Length;
                    }

                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreSignalParams));

                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    if (arraySize > 0)
                    {
                        extSemPtr = Marshal.AllocHGlobal(arraySize * IntPtr.Size);
                        paramsPtr = Marshal.AllocHGlobal(arraySize * paramsSize);
                    }

                    Marshal.WriteIntPtr(mainPtr + 0, extSemPtr);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, paramsPtr);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    for (int i = 0; i < arraySize; i++)
                    {
                        Marshal.StructureToPtr(nodeParams.extSemArray[i], extSemPtr + (IntPtr.Size * i), false);
                        Marshal.StructureToPtr(nodeParams.paramsArray[i], paramsPtr + (paramsSize * i), false);
                    }

                    retVal = cuGraphAddExternalSemaphoresSignalNodeInternal(ref phGraphNode, hGraph, dependencies, numDependencies, mainPtr);
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                    if (extSemPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(extSemPtr);
                    }
                    if (paramsPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(paramsPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphAddExternalSemaphoresSignalNode")]
            private static extern CUResult cuGraphAddExternalSemaphoresSignalNodeInternal(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, IntPtr nodeParams);

            /// <summary>
            /// Returns an external semaphore signal node's parameters<para/>
            /// Returns the parameters of an external semaphore signal node \p hNode in \p params_out.
            /// The \p extSemArray and \p paramsArray returned in \p params_out,
            /// are owned by the node.This memory remains valid until the node is destroyed or its
            /// parameters are modified, and should not be modified
            /// directly.Use ::cuGraphExternalSemaphoresSignalNodeSetParams to update the parameters of this node.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="params_out">Pointer to return the parameters</param>
            /// <returns></returns>
            public static CUResult cuGraphExternalSemaphoresSignalNodeGetParams(CUgraphNode hNode, CudaExtSemSignalNodeParams params_out)
            {
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreSignalParams));
                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    Marshal.WriteIntPtr(mainPtr + 0, IntPtr.Zero);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, IntPtr.Zero);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    retVal = cuGraphExternalSemaphoresSignalNodeGetParamsInternal(hNode, mainPtr);

                    int length = Marshal.ReadInt32(mainPtr + 2 * IntPtr.Size);

                    CUexternalSemaphore[] array1 = new CUexternalSemaphore[length];
                    CudaExternalSemaphoreSignalParams[] array2 = new CudaExternalSemaphoreSignalParams[length];

                    //Cuda owns these pointers, we won't free them
                    IntPtr ptr1 = Marshal.ReadIntPtr(mainPtr);
                    IntPtr ptr2 = Marshal.ReadIntPtr(mainPtr + IntPtr.Size);

                    for (int i = 0; i < length; i++)
                    {
                        array1[i] = (CUexternalSemaphore)Marshal.PtrToStructure(ptr1 + (IntPtr.Size * i), typeof(CUexternalSemaphore));
                        array2[i] = (CudaExternalSemaphoreSignalParams)Marshal.PtrToStructure(ptr2 + (paramsSize * i), typeof(CudaExternalSemaphoreSignalParams));
                        //array1[i] = Marshal.PtrToStructure<CUexternalSemaphore>(ptr1 + (IntPtr.Size * i));
                        //array2[i] = Marshal.PtrToStructure<CudaExternalSemaphoreSignalParams>(ptr2 + (paramsSize * i));
                    }

                    params_out.extSemArray = array1;
                    params_out.paramsArray = array2;
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphExternalSemaphoresSignalNodeGetParams")]
            private static extern CUResult cuGraphExternalSemaphoresSignalNodeGetParamsInternal(CUgraphNode hNode, IntPtr params_out);

            /// <summary>
            /// Sets an external semaphore signal node's parameters<para/>
            /// Sets the parameters of an external semaphore signal node \p hNode to \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to set the parameters for</param>
            /// <param name="nodeParams">Parameters to copy</param>
            /// <returns></returns>
            public static CUResult cuGraphExternalSemaphoresSignalNodeSetParams(CUgraphNode hNode, CudaExtSemSignalNodeParams nodeParams)
            {
                IntPtr extSemPtr = IntPtr.Zero;
                IntPtr paramsPtr = IntPtr.Zero;
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    if (nodeParams.extSemArray != null && nodeParams.paramsArray != null)
                    {
                        if (nodeParams.extSemArray.Length != nodeParams.paramsArray.Length)
                        {
                            return CUResult.ErrorInvalidValue;
                        }
                        arraySize = nodeParams.extSemArray.Length;
                    }

                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreSignalParams));

                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    if (arraySize > 0)
                    {
                        extSemPtr = Marshal.AllocHGlobal(arraySize * IntPtr.Size);
                        paramsPtr = Marshal.AllocHGlobal(arraySize * paramsSize);
                    }

                    Marshal.WriteIntPtr(mainPtr + 0, extSemPtr);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, paramsPtr);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    for (int i = 0; i < arraySize; i++)
                    {
                        Marshal.StructureToPtr(nodeParams.extSemArray[i], extSemPtr + (IntPtr.Size * i), false);
                        Marshal.StructureToPtr(nodeParams.paramsArray[i], paramsPtr + (paramsSize * i), false);
                    }

                    retVal = cuGraphExternalSemaphoresSignalNodeSetParamsInternal(hNode, mainPtr);
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                    if (extSemPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(extSemPtr);
                    }
                    if (paramsPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(paramsPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphExternalSemaphoresSignalNodeSetParams")]
            private static extern CUResult cuGraphExternalSemaphoresSignalNodeSetParamsInternal(CUgraphNode hNode, IntPtr nodeParams);

            /// <summary>
            /// Creates an external semaphore wait node and adds it to a graph<para/>
            /// Creates a new external semaphore wait node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p nodeParams.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries. A handle
            /// to the new node will be returned in \p phGraphNode.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="nodeParams">Parameters for the node</param>
            /// <returns></returns>
            public static CUResult cuGraphAddExternalSemaphoresWaitNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, CudaExtSemWaitNodeParams nodeParams)
            {
                IntPtr extSemPtr = IntPtr.Zero;
                IntPtr paramsPtr = IntPtr.Zero;
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    if (nodeParams.extSemArray != null && nodeParams.paramsArray != null)
                    {
                        if (nodeParams.extSemArray.Length != nodeParams.paramsArray.Length)
                        {
                            return CUResult.ErrorInvalidValue;
                        }
                        arraySize = nodeParams.extSemArray.Length;
                    }

                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreWaitParams));

                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    if (arraySize > 0)
                    {
                        extSemPtr = Marshal.AllocHGlobal(arraySize * IntPtr.Size);
                        paramsPtr = Marshal.AllocHGlobal(arraySize * paramsSize);
                    }

                    Marshal.WriteIntPtr(mainPtr + 0, extSemPtr);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, paramsPtr);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    for (int i = 0; i < arraySize; i++)
                    {
                        Marshal.StructureToPtr(nodeParams.extSemArray[i], extSemPtr + (IntPtr.Size * i), false);
                        Marshal.StructureToPtr(nodeParams.paramsArray[i], paramsPtr + (paramsSize * i), false);
                    }

                    retVal = cuGraphAddExternalSemaphoresWaitNodeInternal(ref phGraphNode, hGraph, dependencies, numDependencies, mainPtr);
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                    if (extSemPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(extSemPtr);
                    }
                    if (paramsPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(paramsPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphAddExternalSemaphoresWaitNode")]
            private static extern CUResult cuGraphAddExternalSemaphoresWaitNodeInternal(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, IntPtr nodeParams);

            /// <summary>
            /// Returns an external semaphore wait node's parameters<para/>
            /// Returns the parameters of an external semaphore wait node \p hNode in \p params_out.
            /// The \p extSemArray and \p paramsArray returned in \p params_out,
            /// are owned by the node.This memory remains valid until the node is destroyed or its
            /// parameters are modified, and should not be modified
            /// directly.Use ::cuGraphExternalSemaphoresSignalNodeSetParams to update the
            /// parameters of this node.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="params_out">Pointer to return the parameters</param>
            /// <returns></returns>
            public static CUResult cuGraphExternalSemaphoresWaitNodeGetParams(CUgraphNode hNode, CudaExtSemWaitNodeParams params_out)
            {
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreWaitParams));
                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    Marshal.WriteIntPtr(mainPtr + 0, IntPtr.Zero);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, IntPtr.Zero);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    retVal = cuGraphExternalSemaphoresWaitNodeGetParamsInternal(hNode, mainPtr);

                    int length = Marshal.ReadInt32(mainPtr + 2 * IntPtr.Size);

                    CUexternalSemaphore[] array1 = new CUexternalSemaphore[length];
                    CudaExternalSemaphoreWaitParams[] array2 = new CudaExternalSemaphoreWaitParams[length];

                    //Cuda owns these pointers, we won't free them
                    IntPtr ptr1 = Marshal.ReadIntPtr(mainPtr);
                    IntPtr ptr2 = Marshal.ReadIntPtr(mainPtr + IntPtr.Size);

                    for (int i = 0; i < length; i++)
                    {
                        array1[i] = (CUexternalSemaphore)Marshal.PtrToStructure(ptr1 + (IntPtr.Size * i), typeof(CUexternalSemaphore));
                        array2[i] = (CudaExternalSemaphoreWaitParams)Marshal.PtrToStructure(ptr2 + (paramsSize * i), typeof(CudaExternalSemaphoreWaitParams));
                        //array1[i] = Marshal.PtrToStructure<CUexternalSemaphore>(ptr1 + (IntPtr.Size * i));
                        //array2[i] = Marshal.PtrToStructure<CudaExternalSemaphoreWaitParams>(ptr2 + (paramsSize * i));
                    }

                    params_out.extSemArray = array1;
                    params_out.paramsArray = array2;
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphExternalSemaphoresWaitNodeGetParams")]
            private static extern CUResult cuGraphExternalSemaphoresWaitNodeGetParamsInternal(CUgraphNode hNode, IntPtr params_out);

            /// <summary>
            /// Sets an external semaphore wait node's parameters<para/>
            /// Sets the parameters of an external semaphore wait node \p hNode to \p nodeParams.
            /// </summary>
            /// <param name="hNode">Node to set the parameters for</param>
            /// <param name="nodeParams">Parameters to copy</param>
            /// <returns></returns>
            public static CUResult cuGraphExternalSemaphoresWaitNodeSetParams(CUgraphNode hNode, CudaExtSemWaitNodeParams nodeParams)
            {
                IntPtr extSemPtr = IntPtr.Zero;
                IntPtr paramsPtr = IntPtr.Zero;
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    if (nodeParams.extSemArray != null && nodeParams.paramsArray != null)
                    {
                        if (nodeParams.extSemArray.Length != nodeParams.paramsArray.Length)
                        {
                            return CUResult.ErrorInvalidValue;
                        }
                        arraySize = nodeParams.extSemArray.Length;
                    }

                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreWaitParams));

                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    if (arraySize > 0)
                    {
                        extSemPtr = Marshal.AllocHGlobal(arraySize * IntPtr.Size);
                        paramsPtr = Marshal.AllocHGlobal(arraySize * paramsSize);
                    }

                    Marshal.WriteIntPtr(mainPtr + 0, extSemPtr);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, paramsPtr);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    for (int i = 0; i < arraySize; i++)
                    {
                        Marshal.StructureToPtr(nodeParams.extSemArray[i], extSemPtr + (IntPtr.Size * i), false);
                        Marshal.StructureToPtr(nodeParams.paramsArray[i], paramsPtr + (paramsSize * i), false);
                    }

                    retVal = cuGraphExternalSemaphoresWaitNodeSetParamsInternal(hNode, mainPtr);
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                    if (extSemPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(extSemPtr);
                    }
                    if (paramsPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(paramsPtr);
                    }
                }
                return retVal;
            }

            /// <summary/>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphExternalSemaphoresWaitNodeSetParams")]
            private static extern CUResult cuGraphExternalSemaphoresWaitNodeSetParamsInternal(CUgraphNode hNode, IntPtr nodeParams);


            /// <summary>
            /// Creates an allocation node and adds it to a graph<para/>
            /// Creates a new allocation node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p nodeParams.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries. A handle
            /// to the new node will be returned in \p phGraphNode.<para/>
            /// When::cuGraphAddMemAllocNode creates an allocation node, it returns the address of the allocation in
            /// \param nodeParams.dptr.The allocation's address remains fixed across instantiations and launches.<para/>
            /// If the allocation is freed in the same graph, by creating a free node using ::cuGraphAddMemFreeNode,
            /// the allocation can be accessed by nodes ordered after the allocation node but before the free node.
            /// These allocations cannot be freed outside the owning graph, and they can only be freed once in the
            /// owning graph.<para/>
            /// If the allocation is not freed in the same graph, then it can be accessed not only by nodes in the
            /// graph which are ordered after the allocation node, but also by stream operations ordered after the
            /// graph's execution but before the allocation is freed.<para/>
            /// Allocations which are not freed in the same graph can be freed by:<para/>
            /// - passing the allocation to ::cuMemFreeAsync or ::cuMemFree;<para/>
            /// - launching a graph with a free node for that allocation; or<para/>
            /// - specifying::CUDA_GRAPH_INSTANTIATE_FLAG_AUTO_FREE_ON_LAUNCH during instantiation, which makes
            /// each launch behave as though it called::cuMemFreeAsync for every unfreed allocation.<para/>
            /// It is not possible to free an allocation in both the owning graph and another graph.If the allocation
            /// is freed in the same graph, a free node cannot be added to another graph.If the allocation is freed
            /// in another graph, a free node can no longer be added to the owning graph.<para/>
            /// The following restrictions apply to graphs which contain allocation and/or memory free nodes:<para/>
            /// - Nodes and edges of the graph cannot be deleted.<para/>
            /// - The graph cannot be used in a child node.<para/>
            /// - Only one instantiation of the graph may exist at any point in time.<para/>
            /// - The graph cannot be cloned.<para/>
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="nodeParams">Parameters for the node</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddMemAllocNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, ref CUDA_MEM_ALLOC_NODE_PARAMS nodeParams);

            /// <summary>
            /// Returns a memory alloc node's parameters<para/>
            /// Returns the parameters of a memory alloc node \p hNode in \p params_out.
            /// The \p poolProps and \p accessDescs returned in \p params_out, are owned by the
            /// node.This memory remains valid until the node is destroyed.The returned
            /// parameters must not be modified.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="params_out">Pointer to return the parameters</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphMemAllocNodeGetParams(CUgraphNode hNode, ref CUDA_MEM_ALLOC_NODE_PARAMS params_out);

            /// <summary>
            /// Creates a memory free node and adds it to a graph<para/>
            /// Creates a new memory free node and adds it to \p hGraph with \p numDependencies
            /// dependencies specified via \p dependencies and arguments specified in \p nodeParams.
            /// It is possible for \p numDependencies to be 0, in which case the node will be placed
            /// at the root of the graph. \p dependencies may not have any duplicate entries. A handle
            /// to the new node will be returned in \p phGraphNode.<para/>
            /// ::cuGraphAddMemFreeNode will return ::CUDA_ERROR_INVALID_VALUE if the user attempts to free:<para/>
            /// - an allocation twice in the same graph.<para/>
            /// - an address that was not returned by an allocation node.<para/>
            /// - an invalid address.<para/>
            /// The following restrictions apply to graphs which contain allocation and/or memory free nodes:<para/>
            /// - Nodes and edges of the graph cannot be deleted.<para/>
            /// - The graph cannot be used in a child node.<para/>
            /// - Only one instantiation of the graph may exist at any point in time.<para/>
            /// - The graph cannot be cloned.
            /// </summary>
            /// <param name="phGraphNode">Returns newly created node</param>
            /// <param name="hGraph">Graph to which to add the node</param>
            /// <param name="dependencies">Dependencies of the node</param>
            /// <param name="numDependencies">Number of dependencies</param>
            /// <param name="dptr">Address of memory to free</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddMemFreeNode(ref CUgraphNode phGraphNode, CUgraph hGraph, CUgraphNode[] dependencies, SizeT numDependencies, CUdeviceptr dptr);

            /// <summary>
            /// Returns a memory free node's parameters<para/>
            /// Returns the address of a memory free node \p hNode in \p dptr_out.
            /// </summary>
            /// <param name="hNode">Node to get the parameters for</param>
            /// <param name="dptr_out">Pointer to return the device address</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphMemFreeNodeGetParams(CUgraphNode hNode, ref CUdeviceptr dptr_out);

            /// <summary>
            /// Free unused memory that was cached on the specified device for use with graphs back to the OS.<para/>
            /// Blocks which are not in use by a graph that is either currently executing or scheduled to execute are freed back to the operating system.
            /// </summary>
            /// <param name="device">The device for which cached memory should be freed.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGraphMemTrim(CUdevice device);

            /// <summary>
            /// Query asynchronous allocation attributes related to graphs<para/>
            /// Valid attributes are:<para/>
            /// - ::CU_GRAPH_MEM_ATTR_USED_MEM_CURRENT: Amount of memory, in bytes, currently associated with graphs<para/>
            /// - ::CU_GRAPH_MEM_ATTR_USED_MEM_HIGH: High watermark of memory, in bytes, associated with graphs since the last time it was reset.High watermark can only be reset to zero.<para/>
            /// - ::CU_GRAPH_MEM_ATTR_RESERVED_MEM_CURRENT: Amount of memory, in bytes, currently allocated for use by the CUDA graphs asynchronous allocator.<para/>
            /// - ::CU_GRAPH_MEM_ATTR_RESERVED_MEM_HIGH: High watermark of memory, in bytes, currently allocated for use by the CUDA graphs asynchronous allocator.
            /// </summary>
            /// <param name="device">Specifies the scope of the query</param>
            /// <param name="attr">attribute to get</param>
            /// <param name="value">retrieved value</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceGetGraphMemAttribute(CUdevice device, CUgraphMem_attribute attr, ref ulong value);

            /// <summary>
            /// Set asynchronous allocation attributes related to graphs<para/>
            /// Valid attributes are:<para/>
            /// - ::CU_GRAPH_MEM_ATTR_USED_MEM_HIGH: High watermark of memory, in bytes, associated with graphs since the last time it was reset.High watermark can only be reset to zero.<para/>
            /// - ::CU_GRAPH_MEM_ATTR_RESERVED_MEM_HIGH: High watermark of memory, in bytes, currently allocated for use by the CUDA graphs asynchronous allocator.
            /// </summary>
            /// <param name="device">Specifies the scope of the query</param>
            /// <param name="attr">attribute to get</param>
            /// <param name="value">pointer to value to set</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuDeviceSetGraphMemAttribute(CUdevice device, CUgraphMem_attribute attr, ref ulong value);


            /// <summary>
            /// Clones a graph<para/>
            /// This function creates a copy of \p originalGraph and returns it in \p * phGraphClone.
            /// All parameters are copied into the cloned graph. The original graph may be modified
            /// after this call without affecting the clone.<para/>
            /// Child graph nodes in the original graph are recursively copied into the clone.
            /// </summary>
            /// <param name="phGraphClone">Returns newly created cloned graph</param>
            /// <param name="originalGraph">Graph to clone</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphClone(ref CUgraph phGraphClone, CUgraph originalGraph);

            /// <summary>
            /// Finds a cloned version of a node<para/>
            /// This function returns the node in \p hClonedGraph corresponding to \p hOriginalNode
            /// in the original graph.<para/>
            /// \p hClonedGraph must have been cloned from \p hOriginalGraph via ::cuGraphClone.
            /// \p hOriginalNode must have been in \p hOriginalGraph at the time of the call to
            /// ::cuGraphClone, and the corresponding cloned node in \p hClonedGraph must not have
            /// been removed. The cloned node is then returned via \p phClonedNode.
            /// </summary>
            /// <param name="phNode">Returns handle to the cloned node</param>
            /// <param name="hOriginalNode">Handle to the original node</param>
            /// <param name="hClonedGraph">Cloned graph to query</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphNodeFindInClone(ref CUgraphNode phNode, CUgraphNode hOriginalNode, CUgraph hClonedGraph);

            /// <summary>
            /// Returns a node's type<para/>
            /// Returns the node type of \p hNode in \p type.
            /// </summary>
            /// <param name="hNode">Node to query</param>
            /// <param name="type">Pointer to return the node type</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphNodeGetType(CUgraphNode hNode, ref CUgraphNodeType type);

            /// <summary>
            /// Returns a graph's nodes<para/>
            /// Returns a list of \p hGraph's nodes. \p nodes may be NULL, in which case this
            /// function will return the number of nodes in \p numNodes. Otherwise,
            /// \p numNodes entries will be filled in. If \p numNodes is higher than the actual
            /// number of nodes, the remaining entries in \p nodes will be set to NULL, and the
            /// number of nodes actually obtained will be returned in \p numNodes.
            /// </summary>
            /// <param name="hGraph">Graph to query</param>
            /// <param name="nodes">Pointer to return the nodes</param>
            /// <param name="numNodes">See description</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphGetNodes(CUgraph hGraph, [In, Out] CUgraphNode[] nodes, ref SizeT numNodes);

            /// <summary>
            /// Returns a graph's root nodes<para/>
            /// Returns a list of \p hGraph's root nodes. \p rootNodes may be NULL, in which case this
            /// function will return the number of root nodes in \p numRootNodes. Otherwise,
            /// \p numRootNodes entries will be filled in. If \p numRootNodes is higher than the actual
            /// number of root nodes, the remaining entries in \p rootNodes will be set to NULL, and the
            /// number of nodes actually obtained will be returned in \p numRootNodes.
            /// </summary>
            /// <param name="hGraph">Graph to query</param>
            /// <param name="rootNodes">Pointer to return the root nodes</param>
            /// <param name="numRootNodes">See description</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphGetRootNodes(CUgraph hGraph, [In, Out] CUgraphNode[] rootNodes, ref SizeT numRootNodes);

            /// <summary>
            /// Returns a graph's dependency edges<para/>
            /// Returns a list of \p hGraph's dependency edges. Edges are returned via corresponding
            /// indices in \p from and \p to; that is, the node in \p to[i] has a dependency on the
            /// node in \p from[i]. \p from and \p to may both be NULL, in which
            /// case this function only returns the number of edges in \p numEdges. Otherwise,
            /// \p numEdges entries will be filled in. If \p numEdges is higher than the actual
            /// number of edges, the remaining entries in \p from and \p to will be set to NULL, and
            /// the number of edges actually returned will be written to \p numEdges.
            /// </summary>
            /// <param name="hGraph">Graph to get the edges from</param>
            /// <param name="from">Location to return edge endpoints</param>
            /// <param name="to">Location to return edge endpoints</param>
            /// <param name="numEdges">See description</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphGetEdges(CUgraph hGraph, [In, Out] CUgraphNode[] from, [In, Out] CUgraphNode[] to, ref SizeT numEdges);

            /// <summary>
            /// Returns a node's dependencies<para/>
            /// Returns a list of \p node's dependencies. \p dependencies may be NULL, in which case this
            /// function will return the number of dependencies in \p numDependencies. Otherwise,
            /// \p numDependencies entries will be filled in. If \p numDependencies is higher than the actual
            /// number of dependencies, the remaining entries in \p dependencies will be set to NULL, and the
            /// number of nodes actually obtained will be returned in \p numDependencies.
            /// </summary>
            /// <param name="hNode">Node to query</param>
            /// <param name="dependencies">Pointer to return the dependencies</param>
            /// <param name="numDependencies">See description</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphNodeGetDependencies(CUgraphNode hNode, [In, Out] CUgraphNode[] dependencies, ref SizeT numDependencies);

            /// <summary>
            /// Returns a node's dependent nodes<para/>
            /// Returns a list of \p node's dependent nodes. \p dependentNodes may be NULL, in which
            /// case this function will return the number of dependent nodes in \p numDependentNodes.
            /// Otherwise, \p numDependentNodes entries will be filled in. If \p numDependentNodes is
            /// higher than the actual number of dependent nodes, the remaining entries in
            /// \p dependentNodes will be set to NULL, and the number of nodes actually obtained will
            /// be returned in \p numDependentNodes.
            /// </summary>
            /// <param name="hNode">Node to query</param>
            /// <param name="dependentNodes">Pointer to return the dependent nodes</param>
            /// <param name="numDependentNodes">See description</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphNodeGetDependentNodes(CUgraphNode hNode, [In, Out] CUgraphNode[] dependentNodes, ref SizeT numDependentNodes);

            /// <summary>
            /// Adds dependency edges to a graph<para/>
            /// The number of dependencies to be added is defined by \p numDependencies
            /// Elements in \p from and \p to at corresponding indices define a dependency.
            /// Each node in \p from and \p to must belong to \p hGraph.<para/>
            /// If \p numDependencies is 0, elements in \p from and \p to will be ignored.
            /// Specifying an existing dependency will return an error.
            /// </summary>
            /// <param name="hGraph">Graph to which dependencies are added</param>
            /// <param name="from">Array of nodes that provide the dependencies</param>
            /// <param name="to">Array of dependent nodes</param>
            /// <param name="numDependencies">Number of dependencies to be added</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphAddDependencies(CUgraph hGraph, CUgraphNode[] from, CUgraphNode[] to, SizeT numDependencies);

            /// <summary>
            /// Removes dependency edges from a graph<para/>
            /// The number of \p dependencies to be removed is defined by \p numDependencies.<para/>
            /// Elements in \p from and \p to at corresponding indices define a dependency.<para/>
            /// Each node in \p from and \p to must belong to \p hGraph.<para/>
            /// If \p numDependencies is 0, elements in \p from and \p to will be ignored.<para/>
            /// Specifying a non-existing dependency will return an error.<para/>
            /// Dependencies cannot be removed from graphs which contain allocation or free nodes. Any attempt to do so will return an error.
            /// </summary>
            /// <param name="hGraph">Graph from which to remove dependencies</param>
            /// <param name="from">Array of nodes that provide the dependencies</param>
            /// <param name="to">Array of dependent nodes</param>
            /// <param name="numDependencies">Number of dependencies to be removed</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphRemoveDependencies(CUgraph hGraph, CUgraphNode[] from, CUgraphNode[] to, SizeT numDependencies);

            /// <summary>
            /// Remove a node from the graph<para/>
            /// Removes \p hNode from its graph. This operation also severs any dependencies of other nodes on \p hNode and vice versa.<para/>
            /// Nodes which belong to a graph which contains allocation or free nodes cannot be destroyed. Any attempt to do so will return an error.
            /// </summary>
            /// <param name="hNode">Node to remove</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphDestroyNode(CUgraphNode hNode);


            /// <summary>
            /// Creates an executable graph from a graph<para/>
            /// Instantiates \p hGraph as an executable graph. The graph is validated for any
            /// structural constraints or intra-node constraints which were not previously
            /// validated.If instantiation is successful, a handle to the instantiated graph
            /// is returned in \p graphExec.<para/>
            /// If there are any errors, diagnostic information may be returned in \p errorNode and
            /// \p logBuffer.This is the primary way to inspect instantiation errors.The output
            /// will be null terminated unless the diagnostics overflow 
            /// the buffer. In this case, they will be truncated, and the last byte can be
            /// inspected to determine if truncation occurred.
            /// </summary>
            /// <param name="phGraphExec">Returns instantiated graph</param>
            /// <param name="hGraph">Graph to instantiate</param>
            /// <param name="phErrorNode">In case of an instantiation error, this may be modified to indicate a node contributing to the error</param>
            /// <param name="logBuffer">A character buffer to store diagnostic messages</param>
            /// <param name="bufferSize">Size of the log buffer in bytes</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphInstantiate_v2")]
            public static extern CUResult cuGraphInstantiate(ref CUgraphExec phGraphExec, CUgraph hGraph, ref CUgraphNode phErrorNode, [In, Out] byte[] logBuffer, SizeT bufferSize);

            /// <summary>
            /// Creates an executable graph from a graph<para/>
            /// Instantiates \p hGraph as an executable graph. The graph is validated for any
            /// structural constraints or intra-node constraints which were not previously
            /// validated.If instantiation is successful, a handle to the instantiated graph
            /// is returned in \p phGraphExec.<para/>
            /// The \p flags parameter controls the behavior of instantiation and subsequent graph launches.Valid flags are: <para/>
            /// - ::CUDA_GRAPH_INSTANTIATE_FLAG_AUTO_FREE_ON_LAUNCH, which configures a graph containing memory allocation nodes to automatically free any
            /// unfreed memory allocations before the graph is relaunched.
            /// <para/>If \p hGraph contains any allocation or free nodes, there can be at most one
            /// executable graph in existence for that graph at a time.<para/>
            /// An attempt to instantiate a second executable graph before destroying the first
            /// with ::cuGraphExecDestroy will result in an error.
            /// </summary>
            /// <param name="phGraphExec">Returns instantiated graph</param>
            /// <param name="hGraph">Graph to instantiate</param>
            /// <param name="flags">Flags to control instantiation.  See ::CUgraphInstantiate_flags.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphInstantiateWithFlags(ref CUgraphExec phGraphExec, CUgraph hGraph, CUgraphInstantiate_flags flags);


            /// <summary>
            /// Sets the parameters for a kernel node in the given graphExec<para/>
            /// Sets the parameters of a kernel node in an executable graph \p hGraphExec.
            /// The node is identified by the corresponding node \p hNode in the
            /// non-executable graph, from which the executable graph was instantiated.<para/>
            /// \p hNode must not have been removed from the original graph.The \p func field
            /// of \p nodeParams cannot be modified and must match the original value.
            /// All other values can be modified.<para/>
            /// The modifications only affect future launches of \p hGraphExec. Already
            /// enqueued or running launches of \p hGraphExec are not affected by this call.
            /// \p hNode is also not modified by this call.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">kernel node from the graph from which graphExec was instantiated</param>
            /// <param name="nodeParams">Updated Parameters to set</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecKernelNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, ref CudaKernelNodeParams nodeParams);


            /// <summary>
            /// Sets the parameters for a memcpy node in the given graphExec.<para/>
            /// Updates the work represented by \p hNode in \p hGraphExec as though \p hNode had 
            /// contained \p copyParams at instantiation.  hNode must remain in the graph which was 
            /// used to instantiate \p hGraphExec.  Changed edges to and from hNode are ignored.<para/>
            /// The source and destination memory in \p copyParams must be allocated from the same 
            /// contexts as the original source and destination memory.  Both the instantiation-time 
            /// memory operands and the memory operands in \p copyParams must be 1-dimensional.
            /// Zero-length operations are not supported.<para/>
            /// The modifications only affect future launches of \p hGraphExec.  Already enqueued 
            /// or running launches of \p hGraphExec are not affected by this call.  hNode is also 
            /// not modified by this call.<para/>
            /// Returns CUDA_ERROR_INVALID_VALUE if the memory operands' mappings changed or
            /// either the original or new memory operands are multidimensional.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">Memcpy node from the graph which was used to instantiate graphExec</param>
            /// <param name="copyParams">The updated parameters to set</param>
            /// <param name="ctx">Context on which to run the node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecMemcpyNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, ref CUDAMemCpy3D copyParams, CUcontext ctx);


            /// <summary>
            /// Sets the parameters for a memset node in the given graphExec.<para/>
            /// Updates the work represented by \p hNode in \p hGraphExec as though \p hNode had 
            /// contained \p memsetParams at instantiation.  hNode must remain in the graph which was 
            /// used to instantiate \p hGraphExec.  Changed edges to and from hNode are ignored.<para/>
            /// The destination memory in \p memsetParams must be allocated from the same 
            /// contexts as the original destination memory.  Both the instantiation-time 
            /// memory operand and the memory operand in \p memsetParams must be 1-dimensional.
            /// Zero-length operations are not supported.<para/>
            /// The modifications only affect future launches of \p hGraphExec.  Already enqueued 
            /// or running launches of \p hGraphExec are not affected by this call.  hNode is also 
            /// not modified by this call.<para/>
            /// Returns CUDA_ERROR_INVALID_VALUE if the memory operand's mappings changed or
            /// either the original or new memory operand are multidimensional.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">Memset node from the graph which was used to instantiate graphExec</param>
            /// <param name="memsetParams">The updated parameters to set</param>
            /// <param name="ctx">Context on which to run the node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecMemsetNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, ref CudaMemsetNodeParams memsetParams, CUcontext ctx);


            /// <summary>
            /// Sets the parameters for a host node in the given graphExec.<para/>
            /// Updates the work represented by \p hNode in \p hGraphExec as though \p hNode had 
            /// contained \p nodeParams at instantiation.  hNode must remain in the graph which was 
            /// used to instantiate \p hGraphExec.  Changed edges to and from hNode are ignored.<para/>
            /// The modifications only affect future launches of \p hGraphExec.  Already enqueued 
            /// or running launches of \p hGraphExec are not affected by this call.  hNode is also 
            /// not modified by this call.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">Host node from the graph which was used to instantiate graphExec</param>
            /// <param name="nodeParams">The updated parameters to set</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecHostNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, ref CudaHostNodeParams nodeParams);



            /// <summary>
            /// Updates node parameters in the child graph node in the given graphExec.
            /// Updates the work represented by \p hNode in \p hGraphExec as though the nodes contained
            /// in \p hNode's graph had the parameters contained in \p childGraph's nodes at instantiation.
            /// \p hNode must remain in the graph which was used to instantiate \p hGraphExec.
            /// Changed edges to and from \p hNode are ignored.
            /// The modifications only affect future launches of \p hGraphExec.  Already enqueued 
            /// or running launches of \p hGraphExec are not affected by this call.  \p hNode is also
            /// not modified by this call.
            /// The topology of \p childGraph, as well as the node insertion order, must match that
            /// of the graph contained in \p hNode.  See::cuGraphExecUpdate() for a list of restrictions
            /// on what can be updated in an instantiated graph.The update is recursive, so child graph
            /// nodes contained within the top level child graph will also be updated.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">Host node from the graph which was used to instantiate graphExec</param>
            /// <param name="childGraph">The graph supplying the updated parameters</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecChildGraphNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, CUgraph childGraph);

            /// <summary>
            /// Sets the event for an event record node in the given graphExec
            /// Sets the event of an event record node in an executable graph \p hGraphExec.
            /// The node is identified by the corresponding node \p hNode in the
            /// non-executable graph, from which the executable graph was instantiated.
            /// The modifications only affect future launches of \p hGraphExec. Already
            /// enqueued or running launches of \p hGraphExec are not affected by this call.
            /// \p hNode is also not modified by this call.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">event record node from the graph from which graphExec was instantiated</param>
            /// <param name="event_">Updated event to use</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecEventRecordNodeSetEvent(CUgraphExec hGraphExec, CUgraphNode hNode, CUevent event_);


            /// <summary>
            /// Sets the event for an event record node in the given graphExec
            /// Sets the event of an event record node in an executable graph \p hGraphExec.
            /// The node is identified by the corresponding node \p hNode in the
            /// non-executable graph, from which the executable graph was instantiated.
            /// The modifications only affect future launches of \p hGraphExec. Already
            /// enqueued or running launches of \p hGraphExec are not affected by this call.
            /// \p hNode is also not modified by this call.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">event wait node from the graph from which graphExec was instantiated</param>
            /// <param name="event_">Updated event to use</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecEventWaitNodeSetEvent(CUgraphExec hGraphExec, CUgraphNode hNode, CUevent event_);

            /// <summary>
            /// Sets the parameters for an external semaphore signal node in the given graphExec<para/>
            /// Sets the parameters of an external semaphore signal node in an executable graph \p hGraphExec.
            /// The node is identified by the corresponding node \p hNode in the
            /// non-executable graph, from which the executable graph was instantiated.<para/>
            /// hNode must not have been removed from the original graph.<para/>
            /// The modifications only affect future launches of \p hGraphExec. Already
            /// enqueued or running launches of \p hGraphExec are not affected by this call.
            /// hNode is also not modified by this call.<para/>
            /// Changing \p nodeParams->numExtSems is not supported.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">semaphore signal node from the graph from which graphExec was instantiated</param>
            /// <param name="nodeParams">Updated Parameters to set</param>
            /// <returns></returns>
            public static CUResult cuGraphExecExternalSemaphoresSignalNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, CudaExtSemSignalNodeParams nodeParams)
            {
                IntPtr extSemPtr = IntPtr.Zero;
                IntPtr paramsPtr = IntPtr.Zero;
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    if (nodeParams.extSemArray != null && nodeParams.paramsArray != null)
                    {
                        if (nodeParams.extSemArray.Length != nodeParams.paramsArray.Length)
                        {
                            return CUResult.ErrorInvalidValue;
                        }
                        arraySize = nodeParams.extSemArray.Length;
                    }

                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreSignalParams));

                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    if (arraySize > 0)
                    {
                        extSemPtr = Marshal.AllocHGlobal(arraySize * IntPtr.Size);
                        paramsPtr = Marshal.AllocHGlobal(arraySize * paramsSize);
                    }

                    Marshal.WriteIntPtr(mainPtr + 0, extSemPtr);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, paramsPtr);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    for (int i = 0; i < arraySize; i++)
                    {
                        Marshal.StructureToPtr(nodeParams.extSemArray[i], extSemPtr + (IntPtr.Size * i), false);
                        Marshal.StructureToPtr(nodeParams.paramsArray[i], paramsPtr + (paramsSize * i), false);
                    }

                    retVal = cuGraphExecExternalSemaphoresSignalNodeSetParamsInternal(hGraphExec, hNode, mainPtr);
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                    if (extSemPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(extSemPtr);
                    }
                    if (paramsPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(paramsPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphExecExternalSemaphoresSignalNodeSetParams")]
            private static extern CUResult cuGraphExecExternalSemaphoresSignalNodeSetParamsInternal(CUgraphExec hGraphExec, CUgraphNode hNode, IntPtr nodeParams);

            /// <summary>
            /// Sets the parameters for an external semaphore wait node in the given graphExec<para/>
            /// Sets the parameters of an external semaphore wait node in an executable graph \p hGraphExec.<para/>
            /// The node is identified by the corresponding node \p hNode in the
            /// non-executable graph, from which the executable graph was instantiated.<para/>
            /// hNode must not have been removed from the original graph.<para/>
            /// The modifications only affect future launches of \p hGraphExec. Already
            /// enqueued or running launches of \p hGraphExec are not affected by this call.
            /// hNode is also not modified by this call.<para/>
            /// Changing \p nodeParams->numExtSems is not supported.
            /// </summary>
            /// <param name="hGraphExec">The executable graph in which to set the specified node</param>
            /// <param name="hNode">semaphore wait node from the graph from which graphExec was instantiated</param>
            /// <param name="nodeParams">Updated Parameters to set</param>
            /// <returns></returns>
            public static CUResult cuGraphExecExternalSemaphoresWaitNodeSetParams(CUgraphExec hGraphExec, CUgraphNode hNode, CudaExtSemWaitNodeParams nodeParams)
            {
                IntPtr extSemPtr = IntPtr.Zero;
                IntPtr paramsPtr = IntPtr.Zero;
                IntPtr mainPtr = IntPtr.Zero;

                CUResult retVal = CUResult.ErrorInvalidValue;

                try
                {
                    int arraySize = 0;
                    if (nodeParams.extSemArray != null && nodeParams.paramsArray != null)
                    {
                        if (nodeParams.extSemArray.Length != nodeParams.paramsArray.Length)
                        {
                            return CUResult.ErrorInvalidValue;
                        }
                        arraySize = nodeParams.extSemArray.Length;
                    }

                    int paramsSize = Marshal.SizeOf(typeof(CudaExternalSemaphoreWaitParams));

                    mainPtr = Marshal.AllocHGlobal(2 * IntPtr.Size + sizeof(int));

                    if (arraySize > 0)
                    {
                        extSemPtr = Marshal.AllocHGlobal(arraySize * IntPtr.Size);
                        paramsPtr = Marshal.AllocHGlobal(arraySize * paramsSize);
                    }

                    Marshal.WriteIntPtr(mainPtr + 0, extSemPtr);
                    Marshal.WriteIntPtr(mainPtr + IntPtr.Size, paramsPtr);
                    Marshal.WriteInt32(mainPtr + 2 * IntPtr.Size, arraySize);

                    for (int i = 0; i < arraySize; i++)
                    {
                        Marshal.StructureToPtr(nodeParams.extSemArray[i], extSemPtr + (IntPtr.Size * i), false);
                        Marshal.StructureToPtr(nodeParams.paramsArray[i], paramsPtr + (paramsSize * i), false);
                    }

                    retVal = cuGraphExecExternalSemaphoresWaitNodeSetParamsInternal(hGraphExec, hNode, mainPtr);
                }
                catch
                {
                    retVal = CUResult.ErrorInvalidValue;
                }
                finally
                {
                    if (mainPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(mainPtr);
                    }
                    if (extSemPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(extSemPtr);
                    }
                    if (paramsPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(paramsPtr);
                    }
                }
                return retVal;
            }

            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphExecExternalSemaphoresWaitNodeSetParams")]
            private static extern CUResult cuGraphExecExternalSemaphoresWaitNodeSetParamsInternal(CUgraphExec hGraphExec, CUgraphNode hNode, IntPtr nodeParams);


            /// <summary>
            /// Uploads an executable graph in a stream
            /// Uploads \p hGraphExec to the device in \p hStream without executing it.Uploads of
            /// the same \p hGraphExec will be serialized.Each upload is ordered behind both any
            /// previous work in \p hStream and any previous launches of \p hGraphExec.
            /// </summary>
            /// <param name="hGraphExec">Executable graph to upload</param>
            /// <param name="hStream">Stream in which to upload the graph</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphUpload" + CUDA_PTSZ)]
            public static extern CUResult cuGraphUpload(CUgraphExec hGraphExec, CUstream hStream);


            /// <summary>
            /// Launches an executable graph in a stream<para/>
            /// Executes \p hGraphExec in \p hStream. Only one instance of \p hGraphExec may be executing
            /// at a time.Each launch is ordered behind both any previous work in \p hStream
            /// and any previous launches of \p hGraphExec.To execute a graph concurrently, it must be
            /// instantiated multiple times into multiple executable graphs.
            /// </summary>
            /// <param name="hGraphExec">Executable graph to launch</param>
            /// <param name="hStream">Stream in which to launch the graph</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME, EntryPoint = "cuGraphLaunch" + CUDA_PTSZ)]
            public static extern CUResult cuGraphLaunch(CUgraphExec hGraphExec, CUstream hStream);


            /// <summary>
            /// Destroys an executable graph<para/>
            /// Destroys the executable graph specified by \p hGraphExec, as well
            /// as all of its executable nodes.If the executable graph is
            /// in-flight, it will not be terminated, but rather freed
            /// asynchronously on completion.
            /// </summary>
            /// <param name="hGraphExec">Executable graph to destroy</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecDestroy(CUgraphExec hGraphExec);

            /// <summary>
            /// Destroys a graph<para/>
            /// Destroys the graph specified by \p hGraph, as well as all of its nodes.
            /// </summary>
            /// <param name="hGraph">Graph to destroy</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphDestroy(CUgraph hGraph);


            /// <summary>
            /// Check whether an executable graph can be updated with a graph and perform the update if possible<para/>
            /// Updates the node parameters in the instantiated graph specified by \p hGraphExec with the
            /// node parameters in a topologically identical graph specified by \p hGraph.<para/>
            /// Limitations:<para/>
            /// - Kernel nodes:<para/>
            ///   - The function must not change (same restriction as cuGraphExecKernelNodeSetParams())<para/>
            /// - Memset and memcpy nodes:<para/>
            ///   - The CUDA device(s) to which the operand(s) was allocated/mapped cannot change.<para/>
            ///   - The source/destination memory must be allocated from the same contexts as the original<para/>
            ///     source/destination memory.<para/>
            ///   - Only 1D memsets can be changed.<para/>
            /// - Additional memcpy node restrictions:<para/>
            ///   - Changing either the source or destination memory type(i.e. CU_MEMORYTYPE_DEVICE,
            ///     CU_MEMORYTYPE_ARRAY, etc.) is not supported.<para/>
            /// Note:  The API may add further restrictions in future releases.  The return code should always be checked.<para/>
            /// Some node types are not currently supported:<para/>
            /// - Empty graph nodes(CU_GRAPH_NODE_TYPE_EMPTY)<para/>
            /// - Child graphs(CU_GRAPH_NODE_TYPE_GRAPH).<para/>
            /// cuGraphExecUpdate sets \p updateResult_out to CU_GRAPH_EXEC_UPDATE_ERROR_TOPOLOGY_CHANGED under
            /// the following conditions:<para/>
            /// - The count of nodes directly in \p hGraphExec and \p hGraph differ, in which case \p hErrorNode_out
            ///   is NULL.<para/>
            /// - A node is deleted in \p hGraph but not not its pair from \p hGraphExec, in which case \p hErrorNode_out
            ///   is NULL.<para/>
            /// - A node is deleted in \p hGraphExec but not its pair from \p hGraph, in which case \p hErrorNode_out is
            ///   the pairless node from \p hGraph.<para/>
            /// - The dependent nodes of a pair differ, in which case \p hErrorNode_out is the node from \p hGraph.<para/>
            /// cuGraphExecUpdate sets \p updateResult_out to:<para/>
            /// - CU_GRAPH_EXEC_UPDATE_ERROR if passed an invalid value.<para/>
            /// - CU_GRAPH_EXEC_UPDATE_ERROR_TOPOLOGY_CHANGED if the graph topology changed<para/>
            /// - CU_GRAPH_EXEC_UPDATE_ERROR_NODE_TYPE_CHANGED if the type of a node changed, in which case
            ///   \p hErrorNode_out is set to the node from \p hGraph.<para/>
            /// - CU_GRAPH_EXEC_UPDATE_ERROR_FUNCTION_CHANGED if the func field of a kernel changed, in which
            ///   case \p hErrorNode_out is set to the node from \p hGraph<para/>
            /// - CU_GRAPH_EXEC_UPDATE_ERROR_PARAMETERS_CHANGED if any parameters to a node changed in a way 
            ///   that is not supported, in which case \p hErrorNode_out is set to the node from \p hGraph.<para/>
            /// - CU_GRAPH_EXEC_UPDATE_ERROR_NOT_SUPPORTED if something about a node is unsupported, like 
            ///   the nodeâ€™s type or configuration, in which case \p hErrorNode_out is set to the node from \p hGraph<para/>
            /// If \p updateResult_out isnâ€™t set in one of the situations described above, the update check passes
            /// and cuGraphExecUpdate updates \p hGraphExec to match the contents of \p hGraph.  If an error happens
            /// during the update, \p updateResult_out will be set to CU_GRAPH_EXEC_UPDATE_ERROR; otherwise,
            /// \p updateResult_out is set to CU_GRAPH_EXEC_UPDATE_SUCCESS.<para/>
            /// cuGraphExecUpdate returns CUDA_SUCCESS when the updated was performed successfully.  It returns
            /// CUDA_ERROR_GRAPH_EXEC_UPDATE_FAILURE if the graph update was not performed because it included 
            /// changes which violated constraints specific to instantiated graph update.
            /// </summary>
            /// <param name="hGraphExec">The instantiated graph to be updated</param>
            /// <param name="hGraph">The graph containing the updated parameters</param>
            /// <param name="hErrorNode_out">The node which caused the permissibility check to forbid the update, if any</param>
            /// <param name="updateResult_out">Whether the graph update was permitted.  If was forbidden, the reason why</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphExecUpdate(CUgraphExec hGraphExec, CUgraph hGraph, ref CUgraphNode hErrorNode_out, ref CUgraphExecUpdateResult updateResult_out);


            /// <summary>
            /// Copies attributes from source node to destination node.<para/>
            /// Copies attributes from source node \p src to destination node \p dst. Both node must have the same context.
            /// </summary>
            /// <param name="dst">Destination node</param>
            /// <param name="src">Source node</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphKernelNodeCopyAttributes(CUgraphNode dst, CUgraphNode src);

            /// <summary>
            /// Queries node attribute.<para/>
            /// Queries attribute \p attr from node \p hNode and stores it in corresponding member of \p value_out.
            /// </summary>
            /// <param name="hNode"></param>
            /// <param name="attr"></param>
            /// <param name="value_out"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphKernelNodeGetAttribute(CUgraphNode hNode, CUkernelNodeAttrID attr, ref CUkernelNodeAttrValue value_out);

            /// <summary>
            /// Sets node attribute.<para/>
            /// Sets attribute \p attr on node \p hNode from corresponding attribute of value.
            /// </summary>
            /// <param name="hNode"></param>
            /// <param name="attr"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphKernelNodeSetAttribute(CUgraphNode hNode, CUkernelNodeAttrID attr, ref CUkernelNodeAttrValue value);

            /// <summary>
            /// Write a DOT file describing graph structure<para/>
            /// Using the provided \p hGraph, write to \p path a DOT formatted description of the graph.
            /// By default this includes the graph topology, node types, node id, kernel names and memcpy direction.
            /// \p flags can be specified to write more detailed information about each node type such as
            /// parameter values, kernel attributes, node and function handles.
            /// </summary>
            /// <param name="hGraph">The graph to create a DOT file from</param>
            /// <param name="path">The path to write the DOT file to</param>
            /// <param name="flags">Flags from CUgraphDebugDot_flags for specifying which additional node information to write</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphDebugDotPrint(CUgraph hGraph, [MarshalAs(UnmanagedType.LPStr)] string path, CUgraphDebugDot_flags flags);

            /// <summary>
            /// Create a user object<para/>
            /// Create a user object with the specified destructor callback and initial reference count. The initial references are owned by the caller.<para/>
            /// Destructor callbacks cannot make CUDA API calls and should avoid blocking behavior, as they
            /// are executed by a shared internal thread.Another thread may be signaled to perform such
            /// actions, if it does not block forward progress of tasks scheduled through CUDA.<para/>
            /// See CUDA User Objects in the CUDA C++ Programming Guide for more information on user objects.
            /// </summary>
            /// <param name="object_out">Location to return the user object handle</param>
            /// <param name="ptr">The pointer to pass to the destroy function</param>
            /// <param name="destroy">Callback to free the user object when it is no longer in use</param>
            /// <param name="initialRefcount">The initial refcount to create the object with, typically 1. The initial references are owned by the calling thread.</param>
            /// <param name="flags">Currently it is required to pass ::CU_USER_OBJECT_NO_DESTRUCTOR_SYNC, which is the only defined flag. This indicates that the destroy 
            /// callback cannot be waited on by any CUDA API.Users requiring synchronization of the callback should signal its completion manually.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuUserObjectCreate(ref CUuserObject object_out, IntPtr ptr, CUhostFn destroy,
                                                uint initialRefcount, CUuserObject_flags flags);

            /// <summary>
            /// Retain a reference to a user object<para/>
            /// Retains new references to a user object. The new references are owned by the caller.<para/>
            /// See CUDA User Objects in the CUDA C++ Programming Guide for more information on user objects.
            /// </summary>
            /// <param name="obj">The object to retain</param>
            /// <param name="count">The number of references to retain, typically 1. Must be nonzero and not larger than INT_MAX.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuUserObjectRetain(CUuserObject obj, uint count);

            /// <summary>
            /// Release a reference to a user object<para/>
            /// Releases user object references owned by the caller. The object's destructor is invoked if the reference count reaches zero.<para/>
            /// It is undefined behavior to release references not owned by the caller, or to use a user object handle after all references are released.<para/>
            /// See CUDA User Objects in the CUDA C++ Programming Guide for more information on user objects.
            /// </summary>
            /// <param name="obj">The object to release</param>
            /// <param name="count">The number of references to release, typically 1. Must be nonzero and not larger than INT_MAX.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuUserObjectRelease(CUuserObject obj, uint count);

            /// <summary>
            /// Retain a reference to a user object from a graph<para/>
            /// Creates or moves user object references that will be owned by a CUDA graph.<para/>
            /// See CUDA User Objects in the CUDA C++ Programming Guide for more information on user objects.
            /// </summary>
            /// <param name="graph">The graph to associate the reference with</param>
            /// <param name="obj">The user object to retain a reference for</param>
            /// <param name="count">The number of references to add to the graph, typically 1. Must be nonzero and not larger than INT_MAX.</param>
            /// <param name="flags">The optional flag ::CU_GRAPH_USER_OBJECT_MOVE transfers references from the calling thread, rather than create new references.Pass None to create new references.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphRetainUserObject(CUgraph graph, CUuserObject obj, uint count, CUuserObjectRetain_flags flags);

            /// <summary>
            /// Release a user object reference from a graph<para/>
            /// Releases user object references owned by a graph.<para/>
            /// See CUDA User Objects in the CUDA C++ Programming Guide for more information on user objects.
            /// </summary>
            /// <param name="graph">The graph that will release the reference</param>
            /// <param name="obj">The user object to release a reference for</param>
            /// <param name="count">The number of references to release, typically 1. Must be nonzero and not larger than INT_MAX.</param>
            /// <returns></returns>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuGraphReleaseUserObject(CUgraph graph, CUuserObject obj, uint count);
        }
        #endregion

        #region RDMADirect


        /// <summary>
        /// 
        /// </summary>
        public static class RDMADirect
        {
#if (NETCOREAPP)
            static RDMADirect()
            {
                DriverAPINativeMethods.Init();
            }
#endif

            /// <summary>
            /// Blocks until remote writes are visible to the specified scope<para/>
            /// Blocks until GPUDirect RDMA writes to the target context via mappings
            /// created through APIs like nvidia_p2p_get_pages(see
            /// https://docs.nvidia.com/cuda/gpudirect-rdma for more information), are
            /// visible to the specified scope.
            /// <para/>
            /// If the scope equals or lies within the scope indicated by
            /// ::CU_DEVICE_ATTRIBUTE_GPU_DIRECT_RDMA_WRITES_ORDERING, the call
            /// will be a no-op and can be safely omitted for performance.This can be
            /// determined by comparing the numerical values between the two enums, with
            /// smaller scopes having smaller values.
            /// Users may query support for this API via ::CU_DEVICE_ATTRIBUTE_FLUSH_FLUSH_GPU_DIRECT_RDMA_OPTIONS.
            /// </summary>
            /// <param name="target">The target of the operation, see ::CUflushGPUDirectRDMAWritesTarget</param>
            /// <param name="scope">The scope of the operation, see ::CUflushGPUDirectRDMAWritesScope</param>
            [DllImport(CUDA_DRIVER_API_DLL_NAME)]
            public static extern CUResult cuFlushGPUDirectRDMAWrites(CUflushGPUDirectRDMAWritesTarget target, CUflushGPUDirectRDMAWritesScope scope);

        }
        #endregion
    }
}
