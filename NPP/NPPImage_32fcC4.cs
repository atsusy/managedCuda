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
    public partial class NPPImage_32fcC4 : NPPImageBase
    {
        #region Constructors
        /// <summary>
        /// Allocates new memory on device using NPP-Api.
        /// </summary>
        /// <param name="nWidthPixels">Image width in pixels</param>
        /// <param name="nHeightPixels">Image height in pixels</param>
        public NPPImage_32fcC4(int nWidthPixels, int nHeightPixels)
        {
            _sizeOriginal.width = nWidthPixels;
            _sizeOriginal.height = nHeightPixels;
            _sizeRoi.width = nWidthPixels;
            _sizeRoi.height = nHeightPixels;
            _channels = 4;
            _isOwner = true;
            _typeSize = Marshal.SizeOf(typeof(Npp32fc));

            _devPtr = NPPNativeMethods.NPPi.MemAlloc.nppiMalloc_32fc_C4(nWidthPixels, nHeightPixels, ref _pitch);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}, Pitch is: {3}, Number of color channels: {4}", DateTime.Now, "nppiMalloc_32fc_C4", res, _pitch, _channels));

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
        public NPPImage_32fcC4(CUdeviceptr devPtr, int width, int height, int pitch, bool isOwner)
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
            _typeSize = Marshal.SizeOf(typeof(Npp32fc));
        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr. Does not take ownership of decPtr.
        /// </summary>
        /// <param name="devPtr">Already allocated device ptr.</param>
        /// <param name="width">Image width in pixels</param>
        /// <param name="height">Image height in pixels</param>
        /// <param name="pitch">Pitch / Line step</param>
        public NPPImage_32fcC4(CUdeviceptr devPtr, int width, int height, int pitch)
            : this(devPtr, width, height, pitch, false)
        {

        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr. Does not take ownership of inner image device pointer.
        /// </summary>
        /// <param name="image">NPP image</param>
        public NPPImage_32fcC4(NPPImageBase image)
            : this(image.DevicePointer, image.Width, image.Height, image.Pitch, false)
        {

        }

        /// <summary>
        /// Allocates new memory on device using NPP-Api.
        /// </summary>
        /// <param name="size">Image size</param>
        public NPPImage_32fcC4(NppiSize size)
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
        public NPPImage_32fcC4(CUdeviceptr devPtr, NppiSize size, int pitch, bool isOwner)
            : this(devPtr, size.width, size.height, pitch, isOwner)
        {

        }

        /// <summary>
        /// Creates a new NPPImage from allocated device ptr.
        /// </summary>
        /// <param name="devPtr">Already allocated device ptr.</param>
        /// <param name="size">Image size</param>
        /// <param name="pitch">Pitch / Line step</param>
        public NPPImage_32fcC4(CUdeviceptr devPtr, NppiSize size, int pitch)
            : this(devPtr, size.width, size.height, pitch)
        {

        }

        /// <summary>
        /// For dispose
        /// </summary>
        ~NPPImage_32fcC4()
        {
            Dispose(false);
        }
        #endregion

        #region Copy
        /// <summary>
        /// image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void Copy(NPPImage_32fcC4 dst)
        {
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_32fc_C4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image copy. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        public void CopyA(NPPImage_32fcC4 dst)
        {
            status = NPPNativeMethods.NPPi.MemCopy.nppiCopy_32fc_AC4R(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Set
        /// <summary>
        /// Set pixel values to nValue.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 4)</param>
        public void Set(Npp32fc[] nValue)
        {
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_32fc_C4R(nValue, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value. Not affecting alpha channel.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 3)</param>
        public void SetA(Npp32fc[] nValue)
        {
            status = NPPNativeMethods.NPPi.MemSet.nppiSet_32fc_AC4R(nValue, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Add
        /// <summary>
        /// Image addition.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Add(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_32fc_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image addition.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Add(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_32fc_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Add constant to image.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="dest">Destination image</param>
        public void Add(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_32fc_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Add constant to image. Inplace.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        public void Add(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_32fc_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image addition. Unmodified Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void AddA(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_32fc_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image addition. Unmodified Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void AddA(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Add.nppiAdd_32fc_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Add constant to image. Unmodified Alpha.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="dest">Destination image</param>
        public void AddA(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_32fc_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Add constant to image. Inplace. Unmodified Alpha.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        public void AddA(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.AddConst.nppiAddC_32fc_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sub
        /// <summary>
        /// Image subtraction.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Sub(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_32fc_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image subtraction.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Sub(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_32fc_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Subtract constant to image.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="dest">Destination image</param>
        public void Sub(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_32fc_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Subtract constant to image. Inplace.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        public void Sub(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_32fc_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image subtraction. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void SubA(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_32fc_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image subtraction. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void SubA(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Sub.nppiSub_32fc_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Subtract constant to image. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="dest">Destination image</param>
        public void SubA(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_32fc_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Subtract constant to image. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        public void SubA(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.SubConst.nppiSubC_32fc_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Mul
        /// <summary>
        /// Image multiplication.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Mul(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_32fc_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Mul(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_32fc_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        public void Mul(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_32fc_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image. Inplace.
        /// </summary>
        /// <param name="nConstant">Value</param>
        public void Mul(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_32fc_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void MulA(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_32fc_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void MulA(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Mul.nppiMul_32fc_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        public void MulA(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_32fc_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        public void MulA(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.MulConst.nppiMulC_32fc_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Div
        /// <summary>
        /// Image division.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void Div(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_32fc_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void Div(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_32fc_C4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Divide constant to image.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        public void Div(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_32fc_C4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_32fc_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Divide constant to image. Inplace.
        /// </summary>
        /// <param name="nConstant">Value</param>
        public void Div(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_32fc_C4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_32fc_C4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        public void DivA(NPPImage_32fcC4 src2, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_32fc_AC4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        public void DivA(NPPImage_32fcC4 src2)
        {
            status = NPPNativeMethods.NPPi.Div.nppiDiv_32fc_AC4IR(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Divide constant to image. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        public void DivA(Npp32fc[] nConstant, NPPImage_32fcC4 dest)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_32fc_AC4R(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_32fc_AC4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Divide constant to image. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        public void DivA(Npp32fc[] nConstant)
        {
            status = NPPNativeMethods.NPPi.DivConst.nppiDivC_32fc_AC4IR(nConstant, _devPtrRoi, _pitch, _sizeRoi);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_32fc_AC4IR", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MaxError
        /// <summary>
        /// image maximum error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        public void MaxError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = MaxErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.MaximumError.nppiMaximumError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumError_8s_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image maximum error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the MaxError operation.</param>
        public void MaxError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaxErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MaximumError.nppiMaximumError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumError_8s_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for MaxError.
        /// </summary>
        /// <returns></returns>
        public int MaxErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MaximumError.nppiMaximumErrorGetBufferHostSize_8s_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumErrorGetBufferHostSize_8s_C4R", status));
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
        public void AverageError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = AverageErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.AverageError.nppiAverageError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageError_8s_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image average error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the AverageError operation.</param>
        public void AverageError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = AverageErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.AverageError.nppiAverageError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageError_8s_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for AverageError.
        /// </summary>
        /// <returns></returns>
        public int AverageErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.AverageError.nppiAverageErrorGetBufferHostSize_8s_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageErrorGetBufferHostSize_8s_C4R", status));
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
        public void MaximumRelativeError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = MaximumRelativeErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.MaximumRelativeError.nppiMaximumRelativeError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeError_8s_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image maximum relative error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the MaximumRelativeError operation.</param>
        public void MaximumRelativeError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = MaximumRelativeErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.MaximumRelativeError.nppiMaximumRelativeError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeError_8s_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for MaximumRelativeError.
        /// </summary>
        /// <returns></returns>
        public int MaximumRelativeErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.MaximumRelativeError.nppiMaximumRelativeErrorGetBufferHostSize_8s_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeErrorGetBufferHostSize_8s_C4R", status));
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
        public void AverageRelativeError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError)
        {
            int bufferSize = AverageRelativeErrorGetBufferHostSize();
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods.NPPi.AverageRelativeError.nppiAverageRelativeError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeError_8s_C4R", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image average relative error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the AverageRelativeError operation.</param>
        public void AverageRelativeError(NPPImage_8sC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer)
        {
            int bufferSize = AverageRelativeErrorGetBufferHostSize();
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods.NPPi.AverageRelativeError.nppiAverageRelativeError_8s_C4R(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeError_8s_C4R", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for AverageRelativeError.
        /// </summary>
        /// <returns></returns>
        public int AverageRelativeErrorGetBufferHostSize()
        {
            int bufferSize = 0;
            status = NPPNativeMethods.NPPi.AverageRelativeError.nppiAverageRelativeErrorGetBufferHostSize_8s_C4R(_sizeRoi, ref bufferSize);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeErrorGetBufferHostSize_8s_C4R", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion
    }
}
