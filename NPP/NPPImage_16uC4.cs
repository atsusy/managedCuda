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
using System.Diagnostics;
using ManagedCuda.BasicTypes;

namespace ManagedCuda.NPP
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NPPImage_16uC4 : NPPImageBase
    {
        #region Constructors
        /// <summary>
        /// Allocates new memory on device using NPP-Api.
        /// </summary>
        /// <param name="nWidthPixels">Image width in pixels</param>
        /// <param name="nHeightPixels">Image height in pixels</param>
        public NPPImage_16uC4(int nWidthPixels, int nHeightPixels)
        {
            _sizeOriginal.width = nWidthPixels;
            _sizeOriginal.height = nHeightPixels;
            _sizeRoi.width = nWidthPixels;
            _sizeRoi.height = nHeightPixels;
            _channels = 4;
            _isOwner = true;
            _typeSize = sizeof(ushort);

            _devPtr = NPPNativeMethods.NPPi.MemAlloc.nppiMalloc_16u_C4(nWidthPixels, nHeightPixels, ref _pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}, Pitch is: {3}, Number of color channels: {4}", DateTime.Now, "nppiMalloc_16u_C4", res, _pitch, _channels));

            if (_devPtr.Pointer == 0)
            {
                throw new NPPException("Device allocation error", null);
            }
            _devPtrRoi = _devPtr;
        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr.
        /// </summary>
        /// <param name="devPtr">Already allocated device ptr.</param>
        /// <param name="width">Image width in pixels</param>
        /// <param name="height">Image height in pixels</param>
        /// <param name="pitch">Pitch / Line step</param>
        /// <param name="isOwner">If TRUE, devPtr is freed when disposing</param>
        public NPPImage_16uC4(CUdeviceptr devPtr, int width, int height, int pitch, bool isOwner)
        {
            _devPtr = devPtr;
            _devPtrRoi = _devPtr;
            _sizeOriginal.width = width;
            _sizeOriginal.height = height;
            _sizeRoi.width = width;
            _sizeRoi.height = height;
            _pitch = pitch;
            _channels = 4;
            _isOwner = isOwner;
            _typeSize = sizeof(ushort);
        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr. Does not take ownership of decPtr.
        /// </summary>
        /// <param name="devPtr">Already allocated device ptr.</param>
        /// <param name="width">Image width in pixels</param>
        /// <param name="height">Image height in pixels</param>
        /// <param name="pitch">Pitch / Line step</param>
        public NPPImage_16uC4(CUdeviceptr devPtr, int width, int height, int pitch)
            : this(devPtr, width, height, pitch, false)
        {

        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr. Does not take ownership of inner image device pointer.
        /// </summary>
        /// <param name="image">NPP image</param>
        public NPPImage_16uC4(NPPImageBase image)
            : this(image.DevicePointer, image.Width, image.Height, image.Pitch, false)
        {

        }

        /// <summary>
        /// Allocates new memory on device using NPP-Api.
        /// </summary>
        /// <param name="size">Image size</param>
        public NPPImage_16uC4(NppiSize size)
            : this(size.width, size.height)
        {

        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr.
        /// </summary>
        /// <param name="devPtr">Already allocated device ptr.</param>
        /// <param name="size">Image size</param>
        /// <param name="pitch">Pitch / Line step</param>
        /// <param name="isOwner">If TRUE, devPtr is freed when disposing</param>
        public NPPImage_16uC4(CUdeviceptr devPtr, NppiSize size, int pitch, bool isOwner)
            : this(devPtr, size.width, size.height, pitch, isOwner)
        {

        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr.
        /// </summary>
        /// <param name="devPtr">Already allocated device ptr.</param>
        /// <param name="size">Image size</param>
        /// <param name="pitch">Pitch / Line step</param>
        public NPPImage_16uC4(CUdeviceptr devPtr, NppiSize size, int pitch)
            : this(devPtr, size.width, size.height, pitch)
        {

        }

        /// <summary>
        /// For dispose
        /// </summary>
        ~NPPImage_16uC4()
        {
            Dispose(false);
        }
        #endregion

        #region Converter operators

        /// <summary>
        /// Converts a NPPImage to a CudaPitchedDeviceVariable
        /// </summary>
        public CudaPitchedDeviceVariable<VectorTypes.ushort4> ToCudaPitchedDeviceVariable()
        {
            return new CudaPitchedDeviceVariable<VectorTypes.ushort4>(_devPtr, _sizeOriginal.width, _sizeOriginal.height, _pitch);
        }

        /// <summary>
        /// Converts a NPPImage to a CudaPitchedDeviceVariable
        /// </summary>
        /// <param name="img">NPPImage</param>
        /// <returns>CudaPitchedDeviceVariable with the same device pointer and size of NPPImage without ROI information</returns>
        public static implicit operator CudaPitchedDeviceVariable<VectorTypes.ushort4>(NPPImage_16uC4 img)
        {
            return img.ToCudaPitchedDeviceVariable();
        }

        /// <summary>
        /// Converts a CudaPitchedDeviceVariable to a NPPImage 
        /// </summary>
        /// <param name="img">CudaPitchedDeviceVariable</param>
        /// <returns>NPPImage with the same device pointer and size of CudaPitchedDeviceVariable with ROI set to full image</returns>
        public static implicit operator NPPImage_16uC4(CudaPitchedDeviceVariable<VectorTypes.ushort4> img)
        {
            return img.ToNPPImage();
        }
        #endregion

        #region Copy
        /// <summary>
        /// Image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="channel">Channel number. This number is added to the dst pointer</param>
        public void Copy(NPPImage_16uC1 dst, int channel)
        {
            if (channel < 0 | channel >= _channels) throw new ArgumentOutOfRangeException("channel", "channel must be in range [0..3].");
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_C4C1R(_devPtrRoi + channel * _typeSize, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_C4C1R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="channelSrc">Channel number. This number is added to the src pointer</param>
        /// <param name="channelDst">Channel number. This number is added to the dst pointer</param>
        public void Copy(NPPImage_16uC4 dst, int channelSrc, int channelDst)
        {
            if (channelSrc < 0 | channelSrc >= _channels) throw new ArgumentOutOfRangeException("channelSrc", "channelSrc must be in range [0..2].");
            if (channelDst < 0 | channelDst >= dst.Channels) throw new ArgumentOutOfRangeException("channelDst", "channelDst must be in range [0..2].");
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_C4CR(_devPtrRoi + channelSrc * _typeSize, _pitch, dst.DevicePointerRoi + channelDst * _typeSize, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_C4CR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Masked Operation 8-bit unsigned image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="mask">Mask image</param>
        public void Copy(NPPImage_16uC4 dst, NPPImage_8uC1 mask)
        {
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_C4MR(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_C4MR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Masked Operation 8-bit unsigned image copy. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="mask">Mask image</param>
        public void CopyA(NPPImage_16uC4 dst, NPPImage_8uC1 mask)
        {
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_AC4MR(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_AC4MR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void Copy(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image copy. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void CopyA(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Three-channel 8-bit unsigned packed to planar image copy.
        /// </summary>
        /// <param name="dst0">Destination image channel 0</param>
        /// <param name="dst1">Destination image channel 1</param>
        /// <param name="dst2">Destination image channel 2</param>
        /// <param name="dst3">Destination image channel 3</param>
        public void Copy(NPPImage_16uC1 dst0, NPPImage_16uC1 dst1, NPPImage_16uC1 dst2, NPPImage_16uC1 dst3)
        {
            CUdeviceptr[] array = new CUdeviceptr[] { dst0.DevicePointerRoi, dst1.DevicePointerRoi, dst2.DevicePointerRoi, dst3.DevicePointerRoi };
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_C4P4R(_devPtrRoi, _pitch, array, dst0.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_C4P4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Three-channel 8-bit unsigned planar to packed image copy.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="src3">Source image channel 2</param>
        /// <param name="dest">Destination image</param>
        public static void Copy(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 src3, NPPImage_16uC4 dest)
        {
            CUdeviceptr[] array = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_16u_P4C4R(array, src0.Pitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_16u_P4C4R", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region Set
        /// <summary>
        /// Set pixel values to nValue.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 4)</param>
        public void Set(ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_16u_C4R(nValue, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 4)</param>
        /// <param name="mask">Mask image</param>
        public void Set(ushort[] nValue, NPPImage_8uC1 mask)
        {
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_16u_C4MR(nValue, _devPtrRoi, _pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_16u_C4MR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value.
        /// </summary>
        /// <param name="nValue">Value to be set</param>
        /// <param name="channel">Channel number. This number is added to the dst pointer</param>
        public void Set(ushort nValue, int channel)
        {
            if (channel < 0 | channel >= _channels) throw new ArgumentOutOfRangeException("channel", "channel must be in range [0..3].");
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_16u_C4CR(nValue, _devPtrRoi + channel * _typeSize, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_16u_C4CR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value. Not affecting alpha channel.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 3)</param>
        public void SetA(ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_16u_AC4R(nValue, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value. Not affecting alpha channel.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 3)</param>
        /// <param name="mask">Mask image</param>
        public void SetA(ushort[] nValue, NPPImage_8uC1 mask)
        {
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_16u_AC4MR(nValue, _devPtrRoi, _pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_16u_AC4MR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Add
        /// <summary>
        /// Image addition, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Add(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_16u_C4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image addition, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Add(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_16u_C4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Add(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_16u_C4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Add(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_16u_C4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image addition, scale by 2^(-nScaleFactor), then clamp to saturated value. Unmodified Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void AddA(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_16u_AC4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image addition, scale by 2^(-nScaleFactor), then clamp to saturated value. Unmodified Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void AddA(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_16u_AC4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unmodified Alpha.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void AddA(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_16u_AC4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unmodified Alpha.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void AddA(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_16u_AC4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Logical
        /// <summary>
        /// image bit shift by constant (left).
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void LShiftC(uint[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.LeftShiftConst.nppiLShiftC_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (left), inplace.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        public void LShiftC(uint[] nConstant)
        {
            status = NPPNativeMethods.NPPi.LeftShiftConst.nppiLShiftC_16u_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (left). Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void LShiftCA(uint[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.LeftShiftConst.nppiLShiftC_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (left), inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        public void LShiftCA(uint[] nConstant)
        {
            status = NPPNativeMethods.NPPi.LeftShiftConst.nppiLShiftC_16u_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image bit shift by constant (right).
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void RShiftC(uint[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.RightShiftConst.nppiRShiftC_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (right), inplace.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        public void RShiftC(uint[] nConstant)
        {
            status = NPPNativeMethods.NPPi.RightShiftConst.nppiRShiftC_16u_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (right). Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void RShiftCA(uint[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.RightShiftConst.nppiRShiftC_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (right), inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        public void RShiftCA(uint[] nConstant)
        {
            status = NPPNativeMethods.NPPi.RightShiftConst.nppiRShiftC_16u_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void And(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.And.nppiAnd_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void And(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.And.nppiAnd_16u_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void And(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.AndConst.nppiAndC_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        public void And(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.AndConst.nppiAndC_16u_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void AndA(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.And.nppiAnd_16u_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void AndA(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.And.nppiAnd_16u_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void AndA(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.AndConst.nppiAndC_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        public void AndA(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.AndConst.nppiAndC_16u_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Or(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.Or.nppiOr_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Or(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.Or.nppiOr_16u_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void Or(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.OrConst.nppiOrC_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        public void Or(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.OrConst.nppiOrC_16u_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void OrA(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.Or.nppiOr_16u_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void OrA(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.Or.nppiOr_16u_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void OrA(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.OrConst.nppiOrC_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        public void OrA(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.OrConst.nppiOrC_16u_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Xor(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.Xor.nppiXor_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Xor(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.Xor.nppiXor_16u_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void Xor(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.XorConst.nppiXorC_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        public void Xor(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.XorConst.nppiXorC_16u_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void XorA(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.Xor.nppiXor_16u_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void XorA(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.Xor.nppiXor_16u_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        public void XorA(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.XorConst.nppiXorC_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        public void XorA(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.XorConst.nppiXorC_16u_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sub
        /// <summary>
        /// Image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Sub(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_16u_C4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Sub(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_16u_C4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Sub(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_16u_C4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Sub(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_16u_C4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SubA(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_16u_AC4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SubA(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_16u_AC4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SubA(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_16u_AC4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SubA(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_16u_AC4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Mul
        /// <summary>
        /// Image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Mul(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_16u_C4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Mul(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_16u_C4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Mul(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_16u_C4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Mul(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_16u_C4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication and scale by max bit width value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Mul(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.MulScale.nppiMulScale_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication and scale by max bit width value
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Mul(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.MulScale.nppiMulScale_16u_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image and scale by max bit width value
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        public void Mul(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.MulConstScale.nppiMulCScale_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image and scale by max bit width value
        /// </summary>
        /// <param name="nConstant">Value</param>
        public void Mul(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.MulConstScale.nppiMulCScale_16u_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void MulA(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_16u_AC4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void MulA(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_16u_AC4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void MulA(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_16u_AC4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void MulA(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_16u_AC4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void MulA(NPPImage_16uC4 src2, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.MulScale.nppiMulScale_16u_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void MulA(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.MulScale.nppiMulScale_16u_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        public void MulA(ushort[] nConstant, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.MulConstScale.nppiMulCScale_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        public void MulA(ushort[] nConstant)
        {
            status = NPPNativeMethods.NPPi.MulConstScale.nppiMulCScale_16u_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Div
        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Div(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_16u_C4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Div(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_16u_C4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Div(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_16u_C4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Div(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_16u_C4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Div(NPPImage_16uC4 src2, NPPImage_16uC4 dest, NppRoundMode rndMode, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivRound.nppiDiv_Round_16u_C4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, rndMode, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Div(NPPImage_16uC4 src2, NppRoundMode rndMode, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivRound.nppiDiv_Round_16u_C4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, rndMode, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void DivA(NPPImage_16uC4 src2, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_16u_AC4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void DivA(NPPImage_16uC4 src2, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_16u_AC4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void DivA(ushort[] nConstant, NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_16u_AC4RSfs(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void DivA(ushort[] nConstant, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_16u_AC4IRSfs(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void DivA(NPPImage_16uC4 src2, NPPImage_16uC4 dest, NppRoundMode rndMode, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivRound.nppiDiv_Round_16u_AC4RSfs(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, rndMode, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void DivA(NPPImage_16uC4 src2, NppRoundMode rndMode, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.DivRound.nppiDiv_Round_16u_AC4IRSfs(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, rndMode, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sqr
        /// <summary>
        /// Image squared, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Sqr(NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sqr.nppiSqr_16u_C4RSfs(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_16u_C4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image squared, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nScaleFactor">scaling factor</param>
        public void Sqr(int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sqr.nppiSqr_16u_C4IRSfs(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_16u_C4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image squared, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SqrA(NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sqr.nppiSqr_16u_AC4RSfs(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image squared, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SqrA(int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sqr.nppiSqr_16u_AC4IRSfs(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sqrt
        ///// <summary>
        ///// Image square root, scale by 2^(-nScaleFactor), then clamp to saturated value.
        ///// </summary>
        ///// <param name="dest">Destination image</param>
        ///// <param name="nScaleFactor">scaling factor</param>
        //public void Sqrt(NPPImage_16uC4 dest, int nScaleFactor)
        //{
        //	status = NPPNativeMethods.NPPi.Sqrt.nppiSqrt_16u_C4RSfs(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_16u_C4RSfs", status));
        //	NPPException.CheckNppStatus(status, this);
        //}

        ///// <summary>
        ///// Inplace image square root, scale by 2^(-nScaleFactor), then clamp to saturated value.
        ///// </summary>
        ///// <param name="nScaleFactor">scaling factor</param>
        //public void Sqrt(int nScaleFactor)
        //{
        //	status = NPPNativeMethods.NPPi.Sqrt.nppiSqrt_16u_C4IRSfs(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_16u_C4IRSfs", status));
        //	NPPException.CheckNppStatus(status, this);
        //}

        /// <summary>
        /// Image square root, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SqrtA(NPPImage_16uC4 dest, int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sqrt.nppiSqrt_16u_AC4RSfs(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_16u_AC4RSfs", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image square root, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nScaleFactor">scaling factor</param>
        public void SqrtA(int nScaleFactor)
        {
            status = NPPNativeMethods.NPPi.Sqrt.nppiSqrt_16u_AC4IRSfs(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_16u_AC4IRSfs", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Histogram
        /// <summary>
        /// Scratch-buffer size for HistogramEven.
        /// </summary>
        /// <param name="nLevels"></param>
        /// <returns></returns>
        public int HistogramEvenGetBufferSize(int[] nLevels)
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramEvenGetBufferSize_16u_C4R(_sizeRoi, nLevels, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEvenGetBufferSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Scratch-buffer size for HistogramEven. Not affecting Alpha channel. 
        /// </summary>
        /// <param name="nLevels"></param>
        /// <returns></returns>
        public int HistogramEvenGetBufferSizeA(int[] nLevels)
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramEvenGetBufferSize_16u_AC4R(_sizeRoi, nLevels, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEvenGetBufferSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Compute levels with even distribution.
        /// </summary>
        /// <param name="nLevels">The number of levels being computed. nLevels must be at least 2, otherwise an NPP_-
        /// HISTO_NUMBER_OF_LEVELS_ERROR error is returned.</param>
        /// <param name="nLowerBound">Lower boundary value of the lowest level.</param>
        /// <param name="nUpperBound">Upper boundary value of the greatest level.</param>
        /// <returns>An array of size nLevels which receives the levels being computed.</returns>
        public int[] EvenLevels(int nLevels, int nLowerBound, int nUpperBound)
        {
            int[] Levels = new int[nLevels];
            status = NPPNativeMethods.NPPi.Histogram.nppiEvenLevelsHost_32s(Levels, nLevels, nLowerBound, nUpperBound);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiEvenLevelsHost_32s", status));
            NPPException.CheckNppStatus(status, this);
            return Levels;
        }

        /// <summary>
        /// Histogram with evenly distributed bins. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (4 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 4</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 4</param>
        public void HistogramEven(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1, histogram[3].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };


            int bufferSize = HistogramEvenGetBufferSize(size);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramEven_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with evenly distributed bins. No additional buffer is allocated.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (4 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 4</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 4</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramEvenGetBufferSize(int[])"/></param>
        public void HistogramEven(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel, CudaDeviceVariable<byte> buffer)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1, histogram[3].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };

            int bufferSize = HistogramEvenGetBufferSize(size);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramEven_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with evenly distributed bins. Buffer is internally allocated and freed. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (3 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 3</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 3</param>
        public void HistogramEvenA(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };


            int bufferSize = HistogramEvenGetBufferSizeA(size);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramEven_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with evenly distributed bins. No additional buffer is allocated. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (3 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 3</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 3</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramEvenGetBufferSize(int[])"/></param>
        public void HistogramEvenA(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel, CudaDeviceVariable<byte> buffer)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };

            int bufferSize = HistogramEvenGetBufferSizeA(size);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramEven_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Scratch-buffer size for HistogramRange.
        /// </summary>
        /// <param name="nLevels"></param>
        /// <returns></returns>
        public int HistogramRangeGetBufferSize(int[] nLevels)
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramRangeGetBufferSize_16u_C4R(_sizeRoi, nLevels, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRangeGetBufferSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Scratch-buffer size for HistogramRange. Not affecting Alpha channel.
        /// </summary>
        /// <param name="nLevels"></param>
        /// <returns></returns>
        public int HistogramRangeGetBufferSizeA(int[] nLevels)
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramRangeGetBufferSize_16u_AC4R(_sizeRoi, nLevels, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRangeGetBufferSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 4</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 4</param>
        public void HistogramRange(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size, histogram[3].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSize(size);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramRange_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. No additional buffer is allocated.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 4</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 4</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramRangeGetBufferSize(int[])"/></param>
        public void HistogramRange(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels, CudaDeviceVariable<byte> buffer)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size, histogram[3].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSize(size);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramRange_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. Buffer is internally allocated and freed. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 3</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 3</param>
        public void HistogramRangeA(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSizeA(size);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramRange_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. No additional buffer is allocated. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 3</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 3</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramRangeGetBufferSize(int[])"/></param>
        public void HistogramRangeA(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels, CudaDeviceVariable<byte> buffer)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSizeA(size);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Histogram.nppiHistogramRange_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Geometric Transforms

        /// <summary>
        /// Compute shape of rotated image.
        /// </summary>
        /// <param name="nAngle">The angle of rotation in degrees.</param>
        /// <param name="nShiftX">Shift along horizontal axis</param>
        /// <param name="nShiftY">Shift along vertical axis</param>
        public double[,] GetRotateQuad(double nAngle, double nShiftX, double nShiftY)
        {
            double[,] quad = new double[4, 2];
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiGetRotateQuad(new NppiRect(_pointRoi, _sizeRoi), quad, nAngle, nShiftX, nShiftY);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetRotateQuad", status));
            NPPException.CheckNppStatus(status, this);
            return quad;
        }

        /// <summary>
        /// Compute bounding-box of rotated image.
        /// </summary>
        /// <param name="nAngle">The angle of rotation in degrees.</param>
        /// <param name="nShiftX">Shift along horizontal axis</param>
        /// <param name="nShiftY">Shift along vertical axis</param>
        public double[,] GetRotateBound(double nAngle, double nShiftX, double nShiftY)
        {
            double[,] bbox = new double[2, 2];
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiGetRotateBound(new NppiRect(_pointRoi, _sizeRoi), bbox, nAngle, nShiftX, nShiftY);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetRotateBound", status));
            NPPException.CheckNppStatus(status, this);
            return bbox;
        }

        /// <summary>
        /// Rotate images.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nAngle">The angle of rotation in degrees.</param>
        /// <param name="nShiftX">Shift along horizontal axis</param>
        /// <param name="nShiftY">Shift along vertical axis</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        public void Rotate(NPPImage_16uC4 dest, double nAngle, double nShiftX, double nShiftY, InterpolationMode eInterpolation)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiRotate_16u_C4R(_devPtr, _sizeOriginal, _pitch, new NppiRect(_pointRoi, _sizeRoi),
                dest.DevicePointer, dest.Pitch, new NppiRect(dest.PointRoi, dest.SizeRoi), nAngle, nShiftX, nShiftY, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRotate_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Rotate images. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nAngle">The angle of rotation in degrees.</param>
        /// <param name="nShiftX">Shift along horizontal axis</param>
        /// <param name="nShiftY">Shift along vertical axis</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        public void RotateA(NPPImage_16uC4 dest, double nAngle, double nShiftX, double nShiftY, InterpolationMode eInterpolation)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiRotate_16u_AC4R(_devPtr, _sizeOriginal, _pitch, new NppiRect(_pointRoi, _sizeRoi),
                dest.DevicePointer, dest.Pitch, new NppiRect(dest.PointRoi, dest.SizeRoi), nAngle, nShiftX, nShiftY, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRotate_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Mirror image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        public void Mirror(NPPImage_16uC4 dest, NppiAxis flip)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiMirror_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, flip);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Mirror image. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        public void MirrorA(NPPImage_16uC4 dest, NppiAxis flip)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiMirror_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, flip);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Affine Transformations
        /// <summary>
        /// Calculates affine transform coefficients given source rectangular ROI and its destination quadrangle projection
        /// </summary>
        /// <param name="quad">Destination quadrangle [4,2]</param>
        /// <returns>Affine transform coefficients [2,3]</returns>
        public double[,] GetAffineTransform(double[,] quad)
        {
            double[,] coeffs = new double[2, 3];
            NppiRect rect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiGetAffineTransform(rect, quad, coeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetAffineTransform", status));
            NPPException.CheckNppStatus(status, this);
            return coeffs;
        }

        /// <summary>
        /// Calculates affine transform projection of given source rectangular ROI
        /// </summary>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <returns>Destination quadrangle [4,2]</returns>
        public double[,] GetAffineQuad(double[,] coeffs)
        {
            double[,] quad = new double[4, 2];
            NppiRect rect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiGetAffineQuad(rect, quad, coeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetAffineQuad", status));
            NPPException.CheckNppStatus(status, this);
            return quad;
        }

        /// <summary>
        /// Calculates bounding box of the affine transform projection of the given source rectangular ROI
        /// </summary>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <returns>Destination quadrangle [2,2]</returns>
        public double[,] GetAffineBound(double[,] coeffs)
        {
            double[,] bound = new double[2, 2];
            NppiRect rect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiGetAffineBound(rect, bound, coeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetAffineBound", status));
            NPPException.CheckNppStatus(status, this);
            return bound;
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function operates using given transform coefficients that can be obtained
        /// by using nppiGetAffineTransform function or set explicitly. The function
        /// operates on source and destination regions of interest. The affine warp
        /// function transforms the source image pixel coordinates (x,y) according
        /// to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with 
        /// destination ROI specification.<para/>
        /// <para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but does not perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the
        /// fast one.<para/>
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes
        /// aligned. This is always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected 
        /// image are separated from the ROI by at least 63 bytes from each side.
        /// However, this requires the whole ROI to be part of allocated memory. In case
        /// when the conditions above are not satisfied, the function may decrease in
        /// speed slightly and will return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpAffine(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffine_16u_C4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffine_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. Not affecting Alpha channel.<para/>This
        /// function operates using given transform coefficients that can be obtained
        /// by using nppiGetAffineTransform function or set explicitly. The function
        /// operates on source and destination regions of interest. The affine warp
        /// function transforms the source image pixel coordinates (x,y) according
        /// to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with 
        /// destination ROI specification.<para/>
        /// <para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but does not perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the
        /// fast one.<para/>
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes
        /// aligned. This is always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected 
        /// image are separated from the ROI by at least 63 bytes from each side.
        /// However, this requires the whole ROI to be part of allocated memory. In case
        /// when the conditions above are not satisfied, the function may decrease in
        /// speed slightly and will return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpAffineA(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffine_16u_AC4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffine_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse affine transform of an image.<para/>
        /// This function operates using given transform coefficients that can be
        /// obtained by using nppiGetAffineTransform function or set explicitly. Thus
        /// there is no need to invert coefficients in your application before calling
        /// WarpAffineBack. The function operates on source and destination regions of
        /// interest.<para/>
        /// The affine warp function transforms the source image pixel coordinates
        /// (x,y) according to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with
        /// destination ROI specification.<para/><para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpAffineBack(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffineBack_16u_C4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBack_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse affine transform of an image. Not affecting Alpha channel.<para/>
        /// This function operates using given transform coefficients that can be
        /// obtained by using nppiGetAffineTransform function or set explicitly. Thus
        /// there is no need to invert coefficients in your application before calling
        /// WarpAffineBack. The function operates on source and destination regions of
        /// interest.<para/>
        /// The affine warp function transforms the source image pixel coordinates
        /// (x,y) according to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with
        /// destination ROI specification.<para/><para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpAffineBackA(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffineBack_16u_AC4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBack_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function performs affine warping of a the specified quadrangle in the
        /// source image to the specified quadrangle in the destination image. The
        /// function nppiWarpAffineQuad uses the same formulas for pixel mapping as in
        /// nppiWarpAffine function. The transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// eInterpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="dstQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpAffineQuad(double[,] srcQuad, NPPImage_16uC4 dest, double[,] dstQuad, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffineQuad_16u_C4R(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, dstQuad, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineQuad_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. Not affecting Alpha channel. <para/>This
        /// function performs affine warping of a the specified quadrangle in the
        /// source image to the specified quadrangle in the destination image. The
        /// function nppiWarpAffineQuad uses the same formulas for pixel mapping as in
        /// nppiWarpAffine function. The transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// eInterpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="dstQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpAffineQuadA(double[,] srcQuad, NPPImage_16uC4 dest, double[,] dstQuad, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffineQuad_16u_AC4R(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, dstQuad, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineQuad_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function operates using given transform coefficients that can be obtained
        /// by using nppiGetAffineTransform function or set explicitly. The function
        /// operates on source and destination regions of interest. The affine warp
        /// function transforms the source image pixel coordinates (x,y) according
        /// to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with 
        /// destination ROI specification.<para/>
        /// <para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but does not perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the
        /// fast one.<para/>
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes
        /// aligned. This is always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected 
        /// image are separated from the ROI by at least 63 bytes from each side.
        /// However, this requires the whole ROI to be part of allocated memory. In case
        /// when the conditions above are not satisfied, the function may decrease in
        /// speed slightly and will return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public static void WarpAffine(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffine_16u_P4R(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffine_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Inverse affine transform of an image.<para/>
        /// This function operates using given transform coefficients that can be
        /// obtained by using nppiGetAffineTransform function or set explicitly. Thus
        /// there is no need to invert coefficients in your application before calling
        /// WarpAffineBack. The function operates on source and destination regions of
        /// interest.<para/>
        /// The affine warp function transforms the source image pixel coordinates
        /// (x,y) according to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with
        /// destination ROI specification.<para/><para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public static void WarpAffineBack(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffineBack_16u_P4R(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBack_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function performs affine warping of a the specified quadrangle in the
        /// source image to the specified quadrangle in the destination image. The
        /// function nppiWarpAffineQuad uses the same formulas for pixel mapping as in
        /// nppiWarpAffine function. The transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// eInterpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dstQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public static void WarpAffineQuad(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, double[,] srcQuad, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, double[,] dstQuad, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods.NPPi.AffinTransforms.nppiWarpAffineQuad_16u_P4R(src, src0.Size, src0.Pitch, rectIn, srcQuad, dst, dest0.Pitch, rectOut, dstQuad, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineQuad_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region Perspective Transformations
        /// <summary>
        /// Calculates affine transform coefficients given source rectangular ROI and its destination quadrangle projection
        /// </summary>
        /// <param name="quad">Destination quadrangle [4,2]</param>
        /// <returns>Perspective transform coefficients [3,3]</returns>
        public double[,] GetPerspectiveTransform(double[,] quad)
        {
            double[,] coeffs = new double[3, 3];
            NppiRect rect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiGetPerspectiveTransform(rect, quad, coeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetPerspectiveTransform", status));
            NPPException.CheckNppStatus(status, this);
            return coeffs;
        }

        /// <summary>
        ///Calculates perspective transform projection of given source rectangular ROI
        /// </summary>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <returns>Destination quadrangle [4,2]</returns>
        public double[,] GetPerspectiveQuad(double[,] coeffs)
        {
            double[,] quad = new double[4, 2];
            NppiRect rect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiGetPerspectiveQuad(rect, quad, coeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetPerspectiveQuad", status));
            NPPException.CheckNppStatus(status, this);
            return quad;
        }

        /// <summary>
        /// Calculates bounding box of the affine transform projection of the given source rectangular ROI
        /// </summary>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <returns>Destination quadrangle [2,2]</returns>
        public double[,] GetPerspectiveBound(double[,] coeffs)
        {
            double[,] bound = new double[2, 2];
            NppiRect rect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiGetPerspectiveBound(rect, bound, coeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGetPerspectiveBound", status));
            NPPException.CheckNppStatus(status, this);
            return bound;
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. The function operates on source and destination regions 
        /// of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpPerspective(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspective_16u_C4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspective_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image. Not affecting Alpha channel.<para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. The function operates on source and destination regions 
        /// of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpPerspectiveA(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspective_16u_AC4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspective_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse perspective transform of an image. <para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. Thus there is no need to invert coefficients in your application 
        /// before calling WarpPerspectiveBack. The function operates on source and
        /// destination regions of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpPerspectiveBack(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspectiveBack_16u_C4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBack_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse perspective transform of an image. Not affecting Alpha channel. <para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. Thus there is no need to invert coefficients in your application 
        /// before calling WarpPerspectiveBack. The function operates on source and
        /// destination regions of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpPerspectiveBackA(NPPImage_16uC4 dest, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspectiveBack_16u_AC4R(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBack_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function performs perspective warping of a the specified
        /// quadrangle in the source image to the specified quadrangle in the
        /// destination image. The function nppiWarpPerspectiveQuad uses the same
        /// formulas for pixel mapping as in nppiWarpPerspective function. The
        /// transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="destQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpPerspectiveQuad(double[,] srcQuad, NPPImage_16uC4 dest, double[,] destQuad, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspectiveQuad_16u_C4R(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, destQuad, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveQuad_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image. Not affecting Alpha channel.<para/>
        /// This function performs perspective warping of a the specified
        /// quadrangle in the source image to the specified quadrangle in the
        /// destination image. The function nppiWarpPerspectiveQuad uses the same
        /// formulas for pixel mapping as in nppiWarpPerspective function. The
        /// transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="destQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public void WarpPerspectiveQuadA(double[,] srcQuad, NPPImage_16uC4 dest, double[,] destQuad, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspectiveQuad_16u_AC4R(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, destQuad, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveQuad_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. The function operates on source and destination regions 
        /// of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public static void WarpPerspective(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspective_16u_P4R(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspective_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Inverse perspective transform of an image. <para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. Thus there is no need to invert coefficients in your application 
        /// before calling WarpPerspectiveBack. The function operates on source and
        /// destination regions of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public static void WarpPerspectiveBack(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspectiveBack_16u_P4R(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBack_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function performs perspective warping of a the specified
        /// quadrangle in the source image to the specified quadrangle in the
        /// destination image. The function nppiWarpPerspectiveQuad uses the same
        /// formulas for pixel mapping as in nppiWarpPerspective function. The
        /// transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="destQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        public static void WarpPerspectiveQuad(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, double[,] srcQuad, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, double[,] destQuad, InterpolationMode eInterpolation)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods.NPPi.PerspectiveTransforms.nppiWarpPerspectiveQuad_16u_P4R(src, src0.Size, src0.Pitch, rectIn, srcQuad, dst, dest0.Pitch, rectOut, destQuad, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveQuad_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region Alpha Composition

        /// <summary>
        /// Four 8-bit unsigned char channel image composition using image alpha values (0 - max channel pixel value).
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppAlphaOp">alpha compositing operation</param>
        public void AlphaComp(NPPImage_16uC4 src2, NPPImage_16uC4 dest, NppiAlphaOp nppAlphaOp)
        {
            status = NPPNativeMethods.NPPi.AlphaComp.nppiAlphaComp_16u_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppAlphaOp);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaComp_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image composition using constant alpha.
        /// </summary>
        /// <param name="alpha1">constant alpha for this image</param>
        /// <param name="src2">2nd source image</param>
        /// <param name="alpha2">constant alpha for src2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppAlphaOp">alpha compositing operation</param>
        public void AlphaComp(ushort alpha1, NPPImage_16uC4 src2, ushort alpha2, NPPImage_16uC4 dest, NppiAlphaOp nppAlphaOp)
        {
            status = NPPNativeMethods.NPPi.AlphaCompConst.nppiAlphaCompC_16u_C4R(_devPtrRoi, _pitch, alpha1, src2.DevicePointerRoi, src2.Pitch, alpha2, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppAlphaOp);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaCompC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image composition using constant alpha. Not affecting alpha channel.
        /// </summary>
        /// <param name="alpha1">constant alpha for this image</param>
        /// <param name="src2">2nd source image</param>
        /// <param name="alpha2">constant alpha for src2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppAlphaOp">alpha compositing operation</param>
        public void AlphaCompA(ushort alpha1, NPPImage_16uC4 src2, ushort alpha2, NPPImage_16uC4 dest, NppiAlphaOp nppAlphaOp)
        {
            status = NPPNativeMethods.NPPi.AlphaCompConst.nppiAlphaCompC_16u_AC4R(_devPtrRoi, _pitch, alpha1, src2.DevicePointerRoi, src2.Pitch, alpha2, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppAlphaOp);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaCompC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image premultiplication using constant alpha.
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="dest">Destination image</param>
        public void AlphaPremul(ushort alpha, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.AlphaPremulConst.nppiAlphaPremulC_16u_C4R(_devPtrRoi, _pitch, alpha, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// In place alpha premultiplication using constant alpha.
        /// </summary>
        /// <param name="alpha">alpha</param>
        public void AlphaPremul(ushort alpha)
        {
            status = NPPNativeMethods.NPPi.AlphaPremulConst.nppiAlphaPremulC_16u_C4IR(alpha, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image premultiplication using constant alpha. Not affecting alpha channel.
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="dest">Destination image</param>
        public void AlphaPremulA(ushort alpha, NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.AlphaPremulConst.nppiAlphaPremulC_16u_AC4R(_devPtrRoi, _pitch, alpha, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// In place alpha premultiplication using constant alpha. Not affecting alpha channel.
        /// </summary>
        /// <param name="alpha">alpha</param>
        public void AlphaPremulA(ushort alpha)
        {
            status = NPPNativeMethods.NPPi.AlphaPremulConst.nppiAlphaPremulC_16u_AC4IR(alpha, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four 8-bit unsigned char channel image premultiplication with pixel alpha (0 - max channel pixel value).
        /// </summary>
        /// <param name="dest">Destination image</param>
        public void AlphaPremulA(NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.AlphaPremul.nppiAlphaPremul_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremul_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four 8-bit unsigned char channel in place image premultiplication with pixel alpha (0 - max channel pixel value).
        /// </summary>
        public void AlphaPremulA()
        {
            status = NPPNativeMethods.NPPi.AlphaPremul.nppiAlphaPremul_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremul_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Convert
        /// <summary>
        /// 16-bit unsigned to 8-bit unsigned conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void Convert(NPPImage_8uC4 dst)
        {
            status = NPPNativeMethods.NPPi.BitDepthConversion.nppiConvert_16u8u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_16u8u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 16-bit unsigned to 32-bit signed conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void Convert(NPPImage_32sC4 dst)
        {
            status = NPPNativeMethods.NPPi.BitDepthConversion.nppiConvert_16u32s_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_16u32s_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 16-bit unsigned to 32-bit float conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void Convert(NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.BitDepthConversion.nppiConvert_16u32f_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 16-bit unsigned to 8-bit unsigned conversion. Not Affecting alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void ConvertA(NPPImage_8uC4 dst)
        {
            status = NPPNativeMethods.NPPi.BitDepthConversion.nppiConvert_16u8u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_16u8u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 16-bit unsigned to 32-bit signed conversion. Not Affecting alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void ConvertA(NPPImage_32sC4 dst)
        {
            status = NPPNativeMethods.NPPi.BitDepthConversion.nppiConvert_16u32s_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_16u32s_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 16-bit unsigned to 32-bit float conversion. Not Affecting alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void ConvertA(NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.BitDepthConversion.nppiConvert_16u32f_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sum
        /// <summary>
        /// Scratch-buffer size for nppiSum_16u_C4R.
        /// </summary>
        /// <returns></returns>
        public int SumGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Sum.nppiSumGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image sum with 64-bit double precision result. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 4 * sizeof(double)</param>
        public void Sum(CudaDeviceVariable<double> result)
        {
            int bufferSize = SumGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Sum.nppiSum_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit double precision result. No additional buffer is allocated.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SumGetBufferHostSize()"/></param>
        public void Sum(CudaDeviceVariable<double> result, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = SumGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Sum.nppiSum_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for nppiSum_16u_C4R. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int SumGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Sum.nppiSumGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image sum with 64-bit double precision result. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 3 * sizeof(double)</param>
        public void SumA(CudaDeviceVariable<double> result)
        {
            int bufferSize = SumGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Sum.nppiSum_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit double precision result. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SumGetBufferHostSizeA()"/></param>
        public void SumA(CudaDeviceVariable<double> result, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = SumGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Sum.nppiSum_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Min
        /// <summary>
        /// Scratch-buffer size for Min.
        /// </summary>
        /// <returns></returns>
        public int MinGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Min.nppiMinGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        public void Min(CudaDeviceVariable<ushort> min)
        {
            int bufferSize = MinGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Min.nppiMin_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinGetBufferHostSize()"/></param>
        public void Min(CudaDeviceVariable<ushort> min, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MinGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Min.nppiMin_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Min. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int MinGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Min.nppiMinGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        public void MinA(CudaDeviceVariable<ushort> min)
        {
            int bufferSize = MinGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Min.nppiMin_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinGetBufferHostSizeA()"/></param>
        public void MinA(CudaDeviceVariable<ushort> min, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MinGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Min.nppiMin_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinIndex
        /// <summary>
        /// Scratch-buffer size for MinIndex.
        /// </summary>
        /// <returns></returns>
        public int MinIndexGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MinIdx.nppiMinIndxGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndxGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        public void MinIndex(CudaDeviceVariable<ushort> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY)
        {
            int bufferSize = MinIndexGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MinIdx.nppiMinIndx_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinIndexGetBufferHostSize()"/></param>
        public void MinIndex(CudaDeviceVariable<ushort> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MinIndexGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MinIdx.nppiMinIndx_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for MinIndex. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int MinIndexGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MinIdx.nppiMinIndxGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndxGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        public void MinIndexA(CudaDeviceVariable<ushort> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY)
        {
            int bufferSize = MinIndexGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MinIdx.nppiMinIndx_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinIndexGetBufferHostSizeA()"/></param>
        public void MinIndexA(CudaDeviceVariable<ushort> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MinIndexGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MinIdx.nppiMinIndx_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Max
        /// <summary>
        /// Scratch-buffer size for Max.
        /// </summary>
        /// <returns></returns>
        public int MaxGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Max.nppiMaxGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        public void Max(CudaDeviceVariable<ushort> max)
        {
            int bufferSize = MaxGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Max.nppiMax_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel maximum. No additional buffer is allocated.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxGetBufferHostSize()"/></param>
        public void Max(CudaDeviceVariable<ushort> max, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaxGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Max.nppiMax_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Max. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int MaxGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.Max.nppiMaxGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        public void MaxA(CudaDeviceVariable<ushort> max)
        {
            int bufferSize = MaxGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.Max.nppiMax_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel maximum. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxGetBufferHostSizeA()"/></param>
        public void MaxA(CudaDeviceVariable<ushort> max, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaxGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.Max.nppiMax_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MaxIndex
        /// <summary>
        /// Scratch-buffer size for MaxIndex.
        /// </summary>
        /// <returns></returns>
        public int MaxIndexGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MaxIdx.nppiMaxIndxGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndxGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        public void MaxIndex(CudaDeviceVariable<ushort> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY)
        {
            int bufferSize = MaxIndexGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MaxIdx.nppiMaxIndx_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxIndexGetBufferHostSize()"/></param>
        public void MaxIndex(CudaDeviceVariable<ushort> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaxIndexGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MaxIdx.nppiMaxIndx_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for MaxIndex. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int MaxIndexGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MaxIdx.nppiMaxIndxGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndxGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        public void MaxIndexA(CudaDeviceVariable<ushort> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY)
        {
            int bufferSize = MaxIndexGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MaxIdx.nppiMaxIndx_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxIndexGetBufferHostSizeA()"/></param>
        public void MaxIndexA(CudaDeviceVariable<ushort> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaxIndexGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MaxIdx.nppiMaxIndx_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinMax
        /// <summary>
        /// Scratch-buffer size for MinMax.
        /// </summary>
        /// <returns></returns>
        public int MinMaxGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MinMaxNew.nppiMinMaxGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMaxGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum and maximum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        public void MinMax(CudaDeviceVariable<ushort> min, CudaDeviceVariable<ushort> max)
        {
            int bufferSize = MinMaxGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MinMaxNew.nppiMinMax_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, min.DevicePointer, max.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMax_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum and maximum. No additional buffer is allocated.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(ushort)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinMaxGetBufferHostSize()"/></param>
        public void MinMax(CudaDeviceVariable<ushort> min, CudaDeviceVariable<ushort> max, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MinMaxGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MinMaxNew.nppiMinMax_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, min.DevicePointer, max.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMax_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for MinMax. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int MinMaxGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MinMaxNew.nppiMinMaxGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMaxGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum and maximum. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        public void MinMaxA(CudaDeviceVariable<ushort> min, CudaDeviceVariable<ushort> max)
        {
            int bufferSize = MinMaxGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MinMaxNew.nppiMinMax_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, min.DevicePointer, max.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMax_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum and maximum. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(ushort)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinMaxGetBufferHostSizeA()"/></param>
        public void MinMaxA(CudaDeviceVariable<ushort> min, CudaDeviceVariable<ushort> max, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MinMaxGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MinMaxNew.nppiMinMax_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, min.DevicePointer, max.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMax_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Mean
        /// <summary>
        /// Scratch-buffer size for Mean.
        /// </summary>
        /// <returns></returns>
        public int MeanGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MeanNew.nppiMeanGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMeanGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image mean with 64-bit double precision result. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 4 * sizeof(double)</param>
        public void Mean(CudaDeviceVariable<double> mean)
        {
            int bufferSize = MeanGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MeanNew.nppiMean_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image mean with 64-bit double precision result. No additional buffer is allocated.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MeanGetBufferHostSize()"/></param>
        public void Mean(CudaDeviceVariable<double> mean, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MeanGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MeanNew.nppiMean_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Mean. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int MeanGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MeanNew.nppiMeanGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMeanGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image mean with 64-bit double precision result. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 3 * sizeof(double)</param>
        public void MeanA(CudaDeviceVariable<double> mean)
        {
            int bufferSize = MeanGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.MeanNew.nppiMean_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image mean with 64-bit double precision result. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MeanGetBufferHostSize()"/></param>
        public void MeanA(CudaDeviceVariable<double> mean, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MeanGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MeanNew.nppiMean_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormInf
        /// <summary>
        /// Scratch-buffer size for Norm inf.
        /// </summary>
        /// <returns></returns>
        public int NormInfGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormInf.nppiNormInfGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormInfGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image infinity norm. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 4 * sizeof(double)</param>
        public void NormInf(CudaDeviceVariable<double> norm)
        {
            int bufferSize = NormInfGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormInf.nppiNorm_Inf_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_Inf_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image infinity norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormInfGetBufferHostSize()"/></param>
        public void NormInf(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormInfGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormInf.nppiNorm_Inf_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_Inf_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Norm inf. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormInfGetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormInf.nppiNormInfGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormInfGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image infinity norm. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        public void NormInfA(CudaDeviceVariable<double> norm)
        {
            int bufferSize = NormInfGetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormInf.nppiNorm_Inf_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_Inf_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image infinity norm. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormInfGetBufferHostSize()"/></param>
        public void NormInfA(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormInfGetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormInf.nppiNorm_Inf_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_Inf_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormL1
        /// <summary>
        /// Scratch-buffer size for Norm L1.
        /// </summary>
        /// <returns></returns>
        public int NormL1GetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormL1.nppiNormL1GetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormL1GetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image L1 norm. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 4 * sizeof(double)</param>
        public void NormL1(CudaDeviceVariable<double> norm)
        {
            int bufferSize = NormL1GetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormL1.nppiNorm_L1_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L1_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image L1 norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormL1GetBufferHostSize()"/></param>
        public void NormL1(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormL1GetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormL1.nppiNorm_L1_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L1_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Scratch-buffer size for Norm L1. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormL1GetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormL1.nppiNormL1GetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormL1GetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image L1 norm. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        public void NormL1A(CudaDeviceVariable<double> norm)
        {
            int bufferSize = NormL1GetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormL1.nppiNorm_L1_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L1_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image L1 norm. No additional buffer is allocated. Not affecting Alpha.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormL1GetBufferHostSize()"/></param>
        public void NormL1A(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormL1GetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormL1.nppiNorm_L1_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L1_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormL2
        /// <summary>
        /// Scratch-buffer size for Norm L2.
        /// </summary>
        /// <returns></returns>
        public int NormL2GetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormL2.nppiNormL2GetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormL2GetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image L2 norm. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 4 * sizeof(double)</param>
        public void NormL2(CudaDeviceVariable<double> norm)
        {
            int bufferSize = NormL2GetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormL2.nppiNorm_L2_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L2_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image L2 norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormL2GetBufferHostSize()"/></param>
        public void NormL2(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormL2GetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormL2.nppiNorm_L2_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L2_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Norm L2. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormL2GetBufferHostSizeA()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormL2.nppiNormL2GetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormL2GetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image L2 norm. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        public void NormL2A(CudaDeviceVariable<double> norm)
        {
            int bufferSize = NormL2GetBufferHostSizeA();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormL2.nppiNorm_L2_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L2_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image L2 norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormL2GetBufferHostSize()"/></param>
        public void NormL2A(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormL2GetBufferHostSizeA();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormL2.nppiNorm_L2_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L2_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Compare
        /// <summary>
        /// Compare pSrc1's pixels with corresponding pixels in pSrc2.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        public void Compare(NPPImage_16uC4 src2, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Compare.nppiCompare_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompare_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Compare pSrc's pixels with constant value.
        /// </summary>
        /// <param name="nConstant">constant value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        public void Compare(ushort[] nConstant, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Compare.nppiCompareC_16u_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompareC_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Compare pSrc1's pixels with corresponding pixels in pSrc2. Not affecting Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        public void CompareA(NPPImage_16uC4 src2, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Compare.nppiCompare_16u_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompare_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Compare pSrc's pixels with constant value. Not affecting Alpha.
        /// </summary>
        /// <param name="nConstant">constant value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        public void CompareA(ushort[] nConstant, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Compare.nppiCompareC_16u_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompareC_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Threshold
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        public void ThresholdA(NPPImage_16uC4 dest, ushort[] nThreshold, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        public void ThresholdA(ushort[] nThreshold, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThreshold, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdGT
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        public void ThresholdGTA(NPPImage_16uC4 dest, ushort[] nThreshold)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_GT_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GT_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        public void ThresholdGTA(ushort[] nThreshold)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_GT_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThreshold);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GT_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdLT
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        public void ThresholdLTA(NPPImage_16uC4 dest, ushort[] nThreshold)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_LT_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LT_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        public void ThresholdLTA(ushort[] nThreshold)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_LT_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThreshold);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LT_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdVal
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        public void ThresholdA(NPPImage_16uC4 dest, ushort[] nThreshold, ushort[] nValue, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_Val_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nValue, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_Val_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        public void ThresholdA(ushort[] nThreshold, ushort[] nValue, NppCmpOp eComparisonOperation)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_Val_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nValue, eComparisonOperation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_Val_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdGTVal
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        public void ThresholdGTA(NPPImage_16uC4 dest, ushort[] nThreshold, ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_GTVal_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nValue);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GTVal_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        public void ThresholdGTA(ushort[] nThreshold, ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_GTVal_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nValue);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GTVal_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdLTVal
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        public void ThresholdLTA(NPPImage_16uC4 dest, ushort[] nThreshold, ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_LTVal_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nValue);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTVal_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        public void ThresholdLTA(ushort[] nThreshold, ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_LTVal_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nValue);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTVal_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdLTValGTVal
        /// <summary>
        /// Image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is less than nThresholdLT is true, the pixel is set
        /// to nValueLT, else if sourcePixel is greater than nThresholdGT the pixel is set to nValueGT, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThresholdLT">The thresholdLT value.</param>
        /// <param name="nValueLT">The thresholdLT replacement value.</param>
        /// <param name="nThresholdGT">The thresholdGT value.</param>
        /// <param name="nValueGT">The thresholdGT replacement value.</param>
        public void ThresholdLTGTA(NPPImage_16uC4 dest, ushort[] nThresholdLT, ushort[] nValueLT, ushort[] nThresholdGT, ushort[] nValueGT)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_LTValGTVal_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThresholdLT, nValueLT, nThresholdGT, nValueGT);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTValGTVal_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold. Not affecting Alpha.<para/>
        /// If for a comparison operations sourcePixel is less than nThresholdLT is true, the pixel is set
        /// to nValueLT, else if sourcePixel is greater than nThresholdGT the pixel is set to nValueGT, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThresholdLT">The thresholdLT value.</param>
        /// <param name="nValueLT">The thresholdLT replacement value.</param>
        /// <param name="nThresholdGT">The thresholdGT value.</param>
        /// <param name="nValueGT">The thresholdGT replacement value.</param>
        public void ThresholdLTGTA(ushort[] nThresholdLT, ushort[] nValueLT, ushort[] nThresholdGT, ushort[] nValueGT)
        {
            status = NPPNativeMethods.NPPi.Threshold.nppiThreshold_LTValGTVal_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, nThresholdLT, nValueLT, nThresholdGT, nValueGT);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTValGTVal_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        //new in Cuda 5.5
        #region DotProduct
        /// <summary>
        /// Device scratch buffer size (in bytes) for nppiDotProd_16u64f_C4R.
        /// </summary>
        /// <returns></returns>
        public int DotProdGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.DotProd.nppiDotProdGetBufferHostSize_16u64f_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProdGetBufferHostSize_16u64f_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Four-channel 16-bit unsigned image DotProd.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (4 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="DotProdGetBufferHostSize()"/></param>
        public void DotProduct(NPPImage_16uC4 src2, CudaDeviceVariable<double> pDp, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = DotProdGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.DotProd.nppiDotProd_16u64f_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_16u64f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four-channel 16-bit unsigned image DotProd. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (4 * sizeof(double))</param>
        public void DotProduct(NPPImage_16uC4 src2, CudaDeviceVariable<double> pDp)
        {
            int bufferSize = DotProdGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.DotProd.nppiDotProd_16u64f_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_16u64f_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for nppiDotProd_16u64f_C4R. Ignoring alpha channel.
        /// </summary>
        /// <returns></returns>
        public int ADotProdGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.DotProd.nppiDotProdGetBufferHostSize_16u64f_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProdGetBufferHostSize_16u64f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Four-channel 16-bit unsigned image DotProd. Ignoring alpha channel.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ADotProdGetBufferHostSize()"/></param>
        public void ADotProduct(NPPImage_16uC4 src2, CudaDeviceVariable<double> pDp, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = DotProdGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.DotProd.nppiDotProd_16u64f_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_16u64f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four-channel 16-bit unsigned image DotProd. Buffer is internally allocated and freed. Ignoring alpha channel.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (3 * sizeof(double))</param>
        public void ADotProduct(NPPImage_16uC4 src2, CudaDeviceVariable<double> pDp)
        {
            int bufferSize = DotProdGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.DotProd.nppiDotProd_16u64f_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_16u64f_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region LUT


        /// <summary>
        /// look-up-table color conversion.<para/>
        /// The LUT is derived from a set of user defined mapping points through linear interpolation.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="values0">array of user defined OUTPUT values, channel 0</param>
        /// <param name="levels0">array of user defined INPUT values, channel 0</param>
        /// <param name="values1">array of user defined OUTPUT values, channel 1</param>
        /// <param name="levels1">array of user defined INPUT values, channel 1</param>
        /// <param name="values2">array of user defined OUTPUT values, channel 2</param>
        /// <param name="levels2">array of user defined INPUT values, channel 2</param>
        /// <param name="values3">array of user defined OUTPUT values, channel 3</param>
        /// <param name="levels3">array of user defined INPUT values, channel 3</param>
        public void Lut(NPPImage_16uC4 dest, CudaDeviceVariable<int> values0, CudaDeviceVariable<int> levels0, CudaDeviceVariable<int> values1, CudaDeviceVariable<int> levels1,
            CudaDeviceVariable<int> values2, CudaDeviceVariable<int> levels2, CudaDeviceVariable<int> values3, CudaDeviceVariable<int> levels3)
        {
            CUdeviceptr[] values = new CUdeviceptr[4];
            CUdeviceptr[] levels = new CUdeviceptr[4];
            int[] levelLengths = new int[4];

            values[0] = values0.DevicePointer;
            values[1] = values1.DevicePointer;
            values[2] = values2.DevicePointer;
            values[3] = values3.DevicePointer;

            levels[0] = levels0.DevicePointer;
            levels[1] = levels1.DevicePointer;
            levels[2] = levels2.DevicePointer;
            levels[3] = levels3.DevicePointer;

            levelLengths[0] = levels0.Size;
            levelLengths[1] = levels1.Size;
            levelLengths[2] = levels2.Size;
            levelLengths[3] = levels3.Size;

            status = NPPNativeMethods.NPPi.ColorLUTLinear.nppiLUT_Linear_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, values, levels, levelLengths);

            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUT(NPPImage_16uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods.NPPi.ColorLUT.nppiLUT_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation. 
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTCubic(NPPImage_16uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods.NPPi.ColorLUTCubic.nppiLUT_Cubic_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// range restricted palette look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points in a palette and 
        /// source pixels are then processed using a restricted bit range when looking up palette values.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pTable">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT palette values.</param>
        /// <param name="nBitSize">Number of least significant bits (must be &gt; 0 and &lt;= 8) of each source pixel value to use as index into palette table during conversion.</param>
        public void LUTPalette(NPPImage_16uC4 dst, CudaDeviceVariable<byte>[] pTable, int nBitSize)
        {
            CUdeviceptr[] ptrsT = new CUdeviceptr[] { pTable[0].DevicePointer, pTable[1].DevicePointer, pTable[2].DevicePointer, pTable[3].DevicePointer };
            status = NPPNativeMethods.NPPi.ColorLUTPalette.nppiLUTPalette_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsT, nBitSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUTPalette_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUT(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods.NPPi.ColorLUT.nppiLUT_16u_C4IR(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation. 
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTCubic(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods.NPPi.ColorLUTCubic.nppiLUT_Cubic_16u_C4IR(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace linear interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation. 
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTLinear(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods.NPPi.ColorLUTLinear.nppiLUT_Linear_16u_C4IR(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Transpose
        /// <summary>
        /// image transpose
        /// </summary>
        /// <param name="dest">Destination image</param>
        public void Transpose(NPPImage_16uC4 dest)
        {
            status = NPPNativeMethods.NPPi.Transpose.nppiTranspose_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiTranspose_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Copy

        /// <summary>
        /// Copy image and pad borders with a constant, user-specifiable color.
        /// </summary>
        /// <param name="dst">Destination image. The image ROI defines the destination region, i.e. the region that gets filled with data from
        /// the source image (inner part) and constant border color (outer part).</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        /// <param name="nValue">The pixel value to be set for border pixels.</param>
        public void Copy(NPPImage_16uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.CopyConstBorder.nppiCopyConstBorder_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nValue);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyConstBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image copy with nearest source image pixel color.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyReplicateBorder(NPPImage_16uC4 dst, int nTopBorderHeight, int nLeftBorderWidth)
        {
            status = NPPNativeMethods.NPPi.CopyReplicateBorder.nppiCopyReplicateBorder_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyReplicateBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image copy with the borders wrapped by replication of source image pixel colors.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyWrapBorder(NPPImage_16uC4 dst, int nTopBorderHeight, int nLeftBorderWidth)
        {
            status = NPPNativeMethods.NPPi.CopyWrapBorder.nppiCopyWrapBorder_16u_C4R(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyWrapBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// linearly interpolated source image subpixel coordinate color copy.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nDx">Fractional part of source image X coordinate.</param>
        /// <param name="nDy">Fractional part of source image Y coordinate.</param>
        public void CopySubpix(NPPImage_16uC4 dst, float nDx, float nDy)
        {
            status = NPPNativeMethods.NPPi.CopySubpix.nppiCopySubpix_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nDx, nDy);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopySubpix_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinMaxEveryNew
        /// <summary>
        /// image MinEvery
        /// </summary>
        /// <param name="src2">Source-Image</param>
        public void MinEvery(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.MinMaxEvery.nppiMinEvery_16u_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinEvery_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image MaxEvery
        /// </summary>
        /// <param name="src2">Source-Image</param>
        public void MaxEvery(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.MinMaxEvery.nppiMaxEvery_16u_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxEvery_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MirrorNew


        /// <summary>
        /// Mirror image inplace.
        /// </summary>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        public void Mirror(NppiAxis flip)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiMirror_16u_C4IR(_devPtrRoi, _pitch, _sizeRoi, flip);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MorphologicalNew
        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        public void Dilate(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiDilate_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        public void Erode(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiErode_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 dilation.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        public void Dilate3x3(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiDilate3x3_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        public void Erode3x3(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiErode3x3_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void DilateBorder(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.DilationWithBorderControl.nppiDilateBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilateBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void ErodeBorder(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.ErosionWithBorderControl.nppiErodeBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErodeBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 dilation with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void Dilate3x3Border(NPPImage_16uC4 dest, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.Dilate3x3Border.nppiDilate3x3Border_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3Border_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void Erode3x3Border(NPPImage_16uC4 dest, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.Erode3x3Border.nppiErode3x3Border_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3Border_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void DilateBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.DilationWithBorderControl.nppiDilateBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilateBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void ErodeBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.ErosionWithBorderControl.nppiErodeBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErodeBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 dilation with border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void Dilate3x3BorderA(NPPImage_16uC4 dest, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.Dilate3x3Border.nppiDilate3x3Border_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3Border_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion with border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void Erode3x3BorderA(NPPImage_16uC4 dest, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.Erode3x3Border.nppiErode3x3Border_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3Border_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter
        /// <summary>
        /// 1D column convolution.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference relative to the source pixel.</param>
        public void FilterColumn(NPPImage_16uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumn32f_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn32f_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 1D row convolution.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference relative to the source pixel.</param>
        public void FilterRow(NPPImage_16uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRow32f_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow32f_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// convolution filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array.<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="oKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference</param>
        public void Filter(NPPImage_16uC4 dst, CudaDeviceVariable<float> pKernel, NppiSize oKernelSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.Convolution.nppiFilter32f_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, oKernelSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter32f_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Gauss filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        public void FilterGauss(NPPImage_16uC4 dst, MaskSize eMaskSize)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterGauss_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGauss_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// High pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        public void FilterHighPass(NPPImage_16uC4 dst, MaskSize eMaskSize)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterHighPass_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPass_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        public void FilterLowPass(NPPImage_16uC4 dst, MaskSize eMaskSize)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterLowPass_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPass_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        public void FilterSharpen(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterSharpen_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpen_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Pixels under the mask are multiplied by the respective weights in the mask and the results are summed.<para/>
        /// Before writing the result pixel the sum is scaled back via division by nDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="aKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        public void Filter(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, NppiSize aKernelSize, NppiPoint oAnchor, int nDivisor)
        {
            status = NPPNativeMethods.NPPi.Convolution.nppiFilter_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, aKernelSize, oAnchor, nDivisor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply convolution filter with user specified 1D column of weights. Result pixel is equal to the sum of
        /// the products between the kernel coefficients (pKernel array) and corresponding neighboring column pixel
        /// values in the source image defined by nKernelDim and nAnchorY, divided by nDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        public void FilterColumn(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumn_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        public void FilterRow(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRow_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel with border control. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterRowBorder(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRowBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void FilterBox(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.LinearFixedFilters2D.nppiFilterBox_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBox_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void FilterMin(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMin_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMin_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void FilterMax(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMax_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMax_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormNew
        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_Inf.
        /// </summary>
        /// <returns></returns>
        public int NormDiffInfGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiffInfGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffInfGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_Inf.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffInfGetBufferHostSize()"/></param>
        public void NormDiff_Inf(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormDiffInfGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_Inf_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_Inf. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        public void NormDiff_Inf(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff)
        {
            int bufferSize = NormDiffInfGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_Inf_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L1.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL1GetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiffL1GetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL1GetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L1.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL1GetBufferHostSize()"/></param>
        public void NormDiff_L1(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormDiffL1GetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L1_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L1. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        public void NormDiff_L1(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff)
        {
            int bufferSize = NormDiffL1GetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L1_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L2.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL2GetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiffL2GetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL2GetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L2.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL2GetBufferHostSize()"/></param>
        public void NormDiff_L2(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormDiffL2GetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L2_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L2. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        public void NormDiff_L2(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff)
        {
            int bufferSize = NormDiffL2GetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L2_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_Inf.
        /// </summary>
        /// <returns></returns>
        public int NormRelInfGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormRel.nppiNormRelInfGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelInfGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_Inf.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelInfGetBufferHostSize()"/></param>
        public void NormRel_Inf(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormRelInfGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_Inf_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_Inf. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        public void NormRel_Inf(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel)
        {
            int bufferSize = NormRelInfGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_Inf_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L1.
        /// </summary>
        /// <returns></returns>
        public int NormRelL1GetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormRel.nppiNormRelL1GetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL1GetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L1.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL1GetBufferHostSize()"/></param>
        public void NormRel_L1(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormRelL1GetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L1_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L1. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        public void NormRel_L1(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel)
        {
            int bufferSize = NormRelL1GetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L1_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L2.
        /// </summary>
        /// <returns></returns>
        public int NormRelL2GetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormRel.nppiNormRelL2GetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL2GetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L2.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL2GetBufferHostSize()"/></param>
        public void NormRel_L2(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormRelL2GetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L2_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L2. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        public void NormRel_L2(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel)
        {
            int bufferSize = NormRelL2GetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L2_16u_C4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }





        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrFull_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int FullNormLevelGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageProximity.nppiFullNormLevelGetBufferHostSize_16u32f_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFullNormLevelGetBufferHostSize_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrFull_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="FullNormLevelGetBufferHostSize()"/></param>
        public void CrossCorrFull_NormLevel(NPPImage_16uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = FullNormLevelGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        public void CrossCorrFull_NormLevel(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            int bufferSize = FullNormLevelGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_16u32f_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrSame_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int SameNormLevelGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSameNormLevelGetBufferHostSize_16u32f_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSameNormLevelGetBufferHostSize_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrSame_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SameNormLevelGetBufferHostSize()"/></param>
        public void CrossCorrSame_NormLevel(NPPImage_16uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = SameNormLevelGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        public void CrossCorrSame_NormLevel(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            int bufferSize = SameNormLevelGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_16u32f_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrValid_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int ValidNormLevelGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageProximity.nppiValidNormLevelGetBufferHostSize_16u32f_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiValidNormLevelGetBufferHostSize_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrValid_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ValidNormLevelGetBufferHostSize()"/></param>
        public void CrossCorrValid_NormLevel(NPPImage_16uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = ValidNormLevelGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        public void CrossCorrValid_NormLevel(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            int bufferSize = ValidNormLevelGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_16u32f_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }












        /// <summary>
        /// image SqrDistanceFull_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void SqrDistanceFull_Norm(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSqrDistanceFull_Norm_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceFull_Norm_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceSame_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void SqrDistanceSame_Norm(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSqrDistanceSame_Norm_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceSame_Norm_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceValid_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void SqrDistanceValid_Norm(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSqrDistanceValid_Norm_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceValid_Norm_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }







        /// <summary>
        /// image CrossCorrFull_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void CrossCorrFull_Norm(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrFull_Norm_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_Norm_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrSame_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void CrossCorrSame_Norm(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrSame_Norm_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_Norm_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrValid_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void CrossCorrValid_Norm(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrValid_Norm_16u32f_C4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_Norm_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }




        #endregion

        #region GeometryNew

        /// <summary>
        /// image resize.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nXFactor">Factor by which x dimension is changed. </param>
        /// <param name="nYFactor">Factor by which y dimension is changed. </param>
        /// <param name="nXShift">Source pixel shift in x-direction.</param>
        /// <param name="nYShift">Source pixel shift in y-direction.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        public void ResizeSqrPixel(NPPImage_16uC4 dst, double nXFactor, double nYFactor, double nXShift, double nYShift, InterpolationMode eInterpolation)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect dstRect = new NppiRect(dst.PointRoi, dst.SizeRoi);
            status = NPPNativeMethods.NPPi.ResizeSqrPixel.nppiResizeSqrPixel_16u_C4R(_devPtr, _sizeOriginal, _pitch, srcRect, dst.DevicePointer, dst.Pitch, dstRect, nXFactor, nYFactor, nXShift, nYShift, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeSqrPixel_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image remap.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pXMap">Device memory pointer to 2D image array of X coordinate values to be used when sampling source image. </param>
        /// <param name="pYMap">Device memory pointer to 2D image array of Y coordinate values to be used when sampling source image. </param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        public void Remap(NPPImage_16uC4 dst, NPPImage_32fC1 pXMap, NPPImage_32fC1 pYMap, InterpolationMode eInterpolation)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.Remap.nppiRemap_16u_C4R(_devPtr, _sizeOriginal, _pitch, srcRect, pXMap.DevicePointerRoi, pXMap.Pitch, pYMap.DevicePointerRoi, pYMap.Pitch, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRemap_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// image conversion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="hint">algorithm performance or accuracy selector, currently ignored</param>
        public void Scale(NPPImage_8uC4 dst, NppHintAlgorithm hint)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.Scale.nppiScale_16u8u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, hint);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_16u8u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        #endregion

        #region SwapChannelNew


        /// <summary>
        /// Swap channels.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aDstOrder">Integer array describing how channel values are permutated. The n-th entry of the array
        /// contains the number of the channel that is stored in the n-th channel of the output image. E.g.
        /// Given an RGBA image, aDstOrder = [3,2,1,0] converts this to ABGR channel order.</param>
        public void SwapChannels(NPPImage_16uC4 dest, int[] aDstOrder)
        {
            status = NPPNativeMethods.NPPi.SwapChannel.nppiSwapChannels_16u_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aDstOrder);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Swap channels.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aDstOrder">Host memory integer array describing how channel values are permutated. The n-th entry
        /// of the array contains the number of the channel that is stored in the n-th channel of
        /// the output image. <para/>E.g. Given an RGBA image, aDstOrder = [2,1,0] converts this to a 3 channel BGR
        /// channel order.</param>
        public void SwapChannels(NPPImage_16uC3 dest, int[] aDstOrder)
        {
            status = NPPNativeMethods.NPPi.SwapChannel.nppiSwapChannels_16u_C4C3R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aDstOrder);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_16u_C4C3R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Swap channels, in-place.
        /// </summary>
        /// <param name="aDstOrder">Integer array describing how channel values are permutated. The n-th entry of the array
        /// contains the number of the channel that is stored in the n-th channel of the output image. E.g.
        /// Given an RGBA image, aDstOrder = [3,2,1,0] converts this to ABGR channel order.</param>
        public void SwapChannels(int[] aDstOrder)
        {
            status = NPPNativeMethods.NPPi.SwapChannel.nppiSwapChannels_16u_C4IR(_devPtrRoi, _pitch, _sizeRoi, aDstOrder);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_16u_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion



        //Alpha
        #region Color...New
        /// <summary>
        /// Swap channels. Not affecting Alpha
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aDstOrder">Integer array describing how channel values are permutated. The n-th entry of the array contains the number of the channel that is stored in the n-th channel of
        /// the output image. E.g. Given an RGBA image, aDstOrder = [2,1,0] converts this to BGRA
        /// channel order. In the AC4R case, the alpha channel is always assumed to be channel 3.
        /// </param>
        public void SwapChannelsA(NPPImage_16uC4 dest, int[] aDstOrder)
        {
            status = NPPNativeMethods.NPPi.SwapChannel.nppiSwapChannels_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aDstOrder);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// An input color twist matrix with floating-point pixel values is applied
        /// within ROI. Alpha channel is the last channel and is not processed.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="twistMatrix">The color twist matrix with floating-point pixel values [3,4].</param>
        public void ColorTwistA(NPPImage_16uC4 dest, float[,] twistMatrix)
        {
            status = NPPNativeMethods.NPPi.ColorTwist.nppiColorTwist32f_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, twistMatrix);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// RGB to Gray conversion, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        public void RGBToGrayA(NPPImage_16uC1 dest)
        {
            status = NPPNativeMethods.NPPi.RGBToGray.nppiRGBToGray_16u_AC4C1R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToGray_16u_AC4C1R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Color to Gray conversion, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aCoeffs">fixed size array of constant floating point conversion coefficient values, one per color channel.</param>
        public void ColorToGrayA(NPPImage_16uC1 dest, float[] aCoeffs)
        {
            status = NPPNativeMethods.NPPi.ColorToGray.nppiColorToGray_16u_AC4C1R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aCoeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorToGray_16u_AC4C1R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Color to Gray conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aCoeffs">fixed size array of constant floating point conversion coefficient values, one per color channel.</param>
        public void ColorToGray(NPPImage_16uC1 dest, float[] aCoeffs)
        {
            status = NPPNativeMethods.NPPi.ColorToGray.nppiColorToGray_16u_C4C1R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aCoeffs);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorToGray_16u_C4C1R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// in place color twist, not affecting Alpha.
        /// 
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within ROI.
        /// </summary>
        /// <param name="aTwist">The color twist matrix with floating-point coefficient values. [3,4]</param>
        public void ColorTwistA(float[,] aTwist)
        {
            status = NPPNativeMethods.NPPi.ColorTwist.nppiColorTwist32f_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, aTwist);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32f_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MorphologicalNew
        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        public void DilateA(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiDilate_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        public void ErodeA(NPPImage_16uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiErode_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 3x3 dilation, not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        public void Dilate3x3A(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiDilate3x3_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion, not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        public void Erode3x3A(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.MorphologyFilter2D.nppiErode3x3_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter
        /// <summary>
        /// Pixels under the mask are multiplied by the respective weights in the mask and the results are summed.<para/>
        /// Before writing the result pixel the sum is scaled back via division by nDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="aKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        public void FilterA(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, NppiSize aKernelSize, NppiPoint oAnchor, int nDivisor)
        {
            status = NPPNativeMethods.NPPi.Convolution.nppiFilter_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, aKernelSize, oAnchor, nDivisor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply convolution filter with user specified 1D column of weights. Result pixel is equal to the sum of
        /// the products between the kernel coefficients (pKernel array) and corresponding neighboring column pixel
        /// values in the source image defined by nKernelDim and nAnchorY, divided by nDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        public void FilterColumnA(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumn_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        public void FilterRowA(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRow_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel with border control. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterRowBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRowBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void FilterBoxA(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.LinearFixedFilters2D.nppiFilterBox_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBox_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void FilterMinA(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMin_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMin_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void FilterMaxA(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMax_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMax_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 1D column convolution. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference relative to the source pixel.</param>
        public void FilterColumnA(NPPImage_16uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumn32f_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 1D row convolution. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference relative to the source pixel.</param>
        public void FilterRowA(NPPImage_16uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor)
        {
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRow32f_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// convolution filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array.<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="oKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference</param>
        public void FilterA(NPPImage_16uC4 dst, CudaDeviceVariable<float> pKernel, NppiSize oKernelSize, NppiPoint oAnchor)
        {
            status = NPPNativeMethods.NPPi.Convolution.nppiFilter32f_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, oKernelSize, oAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Gauss filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        public void FilterGaussA(NPPImage_16uC4 dst, MaskSize eMaskSize)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterGauss_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGauss_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// High pass filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        public void FilterHighPassA(NPPImage_16uC4 dst, MaskSize eMaskSize)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterHighPass_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPass_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        public void FilterLowPassA(NPPImage_16uC4 dst, MaskSize eMaskSize)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterLowPass_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPass_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        public void FilterSharpenA(NPPImage_16uC4 dst)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterSharpen_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpen_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region QualityIndex
        /// <summary>
        /// Device scratch buffer size (in bytes) for QualityIndex.
        /// </summary>
        /// <returns></returns>
        public int QualityIndexAGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.QualityIndex.nppiQualityIndexGetBufferHostSize_16u32f_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiQualityIndexGetBufferHostSize_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image QualityIndex. Not affecting Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dst">Pointer to the quality index. (3 * sizeof(float))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="QualityIndexAGetBufferHostSize()"/></param>
        public void QualityIndexA(NPPImage_16uC4 src2, CudaDeviceVariable<float> dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = QualityIndexAGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.QualityIndex.nppiQualityIndex_16u32f_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, dst.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiQualityIndex_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image QualityIndex. Not affecting Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dst">Pointer to the quality index. (3 * sizeof(float))</param>
        public void QualityIndexA(NPPImage_16uC4 src2, CudaDeviceVariable<float> dst)
        {
            int bufferSize = QualityIndexAGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.QualityIndex.nppiQualityIndex_16u32f_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, dst.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiQualityIndex_16u32f_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region GeometryNew

        /// <summary>
        /// image resize. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nXFactor">Factor by which x dimension is changed. </param>
        /// <param name="nYFactor">Factor by which y dimension is changed. </param>
        /// <param name="nXShift">Source pixel shift in x-direction.</param>
        /// <param name="nYShift">Source pixel shift in y-direction.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        public void ResizeSqrPixelA(NPPImage_16uC4 dst, double nXFactor, double nYFactor, double nXShift, double nYShift, InterpolationMode eInterpolation)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect dstRect = new NppiRect(dst.PointRoi, dst.SizeRoi);
            status = NPPNativeMethods.NPPi.ResizeSqrPixel.nppiResizeSqrPixel_16u_AC4R(_devPtr, _sizeOriginal, _pitch, srcRect, dst.DevicePointer, dst.Pitch, dstRect, nXFactor, nYFactor, nXShift, nYShift, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeSqrPixel_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image remap. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pXMap">Device memory pointer to 2D image array of X coordinate values to be used when sampling source image. </param>
        /// <param name="pYMap">Device memory pointer to 2D image array of Y coordinate values to be used when sampling source image. </param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        public void RemapA(NPPImage_16uC4 dst, NPPImage_32fC1 pXMap, NPPImage_32fC1 pYMap, InterpolationMode eInterpolation)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.Remap.nppiRemap_16u_AC4R(_devPtr, _sizeOriginal, _pitch, srcRect, pXMap.DevicePointerRoi, pXMap.Pitch, pYMap.DevicePointerRoi, pYMap.Pitch, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRemap_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// image conversion. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="hint">algorithm performance or accuracy selector, currently ignored</param>
        public void ScaleA(NPPImage_32sC4 dst, NppHintAlgorithm hint)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods.NPPi.Scale.nppiScale_16u8u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, hint);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_16u8u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region LUT

        /// <summary>
        /// look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTA(NPPImage_16uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods.NPPi.ColorLUT.nppiLUT_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation.  Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTCubicA(NPPImage_16uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods.NPPi.ColorLUTCubic.nppiLUT_Cubic_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// range restricted palette look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points in a palette and 
        /// source pixels are then processed using a restricted bit range when looking up palette values. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pTable">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT palette values.</param>
        /// <param name="nBitSize">Number of least significant bits (must be &gt; 0 and &lt;= 16) of each source pixel value to use as index into palette table during conversion.</param>
        public void LUTPaletteA(NPPImage_16uC4 dst, CudaDeviceVariable<ushort>[] pTable, int nBitSize)
        {
            CUdeviceptr[] ptrsT = new CUdeviceptr[] { pTable[0].DevicePointer, pTable[1].DevicePointer, pTable[2].DevicePointer };
            status = NPPNativeMethods.NPPi.ColorLUTPalette.nppiLUTPalette_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsT, nBitSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUTPalette_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation. Not affecting Alpha.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTA(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods.NPPi.ColorLUT.nppiLUT_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation.  Not affecting Alpha.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTCubicA(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods.NPPi.ColorLUTCubic.nppiLUT_Cubic_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace linear interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation.  Not affecting Alpha.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        public void LUTLinearA(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods.NPPi.ColorLUTLinear.nppiLUT_Linear_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// look-up-table color conversion.<para/>
        /// The LUT is derived from a set of user defined mapping points through linear interpolation. Not affecting alpha channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="values0">array of user defined OUTPUT values, channel 0</param>
        /// <param name="levels0">array of user defined INPUT values, channel 0</param>
        /// <param name="values1">array of user defined OUTPUT values, channel 1</param>
        /// <param name="levels1">array of user defined INPUT values, channel 1</param>
        /// <param name="values2">array of user defined OUTPUT values, channel 2</param>
        /// <param name="levels2">array of user defined INPUT values, channel 2</param>
        public void LutA(NPPImage_16uC4 dest, CudaDeviceVariable<int> values0, CudaDeviceVariable<int> levels0, CudaDeviceVariable<int> values1,
            CudaDeviceVariable<int> levels1, CudaDeviceVariable<int> values2, CudaDeviceVariable<int> levels2)
        {
            if (values0.Size != levels0.Size) throw new ArgumentException("values0 and levels0 must have same size.");
            if (values1.Size != levels1.Size) throw new ArgumentException("values1 and levels1 must have same size.");
            if (values2.Size != levels2.Size) throw new ArgumentException("values2 and levels2 must have same size.");

            CUdeviceptr[] values = new CUdeviceptr[3];
            CUdeviceptr[] levels = new CUdeviceptr[3];
            int[] levelLengths = new int[3];

            values[0] = values0.DevicePointer;
            values[1] = values1.DevicePointer;
            values[2] = values2.DevicePointer;

            levels[0] = levels0.DevicePointer;
            levels[1] = levels1.DevicePointer;
            levels[2] = levels2.DevicePointer;

            levelLengths[0] = levels0.Size;
            levelLengths[1] = levels1.Size;
            levelLengths[2] = levels2.Size;

            status = NPPNativeMethods.NPPi.ColorLUTLinear.nppiLUT_Linear_16u_AC4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, values, levels, levelLengths);

            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinMaxEveryNew
        /// <summary>
        /// image MinEvery Not affecting Alpha.
        /// </summary>
        /// <param name="src2">Source-Image</param>
        public void MinEveryA(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.MinMaxEvery.nppiMinEvery_16u_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinEvery_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image MaxEvery Not affecting Alpha.
        /// </summary>
        /// <param name="src2">Source-Image</param>
        public void MaxEveryA(NPPImage_16uC4 src2)
        {
            status = NPPNativeMethods.NPPi.MinMaxEvery.nppiMaxEvery_16u_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxEvery_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MirrorNew


        /// <summary>
        /// Mirror image inplace. Not affecting Alpha.
        /// </summary>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        public void MirrorA(NppiAxis flip)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiMirror_16u_AC4IR(_devPtrRoi, _pitch, _sizeRoi, flip);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_16u_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Copy
        /// <summary>
        /// Copy image and pad borders with a constant, user-specifiable color. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image. The image ROI defines the destination region, i.e. the region that gets filled with data from
        /// the source image (inner part) and constant border color (outer part).</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        /// <param name="nValue">The pixel value to be set for border pixels.</param>
        public void CopyConstBorderA(NPPImage_16uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, ushort[] nValue)
        {
            status = NPPNativeMethods.NPPi.CopyConstBorder.nppiCopyConstBorder_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nValue);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyConstBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image copy with nearest source image pixel color. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyReplicateBorderA(NPPImage_16uC4 dst, int nTopBorderHeight, int nLeftBorderWidth)
        {
            status = NPPNativeMethods.NPPi.CopyReplicateBorder.nppiCopyReplicateBorder_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyReplicateBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image copy with the borders wrapped by replication of source image pixel colors. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyWrapBorderA(NPPImage_16uC4 dst, int nTopBorderHeight, int nLeftBorderWidth)
        {
            status = NPPNativeMethods.NPPi.CopyWrapBorder.nppiCopyWrapBorder_16u_AC4R(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyWrapBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// linearly interpolated source image subpixel coordinate color copy. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nDx">Fractional part of source image X coordinate.</param>
        /// <param name="nDy">Fractional part of source image Y coordinate.</param>
        public void CopySubpixA(NPPImage_16uC4 dst, float nDx, float nDy)
        {
            status = NPPNativeMethods.NPPi.CopySubpix.nppiCopySubpix_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nDx, nDy);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopySubpix_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        ///// <summary>
        ///// resizes planar images.
        ///// </summary>
        ///// <param name="src0">Source image (Channel 0)</param>
        ///// <param name="src1">Source image (Channel 1)</param>
        ///// <param name="src2">Source image (Channel 2)</param>
        ///// <param name="src3">Source image (Channel 3)</param>
        ///// <param name="dest0">Destination image (Channel 0)</param>
        ///// <param name="dest1">Destination image (Channel 1)</param>
        ///// <param name="dest2">Destination image (Channel 2)</param>
        ///// <param name="dest3">Destination image (Channel 3)</param>
        ///// <param name="xFactor">X scaling factor</param>
        ///// <param name="yFactor">Y scaling factor</param>
        ///// <param name="eInterpolation">Interpolation mode</param>
        //public static void Resize(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 src3, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, NPPImage_16uC1 dest3, double xFactor, double yFactor, InterpolationMode eInterpolation)
        //{
        //	CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
        //	CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
        //	NppStatus status = NPPNativeMethods.NPPi.GeometricTransforms.nppiResize_16u_P4R(src, src0.Size, src0.Pitch, new NppiRect(src0.PointRoi, src0.SizeRoi), dst, dest0.Pitch, dest0.SizeRoi, xFactor, yFactor, eInterpolation);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_16u_P4R", status));
        //	NPPException.CheckNppStatus(status, null);
        //}



        /// <summary>
        /// planar image resize.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="src3">Source image (Channel 3)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dest3">Destination image (Channel 3)</param>
        /// <param name="nXFactor">Factor by which x dimension is changed. </param>
        /// <param name="nYFactor">Factor by which y dimension is changed. </param>
        /// <param name="nXShift">Source pixel shift in x-direction.</param>
        /// <param name="nYShift">Source pixel shift in y-direction.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        public static void ResizeSqrPixel(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 src3, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, NPPImage_16uC1 dest3, double nXFactor, double nYFactor, double nXShift, double nYShift, InterpolationMode eInterpolation)
        {
            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer, dest3.DevicePointer };
            NppiRect srcRect = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect dstRect = new NppiRect(dest0.PointRoi, dest0.SizeRoi);
            NppStatus status = NPPNativeMethods.NPPi.ResizeSqrPixel.nppiResizeSqrPixel_16u_P4R(src, src0.SizeRoi, src0.Pitch, srcRect, dst, dest0.Pitch, dstRect, nXFactor, nYFactor, nXShift, nYShift, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeSqrPixel_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// planar image remap.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="src3">Source image (Channel 3)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dest3">Destination image (Channel 3)</param>
        /// <param name="pXMap">Device memory pointer to 2D image array of X coordinate values to be used when sampling source image. </param>
        /// <param name="pYMap">Device memory pointer to 2D image array of Y coordinate values to be used when sampling source image. </param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        public static void Remap(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 src3, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, NPPImage_16uC1 dest3, NPPImage_32fC1 pXMap, NPPImage_32fC1 pYMap, InterpolationMode eInterpolation)
        {
            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppiRect srcRect = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppStatus status = NPPNativeMethods.NPPi.Remap.nppiRemap_16u_P4R(src, src0.SizeRoi, src0.Pitch, srcRect, pXMap.DevicePointerRoi, pXMap.Pitch, pYMap.DevicePointerRoi, pYMap.Pitch, dst, dest0.Pitch, dest0.SizeRoi, eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRemap_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region NormNew
        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_Inf. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormDiffInfAGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiffInfGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffInfGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_Inf. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffInfAGetBufferHostSize()"/></param>
        public void NormDiff_InfA(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormDiffInfAGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_Inf_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_Inf. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        public void NormDiff_InfA(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff)
        {
            int bufferSize = NormDiffInfAGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_Inf_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L1. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL1AGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiffL1GetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL1GetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L1. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL1AGetBufferHostSize()"/></param>
        public void NormDiff_L1A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormDiffL1AGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L1_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L1. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        public void NormDiff_L1A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff)
        {
            int bufferSize = NormDiffL1AGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L1_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L2. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL2AGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiffL2GetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL2GetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L2. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL2AGetBufferHostSize()"/></param>
        public void NormDiff_L2A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormDiffL2AGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L2_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L2. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        public void NormDiff_L2A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormDiff)
        {
            int bufferSize = NormDiffL2AGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormDiff.nppiNormDiff_L2_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_Inf. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormRelInfAGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormRel.nppiNormRelInfGetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelInfGetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_Inf. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelInfAGetBufferHostSize()"/></param>
        public void NormRel_InfA(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormRelInfAGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_Inf_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_Inf. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        public void NormRel_InfA(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel)
        {
            int bufferSize = NormRelInfAGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_Inf_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L1. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormRelL1AGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormRel.nppiNormRelL1GetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL1GetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L1. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL1AGetBufferHostSize()"/></param>
        public void NormRel_L1A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormRelL1AGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L1_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L1. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        public void NormRel_L1A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel)
        {
            int bufferSize = NormRelL1AGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L1_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L2. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormRelL2AGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.NormRel.nppiNormRelL2GetBufferHostSize_16u_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL2GetBufferHostSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L2. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL2AGetBufferHostSize()"/></param>
        public void NormRel_L2A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = NormRelL2AGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L2_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L2. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        public void NormRel_L2A(NPPImage_16uC4 tpl, CudaDeviceVariable<double> pNormRel)
        {
            int bufferSize = NormRelL2AGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.NormRel.nppiNormRel_L2_16u_AC4R(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }





        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrFull_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int FullNormLevelAGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageProximity.nppiFullNormLevelGetBufferHostSize_16u32f_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFullNormLevelGetBufferHostSize_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="FullNormLevelAGetBufferHostSize()"/></param>
        public void CrossCorrFull_NormLevelA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = FullNormLevelAGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        public void CrossCorrFull_NormLevelA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            int bufferSize = FullNormLevelAGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_16u32f_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrSame_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int SameNormLevelAGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSameNormLevelGetBufferHostSize_16u32f_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSameNormLevelGetBufferHostSize_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SameNormLevelAGetBufferHostSize()"/></param>
        public void CrossCorrSame_NormLevelA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = SameNormLevelAGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        public void CrossCorrSame_NormLevelA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            int bufferSize = SameNormLevelAGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_16u32f_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrValid_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int ValidNormLevelAGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageProximity.nppiValidNormLevelGetBufferHostSize_16u32f_AC4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiValidNormLevelGetBufferHostSize_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ValidNormLevelAGetBufferHostSize()"/></param>
        public void CrossCorrValid_NormLevelA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = ValidNormLevelAGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        public void CrossCorrValid_NormLevelA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            int bufferSize = ValidNormLevelAGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_16u32f_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }











        /// <summary>
        /// image SqrDistanceFull_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void SqrDistanceFull_NormA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSqrDistanceFull_Norm_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceFull_Norm_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceSame_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void SqrDistanceSame_NormA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSqrDistanceSame_Norm_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceSame_Norm_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceValid_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void SqrDistanceValid_NormA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiSqrDistanceValid_Norm_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceValid_Norm_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }





        /// <summary>
        /// image CrossCorrFull_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void CrossCorrFull_NormA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrFull_Norm_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_Norm_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrSame_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void CrossCorrSame_NormA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrSame_Norm_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_Norm_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrValid_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        public void CrossCorrValid_NormA(NPPImage_16uC4 tpl, NPPImage_32fC4 dst)
        {
            status = NPPNativeMethods.NPPi.ImageProximity.nppiCrossCorrValid_Norm_16u32f_AC4R(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_Norm_16u32f_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }




        #endregion

        //New in Cuda 6.0

        #region SumWindow
        /// <summary>
        /// 16-bit unsigned 1D (column) sum to 32f.
        /// Apply Column Window Summation filter over a 1D mask region around each
        /// source pixel for 4-channel 16 bit/pixel input images with 32-bit floating point
        /// output.  <para/>
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring column pixel values in a mask region of the source image defined by
        /// nMaskSize and nAnchor. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void SumWindowColumn(NPPImage_32fC4 dest, int nMaskSize, int nAnchor)
        {
            status = NPPNativeMethods.NPPi.WindowSum1D.nppiSumWindowColumn_16u32f_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nMaskSize, nAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowColumn_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 16-bit unsigned 1D (row) sum to 32f.<para/>
        /// Apply Row Window Summation filter over a 1D mask region around each source
        /// pixel for 4-channel 16-bit pixel input images with 32-bit floating point output.  
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring row pixel values in a mask region of the source image defined
        /// by nKernelDim and nAnchorX. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        public void SumWindowRow(NPPImage_32fC4 dest, int nMaskSize, int nAnchor)
        {
            status = NPPNativeMethods.NPPi.WindowSum1D.nppiSumWindowRow_16u32f_C4R(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nMaskSize, nAnchor);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowRow_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter Median
        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        public void FilterMedian(NPPImage_16uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            int bufferSize = FilterMedianGetBufferHostSize(oMaskSize);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.ImageMedianFilter.nppiFilterMedian_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the Median operation.</param>
        public void FilterMedian(NPPImage_16uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = FilterMedianGetBufferHostSize(oMaskSize);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageMedianFilter.nppiFilterMedian_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for FilterMedian.
        /// </summary>
        /// <returns></returns>
        public int FilterMedianGetBufferHostSize(NppiSize oMaskSize)
        {
            uint bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageMedianFilter.nppiFilterMedianGetBufferSize_16u_C4R(_sizeRoi, oMaskSize, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedianGetBufferSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return (int)bufferSize; //We stay consistent with other GetBufferHostSize functions and convert to int.
        }
        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region, ignoring alpha channel.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        public void FilterMedianA(NPPImage_16uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor)
        {
            int bufferSize = FilterMedianGetBufferHostSizeA(oMaskSize);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.ImageMedianFilter.nppiFilterMedian_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_16u_AC4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region, ignoring alpha channel.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the Median operation.</param>
        public void FilterMedianA(NPPImage_16uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = FilterMedianGetBufferHostSizeA(oMaskSize);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.ImageMedianFilter.nppiFilterMedian_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for FilterMedian, ignoring alpha channel.
        /// </summary>
        /// <returns></returns>
        public int FilterMedianGetBufferHostSizeA(NppiSize oMaskSize)
        {
            uint bufferSize = 0;
            status = NPPNativeMethods.NPPi.ImageMedianFilter.nppiFilterMedianGetBufferSize_16u_AC4R(_sizeRoi, oMaskSize, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedianGetBufferSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return (int)bufferSize; //We stay consistent with other GetBufferHostSize functions and convert to int.
        }
        #endregion

        #region MaxError
        /// <summary>
        /// image maximum error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        public void MaxError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = MaxErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.MaximumError.nppiMaximumError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumError_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image maximum error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the MaxError operation.</param>
        public void MaxError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaxErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MaximumError.nppiMaximumError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumError_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for MaxError.
        /// </summary>
        /// <returns></returns>
        public int MaxErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MaximumError.nppiMaximumErrorGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumErrorGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region AverageError
        /// <summary>
        /// image average error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        public void AverageError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = AverageErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.AverageError.nppiAverageError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageError_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image average error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the AverageError operation.</param>
        public void AverageError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = AverageErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.AverageError.nppiAverageError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageError_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for AverageError.
        /// </summary>
        /// <returns></returns>
        public int AverageErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.AverageError.nppiAverageErrorGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageErrorGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region MaximumRelative_Error
        /// <summary>
        /// image maximum relative error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        public void MaximumRelativeError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = MaximumRelativeErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.MaximumRelativeError.nppiMaximumRelativeError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeError_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image maximum relative error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the MaximumRelativeError operation.</param>
        public void MaximumRelativeError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaximumRelativeErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MaximumRelativeError.nppiMaximumRelativeError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeError_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for MaximumRelativeError.
        /// </summary>
        /// <returns></returns>
        public int MaximumRelativeErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MaximumRelativeError.nppiMaximumRelativeErrorGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeErrorGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region AverageRelative_Error
        /// <summary>
        /// image average relative error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        public void AverageRelativeError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = AverageRelativeErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.AverageRelativeError.nppiAverageRelativeError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeError_16u_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image average relative error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the AverageRelativeError operation.</param>
        public void AverageRelativeError(NPPImage_16uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = AverageRelativeErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.AverageRelativeError.nppiAverageRelativeError_16u_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeError_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for AverageRelativeError.
        /// </summary>
        /// <returns></returns>
        public int AverageRelativeErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.AverageRelativeError.nppiAverageRelativeErrorGetBufferHostSize_16u_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeErrorGetBufferHostSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region FilterBorder
        /// <summary>
        /// Four channel 16-bit unsigned convolution filter with border control.<para/>
        /// General purpose 2D convolution filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided.
        /// If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterBorder(NPPImage_16uC4 dest, CudaDeviceVariable<int> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, int nDivisor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterBorder.nppiFilterBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, nDivisor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Four channel 16-bit unsigned convolution filter with border control.<para/>
        /// General purpose 2D convolution filter using floating-point weights with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image. <para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterBorder(NPPImage_16uC4 dest, CudaDeviceVariable<float> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterBorder32f.nppiFilterBorder32f_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder32f_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Four channel 16-bit unsigned convolution filter with border control, ignoring alpha channel.<para/>
        /// General purpose 2D convolution filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided.
        /// If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<int> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, int nDivisor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterBorder.nppiFilterBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, nDivisor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Four channel 16-bit unsigned convolution filter with border control, ignoring alpha channel.<para/>
        /// General purpose 2D convolution filter using floating-point weights with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image. <para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<float> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterBorder32f.nppiFilterBorder32f_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterGauss
        /// <summary>Filters the image using a Gaussian filter kernel with border control:<para/>
        /// 1/16 2/16 1/16<para/>
        /// 2/16 4/16 2/16<para/>
        /// 1/16 2/16 1/16<para/>
        /// <para/> or <para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 12/571 52/571 127/571 52/571 12/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterGaussBorder(NPPImage_16uC4 dest, MaskSize eMaskSize, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterGaussBorder.nppiFilterGaussBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eMaskSize, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>Filters the image using a Gaussian filter kernel with border control, ignoring alpha channel:<para/>
        /// 1/16 2/16 1/16<para/>
        /// 2/16 4/16 2/16<para/>
        /// 1/16 2/16 1/16<para/>
        /// <para/> or <para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 12/571 52/571 127/571 52/571 12/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterGaussBorderA(NPPImage_16uC4 dest, MaskSize eMaskSize, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterGaussBorder.nppiFilterGaussBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eMaskSize, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        //New in Cuda 7.0

        #region FilterColumnBorder
        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterColumnBorder(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumnBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, nDivisor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterColumnBorder(NPPImage_16uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumnBorder32f_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder32f_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterColumnBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<int> Kernel, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumnBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, nDivisor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterColumnBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterColumnBorder32f_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterRow
        /// <summary>
        /// General purpose 1D convolution row filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterRowBorder(NPPImage_16uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRowBorder32f_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder32f_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// General purpose 1D convolution row filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterRowBorderA(NPPImage_16uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFilter1D.nppiFilterRowBorder32f_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder32f_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        #endregion

        #region SumWindow

        /// <summary>
        /// Apply Column Window Summation filter over a 1D mask region around each
        /// source pixel for 3-channel 8 bit/pixel input images with 32-bit floating point
        /// output.  
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring column pixel values in a mask region of the source image defined by
        /// nMaskSize and nAnchor. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void SumWindowColumnBorder(NPPImage_32fC4 dest, int nMaskSize, int nAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.WindowSum1D.nppiSumWindowColumnBorder_16u32f_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, nMaskSize, nAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowColumnBorder_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Apply Row Window Summation filter over a 1D mask region around each source
        /// pixel for 3-channel 8-bit pixel input images with 32-bit floating point output.  
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring row pixel values in a mask region of the source image defined
        /// by nKernelDim and nAnchorX. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void SumWindowRowBorder(NPPImage_32fC4 dest, int nMaskSize, int nAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.WindowSum1D.nppiSumWindowRowBorder_16u32f_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, nMaskSize, nAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowRowBorder_16u32f_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterBox


        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterBoxBorder(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFixedFilters2D.nppiFilterBoxBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBoxBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterBoxBorderA(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.LinearFixedFilters2D.nppiFilterBoxBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBoxBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Filter Min/Max


        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterMinBorder(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMinBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMinBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterMaxBorder(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMaxBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMaxBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterMinBorderA(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMinBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMinBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterMaxBorderA(NPPImage_16uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.RankFilters.nppiFilterMaxBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMaxBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterOthers




        /// <summary>
        /// High pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterHighPassBorder(NPPImage_16uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterHighPassBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPassBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// High pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterHighPassBorderA(NPPImage_16uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterHighPassBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPassBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterLowPassBorder(NPPImage_16uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterLowPassBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPassBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterLowPassBorderA(NPPImage_16uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterLowPassBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPassBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterSharpenBorder(NPPImage_16uC4 dst, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterSharpenBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpenBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterSharpenBorderA(NPPImage_16uC4 dst, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterSharpenBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpenBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter Unsharp

        /// <summary>
        /// Scratch-buffer size for unsharp filter.
        /// </summary>
        /// <param name="nRadius">The radius of the Gaussian filter, in pixles, not counting the center pixel.</param>
        /// <param name="nSigma">The standard deviation of the Gaussian filter, in pixel.</param>
        /// <returns></returns>
        public int FilterUnsharpGetBufferSize(float nRadius, float nSigma)
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterUnsharpGetBufferSize_16u_C4R(nRadius, nSigma, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterUnsharpGetBufferSize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Scratch-buffer size for unsharp filter.
        /// </summary>
        /// <param name="nRadius">The radius of the Gaussian filter, in pixles, not counting the center pixel.</param>
        /// <param name="nSigma">The standard deviation of the Gaussian filter, in pixel.</param>
        /// <returns></returns>
        public int FilterUnsharpGetBufferSizeA(float nRadius, float nSigma)
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterUnsharpGetBufferSize_16u_AC4R(nRadius, nSigma, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterUnsharpGetBufferSize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Filters the image using a unsharp-mask sharpening filter kernel with border control.<para/>
        /// The algorithm involves the following steps:<para/>
        /// Smooth the original image with a Gaussian filter, with the width controlled by the nRadius.<para/>
        /// Subtract the smoothed image from the original to create a high-pass filtered image.<para/>
        /// Apply any clipping needed on the high-pass image, as controlled by the nThreshold.<para/>
        /// Add a certain percentage of the high-pass filtered image to the original image, 
        /// with the percentage controlled by the nWeight.
        /// In pseudocode this algorithm can be written as:<para/>
        /// HighPass = Image - Gaussian(Image)<para/>
        /// Result = Image + nWeight * HighPass * ( |HighPass| >= nThreshold ) <para/>
        /// where nWeight is the amount, nThreshold is the threshold, and >= indicates a Boolean operation, 1 if true, or 0 otherwise.
        /// <para/>
        /// If any portion of the mask overlaps the source image boundary, the requested border type 
        /// operation is applied to all mask pixels which fall outside of the source image.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nRadius">The radius of the Gaussian filter, in pixles, not counting the center pixel.</param>
        /// <param name="nSigma">The standard deviation of the Gaussian filter, in pixel.</param>
        /// <param name="nWeight">The percentage of the difference between the original and the high pass image that is added back into the original.</param>
        /// <param name="nThreshold">The threshold needed to apply the difference amount.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="buffer">Pointer to the user-allocated device scratch buffer required for the unsharp operation.</param>
        public void FilterUnsharpBorder(NPPImage_16uC4 dst, float nRadius, float nSigma, float nWeight, float nThreshold, NppiBorderType eBorderType, CudaDeviceVariable<byte> buffer)
        {
            if (buffer.Size < FilterUnsharpGetBufferSize(nRadius, nSigma))
                throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterUnsharpBorder_16u_C4R(_devPtrRoi, _pitch, new NppiPoint(), dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nRadius, nSigma, nWeight, nThreshold, eBorderType, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterUnsharpBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a unsharp-mask sharpening filter kernel with border control.<para/>
        /// The algorithm involves the following steps:<para/>
        /// Smooth the original image with a Gaussian filter, with the width controlled by the nRadius.<para/>
        /// Subtract the smoothed image from the original to create a high-pass filtered image.<para/>
        /// Apply any clipping needed on the high-pass image, as controlled by the nThreshold.<para/>
        /// Add a certain percentage of the high-pass filtered image to the original image, 
        /// with the percentage controlled by the nWeight.
        /// In pseudocode this algorithm can be written as:<para/>
        /// HighPass = Image - Gaussian(Image)<para/>
        /// Result = Image + nWeight * HighPass * ( |HighPass| >= nThreshold ) <para/>
        /// where nWeight is the amount, nThreshold is the threshold, and >= indicates a Boolean operation, 1 if true, or 0 otherwise.
        /// <para/>
        /// If any portion of the mask overlaps the source image boundary, the requested border type 
        /// operation is applied to all mask pixels which fall outside of the source image.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nRadius">The radius of the Gaussian filter, in pixles, not counting the center pixel.</param>
        /// <param name="nSigma">The standard deviation of the Gaussian filter, in pixel.</param>
        /// <param name="nWeight">The percentage of the difference between the original and the high pass image that is added back into the original.</param>
        /// <param name="nThreshold">The threshold needed to apply the difference amount.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="buffer">Pointer to the user-allocated device scratch buffer required for the unsharp operation.</param>
        public void FilterUnsharpBorderA(NPPImage_16uC4 dst, float nRadius, float nSigma, float nWeight, float nThreshold, NppiBorderType eBorderType, CudaDeviceVariable<byte> buffer)
        {
            if (buffer.Size < FilterUnsharpGetBufferSizeA(nRadius, nSigma))
                throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterUnsharpBorder_16u_AC4R(_devPtrRoi, _pitch, new NppiPoint(), dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nRadius, nSigma, nWeight, nThreshold, eBorderType, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterUnsharpBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Filter Gauss Advanced

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        public void FilterGauss(NPPImage_16uC4 dst, CudaDeviceVariable<float> Kernel)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterGaussAdvanced_16u_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, Kernel.Size, Kernel.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvanced_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterGaussBorder(NPPImage_16uC4 dst, CudaDeviceVariable<float> Kernel, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterGaussBorder.nppiFilterGaussAdvancedBorder_16u_C4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, Kernel.Size, Kernel.DevicePointer, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvancedBorder_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        public void FilterGaussA(NPPImage_16uC4 dst, CudaDeviceVariable<float> Kernel)
        {
            status = NPPNativeMethods.NPPi.FixedFilters.nppiFilterGaussAdvanced_16u_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, Kernel.Size, Kernel.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvanced_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        public void FilterGaussBorderA(NPPImage_16uC4 dst, CudaDeviceVariable<float> Kernel, NppiBorderType eBorderType, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods.NPPi.FilterGaussBorder.nppiFilterGaussAdvancedBorder_16u_AC4R(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, Kernel.Size, Kernel.DevicePointer, eBorderType);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvancedBorder_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region New in Cuda 9.0


        /// <summary>
        /// Resizes images.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        public void Resize(NPPImage_16uC4 dest, InterpolationMode eInterpolation)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiResize_16u_C4R(_devPtr, _pitch, _sizeOriginal, new NppiRect(_pointRoi, _sizeRoi), dest.DevicePointer, dest.Pitch, dest.Size, new NppiRect(dest.PointRoi, dest.SizeRoi), eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_16u_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Resizes images. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        public void ResizeA(NPPImage_16uC4 dest, InterpolationMode eInterpolation)
        {
            status = NPPNativeMethods.NPPi.GeometricTransforms.nppiResize_16u_AC4R(_devPtr, _pitch, _sizeOriginal, new NppiRect(_pointRoi, _sizeRoi), dest.DevicePointer, dest.Pitch, dest.Size, new NppiRect(dest.PointRoi, dest.SizeRoi), eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_16u_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// resizes planar images.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="src3">Source image (Channel 3)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dest3">Destination image (Channel 3)</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        public static void Resize(NPPImage_16uC1 src0, NPPImage_16uC1 src1, NPPImage_16uC1 src2, NPPImage_16uC1 src3, NPPImage_16uC1 dest0, NPPImage_16uC1 dest1, NPPImage_16uC1 dest2, NPPImage_16uC1 dest3, InterpolationMode eInterpolation)
        {
            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer, dest3.DevicePointer };
            NppStatus status = NPPNativeMethods.NPPi.GeometricTransforms.nppiResize_16u_P4R(src, src0.Pitch, src0.Size, new NppiRect(src0.PointRoi, src0.SizeRoi), dst, dest0.Pitch, dest0.Size, new NppiRect(dest0.PointRoi, dest0.SizeRoi), eInterpolation);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_16u_P4R", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion
    }
}
